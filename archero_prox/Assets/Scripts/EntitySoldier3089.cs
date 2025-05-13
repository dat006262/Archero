




public class EntitySoldier3089 : EntityMonsterBase
{
	private void InitAfter()
	{
		int num = 45 + 90 * GameLogic.Random(0, 4);
		m_AttackCtrl.SetRotate(num);
	}
}
