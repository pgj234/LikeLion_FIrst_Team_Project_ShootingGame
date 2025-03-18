using UnityEngine;

// 아주 적은 기능들이라 인터페이스 이름을 이니셜로 해놓음
interface PGJInterface
{
    void BulletChange(Collider2D col);
    void WeaponChange(Collider2D col);
}
