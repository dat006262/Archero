




using Dxx.Util;
using UnityEngine;

public class AIMove1004 : AIMoveBase
{
	private float movedis;

	private float alldis;

	private string prevRun;

	protected float randomDisMin = 5f;

	protected float randomDisMax = 7f;

	private int randomcount;

	public AIMove1004(EntityBase entity)
		: base(entity)
	{
	}

	protected override void OnInitBase()
	{
		movedis = 0f;
		randomcount = 5;
		m_Entity.m_AniCtrl.SendEvent("Skill");
		RandomNextMoveLoop();
	}

	private void RandomNextMoveLoop()
	{
		float angle = GameLogic.Random(0f, 360f);
		float x = MathDxx.Sin(angle);
		float z = MathDxx.Cos(angle);
		m_MoveData.direction = new Vector3(x, 0f, z).normalized;
		float num = GameLogic.Random(5f, 7f);
		float num2 = 0.1f;
		alldis = num;
		if (Physics.SphereCast(m_Entity.position - m_MoveData.direction * num2, m_Entity.GetCollidersSize(), m_MoveData.direction, out RaycastHit hitInfo, alldis + num2, LayerManager.MapAllInt))
		{
			alldis = hitInfo.distance - num2;
			if (alldis < 2f && randomcount > 0)
			{
				randomcount--;
				RandomNextMoveLoop();
				return;
			}
		}
		m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(m_MoveData.direction));
	}

	protected override void OnUpdate()
	{
		float num = m_Entity.m_EntityData.GetSpeed() * Updater.delta;
		if (movedis + num > alldis)
		{
			num = alldis - movedis;
		}
		movedis += num;
		m_Entity.SetPositionBy(m_MoveData.direction * num);
		if (movedis >= alldis)
		{
			End();
		}
	}

	protected override void OnEnd()
	{
	}
}
