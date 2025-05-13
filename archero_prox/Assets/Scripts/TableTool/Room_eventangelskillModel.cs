




namespace TableTool
{
	public class Room_eventangelskillModel : LocalModel<Room_eventangelskill, int>
	{
		private const string _Filename = "Room_eventangelskill";

		protected override string Filename => "Room_eventangelskill";

		protected override int GetBeanKey(Room_eventangelskill bean)
		{
			return bean.EventID;
		}
	}
}
