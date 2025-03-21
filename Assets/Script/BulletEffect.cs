using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    protected IEnumerator BulletDestroyAndEffect(GameObject bulletObj)
    {
        bulletObj.GetComponent<PolygonCollider2D>().enabled = false;

        bulletObj.transform.DOScale(Vector2.zero, 0.5f);
        bulletObj.transform.DORotate(new Vector3(0, 0, 1500), 0.5f).SetRelative();

        yield return new WaitForSeconds(0.5f);

        Destroy(bulletObj);
    }
}