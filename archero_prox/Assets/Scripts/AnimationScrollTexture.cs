




using UnityEngine;

public class AnimationScrollTexture : MonoBehaviour
{
	private Material mRenderMat;

	public float SpeedX = 0.25f;

	public float SpeedY = 0.25f;

	private float offsetX;

	private float offsetY;

	private Vector2 offset = default(Vector2);

	private void Awake()
	{
		mRenderMat = GetComponent<Renderer>().material;
	}

	private void Update()
	{
		offsetX += Time.deltaTime * SpeedX;
		offsetY += Time.deltaTime * SpeedY;
		offsetX = Mathf.Repeat(offsetX, 1f);
		offsetY = Mathf.Repeat(offsetY, 1f);
		offset.Set(offsetX, offsetY);
		mRenderMat.mainTextureOffset = offset;
	}
}
