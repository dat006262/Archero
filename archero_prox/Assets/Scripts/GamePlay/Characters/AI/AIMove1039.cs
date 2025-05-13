




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1039 : AIMoveBase
{
	private EntityBase target;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	private Vector3 endpos;

	private int range;

	public AIMove1039(EntityBase entity, int range)
		: base(entity)
	{
		this.range = range;
	}

	protected override void OnInitBase()
	{
		GameLogic.Release.MapCreatorCtrl.RandomItemSide(m_Entity, range, out float endx, out float endz);
		endpos = new Vector3(endx, 0f, endz);
		target = GameLogic.Self;
		Find();
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		UpdateMoveData();
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
			if ((nextpos - m_Entity.position).magnitude < 0.5f)
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
			End();
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

	private void Find()
	{
		if ((bool)target)
		{
			findpath = GameLogic.Release.Path.FindingPath(m_Entity.position, endpos);
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
