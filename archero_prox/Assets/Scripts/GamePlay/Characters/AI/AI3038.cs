




public class AI3038 : AIBase
{
	private float moveMaxDis = 4f;

	protected override void OnInit()
	{
		AddAction(new AIMove1033(m_Entity, moveMaxDis, 1, move2target: true));
		AddAction(GetActionWait(string.Empty, 400));
		AddAction(GetActionAttack(string.Empty, m_Entity.m_Data.WeaponID));
		AddAction(GetActionWait(string.Empty, 100));
	}

	protected override void OnAIDeInit()
	{
	}
}
