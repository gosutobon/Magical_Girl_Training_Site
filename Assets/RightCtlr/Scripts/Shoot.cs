using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Rigidbody bulletPrefab;
    public Transform firePoint;
    public RaycastAim aimSystem;

    public float bulletSpeed = 50f;

    public GameObject muzzleFlashPrefab;

    public void Fire()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("bulletPrefab 是空的");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("firePoint 是空的");
            return;
        }

        Vector3 targetPoint =
            aimSystem != null
                ? aimSystem.GetAimPoint()
                : firePoint.position + firePoint.forward * 100f;

        Vector3 direction =
            (targetPoint - firePoint.position).normalized;

        Rigidbody bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(direction));

        bullet.linearVelocity =
            direction * bulletSpeed;

        Destroy(bullet.gameObject, 5f);

        if (muzzleFlashPrefab != null)
        {
            GameObject flash =
                Instantiate(
                    muzzleFlashPrefab,
                    firePoint.position,
                    firePoint.rotation,
                    firePoint);

            Destroy(flash, 2f);
        }
    }
}