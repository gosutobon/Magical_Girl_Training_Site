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

            flyEffect.transform.localPosition = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitEffectPrefab != null)
        {
            GameObject effect =
                Instantiate(
                    hitEffectPrefab,
                    collision.contacts[0].point,
                    Quaternion.identity);

            Destroy(effect, 3f);
        }

        if (collision.gameObject.CompareTag("Target"))
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject);
    }
}