




using DG.Tweening;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EventChest1OneCtrl : MonoBehaviour
{
	public Transform child;

	public Image Image_Icon;

	public Text Text_Value;

	private PropOneEquip _equipone;

	private PropOneEquip equipone
	{
		get
		{
			if (_equipone == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/CharUI/EquipPropOne"));
				gameObject.SetParentNormal(Image_Icon.transform);
				(gameObject.transform as RectTransform).sizeDelta = Vector3.one * 100f;
				_equipone = gameObject.GetComponent<PropOneEquip>();
			}
			return _equipone;
		}
	}

	public TurnTableData mData
	{
		get;
		private set;
	}

	public void Init(TurnTableData data)
	{
		mData = data;
		child.localScale = Vector3.one;
		Text_Value.text = string.Empty;
		Image_Icon.enabled = true;
		if (data.type != TurnTableType.Get && _equipone != null)
		{
			_equipone.gameObject.SetActive(value: false);
		}
		switch (data.type)
		{
		case TurnTableType.Gold:
		case TurnTableType.Diamond:
			break;
		case TurnTableType.BigEquip:
		case TurnTableType.SmallEquip:
		{
			Image_Icon.enabled = false;
			Drop_DropModel.DropData dropData = data.value as Drop_DropModel.DropData;
			equipone.gameObject.SetActive(value: true);
			equipone.InitEquip(dropData.id, dropData.count);
			Image_Icon.sprite = SpriteManager.GetEquip(LocalModelManager.Instance.Equip_equip.GetBeanById(dropData.id).EquipIcon);
			break;
		}
		case TurnTableType.Boss:
			Image_Icon.sprite = SpriteManager.GetBattle("GameTurn_Monster");
			break;
		case TurnTableType.Hitted:
			Image_Icon.sprite = SpriteManager.GetBattle("GameTurn_Hitted");
			break;
		case TurnTableType.Get:
		{
			Sequence sequence = DOTween.Sequence();
			sequence.SetUpdate(isIndependentUpdate: true);
			sequence.Append(child.DOScale(1.3f, 0.18f));
			sequence.Append(child.DOScale(1f, 0.18f));
			sequence.AppendCallback(delegate
			{
				if (_equipone != null)
				{
					_equipone.gameObject.SetActive(value: false);
				}
				Image_Icon.sprite = SpriteManager.GetBattle("GameTurn_Tick");
			});
			break;
		}
		}
	}
}
