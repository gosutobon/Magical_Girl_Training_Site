using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("子彈Prefab")]
    public Rigidbody bulletPrefab;

    [Header("槍口位置")]
    public Transform firePoint;

    [Header("瞄準系統")]
    public RaycastAim aimSystem;

    [Header("子彈速度")]
    public float bulletSpeed = 50f;

    [Header("槍口特效")]
    public GameObject muzzleFlashPrefab;

    public void Fire()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("bulletPrefab 是空的");
        }

        if (firePoint == null)
        {
            Debug.LogError("firePoint 是空的");
        }

        if (aimSystem == null)
        {
            Debug.LogError("aimSystem 是空的");
        }

        Vector3 targetPoint =
            aimSystem != null
                ? aimSystem.GetAimPoint()
                : firePoint.position + firePoint.forward * 100f;

        Vector3 direction =
            (targetPoint - firePoint.position).normalized;

        if (direction == Vector3.zero)
            direction = firePoint.forward;

        Rigidbody bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(direction));

        bullet.linearVelocity =
            direction * bulletSpeed;

        Destroy(bullet.gameObject, 5f);

        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(
                muzzleFlashPrefab,
                firePoint.position,
                firePoint.rotation,
                firePoint);

            Destroy(flash, 2f);
        }
    }
}