




using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class LayerBoxOneCtrl : MonoBehaviour
{
	public Image Image_BG;

	public Text Text_Stage;

	public Text Text_Count;

	public Box_ChapterBox m_Data
	{
		get;
		private set;
	}

	public void Init(Box_ChapterBox data)
	{
		m_Data = data;
		LocalSave.Instance.mStage.GetLayerBoxStageLayer(m_Data.Chapter, out int stage, out int layer);
		Text_Stage.text = GameLogic.Hold.Language.GetLanguageByTID("ChapterIndex_x", stage);
		Text_Count.text = layer.ToString();
	}
}
