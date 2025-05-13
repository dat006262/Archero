




namespace TableTool
{
	public class Room_eventgameturn : LocalBean
	{
		public int EventID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public int GetID
		{
			get;
			private set;
		}

		public int Weight
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			EventID = readInt();
			Notes = readLocalString();
			GetID = readInt();
			Weight = readInt();
			return true;
		}

		public Room_eventgameturn Copy()
		{
			Room_eventgameturn room_eventgameturn = new Room_eventgameturn();
			room_eventgameturn.EventID = EventID;
			room_eventgameturn.Notes = Notes;
			room_eventgameturn.GetID = GetID;
			room_eventgameturn.Weight = Weight;
			return room_eventgameturn;
		}
	}
}
