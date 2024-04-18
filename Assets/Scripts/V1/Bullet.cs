using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float disableDelay = 1f;
    private const float BOUNDS_OFFSET = 2f;

    private void Start()
    {
        StartCoroutine(DisableBulletAfterDelay(disableDelay));
    }

    private void FixedUpdate()
    {
        MoveBullet();
        CheckBounds();
    }

    private void MoveBullet()
    {
        transform.Translate(Vector3.up * bulletSpeed);
    }

    private void CheckBounds()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);

        if (viewportPosition.y > 1f + BOUNDS_OFFSET || viewportPosition.y < -1f - BOUNDS_OFFSET)
        {
            DeactivateBullet();
        }
    }

    private IEnumerator DisableBulletAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DeactivateBullet();
    }

    private void DeactivateBullet()
    {
        gameObject.SetActive(false);
    }
}