using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{



    // 보스 불릿
    public GameObject bossbullet;
    public GameObject bossbullet2;
    public Transform pos1;
    public Transform pos2;
    [SerializeField] private Slider bhpBar;

    private int BHP;

    public int HP
    {
        get => BHP;
        //0이하로 떨어지지 않음
        private set => BHP = Math.Clamp(value, 0, BHP);
    }

    private void Awake()
    {
        BHP = 4000;
        SetMaxHP(BHP);
    }
    public void SetMaxHP(int health)
    {
        bhpBar.maxValue = health;
        bhpBar.value = health;
    }

    void Start()
    {
        StartCoroutine(BossBullet());
        StartCoroutine(CircleFire());
    }
    IEnumerator BossBullet()
    {
        while (true)
        {
            //미사일
            Instantiate(bossbullet, pos1.position, Quaternion.identity);
            yield return new WaitForSeconds(2.0f);

        }
    }
    IEnumerator CircleFire()
    {
        //공격주기
        float attackRATE = 5;
        int count = 10;
        //발사체 사이의 각도
        float intervalangle = -180 / count;
        //가중되는 각도(같은 위치로 발사하지 않도록 설정)
        float weightangle = 0f;

        //원 형태로 발사하는 발사체 생성(count 만큼 생성)
        while (true)
        {
            for (int i = 0; i < count; ++i)
            {
                //발사체 생성
                GameObject clone = Instantiate(bossbullet2, transform.position, Quaternion.identity);
                //발사체 이동방향(각도)
                float angle = weightangle + intervalangle * i;
                //발사체 이동방향(벡터)
                //cos(각도) 라디안 단위의 각도 표현을 위해 pi/180을 곱함
                float x = Mathf.Cos(angle * Mathf.Deg2Rad);
                //sin(각도) 라디안 단위의 각도표현을 위해 pi/180을 곱함
                float y = Mathf.Sin(angle * Mathf.Deg2Rad);
                //발사체 이동방향 설정
                clone.GetComponent<BossBullet2>().Move(new Vector2(x, y));
            }
            //발사체가 생성도
            weightangle += 1;
            //3초마다 미사일 발사
            yield return new WaitForSeconds(attackRATE);
        }
    }

    private void Update()
    {

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
            Destroy(gameObject);
        }
    }
}
