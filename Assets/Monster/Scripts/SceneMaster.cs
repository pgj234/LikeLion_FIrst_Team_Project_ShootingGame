using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{
	public static SceneMaster instance;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void OnClickStart()
	{
		SoundManager.instance.PlaySFX(Sound.ButtonClick);
		SceneManager.LoadScene(1);
		SoundManager.instance.PlayBGM(BGM.Play);
	}

	public void OnClickReStart()
	{
        SoundManager.instance.PlaySFX(Sound.ButtonClick);
        SceneManager.LoadScene(1);
        SoundManager.instance.PlayBGM(BGM.Play);
    }

	public void OnClickHome()
	{
        SoundManager.instance.PlaySFX(Sound.ButtonClick);
        SceneManager.LoadScene(0);
        SoundManager.instance.PlayBGM(BGM.Main);
    }

	public void OnClickOut()
	{
        SoundManager.instance.PlaySFX(Sound.ButtonClick);

        // 게임 종료 버튼 클릭 시 애플리케이션 종료
        Application.Quit();

#if UNITY_EDITOR
		EditorApplication.isPlaying = false;  // Unity 에디터에서 실행 중지
#endif
	}

}
