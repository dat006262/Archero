




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class RewardSimpleUICtrl : MediatorCtrlBase
{
	public RectTransform child;

	public Text Text_Title;

	public RectTransform itemparent;

	public TapToCloseCtrl mTapCloseCtrl;

	public RectTransform itembg;

	private const int LineCount = 4;

	private const float TitleHeight = 50f;

	private const float OneWidth = 140f;

	private const float OneHeight = 140f;

	private const float playTime = 0.03f;

	private RewardSimpleProxy.Transfer mTransfer;

	private SequencePool mSeqPool = new SequencePool();

	private LocalUnityObjctPool mPool;

	protected override void OnInit()
	{
		GameObject gameObject = CInstance<UIResourceCreator>.Instance.GetPropOneEquip(base.transform).gameObject;
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<PropOneEquip>(gameObject);
		gameObject.SetActive(value: false);
		mTapCloseCtrl.OnClose = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_RewardSimple);
		};
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("RewardSimpleProxy");
		if (proxy != null && proxy.Data != null)
		{
			mTransfer = (proxy.Data as RewardSimpleProxy.Transfer);
			if (mTransfer != null)
			{
				InitUI();
			}
		}
	}

	private void InitUI()
	{
		mPool.Collect<PropOneEquip>();
		mTapCloseCtrl.Show(value: false);
		int count = mTransfer.list.Count;
		int currentlinecount = 4;
		int num = MathDxx.CeilBig((float)count / 4f);
		child.anchoredPosition = new Vector2(0f, 140f * (float)(num - 1) / 2f);
		RectTransform rectTransform = itembg;
		Vector2 sizeDelta = itembg.sizeDelta;
		rectTransform.sizeDelta = new Vector2(sizeDelta.x, (float)num * 140f + 50f);
		if (count < 4)
		{
			currentlinecount = count;
		}
		Sequence s = mSeqPool.Get();
		for (int i = 0; i < count; i++)
		{
			int index = i;
			s.AppendCallback(delegate
			{
				Drop_DropModel.DropData data = mTransfer.list[index];
				PropOneEquip propOneEquip = mPool.DeQueue<PropOneEquip>();
				RectTransform rectTransform2 = propOneEquip.transform as RectTransform;
				rectTransform2.SetParentNormal(itemparent);
				propOneEquip.InitProp(data);
				float x = (float)(index % currentlinecount) * 140f - 140f * (float)(currentlinecount - 1) / 2f;
				float y = (float)(index / currentlinecount) * -140f;
				rectTransform2.anchoredPosition = new Vector2(x, y);
				rectTransform2.localScale = Vector3.one * 0.3f;
				rectTransform2.DOScale(Vector3.one, 0.2f);
			});
			s.AppendInterval(0.03f);
		}
		s.AppendCallback(delegate
		{
			mTapCloseCtrl.Show(value: true);
		});
	}

	protected override void OnClose()
	{
		mSeqPool.Clear();
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("rewardsimple_title");
	}
}
