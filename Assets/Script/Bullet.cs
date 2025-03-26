using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public Bullet bullet;
    //public Boss boss;

    [HideInInspector] public float speed = 4.0f;      // Åº¼Ó
    [HideInInspector] public int attack;              // °ø°Ý·Â

    void Update()
    {
        transform.Translate(Vector2.up*speed*Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
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

    }

    IEnumerator BulletDestroyAndEffect()
    {
        GetComponent<PolygonCollider2D>().enabled = false;

        transform.DOScale(Vector2.zero, 0.5f);
        transform.DORotate(new Vector3(0, 0, 1500), 0.5f).SetRelative();

        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}