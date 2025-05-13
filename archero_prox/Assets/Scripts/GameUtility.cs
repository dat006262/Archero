




using System.Collections;
using UnityEngine;

public static class GameUtility
{
	public static Transform FindDeepChild(GameObject _target, string _childName)
	{
		Transform transform = null;
		transform = _target.transform.Find(_childName);
		if (transform == null)
		{
			IEnumerator enumerator = _target.transform.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Transform transform2 = enumerator.Current as Transform;
				transform = FindDeepChild(transform2.gameObject, _childName);
				if (transform != null)
				{
					return transform;
				}
			}
		}
		return transform;
	}

	public static T FindDeepChild<T>(GameObject _target, string _childName) where T : Component
	{
		Transform transform = FindDeepChild(_target, _childName);
		if (transform != null)
		{
			return transform.gameObject.GetComponent<T>();
		}
		return (T)null;
	}

	public static void AddChildToTarget(Transform target, Transform child)
	{
		child.parent = target;
		child.localScale = Vector3.one;
		child.localPosition = Vector3.zero;
		child.localEulerAngles = Vector3.zero;
		child.ChangeChildLayer(target.gameObject.layer);
	}

	public static void ChangeChildLayer(this GameObject o, int layer)
	{
		o.transform.ChangeChildLayer(layer);
	}

	public static void ChangeChildLayer(this Transform t, int layer)
	{
		t.gameObject.layer = layer;
		for (int i = 0; i < t.childCount; i++)
		{
			Transform child = t.GetChild(i);
			child.gameObject.layer = layer;
			child.ChangeChildLayer(layer);
		}
	}
}
