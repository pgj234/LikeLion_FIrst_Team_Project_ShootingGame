using System;

public class StageEvents
{
    public event Action<int> onChangeStage;
    public void ChangeStage(int stage)
    {
        onChangeStage?.Invoke(stage);
    }
}
