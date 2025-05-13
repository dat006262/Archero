




using UnityEngine;

public class AI5026 : AIBase
{
	protected override void OnInit()
	{
		ActionChoose actionChoose = new ActionChoose();
		actionChoose.name = "actionchoose";
		actionChoose.m_Entity = m_Entity;
		actionChoose.Condition = GetCanMoveToHatred;
		ActionSequence actionSequence = new ActionSequence();
		actionSequence.m_Entity = m_Entity;
		actionSequence.name = "actionseqtrue";
		actionSequence.AddAction(new AIMove1047(m_Entity));
		actionSequence.AddAction(new AIMove1006(m_Entity, 1800, 2500));
		actionChoose.ResultTrue = actionSequence;
		ActionSequence actionSequence2 = new ActionSequence();
		actionSequence2.m_Entity = m_Entity;
		actionSequence2.name = "actionseq";
		actionSequence2.AddAction(GetActionWaitRandom("actionwaitrandom1", 300, 600));
		actionSequence2.AddAction(new AIMove1002(m_Entity, 800, 1000));
		actionChoose.ResultFalse = actionSequence2;
		ActionSequence actionSequence3 = new ActionSequence();
		actionSequence3.m_Entity = m_Entity;
		actionSequence3.name = "actionseq1";
		actionSequence3.AddAction(actionChoose);
		AddAction(actionSequence3);
	}

	protected override void OnAIDeInit()
	{
	}

	private bool GetCanMoveToHatred()
	{
		return (bool)m_Entity.m_HatredTarget && Vector3.Distance(m_Entity.m_HatredTarget.position, m_Entity.position) > 0.5f;
	}
}
