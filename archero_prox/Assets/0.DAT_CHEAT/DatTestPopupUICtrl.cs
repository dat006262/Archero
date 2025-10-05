using System.Collections;
using System.Collections.Generic;
using PureMVC.Interfaces;
using UnityEngine;

public class DatTestPopupUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Close;

    public override void OnLanguageChange()
    {
      
    }
    protected override void OnInit()
    {
       
        Button_Close.onClick = delegate
        {
            WindowUI.CloseWindow(WindowID.WindowID_DatTestPopup);
        };
    }
    protected override void OnOpen()
    {
      
    }
    
    protected override void OnClose()
    {
    }
    
    public override object OnGetEvent(string eventName)
    {
        if (eventName != null)
        {
            if (eventName == "GetPos")
            {
                return this.transform.position+Vector3.up*100;
            }
        }
        return null;
        return null;
    }

    public override void OnHandleNotification(INotification notification)
    {
        Debug.Log("DatTestPopupUICtrl: HandleNotification"+notification.Name);
    }

}
