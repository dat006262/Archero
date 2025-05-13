




using DG.Tweening;
using UnityEngine;

public class SkillCreate1012 : SkillCreateBase
{
	private Transform child;

	private float hitratio;

	private float range;

	private Sequence seq;

	private Bullet1056 bullet;

	protected override void OnAwake()
	{
		child = base.transform.Find("child");
	}

	protected override void OnInit(string[] args)
	{
		time = float.Parse(args[0]);
		range = float.Parse(args[1]);
		hitratio = float.Parse(args[2]);
		bullet = (GameLogic.Release.Bullet.CreateBullet(m_Entity, 1056, child.position, 0f) as Bullet1056);
		bullet.InitData(range, hitratio);
	}

	protected override void OnDeinit()
	{
		if ((bool)bullet)
		{
			bullet.DeInit();
			bullet = null;
		}
	}
}
