




namespace TableTool
{
	public class Stage_Level_chapter7Model : LocalModel<Stage_Level_chapter7, string>
	{
		private const string _Filename = "Stage_Level_chapter7";

		protected override string Filename => "Stage_Level_chapter7";

		protected override string GetBeanKey(Stage_Level_chapter7 bean)
		{
			return bean.RoomID;
		}
	}
}
