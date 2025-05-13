




public class AI3044 : AIGroundBase
{
	protected override void OnInit()
	{
		if (m_Entity.IsElite)
		{
			AddAction(GetActionAttacks(m_Entity.m_Data.WeaponID, 0, 0));
			AddAction(GetActionAttacks(1098, 0, 0));
		}
		else
		{
			AddAction(GetActionAttacks(m_Entity.m_Data.WeaponID, 1000, 1000));
		}
		AddAction(new AIMove1026(m_Entity, 4));
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
