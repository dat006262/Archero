




using UnityEngine;

public class AIGroundBase : AIBase
{
	private Animation mAni_Ground;

	protected override void OnInitOnce()
	{
		if (mAni_Ground == null)
		{
			GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>("Game/Models/3028_GroundBreak"));
			gameObject.SetParentNormal(m_Entity.transform);
			mAni_Ground = gameObject.transform.Find("scale/sprite").GetComponent<Animation>();
		}
		base.OnInitOnce();
	}

	public void GroundShow(bool value)
	{
		if (value)
		{
			mAni_Ground.Play("3028_GroundBreak_Show");
		}
		else
		{
			mAni_Ground.Play("3028_GroundBreak_Miss");
		}
	}
}
