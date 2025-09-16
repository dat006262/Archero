




using Dxx.Util;
using System.Collections.Generic;
using UnityEngine;

public class BulletRedLineCtrls
{
	private float time;

	private float alltime = 0.5f;

	private List<BulletRedLineCtrl> list = new List<BulletRedLineCtrl>();

	private List<float> mindiss = new List<float>();

	public void Init(EntityBase entity, int count, float[] angles)
	{
		Deinit();
		time = 0f;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>("Game/Bullet/Bullet1066_RedLine"));
			gameObject.SetParentNormal(entity.m_Body.transform);
			gameObject.transform.localRotation = Quaternion.Euler(0f, angles[i], 0f);
			BulletRedLineCtrl component = gameObject.GetComponent<BulletRedLineCtrl>();
			component.SetLine(islast: true, 0f);
			Vector3 b = new Vector3(MathDxx.Sin(angles[i] + 90f), 0f, MathDxx.Cos(angles[i] + 90f)) * 0.5f;
			Vector3 startpos = entity.m_Body.transform.position + b;
			Vector3 eulerAngles = entity.eulerAngles;
			RayCastManager.CastMinDistance(startpos, eulerAngles.y, fly: false, out float mindis);
			Vector3 startpos2 = entity.m_Body.transform.position - b;
			Vector3 eulerAngles2 = entity.eulerAngles;
			RayCastManager.CastMinDistance(startpos2, eulerAngles2.y, fly: false, out float mindis2);
			mindiss.Add((!(mindis < mindis2)) ? mindis2 : mindis);
			list.Add(component);
		}
		Updater.AddUpdate("Weapon1066", OnUpdate);
	}

	private void OnUpdate(float delta)
	{
		time += delta;
		time = MathDxx.Clamp(time, 0f, alltime);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			list[i].SetLine(islast: true, mindiss[i] * (time / alltime));
		}
	}

	public void Deinit()
	{
		Updater.RemoveUpdate("Weapon1066", OnUpdate);
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			UnityEngine.Object.Destroy(list[i].gameObject);
		}
		list.Clear();
		mindiss.Clear();
	}
}
