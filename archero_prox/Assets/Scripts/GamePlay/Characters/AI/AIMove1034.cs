




using Dxx.Util;
using UnityEngine;

public class AIMove1034 : AIMoveBase
{
	private EntityBase target;

	private int offsetx;

	private int offsety;

	private int move2player = 50;

	private Vector3 endpos;

	private float movedis;

	private float alldis;

	public AIMove1034(EntityBase entity)
		: base(entity)
	{
		target = GameLogic.Self;
	}

	protected override void OnInitBase()
	{
		movedis = 0f;
		Vector2Int v = GameLogic.Release.MapCreatorCtrl.RandomItemSide(m_Entity);
		if (GetMove2Player())
		{
			v = UpdateMove2Player();
		}
		Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(m_Entity.position);
		offsetx = v.x - roomXY.x;
		offsety = v.y - roomXY.y;
		for (int i = 0; i < 2; i++)
		{
			if (!GameLogic.Release.MapCreatorCtrl.IsEmpty(new Vector2Int(v.x + offsetx, v.y + offsety)))
			{
				break;
			}
			v.x += offsetx;
			v.y += offsety;
		}
		endpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(v);
		UpdateDirection();
		alldis = (endpos - m_Entity.position).magnitude;
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
	}

	private Vector2Int UpdateMove2Player()
	{
		Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(m_Entity.position);
		float angle = Utils.getAngle(target.position - m_Entity.position);
		if ((angle >= 337.5f || angle <= 22.5f) && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(0, -1)))
		{
			return roomXY + new Vector2Int(0, -1);
		}
		if (angle >= 22.5f && angle <= 67.5f && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(1, -1)))
		{
			return roomXY + new Vector2Int(1, -1);
		}
		if (angle >= 67.5f && angle <= 112.5f && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(1, 0)))
		{
			return roomXY + new Vector2Int(1, 0);
		}
		if (angle >= 112.5f && angle <= 157.5f && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(1, 1)))
		{
			return roomXY + new Vector2Int(1, 1);
		}
		if (angle >= 157.5f && angle <= 202.5f && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(0, 1)))
		{
			return roomXY + new Vector2Int(0, 1);
		}
		if (angle >= 202.5f && angle <= 247.5f && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(-1, 1)))
		{
			return roomXY + new Vector2Int(-1, 1);
		}
		if (angle >= 247.5f && angle <= 292.5f && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(-1, 0)))
		{
			return roomXY + new Vector2Int(-1, 0);
		}
		if (angle >= 292.5f && angle <= 337.5f && GameLogic.Release.MapCreatorCtrl.IsEmpty(roomXY + new Vector2Int(-1, -1)))
		{
			return roomXY + new Vector2Int(-1, -1);
		}
		return GameLogic.Release.MapCreatorCtrl.RandomItemSide(m_Entity);
	}

	private void UpdateDirection()
	{
		float x = endpos.x;
		Vector3 position = m_Entity.position;
		float x2 = x - position.x;
		float z = endpos.z;
		Vector3 position2 = m_Entity.position;
		float num = z - position2.z;
		m_MoveData.angle = Utils.getAngle(x2, num);
		m_MoveData.direction = new Vector3(x2, 0f, num).normalized;
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	protected override void OnUpdate()
	{
		float num = m_Entity.m_EntityData.GetSpeed() * Updater.delta;
		if (movedis + num > alldis)
		{
			num = alldis - movedis;
		}
		movedis += num;
		m_Entity.m_MoveCtrl.AIMoving(m_MoveData);
		if (movedis >= alldis)
		{
			End();
		}
	}

	private bool GetMove2Player()
	{
		return GameLogic.Random(0, 100) < move2player;
	}

	protected override void OnEnd()
	{
		m_Entity.m_MoveCtrl.AIMoveEnd(m_MoveData);
	}
}
