




namespace TableTool
{
	public class Stage_Level_chapter11Model : LocalModel<Stage_Level_chapter11, string>
	{
		private const string _Filename = "Stage_Level_chapter11";

		protected override string Filename => "Stage_Level_chapter11";

		protected override string GetBeanKey(Stage_Level_chapter11 bean)
		{
			return bean.RoomID;
		}
	}
}
