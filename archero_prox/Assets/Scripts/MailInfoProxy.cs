




using GameProtocol;
using PureMVC.Patterns;

public class MailInfoProxy : Proxy
{
	public enum EMailPopType
	{
		eNormal,
		eMain
	}

	public class Transfer
	{
		public CMailInfo data;

		public MailOneCtrl ctrl;

		public EMailPopType poptype;
	}

	public new const string NAME = "MailInfoProxy";

	public MailInfoProxy(object data)
		: base("MailInfoProxy", data)
	{
	}
}
