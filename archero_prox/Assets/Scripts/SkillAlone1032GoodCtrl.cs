




public class SkillAlone1032GoodCtrl : SkillAloneAttrGoodBase
{
	protected override void TriggerEnter(EntityBase entity)
	{
		GameLogic.SendBuff(entity, m_Entity, 1017);
	}
}
