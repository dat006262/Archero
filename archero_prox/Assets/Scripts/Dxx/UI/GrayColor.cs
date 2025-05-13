




using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dxx.UI
{
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Effects/Gray Color")]
	[RequireComponent(typeof(Graphic))]
	[DisallowMultipleComponent]
	public class GrayColor : UIBehaviour
	{
		public Material hueMaterial;

		private Graphic m_Graphic;

		public Graphic graphic
		{
			get
			{
				if (m_Graphic == null)
				{
					m_Graphic = GetComponent<Graphic>();
				}
				return m_Graphic;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			graphic.material = hueMaterial;
			graphic.material.SetColor("_Color", graphic.color);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			graphic.material = null;
		}
	}
}
