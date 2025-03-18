using UnityEngine;

enum WeaponKind
{
    Feather,
    None
}

public class PGjTmpScript : MonoBehaviour, PGJInterface
{
    // 한번에 나가는 총알 수
    int bulletNum = 0;

    public void BulletChange(Collider2D col)
    {
        // 총알 개수 변경 (아래 둘 중 하나)
        // bulletNum = col.
        // bulletNum += col.
    }

    public void WeaponChange(Collider2D col)
    {
        // 무기 변경
        //col.
    }
}
