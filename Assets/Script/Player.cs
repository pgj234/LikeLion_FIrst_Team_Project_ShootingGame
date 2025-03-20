using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public int attack;

    Animator moveAni;

    public GameObject bullet;
    public Transform pos;

    //public GameObject powerUpItem;

    void Start()
    {
        moveAni = GetComponent<Animator>();
        InvokeRepeating("Shoot", 2f, 2f);
    }

    
    void Update()
    {
        float distanceX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        if (Input.GetAxis("Horizontal") <= -0.5f)
            moveAni.SetBool("left", true);
        else
            moveAni.SetBool("left", false);

        if (Input.GetAxis("Horizontal") >= 0.5f)
            moveAni.SetBool("right", true);
        else
            moveAni.SetBool("right", false);

        transform.Translate(distanceX, 0, 0);
        
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EBullet"))
        {
            Destroy(gameObject);
        }
    }

    void Shoot()
    {
        Instantiate(bullet, pos.position, Quaternion.identity);
    }
}
