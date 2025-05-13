




public class AI3084 : AIBase
{
	protected override void OnInit()
	{
		AddAction(GetActionWaitRandom(string.Empty, 800, 1600));
		AddAction(new AIMove1035(m_Entity, 4, 7f, 0.2f));
		AddAction(GetActionWaitRandom(string.Empty, 600, 1000));
	}
}
