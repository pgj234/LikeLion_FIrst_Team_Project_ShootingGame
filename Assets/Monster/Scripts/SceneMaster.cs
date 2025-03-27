using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMaster : MonoBehaviour
{


	public void OnClickStart()
	{
		SceneManager.LoadScene(1);
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
