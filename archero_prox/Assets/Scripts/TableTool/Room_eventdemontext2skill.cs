




namespace TableTool
{
	public class Room_eventdemontext2skill : LocalBean
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

		public int[] Loses
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
			Loses = readArrayint();
			GetID = readInt();
			Weight = readInt();
			return true;
		}

		public Room_eventdemontext2skill Copy()
		{
			Room_eventdemontext2skill room_eventdemontext2skill = new Room_eventdemontext2skill();
			room_eventdemontext2skill.EventID = EventID;
			room_eventdemontext2skill.Notes = Notes;
			room_eventdemontext2skill.Loses = Loses;
			room_eventdemontext2skill.GetID = GetID;
			room_eventdemontext2skill.Weight = Weight;
			return room_eventdemontext2skill;
		}
	}
}
