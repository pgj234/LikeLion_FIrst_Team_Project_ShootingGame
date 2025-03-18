using UnityEngine;

public class GunItem : MonoBehaviour
{

    [SerializeField] private TextMesh text;
    [SerializeField] private Rigidbody2D rb2d;

    //data
    private int hp = 0;



    public void Init(int hp, float velocity)
    {
        this.hp = hp;
        text.text = hp.ToString();
        rb2d.linearVelocityY = -velocity;

        Destroy(gameObject, 20);//임시작업: 20초후 삭제.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (true)//TODO. 총알 판단필요.
        {
            hp--;
            UpdateText();
            if (hp <= 0)
            {
                //TODO. 플레이어 총기류 업그레이드
                Destroy(gameObject);
            }
        }
    }
    void UpdateText()
    {
        text.text = hp.ToString();
    }
}
