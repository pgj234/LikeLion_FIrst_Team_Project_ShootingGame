using UnityEngine;

public class BossClearUi : MonoBehaviour
{
    void OnEnable()
    {
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySFX(Sound.Victory);
    }

    public void Home()
    {
        SceneMaster.instance.OnClickHome();
    }

    public void Restart()
    {
        SceneMaster.instance.OnClickReStart();
    }
}
