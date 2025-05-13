




public class AI1804 : AIBase
{
	protected override void OnInit()
	{
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.name = "actionseq";
		actionSequence.m_Entity = m_Entity;
		ActionSequence actionSequence2 = actionSequence;
		ActionBase actionAttack = GetActionAttack("attack", m_Entity.m_Data.WeaponID);
		actionAttack.ConditionBase = (() => m_Entity.m_HatredTarget != null);
		actionSequence2.AddAction(actionAttack);
		actionSequence2.AddAction(GetActionWaitRandom("actionwait", 500, 500));
		AddAction(actionSequence2);
	}
}
