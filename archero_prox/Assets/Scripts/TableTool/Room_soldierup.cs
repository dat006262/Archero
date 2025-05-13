




namespace TableTool
{
	public class Room_soldierup : LocalBean
	{
		public int RoomID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public string[] Attributes
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			RoomID = readInt();
			Notes = readLocalString();
			Attributes = readArraystring();
			return true;
		}

		public Room_soldierup Copy()
		{
			Room_soldierup room_soldierup = new Room_soldierup();
			room_soldierup.RoomID = RoomID;
			room_soldierup.Notes = Notes;
			room_soldierup.Attributes = Attributes;
			return room_soldierup;
		}
	}
}
