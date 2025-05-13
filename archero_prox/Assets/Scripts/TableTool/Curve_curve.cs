




using UnityEngine;

namespace TableTool
{
	public class Curve_curve : LocalBean
	{
		private AnimationCurve curve;

		public int ID
		{
			get;
			private set;
		}

		public string Notes
		{
			get;
			private set;
		}

		public string[] Values
		{
			get;
			private set;
		}

		protected override bool ReadImpl()
		{
			ID = readInt();
			Notes = readLocalString();
			Values = readArraystring();
			return true;
		}

		public Curve_curve Copy()
		{
			Curve_curve curve_curve = new Curve_curve();
			curve_curve.ID = ID;
			curve_curve.Notes = Notes;
			curve_curve.Values = Values;
			return curve_curve;
		}

		public AnimationCurve GetCurve()
		{
			if (curve == null)
			{
				InitCurve();
			}
			return curve;
		}

		private void InitCurve()
		{
			int num = Values.Length;
			Keyframe[] array = new Keyframe[num];
			for (int i = 0; i < num; i++)
			{
				string[] array2 = Values[i].Split(',');
				array[i] = new Keyframe(float.Parse(array2[0]), float.Parse(array2[1]));
				array[i].inTangent = float.Parse(array2[2]);
				array[i].outTangent = float.Parse(array2[3]);
			}
			curve = new AnimationCurve(array);
		}
	}
}
