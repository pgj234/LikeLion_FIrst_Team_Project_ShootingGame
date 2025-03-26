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
    private float curSpeed = 0f;
    
    private void Start()
    {
        EventManager.instance.stageEvents.onChangeStage += ChangeStage;
    }

    private void OnDestroy()
    {
        EventManager.instance.stageEvents.onChangeStage -= ChangeStage;
    }


    public void Init()
    {
        this.hp = StageManager.instance.GetFenceHP();
        this.curSpeed = StageManager.instance.GetCurSpeed();
        InitSprite();
        InitText();
        UpdateText();
        Destroy(gameObject, 20);//임시작업: 20초후 삭제.
    }

    private void Update()
    {
        //rigidbody velocity에서, translate로 변경.
        transform.Translate(Vector2.down * curSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PBullet"))
        {
            hp--;
            UpdateText();
            ShowHitEffect();
            if (hp <= 0)
            {
                EventManager.instance.playerEvents.WeaponUpgrade();
                Destroy(gameObject);
            }
        }
    }
    private void ChangeStage(int stage)
    {
        Destroy(gameObject);
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
    private Coroutine colorCo = null;

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