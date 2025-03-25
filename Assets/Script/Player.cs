using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public int attack;

    Animator moveAni;

    public GameObject bullet;
    public Transform pos;

    //public GameObject powerUpItem;

    public float attackSpeed = 1;
    float shotBasicReduceSpeed = 0.05f;
    float maxShotSpeed = 0.15f;

    WaitForSeconds wait;

    void Start()
    {
        wait = new WaitForSeconds(attackSpeed);

        moveAni = GetComponent<Animator>();

        StartCoroutine(Shoot());
        EventManager.instance.playerEvents.onWeaponUpgrade += ShootSpeedSet;
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


        //Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        //viewPos.x = Mathf.Clamp01(viewPos.x);
        //viewPos.y = Mathf.Clamp01(viewPos.y);
        //Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        //transform.position = worldPos;

        Vector3 groundPos;
        groundPos = transform.position;
        groundPos.x = Mathf.Clamp(groundPos.x, -2, 2);
        transform.position = groundPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EBullet"))
        {
            Destroy(gameObject);
            EventManager.instance.playerEvents.onWeaponUpgrade -= ShootSpeedSet;
        }
    }

    // 공속 및 발사체 속도 변경
    void ShootSpeedSet()
    {
        attackSpeed -= shotBasicReduceSpeed;

        if (attackSpeed < maxShotSpeed)
        {
            attackSpeed = maxShotSpeed;
        }

        wait = new WaitForSeconds(attackSpeed);

        Debug.Log("attackSpeed : " + attackSpeed);
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return wait;

            GameObject go = Instantiate(bullet, pos.position, Quaternion.identity);
            go.GetComponent<Bullet>().speed += (1 - attackSpeed) * 4;
        }
    }
}
