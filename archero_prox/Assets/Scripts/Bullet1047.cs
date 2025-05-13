




public class Bullet1047 : BulletBase
{
	protected override void AwakeInit()
	{
	}

	protected override void OnInit()
	{
		base.OnInit();
		base.Speed *= GameLogic.Random(1f, 1.5f);
		Parabola_MaxHeight = GameLogic.Random(2f, 5f);
		EntityBase entityBase = GameLogic.FindTarget(base.m_Entity);
		PosFromStart2Target = GameLogic.Random(-3f, 3f);
		if (entityBase != null)
		{
			PosFromStart2Target += (entityBase.position - base.m_Entity.position).magnitude;
		}
	}
}
