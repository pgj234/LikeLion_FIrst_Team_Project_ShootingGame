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
        InvokeRepeating("Shoot", 1f, 1f);
    }

    
    void Update()
    {
        

        float distanceX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        if (Input.GetAxis("Horizontal") <= -2f)
            moveAni.SetBool("left", true);
        else
            moveAni.SetBool("left", false);

        if (Input.GetAxis("Horizontal") >= 2f)
            moveAni.SetBool("right", true);
        else
            moveAni.SetBool("right", false);

        transform.Translate(distanceX, 0, 0);

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        transform.position = worldPos;
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
