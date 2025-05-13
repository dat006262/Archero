




using UnityEngine;

public class SkillAlone1035 : SkillAlonePartBase
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
		if ((bool)partbody && (bool)deadentity)
		{
			Vector3 position = deadentity.position;
			partbody.SetPosition(position);
		}
	}
}
