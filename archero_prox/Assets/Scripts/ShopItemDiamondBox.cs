




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDiamondBox : MonoBehaviour
{
	public Text Text_Title;

	public Text Text_Free;

	public Image Image_BG;

	public ButtonCtrl Button_Get;

	public Image Image_Icon;

	public GoldTextCtrl mGoldCtrl;

	public CountDownCtrl mCountDownCtrl;

	public Action<int, ShopItemDiamondBox> OnClickButton;

	private Shop_Shop shopdata;

	private int mIndex;

	private bool bFreeShow = true;

	private LocalSave.TimeBoxType mBoxType = LocalSave.TimeBoxType.BoxChoose_DiamondNormal;

	private long mStartTime;

	private int PerTime;

	private long currenttime;

	private int count;

	private long last;

	private void Awake()
	{
		Button_Get.onClick = delegate
		{
			if (OnClickButton != null)
			{
				OnClickButton(mIndex, this);
			}
		};
		PerTime = GameConfig.GetTimeBoxTime(mBoxType);
	}

	public void Init(int index)
	{
		mIndex = index;
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_宝箱{0}", index));
		mGoldCtrl.SetValue(index * 4400 + 600);
		UpdateBox();
	}

	private void FreeShow(bool value)
	{
		if (bFreeShow == value)
		{
			return;
		}
		bFreeShow = value;
		if ((bool)Text_Free)
		{
			Text_Free.gameObject.SetActive(value);
			mGoldCtrl.gameObject.SetActive(!value);
			Image_BG.color = ((!value) ? Color.white : Color.yellow);
			if (value)
			{
				Text_Free.text = GameLogic.Hold.Language.GetLanguageByTID("商店_免费抽取");
			}
		}
	}

	private void CountDownShow(bool value)
	{
		if ((bool)mCountDownCtrl)
		{
			mCountDownCtrl.Show(value);
		}
	}

	public void OnLanguageChange()
	{
		Init(mIndex);
	}

	private void Update()
	{
		if (mCountDownCtrl == null || mIndex != 0)
		{
			return;
		}
		if (LocalSave.Instance.IsTimeBoxMax(mBoxType))
		{
			CountDownShow(value: false);
			FreeShow(value: true);
			return;
		}
		FreeShow(value: false);
		CountDownShow(value: true);
		currenttime = Utils.GetTimeStamp();
		count = (int)((float)(currenttime - mStartTime) / (float)PerTime);
		if (count > 0)
		{
			LocalSave.Instance.Modify_TimeBoxCount(mBoxType, count);
			mStartTime += count * PerTime;
			LocalSave.Instance.SetTimeBoxTime(mBoxType, mStartTime);
			UpdateBox();
		}
		else
		{
			last = PerTime - (currenttime - mStartTime);
			mCountDownCtrl.Refresh(last, 1f - (float)last / ((float)PerTime / 1000f));
		}
	}

	private void UpdateBox()
	{
		if (mIndex == 0)
		{
			mStartTime = LocalSave.Instance.GetTimeBoxTime(mBoxType);
			int timeBoxCount = LocalSave.Instance.GetTimeBoxCount(mBoxType);
			if (timeBoxCount > 0)
			{
				FreeShow(value: true);
				CountDownShow(value: false);
			}
			else
			{
				FreeShow(value: false);
				CountDownShow(value: true);
			}
		}
		else
		{
			FreeShow(value: false);
			CountDownShow(value: false);
		}
	}

	public bool GetCanFree()
	{
		if (mIndex == 0)
		{
			return LocalSave.Instance.GetTimeBoxCount(mBoxType) > 0;
		}
		return false;
	}
}
