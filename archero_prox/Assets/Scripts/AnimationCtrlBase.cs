




using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class AnimationCtrlBase
{
    [Serializable]
	public class AniClass
	{
        [SerializeField]
		private float speedinit = 1f;

        [SerializeField]
        private string _name;
		public string name
		{
			get { return _name; }
			private set { _name = value; }
		}

        [SerializeField]
        private string _value;
        public string value
        {
#if ENABLE_COMPILER_GENERATED
            [CompilerGenerated]
#endif
            get { return _value; }
#if ENABLE_COMPILER_GENERATED
            [CompilerGenerated]
#endif
            set { this._value = value; }
        }

		public bool revert
		{
			get;
			set;
		}

		private float speed_out
		{
			get;
			set;
		}

		public float Speed_Weapon
		{
			get;
			private set;
		}

		private float speed_in
		{
			get;
			set;
		}

		public float Speed
		{
			get;
			private set;
		}

        [SerializeField]
        private List<string> _action_list;

        public List<string> action_list
		{
			get { return _action_list; }
			private set { _action_list = value; }
		}

		public Action eventCmd
		{
			get;
			set;
		}

		public AniClass(string name, string value, List<string> action_list)
		{
			this.name = name;
			this.value = value;
			this.action_list = action_list;
			revert = false;
			speedinit = 1f;
			speed_out = 1f;
			speed_in = 1f;
			Speed_Weapon = 1f;
			UpdateSpeed();
		}

		public void InitSpeedInit(float speed)
		{
			speedinit = speed;
			UpdateSpeed();
		}

		public void UpdateSpeedOut(float speed)
		{
			speed_out = speed;
			UpdateSpeed();
		}

		public void UpdateSpeedIn(float speed)
		{
			speed_in += speed;
			UpdateSpeed();
		}

		public void UpdateSpeedWeapon(float speed)
		{
			Speed_Weapon *= speed;
			UpdateSpeed();
		}

		private void UpdateSpeed()
		{
			Speed = speedinit * speed_out * speed_in * Speed_Weapon;
		}

		public float GetSpeed()
		{
			return Speed;
		}

		public void AddAction(string action)
		{
			if (!action_list.Contains(action))
			{
				action_list.Add(action);
			}
		}

		public bool HaveEvent(string name)
		{
			return action_list.Contains(name);
		}
	}

	public const string AttackPrev = "AttackPrev";

	public const string AttackEnd = "AttackEnd";

	public const string Call = "Call";

	public const string Dead = "Dead";

	public const string Hitted = "Hitted";

	public const string Run = "Run";

	public const string Idle = "Idle";

	public const string Skill = "Skill";

	public const string Dizzy = "Dizzy";

	public const string Continuous = "Continuous";

	public const string SkillEnd = "SkillEnd";

	public const string TouchMoveJoy = "TouchMoveJoy";

	protected Dictionary<string, AniClass> mAniStringList = new Dictionary<string, AniClass>
	{
		{
			"AttackPrev",
			new AniClass("AttackPrev", "AttackPrev", new List<string>
			{
				"AttackEnd",
				"Run",
				"Dizzy"
			})
		},
		{
			"AttackEnd",
			new AniClass("AttackEnd", "AttackEnd", new List<string>
			{
				"Dizzy",
				"Run"
			})
		},
		{
			"Call",
			new AniClass("Call", "Call", new List<string>
			{
				"Dizzy"
			})
		},
		{
			"Dead",
			new AniClass("Dead", "Dead", new List<string>())
		},
		{
			"Hitted",
			new AniClass("Hitted", "Hitted", new List<string>
			{
				"AttackPrev",
				"Skill",
				"Call",
				"Continuous",
				"Dizzy"
			})
		},
		{
			"Run",
			new AniClass("Run", "Run", new List<string>
			{
				"Idle",
				"Hitted",
				"AttackPrev",
				"Skill",
				"Call",
				"Continuous",
				"Dizzy"
			})
		},
		{
			"Idle",
			new AniClass("Idle", "Idle", new List<string>
			{
				"Run",
				"Hitted",
				"AttackPrev",
				"Skill",
				"Call",
				"Continuous",
				"Dizzy"
			})
		},
		{
			"Skill",
			new AniClass("Skill", "Skill", new List<string>
			{
				"Dizzy"
			})
		},
		{
			"Continuous",
			new AniClass("Continuous", "Continuous", new List<string>
			{
				"Dizzy"
			})
		},
		{
			"Dizzy",
			new AniClass("Dizzy", "Dizzy", new List<string>())
		},
		{
			"SkillEnd",
			new AniClass("SkillEnd", "SkillEnd", new List<string>
			{
				"Skill"
			})
		}
	};

	protected Dictionary<string, bool> mAniBoolList = new Dictionary<string, bool>
	{
		{
			"TouchMoveJoy",
			false
		}
	};

	protected bool bPlayHittedAction = true;

	private List<string> mGlobalActList = new List<string>
	{
		"Dead"
	};

	private Action<string, float> mActionSpeed;

	private Action mActionHitted;

	protected AniClass PrevState;

	protected AniClass CurrentState;

	protected Animation ani;

	protected AnimatorBase mAniBase;

	protected EntityBase m_Entity;

	protected bool mAttackInterrupt;

	private bool bHittedCallback;

	private bool bInit;

	protected Dictionary<string, ActionBasic> mActionList = new Dictionary<string, ActionBasic>();

	public bool GetPlayHittedCallback()
	{
		return bHittedCallback;
	}

	public void OnStart()
	{
        UnityEngine.Debug.Log("@LOG AnimationCtrlBase.OnStart entity:" + m_Entity);
        CurrentState = mAniStringList["Idle"];
		mAniStringList["Idle"].eventCmd = Event_Idle;
		mAniStringList["Run"].eventCmd = Event_Run;
		mAniStringList["Hitted"].eventCmd = Event_Hitted;
		mAniStringList["AttackPrev"].eventCmd = Event_AttackPrev;
		mAniStringList["AttackEnd"].eventCmd = Event_AttackEnd;
		mAniStringList["Dead"].eventCmd = Event_Dead;
		mAniStringList["Call"].eventCmd = Event_Call;
		mAniStringList["Skill"].eventCmd = Event_Skill;
		mAniStringList["Continuous"].eventCmd = Event_Continuous;
		mAniStringList["Dizzy"].eventCmd = Event_Dizzy;
		mAniStringList["SkillEnd"].eventCmd = Event_SkillEnd;
		mActionList.Add("Idle", new ActionBasic());
		mActionList.Add("Run", new ActionBasic());
		mActionList.Add("Hitted", new ActionBasic());
		mActionList.Add("AttackPrev", new ActionBasic());
		mActionList.Add("AttackEnd", new ActionBasic());
		mActionList.Add("Call", new ActionBasic());
		mActionList.Add("Skill", new ActionBasic());
		mActionList.Add("Dizzy", new ActionBasic());
		mActionList.Add("Continuous", new ActionBasic());
		mActionList.Add("Dead", new ActionBasic());
		mActionList.Add("SkillEnd", new ActionBasic());
		Dictionary<string, ActionBasic>.Enumerator enumerator = mActionList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.Init();
		}
		bInit = true;
        UnityEngine.Debug.Log("@LOG AnimationCtrlBase.OnStart 1");
    }

    public void DeInit()
	{
		bInit = false;
		Dictionary<string, ActionBasic>.Enumerator enumerator = mActionList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<string, ActionBasic> current = enumerator.Current;
			current.Value.ActionClear();
			current.Value.DeInit();
		}
	}

	public void SetAnimation(Animation ani)
	{
		this.ani = ani;
	}

	public void SetAnimationValue(string name, string value = "")
	{
		if (mAniStringList.TryGetValue(name, out AniClass value2))
		{
			value2.value = ((!(value == string.Empty)) ? value : name);
		}
	}

	public string GetAnimationValue(string name)
	{
		if (mAniStringList.TryGetValue(name, out AniClass value))
		{
			return value.value;
		}
		return string.Empty;
	}

	public void SetAnimationClear(string name)
	{
		if (mAniStringList.TryGetValue(name, out AniClass value))
		{
			value.value = string.Empty;
		}
	}

	public float GetAnimationTime(string name)
	{
        try
        {
            //Debug.Log("@LOG name:" + name);
            //foreach (var pair in mAniStringList)
            //{
            //    Debug.Log("@LOG KEY:" + pair.Key);
            //}
            if (mAniStringList.TryGetValue(name, out AniClass value) && (bool)ani)
            {
                return ani[value.value].length / value.Speed;
            }
            return 1f;
        }
        catch(Exception ex)
        {
            Debug.LogError("@LOG GetAnimationTime Exception ex:" + ex);
            return 1f;
        }
	}

	public float GetAnimationSpeed(string name)
	{
		if (mAniStringList.TryGetValue(name, out AniClass value) && (bool)ani)
		{
			return value.GetSpeed();
		}
		return 1f;
	}

	public void UpdateAnimationSpeed(string name, float speed)
	{
		if (mAniStringList.TryGetValue(name, out AniClass value))
		{
			value.UpdateSpeedIn(speed);
		}
	}

	public void UpdateSpeedOut(float speed)
	{
		mAniStringList["AttackPrev"].UpdateSpeedOut(speed);
		mAniStringList["AttackEnd"].UpdateSpeedOut(speed);
	}

	public void InitWeaponSpeed(float speed)
	{
		mAniStringList["AttackPrev"].UpdateSpeedWeapon(speed);
		mAniStringList["AttackEnd"].UpdateSpeedWeapon(speed);
	}

	public void UpdateAttackPrevSpeed(float speed)
	{
	}

	public void UpdateWeaponSpeed(float speed)
	{
		float speed2 = 0f;
		if (speed < 0f)
		{
			speed2 = 1f + speed;
		}
		else if (speed > 0f)
		{
			speed2 = 1f / (1f - speed);
		}
		mAniStringList["AttackPrev"].UpdateSpeedWeapon(speed2);
		mAniStringList["AttackEnd"].UpdateSpeedWeapon(speed2);
	}

	public void SetAllSpeed(float speed)
	{
		Dictionary<string, AniClass>.Enumerator enumerator = mAniStringList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			UpdateAnimationSpeed(enumerator.Current.Key, speed);
		}
		UpdateWeaponSpeed(speed);
	}

	protected void UpdateAnimationSpeed(string name)
	{
		if (!ani)
		{
			return;
		}
		mAniStringList.TryGetValue(name, out AniClass value);
		if (value != null)
		{
			AnimationState animationState = ani[value.value];
			if (animationState != null)
			{
				animationState.speed = value.Speed * (float)((!value.revert) ? 1 : (-1));
			}
		}
	}

	public void SetAnimationRevert(string name, bool revert)
	{
		if (mAniStringList.TryGetValue(name, out AniClass value))
		{
			value.revert = revert;
		}
	}

	public bool GetAnimationRevert(string name)
	{
		if (mAniStringList.TryGetValue(name, out AniClass value))
		{
			return value.revert;
		}
		return false;
	}

	public void SetBool(string name, bool value)
	{
		if (mAniBoolList.ContainsKey(name))
		{
			mAniBoolList[name] = value;
		}
	}

	public void SetAnimatorBase(AnimatorBase b)
	{
		mAniBase = b;
		m_Entity = mAniBase.m_Entity;
		mAniStringList["Idle"].InitSpeedInit(mAniBase.m_Entity.m_Data.ActionSpeed[0]);
		mAniStringList["Run"].InitSpeedInit(mAniBase.m_Entity.m_Data.ActionSpeed[1]);
		mAniStringList["Hitted"].InitSpeedInit(mAniBase.m_Entity.m_Data.ActionSpeed[2]);
		mAniStringList["AttackPrev"].InitSpeedInit(mAniBase.m_Entity.m_Data.ActionSpeed[3]);
		mAniStringList["AttackEnd"].InitSpeedInit(mAniBase.m_Entity.m_Data.ActionSpeed[4]);
		mAniStringList["Dead"].InitSpeedInit(mAniBase.m_Entity.m_Data.ActionSpeed[5]);
		mAniStringList["Call"].InitSpeedInit(mAniBase.m_Entity.m_Data.ActionSpeed[6]);
		mAniStringList["Skill"].InitSpeedInit(mAniBase.m_Entity.m_Data.ActionSpeed[7]);
		mAniStringList["Continuous"].InitSpeedInit(mAniBase.m_Entity.m_Data.ActionSpeed[8]);
		mAniStringList["Dizzy"].InitSpeedInit(mAniBase.m_Entity.m_Data.ActionSpeed[9]);
		mAniStringList["SkillEnd"].InitSpeedInit(1f);
		Dictionary<string, AniClass>.Enumerator enumerator = mAniStringList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			UpdateAnimationSpeed(enumerator.Current.Key, 0f);
		}
	}

	public void SetHittedCallBack(Action callback)
	{
		mActionHitted = callback;
	}

	public void SetHeroPlayMakerColtrol(HeroPlayMakerControl ctrl)
	{
	}

	public void Reborn()
	{
		if (mAniStringList.TryGetValue("Idle", out AniClass value))
		{
			ChangeState(value);
		}
	}

	public void SetDontPlayHittedAction()
	{
		bPlayHittedAction = false;
	}

	public void SendEvent(string eventName, bool force = false)
	{
        //Debug.LogFormat("@LOG AnimationCtrlBase.SendEvent {0}, {1}", eventName, force);
        if (!bPlayHittedAction && eventName == "Hitted")
		{
            //Debug.LogFormat("@LOG AnimationCtrlBase.SendEvent 1 {0}, {1}", eventName, force);
            return;
        }
		if (eventName == "Hitted" && mActionHitted != null)
		{
            //Debug.LogFormat("@LOG AnimationCtrlBase.SendEvent 2 {0}, {1}", eventName, force);
            mActionHitted();
        }
		if (CurrentState.name == "Hitted")
		{
            //Debug.LogFormat("@LOG AnimationCtrlBase.SendEvent 3 {0}, {1}", eventName, force);
            PlayHittedAction(value: false);
        }
		if (mGlobalActList.Contains(eventName) 
            && mAniStringList.TryGetValue(eventName, out AniClass value))
		{
            //Debug.LogFormat("@LOG AnimationCtrlBase.SendEvent 4 {0}, {1}", eventName, force);
            ChangeState(value);
            return;
		}
		if (force && CurrentState.name == "AttackPrev" 
            && (eventName == "Idle" || eventName == "Run"))
		{
            //Debug.LogFormat("@LOG AnimationCtrlBase.SendEvent 5 {0}, {1}", eventName, force);
            force = false;
        }
		if (CurrentState.name != "Dead" && force 
            && mAniStringList.TryGetValue(eventName, out value))
		{
            //Debug.LogFormat("@LOG AnimationCtrlBase.SendEvent 6 {0}, {1}", eventName, force);
            ChangeState(value);
        }
		else if (CurrentState.HaveEvent(eventName) 
            && mAniStringList.TryGetValue(eventName, out value))
		{
            //Debug.LogFormat("@LOG AnimationCtrlBase.SendEvent 7 {0}, {1}", eventName, force);
            ChangeState(value);
        }
	}

	protected void ChangeState(AniClass state)
	{
        //Debug.LogFormat("@LOG AnimationCtrlBase.ChangeState state.name:{0}, state.value:{1}", state.name, state.value);
        if (ani && state != null && !string.IsNullOrEmpty(state.value))
        {
            //Debug.LogErrorFormat("@LOG AnimationCtrlBase.ChangeState ani:{0}", ani.name);
            //foreach (AnimationState s in ani)
            //{
            //    Debug.LogErrorFormat("@LOG AnimationCtrlBase.ChangeState s:{0}", s.name);
            //}
        }
        if ((state.value.Equals(string.Empty) || ani.GetClip(state.value) != null) 
            && ani[state.value] != null)
        {
            //Debug.LogFormat("@LOG AnimationCtrlBase.ChangeState 1");
            PrevState = CurrentState;
            CurrentState = state;
            CurrentState.eventCmd();
        }
	}

	public bool IsCurrentState(string state)
	{
		return CurrentState.name == state;
	}

	protected void ResetPrevState()
	{
		PrevState = mAniStringList["Idle"];
	}

	public void DizzyEnd()
	{
		UpdateTouch();
	}

	private void Event_AttackPrev()
	{
        //Debug.Log("@LOG Event_AttackPrev");
		mActionList["AttackPrev"].ActionClear();
		Event_AttackPrevI(mAniStringList["AttackPrev"]);
	}

	protected virtual void Event_AttackPrevI(AniClass a)
	{
	}

	private void Event_AttackEnd()
	{
		mActionList["AttackEnd"].ActionClear();
		Event_AttackEndI(mAniStringList["AttackEnd"]);
	}

	protected virtual void Event_AttackEndI(AniClass a)
	{
	}

	private void Event_Call()
	{
		mActionList["Call"].ActionClear();
		Event_CallI(mAniStringList["Call"]);
	}

	protected virtual void Event_CallI(AniClass a)
	{
	}

	private void Event_Skill()
	{
		mActionList["Skill"].ActionClear();
		Event_SkillI(mAniStringList["Skill"]);
	}

	protected virtual void Event_SkillI(AniClass a)
	{
	}

	private void Event_Continuous()
	{
		mActionList["Continuous"].ActionClear();
		Event_ContinuousI(mAniStringList["Continuous"]);
	}

	protected virtual void Event_ContinuousI(AniClass a)
	{
	}

	private void Event_Dizzy()
	{
		StopAllActions();
		AttackInterrupt();
		Event_DizzyI(mAniStringList["Dizzy"]);
	}

	private void Event_SkillEnd()
	{
		mActionList["SkillEnd"].ActionClear();
		Event_SkillEndI(mAniStringList["SkillEnd"]);
	}

	protected virtual void Event_SkillEndI(AniClass a)
	{
	}

	protected virtual void AttackInterrupt()
	{
	}

	protected virtual void Event_DizzyI(AniClass a)
	{
	}

	private void Event_Dead()
	{
		StopAllActions();
		Event_DeadI(mAniStringList["Dead"]);
	}

	protected virtual void Event_DeadI(AniClass a)
	{
	}

	private void Event_Idle()
	{
		Event_IdleI(mAniStringList["Idle"]);
	}

	protected virtual void Event_IdleI(AniClass a)
	{
	}

	private void Event_Run()
	{
		mActionList["AttackPrev"].ActionClear();
		Event_RunI(mAniStringList["Run"]);
	}

	protected virtual void Event_RunI(AniClass a)
	{
	}

	private void Event_Hitted()
	{
		mActionList["Hitted"].ActionClear();
		Event_HittedZI(mAniStringList["Hitted"]);
	}

	protected virtual void Event_HittedZI(AniClass a)
	{
	}

	protected void UpdateTouch()
	{
		if (mAniBoolList["TouchMoveJoy"])
		{
			SendEvent("Run", force: true);
		}
		else
		{
			SendEvent("Idle", force: true);
		}
	}

	private void StopAllActions()
	{
		Dictionary<string, ActionBasic>.Enumerator enumerator = mActionList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Value.ActionClear();
		}
		PlayHittedAction(value: false);
	}

	protected void PlayHittedAction(bool value)
	{
		if (bHittedCallback != value)
		{
			bHittedCallback = value;
			if (m_Entity.OnPlayHittedAction != null)
			{
				m_Entity.OnPlayHittedAction(bHittedCallback);
			}
		}
	}
}
