




using UnityEngine;

public class FrontEventCtrl : MonoBehaviour
{
	private CanvasGroup mCanvasGroup;

	private Animator ani;

	private bool bInit;

	private void Awake()
	{
	}

	private void Init()
	{
		if (!bInit)
		{
			bInit = true;
			mCanvasGroup = GetComponent<CanvasGroup>();
			if (!mCanvasGroup)
			{
				mCanvasGroup = base.gameObject.AddComponent<CanvasGroup>();
			}
			ani = GetComponent<Animator>();
			if (!ani)
			{
				ani = base.gameObject.AddComponent<Animator>();
				AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController();
				animatorOverrideController.runtimeAnimatorController = ResourceManager.Load<RuntimeAnimatorController>("UIPanel/ACommon/FrontEventCtrl");
				animatorOverrideController.name = "FrontEventCtrlRunTime";
				ani.runtimeAnimatorController = animatorOverrideController;
				ani.updateMode = AnimatorUpdateMode.UnscaledTime;
			}
		}
	}

	public void Play(bool value)
	{
		Init();
		ani.Play((!value) ? "FrontEvent_Hide" : "FrontEvent_Show");
	}
}
