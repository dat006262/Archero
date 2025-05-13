




using UnityEngine;

public class EntityPartBody1803 : EntityPartBodyBase
{
	private void OnTriggerEnter(Collider o)
	{
		GameObject gameObject = null;
		if ((bool)o)
		{
			gameObject = o.gameObject;
		}
		if (gameObject.layer == LayerManager.Player || gameObject.layer == LayerManager.Fly)
		{
			EntityBase entityByChild = GameLogic.Release.Entity.GetEntityByChild(gameObject);
			if (!GameLogic.IsSameTeam(this, entityByChild))
			{
				int num = -m_EntityData.GetBodyHit();
				GameLogic.SendHit_Body(entityByChild, this, num, 4100001);
				GameLogic.Release.Entity.RemovePartBody(this);
			}
		}
	}
}
