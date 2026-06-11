using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("飛行特效")]
    public GameObject flyEffectPrefab;

    [Header("命中特效")]
    public GameObject hitEffectPrefab;

    private GameObject flyEffect;

    void Start()
    {
        if (flyEffectPrefab != null)
        {
            flyEffect = Instantiate(
                flyEffectPrefab,
                transform);

            flyEffect.transform.localPosition =
                Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 hitPoint = transform.position;

        if (collision.contactCount > 0)
        {
            hitPoint = collision.contacts[0].point;
        }

        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(
                hitEffectPrefab,
                hitPoint,
                Quaternion.identity);

            Destroy(effect, 3f);
        }

        if (collision.gameObject.CompareTag("Target"))
        {
            Destroy(collision.gameObject);
            print("我沒了");
        }

        Destroy(gameObject);
    }
}