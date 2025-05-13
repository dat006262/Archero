




using DG.Tweening;
using Dxx.UI;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipOneCtrl : MonoBehaviour
{
    public GameObject[] typeparent = new GameObject[2];

    public Text Text_Count;

    public ButtonCtrl mButton;

    public Text mButtonText;

    public Image Image_BG;

    public Image Image_Icon;

    public Image Image_Type;

    public GameObject levelparent;

    public Text Text_ID;

    public Text Text_Level;

    public CanvasGroup mCanvasGroup;

    public RedNodeCtrl mRedCtrl;

    public Transform child;

    public GameObject wearparent;

    public Transform upparent;

    public Action<EquipOneCtrl> OnClickEvent;

    private GrayColor[] mGrays;

    private EquipWearCtrl mWearCtrl;

    private bool bGray;

    private int bgquality = -1;

    private int iconid = -1;

    private bool bInit;

    private int mIndex;

    public LocalSave.EquipOne equipdata;

    private Tweener tweener_ani;

    private void Awake()
    {
        OnInit();
    }

    private void OnInit()
    {
        if (!bInit)
        {
            bInit = true;
            mButton.onClick = OnClickButton;
        }
    }

    public void Init(LocalSave.EquipOne equip)
    {
        equipdata = equip;
        Init();
    }

    public void Init()
    {
        OnInit();
        miss_all_type();
        SetBGShow(value: true);
        ShowAniEnable(value: true);
        if ((bool)Image_Type)
        {
            Sprite typeIcon = equipdata.TypeIcon;
            Image_Type.enabled = (typeIcon != null);
            Image_Type.sprite = typeIcon;
        }

        Debug.Log("@LOG EquipOneCtrl.Init:" + equipdata.PropType);
        switch (equipdata.PropType)
        {
            case EquipType.eEquip:
                ShowLevel(value: true);
                SetCountShow(value: false);
                type_show(0, value: true);
                Text_Level.text = Utils.FormatString("Lv.{0}", equipdata.Level);
                break;
            case EquipType.eMaterial:
                type_show(1, value: true);
                ShowLevel(value: false);
                SetCountShow(value: true);
                Text_Count.text = Utils.FormatString("x{0}", equipdata.Count);
                break;
            default:
                SdkManager.Bugly_Report("EquipOneCtrl", Utils.FormatString("Init Equip[{0}]  PropType[{1}] is not achieve! ", equipdata.EquipID, equipdata.PropType));
                break;
        }
        SetButtonEnable(value: true);
        SetBGQuality(equipdata.Quality);
        set_icon(equipdata.data.EquipIcon);
        if ((bool)Text_ID)
        {
            Text_ID.text = Utils.FormatString("ID: {0}", equipdata.RowID);
        }
        mRedCtrl.DestroyChild();
    }

    private void miss_all_type()
    {
        int i = 0;
        for (int num = typeparent.Length; i < num; i++)
        {
            typeparent[i].SetActive(value: false);
        }
    }

    private void type_show(int index, bool value)
    {
        typeparent[index].SetActive(value);
    }

    public void UpdateWear()
    {
        wearparent.SetActive(value: false);
        if (equipdata != null && equipdata.IsWear)
        {
            wearparent.SetActive(value: true);
            if (mWearCtrl == null)
            {
                mWearCtrl = CInstance<UIResourceCreator>.Instance.GetEquipWear(wearparent.transform);
            }
        }
    }

    public void UpdateRedShow()
    {
        if (equipdata != null)
        {
            if (equipdata.PropType == EquipType.eEquip && !equipdata.IsWear && LocalSave.Instance.Equip_GetIsEmpty(equipdata) && !LocalSave.Instance.Equip_is_same_wear(equipdata))
            {
                mRedCtrl.SetType(RedNodeType.eRedWear);
                mRedCtrl.Value = 1;
                LocalSave.Instance.mEquip.SetNew(equipdata.UniqueID);
            }
            else if (equipdata.bNew)
            {
                mRedCtrl.SetType(RedNodeType.eRedNew);
                mRedCtrl.Value = 1;
                LocalSave.Instance.mEquip.SetNew(equipdata.UniqueID);
            }
            else
            {
                mRedCtrl.SetType(RedNodeType.eRedCount);
                mRedCtrl.Value = 0;
                mRedCtrl.DestroyChild();
            }
        }
    }

    public void SetRedNodeType(RedNodeType type)
    {
        mRedCtrl.SetType(RedNodeType.eWarning);
        mRedCtrl.Value = 1;
    }

    public void UpdateUpShow()
    {
        SetUpShow(equipdata.IsWear && equipdata.CanLevelUp);
    }

    private void SetUpShow(bool value)
    {
        if ((bool)upparent)
        {
            upparent.gameObject.SetActive(value);
        }
        if (value && upparent.childCount == 0)
        {
            CInstance<UIResourceCreator>.Instance.GetEquipOne_UP(upparent);
        }
        else if (!value && upparent.childCount > 0)
        {
            upparent.DestroyChildren();
        }
    }

    public void SetBGShow(bool value)
    {
        if ((bool)Image_BG)
        {
            Image_BG.gameObject.SetActive(value);
        }
    }

    private void SetBGQuality(int quality)
    {
        if (bgquality != quality)
        {
            bgquality = quality;
            Image_BG.sprite = SpriteManager.GetCharUI(Utils.FormatString("CharUI_Quality{0}", quality));
        }
    }

    private void set_icon(int iconid)
    {
        if (this.iconid != iconid)
        {
            this.iconid = iconid;
            Image_Icon.sprite = equipdata.Icon;
        }
    }

    public void SetCountShow(bool value)
    {
        Text_Count.gameObject.SetActive(value);
    }

    public void SetButtonEnable(bool value)
    {
        mButton.enabled = value;
        if (mButtonText != null)
        {
            mButtonText.enabled = value;
        }
    }

    private void OnClickButton()
    {
        if (OnClickEvent != null)
        {
            OnClickEvent(this);
        }
    }

    public void ShowLevel(bool value)
    {
        levelparent.SetActive(value);
    }

    public void ShowAniEnable(bool value)
    {
    }

    public CanvasGroup GetCanvasGroup()
    {
        return mCanvasGroup;
    }
}
