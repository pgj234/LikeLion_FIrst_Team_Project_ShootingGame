using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{



    // ���� �Ҹ�
    public GameObject bossbullet;
    public GameObject bossbullet2;
    public Transform pos1;
    public Transform pos2;

    //���� ü��
    public int bossHp=3;

    //�Ѿ�
    public GameObject PlayerBullet;

    void Start()
    {
        StartCoroutine(BossBullet());
        StartCoroutine(CircleFire());
    }
    IEnumerator BossBullet()
    {
        while (true)
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
        while (true)
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

    public void Damage(int attack)
    {
        bossHp -= attack;

        if(bossHp<=0)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {

    }
}
