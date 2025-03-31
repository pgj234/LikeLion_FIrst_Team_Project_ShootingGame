using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUi : MonoBehaviour
{
    void OnEnable()
    {
        SoundManager.instance.StopBGM();
        SoundManager.instance.PlaySFX(Sound.GameOver);
        Time.timeScale = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneMaster.instance.OnClickReStart();
            Time.timeScale = 1;
        }
    }
}
