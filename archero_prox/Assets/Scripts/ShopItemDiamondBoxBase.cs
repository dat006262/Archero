




using Dxx.Util;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDiamondBoxBase : MonoBehaviour
{
	public Text Text_Content;

	public Text Text_Title;

	public Text Text_BoxContent;

	public Image Image_BG;

	public ButtonCtrl Button_Get;

	public GoldTextCtrl mGoldCtrl;

	public Text Text_Free;

	public RedNodeCtrl mRedCtrl;

	public GameObject FreeParent;

	public GameObject NotFreeParent;

	public CountDownCtrl mCountDownCtrl;

	public GameObject extraparent;

	public Text Text_Extra;

	protected bool bFreeShow = true;

	protected long mStartTime;

	protected int PerTime;

	protected long currenttime;

	protected int count;

	protected long last;

	protected float Text_FreeX;

	protected BoxOpenSingleProxy.Transfer mTransfer = new BoxOpenSingleProxy.Transfer();

	protected LocalSave.TimeBoxType mBoxType = LocalSave.TimeBoxType.BoxChoose_DiamondNormal;

	protected int mIndex;

	private void Awake()
	{
		Button_Get.SetDepondNet(value: true);
		Button_Get.onClick = ClickButton;
		Vector2 anchoredPosition = Text_Free.rectTransform.anchoredPosition;
		Text_FreeX = anchoredPosition.x;
		OnAwake();
	}

	protected virtual void OnAwake()
	{
	}

	public void Init(int index)
	{
		mIndex = index;
		mRedCtrl.SetType(RedNodeType.eRedCount);
		OnInit();
	}

	protected virtual void OnInit()
	{
	}

	protected void set_red(int count)
	{
		mRedCtrl.Value = count;
	}

	public void update_red()
	{
		set_red(LocalSave.Instance.GetDiamondBoxFreeCount(mBoxType));
	}

	protected void FreeShow(bool value)
	{
		if (!value)
		{
			int diamondExtraCount = LocalSave.Instance.GetDiamondExtraCount(mBoxType);
			bool flag = diamondExtraCount > 0;
			if ((bool)extraparent)
			{
				extraparent.SetActive(flag);
			}
			if ((bool)mGoldCtrl)
			{
				mGoldCtrl.gameObject.SetActive(!flag);
			}
			if ((bool)Text_Content)
			{
				Text_Content.enabled = !flag;
			}
			if (flag && (bool)Text_Extra)
			{
				Text_Extra.text = Utils.FormatString("{0}/1", diamondExtraCount);
			}
		}
		if (bFreeShow != value)
		{
			bFreeShow = value;
			if (FreeParent != null)
			{
				FreeParent.SetActive(value);
			}
			if (NotFreeParent != null)
			{
				NotFreeParent.SetActive(!value);
			}
		}
	}

	protected void CountDownShow(bool value)
	{
		if ((bool)mCountDownCtrl)
		{
			mCountDownCtrl.Show(value);
		}
	}

	private void Update()
	{
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
			mCountDownCtrl.Refresh(last, 1f - (float)last / (float)PerTime);
			mCountDownCtrl.Text_Time.text = GameLogic.Hold.Language.GetLanguageByTID("diamondbox1_freetime", mCountDownCtrl.GetTimeString());
		}
	}

	protected void UpdateBox()
	{
        Debug.Log("@LOG UpdateBox");
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
		update_red();
	}

	public void Deinit()
	{
		OnDeinit();
	}

	protected virtual void OnDeinit()
	{
	}

	public void LanguageChange()
	{
		OnLanguageChange();
	}

	protected virtual void OnLanguageChange()
	{
	}

	public void ClickButton()
	{
		OnClickButton();
	}

	protected bool CheckCanOpen(int type, int price)
	{
		switch (type)
		{
		case 1:
			if (LocalSave.Instance.GetGold() >= price)
			{
				return true;
			}
			CInstance<TipsUIManager>.Instance.Show(ETips.Tips_GoldNotEnough);
			Facade.Instance.SendNotification("MainUI_GotoShop", "ShopOneGold");
			return false;
		case 2:
			if (LocalSave.Instance.GetDiamond() >= price)
			{
				return true;
			}
			CInstance<TipsUIManager>.Instance.Show(ETips.Tips_DiamondNotEnough);
			Facade.Instance.SendNotification("MainUI_GotoShop", "ShopOneDiamond");
			break;
		}
		return false;
	}

	protected virtual void OnClickButton()
	{
	}

	public void UpdateNet()
	{
	}
}
