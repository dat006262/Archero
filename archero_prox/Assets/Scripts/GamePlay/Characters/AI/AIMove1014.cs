




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1014 : AIMoveBase
{
	private EntityBase target;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	private float startTime;

	private float time;

	public AIMove1014(EntityBase entity, int time)
		: base(entity)
	{
		this.time = (float)time / 1000f;
	}

	protected override void OnInitBase()
	{
		target = GameLogic.Self;
		Find();
		startTime = Updater.AliveTime;
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		UpdateFindPath();
		AIMoving();
	}

	private void AIMoveStart()
	{
		UpdateMoveData();
		m_MoveData.action = "Skill";
		GameLogic.Hold.Sound.PlayMonsterSkill(5100001, m_Entity.position);
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void UpdateMoveData()
	{
		if (!target)
		{
			return;
		}
		if (findpath.Count > 0)
		{
			UpdateDirection();
			if ((nextpos - m_Entity.position).magnitude < 0.2f)
			{
				findpath.RemoveAt(0);
				if (findpath.Count > 0)
				{
					Grid.NodeItem nodeItem = findpath[0];
					nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(nodeItem.x, nodeItem.y);
					UpdateDirection();
				}
			}
		}
		else
		{
			nextpos = target.position;
			UpdateDirection();
		}
	}

	private void UpdateDirection()
	{
		float x = nextpos.x;
		Vector3 position = m_Entity.position;
		float x2 = x - position.x;
		float z = nextpos.z;
		Vector3 position2 = m_Entity.position;
		float num = z - position2.z;
		Vector3 direction = new Vector3(x2, 0f, num).normalized * 5f;
		m_MoveData.angle = Utils.getAngle(x2, num);
		m_MoveData.direction = direction;
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	private void UpdateFindPath()
	{
		if (Updater.AliveTime - startTime > time)
		{
			End();
		}
	}

	private void Find()
	{
		if ((bool)target)
		{
			findpath = GameLogic.Release.Path.FindingPath(m_Entity.position, target.position);
			if (findpath.Count > 0)
			{
				Grid.NodeItem nodeItem = findpath[0];
				nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(nodeItem.x, nodeItem.y);
				UpdateDirection();
			}
		}
	}

	private void AIMoving()
	{
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
