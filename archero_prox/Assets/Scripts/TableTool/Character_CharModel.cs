




namespace TableTool
{
	public class Character_CharModel : LocalModel<Character_Char, int>
	{
		private const string _Filename = "Character_Char";

		protected override string Filename => "Character_Char";

		protected override int GetBeanKey(Character_Char bean)
		{
			return bean.CharID;
		}
	}
}
