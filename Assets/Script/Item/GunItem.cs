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
    
    // 이벤트와 플레이어 추가 중 하나만 선택하는 옵션
    [SerializeField] private bool useDirectPlayerAdd = true; // true: 직접 추가, false: 이벤트 사용
    
    // 스테이지 변경 시 파괴 여부
    [SerializeField] private bool destroyOnStageChange = false; // 스테이지 변경 시 파괴할지 여부
    
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
        Debug.Log("GunItem 초기화 완료: HP = " + hp);
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
            Debug.Log("GunItem이 총알에 맞음! 남은 HP: " + (hp-1));
            
            hp--;
            UpdateText();
            ShowHitEffect();
            
            if (hp <= 0)
            {
                Debug.Log("GunItem HP가 0이 되어 파괴됨!");
                
                // 두 가지 방법 중 하나만 선택 (중복 방지)
                if (useDirectPlayerAdd)
                {
                    // 방법 1: PlayerManager를 통해 직접 플레이어 추가
                    if (PlayerManager.instance != null)
                    {
                        Debug.Log("방법 1: PlayerManager를 통해 플레이어 추가 시도...");
                        int beforeCount = PlayerManager.instance.GetPlayerCount();
                        
                        PlayerManager.instance.AddPlayer();
                        
                        int afterCount = PlayerManager.instance.GetPlayerCount();
                        Debug.Log("플레이어 추가 결과: " + beforeCount + " -> " + afterCount);
                    }
                    else
                    {
                        Debug.LogError("PlayerManager가 없어 플레이어를 추가할 수 없습니다!");
                        // 플레이어 추가 실패 시 이벤트 방식 시도
                        TriggerWeaponUpgradeEvent();
                    }
                }
                else
                {
                    // 방법 2: 무기 업그레이드 이벤트를 통해 추가
                    TriggerWeaponUpgradeEvent();
                }
                
                Destroy(gameObject);
            }
        }
    }
    
    // 무기 업그레이드 이벤트 발생 (방법 2)
    private void TriggerWeaponUpgradeEvent()
    {
        if (EventManager.instance != null && EventManager.instance.playerEvents != null)
        {
            Debug.Log("방법 2: 무기 업그레이드 이벤트 발생!");
            EventManager.instance.playerEvents.WeaponUpgrade();
        }
        else
        {
            Debug.LogError("EventManager 또는 playerEvents가 없어 무기 업그레이드를 할 수 없습니다!");
        }
    }
    
    private void ChangeStage(int stage)
    {
        if (destroyOnStageChange)
        {
            Debug.Log("스테이지 변경으로 GunItem 제거: 스테이지 " + stage);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("스테이지가 " + stage + "로 변경되었지만 GunItem 유지");
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

        colorCo = StartCoroutine(ChangeCo());
    }
}