




using UnityEngine;

namespace TableTool
{
	public class Curve_curveModel : LocalModel<Curve_curve, int>
	{
		private const string _Filename = "Curve_curve";

		protected override string Filename => "Curve_curve";

		protected override int GetBeanKey(Curve_curve bean)
		{
			return bean.ID;
		}

		public AnimationCurve GetCurve(int id)
		{
			return GetBeanById(id).GetCurve();
		}

		public AnimationCurve GetSin()
		{
			return GetCurve(200002);
		}
	}
}
