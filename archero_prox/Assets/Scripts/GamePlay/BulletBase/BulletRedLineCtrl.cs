




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class BulletRedLineCtrl : MonoBehaviour
{
	public SpriteRenderer line1;

	public SpriteRenderer line2;

	private float line1height;

	private float line2height;

	private void Awake()
	{
		if ((bool)line1)
		{
			Vector2 size = line1.size;
			line1height = size.y;
		}
		if ((bool)line2)
		{
			Vector2 size2 = line2.size;
			line2height = size2.y;
		}
	}

	public void SetLine(bool islast, float length)
	{
		if (islast)
		{
			if ((bool)line1)
			{
				line1.gameObject.SetActive(value: true);
				line1.size = new Vector2(length, line1height);
			}
			if ((bool)line2)
			{
				line2.gameObject.SetActive(value: false);
			}
		}
		else
		{
			if ((bool)line1)
			{
				line1.gameObject.SetActive(value: false);
			}
			if ((bool)line2)
			{
				line2.gameObject.SetActive(value: true);
				line2.size = new Vector2(length, line2height);
			}
		}
	}

	public void UpdateLine(bool throughinsidewall, float width)
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		float x = MathDxx.Sin(eulerAngles.y + 90f);
		Vector3 eulerAngles2 = base.transform.eulerAngles;
		Vector3 b = new Vector3(x, 0f, MathDxx.Cos(eulerAngles2.y + 90f)) * width / 2f;
		Vector3 startpos = base.transform.position + b;
		Vector3 eulerAngles3 = base.transform.eulerAngles;
		RayCastManager.CastMinDistance(startpos, eulerAngles3.y, throughinsidewall, out float mindis);
		Vector3 startpos2 = base.transform.position - b;
		Vector3 eulerAngles4 = base.transform.eulerAngles;
		RayCastManager.CastMinDistance(startpos2, eulerAngles4.y, throughinsidewall, out float mindis2);
		float length = (!(mindis < mindis2)) ? mindis2 : mindis;
		SetLine(islast: true, length);
	}

	public void PlayLineWidth(float start, float end, float time)
	{
		if ((bool)line1)
		{
			line1.transform.localScale = new Vector3(start, 1f, 1f);
			line1.transform.DOScaleX(end, time).SetEase(Ease.Linear);
		}
		if ((bool)line2)
		{
			line2.transform.localScale = new Vector3(start, 1f, 1f);
			line2.transform.DOScaleX(end, time).SetEase(Ease.Linear);
		}
	}

	public void PlayLineWidth()
	{
		PlayLineWidth(0f, 1f, 0.3f);
	}
}
