




using Dxx.Util;

public class Weapon1096 : Weapon1020
{
	protected override void OnInstall()
	{
		base.OnInstall();
	}

	protected override void OnUnInstall()
	{
		base.OnUnInstall();
	}

	protected override void OnAttack(object[] args)
	{
		if (MathDxx.RandomBool())
		{
			int num = 3;
			for (int i = 0; i < num; i++)
			{
				CreateBullet1020(Utils.GetBulletAngle(i, num, 90f));
			}
			return;
		}
		int num2 = 3;
		for (int j = 0; j < num2; j++)
		{
			action.AddActionDelegate(delegate
			{
				CreateBullet1020(0f);
			});
			action.AddActionWait(0.15f);
		}
	}
}
