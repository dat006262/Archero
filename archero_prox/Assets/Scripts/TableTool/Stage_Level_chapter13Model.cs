




namespace TableTool
{
	public class Stage_Level_chapter13Model : LocalModel<Stage_Level_chapter13, string>
	{
		private const string _Filename = "Stage_Level_chapter13";

		protected override string Filename => "Stage_Level_chapter13";

		protected override string GetBeanKey(Stage_Level_chapter13 bean)
		{
			return bean.RoomID;
		}
	}
}
