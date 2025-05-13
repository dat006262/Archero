




namespace TableTool
{
	public class Room_eventgameturnModel : LocalModel<Room_eventgameturn, int>
	{
		private const string _Filename = "Room_eventgameturn";

		protected override string Filename => "Room_eventgameturn";

		protected override int GetBeanKey(Room_eventgameturn bean)
		{
			return bean.EventID;
		}
	}
}
