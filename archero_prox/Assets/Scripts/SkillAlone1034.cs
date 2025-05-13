




using UnityEngine;

public class SkillAlone1034 : SkillAlonePartBase
{
	protected override void OnInstall()
	{
		base.OnInstall();
	}

	protected override void OnUninstall()
	{
		base.OnUninstall();
	}

	protected override void OnDeadAction(EntityBase deadentity, EntityPartBodyBase partbody)
	{
		Vector3 position = m_Entity.position + new Vector3(GameLogic.Random(-2f, 2f), 0f, GameLogic.Random(-2f, 2f));
		if (partbody != null)
		{
			partbody.SetPosition(position);
			m_Entity.AddRotateFollow(partbody);
		}
	}
}
