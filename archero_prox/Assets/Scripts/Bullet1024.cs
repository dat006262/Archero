




using DG.Tweening;
using Dxx.Util;
using UnityEngine;

public class Bullet1024 : BulletBase
{
	private Transform mStart;

	private Transform mEnd;

	private LineRenderer line;

	private const float textureLengthScale = 3f;

	private const float textureScrollSpeed = 8f;

	private int layerMask;

	private float startwidth;

	private float starttime;

	private float line_update_time = 0.2f;

	private bool bNearEnd;

	private SequencePool mPool = new SequencePool();

	protected override void OnInit()
	{
		base.OnInit();
		mStart = mBulletModel.Find("FireBeamStart");
		mEnd = mBulletModel.Find("FireBeamEnd");
		Transform transform = mBulletModel.Find("Fire Beam");
		if ((bool)transform)
		{
			line = transform.GetComponent<LineRenderer>();
			line.sortingLayerName = "Hit";
			startwidth = line.widthMultiplier;
		}
		if (m_Data.bThroughInsideWall)
		{
			layerMask = 1 << LayerManager.MapOutWall;
		}
		else
		{
			layerMask = ((1 << LayerManager.Stone) | (1 << LayerManager.MapOutWall));
		}
		CheckBulletLength();
		starttime = 0f;
		bNearEnd = false;
		UpdateLineWidth();
		mPool.Clear();
		Sequence s = mPool.Get();
		float interval = (float)m_Data.AliveTime / 1000f - line_update_time;
		s.AppendInterval(interval);
		s.AppendCallback(delegate
		{
			bNearEnd = true;
			starttime = 0f;
		});
	}

	protected override void OnDeInit()
	{
		base.OnDeInit();
		mPool.Clear();
	}

	public void CheckBulletLength()
	{
		float x = MathDxx.Sin(bulletAngle);
		float z = MathDxx.Cos(bulletAngle);
		Vector3 vector = new Vector3(x, 0f, z);
		RaycastHit[] array = Physics.RaycastAll(mTransform.position, vector, 100f, layerMask);
		float num = 100f;
		int i = 0;
		for (int num2 = array.Length; i < num2; i++)
		{
			RaycastHit raycastHit = array[i];
			if (((raycastHit.collider.gameObject.layer == LayerManager.Stone && !m_Data.bThroughWall) || raycastHit.collider.gameObject.layer == LayerManager.MapOutWall) && num > raycastHit.distance)
			{
				num = raycastHit.distance;
			}
		}
		if ((bool)mStart)
		{
			mStart.position = mTransform.position;
		}
		Vector3 vector2 = mTransform.position + vector * num;
		if ((bool)mEnd)
		{
			mEnd.position = vector2;
		}
		if ((bool)line)
		{
			line.positionCount = 2;
			line.SetPosition(0, mTransform.position);
			line.SetPosition(1, vector2);
			float num3 = Vector3.Distance(mTransform.position, vector2);
			line.material.mainTextureScale = new Vector2(num3 / 3f, 1f);
			line.material.mainTextureOffset -= new Vector2(Time.deltaTime * 8f, 0f);
		}
		boxList[0].center = new Vector3(0f, 0f, num / 2f);
		BoxCollider obj = boxList[0];
		Vector3 size = boxList[0].size;
		float x2 = size.x;
		Vector3 size2 = boxList[0].size;
		obj.size = new Vector3(x2, size2.y, num);
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
		UpdateLineWidth();
	}

	protected override void OnOverDistance()
	{
		base.OnOverDistance();
		line.widthMultiplier = startwidth;
	}

	private void UpdateLineWidth()
	{
		if ((bool)line)
		{
			if (!bNearEnd && starttime < line_update_time + 0.1f)
			{
				starttime += Updater.delta;
				starttime = MathDxx.Clamp(starttime, 0f, line_update_time);
				line.widthMultiplier = starttime / line_update_time * startwidth;
			}
			if (bNearEnd)
			{
				starttime += Updater.delta;
				starttime = MathDxx.Clamp(starttime, 0f, line_update_time);
				line.widthMultiplier = (1f - starttime / line_update_time) * startwidth;
			}
		}
	}
}
