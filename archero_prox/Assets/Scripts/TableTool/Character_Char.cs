




namespace TableTool
{
	public class Character_Char : LocalBean
	{
		public int CharID
		{
			get;
			private set;
		}

		public int TypeID
		{
			get;
			private set;
		}

		public string ModelID
		{
			get;
			private set;
		}

		public float BodyScale
		{
			get;
			private set;
		}

		public string TextureID
		{
			get;
			private set;
		}

		public int WeaponID
		{
			get;
			private set;
		}

		public int Attackrangetype
		{
			get;
			private set;
		}

		public int Speed
		{
			get;
			private set;
		}

		public int HP
		{
			get;
			private set;
		}

		public int RotateSpeed
		{
			get;
			private set;
		}

		public int BodyAttack
		{
			get;
			private set;
		}

		public int Divide
		{
			get;
			private set;
		}

		public int[] Skills
		{
			get;
			private set;
		}

		public float BackRatio
		{
			get;
			private set;
		}

		public float[] ActionSpeed
		{
			get;
			private set;
		}

		public int HittedEffectID
		{
			get;
			private set;
		}

		public int DeadSoundID
		{
			get;
			private set;
		}

		public int Cache
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			CharID = readInt();
			TypeID = readInt();
			ModelID = readLocalString();
			BodyScale = readFloat();
			TextureID = readLocalString();
			WeaponID = readInt();
			Attackrangetype = readInt();
			Speed = readInt();
			HP = readInt();
			RotateSpeed = readInt();
			BodyAttack = readInt();
			Divide = readInt();
			Skills = readArrayint();
			BackRatio = readFloat();
			ActionSpeed = readArrayfloat();
			HittedEffectID = readInt();
			DeadSoundID = readInt();
			Cache = readInt();
			return true;
		}

		public Character_Char Copy()
		{
			Character_Char character_Char = new Character_Char();
			character_Char.CharID = CharID;
			character_Char.TypeID = TypeID;
			character_Char.ModelID = ModelID;
			character_Char.BodyScale = BodyScale;
			character_Char.TextureID = TextureID;
			character_Char.WeaponID = WeaponID;
			character_Char.Attackrangetype = Attackrangetype;
			character_Char.Speed = Speed;
			character_Char.HP = HP;
			character_Char.RotateSpeed = RotateSpeed;
			character_Char.BodyAttack = BodyAttack;
			character_Char.Divide = Divide;
			character_Char.Skills = Skills;
			character_Char.BackRatio = BackRatio;
			character_Char.ActionSpeed = ActionSpeed;
			character_Char.HittedEffectID = HittedEffectID;
			character_Char.DeadSoundID = DeadSoundID;
			character_Char.Cache = Cache;
			return character_Char;
		}
	}
}
