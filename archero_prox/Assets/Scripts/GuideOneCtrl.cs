




using UnityEngine;
using UnityEngine.UI;

public class GuideOneCtrl : MonoBehaviour
{
	private RectTransform container;

	private Transform particle;

	private ParticleSystem ring;

	private float ringStartSize;

	private RectTransform arrow;

	private RectTransform effect;

	private HighLightMask mHighLight;

	private RectTransform target;

	private float arrowy;

	private void Awake()
	{
		mHighLight = base.transform.GetComponentInChildren<HighLightMask>();
		effect = (base.transform.Find("effect") as RectTransform);
		container = (effect.Find("container") as RectTransform);
		particle = effect.Find("particle");
		ring = particle.Find("Ring").GetComponent<ParticleSystem>();
		ringStartSize = ring.main.startSizeMultiplier;
		arrow = (container.Find("arrow") as RectTransform);
	}

	public void Init(RectTransform target)
	{
		this.target = target;
		mHighLight.SetTarget(target);
		ring.Clear();
		float sizeMax = GetSizeMax(target.sizeDelta);
		arrow.anchoredPosition = new Vector3(0f, sizeMax / 2f, 0f);
		container.sizeDelta = target.sizeDelta;
		container.anchoredPosition = Vector2.zero;
        //@TODO ParticleSystem
        var main = ring.main;
        main.startSizeMultiplier = ringStartSize * sizeMax / 200f;
		effect.position = target.position;
	}

	private void Update()
	{
		if ((bool)effect && (bool)target)
		{
			effect.position = target.position;
		}
	}

	private float GetSizeMax(Vector2 size)
	{
		return (!(size.x > size.y)) ? size.y : size.x;
	}
}
