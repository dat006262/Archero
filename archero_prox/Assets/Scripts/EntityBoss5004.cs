




public class EntityBoss5004 : EntityBossBase
{
	private float up;

	protected override void OnInit()
	{
		base.OnInit();
		InitAfter();
		InitDivideID();
	}

	protected override long GetBossHP()
	{
		long maxHP = m_EntityData.MaxHP;
		long maxHP2 = GameLogic.GetMaxHP(3008);
		long maxHP3 = GameLogic.GetMaxHP(3009);
		long maxHP4 = GameLogic.GetMaxHP(3010);
		return maxHP + maxHP2 * 2 + maxHP3 * 2 * 2 + maxHP4 * 2 * 2 * 2;
	}

	private void InitAfter()
	{
		int num = 45 + 90 * GameLogic.Random(0, 4);
		m_AttackCtrl.SetRotate(num);
	}
}
