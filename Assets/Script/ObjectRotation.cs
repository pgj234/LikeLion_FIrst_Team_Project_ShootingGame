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
        // 게임 오브젝트가 활성화 되어있는 동안
        while (true == gameObject.activeSelf)
        {
            transform.DORotate(new Vector3(0, 0, 360), rotateSpd).SetEase(Ease.Linear).SetRelative();
            
            yield return wait;
        }
    }
}
