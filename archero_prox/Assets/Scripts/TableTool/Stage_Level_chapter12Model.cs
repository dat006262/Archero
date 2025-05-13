




namespace TableTool
{
	public class Stage_Level_chapter12Model : LocalModel<Stage_Level_chapter12, string>
	{
		private const string _Filename = "Stage_Level_chapter12";

		protected override string Filename => "Stage_Level_chapter12";

		protected override string GetBeanKey(Stage_Level_chapter12 bean)
		{
			return bean.RoomID;
		}
	}
}
