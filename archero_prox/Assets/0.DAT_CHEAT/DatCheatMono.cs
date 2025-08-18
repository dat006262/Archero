using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Sirenix.OdinInspector;
using TableTool;
using UnityEngine;

public class DatCheatMono : MonoBehaviour
{
    [Button]
    public static void Getexp(int exp)
    {
        LocalSave.Instance.Modify_Exp(exp, true);
     
        Debug.Log("Player current level:"+ LocalSave.Instance.GetLevel()+"need "+LocalSave.Instance.GetExpByLevel( LocalSave.Instance.GetLevel()) + ".And exp:" + LocalSave.Instance.GetExp());
        LocalSave.Instance.LevelUp();
    }
    [Button]
    public static void SetStage(int stage=1)
    {
        LocalSave.Instance.mStage.InitMaxLevel(stage*50);
    }
    [Button]
    public void OpenWindow(WindowID windowID)
    {
        WindowUI.ShowWindow(windowID);
    }

    [Button]
    public void TestNotification()
    {
        Facade.Instance.SendNotification("DatTESTEVENT");
    }
    [Button]
    public void TestEvent()
    {
        Facade.Instance.SendNotification("UseCurrencyKey");
    }
}
