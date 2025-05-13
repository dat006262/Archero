




public class BattleStruct
{
	public struct HittedAllStruct
	{
		public EntityBase entity;
	}

	public struct DeadStruct
	{
		public EntityBase entity;
	}

	public struct BuffStruct
	{
		public EntityBase entity;

		public int buffId;

		public float[] args;
	}
}
