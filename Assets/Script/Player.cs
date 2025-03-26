using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject gameOverUIObj;      // ���ӿ��� UI ������Ʈ

    public float speed;
    public int attack;

    public int monsterKillCount=0;

    Animator moveAni;

    public GameObject bullet;
    public Transform pos;

    EventManager eventM;

    //public GameObject powerUpItem;

    public float attackSpeed = 1;           // ����
    float shotBasicReduceSpeed = 0.05f;     // �⺻���� ���۴� ������ ����ġ
    float maxShotSpeed = 0.15f;             // ���� �ִ�ġ

    WaitForSeconds wait;                    // ���� wait �ð�

    void Start()
    {
        attack = 1;
        wait = new WaitForSeconds(attackSpeed);

        moveAni = GetComponent<Animator>();

        StartCoroutine(Shoot());
        EventManager.instance.playerEvents.onWeaponUpgrade += ShootSpeedSet;
        EventManager.instance.playerEvents.onPlayerDead += GameOverUiOpen;
        EventManager.instance.playerEvents.onMonsterDead += ChangeBullet;
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
            EventManager.instance.playerEvents.PlayerDead();
            EventManager.instance.playerEvents.onPlayerDead -= GameOverUiOpen;
            
            //Destroy(gameObject);
            GetComponent<Collider2D>().enabled = false;
            EventManager.instance.playerEvents.onWeaponUpgrade -= ShootSpeedSet;
        }
    }

    // ���� �� �߻�ü �ӵ� ����
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
            Bullet bulletScript = go.GetComponent<Bullet>();
            bulletScript.speed += (1 - attackSpeed) * 4;
            bulletScript.attack = this.attack;
        }
    }

    void ChangeBullet() //���⺯�� 1���� 2��, 2���� 3��, 3���� 4��, 5���� 5��(�ӽ÷� ����) 
    {
        monsterKillCount += 1;

        if (monsterKillCount == 1)
            attack = 2;
        else if (monsterKillCount == 2)
            attack = 6;
        else if (monsterKillCount == 3)
            attack = 24;
        else if (monsterKillCount == 4)
            attack = 125;


            //Debug.Log("���� ����Ƚ��: " +monsterKillCount );
            Debug.Log("���ݷ�: " + attack);
    }

    void GameOverUiOpen()
    {
        gameOverUIObj.SetActive(true);
    }
}