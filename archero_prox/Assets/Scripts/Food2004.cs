




using UnityEngine;

public class Food2004 : Food2001
{
	private Animator effect;

	protected override void OnAwakeInit()
	{
		effect = base.transform.Find("Equip_EffectShow").GetComponent<Animator>();
		base.OnAwakeInit();
	}

	protected override void OnEnables()
	{
		base.OnEnables();
		effect.gameObject.SetActive(value: false);
	}

	protected override void OnDropEnd()
	{
		effect.gameObject.SetActive(value: true);
		effect.Play("Equip_Show");
		GameNode.CameraShake(CameraShakeType.EquipDrop);
	}
}
