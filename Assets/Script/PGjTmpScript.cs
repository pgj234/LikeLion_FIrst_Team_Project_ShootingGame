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

    // 총알 데미지 및 비주얼 업글
    public void BulletChange(Collider2D col)
    {

    }

    // 플레이어 수 교체
    public void PlayerNumChange(Collider2D col)
    {
        
    }
}
