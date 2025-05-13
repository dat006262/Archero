




public class AI1802 : AIBase
{
	private EntityCallBase callentity;

	protected override void OnInit()
	{
		callentity = (m_Entity as EntityCallBase);
		AddAction(new AIMove1003(callentity));
		AddAction(new AIMove1005(callentity));
	}
}
