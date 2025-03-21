using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunItem : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private TextMesh text;
    [SerializeField] private TextMesh textShadow;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private int textSortingOrder;
    [SerializeField] private Color originColor = Color.white;
    [SerializeField] private Color attackedColor = Color.red;

    private int hp = 0;
    private Coroutine colorCo = null;
    private void Start()
    {
        BGScroller.onChangeSpeed += ChangeSpeed;
    }
    private void OnDestroy()
    {
        BGScroller.onChangeSpeed -= ChangeSpeed;
    }

    private void ChangeSpeed(float speed)
    {
        rb2d.linearVelocityY = -speed;
    }

    public void Init(int hp)
    {
        this.hp = hp;
        InitSprite();
        InitText();
        UpdateText();
        rb2d.linearVelocityY = -BGScroller.curSpeed;

        Destroy(gameObject, 20);//임시작업: 20초후 삭제.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PBullet"))//플레이어 총알이면,
        {
            hp--;
            UpdateText();
            ShowHitEffect();
            if (hp <= 0)
            {
                //TODO. 플레이어 총기류 업그레이드
                Destroy(gameObject);
            }
        }
    }
    void InitSprite()
    {
        sr.color = originColor;
    }
    void InitText()
    {
        SetSortingOrder(text.GetComponent<MeshRenderer>(), textSortingOrder);
        SetSortingOrder(textShadow.GetComponent<MeshRenderer>(), textSortingOrder - 1);
    }

    void SetSortingOrder(Renderer render, int sortingOrder)
    {
        render.sortingLayerName = "Default";
        render.sortingOrder = sortingOrder;
    }

    IEnumerator ChangeCo()
    {
        sr.color = attackedColor;
        yield return new WaitForSeconds(0.1f);

        float duration = 0.2f; // 원래 색으로 복귀하는 데 걸리는 시간
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            sr.color = Color.Lerp(attackedColor, originColor, t);
            yield return null;
        }

        sr.color = originColor;
    }
    void UpdateText()
    {
        text.text = hp.ToString();
        textShadow.text = hp.ToString();
    }
    void ShowHitEffect()
    {
        if (colorCo != null)
            StopCoroutine(colorCo);

        StartCoroutine(ChangeCo());
    }
}