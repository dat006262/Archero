using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatTestPopupUIMediator : MediatorBase
{
    public override List<string> OnListNotificationInterests
    {
        get
        {
            List<string> list = new List<string>();
            list.Add("PUB_UI_UPDATE_CURRENCY");
            list.Add("DatTESTEVENT");
            list.Add("GetCurrency");
            list.Add("UseCurrencyKey");
            list.Add("CurrencyKeyRotate");
            return list;
        }
    }
    public DatTestPopupUIMediator() : base("PopupDatTest")
    {
    }
}
