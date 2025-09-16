




using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

//@TODO Show info
//[Serializable]
public class AttackControl
{
	private static Dictionary<KeyCode, MoveControl.EMoveDirection> m_KeyDic = new Dictionary<KeyCode, MoveControl.EMoveDirection>
	{
		{
			KeyCode.UpArrow,
			MoveControl.EMoveDirection.Up
		},
		{
			KeyCode.DownArrow,
			MoveControl.EMoveDirection.Down
		},
		{
			KeyCode.LeftArrow,
			MoveControl.EMoveDirection.Left
		},
		{
			KeyCode.RightArrow,
			MoveControl.EMoveDirection.Right
		}
	};

	private static Dictionary<MoveControl.EMoveDirection, Vector2> m_DirDic = new Dictionary<MoveControl.EMoveDirection, Vector2>
	{
		{
			MoveControl.EMoveDirection.Up,
			new Vector2(0f, 1f)
		},
		{
			MoveControl.EMoveDirection.Down,
			new Vector2(0f, -1f)
		},
		{
			MoveControl.EMoveDirection.Left,
			new Vector2(-1f, 0f)
		},
		{
			MoveControl.EMoveDirection.Right,
			new Vector2(1f, 0f)
		}
	};

	protected EntityBase m_EntityHero;

	protected EntityHero m_EntitySelf;

	protected JoyData m_JoyData = default(JoyData);

	private float pAliveTime;

	private GameObject child;

	[SerializeField]
	private float RotateAngle = 270f;

	private Vector3 Direction = new Vector3(0f, 0f, -1f);

	private float updateangley;

	private bool bRegister;

	private AttackCtrl_Bomberman mBomberman;

	private bool bCanRotateP = true;

	public Func<bool> OnRotateOverEvent;

	protected float AliveTime => pAliveTime;

	public bool CanRotate => bCanRotateP;

	public void SetCanRotate(bool value)
	{
		bCanRotateP = value;
	}

	public void Init(EntityBase entity)
	{
		m_EntityHero = entity;
		m_EntitySelf = (m_EntityHero as EntityHero);
		m_JoyData.name = "AttackJoy";
		child = m_EntityHero.Child;
	}

	public void Start()
	{
		RegisterJoyEvent();
		if (GameLogic.Hold.BattleData.Challenge_BombermanEnable())
		{
			mBomberman = new AttackCtrl_Bomberman();
			mBomberman.Init(m_EntityHero);
		}
		OnStart();
	}

	protected virtual void OnStart()
	{
	}

	protected virtual void OnDestroys()
	{
		if (mBomberman != null)
		{
			mBomberman.DeInit();
			mBomberman = null;
		}
	}

	public void DeInit()
	{
		RemoveJoyEvent();
		OnDestroys();
	}

	public virtual void Reset()
	{
	}

	public float GetCurrentAngle()
	{
		return updateangley;
	}

	public void RegisterJoyEvent()
	{
		if (!bRegister && m_EntityHero.IsSelf)
		{
			ScrollCircle.On_JoyTouchStart += OnMoveStart;
			ScrollCircle.On_JoyTouching += OnMoving;
			ScrollCircle.On_JoyTouchEnd += OnMoveEnd;
			bRegister = true;
		}
	}

	public void RemoveJoyEvent()
	{
		if (bRegister)
		{
			ScrollCircle.On_JoyTouchStart -= OnMoveStart;
			ScrollCircle.On_JoyTouching -= OnMoving;
			ScrollCircle.On_JoyTouchEnd -= OnMoveEnd;
			bRegister = false;
		}
	}

	public void OnMoveStart(JoyData data)
	{
		if (data.name == "AttackJoy")
		{
            //Debug.Log("@LOG AttackControl.OnMoveStart");
			m_EntityHero.m_AniCtrl.SendEvent("AttackPrev");
			if (m_EntityHero.m_Weapon != null)
			{
                //Debug.LogFormat("@LOG AttackControl.OnMoveStart 2 {0}, {1}", m_EntityHero.m_Weapon, m_EntityHero.m_HatredTarget.name);
                m_EntityHero.m_Weapon.SetTarget(m_EntityHero.m_HatredTarget);
				m_EntityHero.m_Weapon.AttackJoyTouchDown();
			}
			RotateHero(data.angle);
			m_EntityHero.m_MoveCtrl.SetMoving(moving: false);
		}
	}

	public void OnMoving(JoyData data)
	{
		if (!(data.name == "AttackJoy"))
		{
		}
	}

	public void OnMoveEnd(JoyData data)
	{
		if (data.name == "AttackJoy" && m_EntityHero.m_Weapon != null)
		{
            //Debug.LogFormat("@LOG OnMoveEnd data.name:{0}, type:{1}, weapon:{2}",
            //    data.name, this.GetType(), m_EntityHero.m_Weapon);
            m_EntityHero.m_Weapon.AttackJoyTouchUp();
		}
	}

	public void OnMoveEnd()
	{
		OnMoveEnd(m_JoyData);
	}

	public bool GetAttacking()
	{
		return m_EntityHero.mAniCtrlBase.IsCurrentState("AttackPrev") || m_EntityHero.mAniCtrlBase.IsCurrentState("AttackEnd");
	}

	public void RotateUpdate(EntityBase target)
	{
		if (!m_EntityHero.m_MoveCtrl.GetMoving() && target != null && !target.GetIsDead())
		{
			Vector3 position = target.transform.position;
			float x = position.x;
			Vector3 position2 = m_EntityHero.position;
			float num = x - position2.x;
			Vector3 position3 = target.transform.position;
			float z = position3.z;
			Vector3 position4 = m_EntityHero.position;
			float num2 = z - position4.z;
			float num3 = new Vector2(num, num2).magnitude;
			if (num3 == 0f)
			{
				num3 = 1f;
			}
			num /= num3;
			num2 /= num3;
			m_JoyData.direction = new Vector3(num, 0f, num2);
			m_JoyData.angle = Utils.getAngle(m_JoyData.direction);
			RotateHero(m_JoyData.angle);
		}
	}

	public void RotateHero(float angle)
	{
		float x = MathDxx.Sin(angle);
		float z = MathDxx.Cos(angle);
		Direction = new Vector3(x, 0f, z).normalized;
		RotateAngle = angle;
		RotateAngle %= 360f;
	}

	public void RotateHero(EntityBase entity)
	{
		float angle = Utils.getAngle(entity.position - m_EntityHero.position);
		RotateHero(angle);
	}

	public bool RotateOver()
	{
		if (OnRotateOverEvent != null)
		{
			return OnRotateOverEvent();
		}
		if (MathDxx.Abs(updateangley - RotateAngle) < 1f)
		{
			updateangley = Utils.getAngle(Direction.x, Direction.z);
			if ((bool)m_EntityHero.Child)
			{
				m_EntityHero.Child.transform.localRotation = Quaternion.Euler(0f, updateangley, 0f);
			}
			return true;
		}
		return false;
	}

	public float GetHeroRotate()
	{
		return RotateAngle;
	}

	public void SetRotate(float angle)
	{
		float x = MathDxx.Sin(angle);
		float z = MathDxx.Cos(angle);
		Direction = new Vector3(x, 0f, z);
		RotateAngle = angle;
		updateangley = angle;
		if ((bool)m_EntityHero.Child)
		{
			m_EntityHero.Child.transform.localRotation = Quaternion.Euler(0f, updateangley, 0f);
		}
	}

	public virtual void UpdateProgress()
	{
		if (!CanRotate || !m_EntityHero || m_EntityHero.GetIsDead() || m_EntityHero.m_EntityData.IsDizzy() || m_EntityHero.m_EntityData == null || m_EntityHero.m_EntityData.attribute == null)
		{
			return;
		}
		pAliveTime = Updater.AliveTime;
		updateangley = MathDxx.MoveTowardsAngle(updateangley, RotateAngle, (float)m_EntityHero.m_EntityData.attribute.RotateSpeed.Value * Updater.delta);
		if ((bool)m_EntityHero.Child)
		{
			m_EntityHero.Child.transform.localRotation = Quaternion.Euler(0f, updateangley, 0f);
			m_EntityHero.SetEulerAngles(m_EntityHero.Child.transform.eulerAngles);
			if ((bool)m_EntitySelf)
			{
				m_EntitySelf.Coin_Absorb.localRotation = m_EntityHero.Child.transform.localRotation;
			}
		}
		if (mBomberman != null)
		{
			mBomberman.Update();
		}
	}

	private float GetHeroRotate(float currenty, float endx, float endy, float framepery)
	{
		float angle = Utils.getAngle(endx, endy);
		angle = (angle + 360f) % 360f;
		if (MathDxx.Abs(currenty - angle) < 0.1f || MathDxx.Abs(currenty - angle + 360f) < 0.1f || MathDxx.Abs(currenty - angle - 360f) < 0.1f)
		{
			return angle;
		}
		if (angle < currenty)
		{
			if (currenty - angle < 180f)
			{
				currenty -= framepery;
				if (currenty < angle)
				{
					currenty = angle;
				}
			}
			else
			{
				currenty += framepery;
				if (currenty >= 360f)
				{
					currenty -= 360f;
					if (currenty > angle)
					{
						currenty = angle;
					}
				}
			}
		}
		else if (angle - currenty >= 180f)
		{
			currenty -= framepery;
			if (currenty < 0f)
			{
				currenty += 360f;
				if (currenty < angle)
				{
					currenty = angle;
				}
			}
		}
		else
		{
			currenty += framepery;
			if (currenty > angle)
			{
				currenty = angle;
			}
		}
		return Utils.GetFloat2(currenty);
	}

	private void FixedUpdate()
	{
	}

	public virtual void MoveEndCallBack()
	{
	}
}
