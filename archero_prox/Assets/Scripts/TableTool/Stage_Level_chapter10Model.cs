




namespace TableTool
{
	public class Stage_Level_chapter10Model : LocalModel<Stage_Level_chapter10, string>
	{
		private const string _Filename = "Stage_Level_chapter10";

		protected override string Filename => "Stage_Level_chapter10";

		protected override string GetBeanKey(Stage_Level_chapter10 bean)
		{
			return bean.RoomID;
		}
	}
}
