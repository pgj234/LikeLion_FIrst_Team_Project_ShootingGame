using System;

public class StageEvents
{
    public event Action<int> onChangeStage;
    public event Action onSpawnPause;
    public void ChangeStage(int stage)
    {
        onChangeStage?.Invoke(stage);
    }

    public void SpawnPause()
    {
        onSpawnPause?.Invoke();
    }

}
