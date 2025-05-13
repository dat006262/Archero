




namespace TableTool
{
	public class Soldier_standard : LocalBean
	{
		public int Level
		{
			get;
			private set;
		}

		public int Integral_Up
		{
			get;
			private set;
		}

		public int Integral_Down
		{
			get;
			private set;
		}

		public int Standard_Attack
		{
			get;
			private set;
		}

		public int Standard_HpMax
		{
			get;
			private set;
		}

		public float Coins_Ratio
		{
			get;
			private set;
		}

		public float Exp_Ratio
		{
			get;
			private set;
		}

		public string[] ScrollRate
		{
			get;
			private set;
		}

		public string[] ScrollRateBoss
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			Level = readInt();
			Integral_Up = readInt();
			Integral_Down = readInt();
			Standard_Attack = readInt();
			Standard_HpMax = readInt();
			Coins_Ratio = readFloat();
			Exp_Ratio = readFloat();
			ScrollRate = readArraystring();
			ScrollRateBoss = readArraystring();
			return true;
		}

		public Soldier_standard Copy()
		{
			Soldier_standard soldier_standard = new Soldier_standard();
			soldier_standard.Level = Level;
			soldier_standard.Integral_Up = Integral_Up;
			soldier_standard.Integral_Down = Integral_Down;
			soldier_standard.Standard_Attack = Standard_Attack;
			soldier_standard.Standard_HpMax = Standard_HpMax;
			soldier_standard.Coins_Ratio = Coins_Ratio;
			soldier_standard.Exp_Ratio = Exp_Ratio;
			soldier_standard.ScrollRate = ScrollRate;
			soldier_standard.ScrollRateBoss = ScrollRateBoss;
			return soldier_standard;
		}
	}
}
