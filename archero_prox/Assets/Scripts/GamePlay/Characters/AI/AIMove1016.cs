




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1016 : AIMoveBase
{
	private EntityBase target;

	private List<Grid.NodeItem> findpath;

	private Vector3 nextpos;

	public AIMove1016(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
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

	private void AIMoveStart()
	{
		m_Entity.m_MoveCtrl.AIMoveStart(m_MoveData);
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
		if (!target)
		{
			return;
		}
		List<Vector2Int> list = new List<Vector2Int>();
		Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(target.position);
		int[,] findPathRect = GameLogic.Release.MapCreatorCtrl.GetFindPathRect();
		int width = GameLogic.Release.MapCreatorCtrl.width;
		int height = GameLogic.Release.MapCreatorCtrl.height;
		if ((float)roomXY.x <= (float)width / 2f && (float)roomXY.y <= (float)height / 2f)
		{
			int i = width / 2 + 1;
			for (int num = width; i < num; i++)
			{
				int j = height / 2 + 1;
				for (int num2 = height; j < num2; j++)
				{
					if (findPathRect[i, j] == 0)
					{
						list.Add(new Vector2Int(i, j));
					}
				}
			}
		}
		else if ((float)roomXY.x <= (float)width / 2f && (float)roomXY.y > (float)height / 2f)
		{
			int k = width / 2 + 1;
			for (int num3 = width; k < num3; k++)
			{
				int l = 0;
				for (int num4 = height / 2; l < num4; l++)
				{
					if (findPathRect[k, l] == 0)
					{
						list.Add(new Vector2Int(k, l));
					}
				}
			}
		}
		else if ((float)roomXY.x > (float)width / 2f && (float)roomXY.y <= (float)height / 2f)
		{
			int m = 0;
			for (int num5 = width / 2; m < num5; m++)
			{
				int n = height / 2 + 1;
				for (int num6 = height; n < num6; n++)
				{
					if (findPathRect[m, n] == 0)
					{
						list.Add(new Vector2Int(m, n));
					}
				}
			}
		}
		else
		{
			int num7 = 0;
			for (int num8 = width / 2; num7 < num8; num7++)
			{
				int num9 = 0;
				for (int num10 = height / 2; num9 < num10; num9++)
				{
					if (findPathRect[num7, num9] == 0)
					{
						list.Add(new Vector2Int(num7, num9));
					}
				}
			}
		}
		int index = GameLogic.Random(0, list.Count);
		Vector2Int vector2Int = list[index];
		nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(vector2Int.x, vector2Int.y);
		findpath = GameLogic.Release.Path.FindingPath(m_Entity.position, nextpos);
		if (findpath.Count > 0)
		{
			Grid.NodeItem nodeItem = findpath[0];
			nextpos = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(nodeItem.x, nodeItem.y);
			UpdateDirection();
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
