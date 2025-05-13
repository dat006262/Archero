




using UnityEngine;

public class TargetRedCtrl : MonoBehaviour
{
	private Transform child;

	private void Awake()
	{
		child = base.transform.Find("child");
	}

	private void Update()
	{
		base.transform.rotation = Quaternion.identity;
	}
}
