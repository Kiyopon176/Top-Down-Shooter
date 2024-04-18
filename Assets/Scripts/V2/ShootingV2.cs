using System.Collections;
using UnityEngine;

public class ShootingV2 : MonoBehaviour
{
    [SerializeField] private float shootDelay = 0.5f;
    
    private bool canShoot = true;
    
    private void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    public void Shoot()
    {
        if (!canShoot) return;

        GameObject bullet = ObjectPooling.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            FireBullet(bullet);
        }

        ResetShootFlag();
    }

    private void FireBullet(GameObject bullet)
    {
        EventManager.OnSoundNeed?.Invoke("Shoot");
        bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
        bullet.SetActive(true);
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => canShoot);
            
            Shoot();
            
            yield return new WaitForSeconds(shootDelay);
        }
    }

    private void ResetShootFlag()
    {
        canShoot = false;
        StartCoroutine(ResetShootDelay());
    }

    private IEnumerator ResetShootDelay()
    {
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
}
