




using Dxx.Util;
using UnityEngine;

public class RayCastManager
{
	private static int _groundlayer = -1;

	private static int _flylayer = -1;

	public static int groundLayer
	{
		get
		{
			if (_groundlayer < 0)
			{
				_groundlayer = ((1 << LayerManager.Bullet2Map) | (1 << LayerManager.MapOutWall));
			}
			return _groundlayer;
		}
	}

	public static int flyLayer
	{
		get
		{
			if (_flylayer < 0)
			{
				_flylayer = 1 << LayerManager.MapOutWall;
			}
			return _flylayer;
		}
	}

	public static void CastMinDistance(Vector3 startpos, float angle, bool fly, out float mindis)
	{
		Vector3 dir = new Vector3(MathDxx.Sin(angle), 0f, MathDxx.Cos(angle));
		CastMinDistance(startpos, dir, fly, out mindis);
	}

	public static void CastMinDistance(Vector3 startpos, Vector3 dir, bool fly, out float mindis)
	{
		CastMinDistance(startpos, dir, fly, out mindis, out Vector3 _, out Collider _);
	}

	public static void CastMinDistance(Vector3 startpos, Vector3 dir, bool fly, out float mindis, out Vector3 minpos)
	{
		CastMinDistance(startpos, dir, fly, out mindis, out minpos, out Collider _);
	}

	public static void CastMinDistance(Vector3 startpos, Vector3 dir, bool fly, out float mindis, out Vector3 minpos, out Collider minCollider)
	{
		RaycastHit[] array = Physics.RaycastAll(startpos, dir, 50f, (!fly) ? groundLayer : flyLayer);
		mindis = 40f;
		minpos = startpos;
		minCollider = null;
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			RaycastHit raycastHit = array[i];
			float distance = raycastHit.distance;
			if (distance < mindis)
			{
				mindis = distance;
				minCollider = raycastHit.collider;
				minpos = raycastHit.point;
			}
		}
	}
}
