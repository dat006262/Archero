




using Dxx.Util;
using UnityEngine;

public class ChallengeHideCtrl : MonoBehaviour
{
	public void Init()
	{
		GameLogic.Self.Event_PositionBy += OnPlayerMove;
	}

	private void OnPlayerMove(Vector3 pos)
	{
		Vector3 vector = Utils.World2Screen(GameLogic.Self.position);
		float x = vector.x;
		float y = vector.y;
		base.transform.position = new Vector3(x, y, 0f);
	}

	public void DeInit()
	{
		if ((bool)GameLogic.Self)
		{
			GameLogic.Self.Event_PositionBy -= OnPlayerMove;
		}
	}
}
