using UnityEngine;

public class PlayerLauncher : MonoBehaviour
{
    public float speed = 3.0f;
    //public int attack;
    
    void Start()
    {
            
    }

    
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);    
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroy(collision.gameObject);
        
        Destroy(gameObject);
    }



}
