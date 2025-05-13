




using Dxx.Util;
using System;
using UnityEngine;

public class HeroAttackControl : AttackControl
{
	private bool bAttackUpdate = true;

	private EntityBase m_LastTarget;

	private float attackinterval;

	private float attackEndTime = -1f;

	private int findIndex;

	private long iii;

	private bool bAddAttack;

	private bool bPause;

	private GameObject m_TargetImageP;

	private GameObject m_TargetRedP;

	private Animation _TargetAni;

	private bool showTarget = true;

	private Vector3 TargetPos;

	public GameObject TargetImage
	{
		get
		{
			if (m_TargetImageP == null)
			{
				m_TargetImageP = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("Effect/Target"));
				m_TargetImageP.transform.SetParent(GameNode.m_Root);
				m_TargetImageP.transform.localScale = Vector3.one;
				m_TargetImageP.transform.localRotation = Quaternion.identity;
				m_TargetImageP.transform.localPosition = new Vector3(10000f, 0f, 0f);
			}
			return m_TargetImageP;
		}
	}

	private GameObject TargetRed
	{
		get
		{
			if (m_TargetRedP == null)
			{
				m_TargetRedP = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("Game/UI/Target_Red"));
				m_TargetRedP.transform.SetParent(GameNode.m_Root);
				m_TargetRedP.transform.localScale = Vector3.one;
				m_TargetRedP.transform.localRotation = Quaternion.identity;
				m_TargetRedP.transform.localPosition = new Vector3(10000f, 0f, 0f);
				m_TargetRedP.name = "TargetRed";
			}
			return m_TargetRedP;
		}
	}

	public Animation TargetAni
	{
		get
		{
			if (_TargetAni == null)
			{
				_TargetAni = TargetImage.GetComponent<Animation>();
			}
			return _TargetAni;
		}
	}

	protected override void OnStart()
	{
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		TargetImage.SetActive(value: false);
		TargetRed.SetActive(value: false);
	}

	protected override void OnDestroys()
	{
		base.OnDestroys();
		ReleaseModeManager mode = GameLogic.Release.Mode;
		mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoom));
		if (TargetImage != null)
		{
			UnityEngine.Object.Destroy(TargetImage);
		}
	}

	private void OnGotoNextRoom(RoomGenerateBase.Room room)
	{
		m_EntityHero.m_HatredTarget = null;
	}

	public override void UpdateProgress()
	{
		base.UpdateProgress();
		AutoAttackUpdate();
	}

	private void AutoAttackUpdate()
	{
		if (!GameLogic.Hold.BattleData.Challenge_AttackEnable() || m_EntityHero.GetIsDead())
		{
			return;
		}
		if (m_EntityHero.m_MoveCtrl.GetMoving())
		{
			MissTargetImage();
			ReSearchTarget();
			SetCurrentTarget();
		}
		else
		{
			if (!GetAttacking() && Updater.AliveTime - attackEndTime > attackinterval)
			{
				FindTarget();
				if (m_EntityHero.m_HatredTarget != null)
				{
					OnMoveStart(m_JoyData);
				}
				else
				{
					MissTargetImage();
				}
			}
			if (m_EntityHero.m_HatredTarget != null && RotateOver())
			{
				OnMoveEnd(m_JoyData);
				attackEndTime = Updater.AliveTime;
			}
			SetCurrentTarget();
			RotateUpdate(m_EntityHero.m_HatredTarget);
		}
		CheckCurrentTarget();
	}

	private void FindTarget()
	{
		if (m_EntityHero.m_HatredTarget == null || !m_EntityHero.m_HatredTarget.GetIsInCamera() || m_EntityHero.m_HatredTarget.GetIsDead() || ((!GameLogic.GetCanHit(m_EntityHero, m_EntityHero.m_HatredTarget) || !m_EntityHero.m_HatredTarget.GetColliderEnable()) && !GetAttacking()))
		{
			ReSearchTarget();
		}
	}

	public override void MoveEndCallBack()
	{
		base.MoveEndCallBack();
		if (m_EntityHero.m_HatredTarget != null)
		{
			m_LastTarget = m_EntityHero.m_HatredTarget;
		}
		m_EntityHero.m_HatredTarget = null;
	}

	public void ReSearchTarget()
	{
		if (m_EntityHero.m_HatredTarget != null)
		{
			m_LastTarget = m_EntityHero.m_HatredTarget;
		}
		m_EntityHero.m_HatredTarget = GameLogic.Release.Entity.FindTargetInCamera();
		if ((bool)m_EntityHero.m_HatredTarget)
		{
			m_EntityHero.m_HatredTarget.m_Body.SetTarget(value: true);
		}
		if (m_EntityHero.m_HatredTarget != m_LastTarget && (bool)m_LastTarget)
		{
			m_LastTarget.m_Body.SetTarget(value: false);
		}
	}

	private void SetCurrentTarget()
	{
		if (GameLogic.GetCanHit(m_EntityHero, m_EntityHero.m_HatredTarget))
		{
			CreateTarget();
		}
		else
		{
			MissTargetImage();
		}
	}

	private void MissTargetImage()
	{
		if (!m_EntityHero.m_HatredTarget || ((bool)m_EntityHero.m_HatredTarget && (m_EntityHero.m_HatredTarget.GetIsDead() || !m_EntityHero.m_HatredTarget.gameObject.activeInHierarchy) && (bool)TargetImage && (bool)TargetRed))
		{
			TargetImage.SetActive(value: false);
			TargetRed.SetActive(value: false);
		}
		if (TargetImage != null && showTarget)
		{
			showTarget = false;
		}
	}

	private void CheckCurrentTarget()
	{
		if (m_EntityHero.m_HatredTarget == null || !GameLogic.GetCanHit(m_EntityHero, m_EntityHero.m_HatredTarget))
		{
			MissTargetImage();
		}
	}

	private void CreateTarget()
	{
		if (!showTarget || m_LastTarget != m_EntityHero.m_HatredTarget)
		{
			showTarget = true;
			if (m_LastTarget != m_EntityHero.m_HatredTarget && (bool)TargetAni)
			{
				m_LastTarget = m_EntityHero.m_HatredTarget;
				TargetAni.Play("TargetShow");
			}
			if ((bool)TargetImage)
			{
				TargetImage.SetActive(value: true);
			}
			if ((bool)TargetRed)
			{
				TargetRed.SetActive(value: true);
			}
		}
		if ((bool)m_EntityHero.m_HatredTarget)
		{
			if ((bool)TargetImage && (bool)m_EntityHero.m_HatredTarget.m_Body)
			{
				TargetImage.transform.SetParent(m_EntityHero.m_HatredTarget.m_Body.transform);
				TargetImage.transform.localScale = Vector3.one;
				TargetImage.transform.localPosition = Vector3.zero;
			}
			if ((bool)TargetRed && (bool)m_EntityHero.m_HatredTarget.m_HPSlider)
			{
				TargetRed.transform.SetParent(m_EntityHero.m_HatredTarget.m_HPSlider.transform);
				TargetRed.transform.localScale = Vector3.one;
				TargetRed.transform.localPosition = Vector3.zero;
			}
		}
	}

	public override void Reset()
	{
	}
}
