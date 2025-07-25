using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DatCheatState
{
    public static bool isHeroNeverDead = false;
}

public class DatCheatToolBar : EditorWindow
{
    [MenuItem("DatCheat/Hero Never Dead")]
    public static void MakeHeroNeverDead()
    {
        Debug.Log("isHeroNeverDead = true");
        DatCheatState.isHeroNeverDead = true;
    }

    [MenuItem("DatCheat/Hero Can Dead")]
    public static void MakeHeroCanDead()
    {
        Debug.Log("isHeroNeverDead = false");
        DatCheatState.isHeroNeverDead = false;
    }

    [MenuItem("DatCheat/Get100exp")]
    public static void Get100exp()
    {
        LocalSave.Instance.Modify_Exp(100, true);
     
        Debug.Log("Player current level:"+ LocalSave.Instance.GetLevel()+"need "+LocalSave.Instance.GetExpByLevel( LocalSave.Instance.GetLevel()) + ".And exp:" + LocalSave.Instance.GetExp());
        LocalSave.Instance.LevelUp();
    }
    [MenuItem("DatCheat/Get1000exp")]
    public static void Get100exp10times()
    {
        LocalSave.Instance.Modify_Exp(1000, true);
     
        Debug.Log("Player current level:"+ LocalSave.Instance.GetLevel()+"need "+LocalSave.Instance.GetExpByLevel( LocalSave.Instance.GetLevel()) + ".And exp:" + LocalSave.Instance.GetExp());
        LocalSave.Instance.LevelUp();
    }
 
}