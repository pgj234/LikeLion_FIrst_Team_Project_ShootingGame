using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject gameOverUIObj;      // ���ӿ��� UI ������Ʈ

    [Space(10)]
    [Tooltip("0:���̽�, 1:���̾�, 2:������, 3:ȭ��Ʈ")]
    [SerializeField] GameObject[] bulletObjArray;

    [Space(10)]
    public float speed;
    public int attack;
    public int weaponLevel;

    //public GameObject powerUpItem;

    public float attackSpeed = 1;           // ����
    float shotBasicReduceSpeed = 0.2f;      // �⺻���� ���۴� ������ ����ġ (�ӽ� �׽�Ʈ ��ġ ����:0.05)
    float maxShotSpeed = 0.15f;             // ���� �ִ�ġ

    public Transform pos;

    Animator moveAni;
    WaitForSeconds wait;                    // ���� wait �ð�

    [HideInInspector] public int monsterKillCount;
    [HideInInspector] public bool isDead;                     // �÷��̾� ���� ����

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

            GameObject go = Instantiate(bulletObjArray[weaponLevel - 1], pos.position, Quaternion.identity);
            Bullet bulletScript = go.GetComponent<Bullet>();
            bulletScript.speed += (1 - attackSpeed) * 4;
            bulletScript.attack = this.attack;
        }
    }

    void ChangeBullet()     //���⺯��  (�ӽ�) 
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

        //Debug.Log("���� ����Ƚ��: " +monsterKillCount );
        Debug.Log("���ݷ�: " + attack);
    }

    void GameOverUiOpen()
    {
        gameOverUIObj.SetActive(true);
    }
}