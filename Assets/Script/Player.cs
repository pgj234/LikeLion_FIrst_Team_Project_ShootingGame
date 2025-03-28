using System.Collections;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject gameOverUIObj;      // 게임오버 UI 오브젝트

    [Space(10)]
    [Tooltip("0:아이스, 1:파이어, 2:베이직, 3:화이트")]
    [SerializeField] GameObject[] bulletObjArray;

    //public GameObject powerUpItem;

    internal float attackSpeed = 1;           // 공속
    internal float shotBasicReduceSpeed = 0.2f;      // 기본적인 업글당 오르는 공속치 (임시 테스트 수치 기존:0.05)
    internal float maxShotSpeed = 0.15f;             // 공속 최대치
    internal WaitForSeconds wait;                    // 공속 wait 시간

    public Transform pos;

    Animator moveAni;

    PlayerManager playerManager;   // 플레이어 매니저

    internal bool isCopyPlayer;

    internal PlayerManager GetPlayerManager()
    {
        return playerManager;
    }

    void Awake()
    {
        moveAni = GetComponent<Animator>();
        playerManager = transform.parent.GetComponent<PlayerManager>();
    }

    void Start()
    {
        StartCoroutine(Shoot());
    }

    internal void Init()
    {
        if (true == isCopyPlayer)
        {
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().DOFade(0.4f, 1).SetEase(Ease.Linear);
            return;
        }

        attackSpeed = 1;
        shotBasicReduceSpeed = 0.2f;
        maxShotSpeed = 0.15f;

        wait = new WaitForSeconds(attackSpeed);

        EventManager.instance.playerEvents.onWeaponUpgrade += playerManager.ShootSpeedSet;
        EventManager.instance.playerEvents.onPlayerDead += GameOverUiOpen;
        EventManager.instance.playerEvents.onMonsterDead += playerManager.ChangeBullet;
    }

    //void Update()
    //{
    //    if (Input.GetAxis("Horizontal") <= -2f)
    //        moveAni.SetBool("left", true);
    //    else
    //        moveAni.SetBool("left", false);

    //    if (Input.GetAxis("Horizontal") >= 2f)
    //        moveAni.SetBool("right", true);
    //    else
    //        moveAni.SetBool("right", false);
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (true == isCopyPlayer)
        {
            return;
        }

        // 적 탄
        if (collision.CompareTag("EBullet"))
        {
            playerManager.isDead = true;

            EventManager.instance.playerEvents.PlayerDead();
            EventManager.instance.playerEvents.onPlayerDead -= GameOverUiOpen;

            //Destroy(gameObject);
            GetComponent<CapsuleCollider2D>().enabled = false;
            EventManager.instance.playerEvents.onWeaponUpgrade -= playerManager.ShootSpeedSet;

            EventManager.instance.playerEvents.onMonsterDead -= playerManager.ChangeBullet;
        }

        // 플레이어 증감 울타리
        if (collision.CompareTag("PeopleUpFence"))
        {
            var peopleItem = collision.GetComponentInParent<PeopleItem>();
            if (peopleItem != null)
            {
                playerManager.PlayerAddOrDecrease(peopleItem.PowerGet());
                Destroy(peopleItem.gameObject);
            }
        }
    }

    internal IEnumerator Shoot()
    {
        while (false == playerManager.isDead)
        {
            yield return wait;

            if (false == UIManager.instance.ClearStageUiOpenStateGet())
            {
                GameObject go = Instantiate(bulletObjArray[playerManager.weaponLevel - 1], pos.position, Quaternion.identity);
                Bullet bulletScript = go.GetComponent<Bullet>();
                bulletScript.speed += (1 - attackSpeed) * 4;
                bulletScript.attack = playerManager.attack;
            }
        }
    }

    void GameOverUiOpen()
    {
        gameOverUIObj.SetActive(true);
    }
}