




using System.Collections.Generic;

public class SuperSkill1003 : SuperSkillBase
{
	protected override void OnInit()
	{
	}

	protected override void OnDeInit()
	{
	}

	protected override void OnUseSkill()
	{
		AllEntitiesAddBuff(1024);
		GameNode.CameraShake(CameraShakeType.FirstDrop);
	}

	public void AllEntitiesAddBuff(int buffid)
	{
		List<EntityBase> entities = GameLogic.Release.Entity.GetEntities();
		int i = 0;
		for (int count = entities.Count; i < count; i++)
		{
			EntityBase entityBase = entities[i];
			if ((bool)entityBase && entityBase.gameObject.activeInHierarchy && !entityBase.GetIsDead() && !GameLogic.IsSameTeam(base.m_Entity, entityBase))
			{
				GameLogic.SendBuff(entityBase, base.m_Entity, buffid);
				entityBase.PlayEffect(1101003);
			}
		}
	}
}
