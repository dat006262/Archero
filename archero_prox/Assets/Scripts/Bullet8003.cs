




using UnityEngine;

public class Bullet8003 : BulletBase
{
	private Transform trailattparent;

	private Material mMaterial;

	private Color meshColor;

	private bool bHasColor;

	private Transform trailtran;

	private TrailRenderer trailrender;

	protected override void AwakeInit()
	{
		meshAlphaAction = OnMeshAlpha;
		HitWallAction = OnHitWalls;
		OnTrailShowEvent = OnTrailShowEvents;
	}

	protected override void OnInit()
	{
		base.OnInit();
		if (mMaterial == null && childMeshRender != null)
		{
			mMaterial = childMeshRender.sharedMaterial;
			bHasColor = mMaterial.HasProperty("_Color");
			if (bHasColor)
			{
				meshColor = mMaterial.GetColor("_Color");
			}
		}
		if (bHasColor)
		{
			mMaterial.SetColor("_Color", meshColor);
		}
		if (childMeshRender != null)
		{
			childMeshRender.sharedMaterial = mMaterial;
		}
		trailattparent = mBulletModel.Find("child/child/trailattparent");
		trailtran = mBulletModel.Find("child/child/trail");
		if ((bool)trailtran)
		{
			trailrender = trailtran.GetComponent<TrailRenderer>();
		}
	}

	private void OnHitWalls(Collider o)
	{
		base.m_Entity.PlayEffect(1408003, mTransform.position);
		Catapult();
	}

	protected override void OnHitHero(EntityBase entity)
	{
	}

	protected override void OnOverDistance()
	{
	}

	protected override void BoxEnable(bool enable)
	{
		base.BoxEnable(enable);
	}

	protected override void OnThroughTrailShow(bool show)
	{
		if (mTrailCtrl != null)
		{
			if (show)
			{
				mTrailCtrl.SetTrailTime(2f);
			}
			else
			{
				mTrailCtrl.SetTrailTime(1f);
			}
		}
	}

	protected override void OnUpdate()
	{
		float frameDistance = base.FrameDistance;
		UpdateMoveDirection();
		mTransform.position += new Vector3(moveX, 0f, moveY) * frameDistance;
		Transform childMesh = base.childMesh;
		Vector3 eulerAngles = base.childMesh.eulerAngles;
		float x = eulerAngles.x;
		Vector3 eulerAngles2 = base.childMesh.eulerAngles;
		float y = eulerAngles2.y + m_Data.RotateSpeed;
		Vector3 eulerAngles3 = base.childMesh.eulerAngles;
		childMesh.rotation = Quaternion.Euler(x, y, eulerAngles3.z);
		base.CurrentDistance += frameDistance;
		if (base.CurrentDistance >= base.Distance)
		{
			overDistance();
		}
	}

	private void OnMeshAlpha(float value)
	{
		if (bHasColor)
		{
			mMaterial.SetColor("_Color", new Color(meshColor.r, meshColor.g, meshColor.b, value));
		}
		if (childMeshRender != null)
		{
			childMeshRender.material = mMaterial;
		}
	}

	protected override Transform GetTrailAttParent()
	{
		if ((bool)trailattparent)
		{
			return trailattparent;
		}
		return mTransform;
	}

	private void OnTrailShowEvents(bool show)
	{
		if ((bool)trailtran)
		{
			trailtran.gameObject.SetActive(show);
		}
		if ((bool)trailrender)
		{
			trailrender.Clear();
		}
	}
}
