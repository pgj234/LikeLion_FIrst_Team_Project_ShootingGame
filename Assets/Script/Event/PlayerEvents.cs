using System;

public class PlayerEvents
{
    public event Action onWeaponUpgrade;
    public event Action onPlayerDead;
    public event Action onMonsterDead;
    public void WeaponUpgrade()
    {
        onWeaponUpgrade?.Invoke();
    }
    public void PlayerDead()
    {
        onPlayerDead?.Invoke();
    }
    public void MonsterDead()
    {
        onMonsterDead?.Invoke();
    }
}
