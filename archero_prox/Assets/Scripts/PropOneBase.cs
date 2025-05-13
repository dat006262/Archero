




using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class PropOneBase : MonoBehaviour
{
	public ButtonCtrl mButton;

	public Image Image_BG;

	public Image Image_Icon;

	public Text Text_Value;

	public Text Text_Content;

	public Image Image_Type;

	public Image Image_QualityGold;

	private Text Text_Button;

	protected PropOneEquip.Transfer data;

	public Action<PropOneBase, object> OnClickEvent;

	private void Awake()
	{
		mButton.onClick = OnClickBase;
		Text_Button = mButton.GetComponent<Text>();
		OnAwake();
	}

	protected virtual void OnAwake()
	{
	}

	public void SetButtonEnable(bool value)
	{
		mButton.enabled = value;
		if ((bool)Text_Button)
		{
			Text_Button.enabled = value;
		}
	}

	private void Init(PropOneEquip.Transfer data)
	{
		this.data = data;
		OnInit();
	}

	public void InitCurrency(int id, long count)
	{
		Init(new PropOneEquip.Transfer
		{
			type = PropType.eCurrency,
			data = new PropOneEquip.CurrencyData
			{
				id = id,
				count = count
			}
		});
	}

	public void InitEquip(int id, int count)
	{
		Init(new PropOneEquip.Transfer
		{
			type = PropType.eEquip,
			data = new PropOneEquip.EquipData
			{
				id = id,
				count = count
			}
		});
	}

	public void InitProp(Drop_DropModel.DropData data)
	{
		if (data.type == PropType.eCurrency)
		{
			InitCurrency(data.id, data.count);
		}
		else if (data.type == PropType.eEquip)
		{
			InitEquip(data.id, data.count);
		}
	}

	protected virtual void OnInit()
	{
	}

	private void OnClickBase()
	{
		if (OnClickEvent != null)
		{
			OnClickEvent(this, data);
		}
		OnClicked();
	}

	protected virtual void OnClicked()
	{
	}
}
