using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public Bullet bullet;
    //public Boss boss;

    [HideInInspector] public float speed = 4.0f;      // ź��
    [HideInInspector] public int attack;              // ���ݷ�

    Rigidbody2D rb;

    void Update()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.up * speed; // Y�� �������� �̵�
        //rb.angularVelocity = 360f;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    void OnDestroy()
    {
        DOTween.Kill(transform);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
        {
            collision.gameObject.GetComponent<Boss>().Damage(attack);

            StartCoroutine(BulletDestroyAndEffect());
        }
        
        if(collision.CompareTag("Monster"))
        {
            StartCoroutine(BulletDestroyAndEffect());
        }

        if (collision.CompareTag("Fence"))
        {
            StartCoroutine(BulletDestroyAndEffect());
        }

        // �÷��̾� ���� ��Ÿ��
        if (collision.CompareTag("PeopleUpFence"))
        {
            StartCoroutine(BulletDestroyAndEffect());
        }
    }

    IEnumerator BulletDestroyAndEffect()
    {
        speed = 0;
        GetComponent<PolygonCollider2D>().enabled = false;

        transform.DOScale(Vector2.zero, 0.5f);
        transform.DORotate(new Vector3(0, 0, 1500), 0.5f).SetRelative();

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}