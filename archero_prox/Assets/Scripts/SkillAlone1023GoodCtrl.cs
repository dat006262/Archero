




using Dxx.Util;

public class SkillAlone1023GoodCtrl : SkillAloneAttrGoodBase
{
	protected override void TriggerEnter(EntityBase entity)
	{
		int num = MathDxx.CeilToInt((float)m_Entity.m_EntityData.attribute.AttackValue.Value * args[0]);
		GameLogic.SendHit_Skill(entity, -num);
	}
}
