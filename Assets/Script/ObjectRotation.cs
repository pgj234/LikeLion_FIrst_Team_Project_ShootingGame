using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    float rotateSpd = 0.5f;

    WaitForSeconds wait;

    void Start()
    {
        wait = new WaitForSeconds(rotateSpd);

        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        // ���� ������Ʈ�� Ȱ��ȭ �Ǿ��ִ� ����
        while (true == gameObject.activeSelf)
        {
            transform.DORotate(new Vector3(0, 0, 360), rotateSpd).SetEase(Ease.Linear).SetRelative();
            
            yield return wait;
        }
    }
}
