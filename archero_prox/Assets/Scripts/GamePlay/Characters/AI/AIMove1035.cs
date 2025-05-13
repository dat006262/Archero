




using Dxx.Util;
using TableTool;
using UnityEngine;

public class AIMove1035 : AIMoveBase
{
	private EntityBase target;

	private AnimationCurve curve;

	private int range;

	private float playerrange;

	private float ratio;

	private Vector3 startpos;

	private Vector3 endpos;

	private float alltime;

	private float height = 5f;

	private float currenttime;

	public AIMove1035(EntityBase entity, int range, float playerrange, float ratio)
		: base(entity)
	{
		target = GameLogic.Self;
		this.range = range;
		this.playerrange = playerrange;
		this.ratio = ratio;
		curve = LocalModelManager.Instance.Curve_curve.GetCurve(100023);
	}

	protected override void OnInitBase()
	{
		currenttime = 0f;
		float magnitude = (m_Entity.position - target.position).magnitude;
		float num = GameLogic.Random(0f, 1f);
		if (magnitude < playerrange && num < ratio)
		{
			endpos = target.position;
		}
		else
		{
			GameLogic.Release.MapCreatorCtrl.RandomItemSide(m_Entity, range, out float endx, out float endz);
			endpos = new Vector3(endx, 0f, endz);
		}
		UpdateDirection();
		startpos = m_Entity.position;
		alltime = (endpos - startpos).magnitude / 7f;
		GameLogic.Hold.Sound.PlayMonsterSkill(5100007, m_Entity.position);
	}

	private void UpdateDirection()
	{
		float x = endpos.x;
		Vector3 position = m_Entity.position;
		float x2 = x - position.x;
		float z = endpos.z;
		Vector3 position2 = m_Entity.position;
		float num = z - position2.z;
		m_MoveData.angle = Utils.getAngle(x2, num);
		m_MoveData.direction = new Vector3(x2, 0f, num).normalized;
		m_Entity.m_AttackCtrl.RotateHero(m_MoveData.angle);
	}

	protected override void OnUpdate()
	{
		currenttime += Updater.delta;
		if (currenttime > alltime)
		{
			currenttime = alltime;
		}
		float y = curve.Evaluate(currenttime / alltime) * height;
		float d = currenttime / alltime;
		m_Entity.SetPosition((endpos - startpos) * d + startpos + new Vector3(0f, y, 0f));
		if (currenttime == alltime)
		{
			CreateBullet();
			End();
		}
	}

	private void CreateBullet()
	{
		if (m_Entity.IsElite)
		{
			float num = GameLogic.Random(0, 360);
			int num2 = 6;
			for (int i = 0; i < num2; i++)
			{
				GameLogic.Release.Bullet.CreateBullet(m_Entity, 5061, m_Entity.position + new Vector3(0f, 0.5f, 0f), num + (float)i * 60f);
			}
		}
	}

	protected override void OnEnd()
	{
		m_Entity.m_AniCtrl.SetString("Skill", "Jump3011");
		m_Entity.m_AniCtrl.SendEvent("Skill");
	}
}
