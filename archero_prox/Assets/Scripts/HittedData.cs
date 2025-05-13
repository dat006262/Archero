




using UnityEngine;

public class HittedData
{
	public EHittedType type = EHittedType.eNormal;

	public float hitratio = 1f;

	public float backtatio = 1f;

	public BulletBase bullet;

	public HitType hittype;

	public float angle
	{
		get
		{
			if (bullet != null)
			{
				Vector3 eulerAngles = bullet.transform.eulerAngles;
				return eulerAngles.y;
			}
			return 0f;
		}
	}

	public void SetBullet(BulletBase bullet)
	{
		this.bullet = bullet;
	}

	public void AddBackRatio(float back)
	{
		backtatio *= back;
	}

	public bool GetCanHitted()
	{
		return type != EHittedType.eInvincible;
	}

	public bool GetPlayHitted()
	{
		return type != EHittedType.eDefence;
	}
}
