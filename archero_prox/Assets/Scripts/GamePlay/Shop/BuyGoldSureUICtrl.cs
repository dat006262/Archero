




using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using TableTool;
using UnityEngine.UI;

public class BuyGoldSureUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public GoldTextCtrl mGoldCtrl;

	public Image Image_Icon;

	public Text Text_Count;

	public ButtonCtrl Button_Sure;

	public ButtonCtrl Button_Refuse;

	public ButtonCtrl Button_Shadow;

	private BuyGoldSureProxy.Transfer mTransfer;

	private Shop_Shop shopdata;

	protected override void OnInit()
	{
		Button_Refuse.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_BuyGoldSure);
		};
		Button_Shadow.onClick = Button_Refuse.onClick;
		Button_Sure.onClick = delegate
		{
			Button_Refuse.onClick();
			if (mTransfer != null && mTransfer.callback != null)
			{
				mTransfer.callback(mTransfer.index, mTransfer.item);
			}
		};
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("BuyGoldSureProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("BuyGoldSureUICtrl", "OnOpen BuyGoldSureProxy is null");
			return;
		}
		if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("BuyGoldSureUICtrl", "OnOpen BuyGoldSureProxy.Data is null");
			return;
		}
		mTransfer = (proxy.Data as BuyGoldSureProxy.Transfer);
		if (mTransfer == null)
		{
			SdkManager.Bugly_Report("BuyGoldSureUICtrl", "OnOpen BuyGoldSureProxy.Data is null");
		}
		else
		{
			InitUI();
		}
	}

	private void InitUI()
	{
		Image_Icon.sprite = SpriteManager.GetMain(Utils.FormatString("ic_coin_{0}", mTransfer.index + 1));
		shopdata = LocalModelManager.Instance.Shop_Shop.GetBeanById(101 + mTransfer.index);
		Text_Count.text = mTransfer.item.GetGold().ToString();
		mGoldCtrl.SetCurrencyType(CurrencyType.Diamond);
		mGoldCtrl.SetValue(shopdata.Price);
	}

	protected override void OnClose()
	{
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	public override void OnLanguageChange()
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_金币{0}", shopdata.ID));
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("buygoldui_title", languageByTID);
	}
}
