using UnityEngine;

enum WeaponKind
{
    Feather,
    None
}

public class PGjTmpScript : MonoBehaviour, PGJInterface
{
    // �ѹ��� ������ �Ѿ� ��
    int bulletNum = 0;

    // �Ѿ� ������ �� ���־� ����
    public void BulletChange(Collider2D col)
    {

    }

    // �÷��̾� �� ��ü
    public void PlayerNumChange(Collider2D col)
    {
        
    }
}
