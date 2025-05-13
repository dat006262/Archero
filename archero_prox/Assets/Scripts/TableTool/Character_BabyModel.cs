




namespace TableTool
{
	public class Character_BabyModel : LocalModel<Character_Baby, string>
	{
		private const string _Filename = "Character_Baby";

		protected override string Filename => "Character_Baby";

		protected override string GetBeanKey(Character_Baby bean)
		{
			return bean.BabyID;
		}
	}
}
