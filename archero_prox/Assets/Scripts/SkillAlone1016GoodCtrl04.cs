




public class SkillAlone1016GoodCtrl04 : SkillAlone1016GoodCtrl01
{
	protected override void TriggerEnter(EntityBase entity)
	{
		float num = args[0];
		int buffid = (int)args[1];
		GameLogic.SendBuff(entity, m_Entity, buffid, num);
		long num2 = (long)((float)m_Entity.m_EntityData.GetAttackBase() * num);
		GameLogic.SendHit_Skill(entity, -num2);
	}
}
