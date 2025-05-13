




using Dxx.Util;
using UnityEngine;

public class Bullet1076 : BulletBase
{
	private Transform line;

	private Transform ball_left;

	private Transform ball_right;

	private float boxx;

	private float maxtime = 1f;

	private float currenttime;

	public float maxWidth = 2f;

	protected override void OnInit()
	{
		base.OnInit();
		boxx = 0f;
		SetBoxX(boxx);
		if (!line)
		{
			line = mBulletModel.Find("child/line");
		}
		if (!ball_left)
		{
			ball_left = mBulletModel.Find("child/left");
		}
		if (!ball_right)
		{
			ball_right = mBulletModel.Find("child/right");
		}
		currenttime = Updater.AliveTime;
	}

	private void SetBoxX(float x)
	{
		x = MathDxx.Clamp(x, x, maxWidth);
		if (boxList.Length > 0)
		{
			BoxCollider obj = boxList[0];
			float x2 = x + 0.4f;
			Vector3 size = boxList[0].size;
			float y = size.y;
			Vector3 size2 = boxList[0].size;
			obj.size = new Vector3(x2, y, size2.z);
		}
		if ((bool)line)
		{
			Transform transform = line;
			Vector3 localScale = line.localScale;
			float x3 = localScale.x;
			float y2 = x / 2f;
			Vector3 localScale2 = line.localScale;
			transform.localScale = new Vector3(x3, y2, localScale2.z);
		}
		if ((bool)ball_left)
		{
			ball_left.localPosition = new Vector3((0f - x) / 2f, 0f, 0f);
			ball_left.rotation = Quaternion.Euler(-35f, 0f, 0f);
		}
		if ((bool)ball_right)
		{
			ball_right.localPosition = new Vector3(x / 2f, 0f, 0f);
			ball_right.rotation = Quaternion.Euler(-35f, 0f, 0f);
		}
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		SetBoxX((Updater.AliveTime - currenttime) / maxtime * maxWidth);
	}
}
