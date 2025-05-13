




using Dxx.Util;
using UnityEngine;
using UnityEngine.UI;

public class BoxChooseButtonCtrl : MonoBehaviour
{
	private Text Text_Get;

	private Text Text_FreeGet;

	private Text Text_FreeTime;

	private GoldTextCtrl mGoldCtrl;

	private ButtonCtrl mButton;

	private LocalSave.TimeBoxType chooseType;

	private long localtime;

	private long needtime;

	private int needgold;

	private bool textgetshow = true;

	private bool freetimeshow = true;

	private bool goldshow = true;

	private void Awake()
	{
		Text_Get = base.transform.Find("Button/fg/Text_Get").GetComponent<Text>();
		Text_FreeGet = base.transform.Find("Button/fg/Text_FreeGet").GetComponent<Text>();
		Text_FreeTime = base.transform.Find("Button/Text_FreeTime").GetComponent<Text>();
		mGoldCtrl = base.transform.Find("Button/fg/ResourceText").GetComponent<GoldTextCtrl>();
		mButton = base.transform.Find("Button").GetComponent<ButtonCtrl>();
		mButton.onClick = OnClickButton;
	}

	public void Init(LocalSave.TimeBoxType type)
	{
		chooseType = type;
		if (type == LocalSave.TimeBoxType.BoxChoose_DiamondNormal || type == LocalSave.TimeBoxType.BoxChoose_DiamondLarge)
		{
			Text_Get.text = GameLogic.Hold.Language.GetLanguageByTID("BoxChoose_Name1");
		}
		else
		{
			Text_Get.text = GameLogic.Hold.Language.GetLanguageByTID("BoxChoose_Name10");
		}
		needgold = GameConfig.GetBoxChooseGold(chooseType);
		mGoldCtrl.SetValue(needgold);
		SetTextGetShow(value: false);
		SetFreeTimeShow(value: false);
		SetGoldShow(value: true);
		needtime = GameConfig.GetBoxChooseTime(chooseType);
		UpdateBoxChooseTime();
	}

	private void OnClickButton()
	{
	}

	private void UpdateBoxChooseTime()
	{
		localtime = LocalSave.Instance.GetTimeBoxTime(chooseType);
	}

	private void SetTextGetShow(bool value)
	{
		if (textgetshow != value)
		{
			textgetshow = value;
			if ((bool)Text_Get)
			{
				Text_FreeGet.gameObject.SetActive(value);
			}
		}
	}

	private void SetFreeTimeShow(bool value)
	{
		if (freetimeshow != value)
		{
			freetimeshow = value;
			if ((bool)Text_FreeTime)
			{
				Text_FreeTime.gameObject.SetActive(value);
			}
		}
	}

	private void SetGoldShow(bool value)
	{
		if (goldshow != value)
		{
			goldshow = value;
			if ((bool)mGoldCtrl)
			{
				mGoldCtrl.gameObject.SetActive(value);
			}
		}
	}

	private void Update()
	{
		if (!LocalSave.Instance.IsTimeBoxMax(chooseType))
		{
			long num = needtime - (Utils.GetTimeStamp() - localtime);
			if (num > 0)
			{
				string second3String = Utils.GetSecond3String(num / 1000);
				Text_FreeTime.text = GameLogic.Hold.Language.GetLanguageByTID("BoxChoose_FreeTime", second3String);
				SetTextGetShow(value: false);
				SetFreeTimeShow(value: true);
				SetGoldShow(value: true);
			}
			else
			{
				SetTextGetShow(value: true);
				SetFreeTimeShow(value: false);
				SetGoldShow(value: false);
			}
		}
	}
}
