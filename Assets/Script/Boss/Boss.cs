using System;
using System.Collections;
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

    //�÷��̾� �������� Ȯ��
    bool check;

    public int HP
    {
        get => BHP;
        //0���Ϸ� �������� ����
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
        while (isPlayerAlive)
        {
            //�̻���
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

    private void Update()
    {
        //�ÿ��̾ �׾����� �Ѿ� �߻縦 ���߱�
        if (GameObject.FindWithTag("Player") != null)
        {
            isPlayerAlive = !GameObject.FindWithTag("Player").GetComponent<Player>().isDead;
        }
        else
        {
            isPlayerAlive = false;
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
            Destroy(gameObject);
        }
    }
}
