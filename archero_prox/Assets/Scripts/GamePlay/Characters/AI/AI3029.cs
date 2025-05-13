




public class AI3029 : AIGroundBase
{
	protected override void OnInit()
	{
		InitAI();
	}

	private void InitAI()
	{
		AddAction(new AIMove1027(m_Entity, 2));
		AddAction(GetActionAttacks(m_Entity.m_Data.WeaponID, 300, 600));
	}

	private ActionBase GetActionAttacks(int attackid, int attacktime, int attackmaxtime)
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq1016";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		actionSequence2.AddAction(GetActionAttack("attacksss", attackid));
		actionSequence2.AddAction(GetActionWaitRandom("waitsss", attacktime, attackmaxtime));
		return actionSequence2;
	}

	private bool Conditions()
	{
		return GetIsAlive() && m_Entity.m_HatredTarget != null;
	}
}
