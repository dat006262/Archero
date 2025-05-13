




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1011 : AIMoveBase
{
	private EntityBase target;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	private float findTime;

	private float findDelay;

	private bool bUpdateTime;

	private float updatetime;

	private float starttime;

	public AIMove1011(EntityBase entity, int min = 0, int max = 0)
		: base(entity)
	{
		bUpdateTime = (min > 0);
		if (bUpdateTime)
		{
			if (max == 0)
			{
				max = min;
			}
			updatetime = (float)GameLogic.Random(min, max) / 1000f;
		}
	}

	protected override void OnInitBase()
	{
		target = GameLogic.Self;
		findDelay = GameLogic.Random(0.5f, 0.7f);
		Find();
		findTime = Updater.AliveTime;
		starttime = Updater.AliveTime;
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		if (bUpdateTime && Updater.AliveTime - starttime >= updatetime)
		{
			End();
		}
		else
		{
			MoveNormal();
		}
	}

	private void MoveNormal()
	{
		UpdateFindPath();
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
			findTime = Updater.AliveTime;
			Find();
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

	private void AIMoveEnd()
	{
		End();
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
