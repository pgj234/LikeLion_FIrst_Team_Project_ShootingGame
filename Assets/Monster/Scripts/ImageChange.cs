using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageChange : MonoBehaviour
{
	[SerializeField] float textLerpTime;

	[SerializeField] Color startColor = Color.white;
	[SerializeField] Color endColor = Color.red;

	Image image;


	void Awake()
	{
		image = GetComponent<Image>();
	}
	void Start()
	{
		StartCoroutine( TextColorChange() );
	}

	void Update()
	{

	}

	IEnumerator TextColorChange()
	{
		while (true)
		{
			yield return StartCoroutine( TextColorLerp( startColor, endColor ) );
			yield return StartCoroutine( TextColorLerp( endColor, startColor ) );
		}
	}

	IEnumerator TextColorLerp(Color start, Color end)
	{
		float currentTime = 0;
		float percent = 0;

		while (percent < 1)
		{
			currentTime += Time.deltaTime;
			percent = currentTime / textLerpTime;
			image.color = Color.Lerp( start, end, percent );
			yield return null;
		}
	}
}
