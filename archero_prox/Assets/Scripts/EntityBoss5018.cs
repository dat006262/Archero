




public class EntityBoss5018 : EntityBossBase
{
	private float up;

	protected override void OnInit()
	{
		base.OnInit();
		InitAfter();
	}

	protected override long GetBossHP()
	{
		long maxHP = m_EntityData.MaxHP;
		long maxHP2 = GameLogic.GetMaxHP(3061);
		long maxHP3 = GameLogic.GetMaxHP(3062);
		return maxHP + maxHP2 * 2 + maxHP3 * 2 * 3;
	}

	private void InitAfter()
	{
	}
}
