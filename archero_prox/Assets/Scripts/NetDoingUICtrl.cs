




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.UI;

public class NetDoingUICtrl : MediatorCtrlBase
{
	public GameObject window;

	public Transform RotatingParent;

	public Image Image_Rotate;

	public Text Text_Count;

	public Text Text_Code;

	public Text Text_Loading;

	public CanvasGroup mCanvasGroup;

	private Sequence seq_load;

	private Sequence seq_delay;

	private int loadingindex;

	private RectTransform t;

	private string m_sCode = string.Empty;

	private NetDoingProxy.Transfer mTransfer;

	protected override void OnInit()
	{
		t = (base.transform as RectTransform);
		t.sizeDelta = GameLogic.ScreenSize;
	}

	private void OnUpdate(float delta)
	{
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("NetDoingProxy");
		mTransfer = (proxy.Data as NetDoingProxy.Transfer);
		mCanvasGroup.alpha = 0f;
		seq_delay = DOTween.Sequence().AppendInterval(0.6f).Append(mCanvasGroup.DOFade(2f / 3f, 1f))
			.SetUpdate(isIndependentUpdate: true);
		SetLoading(0);
		seq_load = DOTween.Sequence().AppendInterval(0.5f).AppendCallback(delegate
		{
			loadingindex++;
			loadingindex %= 4;
			SetLoading(loadingindex);
		})
			.SetLoops(-1)
			.SetUpdate(isIndependentUpdate: true);
		Updater.AddUpdate("netdoing", OnUpdate, IgnoreTimeScale: true);
	}

	private void SetLoading(int index)
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(mTransfer.type.ToString());
		string text = string.Empty;
		for (int i = 0; i < index; i++)
		{
			text += ".";
		}
		Text_Loading.text = Utils.FormatString("{0}{1}", languageByTID, text);
	}

	protected override void OnClose()
	{
		if (seq_load != null)
		{
			seq_load.Kill();
			seq_load = null;
		}
		if (seq_delay != null)
		{
			seq_delay.Kill();
			seq_delay = null;
		}
		Updater.RemoveUpdate("netdoing", OnUpdate);
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
	}
}
