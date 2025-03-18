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

        Destroy(gameObject, 20);//�ӽ��۾�: 20���� ����.
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (true)//TODO. �Ѿ� �Ǵ��ʿ�.
        {
            hp--;
            UpdateText();
            if (hp <= 0)
            {
                //TODO. �÷��̾� �ѱ�� ���׷��̵�
                Destroy(gameObject);
            }
        }
    }
    void UpdateText()
    {
        text.text = hp.ToString();
    }
}
