using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleItem : MonoBehaviour
{

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private TextMesh text;
    [SerializeField] private TextMesh textShadow;
    [SerializeField] private int textSortingOrder;
    [SerializeField] Color red;
    [SerializeField] Color blue;


    private int power;
    private float curSpeed = 0f;
    private Coroutine scaleCo = null;
    private int originFontSize;
    private int targetFontSize;
    public void Init()
    {
        this.power = StageManager.instance.GetPeopleFenceInitValue();
        this.curSpeed = StageManager.instance.GetCurSpeed();
        sr.color = power < 0 ? red : blue;
        InitText();
        UpdateText();
    }

    void Update()
    {
        transform.Translate(Vector2.down * curSpeed * Time.deltaTime);
        //화면밖으로 나가면 삭제.
        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PBullet"))
        {
            power++;
            UpdateText();
            UpdateSprite();
            ShowHitEffect();
        }
    }

    private void UpdateSprite()
    {
        sr.color = power < 0 ? red : blue;
    }

    void InitText()
    {
        originFontSize = text.fontSize;
        targetFontSize = originFontSize + 10;
        SetSortingOrder(text.GetComponent<MeshRenderer>(), textSortingOrder);
        SetSortingOrder(textShadow.GetComponent<MeshRenderer>(), textSortingOrder - 1);
    }

    void SetSortingOrder(Renderer render, int sortingOrder)
    {
        render.sortingLayerName = "Default";
        render.sortingOrder = sortingOrder;
    }
    void UpdateText()
    {
        string prefix = power < 0 ? "-" : "+";
        string powerText = $"{prefix} {Mathf.Abs(power)}";
        text.text = powerText;
        textShadow.text = powerText;
    }
    void ShowHitEffect()
    {
        if (scaleCo != null)
            StopCoroutine(scaleCo);

        StartCoroutine(ScaleCo());
    }
    IEnumerator ScaleCo()
    {
        text.fontSize = targetFontSize;
        textShadow.fontSize = targetFontSize;
        yield return new WaitForSeconds(0.1f); //0.1초 유지.

        float duration = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            text.fontSize = (int)Mathf.Lerp(targetFontSize, originFontSize, t);
            textShadow.fontSize = text.fontSize;
            yield return null;
        }

        text.fontSize = originFontSize;
        textShadow.fontSize = originFontSize;
    }
}
