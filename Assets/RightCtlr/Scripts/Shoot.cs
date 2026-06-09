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
        // 1. 安全檢查
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Shoot 腳本尚未指派 bulletPrefab 或 firePoint！");
            return;
        }

        // 2. 計算目標點與方向
        Vector3 targetPoint = (aimSystem != null) 
            ? aimSystem.GetAimPoint() 
            : firePoint.position + firePoint.forward * 100f;

        Vector3 direction = (targetPoint - firePoint.position).normalized;

        // 防呆：如果距離太近導致方向為零，則預設朝槍口正前方
        if (direction == Vector3.zero) direction = firePoint.forward;

        // 3. 生成子彈並賦予速度
        Rigidbody bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        bullet.linearVelocity = direction * bulletSpeed;

        // 🔥 優化：將子彈自身的超時銷毀移交給子彈自己處理，
        // 或者保留在這裡，但子彈碰撞時就不用重複寫。這裡保留作為保險。
        Destroy(bullet.gameObject, 5f);

        // 4. 生成槍口特效 (修正 VR 甩手脫節問題)
        if (muzzleFlashPrefab != null)
        {
            // 將 firePoint 作為 Parent 傳入，讓特效跟隨槍口移動
            GameObject flash = Instantiate(
                muzzleFlashPrefab,
                firePoint.position,
                firePoint.rotation,
                firePoint 
            );

            Destroy(flash, 2f);
        }
    }
}