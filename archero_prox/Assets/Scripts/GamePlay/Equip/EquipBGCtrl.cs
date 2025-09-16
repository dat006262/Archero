




using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipBGCtrl : MonoBehaviour
{
	private LocalSave.EquipOne _equipdata;

	private int index;

	private GameObject addparent;

	private Animation addani;

	private Text Text_Add;

	private Image Image_BG;

	private GameObject equipparent;

	private EquipOneCtrl _ctrl;

	private GameObject buttonObj;

	private ButtonCtrl button;

	private Text buttonText;

	private Action<int> mClick;

	private bool bInit;

	public LocalSave.EquipOne equipdata
	{
		get
		{
			return _equipdata;
		}
		set
		{
			_equipdata = value;
			UpdateButtonEnable();
		}
	}

	public EquipOneCtrl ctrl
	{
		get
		{
			if (_ctrl == null)
			{
				_ctrl = CInstance<UIResourceCreator>.Instance.GetEquip(equipparent.transform);
				_ctrl.SetButtonEnable(value: false);
				_ctrl.ShowAniEnable(value: false);
			}
			return _ctrl;
		}
	}

	private void Awake()
	{
		index = int.Parse(base.name.Substring(base.name.Length - 1, 1));
		buttonObj = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/CharUI/EquipBG"));
		button = buttonObj.GetComponent<ButtonCtrl>();
		button.onClick = OnClick;
		buttonText = button.GetComponent<Text>();
		Transform transform = buttonObj.transform;
		transform.SetParentNormal(base.transform);
		addani = transform.Find("fg/Image_Add").GetComponent<Animation>();
		Image_BG = transform.Find("fg/Image_BG").GetComponent<Image>();
		equipparent = addani.transform.Find("equipparent").gameObject;
		string value = string.Empty;
		switch (index)
		{
		case 0:
			value = "EquipUI_BG_Weapon";
			break;
		case 1:
			value = "EquipUI_BG_Cloth";
			break;
		case 2:
		case 3:
			value = "EquipUI_BG_Ornament";
			break;
		case 4:
		case 5:
			value = "EquipUI_BG_Pet";
			break;
		}
		Image_BG.sprite = SpriteManager.GetCharUI(value);
		addparent = addani.transform.Find("addparent").gameObject;
		Text_Add = addparent.transform.Find("Image_Shadow/Text_Add").GetComponent<Text>();
		equipdata = null;
		UpdateBGShow();
		ShowAdd(value: false);
	}

	private void Start()
	{
	}

	public void Init(LocalSave.EquipOne equipdata)
	{
		this.equipdata = equipdata;

        if (this.equipdata == null)
		{
            ctrl.gameObject.SetActive(value: false);
		}
		else
		{
            Debug.Log("@LOG EquipBGCtrl.Init name:" + name);
            Debug.Log("@LOG EquipBGCtrl.Init EquipID:" + equipdata.EquipID);
            ctrl.gameObject.SetActive(value: true);
			ctrl.Init(equipdata);
			ctrl.SetButtonEnable(value: false);
		}
		UpdateBGShow();
		UpdateRedNode();
		RectTransform rectTransform = ctrl.transform as RectTransform;
		rectTransform.localScale = Vector3.one * 1.1f;
		rectTransform.anchoredPosition = Vector2.zero;
		StopRotate();
	}

	private void UpdateBGShow()
	{
		Image_BG.gameObject.SetActive(equipdata == null);
	}

	public void UpdateRedNode()
	{
		if (equipdata != null && ctrl != null && ctrl.equipdata != null)
		{
			ctrl.UpdateRedShow();
			ctrl.UpdateUpShow();
		}
	}

	public void SetClick(Action<int> click)
	{
		mClick = click;
	}

	private void PlayRotate()
	{
		if ((bool)addani)
		{
			ShowAdd(value: true);
		}
	}

	private void StopRotate()
	{
		if (!addani)
		{
		}
	}

	public void DoWear()
	{
		PlayRotate();
	}

	public void WearOver()
	{
		StopRotate();
	}

	public void MissAdd()
	{
		ShowAdd(value: false);
	}

	private void ShowAdd(bool value)
	{
		addparent.SetActive(value);
		if (value)
		{
			if (equipdata != null && ctrl != null)
			{
				Text_Add.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_ClickChange");
			}
			else
			{
				Text_Add.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_ClickWear");
			}
		}
	}

	private void OnClick()
	{
		if (mClick != null)
		{
			mClick(index);
		}
	}

	public bool GetIsWear()
	{
		return equipdata != null;
	}

	public void SetButtonEnable(bool value)
	{
		button.enabled = value;
		buttonText.enabled = value;
	}

	public void UpdateButtonEnable()
	{
		SetButtonEnable(_equipdata != null);
	}

	public void Unwear(Vector3 endpos, Action<LocalSave.EquipOne> onFinish = null)
	{
		if (equipdata != null)
		{
			Sequence s = DOTween.Sequence();
			s.Append(ctrl.transform.DOMove(endpos, 0.3f).OnComplete(delegate
			{
				ctrl.transform.localPosition = Vector3.zero;
				ctrl.GetCanvasGroup().alpha = 1f;
				ctrl.gameObject.SetActive(value: false);
				if (onFinish != null)
				{
					onFinish(equipdata);
				}
				equipdata = null;
				UpdateBGShow();
			}));
			s.Join(DOTween.Sequence().AppendInterval(0.1f).Append(ctrl.GetCanvasGroup().DOFade(0f, 0.1f)));
		}
	}
}
