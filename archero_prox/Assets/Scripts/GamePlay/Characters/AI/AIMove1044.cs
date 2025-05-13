




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1044 : AIMoveBase
{
	private EntityBase target;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	private float starttime;

	private const float MOVEADD = 3f;

	private const float MOVEADD_TIME = 1f;

	private float mMoveAdd;

	private Vector2Int mMoveDir;

	private bool bSameLine;

	public AIMove1044(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		target = GameLogic.Self;
		bSameLine = false;
		mMoveAdd = 1f;
		Find();
		AIMoveStart();
	}

	protected override void OnUpdate()
	{
		if (!bSameLine)
		{
			Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(m_Entity.position);
			Vector2Int roomXY2 = GameLogic.Release.MapCreatorCtrl.GetRoomXY(target.position);
			if (GameLogic.Release.MapCreatorCtrl.ExcuteRelativeDirection(roomXY, roomXY2, out mMoveDir, m_Entity.GetFlying()))
			{
				bSameLine = true;
				UpdateSprint(mMoveDir);
			}
			else
			{
				MoveNormal();
			}
		}
		else if (Updater.AliveTime - starttime < 1f)
		{
			AIMoving();
			Vector2Int roomXY3 = GameLogic.Release.MapCreatorCtrl.GetRoomXY(m_Entity.position);
			if (!m_Entity.GetFlying())
			{
				if (!GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY3 + mMoveDir))
				{
					End();
				}
				return;
			}
			Vector2Int v = roomXY3 + mMoveDir;
			if (!GameLogic.Release.MapCreatorCtrl.IsValid(v))
			{
				End();
			}
		}
		else
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
			Find();
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
		m_MoveData.direction = normalized * mMoveAdd;
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	private void UpdateSprint(Vector2Int dir)
	{
		starttime = Updater.AliveTime;
		m_Entity.m_AniCtrl.SendEvent("Continuous");
		mMoveAdd = 3f;
		Vector3 a = new Vector3(dir.x, 0f, dir.y);
		m_MoveData.angle = Utils.getAngle(dir.x, dir.y);
		m_MoveData.direction = a * mMoveAdd;
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	private void Find()
	{
		if ((bool)target)
		{
			Vector3 e = GameLogic.Release.MapCreatorCtrl.RandomPosition();
			findpath = GameLogic.Release.Path.FindingPath(m_Entity.position, e);
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
		m_Entity.m_AniCtrl.SendEvent("Idle", force: true);
	}
}
