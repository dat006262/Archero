using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using PureMVC.Patterns;
using Sirenix.OdinInspector;
using TableTool;
using TMPro;
using UnityEngine;

public class DatCheatMono : MonoBehaviour
{
    [Button]
    public static void Getexp(int exp)
    {
        LocalSave.Instance.Modify_Exp(exp, true);

        Debug.Log("Player current level:" + LocalSave.Instance.GetLevel() + "need " +
                  LocalSave.Instance.GetExpByLevel(LocalSave.Instance.GetLevel()) + ".And exp:" +
                  LocalSave.Instance.GetExp());
        LocalSave.Instance.LevelUp();
    }

    [Button]
    public static void SetStage(int stage = 1)
    {
        LocalSave.Instance.mStage.InitMaxLevel(stage * 50);
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

    public List<Stage_Level_chapter7> stageLevelChapter7s = new List<Stage_Level_chapter7>();

    [Button]
    public bool TryToReadDataStageLevel7()
    {
        stageLevelChapter7s = new List<Stage_Level_chapter7>();
        Stage_Level_chapter7Model tesstModel = new Stage_Level_chapter7Model();
        Debug.Log(tesstModel.GetAllBeans().Count);
        foreach (var VARIABLE in tesstModel.GetAllBeans())
        {
            stageLevelChapter7s.Add(VARIABLE.Copy());
        }
        return true;
    }

    [Button]
    public bool TryToWriteDataStageLevel7()
    {
        Stage_Level_chapter7Model tesstModel = new Stage_Level_chapter7Model();
        bool                      result     = tesstModel.WriteToByte(stageLevelChapter7s);
        UnityEditor.AssetDatabase.Refresh();

        return result;
    }
    //ExamCreateLocalData
    public List<Dat_TEST_LocalDATA> TESTDATA = new List<Dat_TEST_LocalDATA>();
    [Button]
    public bool ExamCreateFileByteLocalData()
    {



        Dat_TEST_LocalDATAModel tesstModel = new Dat_TEST_LocalDATAModel();
      bool result = tesstModel.WriteToByte(TESTDATA);
      UnityEditor.AssetDatabase.Refresh();

      return result;
    }
    [Button]
    public void ExamGetFileByteLocalData()
    {
        Dat_TEST_LocalDATAModel tesstModel = new Dat_TEST_LocalDATAModel();
        TESTDATA = new List<Dat_TEST_LocalDATA>();
        foreach (var VARIABLE in tesstModel.GetAllBeans())
        {
            TESTDATA.Add(VARIABLE.Copy());
        }
    }
}
[System.Serializable]
public class Dat_TEST_LocalDATA : LocalBean
{
    public int    stt;
    public bool   bool1;
    public float  float1;
    public double double1;
    public short short1;
    public long   long1;
    public DateTime DateTime1;
    public int[]  ArrayInt1;
    public double[] ArrayDouble1;
    public bool[]  ArrayBool1;
    public float[] ArrayFloat1;
    public short[] ArrayShort1;
    public long[]  ArrayLong1;
    public string Name; 
    // Các dạng dữ liệu được hỗ trợ:
    // Short ,Bool , Int, DateTime, Float, Double, ArrayInt, ArrayString, ArrayDouble, ArrayFloat, ArrayBool , ArrayShort, ArrayLong , 
    public string[] TESTDATArray;

    protected override bool ReadImpl()
    {
        stt          = readInt();
        Name         = readLocalString();
        bool1        = readBool();
        float1       = readFloat();
        double1      = readDouble();
        short1       = readShort();
        long1        = readLong();
        DateTime1    = readDate();
        ArrayInt1    = readArrayint();
        ArrayDouble1 = readArraydouble();
        ArrayBool1   = readArraybool();
        ArrayFloat1  = readArrayfloat();
        ArrayShort1  = readArrayshort();
        ArrayLong1   = readArraylong();
        TESTDATArray = readArraystring();
        return true;
    }

    protected override List<byte> WriteImpl()
    {
       writeInt(stt);
        writeLocalString(Name);
        writeBool(bool1);
        writeFloat(float1);
        writeDouble(double1);
        writeShort(short1);
        writeLong(long1);
        writeDate(DateTime1);
        writeArrayInt(ArrayInt1);
        writeArrayDouble(ArrayDouble1);
        writeArrayBool(ArrayBool1);
        writeArrayFloat(ArrayFloat1);
        writeArrayShort(ArrayShort1);
        writeArrayLong(ArrayLong1);
        writeArrayString(TESTDATArray);
        return byteList;
    }
    public Dat_TEST_LocalDATA Copy()
    {
        Dat_TEST_LocalDATA Dat_TEST_LocalDATA = new Dat_TEST_LocalDATA();
        Dat_TEST_LocalDATA.stt          = stt;
        Dat_TEST_LocalDATA.Name         = Name;
        Dat_TEST_LocalDATA.bool1        = bool1;
        Dat_TEST_LocalDATA.float1       = float1;
        Dat_TEST_LocalDATA.double1      = double1;
        Dat_TEST_LocalDATA.short1       = short1;
        Dat_TEST_LocalDATA.long1        = long1;
        Dat_TEST_LocalDATA.DateTime1    = DateTime1;
        Dat_TEST_LocalDATA.ArrayInt1    = ArrayInt1;
        Dat_TEST_LocalDATA.ArrayDouble1 = ArrayDouble1;
        Dat_TEST_LocalDATA.ArrayBool1   = ArrayBool1;
        Dat_TEST_LocalDATA.ArrayFloat1  = ArrayFloat1;
        Dat_TEST_LocalDATA.ArrayShort1  = ArrayShort1;
        Dat_TEST_LocalDATA.ArrayLong1   = ArrayLong1;
        Dat_TEST_LocalDATA.TESTDATArray = TESTDATArray;
  
        return Dat_TEST_LocalDATA;
    }




}
public class Dat_TEST_LocalDATAModel : LocalModel<Dat_TEST_LocalDATA, string>
{
    private const string _Filename = "Dat_TEST_LocalDATA";

    protected override string Filename => "Dat_TEST_LocalDATA";

    protected override string GetBeanKey(Dat_TEST_LocalDATA bean)
    {
        return bean.Name;
    }
}