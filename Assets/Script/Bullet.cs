using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Bullet bullet;
    public Boss boss;


    public float speed = 4.0f;
    public int attack=2;

    void Start()
    {

    }


    void Update()
    {
        transform.Translate(Vector2.up*speed*Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<Boss>().Damage(attack);

            Destroy(gameObject);
        }
        
        if(collision.CompareTag("Monster"))
        {
            Destroy(gameObject);
        }

    }
}