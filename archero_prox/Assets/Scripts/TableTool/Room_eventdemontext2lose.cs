




namespace TableTool
{
	public class Room_eventdemontext2lose : LocalBean
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

		public string Content1
		{
			get;
			private set;
		}

		public string Content2
		{
			get;
			private set;
		}

		public string Image1
		{
			get;
			private set;
		}

		public int LoseID
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			EventID = readInt();
			Notes = readLocalString();
			Content1 = readLocalString();
			Content2 = readLocalString();
			Image1 = readLocalString();
			LoseID = readInt();
			return true;
		}

		public Room_eventdemontext2lose Copy()
		{
			Room_eventdemontext2lose room_eventdemontext2lose = new Room_eventdemontext2lose();
			room_eventdemontext2lose.EventID = EventID;
			room_eventdemontext2lose.Notes = Notes;
			room_eventdemontext2lose.Content1 = Content1;
			room_eventdemontext2lose.Content2 = Content2;
			room_eventdemontext2lose.Image1 = Image1;
			room_eventdemontext2lose.LoseID = LoseID;
			return room_eventdemontext2lose;
		}
	}
}
