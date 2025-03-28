using UnityEngine;

public class BossClearUi : MonoBehaviour
{
    public void Home()
    {
        SceneMaster.instance.OnClickHome();
    }

    public void Restart()
    {
        SceneMaster.instance.OnClickReStart();
    }
}
