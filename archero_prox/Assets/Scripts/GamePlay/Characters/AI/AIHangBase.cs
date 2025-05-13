




public class AIHangBase : AIBase
{
	protected override void OnInit()
	{
		mRoomTime = -1f;
		AddAction(new AIMove1001(m_Entity));
	}

	protected override void OnAIDeInit()
	{
	}
}
