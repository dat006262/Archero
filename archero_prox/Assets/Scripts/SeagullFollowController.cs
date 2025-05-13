




using UnityEngine;

public class SeagullFollowController : MonoBehaviour
{
	public float speed = 1f;

	public float dampTime = 2f;

	public Transform targetTransform;

	public float scatter;

	public float scatterSpeed = 1f;

	public float orthoCamScale = 1f;

	public bool isUseOffset;

	public Vector3 offset;

	private Animator animator;

	private int animFacingParam = Animator.StringToHash("Facing");

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void Update()
	{
		Vector3 a = targetTransform.position;
		if (isUseOffset)
		{
			a += offset * orthoCamScale;
		}
		a.x += scatter * Mathf.Sin(Time.time * scatterSpeed);
		Vector3 upwards = a - base.transform.position;
		Quaternion b = Quaternion.LookRotation(Vector3.forward, upwards);
		base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, dampTime * Time.deltaTime);
		base.transform.position += base.transform.up * speed * orthoCamScale * Time.deltaTime;
		animator.SetFloat(animFacingParam, Vector3.Dot(base.transform.up, Vector3.down));
	}
}
