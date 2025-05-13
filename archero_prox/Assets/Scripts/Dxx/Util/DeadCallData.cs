




using System;

namespace Dxx.Util
{
	public class DeadCallData : WeightRandomDataBase
	{
		public Action<EntityBase> OnDead;

		public DeadCallData(int id, Action<EntityBase> OnDead, int weight)
			: base(id)
		{
			this.OnDead = OnDead;
			base.weight = weight;
		}

		public override string ToString()
		{
			return Utils.FormatString("DeadCallData id:{0}, weight:{1}", id, weight);
		}
	}
}
