




using UnityEngine;

public class SkillCreate1022 : SkillCreateBase
{
	private Transform mCloud;

	protected override void OnAwake()
	{
		mCloud = base.transform.Find("child/cloud");
	}

	protected override void OnInit(string[] args)
	{
		time = float.Parse(args[0]);
	}

	protected override void OnDeinit()
	{
	}

	private void OnTriggerEnter(Collider o)
	{
		if (o.gameObject.layer == LayerManager.Player)
		{
			EntityBase component = o.GetComponent<EntityBase>();
			if (!GameLogic.IsSameTeam(m_Entity, component))
			{
				GameLogic.SendBuff(component, m_Entity, 1031);
			}
		}
	}
}
