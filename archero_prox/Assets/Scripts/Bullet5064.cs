




using DG.Tweening;
using Dxx.Util;

public class Bullet5064 : BulletBase
{
	private float angle;

	private float time = 1.25f;

	private float starttime;

	private float updatetime;

	private float alltime = 3.9f;

	private bool bStartCreate;

	protected override void OnInit()
	{
		base.OnInit();
		bStartCreate = false;
		Sequence s = mSeqPool.Get();
		s.Append(mTransform.DOMoveY(0.8f, 1f)).AppendCallback(delegate
		{
			bStartCreate = true;
			starttime = Updater.AliveTime - GameLogic.Random(0f, 0.5f);
			updatetime = starttime;
		});
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
	}

	protected override void UpdateProcess()
	{
		if (bStartCreate)
		{
			base.UpdateProcess();
			if (Updater.AliveTime - updatetime > time)
			{
				updatetime += time;
				CreateBullets();
			}
			if (Updater.AliveTime - starttime > alltime)
			{
				overDistance();
			}
		}
	}

	private void CreateBullets()
	{
		angle = GameLogic.Random(0f, 360f);
		for (int i = 0; i < 4; i++)
		{
			GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 5063, mTransform.position, angle + bulletAngle + (float)(i * 90));
		}
	}
}
