




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class AIMove1045 : AIMoveBase
{
	private EntityBase target;

	private float findrange;

	private Vector3 dir;

	private int flyframe = 20;

	private float g = 3f;

	private float endx;

	private float endz;

	private float perendx;

	private float perendz;

	private float delaytime;

	private float starttime;

	private float alltime = 0.6f;

	private float halftime;

	private Vector3 startpos;

	public AIMove1045(EntityBase entity, float findrange)
		: base(entity)
	{
		this.findrange = findrange;
	}

	protected override void OnInitBase()
	{
		delaytime = Updater.AliveTime + 0.15f;
		starttime = Updater.AliveTime;
		halftime = alltime / 2f;
		startpos = m_Entity.position;
		target = GameLogic.Self;
		if ((m_Entity.position - target.position).magnitude < findrange)
		{
			Vector3 position = target.position;
			endx = position.x;
			Vector3 position2 = target.position;
			endz = position2.z;
		}
		else
		{
			GameLogic.Release.MapCreatorCtrl.RandomItem(m_Entity, 3, out endx, out endz);
		}
		float num = endx;
		Vector3 position3 = m_Entity.position;
		perendx = (num - position3.x) / (float)flyframe;
		float num2 = endz;
		Vector3 position4 = m_Entity.position;
		perendz = (num2 - position4.z) / (float)flyframe;
		float angle = Utils.getAngle(perendx, perendz);
		m_Entity.m_AttackCtrl.RotateHero(angle);
		m_Entity.m_AniCtrl.SendEvent("Skill");
	}

	protected override void OnUpdate()
	{
		MoveNormal();
	}

	private void MoveNormal()
	{
		if (Updater.AliveTime >= delaytime)
		{
			OnFly();
		}
	}

	private void OnFly()
	{
		float num = 1f - (delaytime + alltime - Updater.AliveTime) / alltime;
		float num2 = -4f * g * (num - 0.5f) * (num - 0.5f) + g;
		if (Updater.AliveTime < delaytime + halftime)
		{
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num * (float)flyframe, startpos.y + num2, startpos.z + perendz * num * (float)flyframe));
			return;
		}
		if (Updater.AliveTime < delaytime + alltime)
		{
			m_Entity.SetPosition(new Vector3(startpos.x + perendx * num * (float)flyframe, startpos.y + num2, startpos.z + perendz * num * (float)flyframe));
			return;
		}
		EntityBase entity = m_Entity;
		Vector3 position = m_Entity.position;
		float x = position.x;
		Vector3 position2 = m_Entity.position;
		entity.SetPosition(new Vector3(x, 0f, position2.z));
		End();
	}

	private void RandomItem(out float endx, out float endz)
	{
		int[,] findPathRect = GameLogic.Release.MapCreatorCtrl.GetFindPathRect();
		int width = GameLogic.Release.MapCreatorCtrl.width;
		int height = GameLogic.Release.MapCreatorCtrl.height;
		Vector2Int roomXY = GameLogic.Release.MapCreatorCtrl.GetRoomXY(m_Entity.position);
		int num = 1;
		List<Vector2Int> list = new List<Vector2Int>();
		int i = roomXY.x - num;
		for (int num2 = roomXY.x + num + 1; i < num2; i++)
		{
			if (i < 0 || i >= width)
			{
				continue;
			}
			int j = roomXY.y - num;
			for (int num3 = roomXY.y + num + 1; j < num3; j++)
			{
				if (j >= 0 && j < height && (i != roomXY.x || j != roomXY.y) && (i == roomXY.x || j == roomXY.y) && findPathRect[i, j] == 0)
				{
					list.Add(new Vector2Int(i, j));
				}
			}
		}
		if (list.Count == 0)
		{
			Vector3 position = m_Entity.position;
			endx = position.x;
			Vector3 position2 = m_Entity.position;
			endz = position2.z;
		}
		else
		{
			int index = GameLogic.Random(0, list.Count);
			Vector3 worldPosition = GameLogic.Release.MapCreatorCtrl.GetWorldPosition(list[index]);
			endx = worldPosition.x;
			endz = worldPosition.z;
		}
	}
}
