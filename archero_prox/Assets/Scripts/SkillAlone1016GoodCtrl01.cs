




public class SkillAlone1016GoodCtrl01 : SkillAloneAttrGoodBase
{
	protected override void TriggerEnter(EntityBase entity)
	{
		if (!(m_Entity == null) && m_Entity.m_EntityData != null && !(entity == null))
		{
			float num = args[0];
			int buffid = (int)args[1];
			long num2 = (long)((float)m_Entity.m_EntityData.GetAttack(0) * num);
			GameLogic.SendHit_Skill(entity, -num2);
			GameLogic.SendBuff(entity, m_Entity, buffid);
			entity.PlayHittedSound();
		}
	}
}
