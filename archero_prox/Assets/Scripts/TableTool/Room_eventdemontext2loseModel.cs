




namespace TableTool
{
	public class Room_eventdemontext2loseModel : LocalModel<Room_eventdemontext2lose, int>
	{
		private const string _Filename = "Room_eventdemontext2lose";

		protected override string Filename => "Room_eventdemontext2lose";

		protected override int GetBeanKey(Room_eventdemontext2lose bean)
		{
			return bean.EventID;
		}
	}
}
