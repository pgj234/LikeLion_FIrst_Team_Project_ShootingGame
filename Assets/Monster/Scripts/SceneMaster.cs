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
		// ���� ���� ��ư Ŭ�� �� ���ø����̼� ����
		Application.Quit();

#if UNITY_EDITOR
		EditorApplication.isPlaying = false;  // Unity �����Ϳ��� ���� ����
#endif
	}

}
