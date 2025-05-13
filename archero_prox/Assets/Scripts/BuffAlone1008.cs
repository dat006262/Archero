




public class BuffAlone1008 : BuffAloneBase
{
	protected override void OnStart()
	{
		GameLogic.Hold.Sound.PlayBattleSpecial(5000011, m_Entity.position);
	}

	protected override void OnRemove()
	{
	}
}
