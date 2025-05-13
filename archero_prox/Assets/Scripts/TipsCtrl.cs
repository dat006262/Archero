




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class TipsCtrl : MonoBehaviour
{
	private Text text1;

	private Text text2;

	private Animation ani;

	private float time;

	private bool bPlayMiss;

	private bool bCanShowNext;

	private void Awake()
	{
		text1 = base.transform.Find("Image_BG/Text1").GetComponent<Text>();
		text2 = base.transform.Find("Image_BG/Text2").GetComponent<Text>();
		ani = GetComponent<Animation>();
	}

	private void OnEnable()
	{
		ani.enabled = true;
	}

	private void OnDisable()
	{
		ani.enabled = false;
	}

	public void Init(string value1, string value2)
	{
		text1.text = value1;
		text2.text = value2;
		time = 0f;
		bPlayMiss = false;
		bCanShowNext = false;
		ani.Play("TipsOne");
		GameLogic.Hold.Sound.PlayUI(1000007);
	}

	private void Update()
	{
		time += Updater.delta;
		if (time >= 3f && !bPlayMiss)
		{
			bPlayMiss = true;
			ani.Play("TipsOne_Miss");
		}
		if (bPlayMiss)
		{
			if (time >= 3.2f && !bCanShowNext)
			{
				bCanShowNext = true;
				CInstance<TipsManager>.Instance.CanShowNext();
			}
			if (time >= 3.5f)
			{
				bPlayMiss = false;
				CInstance<TipsManager>.Instance.Cache(base.gameObject);
			}
		}
	}
}
