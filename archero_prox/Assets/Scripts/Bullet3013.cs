




using DG.Tweening;

public class Bullet3013 : BulletBase
{
	private float enabletime = 0.03f;

	private Sequence seq_enableonce;

	protected override void OnInit()
	{
		bLight45 = true;
		base.OnInit();
		SetBoxEnableOnce(enabletime);
	}
}
