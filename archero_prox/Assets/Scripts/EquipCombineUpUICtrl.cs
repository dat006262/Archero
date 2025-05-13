




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EquipCombineUpUICtrl : MediatorCtrlBase
{
	public Text Text_Name;

	public Text Text_Quality;

	public RectTransform iconparent;

	public GameObject successparent;

	public GameObject attributeparent;

	public Text Text_Success;

	public Transform beforeparent;

	public Transform afterparent;

	public GameObject effect_thunder;

	public GameObject effect_rotate;

	public GameObject effect_bomb;

	public Transform attparent;

	public GameObject copyitems;

	public GameObject copyatt;

	public TapToCloseCtrl mCloseCtrl;

	private EquipOneCtrl mEquipBefore;

	private EquipOneCtrl mEquipAfter;

	private LocalUnityObjctPool mPool;

	private EquipCombineUpProxy.Transfer mTransfer;

	private List<EquipCombineAttCtrl> mAttList = new List<EquipCombineAttCtrl>();

	private AnimationCurve curve_move;

	private AnimationCurve curve_sin;

	protected override void OnInit()
	{
		curve_move = LocalModelManager.Instance.Curve_curve.GetCurve(200001);
		curve_sin = LocalModelManager.Instance.Curve_curve.GetSin();
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<EquipCombineAttCtrl>(copyatt);
		EquipOneCtrl equip = CInstance<UIResourceCreator>.Instance.GetEquip();
		equip.SetButtonEnable(value: false);
		mPool.CreateCache<EquipOneCtrl>(equip.gameObject);
		equip.gameObject.SetActive(value: false);
		copyitems.SetActive(value: false);
		mCloseCtrl.OnClose = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_EquipCombineUp);
		};
		if (mEquipBefore == null)
		{
			mEquipBefore = CInstance<UIResourceCreator>.Instance.GetEquip(beforeparent);
		}
		if (mEquipAfter == null)
		{
			mEquipAfter = CInstance<UIResourceCreator>.Instance.GetEquip(afterparent);
		}
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("EquipCombineUpProxy");
		if (proxy == null || proxy.Data == null)
		{
			SdkManager.Bugly_Report("EquipCombineUpUICtrl", Utils.FormatString("Proxy is null."));
			mCloseCtrl.OnClose();
		}
		else
		{
			mTransfer = (proxy.Data as EquipCombineUpProxy.Transfer);
			InitUI();
		}
	}

	private void InitUI()
	{
		mCloseCtrl.Show(value: false);
		effect_bomb.SetActive(value: false);
		effect_rotate.SetActive(value: false);
		effect_thunder.SetActive(value: true);
		effect_thunder.transform.localScale = Vector3.one;
		mAttList.Clear();
		mPool.Collect<EquipOneCtrl>();
		mPool.Collect<EquipCombineAttCtrl>();
		Text_Name.text = mTransfer.equip.NameOnlyString;
		Text_Name.color = mTransfer.equip.qualityColor;
		Text_Quality.text = mTransfer.equip.QualityString;
		Text_Quality.color = mTransfer.equip.qualityColor;
		UnityEngine.Debug.Log("@LOG combine up success " + mTransfer.equip.EquipID + " quality " + mTransfer.equip.Quality + " color " + mTransfer.equip.qualityColor);
		LocalSave.EquipOne equipOne = new LocalSave.EquipOne();
		equipOne.EquipID = mTransfer.equip.EquipID - 1;
		equipOne.Level = mTransfer.equip.Level;
		successparent.SetActive(value: false);
		attributeparent.SetActive(value: false);
		mEquipBefore.Init(equipOne);
		mEquipAfter.Init(mTransfer.equip);
		int num = 1;
		float num2 = 0f;
		EquipCombineAttCtrl equipCombineAttCtrl = mPool.DeQueue<EquipCombineAttCtrl>();
		RectTransform rectTransform = equipCombineAttCtrl.transform as RectTransform;
		rectTransform.SetParentNormal(attparent);
		equipCombineAttCtrl.UpdateMaxLevel(equipOne, mTransfer.equip);
		rectTransform.anchoredPosition = new Vector2(0f, num2);
		num2 -= equipCombineAttCtrl.GetHeight();
		mAttList.Add(equipCombineAttCtrl);
		equipCombineAttCtrl.gameObject.SetActive(value: false);
		num += equipOne.data.Attributes.Length;
		if (mTransfer.equip.data.AdditionSkills.Length > equipOne.data.AdditionSkills.Length)
		{
			num++;
		}
		for (int i = 0; i < num - 1; i++)
		{
			EquipCombineAttCtrl equipCombineAttCtrl2 = mPool.DeQueue<EquipCombineAttCtrl>();
			RectTransform rectTransform2 = equipCombineAttCtrl2.transform as RectTransform;
			rectTransform2.SetParentNormal(attparent);
			equipCombineAttCtrl2.UpdateUI(equipOne, mTransfer.equip, i);
			rectTransform2.anchoredPosition = new Vector2(0f, num2);
			num2 -= equipCombineAttCtrl2.GetHeight();
			mAttList.Add(equipCombineAttCtrl2);
			equipCombineAttCtrl2.gameObject.SetActive(value: false);
		}
		iconparent.anchoredPosition = new Vector2(0f, -380f);
		Text_Name.enabled = false;
		Text_Quality.enabled = false;
		EquipOneCtrl left = mPool.DeQueue<EquipOneCtrl>();
		EquipOneCtrl right = mPool.DeQueue<EquipOneCtrl>();
		EquipOneCtrl three = null;
		left.Init(equipOne);
		right.Init(equipOne);
		left.ShowLevel(value: false);
		right.ShowLevel(value: false);
		left.transform.SetParentNormal(iconparent);
		right.transform.SetParentNormal(iconparent);
		update_canvas(left.gameObject, add: true);
		update_canvas(right.gameObject, add: true);
		left.transform.localPosition = new Vector3(-200f, 0f);
		right.transform.localPosition = new Vector3(200f, 0f);
		left.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		right.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		if (equipOne.data.BreakNeed == 3)
		{
			three = mPool.DeQueue<EquipOneCtrl>();
			three.Init(equipOne);
			three.ShowLevel(value: false);
			three.transform.SetParentNormal(iconparent);
			update_canvas(three.gameObject, add: true);
		}
		DOTween.Sequence().AppendCallback(delegate
		{
			left.transform.DORotate(new Vector3(0f, 0f, 5f), 0.1f).SetEase(curve_sin).SetLoops(10);
			right.transform.DORotate(new Vector3(0f, 0f, -5f), 0.1f).SetEase(curve_sin).SetLoops(10);
			if (three != null)
			{
				three.transform.DORotate(new Vector3(0f, 0f, -5f), 0.1f).SetEase(curve_sin).SetLoops(10);
			}
		}).AppendInterval(1f)
			.AppendCallback(delegate
			{
				effect_thunder.transform.DOScaleX(0f, 0.7f).SetEase(curve_move);
				left.transform.DOLocalMoveX(0f, 0.7f).SetEase(curve_move);
				right.transform.DOLocalMoveX(0f, 0.7f).SetEase(curve_move).OnComplete(delegate
				{
					left.gameObject.SetActive(value: false);
					right.gameObject.SetActive(value: false);
					if (three != null)
					{
						three.gameObject.SetActive(value: false);
					}
					EquipOneCtrl equipOneCtrl = mPool.DeQueue<EquipOneCtrl>();
					update_canvas(equipOneCtrl.gameObject, add: false);
					equipOneCtrl.transform.SetParentNormal(iconparent);
					equipOneCtrl.transform.SetAsFirstSibling();
					effect_rotate.SetActive(value: true);
					effect_bomb.SetActive(value: true);
					effect_rotate.SetParentNormal(equipOneCtrl.transform);
					effect_rotate.transform.SetAsFirstSibling();
					equipOneCtrl.Init(mTransfer.equip);
					DOTween.Sequence().AppendInterval(0.7f).Append(iconparent.DOLocalMoveY(-120f, 0.6f).SetEase(Ease.Linear).OnComplete(delegate
					{
						successparent.SetActive(value: true);
						Text_Name.enabled = true;
						Text_Quality.enabled = true;
						DOTween.Sequence().AppendInterval(0.3f).AppendCallback(delegate
						{
							attributeparent.SetActive(value: true);
							Sequence s = DOTween.Sequence();
							int j = 0;
							for (int count = mAttList.Count; j < count; j++)
							{
								int index = j;
								s.AppendCallback(delegate
								{
									EquipCombineAttCtrl equipCombineAttCtrl3 = mAttList[index];
									equipCombineAttCtrl3.gameObject.SetActive(value: true);
									equipCombineAttCtrl3.transform.localScale = Vector3.one * 0.3f;
									equipCombineAttCtrl3.transform.DOScale(1f, 0.3f);
								});
								s.AppendInterval(0.3f);
							}
							s.AppendCallback(delegate
							{
								List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
								for (int k = 0; k < mTransfer.mats.Count; k++)
								{
									LocalSave.Instance.GetEquipByUniqueID(mTransfer.mats[k])?.CombineReturn(list);
									LocalSave.Instance.Equip_Remove(mTransfer.mats[k]);
								}
								WindowUI.ShowRewardSimple(list);
								LocalSave.Instance.AddProps(list);
								mCloseCtrl.Show(value: true);
							});
						});
					}));
				});
			});
	}

	private void update_canvas(GameObject o, bool add)
	{
		Canvas component = o.GetComponent<Canvas>();
		if (component == null && add)
		{
			component = o.AddComponent<Canvas>();
			component.overrideSorting = true;
			component.sortingLayerName = "UI";
			component.sortingOrder = 233;
		}
		else if (!add && component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
	}

	protected override void OnClose()
	{
		if (mTransfer.onClose != null)
		{
			mTransfer.onClose();
		}
		effect_rotate.SetParentNormal(iconparent);
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
		Text_Success.text = GameLogic.Hold.Language.GetLanguageByTID("equip_combine_success");
	}
}
