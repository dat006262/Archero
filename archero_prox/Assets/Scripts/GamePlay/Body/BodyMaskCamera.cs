




using Dxx.Util;
using UnityEngine;

public class BodyMaskCamera
{
	private EntityBase m_Entity;

	private float updateTime = 0.1f;

	private float currentTime;

	private RoomState state = RoomState.Runing;

	protected virtual float Width => GameLogic.Width;

	protected virtual float Height => GameLogic.Height;

	public BodyMaskCamera(EntityBase entity)
	{
		m_Entity = entity;
		if (!m_Entity.IsSelf)
		{
			Updater.AddUpdate(Utils.FormatString("{0}.BodyMaskCamera", m_Entity.m_EntityData.CharID), OnUpdate);
		}
	}

	public void DeInit()
	{
		Updater.RemoveUpdate(Utils.FormatString("{0}.BodyMaskCamera", m_Entity.m_EntityData.CharID), OnUpdate);
	}

	private void OnUpdate(float delta)
	{
		if (!m_Entity || !m_Entity.m_Body || GameLogic.Release.Game.RoomState != RoomState.Runing)
		{
			return;
		}
		if (state == RoomState.Throughing)
		{
			currentTime = -10f;
		}
		state = GameLogic.Release.Game.RoomState;
		if (!(Updater.AliveTime - currentTime > updateTime))
		{
			return;
		}
		Vector3 vector = Utils.World2Screen(m_Entity.m_Body.transform.position);
		if (vector.x < 0f || vector.x > Width || vector.y < 0f || vector.y > Height)
		{
			if (m_Entity.m_Body.GetIsInCamera())
			{
				m_Entity.m_Body.SetIsVislble(value: false);
			}
		}
		else if (!m_Entity.m_Body.GetIsInCamera())
		{
			m_Entity.m_Body.SetIsVislble(value: true);
		}
		currentTime = Updater.AliveTime;
	}
}
