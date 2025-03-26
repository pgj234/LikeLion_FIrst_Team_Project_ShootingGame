using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject gameOverUIObj;      // 게임오버 UI 오브젝트

    [Space(10)]
    [Tooltip("0:아이스, 1:파이어, 2:베이직, 3:화이트")]
    [SerializeField] GameObject[] bulletObjArray;

    [Space(10)]
    public float speed;
    public int attack;
    public int weaponLevel;

    //public GameObject powerUpItem;

    public float attackSpeed = 1;           // 공속
    float shotBasicReduceSpeed = 0.2f;      // 기본적인 업글당 오르는 공속치 (임시 테스트 수치 기존:0.05)
    float maxShotSpeed = 0.15f;             // 공속 최대치

    public Transform pos;

    Animator moveAni;
    WaitForSeconds wait;                    // 공속 wait 시간

    [HideInInspector] public int monsterKillCount;
    [HideInInspector] public bool isDead;                     // 플레이어 죽음 여부

    void Start()
    {
        isDead = false;

        attack = 1;
        weaponLevel = 1;
        monsterKillCount = 0;

        attackSpeed = 1;
        shotBasicReduceSpeed = 0.2f;
        maxShotSpeed = 0.15f;

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
        if (collision.CompareTag("EBullet"))
        {
            isDead = true;

            EventManager.instance.playerEvents.PlayerDead();
            EventManager.instance.playerEvents.onPlayerDead -= GameOverUiOpen;

            //Destroy(gameObject);
            GetComponent<Collider2D>().enabled = false;
            EventManager.instance.playerEvents.onWeaponUpgrade -= ShootSpeedSet;

            EventManager.instance.playerEvents.onMonsterDead -= ChangeBullet;
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

            GameObject go = Instantiate(bulletObjArray[weaponLevel - 1], pos.position, Quaternion.identity);
            Bullet bulletScript = go.GetComponent<Bullet>();
            bulletScript.speed += (1 - attackSpeed) * 4;
            bulletScript.attack = this.attack;
        }
    }

    void ChangeBullet()     //무기변경  (임시) 
    {
        monsterKillCount += 1;

        if (monsterKillCount == 1)
        {
            weaponLevel += 1;
            attack = 6;
        }
        else if (monsterKillCount == 2)
        {
            weaponLevel += 1;
            attack = 24;
        }
        else if (monsterKillCount == 3)
        {
            weaponLevel += 1;
            attack = 125;
        }

        //Debug.Log("몬스터 죽인횟수: " +monsterKillCount );
        Debug.Log("공격력: " + attack);
    }

    void GameOverUiOpen()
    {
        gameOverUIObj.SetActive(true);
    }
}