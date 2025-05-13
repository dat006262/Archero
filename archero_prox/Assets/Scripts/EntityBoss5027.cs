




public class EntityBoss5027 : EntityBossBase
{
	private float up;

	protected override void OnInit()
	{
		base.OnInit();
		InitDivideID();
		PlayEffect(3100021);
	}

	protected override long GetBossHP()
	{
		long maxHP = m_EntityData.MaxHP;
		long maxHP2 = GameLogic.GetMaxHP(3073);
		long maxHP3 = GameLogic.GetMaxHP(3074);
		return maxHP + maxHP2 * 6 + maxHP3 * 6 * 2;
	}
}
