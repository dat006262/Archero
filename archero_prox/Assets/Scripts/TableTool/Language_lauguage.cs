




namespace TableTool
{
	public class Language_lauguage : LocalBean
	{
		public string TID
		{
			get;
			private set;
		}

		public string CN_s
		{
			get;
			private set;
		}

		public string CN_t
		{
			get;
			private set;
		}

		public string EN
		{
			get;
			private set;
		}

		public string FR
		{
			get;
			private set;
		}

		public string DE
		{
			get;
			private set;
		}

		public string ID
		{
			get;
			private set;
		}

		public string JP
		{
			get;
			private set;
		}

		public string KR
		{
			get;
			private set;
		}

		public string PT_BR
		{
			get;
			private set;
		}

		public string RU
		{
			get;
			private set;
		}

		public string ES_ES
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			TID = readLocalString();
			CN_s = readLocalString();
			CN_t = readLocalString();
			EN = readLocalString();
			FR = readLocalString();
			DE = readLocalString();
			ID = readLocalString();
			JP = readLocalString();
			KR = readLocalString();
			PT_BR = readLocalString();
			RU = readLocalString();
			ES_ES = readLocalString();
			return true;
		}

		public Language_lauguage Copy()
		{
			Language_lauguage language_lauguage = new Language_lauguage();
			language_lauguage.TID = TID;
			language_lauguage.CN_s = CN_s;
			language_lauguage.CN_t = CN_t;
			language_lauguage.EN = EN;
			language_lauguage.FR = FR;
			language_lauguage.DE = DE;
			language_lauguage.ID = ID;
			language_lauguage.JP = JP;
			language_lauguage.KR = KR;
			language_lauguage.PT_BR = PT_BR;
			language_lauguage.RU = RU;
			language_lauguage.ES_ES = ES_ES;
			return language_lauguage;
		}
	}
}
