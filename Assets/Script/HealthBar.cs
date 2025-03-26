using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private SpriteRenderer backgroundSprite; // 배경 스프라이트
    [SerializeField] private SpriteRenderer fillSprite;       // 채우기 스프라이트
    
    [SerializeField] private Color backgroundColor = Color.gray;  // 배경 색상
    [SerializeField] private Color fillColor = Color.green;       // 채우기 색상
    
    [SerializeField] private float maxHealth = 100f;  // 최대 체력
    private float currentHealth;                     // 현재 체력
    
    private void Start()
    {
        currentHealth = maxHealth;
        
        // 스프라이트 렌더러가 없으면 생성
        if (backgroundSprite == null)
        {
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(transform);
            bgObj.transform.localPosition = Vector3.zero;
            backgroundSprite = bgObj.AddComponent<SpriteRenderer>();
            
            // 임시 스프라이트 (흰색 픽셀)
            backgroundSprite.sprite = CreateDefaultSprite();
            backgroundSprite.color = backgroundColor;
            
            // 크기 설정
            bgObj.transform.localScale = new Vector3(0.5f, 0.1f, 1f);
        }
        
        if (fillSprite == null)
        {
            GameObject fillObj = new GameObject("Fill");
            fillObj.transform.SetParent(transform);
            fillObj.transform.localPosition = Vector3.zero;
            fillSprite = fillObj.AddComponent<SpriteRenderer>();
            
            // 임시 스프라이트 (흰색 픽셀)
            fillSprite.sprite = CreateDefaultSprite();
            fillSprite.color = fillColor;
            
            // 크기 설정
            fillObj.transform.localScale = new Vector3(0.5f, 0.1f, 1f);
            
            // 정렬 레이어 설정 (배경보다 앞에 있도록)
            fillSprite.sortingOrder = backgroundSprite.sortingOrder + 1;
        }
        
        UpdateHealthBar();
    }
    
    // 체력바 업데이트
    public void UpdateHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthBar();
    }
    
    // 체력바 설정
    public void SetHealth(float amount)
    {
        currentHealth = Mathf.Clamp(amount, 0, maxHealth);
        UpdateHealthBar();
    }
    
    // 색상 설정
    public void SetColor(Color color)
    {
        fillColor = color;
        if (fillSprite != null)
        {
            fillSprite.color = color;
        }
    }
    
    // 체력바 업데이트 (실제 시각적인 부분)
    private void UpdateHealthBar()
    {
        if (fillSprite != null)
        {
            // 체력 비율 계산
            float healthRatio = currentHealth / maxHealth;
            
            // 스케일로 체력 표시 (x축만 변경)
            Vector3 scale = fillSprite.transform.localScale;
            scale.x = 0.5f * healthRatio;
            fillSprite.transform.localScale = scale;
            
            // 앵커 포인트를 왼쪽으로 설정하여 왼쪽에서 오른쪽으로 채워지게
            fillSprite.transform.localPosition = new Vector3(
                -0.25f * (1 - healthRatio), 
                0, 
                0
            );
        }
    }
    
    // 기본 스프라이트 생성 (흰색 픽셀)
    private Sprite CreateDefaultSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
    }
} 