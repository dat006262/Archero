




using Dxx.Util;
using UnityEngine;

public class Bullet3014 : BulletBase
{
	private const float FLY_TIME = 1f;

	private SpriteRenderer sprite_sword;

	private Color mColor = new Color(1f, 1f, 1f, 1f);

	private float flytime;

	protected override void OnInit()
	{
		base.OnInit();
		sprite_sword = GetComponentInChildren<SpriteRenderer>();
		flytime = 1f;
	}

	protected override void OnUpdate()
	{
		if (flytime > 0f)
		{
			flytime -= Updater.delta;
			mColor.a = 1f - flytime / 1f;
			mColor.a = MathDxx.Clamp01(mColor.a);
			sprite_sword.color = mColor;
		}
		else
		{
			base.OnUpdate();
		}
	}
}
