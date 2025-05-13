




using UnityEngine;

public class RealTimeParticle : MonoBehaviour
{
	[SerializeField]
	private bool withChildren = true;

	private ParticleSystem _particle;

	private ParticleSystem[] _particles;

	private int _particles_count;

	private float _deltaTime;

	private float _timeAtLastFrame;

	private void Awake()
	{
		_particle = GetComponent<ParticleSystem>();
		if (withChildren)
		{
			_particles = GetComponentsInChildren<ParticleSystem>();
			_particles_count = _particles.Length;
		}
	}

	private void Update()
	{
		_deltaTime = Time.realtimeSinceStartup - _timeAtLastFrame;
		_timeAtLastFrame = Time.realtimeSinceStartup;
		if (!((double)Mathf.Abs(Time.timeScale) < 1E-06))
		{
			return;
		}
		if (withChildren)
		{
			for (int i = 0; i < _particles_count; i++)
			{
				_particles[i].Simulate(_deltaTime, withChildren: false, restart: false);
				_particles[i].Play();
			}
		}
		else if ((bool)_particle)
		{
			_particle.Simulate(_deltaTime, withChildren: false, restart: false);
			_particle.Play();
		}
	}
}
