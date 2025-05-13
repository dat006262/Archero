




namespace TableTool
{
	public class Test_AttrValue : LocalBean
	{
		public string TypeId
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public int DeltaValue
		{
			get;
			private set;
		}

		public long Startlong
		{
			get;
			private set;
		}

		public bool Test
		{
			get;
			private set;
		}

		public float Testfloat
		{
			get;
			private set;
		}

		public double Testdouble
		{
			get;
			private set;
		}

		public short Testshort
		{
			get;
			private set;
		}

		public int[] Testarrayint
		{
			get;
			private set;
		}

		public float[] Testarrayfloat
		{
			get;
			private set;
		}

		public short[] Testarrayshort
		{
			get;
			private set;
		}

		public long[] Testarraylong
		{
			get;
			private set;
		}

		public double[] Testarraydouble
		{
			get;
			private set;
		}

		public bool[] Testarraybool
		{
			get;
			private set;
		}

		public string[] Testarraystring
		{
			get;
			private set;
		}

		public int Testint
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			TypeId = readLocalString();
			Notes = readLocalString();
			DeltaValue = readInt();
			Startlong = readLong();
			Test = readBool();
			Testfloat = readFloat();
			Testdouble = readDouble();
			Testshort = readShort();
			Testarrayint = readArrayint();
			Testarrayfloat = readArrayfloat();
			Testarrayshort = readArrayshort();
			Testarraylong = readArraylong();
			Testarraydouble = readArraydouble();
			Testarraybool = readArraybool();
			Testarraystring = readArraystring();
			Testint = readInt();
			return true;
		}

		public Test_AttrValue Copy()
		{
			Test_AttrValue test_AttrValue = new Test_AttrValue();
			test_AttrValue.TypeId = TypeId;
			test_AttrValue.Notes = Notes;
			test_AttrValue.DeltaValue = DeltaValue;
			test_AttrValue.Startlong = Startlong;
			test_AttrValue.Test = Test;
			test_AttrValue.Testfloat = Testfloat;
			test_AttrValue.Testdouble = Testdouble;
			test_AttrValue.Testshort = Testshort;
			test_AttrValue.Testarrayint = Testarrayint;
			test_AttrValue.Testarrayfloat = Testarrayfloat;
			test_AttrValue.Testarrayshort = Testarrayshort;
			test_AttrValue.Testarraylong = Testarraylong;
			test_AttrValue.Testarraydouble = Testarraydouble;
			test_AttrValue.Testarraybool = Testarraybool;
			test_AttrValue.Testarraystring = Testarraystring;
			test_AttrValue.Testint = Testint;
			return test_AttrValue;
		}
	}
}
