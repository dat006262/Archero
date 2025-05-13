




using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GuideManager
{
	public class GuideData
	{
		public int index;

		public RectTransform t;

		public override string ToString()
		{
			return Utils.FormatString("index:{0} t:{1}", index, t.name);
		}
	}

	public class GuideCard : GuideUIBase
	{
		protected override void OnInit()
		{
			if (process != 3)
			{
				process = 0;
				if (!GetCanStartGuide())
				{
					process = 3;
				}
				guidecount = 2;
			}
		}

		protected override bool GetCanStartGuide()
		{
			return LocalSave.Instance.GetNoCard();
		}
	}

	public abstract class GuideUIBase
	{
		public int process;

		protected int guidecount;

		protected Dictionary<int, GuideData> mGuideList = new Dictionary<int, GuideData>();

		protected int mGuideIndex;

		protected Action mGuideUpdate;

		protected GuideOneCtrl _GuideOneCtrl;

		protected GuideOneCtrl mGuideOneCtrl
		{
			get
			{
				if (_GuideOneCtrl == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/GuideUI/guide"));
					_GuideOneCtrl = gameObject.GetComponent<GuideOneCtrl>();
					RectTransform rectTransform = gameObject.transform as RectTransform;
					rectTransform.SetParent(GameNode.m_FrontMask);
					rectTransform.localPosition = Vector3.zero;
					rectTransform.anchoredPosition = Vector3.zero;
					rectTransform.localScale = Vector3.one;
					rectTransform.localRotation = Quaternion.identity;
				}
				return _GuideOneCtrl;
			}
		}

		private void CheckBug()
		{
			if (guidecount == 0)
			{
				SdkManager.Bugly_Report("GuideManager_Equip", Utils.FormatString("GuideUIBase.Init guidecount == 0 in [{0}]", GetType().ToString()));
			}
		}

		public void Init()
		{
			OnInit();
			CheckBug();
		}

		protected virtual void OnInit()
		{
		}

		public void StartGuide()
		{
			if (process == 0 && GetCanStartGuide())
			{
				process = 1;
				WindowUI.ShowMask(value: true);
				WindowUI.ShowMask(value: false);
			}
		}

		protected abstract bool GetCanStartGuide();

		public virtual bool CheckGuide()
		{
			if (WindowUI.IsWindowOpened(WindowID.WindowID_Guide))
			{
				return false;
			}
			CheckBug();
			if (process != 1)
			{
				return false;
			}
			process = 2;
			WindowUI.ShowWindow(WindowID.WindowID_Guide);
			GoNext(mGuideList[0]);
			return true;
		}

		public virtual void CurrentOver(int index)
		{
			if (index == guidecount - 1 && process == 2)
			{
				show_guide(value: false);
				process = 3;
				OnGuideEnd();
				WindowUI.CloseWindow(WindowID.WindowID_Guide);
			}
			else if (!IsGuideOver())
			{
				show_guide(value: false);
			}
		}

		protected virtual void OnGuideEnd()
		{
		}

		public bool IsGuideOver()
		{
			return process == 0 || process == 3;
		}

		public bool IsGuiding()
		{
			return process == 2;
		}

		public void GoNext(int index, RectTransform t)
		{
			GoNext(new GuideData
			{
				index = index,
				t = t
			});
		}

		public void GoNext(GuideData data)
		{
			if (!mGuideList.ContainsKey(data.index))
			{
				mGuideList.Add(data.index, data);
			}
			else if (mGuideList[data.index].t == null)
			{
				mGuideList[data.index] = data;
			}
			if (IsGuiding())
			{
				show_guide(value: true);
				mGuideOneCtrl.Init(data.t);
				if (mGuideUpdate != null)
				{
					mGuideUpdate();
				}
			}
		}

		private void show_guide(bool value)
		{
			if ((bool)mGuideOneCtrl)
			{
				mGuideOneCtrl.gameObject.SetActive(value);
			}
			EventSystemCtrl.Instance.SetDragEnable(!value);
		}

		public void DeInit()
		{
			if ((bool)_GuideOneCtrl)
			{
				show_guide(value: false);
				UnityEngine.Object.Destroy(_GuideOneCtrl.gameObject);
				_GuideOneCtrl = null;
			}
			OnDeInit();
		}

		protected virtual void OnDeInit()
		{
		}
	}

	public class GuideEquip : GuideUIBase
	{
		protected override void OnInit()
		{
			guidecount = 3;
			if (process != 3)
			{
				process = 0;
				if (LocalSave.Instance.GetHaveEquips(havewear: true).Count > 1)
				{
					process = 3;
				}
			}
		}

		protected override bool GetCanStartGuide()
		{
			return true;
		}

		public override bool CheckGuide()
		{
			return base.CheckGuide();
		}
	}

	private int guideStep;

	private GameObject currentobj;

	private bool bBattleNeedGuide = true;

	public GuideCard mCard
	{
		get;
		private set;
	}

	public GuideEquip mEquip
	{
		get;
		private set;
	}

	public void Init()
	{
		if (bBattleNeedGuide && (!LocalSave.Instance.GetNoCard() || LocalSave.Instance.GetHaveEquips(havewear: true).Count > 1 || LocalSave.Instance.GetGold() > 100 || PlayerPrefsEncrypt.GetBool("guide_battle")))
		{
			bBattleNeedGuide = false;
			LocalSave.Instance.SaveExtra.SetGuideBattleProcess(2);
		}
		Card_Init();
		Equip_Init();
	}

	public void DeInit()
	{
		RemoveLastGuide();
		Card_DeInit();
		Equip_DeInit();
	}

	public bool GetNeedGuide()
	{
		return bBattleNeedGuide;
	}

	public void GuideBattleNext()
	{
		if (GetNeedGuide())
		{
			RemoveLastGuide();
			guideStep++;
			int num = guideStep;
			if (num == 1 || num == 2 || num == 3)
			{
				currentobj = GetGuideObj(guideStep);
				GameLogic.Release.Mode.RoomGenerate.AddGuildToMap(currentobj);
			}
			if (guideStep == 3)
			{
				PlayerPrefsEncrypt.SetBool("guide_battle", val: true);
				bBattleNeedGuide = false;
				LocalSave.Instance.SaveExtra.SetGuideBattleProcess(1);
			}
		}
	}

	private void RemoveLastGuide()
	{
		if ((bool)currentobj)
		{
			UnityEngine.Object.Destroy(currentobj);
			currentobj = null;
		}
	}

	public bool GetFlowerAttack()
	{
		if (bBattleNeedGuide && GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID() < 3)
		{
			return false;
		}
		return true;
	}

	private GameObject GetGuideObj(int index)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(Utils.FormatString("Game/Map/Guide/Guide{0}", index)));
		MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			componentsInChildren[i].sortingLayerName = "MapBG";
		}
		gameObject.transform.Find("text_content").GetComponent<TextMesh>().text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("新手引导_{0}", index));
		return gameObject;
	}

	private void Card_Init()
	{
		if (mCard == null)
		{
			mCard = new GuideCard();
		}
		mCard.Init();
	}

	public void Card_DeInit()
	{
		if (mCard != null)
		{
			mCard.DeInit();
		}
	}

	private void Equip_Init()
	{
		if (mEquip == null)
		{
			mEquip = new GuideEquip();
		}
		mEquip.Init();
	}

	public void Equip_DeInit()
	{
		if (mEquip != null)
		{
			mEquip.DeInit();
		}
	}
}
