




using System;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1026 : AIMoveBase
{
	public Action onDown;

	public Action onUp;

	private AIGroundBase m_AIGroiund;

	private int range;

	protected EntityBase target;

	protected List<Grid.NodeItem> findpath;

	protected Vector3 nextpos;

	protected Vector3 endpos;

	private ActionBattle action = new ActionBattle();

	private bool bDizzy;

	private bool bMissHP;

	public AIMove1026(EntityBase entity, int range)
		: base(entity)
	{
		if (entity != null)
		{
			EntityMonsterBase entityMonsterBase = entity as EntityMonsterBase;
			if (entityMonsterBase != null)
			{
				AIBase aI = entityMonsterBase.GetAI();
				if (aI != null)
				{
					m_AIGroiund = (aI as AIGroundBase);
				}
			}
		}
		name = "1026move";
		this.range = range;
	}

	protected override void OnInitBase()
	{
		if (m_AIGroiund == null)
		{
			End();
			return;
		}
		bMissHP = false;
		target = GameLogic.Self;
		GameLogic.Release.MapCreatorCtrl.RandomItemSide(m_Entity, range, out float endx, out float endz);
		endpos = new Vector3(endx, 0f, endz);
		SetAnimation();
	}

	private void SetAnimation()
	{
		m_Entity.m_AniCtrl.SetString("Skill", string.Empty);
		m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", revert: true);
		m_Entity.m_AniCtrl.SendEvent("Skill");
		action.DeInit();
		if (!bMissHP)
		{
			m_Entity.ShowHP(show: false);
			bMissHP = true;
		}
		action.Init(m_Entity);
		float animationTime = m_Entity.m_AniCtrl.GetAnimationTime("Skill");
		action.AddActionWaitDelegate(animationTime - 0.2f, delegate
		{
			m_AIGroiund.GroundShow(value: false);
			if (onDown != null)
			{
				onDown();
			}
		});
		action.AddActionWaitDelegate(0.1f, delegate
		{
			m_Entity.SetCollider(enable: false);
			EntityBase entity = m_Entity;
			Vector3 position = m_Entity.position;
			float x = position.x;
			Vector3 position2 = m_Entity.position;
			entity.SetPosition(new Vector3(x, -5f, position2.z));
		});
		action.AddActionWaitDelegate(1.5f, delegate
		{
			m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
			m_Entity.SetPosition(new Vector3(endpos.x, 0f, endpos.z));
			m_AIGroiund.GroundShow(value: true);
			m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", revert: false);
			m_Entity.m_AniCtrl.SetString("Skill", string.Empty);
			m_Entity.m_AniCtrl.SendEvent("Skill");
			m_Entity.ShowHP(show: true);
			bMissHP = false;
			GameLogic.Hold.Sound.PlayMonsterSkill(5100006, m_Entity.position);
		});
		action.AddActionWaitDelegate(0.3f, delegate
		{
			if (onUp != null)
			{
				onUp();
			}
		});
		action.AddActionWaitDelegate(0.3f, delegate
		{
			m_Entity.SetCollider(enable: true);
			End();
		});
	}

	private void OnDizzy(bool dizzy)
	{
		bDizzy = dizzy;
		if (dizzy)
		{
			target = null;
			action.DeInit();
			EntityBase entity = m_Entity;
			Vector3 position = m_Entity.position;
			float x = position.x;
			Vector3 position2 = m_Entity.position;
			entity.SetPosition(new Vector3(x, 0f, position2.z));
		}
		else
		{
			target = GameLogic.Self;
			SetAnimation();
		}
	}

	protected override void OnEnd()
	{
		EntityBase entity = m_Entity;
		Vector3 position = m_Entity.position;
		float x = position.x;
		Vector3 position2 = m_Entity.position;
		entity.SetPosition(new Vector3(x, 0f, position2.z));
		m_Entity.SetCollider(enable: true);
		action.DeInit();
	}
}
