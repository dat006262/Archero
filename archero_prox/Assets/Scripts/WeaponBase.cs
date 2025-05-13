




using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

//@TODO Show info
//[Serializable]
public class WeaponBase
{
	public Action OnAttackStartStartAction;

	public Action OnAttackStartEndAction;

	public Action OnAttackEndStartAction;

	public Action OnAttackEndEndAction;

	public Action OnAttackInterruptAction;

	public Action OnBulletCache;

	public Action Event_EntityAttack_AttackEnd;

	private bool _attack_ani_end;

	private bool _attackend_actionend = true;

	private bool bInit;

	public Weapon_weapon m_Data;

	protected EntityBase m_Entity;

	protected bool pShowDirection = true;

	protected int BulletID;

	private string prevAttackPrev;

	private string prevAttackEnd;

	protected int ParabolaSize = 1;

	protected ActionBasic action;

	protected SequencePool mSeqPool = new SequencePool();

	private WaitForSeconds continue_delay = new WaitForSeconds(0.1f);

	private bool bClear;

	protected bool bDizzyRemove = true;

	protected EntityBase Target;

	protected bool bAttackEndActionEnd
	{
		get
		{
			return _attackend_actionend;
		}
		set
		{
			if (!_attackend_actionend && value)
			{
				_attackend_actionend = true;
				Event2EntityAttack();
			}
			else
			{
				_attackend_actionend = value;
			}
		}
	}

	public bool ShowDirection
	{
		set
		{
			pShowDirection = value;
		}
	}

	public void Init(EntityBase entity, int weaponid)
	{
		m_Entity = entity;
		BulletID = weaponid;
		m_Data = LocalModelManager.Instance.Weapon_weapon.GetBeanById(BulletID);
		OnInit();
		Install();
	}

	protected virtual void OnInit()
	{
	}

	public void SetTarget(EntityBase entity)
	{
		Target = entity;
	}

	public void SetDizzyCantRemove()
	{
		bDizzyRemove = false;
	}

	public void Install()
	{
		if (!bInit)
		{
			bInit = true;
			EntityBase entity = m_Entity;
			entity.OnDizzy = (Action<bool>)Delegate.Combine(entity.OnDizzy, new Action<bool>(OnDizzy));
			m_Entity.WeaponHandUpdate();
			prevAttackPrev = m_Entity.m_AniCtrl.GetString("AttackPrev");
			prevAttackEnd = m_Entity.m_AniCtrl.GetString("AttackEnd");
			m_Entity.m_AniCtrl.SetString("AttackPrev", m_Data.AttackPrevString);
			m_Entity.m_AniCtrl.SetString("AttackEnd", m_Data.AttackEndString);
			OnAttackEndEndAction = (Action)Delegate.Combine(OnAttackEndEndAction, new Action(OnAttackEnd));
			m_Entity.mAniCtrlBase.InitWeaponSpeed(m_Data.AttackSpeed);
			action = new ActionBasic();
			action.Init();
			OnInstall();
		}
	}

	protected virtual void OnInstall()
	{
	}

	public void UnInstall()
	{
		mSeqPool.Clear();
		if (!bInit)
		{
			return;
		}
		bInit = false;
		if ((bool)m_Entity)
		{
			EntityBase entity = m_Entity;
			entity.OnDizzy = (Action<bool>)Delegate.Remove(entity.OnDizzy, new Action<bool>(OnDizzy));
			if (m_Entity.m_AniCtrl != null)
			{
				m_Entity.m_AniCtrl.SetString(prevAttackPrev, "AttackPrev");
				m_Entity.m_AniCtrl.SetString(prevAttackEnd, "AttackEnd");
				if (m_Entity.mAniCtrlBase != null)
				{
					m_Entity.mAniCtrlBase.InitWeaponSpeed(1f / m_Data.AttackSpeed);
				}
			}
		}
		action.DeInit();
		OnUnInstall();
	}

	protected virtual void OnUnInstall()
	{
	}

	private void OnDizzy(bool value)
	{
		if (bDizzyRemove)
		{
			UnInstall();
		}
	}

	public static Transform GetWeaponNode(BodyMask body, int weaponnode)
	{
		if (body == null)
		{
			return null;
		}
		if (weaponnode == 1)
		{
			return body.RightWeapon.transform;
		}
		return body.LeftWeapon.transform;
	}

	public void SetFlying(bool fly)
	{
		if ((bool)m_Entity.m_WeaponHand)
		{
			MeshRenderer componentInChildren = m_Entity.m_WeaponHand.GetComponentInChildren<MeshRenderer>();
			if ((bool)componentInChildren)
			{
				componentInChildren.material.renderQueue = ((!fly) ? 2000 : 3000);
			}
		}
	}

	public virtual void AttackJoyTouchDown()
	{
	}

	public virtual void AttackJoyTouchUp()
	{
	}

	private void OnAttackEnd()
	{
		_attack_ani_end = true;
		if (bAttackEndActionEnd)
		{
			Event2EntityAttack();
		}
	}

	protected void Event2EntityAttack()
	{
		Debugger.Log(m_Entity, "WeaponBase,.Event2EntityAttack 2012 start _attack_ani_end " + _attack_ani_end + " have callback " + (Event_EntityAttack_AttackEnd != null));
		if (_attack_ani_end && Event_EntityAttack_AttackEnd != null)
		{
			Event_EntityAttack_AttackEnd();
		}
	}

	public void Attack(params object[] args)
	{
		OnAttack(args);
	}

	protected virtual void OnAttack(params object[] args)
	{
		CreateBullets();
		if (m_Entity.m_EntityData.attribute.Bullet_Continue.Value > 1)
		{
			for (int i = 0; i < m_Entity.m_EntityData.attribute.Bullet_Continue.Value - 1; i++)
			{
				action.AddAction(new ActionBasic.ActionWait
				{
					waitTime = 0.1f
				});
				action.AddAction(new ActionBasic.ActionDelegate
				{
					action = delegate
					{
						CreateBullets();
					}
				});
			}
		}
	}

	private void CreateBullets()
	{
		CreateBullets_(m_Entity.m_EntityData.attribute.Bullet_Forward.Value, 0f);
		CreateBullets_(m_Entity.m_EntityData.attribute.Bullet_Backward.Value, 180f);
		CreateBullets_Side(m_Entity.m_EntityData.attribute.Bullet_ForSide.Value);
		CreateBullets_LeftRight(m_Entity.m_EntityData.attribute.Bullet_Side.Value);
	}

	private void CreateBullets_LeftRight(long count)
	{
		float num = GameLogic.Random(0f - m_Data.RandomAngle, m_Data.RandomAngle);
		float num2 = 0.7f;
		float num3 = num2 * (float)(count - 1);
		for (int i = 0; i < count; i++)
		{
			Vector3 offsetpos = new Vector3(0f, 0f, (float)i * num2 - num3 / 2f);
			Transform transform = CreateBullet(offsetpos, num + 90f);
			BulletBase component = transform.GetComponent<BulletBase>();
			component.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID, bClear));
			component.SetTarget(Target, ParabolaSize);
		}
		for (int j = 0; j < count; j++)
		{
			Vector3 offsetpos2 = new Vector3(0f, 0f, (float)j * num2 - num3 / 2f);
			Transform transform2 = CreateBullet(offsetpos2, num - 90f);
			BulletBase component2 = transform2.GetComponent<BulletBase>();
			component2.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID, bClear));
			component2.SetTarget(Target, ParabolaSize);
		}
	}

	private void CreateBullets_Side(long count)
	{
		float num = 90f / (float)(count + 1);
		float num2 = GameLogic.Random(0f - m_Data.RandomAngle, m_Data.RandomAngle);
		for (int i = 0; i < count; i++)
		{
			float rota = (0f - num) * (float)(i + 1) + num2;
			Transform transform = CreateBullet(Vector3.zero, rota);
			BulletBase component = transform.GetComponent<BulletBase>();
			component.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID, bClear));
			component.SetTarget(Target, ParabolaSize);
		}
		for (int j = 0; j < count; j++)
		{
			float rota2 = 0f - ((0f - num) * (float)(j + 1) + num2);
			Transform transform2 = CreateBullet(Vector3.zero, rota2);
			BulletBase component2 = transform2.GetComponent<BulletBase>();
			component2.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID, bClear));
			component2.SetTarget(Target, ParabolaSize);
		}
	}

	private void CreateBullets_(long count, float rotaoffset)
	{
		float num = GameLogic.Random(0f - m_Data.RandomAngle, m_Data.RandomAngle);
		float num2 = 0.7f;
		float num3 = num2 * (float)(count - 1);
		for (int i = 0; i < count; i++)
		{
			Vector3 offsetpos = new Vector3((float)i * num2 - num3 / 2f, 0f, 0f);
			Transform transform = CreateBullet(offsetpos, rotaoffset + num);
			BulletBase component = transform.GetComponent<BulletBase>();
			component.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID, bClear));
			component.SetTarget(Target, ParabolaSize);
		}
	}

	protected Transform CreateBullet(float rota)
	{
		return CreateBullet(Vector3.zero, rota);
	}

	protected Transform CreateBullet(Vector3 offsetpos)
	{
		return CreateBullet(offsetpos, 0f);
	}

	protected Transform CreateBullet(Vector3 offsetpos, float rota)
	{
		Transform transform = null;
		if (m_Entity.IsSelf)
		{
			transform = GameLogic.Release.PlayerBullet.Get(BulletID).transform;
			transform.SetParent(m_Entity.m_Body.GetWeaponNode(m_Data.CreateNode, m_Entity.GetBulletCreateNode(m_Data.CreateNode)));
			offsetpos /= m_Entity.m_Body.GetBodyScale();
		}
		else
		{
			transform = GameLogic.BulletGet(BulletID).transform;
			transform.SetParent(m_Entity.GetBulletCreateNode(m_Data.CreateNode));
		}
		transform.localPosition = offsetpos;
		transform.SetParent(GameNode.m_PoolParent);
		Transform transform2 = transform;
		Vector3 eulerAngles = m_Entity.eulerAngles;
		transform2.rotation = Quaternion.Euler(0f, eulerAngles.y + rota, 0f);
		transform.localScale = Vector3.one;
		BulletBase component = transform.GetComponent<BulletBase>();
		OnBulletCreate(component);
		component.Init(m_Entity, BulletID);
		component.SetLastBullet(m_Entity.m_EntityData.mLastBullet);
		m_Entity.m_EntityData.mLastBullet = component;
		return transform;
	}

	protected virtual void OnBulletCreate(BulletBase bullet)
	{
	}

	protected BulletBase CreateBulletOverride()
	{
		return CreateBulletOverride(Vector3.zero, 0f);
	}

	protected BulletBase CreateBulletOverride(Vector3 offsetpos)
	{
		return CreateBulletOverride(offsetpos, 0f);
	}

	protected BulletBase CreateBulletOverride(float rota)
	{
		return CreateBulletOverride(Vector3.zero, rota);
	}

	protected BulletBase CreateBulletOverride(Vector3 offsetpos, float rota)
	{
		Transform transform = CreateBullet(offsetpos, rota + GameLogic.Random(0f - m_Data.RandomAngle, m_Data.RandomAngle));
		BulletBase component = transform.GetComponent<BulletBase>();
		component.SetBulletAttribute(new BulletTransmit(m_Entity, BulletID, bClear));
		component.SetTarget(Target);
		return component;
	}

	public void SetOrder(int order)
	{
	}
}
