




namespace TableTool
{
	public class Stage_Level_chapter5Model : LocalModel<Stage_Level_chapter5, string>
	{
		private const string _Filename = "Stage_Level_chapter5";

		protected override string Filename => "Stage_Level_chapter5";

		protected override string GetBeanKey(Stage_Level_chapter5 bean)
		{
			return bean.RoomID;
		}
	}
}
