




public class AI1801 : AIBase
{
	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		ActionBase actionAttackSpecial = GetActionAttackSpecial("attack", m_Entity.m_Data.WeaponID);
		actionAttackSpecial.ConditionBase = (() => m_Entity.m_HatredTarget != null);
		actionSequence2.AddAction(actionAttackSpecial);
		actionSequence2.AddAction(GetActionWaitRandom("actionwait", 500, 500));
		AddAction(actionSequence2);
	}
}
