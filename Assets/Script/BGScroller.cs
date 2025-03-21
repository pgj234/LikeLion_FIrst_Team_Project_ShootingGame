using UnityEngine;

public class BGScroller : MonoBehaviour
{
    public float speed = 5f;
    public GameObject[] step1;
    public GameObject[] step2;
    bool isStep1_Top;
    Vector3 gapY = new Vector3(0, 10, 0);
    private void Update()
    {

        if (isStep1_Top)
        {
            for (int i = 0; i < step1.Length; i++)
            {
                step2[i].transform.Translate(speed * Vector2.down * Time.deltaTime);
            }
            for (int i = 0; i < step2.Length; i++)
            {
                step1[i].transform.position = step2[i].transform.position + gapY;
            }

            if (step2[0].transform.position.y < -10)
            {
                isStep1_Top = false;
            }
        }
        else
        {
            for (int i = 0; i < step2.Length; i++)
            {
                step1[i].transform.Translate(speed * Vector2.down * Time.deltaTime);
            }
            for (int i = 0; i < step1.Length; i++)
            {
                step2[i].transform.position = step1[i].transform.position + gapY;
            }

            if (step1[0].transform.position.y < -10)
            {
                isStep1_Top = true;
            }
        }
    }
}