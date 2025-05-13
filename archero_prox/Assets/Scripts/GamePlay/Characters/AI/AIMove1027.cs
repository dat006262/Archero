




using System.Collections.Generic;
using UnityEngine;

public class AIMove1027 : AIMoveBase
{
	private int range;

	protected EntityBase target;

	protected List<Grid.NodeItem> findpath;

	protected Vector3 nextpos;

	protected Vector3 endpos;

	private ActionBattle action = new ActionBattle();

	private Animation ani;

	private bool bDizzy;

	public AIMove1027(EntityBase entity, int range)
		: base(entity)
	{
		if (range < 1)
		{
			range = 1;
		}
		this.range = range;
	}

	protected override void OnInitBase()
	{
		if ((bool)m_Entity && (bool)m_Entity.m_Body)
		{
			Transform transform = m_Entity.m_Body.AnimatorBodyObj.transform.Find("GroundBreak/scale/sprite");
			if ((bool)transform)
			{
				ani = transform.GetComponent<Animation>();
			}
		}
		target = GameLogic.Self;
		SetAnimation();
	}

	private void SetAnimation()
	{
		m_Entity.m_AniCtrl.SetString("Skill", string.Empty);
		m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", revert: true);
		m_Entity.m_AniCtrl.SendEvent("Skill");
		action.DeInit();
		m_Entity.ShowHP(show: false);
		action.Init(m_Entity);
		float animationTime = m_Entity.m_AniCtrl.GetAnimationTime("Skill");
		action.AddActionWaitDelegate(animationTime - 0.2f, delegate
		{
			if ((bool)ani)
			{
				ani.Play("3028_GroundBreak_Miss");
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
			GameLogic.Release.MapCreatorCtrl.RandomItem(GameLogic.Self, range, out float endx, out float endz);
			endpos = new Vector3(endx, 0f, endz);
			m_Entity.SetPosition(new Vector3(endpos.x, 0f, endpos.z));
			if ((bool)ani)
			{
				ani.Play("3028_GroundBreak_Show");
			}
			m_Entity.mAniCtrlBase.SetAnimationRevert("Skill", revert: false);
			m_Entity.m_AniCtrl.SetString("Skill", string.Empty);
			m_Entity.m_AniCtrl.SendEvent("Skill");
			m_Entity.ShowHP(show: true);
		});
		action.AddActionWaitDelegate(0.3f, delegate
		{
			m_Entity.SetCollider(enable: true);
		});
		action.AddActionWaitDelegate(0.2f, delegate
		{
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
