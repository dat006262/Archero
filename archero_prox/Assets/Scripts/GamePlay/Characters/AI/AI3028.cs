




public class AI3028 : AIGroundBase
{
	protected override void OnInit()
	{
		int num = 1;
		for (int i = 0; i < num; i++)
		{
			AddAction(GetActionAttacks(m_Entity.m_Data.WeaponID, 400, 700));
		}
		AddAction(new AIMove1026(m_Entity, 4));
		bReRandom = true;
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
