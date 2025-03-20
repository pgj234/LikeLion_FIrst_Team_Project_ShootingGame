using UnityEngine;

public class BossBullet2 : MonoBehaviour
{
    public float Speed = 3.5f;
    Vector2 vec2 = Vector2.down;
    void Start()
    {

    }

    void Update()
    {
        transform.Translate(vec2 * Speed * Time.deltaTime);
    }

    public void Move(Vector2 vec)
    {
        vec2 = vec;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //미사일지움
            Destroy(gameObject);
        }
    }
}
