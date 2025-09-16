




using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityHitCtrl
{
	public EntityBase m_Entity;

	public BoxCollider m_BoxCollider;

	public SphereCollider m_SphereCollider;

	public CapsuleCollider m_CapsuleCollider;

	private bool bEnable = true;

	private Vector3 box_scale;

	private float sphere_scale;

	private float capsule_scale;

	private List<float> scales = new List<float>();

	protected Dictionary<string, BoxCollider> m_ChildsBoxCollider = new Dictionary<string, BoxCollider>();

	protected Dictionary<string, SphereCollider> m_ChildsSphereCollider = new Dictionary<string, SphereCollider>();

	protected Dictionary<string, CapsuleCollider> m_ChildsCapsuleCollider = new Dictionary<string, CapsuleCollider>();

	protected const string Entity2MapOutWall = "Entity2MapOutWall";

	protected const string Entity2Stone = "Entity2Stone";

	protected const string Entity2Water = "Entity2Water";

	public Action<Collider> Event_TriggerEnter;

	public Action<Collider> Event_TriggerExit;

	public Action<Collision> Event_CollisionEnter;

	private int triggerCount;

	public void Init(EntityBase entity)
	{
		m_Entity = entity;
		m_BoxCollider = entity.GetComponent<BoxCollider>();
		m_SphereCollider = entity.GetComponent<SphereCollider>();
		m_CapsuleCollider = entity.GetComponent<CapsuleCollider>();
		if ((bool)m_BoxCollider)
		{
			box_scale = m_BoxCollider.size;
		}
		if ((bool)m_SphereCollider)
		{
			sphere_scale = m_SphereCollider.radius;
		}
		if ((bool)m_CapsuleCollider)
		{
			capsule_scale = m_CapsuleCollider.radius;
		}
		InitCollider();
	}

	public void SetCollider(bool enable)
	{
		bEnable = enable;
		if ((bool)m_BoxCollider)
		{
			m_BoxCollider.enabled = enable;
		}
		if ((bool)m_CapsuleCollider)
		{
			m_CapsuleCollider.enabled = enable;
		}
		if ((bool)m_SphereCollider)
		{
			m_SphereCollider.enabled = enable;
		}
		Dictionary<string, BoxCollider>.Enumerator enumerator = m_ChildsBoxCollider.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if ((bool)enumerator.Current.Value)
			{
				enumerator.Current.Value.enabled = enable;
			}
		}
		Dictionary<string, SphereCollider>.Enumerator enumerator2 = m_ChildsSphereCollider.GetEnumerator();
		while (enumerator2.MoveNext())
		{
			if ((bool)enumerator2.Current.Value)
			{
				enumerator2.Current.Value.enabled = enable;
			}
		}
		Dictionary<string, CapsuleCollider>.Enumerator enumerator3 = m_ChildsCapsuleCollider.GetEnumerator();
		while (enumerator3.MoveNext())
		{
			if ((bool)enumerator3.Current.Value)
			{
				enumerator3.Current.Value.enabled = enable;
			}
		}
	}

	public void SetBodyScale(float scale)
	{
		float num = addscale(scale);
		if ((bool)m_BoxCollider)
		{
			m_BoxCollider.size = box_scale * num;
		}
		if ((bool)m_SphereCollider)
		{
			m_SphereCollider.radius = sphere_scale * num;
		}
		if ((bool)m_CapsuleCollider)
		{
			m_CapsuleCollider.radius = capsule_scale * num;
		}
	}

	public void SetTrigger(bool value)
	{
		triggerCount += (value ? 1 : (-1));
		bool isTrigger = triggerCount > 0;
		if ((bool)m_BoxCollider)
		{
			m_BoxCollider.isTrigger = isTrigger;
		}
		if ((bool)m_CapsuleCollider)
		{
			m_CapsuleCollider.isTrigger = isTrigger;
		}
		if ((bool)m_SphereCollider)
		{
			m_SphereCollider.isTrigger = isTrigger;
		}
	}

	public bool GetTrigger()
	{
		if ((bool)m_BoxCollider)
		{
			return m_BoxCollider.isTrigger;
		}
		if ((bool)m_CapsuleCollider)
		{
			return m_CapsuleCollider.isTrigger;
		}
		if ((bool)m_SphereCollider)
		{
			return m_SphereCollider.isTrigger;
		}
		return true;
	}

	private void InitCollider()
	{
		CreateCollider("Entity2MapOutWall", LayerManager.Entity2MapOutWall);
		CreateCollider("Entity2Stone", LayerManager.Entity2Stone);
		CreateCollider("Entity2Water", LayerManager.Entity2Water);
	}

	private void CreateCollider(string name, int layer)
	{
		GameObject gameObject = new GameObject(name);
		gameObject.transform.SetParent(m_Entity.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.layer = layer;
		if ((bool)m_BoxCollider)
		{
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			BoxCollider boxCollider2 = boxCollider;
			Vector3 center = m_BoxCollider.center;
			float x = center.x;
			Vector3 center2 = m_BoxCollider.center;
			boxCollider2.center = new Vector3(x, 0f, center2.z);
			boxCollider.size = m_BoxCollider.size;
			m_ChildsBoxCollider.Add(name, boxCollider);
		}
		else if ((bool)m_SphereCollider)
		{
			SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
			SphereCollider sphereCollider2 = sphereCollider;
			Vector3 center3 = m_SphereCollider.center;
			float x2 = center3.x;
			Vector3 center4 = m_SphereCollider.center;
			sphereCollider2.center = new Vector3(x2, 0f, center4.z);
			sphereCollider.radius = m_SphereCollider.radius;
			m_ChildsSphereCollider.Add(name, sphereCollider);
		}
		else if ((bool)m_CapsuleCollider)
		{
			CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
			CapsuleCollider capsuleCollider2 = capsuleCollider;
			Vector3 center5 = m_CapsuleCollider.center;
			float x3 = center5.x;
			Vector3 center6 = m_CapsuleCollider.center;
			capsuleCollider2.center = new Vector3(x3, 0f, center6.z);
			capsuleCollider.radius = m_CapsuleCollider.radius;
			capsuleCollider.height = m_CapsuleCollider.height;
			capsuleCollider.direction = m_CapsuleCollider.direction;
			m_ChildsCapsuleCollider.Add(name, capsuleCollider);
		}
	}

	private float addscale(float scale)
	{
		scales.Add(scale);
		float num = 1f;
		int i = 0;
		for (int count = scales.Count; i < count; i++)
		{
			num *= scales[i];
		}
		return num;
	}

	public void SetCollidersScale(float scale)
	{
		float num = addscale(scale);
		if ((bool)m_BoxCollider)
		{
			m_BoxCollider.size = box_scale * num;
		}
		if ((bool)m_CapsuleCollider)
		{
			m_CapsuleCollider.radius = capsule_scale * num;
		}
		if ((bool)m_SphereCollider)
		{
			m_SphereCollider.radius = sphere_scale * num;
		}
		Dictionary<string, BoxCollider>.Enumerator enumerator = m_ChildsBoxCollider.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if ((bool)enumerator.Current.Value)
			{
				enumerator.Current.Value.transform.localScale = Vector3.one * scale;
			}
		}
		Dictionary<string, SphereCollider>.Enumerator enumerator2 = m_ChildsSphereCollider.GetEnumerator();
		while (enumerator2.MoveNext())
		{
			if ((bool)enumerator2.Current.Value)
			{
				enumerator2.Current.Value.transform.localScale = Vector3.one * scale;
			}
		}
		Dictionary<string, CapsuleCollider>.Enumerator enumerator3 = m_ChildsCapsuleCollider.GetEnumerator();
		while (enumerator3.MoveNext())
		{
			if ((bool)enumerator3.Current.Value)
			{
				enumerator3.Current.Value.transform.localScale = Vector3.one * scale;
			}
		}
	}

	public float GetColliderHeight()
	{
		return m_CapsuleCollider.height;
	}

	public float GetCollidersSize()
	{
		return m_CapsuleCollider.radius;
	}

	public void RemoveColliders()
	{
		Dictionary<string, BoxCollider>.Enumerator enumerator = m_ChildsBoxCollider.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<string, BoxCollider> current = enumerator.Current;
			if ((bool)current.Value)
			{
				UnityEngine.Object.Destroy(current.Value.gameObject);
			}
		}
		m_ChildsBoxCollider.Clear();
		Dictionary<string, SphereCollider>.Enumerator enumerator2 = m_ChildsSphereCollider.GetEnumerator();
		while (enumerator2.MoveNext())
		{
			KeyValuePair<string, SphereCollider> current2 = enumerator2.Current;
			if ((bool)current2.Value)
			{
				UnityEngine.Object.Destroy(current2.Value.gameObject);
			}
		}
		m_ChildsSphereCollider.Clear();
		Dictionary<string, CapsuleCollider>.Enumerator enumerator3 = m_ChildsCapsuleCollider.GetEnumerator();
		while (enumerator3.MoveNext())
		{
			KeyValuePair<string, CapsuleCollider> current3 = enumerator3.Current;
			if ((bool)current3.Value)
			{
				UnityEngine.Object.Destroy(current3.Value.gameObject);
			}
		}
		m_ChildsCapsuleCollider.Clear();
	}

	public void SetFlyOne(string layer, bool fly)
	{
		if (m_ChildsBoxCollider.TryGetValue(layer, out BoxCollider value) && (bool)value)
		{
			value.enabled = !fly;
		}
		if (m_ChildsSphereCollider.TryGetValue(layer, out SphereCollider value2) && (bool)value2)
		{
			value2.enabled = !fly;
		}
		if (m_ChildsCapsuleCollider.TryGetValue(layer, out CapsuleCollider value3) && (bool)value3)
		{
			value3.enabled = !fly;
		}
	}

	public bool GetColliderEnable()
	{
		return bEnable;
	}

	public bool GetColliderTrigger()
	{
		if ((bool)m_CapsuleCollider)
		{
			return m_CapsuleCollider.isTrigger;
		}
		return false;
	}

	private void OnTriggerEnter(Collider o)
	{
		if (Event_TriggerEnter != null)
		{
			Event_TriggerEnter(o);
		}
	}

	private void OnTriggerExit(Collider o)
	{
		if (Event_TriggerExit != null)
		{
			Event_TriggerExit(o);
		}
	}

	private void OnCollisionEnter(Collision o)
	{
		if (Event_CollisionEnter != null)
		{
			Event_CollisionEnter(o);
		}
	}
}
