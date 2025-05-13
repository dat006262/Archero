




public class BossJumpHit5005 : SkillAloneAttrGoodBase
{
	protected override void TriggerEnter(EntityBase entity)
	{
		int num = -(int)((float)m_Entity.m_EntityData.GetAttack(20) * args[0]);
		GameLogic.SendHit_Body(entity, m_Entity, num, 4100001);
	}
}
