




public class EntityBoss5014 : EntityBossBase
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
		long maxHP2 = GameLogic.GetMaxHP(3058);
		long maxHP3 = GameLogic.GetMaxHP(3059);
		long maxHP4 = GameLogic.GetMaxHP(3060);
		return maxHP + maxHP2 * 2 + maxHP3 * 2 * 2 + maxHP4 * 2 * 2 * 2;
	}

	private void InitAfter()
	{
		int num = 45 + 90 * GameLogic.Random(0, 4);
		m_AttackCtrl.SetRotate(num);
	}
}
