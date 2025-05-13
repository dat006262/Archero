




using System.Collections.Generic;

public class MailMediator : MediatorBase
{
	public new const string NAME = "MailMediator";

	public override List<string> OnListNotificationInterests
	{
		get
		{
			List<string> onListNotificationInterests = base.OnListNotificationInterests;
			onListNotificationInterests.Add("MailUI_MailUpdate");
			return onListNotificationInterests;
		}
	}

	public MailMediator()
		: base("MailUIPanel")
	{
	}
}
