




namespace TableTool
{
	public class Room_eventangelskill : LocalBean
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

		public Room_eventangelskill Copy()
		{
			Room_eventangelskill room_eventangelskill = new Room_eventangelskill();
			room_eventangelskill.EventID = EventID;
			room_eventangelskill.Notes = Notes;
			room_eventangelskill.GetID = GetID;
			room_eventangelskill.Weight = Weight;
			return room_eventangelskill;
		}
	}
}
