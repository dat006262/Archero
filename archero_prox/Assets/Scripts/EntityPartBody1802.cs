




using UnityEngine;

public class EntityPartBody1802 : EntityPartBodyBase
{
	protected override void OnDeInitLogic()
	{
		m_Parent.RemoveRotateFollow(this);
		base.OnDeInitLogic();
	}

	protected override void OnGotoNextRooms(RoomGenerateBase.Room room)
	{
		Vector3 position = m_Parent.position;
		float x = position.x + GameLogic.Random(-2f, 2f);
		Vector3 position2 = m_Parent.position;
		float z = position2.z + GameLogic.Random(0f, 2f);
		base.transform.position = new Vector3(x, 0f, z);
	}

	protected override void UpdateProcess(float delta)
	{
		base.UpdateProcess(delta);
	}

	private void OnTriggerEnter(Collider o)
	{
		if (o.gameObject.layer == LayerManager.Player || o.gameObject.layer == LayerManager.Fly)
		{
			EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(o.gameObject);
			if (!GameLogic.IsSameTeam(this, entityByChild))
			{
				int num = -m_EntityData.GetBodyHit();
				GameLogic.SendHit_Body(entityByChild, this, num, 4100001);
				GameLogic.Release.Entity.RemovePartBody(this);
			}
		}
	}
}
