




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharUICtrl : MediatorCtrlBase
{
	public enum UIState
	{
		eNormal,
		eWear,
		eWearing,
		eEmptyWearing
	}

	public GameObject window;

	[Tooltip("标题")]
	public UILineCtrl mLineCtrl;

	public Text Text_MyCollections;

	public Text Text_Attribute;

	public RectTransform mCollectionsParent;

	public ButtonCtrl Button_Close;

	public List<EquipBGCtrl> mEquipBGList = new List<EquipBGCtrl>();

	public ScrollRectBase mScrollRectBase;

	public MainUIScrollRectInsideCtrl mInsideCtrl;

	public RectTransform board;

	public RectTransform bagParent;

	public GameObject copyitems;

	public CharUIHeroCtrl mHeroCtrl;

	public ButtonCtrl Button_Combine;

	public Text Text_Combine;

	public CharSortCtrl mSortCtrl;

	public UILineCtrl mMaterialLineCtrl;

	public ButtonCtrl Button_Light;

	public CharEquipChooseCtrl mChooseCtrl;

	[Header("穿戴时的装备显示位置")]
	public Transform wearctrlpos;

	public RedNodeCtrl mCombineRedCtrl;

	private GameObject _equipitem;

	private const int ColumnCount = 5;

	private const int EquipWidth = 140;

	private const int EquipHeight = 140;

	private const float BottomHeight = 250f;

	private float AllHeight;

	private List<EquipOneCtrl> mEquipItemList = new List<EquipOneCtrl>();

	private MutiCachePool<EquipOneCtrl> mCachePool = new MutiCachePool<EquipOneCtrl>();

	private Sequence seq;

	private float scrollendpos;

	private Vector2 collisionpos;

	private Vector2 bagparentpos;

	private EquipOneCtrl mClickEquip;

	private bool bGuide1;

	private EquipOneCtrl _WearCtrl;

	private float fringeHeight;

	private bool bOpened;

	private UIState state;

	private float lastframey;

	private GameObject equipitem
	{
		get
		{
			if (_equipitem == null)
			{
				_equipitem = CInstance<UIResourceCreator>.Instance.GetEquip(copyitems.transform).gameObject;
			}
			return _equipitem;
		}
	}

	private EquipOneCtrl mWearCtrl
	{
		get
		{
			if (_WearCtrl == null)
			{
				_WearCtrl = CInstance<UIResourceCreator>.Instance.GetEquip(bagParent.parent);
				_WearCtrl.SetButtonEnable(value: false);
				_WearCtrl.ShowAniEnable(value: false);
				_WearCtrl.transform.localPosition = new Vector3(10000f, 0f);
			}
			return _WearCtrl;
		}
	}

	protected override void OnInit()
	{
		collisionpos = mCollectionsParent.anchoredPosition;
		bagparentpos = bagParent.anchoredPosition;
		mScrollRectBase.bUseScrollEvent = false;
		window.SetActive(value: false);
		fringeHeight = PlatformHelper.GetFringeHeight();
		RectTransform rectTransform = mScrollRectBase.transform as RectTransform;
		rectTransform.anchoredPosition = new Vector3(0f, fringeHeight, 0f);
		RectTransform rectTransform2 = rectTransform;
		Vector2 sizeDelta = rectTransform.sizeDelta;
		float x = sizeDelta.x;
		Vector2 sizeDelta2 = rectTransform.sizeDelta;
		rectTransform2.sizeDelta = new Vector2(x, sizeDelta2.y / GameLogic.WidthScaleAll + fringeHeight);
		Vector2 sizeDelta3 = rectTransform.sizeDelta;
		AllHeight = sizeDelta3.y;
		mScrollRectBase.OnClick = OnClickScrollView;
		Button_Light.onClick = OnClickScrollView;
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_Char);
		};
		Button_Combine.onClick = delegate
		{
			Facade.Instance.RegisterProxy(new EquipCombineProxy(new EquipCombineProxy.Transfer
			{
				onClose = delegate
				{
					UpdateEquipsList();
				}
			}));
			WindowUI.ShowWindow(WindowID.WindowID_EquipCombine);
		};
		mScrollRectBase.BeginDrag = OnDragBegin;
		mScrollRectBase.Drag = OnDrag;
		mScrollRectBase.EndDrag = OnDragEnd;
		mCachePool.Init(base.gameObject, equipitem);
		copyitems.SetActive(value: false);
		InitWears();
		InitChooseCtrl();
		mSortCtrl.OnButtonClick = delegate
		{
			UpdateEquipsList();
		};
		mCombineRedCtrl.SetType(RedNodeType.eWarning);
	}

	protected override void OnSetArgs(object o)
	{
		mInsideCtrl.anotherScrollRect = (o as ScrollRectBase);
	}

	protected override void OnOpen()
	{
		bOpened = true;
		mMaterialLineCtrl.gameObject.SetActive(value: false);
		if (!window.activeSelf)
		{
			window.SetActive(value: true);
		}
		LocalSave.Instance.Equip_AddUpdateAction(UpdateAttribute);
		UpdateAttribute();
		ChooseUIShow(show: false);
		UpdateNet();
		ChangeState(UIState.eNormal);
		mClickEquip = null;
		mChooseCtrl.transform.position = new Vector3(9999f, 0f, 0f);
		UpdateEquipsList();
		mScrollRectBase.verticalNormalizedPosition = 1f;
	}

	private void UpdateHero()
	{
		int weaponid = LocalSave.Instance.Equip_GetWeapon();
		mHeroCtrl.InitWeapon(weaponid);
		mHeroCtrl.InitCloth(LocalSave.Instance.Equip_GetCloth());
		mHeroCtrl.InitPet(0, LocalSave.Instance.Equip_GetPet(0));
		mHeroCtrl.InitPet(1, LocalSave.Instance.Equip_GetPet(1));
		mHeroCtrl.Show(value: true);
	}

	private void UpdateEquipsList()
	{
		mCachePool.clear();
		update_combine_rednode();
		LocalSave.Instance.Equip_SetRefresh();
		mEquipItemList.Clear();
		KillSequence();
		seq = DOTween.Sequence();
		List<LocalSave.EquipOne> haveEquips = LocalSave.Instance.GetHaveEquips(havewear: true);
		int i = 0;
		for (int count = mEquipBGList.Count; i < count; i++)
		{
			mEquipBGList[i].Init(null);
		}
		int j = 0;
		for (int count2 = haveEquips.Count; j < count2; j++)
		{
			LocalSave.EquipOne equipOne = haveEquips[j];
            Debug.Log("@LOG CharUICtrl.UpdateEquipsList EquipID:" + equipOne.EquipID);
			if (equipOne.WearIndex >= 0)
			{
				mEquipBGList[equipOne.WearIndex].Init(equipOne);
			}
		}
		List<LocalSave.EquipOne> list = mSortCtrl.GetList(EquipType.eEquip);
		seq.AppendInterval(0.2f);
		float num = 13f;
		float startx = -280f;
		float num2 = 0f - num - 70f;
		if (list.Count > 0)
		{
			AddBags(list, num, startx, num2);
			float height = GetHeight(list.Count, 140);
			num2 -= height;
			num += height;
		}
		List<LocalSave.EquipOne> list2 = mSortCtrl.GetList(EquipType.eMaterial);
		if (list2.Count > 0)
		{
			mMaterialLineCtrl.gameObject.SetActive(value: true);
			mMaterialLineCtrl.SetY(num2 + 70f);
			num2 -= 50f;
			AddBags(list2, num, startx, num2);
			float height2 = GetHeight(list2.Count, 140);
			num2 -= height2;
			num += height2;
		}
		mCachePool.hold(list.Count + list2.Count);
		RectTransform content = mScrollRectBase.content;
		Vector2 sizeDelta = mScrollRectBase.content.sizeDelta;
		float x = sizeDelta.x;
		float num3 = num;
		Vector2 anchoredPosition = board.anchoredPosition;
		float num4 = num3 + (0f - anchoredPosition.y);
		Vector2 sizeDelta2 = board.sizeDelta;
		content.sizeDelta = new Vector2(x, num4 + sizeDelta2.y + 250f - fringeHeight);
		UpdateHero();
	}

	private void AddBags(List<LocalSave.EquipOne> list, float height, float startx, float starty)
	{
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			int index = i;
			seq.AppendCallback(delegate
			{
				EquipOneCtrl equipOneCtrl = mCachePool.get();
				RectTransform rectTransform = equipOneCtrl.transform as RectTransform;
				rectTransform.SetParentNormal(bagParent);
				rectTransform.anchoredPosition = new Vector2(startx + (float)(index % 5 * 140), starty - (float)(index / 5 * 140));
				equipOneCtrl.Init(list[index]);
				equipOneCtrl.UpdateRedShow();
				equipOneCtrl.OnClickEvent = UpdateChooseEquip;
				mEquipItemList.Add(equipOneCtrl);
				if (!bGuide1 && !equipOneCtrl.equipdata.Overlying)
				{
					GameLogic.Hold.Guide.mEquip.GoNext(1, rectTransform);
				}
			});
			seq.AppendInterval(0.02f);
		}
	}

	private float GetHeight(int count, int perheight)
	{
		if (count % 5 == 0)
		{
			return count / 5 * perheight;
		}
		return count / 5 * perheight + perheight;
	}

	private void InitWears()
	{
		int i = 0;
		for (int count = mEquipBGList.Count; i < count; i++)
		{
			mEquipBGList[i].SetClick(OnClickWearAdd);
		}
	}

	private void UpdateAttribute()
	{
		SelfAttributeData selfAttributeShow = GameLogic.SelfAttributeShow;
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("Attr_Attack");
		string languageByTID2 = GameLogic.Hold.Language.GetLanguageByTID("Attr_HPMax");
		if ((bool)Text_Attribute)
		{
			Text_Attribute.text = Utils.FormatString("{0} {1}  {2} {3}", languageByTID, selfAttributeShow.attribute.AttackValue.Value, languageByTID2, selfAttributeShow.attribute.HPValue.Value);
		}
	}

	private void SetScrollEnable(bool value)
	{
		mScrollRectBase.UseDrag = value;
		mInsideCtrl.anotherScrollRect.UseDrag = value;
		mInsideCtrl.enabled = value;
	}

	private void InitChooseCtrl()
	{
	}

	private void ChooseUIShow(bool show)
	{
		if ((bool)mChooseCtrl)
		{
			mChooseCtrl.Show(show);
		}
	}

	private void OnClickWearAdd(int index)
	{
		if (state == UIState.eWear)
		{
			GameLogic.Hold.Sound.PlayUI(1000007);
			ChangeState(UIState.eWearing);
			mEquipBGList[index].Unwear(wearctrlpos.position);
			DOTween.Sequence().Append(mWearCtrl.transform.DOMove(mEquipBGList[index].transform.position, 0.3f)).Join(mWearCtrl.transform.DOScale(Vector3.one * 1.1f, 0.3f))
				.OnComplete(delegate
				{
					mEquipBGList[index].Init(mWearCtrl.equipdata);
					LocalSave.Instance.EquipWear(mWearCtrl.equipdata, index);
					ChangeState(UIState.eNormal, force: true);
					UpdateEquipsList();
				});
			MissAdd();
			StopWearAction();
		}
		else if (mEquipBGList[index].GetIsWear() && state == UIState.eNormal)
		{
			RectTransform rectTransform = mChooseCtrl.transform as RectTransform;
			Vector3 position = mEquipBGList[index].transform.position;
			UpdateChooseEquip(mEquipBGList[index].ctrl);
			mChooseCtrl.SetIndex(index);
		}
	}

	public void UpdateChooseEquip(EquipOneCtrl one)
	{
		GameLogic.Hold.Guide.mEquip.CurrentOver(1);
		mChooseCtrl.Init(one.equipdata);
		mClickEquip = one;
		OnClickInfo();
	}

	private void UpdateChooseCardScrollView()
	{
		Vector3 position = mChooseCtrl.transform.position;
		float num = position.y / (float)GameLogic.Height * (float)GameLogic.DesignHeight;
		float num2 = 350f;
		float num3 = 1080f;
		float num4 = 0f;
		if (num < num2)
		{
			num4 = num2 - num;
		}
		else if (num > num3)
		{
			num4 = num3 - num;
		}
		if (num4 != 0f)
		{
			Vector2 sizeDelta = mScrollRectBase.content.sizeDelta;
			float num5 = sizeDelta.y - AllHeight;
			scrollendpos = mScrollRectBase.verticalNormalizedPosition - num4 / (GameLogic.WidthScale / GameLogic.HeightScale) / num5;
			scrollendpos = MathDxx.Clamp01(scrollendpos);
			lastframey = num;
			Updater.AddUpdateUI(OnUpdate);
		}
	}

	private void OnUpdate(float delta)
	{
		if (!GetScrolling())
		{
			mScrollRectBase.verticalNormalizedPosition = Mathf.Lerp(mScrollRectBase.verticalNormalizedPosition, scrollendpos, 0.2f);
			Vector3 position = mChooseCtrl.transform.position;
			if (Mathf.Abs(position.y - lastframey) < 0.2f)
			{
				mScrollRectBase.verticalNormalizedPosition = scrollendpos;
				Updater.RemoveUpdateUI(OnUpdate);
			}
			Vector3 position2 = mChooseCtrl.transform.position;
			lastframey = position2.y;
		}
	}

	private void OnClickInfo()
	{
		if (!GetScrolling())
		{
			ChooseUIShow(show: false);
			EquipInfoModuleProxy.Transfer transfer = new EquipInfoModuleProxy.Transfer();
			transfer.one = mChooseCtrl.equipdata;
			transfer.updatecallback = UpgradeCallBack;
			transfer.wearcallback = WearCallBack;
			EquipInfoModuleProxy proxy = new EquipInfoModuleProxy(transfer);
			Facade.Instance.RegisterProxy(proxy);
			WindowUI.ShowWindow(WindowID.WindowID_EquipInfo);
		}
	}

	private void UpgradeCallBack()
	{
		if (mClickEquip != null)
		{
			mClickEquip.Init();
			if (mClickEquip.equipdata.IsWear)
			{
				mClickEquip.SetButtonEnable(value: false);
			}
		}
		UpdateEquipsList();
	}

	private void WearCallBack()
	{
		if (mClickEquip != null)
		{
			if (mClickEquip.equipdata.IsWear)
			{
				OnClickUnwear();
			}
			else
			{
				OnClickWear();
			}
		}
	}

	private void OnClickLevel()
	{
		OnClickInfo();
	}

	private void OnClickWear()
	{
		mScrollRectBase.enabled = true;
		LocalSave.Instance.Equip_GetCanWearIndex(mChooseCtrl.equipdata, out int emptyindex);
		if (emptyindex >= 0)
		{
			GameLogic.Hold.Sound.PlayUI(1000007);
			mEquipBGList[emptyindex].Unwear(wearctrlpos.position);
			ChangeState(UIState.eEmptyWearing);
			mWearCtrl.gameObject.SetActive(value: true);
			mWearCtrl.Init(mChooseCtrl.equipdata);
			mWearCtrl.transform.localScale = Vector3.one;
			mWearCtrl.transform.position = mClickEquip.transform.position;
			DOTween.Sequence().Append(mWearCtrl.transform.DOMove(mEquipBGList[emptyindex].transform.position, 0.3f)).Join(mWearCtrl.transform.transform.DOScale(Vector3.one * 1.1f, 0.3f))
				.OnComplete(delegate
				{
					mEquipBGList[emptyindex].Init(mChooseCtrl.equipdata);
					LocalSave.Instance.EquipWear(mChooseCtrl.equipdata, emptyindex);
					ChangeState(UIState.eNormal, force: true);
					UpdateEquipsList();
				});
			MissAdd();
			ChooseUIShow(show: false);
		}
		else
		{
			UpdateWear(mChooseCtrl.equipdata);
			ChooseUIShow(show: false);
		}
	}

	private void OnClickUnwear()
	{
		int index = mChooseCtrl.GetIndex();
		mEquipBGList[index].Unwear(wearctrlpos.position, delegate(LocalSave.EquipOne data)
		{
			LocalSave.Instance.EquipUnwear(data.UniqueID);
			DOTween.Sequence().Append(ScrollPlayFade(show: false)).AppendCallback(delegate
			{
				UpdateEquipsList();
			})
				.Append(ScrollPlayFade(show: true));
		});
		mEquipBGList[index].MissAdd();
		ChooseUIShow(show: false);
		GameLogic.Hold.Sound.PlayUI(1000007);
	}

	private void ChangeState(UIState state, bool force = false)
	{
		if (this.state != state && (this.state != UIState.eWearing || force) && (this.state != UIState.eEmptyWearing || force))
		{
			this.state = state;
			switch (state)
			{
			case UIState.eNormal:
				ScrollPlayFade(show: true);
				mWearCtrl.gameObject.SetActive(value: false);
				StopWearAction();
				MissAdd();
				break;
			case UIState.eWear:
				ScrollPlayFade(show: false);
				mScrollRectBase.verticalNormalizedPosition = 1f;
				DoWearAction();
				break;
			case UIState.eEmptyWearing:
				mScrollRectBase.verticalNormalizedPosition = 1f;
				DOTween.Sequence().Append(ScrollPlayFade(show: false)).AppendCallback(delegate
				{
				})
					.Append(ScrollPlayFade(show: true))
					.AppendCallback(delegate
					{
					});
				break;
			}
		}
	}

	private void UpdateWear(LocalSave.EquipOne equipdata)
	{
		mWearCtrl.gameObject.SetActive(value: true);
		mWearCtrl.Init(equipdata);
		mWearCtrl.transform.position = mClickEquip.transform.position;
		mWearCtrl.transform.localScale = Vector3.one;
		ChangeState(UIState.eWear);
		mWearCtrl.transform.DOMove(wearctrlpos.position, 0.5f);
	}

	private void DoWearAction()
	{
		MissAdd();
		int i = 0;
		for (int count = mEquipBGList.Count; i < count; i++)
		{
			mEquipBGList[i].SetButtonEnable(value: false);
		}
		List<int> list = LocalSave.Instance.Equip_GetCanWears(mChooseCtrl.equipdata.data.Position);
		int j = 0;
		for (int count2 = list.Count; j < count2; j++)
		{
			mEquipBGList[list[j]].DoWear();
			mEquipBGList[list[j]].SetButtonEnable(value: true);
		}
	}

	private void StopWearAction()
	{
		int i = 0;
		for (int count = mEquipBGList.Count; i < count; i++)
		{
			mEquipBGList[i].WearOver();
		}
		int j = 0;
		for (int count2 = mEquipBGList.Count; j < count2; j++)
		{
			mEquipBGList[j].UpdateButtonEnable();
		}
	}

	private void MissAdd()
	{
		int i = 0;
		for (int count = mEquipBGList.Count; i < count; i++)
		{
			mEquipBGList[i].MissAdd();
		}
	}

	private Sequence ScrollPlayFade(bool show)
	{
		Sequence sequence = DOTween.Sequence();
		if (!show)
		{
			sequence.AppendCallback(delegate
			{
				mCollectionsParent.anchoredPosition = new Vector2(10000f, 0f);
				bagParent.anchoredPosition = new Vector2(10000f, 0f);
			});
		}
		else
		{
			sequence.AppendCallback(delegate
			{
				mCollectionsParent.anchoredPosition = collisionpos;
				bagParent.anchoredPosition = bagparentpos;
			});
		}
		return sequence;
	}

	private void OnDragBegin(PointerEventData eventData)
	{
		ChooseUIShow(show: false);
		Updater.RemoveUpdateUI(OnUpdate);
	}

	private void OnDrag(PointerEventData eventData)
	{
	}

	private void OnDragEnd(PointerEventData eventData)
	{
		ChooseUIShow(show: false);
	}

	private void OnClickScrollView()
	{
		ChooseUIShow(show: false);
		if (state == UIState.eWear)
		{
			ChangeState(UIState.eNormal);
		}
	}

	private bool GetScrolling()
	{
		return mScrollRectBase.velocity != Vector2.zero;
	}

	private void update_combine_rednode()
	{
		int num = LocalSave.Instance.Equip_can_combine_count();
		if (num > 0)
		{
			mCombineRedCtrl.Value = 1;
		}
		else
		{
			mCombineRedCtrl.Value = 0;
		}
	}

	protected override void OnClose()
	{
		mCachePool.clear();
		bOpened = false;
		mHeroCtrl.Show(value: false);
		LocalSave.Instance.Equip_RemoveUpdateAction(UpdateAttribute);
		KillSequence();
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	public override void OnHandleNotification(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
		if (name == null)
		{
			return;
		}
		if (!(name == "PUB_NETCONNECT_UPDATE"))
		{
			if (name == "MainUI_EquipRedCountUpdate")
			{
			}
		}
		else
		{
			UpdateNet();
		}
	}

	private void UpdateNet()
	{
		mChooseCtrl.UpdateNet();
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnLanguageChange()
	{
		mLineCtrl.SetText(GameLogic.Hold.Language.GetLanguageByTID("EquipUI_MyEquip"));
		Text_Combine.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Combine");
		Text_MyCollections.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_MyCollections");
		mMaterialLineCtrl.SetText(GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Materials"));
		mChooseCtrl.OnLanguageChange();
		mSortCtrl.OnLanguageChange();
	}
}
