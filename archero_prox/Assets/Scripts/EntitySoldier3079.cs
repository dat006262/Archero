




public class EntitySoldier3079 : EntityMonsterBase
{
	protected override void StartInit()
	{
		base.StartInit();
		PlayEffect(3100022);
		m_Body.AddElement(EElementType.eFire);
	}

	protected override void OnHitEntity(EntityBase e)
	{
		GameLogic.SendBuff(e, this, 2004);
	}
}
