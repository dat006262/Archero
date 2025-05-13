




using Dxx.Util;
using UnityEngine;

public class Bullet1001 : BulletBase
{
	private Animation ani;

	private GameObject dilie;

	private Transform wave;

	private Vector3 startPos;

	private Quaternion startrota;

	protected override void AwakeInit()
	{
		ani = GetComponentInChildren<Animation>();
		wave = mTransform.Find("wave");
		Transform transform = mTransform.Find("dilie");
		if ((bool)transform)
		{
			dilie = transform.gameObject;
		}
		if ((bool)ani)
		{
			startPos = ani.transform.localPosition;
			startrota = ani.transform.localRotation;
		}
	}

	protected override void OnInit()
	{
		base.OnInit();
		if ((bool)dilie)
		{
			dilie.SetActive(value: false);
		}
		if ((bool)ani)
		{
			ani.transform.localPosition = startPos;
			ani.transform.localRotation = startrota;
			ani.enabled = false;
		}
	}

	protected override void OnDeInit()
	{
		if ((bool)ani)
		{
			ani.enabled = false;
		}
		base.OnDeInit();
	}

	protected override void OnHitWall()
	{
		PHitWallAnimation();
	}

	protected override void OnHitHero(EntityBase entity)
	{
		PHitWallAnimation();
	}

	private void PHitWallAnimation()
	{
		if ((bool)ani)
		{
			ani.enabled = true;
			ani.Play(Utils.GetString(ClassName, "_Wall"));
		}
	}

	protected override void OnOverDistance()
	{
		if ((bool)ani)
		{
			ani.enabled = true;
			ani.Play(ClassName);
		}
		if ((bool)dilie)
		{
			dilie.SetActive(value: true);
		}
	}

	private void WaveActive(bool active)
	{
		if ((bool)wave)
		{
			wave.gameObject.SetActive(active);
		}
	}

	protected override void BoxEnable(bool enable)
	{
		base.BoxEnable(enable);
		WaveActive(enable);
	}

	protected override void OnThroughTrailShow(bool show)
	{
	}
}
