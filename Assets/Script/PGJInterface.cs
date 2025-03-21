using UnityEngine;

// 아주 적은 기능들이라 인터페이스 이름을 이니셜로 해놓음
interface PGJInterface
{
    void BulletChange(Collider2D col);          // 총알 데미지 및 비주얼 업글
    void PlayerNumChange(Collider2D col);       // 플레이어 수 교체
}
