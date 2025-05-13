




using DG.Tweening;
using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1048 : AIMoveBase
{
	private EntityBase target;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	private float startTime;

	private Sequence seq;

	private bool bStartMove;

	private float time;

	public AIMove1048(EntityBase entity, int time)
		: base(entity)
	{
		this.time = (float)time / 1000f;
	}

	protected override void OnInitBase()
	{
		KillSequence();
		bStartMove = true;
		target = GameLogic.Self;
		startTime = Updater.AliveTime;
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		if (bStartMove)
		{
			MoveNormal();
		}
	}

	private void MoveNormal()
	{
		AIMoving();
		if (Updater.AliveTime - startTime > time)
		{
			End();
		}
	}

	private void AIMoveStart()
	{
		m_Entity.SetSuperArmor(value: true);
		m_MoveData.action = "Skill";
		GameLogic.Hold.Sound.PlayMonsterSkill(5100001, m_Entity.position);
		ref JoyData moveData = ref m_MoveData;
		Vector3 eulerAngles = m_Entity.eulerAngles;
		moveData.UpdateDirectionByAngle(eulerAngles.y);
		m_MoveData.direction *= 6f;
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void AIMoving()
	{
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
	}

	private void KillSequence()
	{
		if (seq != null)
		{
			seq.Kill();
			seq = null;
		}
	}

	protected override void OnEnd()
	{
		KillSequence();
		m_Entity.SetSuperArmor(value: false);
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
