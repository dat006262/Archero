




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1018 : AIMoveBase
{
	private EntityBase target;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	private float findTime;

	private float findDelay;

	private bool bUpdateTime;

	private int min;

	private int max;

	public AIMove1018(EntityBase entity, int min = 0, int max = 0)
		: base(entity)
	{
		bUpdateTime = (min > 0);
		if (bUpdateTime)
		{
			this.min = min;
			this.max = ((max != 0) ? max : min);
		}
	}

	protected override void OnInitBase()
	{
		target = GameLogic.Self;
		findDelay = 0.4f;
		Find();
		findTime = Updater.AliveTime;
		AIMoveStart();
		ConditionBase conditionRandomTime = AIMoveBase.GetConditionRandomTime(min, max);
		ConditionUpdate = conditionRandomTime.IsEnd;
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
		Vector3 normalized = new Vector3(x2, 0f, num).normalized;
		m_MoveData.angle = Utils.getAngle(x2, num);
		m_MoveData.direction = normalized;
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	private void UpdateFindPath()
	{
		if (Updater.AliveTime - findTime > findDelay)
		{
			Find();
			findTime = Updater.AliveTime;
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
