using UnityEngine;

public class UI_Stage : MonoBehaviour
{
    public void OnClick()
    {
        gameObject.SetActive(false);
        StageManager.instance.MoveStage(StageManager.instance.curStage + 1);
        SoundManager.instance.PlaySFX(Sound.ButtonClick);
    }
}
