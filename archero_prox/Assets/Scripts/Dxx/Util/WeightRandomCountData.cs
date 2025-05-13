




namespace Dxx.Util
{
	public class WeightRandomCountData : WeightRandomDataBase
	{
		public int randomcount;

		public int lastrandomindex;

		public WeightRandomCountData(int id)
			: base(id)
		{
		}

		public void RandomSelf(int randomindex)
		{
			randomcount++;
			lastrandomindex = randomindex;
		}

		public bool GetCanRandom(int randomindex, int maxcount)
		{
			if (lastrandomindex == randomindex)
			{
				if (randomcount >= maxcount)
				{
					return false;
				}
			}
			else
			{
				randomcount = 0;
			}
			return true;
		}
	}
}
