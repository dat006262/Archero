




using UnityEngine;

public class StrenghEffectCtrl : MonoBehaviour
{
	public ParticleSystem particle;

	private ParticleSystem.ShapeModule shape;

	private SkinnedMeshRenderer mesh;

	public void InitMesh(EntityBase entity)
	{
		shape = particle.shape;
		mesh = entity.m_Body.Body.GetComponent<SkinnedMeshRenderer>();
		shape.mesh = mesh.sharedMesh;
	}
}
