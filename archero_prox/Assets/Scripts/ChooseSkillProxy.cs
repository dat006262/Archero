




using PureMVC.Patterns;

public class ChooseSkillProxy : Proxy
{
	public enum ChooseSkillType
	{
		eLevel,
		eFirst
	}

	public class Transfer
	{
		public ChooseSkillType type;
	}

	public new const string NAME = "ChooseSkillProxy";

	public ChooseSkillProxy(object data)
		: base("ChooseSkillProxy", data)
	{
	}
}
