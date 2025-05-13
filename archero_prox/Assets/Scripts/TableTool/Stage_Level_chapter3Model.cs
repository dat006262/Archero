




namespace TableTool
{
	public class Stage_Level_chapter3Model : LocalModel<Stage_Level_chapter3, string>
	{
		private const string _Filename = "Stage_Level_chapter3";

		protected override string Filename => "Stage_Level_chapter3";

		protected override string GetBeanKey(Stage_Level_chapter3 bean)
		{
			return bean.RoomID;
		}
	}
}
