




public class AI1803 : AIBase
{
	private EntityCallBase callentity;

	private int rerandomcount;

	protected override void OnInit()
	{
		callentity = (m_Entity as EntityCallBase);
		if (rerandomcount == 0)
		{
			AddAction(GetActionWait(string.Empty, 1000));
			AddActionDelegate(delegate
			{
				m_Entity.SetCollider(enable: true);
			});
		}
		AddAction(new AIMove1017(m_Entity, 600, 1000));
		AddAction(new AIMove1012(callentity));
		bReRandom = (rerandomcount == 0);
		rerandomcount++;
	}
}
