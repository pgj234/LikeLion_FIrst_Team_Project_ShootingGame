using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // ���� �Ҹ�
    public GameObject bossbullet;
    public GameObject bossbullet2;
    public Transform pos1;
    public Transform pos2;
    [SerializeField] private Slider bhpBar;

    private int BHP;
    private bool isPlayerAlive = true;

    //체력 절반 이하 체크
    private bool isMovingSide;
    private bool isFast = false; //탄막속도

    private float attackRate = 5f; // 탄막 발사 간격
    private float bulletSpeedMultiplier = 1f; // 탄속 배율


    public int HP
    {
        get => BHP;
        //0���Ϸ� �������� ����
        private set => BHP = Math.Clamp(value, 0, BHP);
    }
    private Vector3 startPosition; // 시작 위치 (화면 위쪽)
    private Vector3 targetPosition; // 목표 위치 (고정될 위치)
    private float moveDuration = 3f; // 이동에 걸리는 시간
    private bool hasReachedTarget = false;
    private void Awake()
    {
        BHP = 4000;
        SetMaxHP(BHP);
        // 화면의 위쪽 외부로 시작 위치 설정 (카메라의 orthographicSize 사용)
        float screenTop = Camera.main.orthographicSize + 2f; // 화면 위쪽 외부
        startPosition = new Vector3(transform.position.x, screenTop, transform.position.z);

        // 보스가 내려올 목표 위치 설정 (화면의 중간 혹은 원하는 고정된 위치)
        targetPosition = new Vector3(transform.position.x, 3.2556f, transform.position.z); // 원하는 고정 위치로 조정 가능
    
    }


    public void SetMaxHP(int health)
    {
        bhpBar.maxValue = health;
        bhpBar.value = health;
    }

    void Start()
    {
        StartCoroutine(MoveDownLerp());
        StartCoroutine(BossBullet());  
        StartCoroutine(CircleFire());
    }
    IEnumerator BossBullet()
    {
        while (!hasReachedTarget) // 보스가 목표 위치에 도달할 때까지 대기
        {
            yield return null;
        }

        while (isPlayerAlive) // 이제 보스가 목표 위치에 도달한 후부터만 실행
        {
            Instantiate(bossbullet, pos1.position, Quaternion.identity);
            yield return new WaitForSeconds(2.0f);
        }
    }
    IEnumerator CircleFire()
    {
        //�����ֱ�
        float attackRATE = 5;
        int count = 10;
        //�߻�ü ������ ����
        float intervalangle = -180 / count;
        //���ߵǴ� ����(���� ��ġ�� �߻����� �ʵ��� ����)
        float weightangle = 0f;

        while (!hasReachedTarget) // 보스가 목표 위치에 도달할 때까지 대기
        {
            yield return null;
        }
        //�� ���·� �߻��ϴ� �߻�ü ����(count ��ŭ ����)
        while (isPlayerAlive)
        {
            for (int i = 0; i < count; ++i)
            {
                //�߻�ü ����
                GameObject clone = Instantiate(bossbullet2, transform.position, Quaternion.identity);
                //�߻�ü �̵�����(����)
                float angle = weightangle + intervalangle * i;
                //�߻�ü �̵�����(����)
                //cos(����) ���� ������ ���� ǥ���� ���� pi/180�� ����
                float x = Mathf.Cos(angle * Mathf.Deg2Rad);
                //sin(����) ���� ������ ����ǥ���� ���� pi/180�� ����
                float y = Mathf.Sin(angle * Mathf.Deg2Rad);
                //�߻�ü �̵����� ����
                clone.GetComponent<BossBullet2>().Move(new Vector2(x, y));
            }
            //�߻�ü�� ������
            weightangle += 1;
            //3�ʸ��� �̻��� �߻�
            yield return new WaitForSeconds(attackRATE);
        }
    }
    private IEnumerator MoveDownLerp()
    {
        float elapsedTime = 0f;

        // Lerp를 통해 보스를 화면 상단에서 지정된 위치로 부드럽게 이동시킴
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 목표 위치에 정확히 도달하도록 보정
        transform.position = targetPosition;
        //등장 후 총알 발사
        hasReachedTarget = true;
        StartCoroutine(BossBullet());
        StartCoroutine(CircleFire());
    }
    private void Update()
    {
        //�ÿ��̾ �׾����� �Ѿ� �߻縦 ���߱�
        if (GameObject.FindWithTag("Player") != null)
        {
            isPlayerAlive = !GameObject.FindWithTag("Player").GetComponent<Player>().GetPlayerManager().isDead;

            if (!isPlayerAlive)
            {
                GetComponent<Collider2D>().enabled = false; // 콜라이더 비활성화
            }
        }
        else
        {
            isPlayerAlive = false;
        }
        // 체력이 절반 이하가 되면 좌우 이동 + 탄속 증가
        if (!isMovingSide && HP <= BHP / 2)
        {
            isMovingSide = true;
            StartCoroutine(MoveSideToSide());
        }

        if (!isFast && HP <= BHP / 2)
        {
            isFast = true;
            bulletSpeedMultiplier = 2f; // 탄막 속도 2배 증가
        }
    }
    private IEnumerator MoveSideToSide()
    {
        float moveSpeed = 3f;
        float moveDistance = 2f;
        bool movingRight = true;

        while (isPlayerAlive && HP > 0)
        {
            float targetX = movingRight ? targetPosition.x + moveDistance : targetPosition.x - moveDistance;
            float elapsedTime = 0f;
            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(targetX, transform.position.y, transform.position.z);

            while (elapsedTime < 1f)
            {
                transform.position = Vector3.Lerp(startPos, endPos, elapsedTime);
                elapsedTime += Time.deltaTime * moveSpeed;
                yield return null;
            }

            transform.position = endPos;
            movingRight = !movingRight;
            yield return new WaitForSeconds(0.5f);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);

    }

    public void Damage(int ATK)
    {
        int getDamage =  HP -= ATK;
        HP = getDamage;
        bhpBar.value = HP;
        if(HP <= 0)
        {
            StartCoroutine(CameraShake.instance.Shake(1f, 0.3f));
            Destroy(gameObject);
        }
    }
   
}
