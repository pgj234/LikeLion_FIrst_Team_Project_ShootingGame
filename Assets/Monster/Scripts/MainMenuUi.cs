using UnityEngine;

public class MainMenuUi : MonoBehaviour
{
    public void StartBtn()
    {
        SceneMaster.instance.OnClickStart();
    }

    public void QuitBtn()
    {
        SceneMaster.instance.OnClickOut();
    }
}
