




using UnityEngine;

public class BulletBombDodge_effect : MonoBehaviour
{
	public ParticleSystem effect01;

	public ParticleSystem effect02;

	public ParticleSystem effect03;

	public ParticleSystem effect04;

	private Renderer[] renderers;

	private short[] counts01;

	private ParticleSystem.MinMaxCurve curve02_init;

	private ParticleSystem.MinMaxCurve curve02;

	private ParticleSystem.MinMaxCurve curve04_init;

	private ParticleSystem.MinMaxCurve curve04;

	private void Awake()
	{
		counts01 = new short[effect01.emission.burstCount];
		for (int i = 0; i < counts01.GetLength(0); i++)
		{
			counts01[i] = effect01.emission.GetBurst(i).maxCount;
		}
		curve02 = default(ParticleSystem.MinMaxCurve);
		curve02.constant = effect02.emission.rateOverTime.constant;
		curve02_init = default(ParticleSystem.MinMaxCurve);
		curve02_init.constant = curve02.constant;
		curve04 = default(ParticleSystem.MinMaxCurve);
		curve04.constant = effect04.emission.rateOverTime.constant;
		curve04_init = default(ParticleSystem.MinMaxCurve);
		curve04_init.constant = curve04.constant;
		renderers = base.transform.GetComponentsInChildren<Renderer>(includeInactive: true);
	}

	public void SetScale(float value)
	{
		Vector3 position = base.transform.position;
		int sortingOrder = (int)(0f - position.z);
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].sortingLayerName = "Player";
			renderers[i].sortingOrder = sortingOrder;
		}
		setcount(effect01.emission, counts01, value);
		curve02.constant = curve02_init.constant * value;
        
        //@TODO ParticleSystem
        var emiss02 = effect02.emission;
        emiss02.rateOverTime = curve02;

        curve04.constant = curve04_init.constant * value;
        
        //@TODO ParticleSystem
        var emiss04 = effect02.emission;
        emiss04.rateOverTime = curve04;
	}

	private void setcount(ParticleSystem.EmissionModule emission, short[] counts, float scale)
	{
		ParticleSystem.Burst[] array = new ParticleSystem.Burst[emission.burstCount];
		emission.GetBursts(array);
		for (int i = 0; i < array.Length; i++)
		{
			array[i].maxCount = (short)((float)counts[i] * scale);
		}
		emission.SetBursts(array);
	}
}
