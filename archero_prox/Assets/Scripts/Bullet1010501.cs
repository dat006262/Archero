




using Dxx.Util;

public class Bullet1010501 : BulletBase
{
	private float mDelayTime;

	private const float DelayTime = 0.4f;

	protected override void OnInit()
	{
		base.OnInit();
		mDelayTime = Updater.AliveTime;
		OnMove(0.7f);
	}

	protected override void OnUpdate()
	{
		if (Updater.AliveTime - mDelayTime > 0.4f)
		{
			base.OnUpdate();
		}
	}
}
