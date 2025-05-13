




using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class PauseUICtrl : MediatorCtrlBase
{
	public ButtonCtrl Button_Sound;

	public ButtonCtrl Button_Continue;

	public ButtonCtrl Button_Home;

	public Image SoundIcon;

	public ScrollRect mScrollRect;

	public RectTransform mScrollContent;

	public Text Text_Title;

	public UILineCtrl mLineCtrl;

	private Sequence seq;

	private GameObject copyitem;

	private LocalUnityObjctPool mPool;

	protected override void OnInit()
	{
		copyitem = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/PauseUI/SkillLearnOne"));
		copyitem.SetParentNormal(base.gameObject);
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<PauseUISkillIconCtrl>(copyitem);
		copyitem.SetActive(value: false);
		Button_Sound.onClick = delegate
		{
			try
			{
				GameLogic.Hold.Sound.ChangeSound();
				UpdateSound();
			}
			catch
			{
				SdkManager.Bugly_Report("PauseUI", "Sound Button error");
			}
		};
		Button_Continue.onClick = delegate
		{
			try
			{
				WindowUI.CloseWindow(WindowID.WindowID_Pause);
			}
			catch
			{
				SdkManager.Bugly_Report("PauseUI", "Continue Button error");
			}
		};
		Button_Home.onClick = delegate
		{
			string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("popwindow_returnhome_title");
			string languageByTID2 = GameLogic.Hold.Language.GetLanguageByTID("popwindow_returnhome_content");
			WindowUI.ShowPopWindowUI(languageByTID, languageByTID2, delegate
			{
				GameLogic.Hold.BattleData.SetWin(value: false);
				int coins = 0;
				try
				{
					coins = (int)GameLogic.Hold.BattleData.GetGold();
				}
				catch
				{
					SdkManager.Bugly_Report("PauseUI", "Home Button gold get error");
				}
				int equipment = 0;
				try
				{
					equipment = GameLogic.Hold.BattleData.GetEquips().Count;
				}
				catch
				{
					SdkManager.Bugly_Report("PauseUI", "Home Button equipcount get error");
				}
				int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
				int end_layer = 0;
				try
				{
					end_layer = GameLogic.Release.Mode.GetCurrentRoomID();
				}
				catch
				{
					SdkManager.Bugly_Report("PauseUI", "Home Button roomid get error");
				}
				SdkManager.send_event_game_end(0, BattleSource.eWorld, BattleEndType.EPAUSE, coins, equipment, level_CurrentStage, end_layer, 0, 0, 0, 0);
				LocalSave.Instance.BattleIn_DeInit();
				WindowUI.ShowLoading(delegate
				{
					WindowUI.ShowWindow(WindowID.WindowID_Main);
				});
			});
		};
	}

	private void InitSkills()
	{
		if (!GameLogic.Self)
		{
			return;
		}
		KillSequence();
		mPool.Collect<PauseUISkillIconCtrl>();
		List<int> skills = GameLogic.Self.GetSkillList();
		int num = skills.Count - 1;
		while (num >= 0 && num < skills.Count)
		{
			int num2 = skills[num];
			if (LocalModelManager.Instance.Skill_skill.GetBeanById(num2).SkillIcon == 0)
			{
				SdkManager.Bugly_Report("PauseUICtrl", Utils.FormatString("Player Skill Have a error SkillID:::{0}", num2));
				skills.RemoveAt(num);
			}
			num--;
		}
		int count = skills.Count;
		mScrollRect.movementType = ((count > 10) ? ScrollRect.MovementType.Elastic : ScrollRect.MovementType.Clamped);
		int width = 120;
		int height = 120;
		int startx = -240;
		int num3 = count / 5 * 120 + 10;
		if (count % 5 > 0)
		{
			num3 += 120;
		}
		RectTransform rectTransform = mScrollContent;
		Vector2 sizeDelta = mScrollContent.sizeDelta;
		rectTransform.sizeDelta = new Vector2(sizeDelta.x, num3);
		seq = DOTween.Sequence();
		seq.SetUpdate(isIndependentUpdate: true);
		float starty = num3 / 2 - 65;
		for (int i = 0; i < count; i++)
		{
			int index = i;
			seq.AppendCallback(delegate
			{
				if (index < skills.Count)
				{
					PauseUISkillIconCtrl pauseUISkillIconCtrl = mPool.DeQueue<PauseUISkillIconCtrl>();
					pauseUISkillIconCtrl.Init(skills[index]);
					RectTransform rectTransform2 = pauseUISkillIconCtrl.transform as RectTransform;
					rectTransform2.SetParentNormal(mScrollRect.content);
					int num4 = index % 5;
					int num5 = index / 5;
					rectTransform2.name = index.ToString();
					rectTransform2.anchoredPosition = new Vector2(startx + num4 * width, starty - (float)(num5 * height));
				}
			});
			seq.AppendInterval(0.1f);
		}
	}

	protected override void OnOpen()
	{
		GameLogic.SetPause(pause: true);
		InitUI();
	}

	private void InitUI()
	{
		UpdateSound();
		InitSkills();
	}

	private void UpdateSound()
	{
		bool sound = GameLogic.Hold.Sound.GetSound();
		SoundIcon.sprite = SpriteManager.GetUICommon((!sound) ? "Setting_Off1" : "Setting_On1");
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	protected override void OnClose()
	{
		KillSequence();
		GameLogic.SetPause(pause: false);
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
		mLineCtrl.SetText(GameLogic.Hold.Language.GetLanguageByTID("暂停_技能学习"));
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("暂停_标题");
	}
}
