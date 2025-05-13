




namespace TableTool
{
	public class Stage_Level_chapter4Model : LocalModel<Stage_Level_chapter4, string>
	{
		private const string _Filename = "Stage_Level_chapter4";

		protected override string Filename => "Stage_Level_chapter4";

		protected override string GetBeanKey(Stage_Level_chapter4 bean)
		{
			return bean.RoomID;
		}
	}
}
