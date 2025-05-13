




using UnityEngine;

public abstract class PauseObject : MonoBehaviour
{
	protected bool useLateUpdate;

	private int DeltaTime;

	private void Update()
	{
		if (!GameLogic.Paused && Time.timeScale > 0f)
		{
			UpdateProcess();
		}
	}

	protected virtual void UpdateProcess()
	{
	}
}
