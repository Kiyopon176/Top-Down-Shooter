using UnityEngine;

public class Shooting: MonoBehaviour
{
    [SerializeField] private float shootDelay = 0.5f;
    
    private bool canShoot = true;

    public void Shoot()
    {
        if (!canShoot) return;

        GameObject bullet = ObjectPooling.SharedInstance.GetPooledObject();
        if (bullet != null)
        {
            FireBullet(bullet);
        }
    }

    private void FireBullet(GameObject bullet)
    {
        EventManager.OnSoundNeed?.Invoke("Shoot");
        bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
        bullet.SetActive(true);
    }
    
}