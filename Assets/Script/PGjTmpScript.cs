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

    public void BulletChange(Collider2D col)
    {
        // �Ѿ� ���� ���� (�Ʒ� �� �� �ϳ�)
        // bulletNum = col.
        // bulletNum += col.
    }

    public void WeaponChange(Collider2D col)
    {
        // ���� ����
        //col.
    }
}
