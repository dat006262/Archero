




using System;
using UnityEngine;

public class Goods1005EffectCtrl : MonoBehaviour
{
	public Action<EntityBase> Event_TriggerEnter;

	private void OnTriggerEnter(Collider o)
	{
		if (o.gameObject.layer == LayerManager.Player && o.gameObject == GameLogic.Self.gameObject && Event_TriggerEnter != null)
		{
			Event_TriggerEnter(GameLogic.Self);
		}
	}
}
