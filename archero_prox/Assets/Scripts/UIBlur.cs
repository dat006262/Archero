




using UnityEngine;
using UnityEngine.UI;

public class UIBlur : MonoBehaviour
{
	private Material mMat;

	private Image image;

	private bool bStartUpdate;

	private float startBlur;

	private float endBlur = 4f;

	private float blurTime = 0.12f;

	private float startTime;

	private float endTime;

	private int Shader_Size;

	private void Awake()
	{
		image = GetComponent<Image>();
		mMat = image.material;
		Shader_Size = Shader.PropertyToID("_Size");
	}

	private void OnEnable()
	{
		bStartUpdate = true;
		startTime = Time.time;
		endTime = startTime + blurTime;
	}

	private void Update()
	{
		if (bStartUpdate)
		{
			float num = (Time.time - startTime) / blurTime;
			if (num < 1f)
			{
				mMat.SetFloat(Shader_Size, num * (endBlur - startBlur) + startBlur);
				return;
			}
			mMat.SetFloat(Shader_Size, endBlur);
			bStartUpdate = false;
		}
	}
}
