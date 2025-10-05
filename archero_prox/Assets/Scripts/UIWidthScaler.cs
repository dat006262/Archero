




using UnityEngine;
using UnityEditor; 
#if UNITY_EDITOR
[CustomEditor(typeof(UIWidthScaler))]
public class UIWidthScalerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		EditorGUILayout.HelpBox("Scale theo design 720:1280 - 9:16", MessageType.Info);
		DrawDefaultInspector();
	}
}
#endif
public class UIWidthScaler : MonoBehaviour
{
	private void Start()
	{
		base.transform.localScale = Vector3.one * GameLogic.WidthScaleAll;
		base.enabled = false;
	}
}
