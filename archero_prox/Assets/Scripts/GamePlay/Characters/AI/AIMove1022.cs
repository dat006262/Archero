




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1022 : AIMoveBase
{
	private EntityBase target;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	private float findTime;

	private float findDelay;

	private Vector3 mDirection;

	private float addspeed = 0.3f;

	private float maxspeed = 2f;

	private float movetime;

	private bool bNear;

	private float neardelaytime;

	private float NearPos;

	private Vector3 dir;

	public AIMove1022(EntityBase entity, float nearpos)
		: base(entity)
	{
		NearPos = nearpos;
	}

	protected override void OnInitBase()
	{
		target = GameLogic.Self;
		bNear = false;
		movetime = 0f;
		neardelaytime = 0.2f;
		findDelay = GameLogic.Random(0.5f, 0.7f);
		Find();
		findTime = Updater.AliveTime;
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		if (bNear)
		{
			neardelaytime -= Updater.delta;
			if (neardelaytime <= 0f)
			{
				End();
			}
		}
		else if (IsNear())
		{
			AIMoveEnd();
			bNear = true;
		}
		else
		{
			MoveNormal();
		}
	}

	private void MoveNormal()
	{
		movetime += Updater.delta;
		UpdateFindPath();
		UpdateMoveData();
		UpdateMoveSpeed();
		AIMoving();
	}

	private bool IsNear()
	{
		dir = target.position - m_Entity.position;
		bool flag = Physics.Raycast(m_Entity.position, dir, dir.magnitude, (1 << LayerManager.Stone) | (1 << LayerManager.Waters));
		return (bool)target && (target.position - m_Entity.position).magnitude < NearPos && !flag;
	}

	private void AIMoveStart()
	{
		UpdateMoveData();
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private void UpdateMoveSpeed()
	{
		float num = movetime * addspeed + 1f;
		num = MathDxx.Clamp(num, num, maxspeed);
		m_MoveData._moveDirection = m_MoveData._moveDirection.normalized * num;
	}

	private void UpdateMoveData()
	{
		if (!target)
		{
			return;
		}
		if (findpath.Count > 0)
		{
			Grid.NodeItem nodeItem = findpath[0];
			nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(nodeItem.x, nodeItem.y);
			UpdateDirection();
			if ((nextpos - m_Entity.position).magnitude < 0.5f)
			{
				findpath.RemoveAt(0);
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
		m_MoveData._moveDirection = normalized;
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
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}

	protected override void OnEnd()
	{
	}
}
