




public class AnimationCtrlHero : AnimationCtrlBase
{
	protected override void Event_AttackPrevI(AniClass a)
	{
        //UnityEngine.Debug.LogFormat("@LOG Event_AttackPrevI name:{0}, speed:{1}", a.name, a.Speed);
		if (m_Entity.m_Weapon != null && m_Entity.m_Weapon.OnAttackStartStartAction != null)
		{
			m_Entity.m_Weapon.OnAttackStartStartAction();
		}
		mActionList["AttackPrev"].ActionClear();
		UpdateAnimationSpeed(a.name);
		if ((bool)ani)
		{
			if (!a.revert)
			{
				ani[a.value].time = 0f;
			}
			else
			{
				ani[a.value].time = ani[a.value].clip.length;
			}
			ani.Play(a.value);
		}
		float waitTime = 1f;
		if ((bool)ani)
		{
			waitTime = ani[a.value].length / a.Speed;
		}
		mActionList[a.name].AddAction(new ActionBasic.ActionWait
		{
			waitTime = waitTime
		});
		mActionList[a.name].AddAction(new ActionBasic.ActionDelegate
		{
			action = delegate
			{
				m_Entity.PlayAttack();
				if ((bool)m_Entity && m_Entity.m_Weapon != null && m_Entity.m_Weapon.OnAttackStartEndAction != null)
				{
					m_Entity.m_Weapon.OnAttackStartEndAction();
				}
				if ((bool)m_Entity && m_Entity.Event_OnAttack != null)
				{
					m_Entity.Event_OnAttack();
				}
			}
		});
	}

	protected override void Event_AttackEndI(AniClass a)
	{
		UpdateAnimationSpeed(a.name);
		float waitTime = 1f;
		if ((bool)ani)
		{
			if (!a.revert)
			{
				ani[a.value].time = 0f;
			}
			else
			{
				ani[a.value].time = ani[a.value].clip.length;
			}
			ani.Play(a.value);
			waitTime = ani[a.value].length / a.Speed;
		}
		if (m_Entity.m_Weapon != null && m_Entity.m_Weapon.OnAttackEndStartAction != null)
		{
			m_Entity.m_Weapon.OnAttackEndStartAction();
		}
		mActionList[a.name].AddAction(new ActionBasic.ActionWait
		{
			waitTime = waitTime
		});
		mActionList[a.name].AddAction(new ActionBasic.ActionDelegate
		{
			action = delegate
			{
				OnAttackEnd();
				UpdateTouch();
			}
		});
	}

	private void OnAttackEnd()
	{
		if ((bool)m_Entity && m_Entity.m_Weapon != null && m_Entity.m_Weapon.OnAttackEndEndAction != null)
		{
			m_Entity.m_Weapon.OnAttackEndEndAction();
		}
	}

	protected override void Event_CallI(AniClass a)
	{
		UpdateAnimationSpeed(a.name);
		float waitTime = 1f;
		if ((bool)ani)
		{
			ani.Play(a.value);
			waitTime = ani[a.value].length / a.Speed;
		}
		mActionList[a.name].AddAction(new ActionBasic.ActionWait
		{
			waitTime = waitTime
		});
		mActionList[a.name].AddAction(new ActionBasic.ActionDelegate
		{
			action = delegate
			{
				UpdateTouch();
			}
		});
	}

	protected override void Event_SkillI(AniClass a)
	{
		if (ani[a.value] == null)
		{
			return;
		}
		UpdateAnimationSpeed(a.name);
		float waitTime = 1f;
		if ((bool)ani)
		{
			if (!a.revert)
			{
				ani[a.value].time = 0f;
			}
			else
			{
				ani[a.value].time = ani[a.value].clip.length;
			}
			ani.Play(a.value);
			waitTime = ani[a.value].length / a.Speed;
		}
		mActionList[a.name].AddAction(new ActionBasic.ActionWait
		{
			waitTime = waitTime
		});
		mActionList[a.name].AddAction(new ActionBasic.ActionDelegate
		{
			action = delegate
			{
				if (mAniStringList["SkillEnd"].value != "SkillEnd")
				{
					if (m_Entity.OnSkillActionEnd != null)
					{
						m_Entity.OnSkillActionEnd();
					}
					m_Entity.m_AniCtrl.SendEvent("SkillEnd", force: true);
				}
				else
				{
					UpdateTouch();
					if (m_Entity.OnSkillActionEnd != null)
					{
						m_Entity.OnSkillActionEnd();
					}
				}
			}
		});
	}

	protected override void Event_ContinuousI(AniClass a)
	{
		UpdateAnimationSpeed(a.name);
		if ((bool)ani)
		{
			if (!a.revert)
			{
				ani[a.value].time = 0f;
			}
			else
			{
				ani[a.value].time = ani[a.value].clip.length;
			}
			ani.Play(a.value);
		}
	}

	protected override void Event_IdleI(AniClass a)
	{
		UpdateAnimationSpeed(a.name);
		if ((bool)ani)
		{
			ani.CrossFade(a.value, 0.2f);
		}
	}

	protected override void Event_RunI(AniClass a)
	{
		if (PrevState.name == "AttackEnd" || PrevState.name == "AttackPrev")
		{
			mAttackInterrupt = true;
			if ((bool)m_Entity && m_Entity.m_Weapon != null && m_Entity.m_Weapon.OnAttackInterruptAction != null)
			{
				m_Entity.m_Weapon.OnAttackInterruptAction();
			}
			mActionList["AttackPrev"].ActionClear();
			mActionList["AttackEnd"].ActionClear();
			AttackInterrupt();
		}
		UpdateAnimationSpeed(a.name);
		if ((bool)ani)
		{
			ani.CrossFade(a.value, 0.2f);
		}
	}

	protected override void Event_HittedZI(AniClass a)
	{
		if (!(a.value == string.Empty))
		{
			PlayHittedAction(value: true);
			UpdateAnimationSpeed(a.name);
			float waitTime = 1f;
			if ((bool)ani)
			{
				ani.Play(a.value);
				waitTime = ani[a.value].length / a.Speed;
			}
			mActionList[a.name].AddAction(new ActionBasic.ActionWait
			{
				waitTime = waitTime
			});
			mActionList[a.name].AddAction(new ActionBasic.ActionDelegate
			{
				action = delegate
				{
					PlayHittedAction(value: false);
					if (IsCurrentState("Hitted"))
					{
						UpdateTouch();
					}
				}
			});
		}
	}

	protected override void Event_DizzyI(AniClass a)
	{
		UpdateAnimationSpeed(a.name);
		if ((bool)ani)
		{
			ani.Play(a.value);
		}
	}

	protected override void Event_DeadI(AniClass a)
	{
		UpdateAnimationSpeed(a.name);
		if ((bool)ani)
		{
			ani.Play(a.value);
		}
	}

	protected override void Event_SkillEndI(AniClass a)
	{
		UpdateAnimationSpeed(a.name);
		float waitTime = 1f;
		if ((bool)ani)
		{
			ani.Play(a.value);
			waitTime = ani[a.value].length / a.Speed;
		}
		mActionList[a.name].AddAction(new ActionBasic.ActionWait
		{
			waitTime = waitTime
		});
		mActionList[a.name].AddAction(new ActionBasic.ActionDelegate
		{
			action = delegate
			{
				UpdateTouch();
			}
		});
	}

	protected override void AttackInterrupt()
	{
		base.AttackInterrupt();
		OnAttackEnd();
	}
}
