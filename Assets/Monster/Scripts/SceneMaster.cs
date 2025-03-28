using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{


	public void OnClickStart()
	{
		SceneManager.LoadScene(1);
	}

	public void OnClickReStart()
	{
		SceneManager.LoadScene(1);
	}

	public void OnClickHome()
	{
		SceneManager.LoadScene(0);
	}

	public void OnClickOut()
	{
		// 게임 종료 버튼 클릭 시 애플리케이션 종료
		Application.Quit();

#if UNITY_EDITOR
		EditorApplication.isPlaying = false;  // Unity 에디터에서 실행 중지
#endif
	}

}
