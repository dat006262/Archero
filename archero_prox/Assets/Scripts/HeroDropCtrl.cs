




using Dxx.Util;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class HeroDropCtrl
{
	private ActionBasic action = new ActionBasic();

	private bool bDropStart;

	private float mDropStartTime;

	private float mDropTime = 0.32f;

	private int frameCount;

	private float starty;

	private AnimationCurve curve;

	public void Init()
	{
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(100007);
		Updater.AddUpdate("HeroDropCtrl", OnUpdate);
		action.Init();
	}

	public void DeInit()
	{
		Updater.RemoveUpdate("HeroDropCtrl", OnUpdate);
		action.DeInit();
	}

	public void StartDrop()
	{
		action.AddAction(new ActionBasic.ActionDelegate
		{
			action = delegate
			{
				StartDrop1();
			}
		});
	}

	private void StartDrop1()
	{
		frameCount = Time.frameCount;
		bDropStart = true;
		mDropStartTime = -0.15f;
		starty = 32.5f;
		EntityHero self = GameLogic.Self;
		Vector3 position = GameLogic.Self.position;
		float x = position.x;
		float y = starty;
		Vector3 position2 = GameLogic.Self.position;
		self.SetPosition(new Vector3(x, y, position2.z));
	}

	private void OnUpdate(float delta)
	{
		HeroDroping();
	}

	private void HeroDroping()
	{
		if (!bDropStart || frameCount > Time.frameCount - 3)
		{
			return;
		}
		mDropStartTime += Updater.delta;
		float value = mDropStartTime / mDropTime;
		value = MathDxx.Clamp01(value);
		EntityHero self = GameLogic.Self;
		Vector3 position = GameLogic.Self.position;
		float x = position.x;
		float y = starty * curve.Evaluate(value);
		Vector3 position2 = GameLogic.Self.position;
		self.SetPosition(new Vector3(x, y, position2.z));
		if (value == 1f)
		{
			Vector3 position3 = GameLogic.Self.position;
			if (position3.y <= 0f)
			{
				EntityHero self2 = GameLogic.Self;
				Vector3 position4 = GameLogic.Self.position;
				float x2 = position4.x;
				Vector3 position5 = GameLogic.Self.position;
				self2.SetPosition(new Vector3(x2, 0f, position5.z));
				bDropStart = false;
				CreateSmoke();
				GameLogic.Hold.Sound.PlayBattleSpecial(5000005, Vector3.zero);
				GameNode.CameraShake(CameraShakeType.FirstDrop);
				action.AddActionWaitDelegate(0.6f, delegate
				{
					GameLogic.Release.Game.ShowJoy(show: true);
					GameLogic.Release.Game.SetRunning();
					DeInit();
					if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel)
					{
						if (LocalSave.Instance.BattleIn_GetIn())
						{
							List<int> list = LocalSave.Instance.BattleIn_GetLevelUpSkills();
							if (list != null)
							{
								Facade.Instance.RegisterProxy(new ChooseSkillProxy(new ChooseSkillProxy.Transfer
								{
									type = (ChooseSkillProxy.ChooseSkillType)LocalSave.Instance.BattleIn_GetLevelUpType()
								}));
								WindowUI.ShowWindow(WindowID.WindowID_ChooseSkill);
							}
						}
						else if (GameLogic.Self.m_EntityData.attribute.ExtraSkill.Value > 0)
						{
							Facade.Instance.RegisterProxy(new ChooseSkillProxy(new ChooseSkillProxy.Transfer
							{
								type = ChooseSkillProxy.ChooseSkillType.eFirst
							}));
							WindowUI.ShowWindow(WindowID.WindowID_ChooseSkill);
						}
					}
					LocalSave.Instance.BattleIn_UpdateIn();
				});
			}
		}
	}

	private void CreateSmoke()
	{
		GameObject gameObject = GameLogic.EffectGet("Effect/Smoke/Smoke");
		if ((bool)gameObject)
		{
			gameObject.transform.position = GameLogic.Self.position;
		}
		GameObject gameObject2 = GameLogic.EffectGet("Game/Player/wave");
		if ((bool)gameObject2)
		{
			gameObject2.transform.position = GameLogic.Self.position;
		}
	}
}
