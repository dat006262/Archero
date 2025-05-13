




namespace TableTool
{
	public class Buff_alone : LocalBean
	{
		public int BuffID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public int FxId
		{
			get;
			private set;
		}

		public int OverType
		{
			get;
			private set;
		}

		public int BuffType
		{
			get;
			private set;
		}

		public int DizzyChance
		{
			get;
			private set;
		}

		public string Attribute
		{
			get;
			private set;
		}

		public string[] FirstEffects
		{
			get;
			private set;
		}

		public string[] Effects
		{
			get;
			private set;
		}

		public string[] Attributes
		{
			get;
			private set;
		}

		public float[] Args
		{
			get;
			private set;
		}

		public string ArgsContent
		{
			get;
			private set;
		}

		public int Time
		{
			get;
			private set;
		}

		public int Delay_time
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			BuffID = readInt();
			Notes = readLocalString();
			FxId = readInt();
			OverType = readInt();
			BuffType = readInt();
			DizzyChance = readInt();
			Attribute = readLocalString();
			FirstEffects = readArraystring();
			Effects = readArraystring();
			Attributes = readArraystring();
			Args = readArrayfloat();
			ArgsContent = readLocalString();
			Time = readInt();
			Delay_time = readInt();
			return true;
		}

		public Buff_alone Copy()
		{
			Buff_alone buff_alone = new Buff_alone();
			buff_alone.BuffID = BuffID;
			buff_alone.Notes = Notes;
			buff_alone.FxId = FxId;
			buff_alone.OverType = OverType;
			buff_alone.BuffType = BuffType;
			buff_alone.DizzyChance = DizzyChance;
			buff_alone.Attribute = Attribute;
			buff_alone.FirstEffects = FirstEffects;
			buff_alone.Effects = Effects;
			buff_alone.Attributes = Attributes;
			buff_alone.Args = Args;
			buff_alone.ArgsContent = ArgsContent;
			buff_alone.Time = Time;
			buff_alone.Delay_time = Delay_time;
			return buff_alone;
		}
	}
}
