




using UnityEngine;

public class Equip1000001 : EquipBase
{
	private Animator effect;

	protected override void AwakeInit()
	{
		effect = base.transform.Find("Equip_EffectShow").GetComponent<Animator>();
		base.AwakeInit();
	}

	protected override void Init()
	{
		effect.gameObject.SetActive(value: false);
	}

	protected override void OnAbsorb()
	{
	}

	protected override void OnDropEnd()
	{
		effect.gameObject.SetActive(value: true);
		effect.Play("Equip_Show");
		GameNode.CameraShake(CameraShakeType.EquipDrop);
	}
}
