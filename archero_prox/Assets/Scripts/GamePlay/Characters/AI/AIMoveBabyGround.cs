




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveBabyGround : AIMoveBase
{
	private EntityBase mParent;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	private float findTime;

	private float findDelay;

	private bool bUpdateTime = true;

	private float updatetime;

	private float starttime;

	private float range;

	private float randomrange;

	private float movespeed;

	private int groundindex;

	public AIMoveBabyGround(EntityBase entity, int groundindex, float movespeed, float range)
		: base(entity)
	{
		this.groundindex = groundindex;
		this.range = range;
		this.movespeed = movespeed;
		if (entity is EntityCallBase)
		{
			EntityCallBase entityCallBase = entity as EntityCallBase;
			if (entityCallBase != null)
			{
				mParent = entityCallBase.GetParent();
			}
		}
	}

	protected override void OnInitBase()
	{
		randomrange = GameLogic.Random(range * 0.8f, range);
		if (mParent == null || Vector3.Distance(m_Entity.position, mParent.position) < randomrange)
		{
			End();
			return;
		}
		findDelay = GameLogic.Random(0.5f, 0.7f);
		Find();
		findTime = Updater.AliveTime;
		starttime = Updater.AliveTime;
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		MoveNormal();
		if (Vector3.Distance(m_Entity.position, mParent.position) < randomrange)
		{
			End();
		}
	}

	private void MoveNormal()
	{
		UpdateMoveData();
		AIMoving();
	}

	private void AIMoveStart()
	{
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void UpdateMoveData()
	{
		if (!mParent)
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
		m_MoveData.direction = normalized * movespeed;
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
		if ((bool)mParent)
		{
			findpath = GameLogic.Release.Path.FindingPath(m_Entity.position, mParent.position + mParent.GetBabyGroundPos(groundindex));
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
