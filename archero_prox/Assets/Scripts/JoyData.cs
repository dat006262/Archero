




using Dxx.Util;
using UnityEngine;

public struct JoyData
{
	public string name;

	public Vector3 direction;

	public Vector3 _moveDirection;

	public float angle;

	public float length;

	public int type;

	public string action;

	public Vector3 MoveDirection => (!(_moveDirection == Vector3.zero)) ? _moveDirection : direction;

	public void Revert()
	{
		direction *= -1f;
		angle = (angle + 180f) % 360f;
	}

	public void UpdateDirectionByAngle(float angle)
	{
		this.angle = angle;
		direction.x = MathDxx.Sin(angle);
		direction.z = MathDxx.Cos(angle);
	}
}
