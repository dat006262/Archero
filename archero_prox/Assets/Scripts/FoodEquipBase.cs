




using Dxx.Util;
using UnityEngine;

public class FoodEquipBase : FoodBase
{
	public Transform effectparent;

	public Transform meshparent;

	public SpriteRenderer sprite;

	private GameObject effect;

	private EquipNameCtrl mNameCtrl;

	public LocalSave.EquipOne equipone
	{
		get;
		private set;
	}

	protected override void OnAwakeInit()
	{
		flyTime = 0.4f;
		flyDelayTime = 0.7f;
		flySpeed = 0.3f;
		sprite = base.transform.Find("child/rotate/sprite").GetComponent<SpriteRenderer>();
		bFlyRotate = false;
	}

	private GameObject getparent()
	{
		if ((bool)meshparent)
		{
			return meshparent.gameObject;
		}
		return base.gameObject;
	}

	protected override void OnInit()
	{
		meshparent.DestroyChildren();
		effectparent.DestroyChildren();
		equipone = (data as LocalSave.EquipOne);
		if (equipone == null)
		{
			SdkManager.Bugly_Report("FoodEquipBase.cs", "[data] is not [LocalSave.EquipOne] type.");
			return;
		}
		string empty = string.Empty;
		if (equipone.Position == 1)
		{
			empty = Utils.FormatString("Game/WeaponHand/WeaponHand{0}", equipone.EquipID);
			GameObject child = Object.Instantiate(ResourceManager.Load<GameObject>(empty));
			child.SetParentNormal(getparent());
			sprite.gameObject.SetActive(value: false);
			base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 45f, 0f));
		}
		else
		{
			sprite.gameObject.SetActive(value: true);
			sprite.sprite = equipone.Icon;
			base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		}
		CreateName();
		SetNameShow(value: false);
	}

	protected override void OnAbsorbStart()
	{
		EffectShow(value: false);
	}

	private void EffectShow(bool value)
	{
		if ((bool)effectparent)
		{
			effectparent.gameObject.SetActive(value);
		}
		if (value && equipone != null)
		{
			if (equipone.Overlying)
			{
				effect = CInstance<BattleResourceCreator>.Instance.GetFoodEquipEffect_EquipExp(effectparent);
			}
			else
			{
				effect = CInstance<BattleResourceCreator>.Instance.GetFoodEquipEffect_Equip(effectparent);
			}
		}
	}

	private void CreateName()
	{
		GameObject gameObject = GameLogic.EffectGet(Utils.GetString("Game/UI/EquipName"));
		gameObject.transform.SetParent(GameNode.m_HP.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localRotation = Quaternion.identity;
		EquipNameCtrl equipNameCtrl = mNameCtrl = gameObject.GetComponent<EquipNameCtrl>();
		mNameCtrl.Init(this);
	}

	protected override void OnDropEnd()
	{
		SetNameShow(value: true);
		GameLogic.Hold.Sound.PlayBattleSpecial(5000013, base.transform.position);
	}

	private void SetNameShow(bool value)
	{
		if (mNameCtrl != null)
		{
			mNameCtrl.gameObject.SetActive(value);
		}
		EffectShow(value);
	}

	protected override void OnGetGoodsEnd()
	{
		OnDeInit();
	}

	protected override void OnDeInit()
	{
		if (mNameCtrl != null)
		{
			GameLogic.EffectCache(mNameCtrl.gameObject);
		}
	}
}
