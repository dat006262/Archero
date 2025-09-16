




using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipCombineUICtrl : MediatorCtrlBase
{
	public RectTransform titletransform;

	public GameObject titlecombine;

	public Text Text_Title;

	public ButtonCtrl Button_Close;

	public Text Text_Guide;

	public GameObject copyitems;

	public GameObject copyone;

	public ScrollRectBase mScrollRect;

	public EquipCombineInfinity mInfinity;

	public RectTransform mScrollChild;

	public EquipCombineParent mCombineParent;

	public ButtonCtrl Button_Combine;

	public Text Text_Combine;

	public GameObject mMaskparent;

	private int leftpadding = 10;

	private int width = 140;

	private int height = 140;

	private int LineCount = 5;

	private int BottomHeight = 200;

	private Vector2 scrollsize;

	private bool bLock;

	private EquipCombineOne mPlayOne;

	private EquipCombineOne mChoose;

	private LocalUnityObjctPool mPool;

	private MutiCachePool<EquipCombineOne> mCachePool = new MutiCachePool<EquipCombineOne>();

	private List<LocalSave.EquipOne> mList;

	private List<EquipCombineOne> mItemList = new List<EquipCombineOne>();

	private SequencePool mSeqPool = new SequencePool();

	private Action onClose;

	protected override void OnInit()
	{
		mCachePool.Init(base.gameObject, copyone);
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<EquipCombineOne>(copyone);
		copyitems.SetActive(value: false);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_EquipCombine);
		};
		Button_Combine.onClick = delegate
		{
            Debug.Log("@LOG EquipCombineUICtrl.Button_Combine.onClick");
			EquipCombineUICtrl equipCombineUICtrl = this;
			SdkManager.send_event_equip_combine("click", 0, string.Empty, string.Empty);
			CEquipCompositeTrans data = new CEquipCompositeTrans();
			data.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
			data.m_arrCompositeInfo = new CEquipmentItem[mChoose.mData.BreakNeed];
			string text = string.Empty;
			bool flag = false;
			bool flag2 = true;
			int num4 = 0;
			for (int i = 0; i < data.m_arrCompositeInfo.Length; i++)
			{
				int index = mCombineParent.GetIndex(i);
				LocalSave.EquipOne equipOne = mList[index];
				CEquipmentItem cEquipmentItem = new CEquipmentItem
				{
					m_nUniqueID = equipOne.UniqueID,
					m_nRowID = equipOne.RowID
				};
				if (num4 == 0)
				{
					num4 = equipOne.EquipID;
				}
				else if (num4 != equipOne.EquipID)
				{
					flag2 = false;
				}
				text = text + equipOne.RowID + ".";
				if (equipOne.RowID == 0)
				{
					flag = true;
				}
				data.m_arrCompositeInfo[i] = cEquipmentItem;
			}
            Debug.Log("@LOG EquipCombineUICtrl.Button_Combine.onClick flag:" + flag);
#if !ENABLE_NET_MANAGER
            flag = false;
#endif
            if (flag)
            {
				CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
				SdkManager.Bugly_Report("EquipCombineUICtrl", Utils.FormatString("rowid=0 : {0}", text));
				SdkManager.send_event_equip_combine("end", 0, "fail", "rowid=0");
			}
			else if (!flag2)
			{
				CInstance<TipsUIManager>.Instance.Show(ETips.Tips_CombineError);
				SdkManager.Bugly_Report("EquipCombineUICtrl", Utils.FormatString("combinerror:{0}", text));
				SdkManager.send_event_equip_combine("end", 0, "fail", "combineerror");
			}
			else
			{
				NetManager.SendInternal(data, SendType.eForceOnce, 3, 10, delegate(NetResponse response)
				{
#if ENABLE_NET_MANAGER
                    if (response.IsSuccess)
#endif
                    {
                        equipCombineUICtrl.mChoose.mData.QualityUp();
						SdkManager.send_event_equipment("GET", equipCombineUICtrl.mChoose.mData.EquipID, 1, equipCombineUICtrl.mChoose.mData.Level, EquipSource.EEquip_Combine, 0);
						SdkManager.send_event_equip_combine("end", equipCombineUICtrl.mChoose.mData.EquipID, "success", string.Empty);
						EquipCombineUpProxy.Transfer transfer = new EquipCombineUpProxy.Transfer
						{
							equip = equipCombineUICtrl.mChoose.mData,
							onClose = equipCombineUICtrl.InitUI
						};
						for (int j = 1; j < data.m_arrCompositeInfo.Length; j++)
						{
							transfer.AddMatUniqueID(data.m_arrCompositeInfo[j].m_nUniqueID);
						}
						Facade.Instance.RegisterProxy(new EquipCombineUpProxy(transfer));
						WindowUI.ShowWindow(WindowID.WindowID_EquipCombineUp);
						equipCombineUICtrl.InitUI();
					}
#if ENABLE_NET_MANAGER
					else if (response.error != null && response.error.m_nStatusCode == 2)
					{
						CInstance<TipsUIManager>.Instance.Show(ETips.Tips_CombineError);
						SdkManager.send_event_equip_combine("end", 0, "fail", "combineerror");
					}
					else
					{
						CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
						SdkManager.send_event_equip_combine("end", 0, "fail", "net_error");
					}
#endif
				});
			}
		};
		mCombineParent.OnCombineDown = OnCombineDown;
		float fringeHeight = PlatformHelper.GetFringeHeight();
		RectTransform rectTransform = titletransform;
		Vector2 anchoredPosition = titletransform.anchoredPosition;
		float x = anchoredPosition.x;
		Vector2 anchoredPosition2 = titletransform.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(x, anchoredPosition2.y + fringeHeight);
		RectTransform rectTransform2 = base.transform as RectTransform;
		RectTransform rectTransform3 = mScrollRect.transform as RectTransform;
		Vector2 sizeDelta = rectTransform2.sizeDelta;
		float y = sizeDelta.y;
		float num = (!(y > 1280f)) ? 0f : (y - 1280f);
		num += fringeHeight;
		RectTransform rectTransform4 = rectTransform3;
		Vector2 sizeDelta2 = rectTransform3.sizeDelta;
		float x2 = sizeDelta2.x;
		Vector2 sizeDelta3 = rectTransform3.sizeDelta;
		rectTransform4.sizeDelta = new Vector2(x2, sizeDelta3.y + num);
		scrollsize = rectTransform3.sizeDelta;
		int num2 = MathDxx.CeilBig(scrollsize.y / (float)height) + 1;
		int num3 = num2 * LineCount;
		mInfinity.copyItem = copyone;
		mInfinity.initDisplayCount = num3;
		mInfinity.Init(num3);
		mInfinity.updatecallback = UpdateChildCallBack;
	}

	protected override void OnOpen()
	{
		SdkManager.send_event_equip_combine("show", 0, string.Empty, string.Empty);
		IProxy proxy = Facade.Instance.RetrieveProxy("EquipCombineProxy");
		if (proxy != null && proxy.Data is EquipCombineProxy.Transfer)
		{
			onClose = (proxy.Data as EquipCombineProxy.Transfer).onClose;
		}
		InitUI();
	}

	private void InitUI()
	{
		mScrollRect.verticalNormalizedPosition = 1f;
		bLock = false;
		mMaskparent.SetActive(value: false);
		show_combine_button(value: false);
		mCombineParent.Show(value: false);
		mList = LocalSave.Instance.GetHaveEquips(havewear: true);
		if (mList.Count > 0)
		{
			for (int num = mList.Count - 1; num >= 0; num--)
			{
				if (!mList[num].QualityCanUp)
				{
					mList.RemoveAt(num);
				}
			}
		}
		mList.Sort(delegate(LocalSave.EquipOne a, LocalSave.EquipOne b)
		{
			bool canCombine = a.CanCombine;
			bool canCombine2 = b.CanCombine;
			if (canCombine && !canCombine2)
			{
				return -1;
			}
			if (!canCombine && canCombine2)
			{
				return 1;
			}
			if (a.Position < b.Position)
			{
				return -1;
			}
			if (a.Position > b.Position)
			{
				return 1;
			}
			if (a.IconBase < b.IconBase)
			{
				return -1;
			}
			if (a.IconBase > b.IconBase)
			{
				return 1;
			}
			if (a.Quality > b.Quality)
			{
				return -1;
			}
			if (a.Quality < b.Quality)
			{
				return 1;
			}
			if (a.Level > b.Level)
			{
				return -1;
			}
			return (a.Level < b.Level) ? 1 : 1;
		});
		update_scroll_height();
		mChoose = null;
		mCachePool.clear();
		mCachePool.hold(mList.Count);
		mPool.Collect<EquipCombineOne>();
		mPlayOne = mPool.DeQueue<EquipCombineOne>();
		mPlayOne.transform.SetParentNormal(mMaskparent);
		mPlayOne.gameObject.SetActive(value: false);
		mItemList.Clear();
		mInfinity.SetItemCount(mList.Count);
		mInfinity.Refresh();
		set_guide_info(0);
	}

	private void UpdateChildCallBack(int index, EquipCombineOne one)
	{
		one.Init(index, mList[index]);
		one.PlayAni(value: false);
		one.OnButtonClick = OnClickOne;
		one.SetChoose(null);
		if (!mItemList.Contains(one))
		{
			mItemList.Add(one);
		}
		set_equip_lock(one, bLock);
	}

	private void set_guide_info(int index)
	{
		Text_Guide.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("combine_guide_{0}", index));
	}

	private void show_combine_button(bool value)
	{
		Button_Combine.transform.parent.gameObject.SetActive(value);
		if (value)
		{
			Button_Combine.transform.parent.localScale = Vector3.zero;
			Button_Combine.transform.parent.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
		}
	}

	private void update_scroll_height()
	{
		int count = mList.Count;
		int num = MathDxx.CeilBig((float)count / (float)LineCount);
		RectTransform content = mScrollRect.content;
		Vector2 sizeDelta = mScrollRect.content.sizeDelta;
		content.sizeDelta = new Vector2(sizeDelta.x, num * height + BottomHeight);
	}

	private void OnCombineDown(EquipCombineChooseOne one)
	{
		if (one.mIndex == 0)
		{
			miss_combine_parent();
			set_guide_info(0);
		}
		else
		{
			mCombineParent.down_one(one.mIndex);
			if ((bool)one.mEquipChoose)
			{
				one.mEquipChoose.PlayAni(value: true);
			}
			set_guide_info(1);
			one.Clear();
		}
		show_combine_button(value: false);
	}

	private void OnClickOne(EquipCombineOne one)
	{
		if (one.mChoose != null)
		{
			OnCombineDown(one.mChoose);
			set_equip_lock(one, value: false);
		}
		else if (mChoose == null)
		{
			int count = one.mData.data.BreakNeed;
			if (count > 0)
			{
				mChoose = one;
				set_guide_info(1);
				mCombineParent.init_data(count, one);
				set_all_equips_lock(value: true);
				play_combine(0, one, delegate
				{
					mCombineParent.Init(count, one);
					mChoose.SetChoose(mCombineParent.GetChoose(0));
					mCombineParent.Show(value: true);
				});
			}
		}
		else
		{
			int num = mCombineParent.FindEmpty();
			if (num >= 0 && mCombineParent.can_choose(one))
			{
				play_combine(num, one, delegate
				{
					one.SetChoose(mCombineParent.ChooseOne(one));
					if (mCombineParent.Is_Full())
					{
						show_combine_button(value: true);
						set_guide_info(2);
					}
				});
			}
		}
	}

	private void play_combine(int index, EquipCombineOne one, Action callback)
	{
		Sequence s = mSeqPool.Get();
		mPlayOne.gameObject.SetActive(value: true);
		mPlayOne.Init(index, one.mData);
		mPlayOne.SetButtonEnable(value: false);
		mPlayOne.transform.position = one.transform.position;
		mPlayOne.transform.localScale = Vector3.one * mCombineParent.GetScale(index);
		s.Append(mPlayOne.transform.DOMove(mCombineParent.GetPosition(index), 0.3f).SetEase(Ease.OutQuad));
		mMaskparent.SetActive(value: true);
		s.AppendCallback(delegate
		{
			mMaskparent.SetActive(value: false);
			if (callback != null)
			{
				callback();
			}
		});
	}

	private void miss_combine_parent()
	{
		mCombineParent.Show(value: false);
		mChoose.SetChoose(null);
		set_all_equips_lock(value: false);
		mChoose = null;
	}

	private void set_all_equips_lock(bool value)
	{
		bLock = value;
		int i = 0;
		for (int count = mItemList.Count; i < count; i++)
		{
			set_equip_lock(mItemList[i], value);
		}
	}

	private void set_equip_lock(EquipCombineOne one, bool value)
	{
		if (mChoose == null)
		{
			return;
		}
		int num = 0;
		int index = mCombineParent.GetIndex(0);
		if (index >= 0 && index < mList.Count)
		{
			num = mList[index].EquipID;
		}
		if (value)
		{
			int num2 = mCombineParent.get_choose_index(one);
			if (num2 >= 0)
			{
				EquipCombineChooseOne choose = mCombineParent.GetChoose(num2);
				if (choose != null)
				{
					one.SetChoose(choose);
					choose.Set_Choose_Equip(one);
				}
				return;
			}
		}
		if (value)
		{
			if (one.mData.EquipID != num)
			{
				one.SetLock(value: true);
				one.PlayAni(value: false);
			}
			else
			{
				one.SetLock(value: false);
				one.PlayAni(value: true);
			}
		}
		else
		{
			one.SetLock(value: false);
			one.SetChoose(null);
			one.PlayAni(value: false);
		}
	}

	protected override void OnClose()
	{
		mSeqPool.Clear();
		if (onClose != null)
		{
			onClose();
		}
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_CombineTitle");
		Text_Combine.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Combine");
	}
}
