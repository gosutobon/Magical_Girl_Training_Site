using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("子彈Prefab")]
    public Rigidbody bulletPrefab;

    [Header("槍口位置")]
    public Transform firePoint;

    [Header("子彈速度")]
    public float bulletSpeed = 50f;

    [Header("槍口特效")]
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

        // 直接使用槍口方向
        Vector3 direction = firePoint.forward;

        Rigidbody bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation);

        bullet.linearVelocity =
            direction * bulletSpeed;

        Destroy(
            bullet.gameObject,
            5f);

        // 槍口特效
        if (muzzleFlashPrefab != null)
        {
            GameObject flash =
                Instantiate(
                    muzzleFlashPrefab,
                    firePoint.position,
                    firePoint.rotation);

            Destroy(flash, 2f);
        }
    }
}