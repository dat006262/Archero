




using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
[RequireComponent(typeof(Animator))]
public class ToggleCtrl : UIBehaviour
{
	private const string ISON_TRIGGER = "IsOn";

	private bool m_UseAnimation = true;

	private Toggle m_Toggle;

	private Animator m_Animator;

	public bool useAnimation
	{
		get
		{
			return m_UseAnimation;
		}
		set
		{
			animator.enabled = value;
			m_UseAnimation = value;
		}
	}

	public Toggle toggle
	{
		get
		{
			if (m_Toggle == null)
			{
				m_Toggle = GetComponent<Toggle>();
			}
			return m_Toggle;
		}
	}

	public Animator animator
	{
		get
		{
			if (m_Animator == null)
			{
				m_Animator = GetComponent<Animator>();
			}
			return m_Animator;
		}
	}

	protected override void Awake()
	{
		animator.updateMode = AnimatorUpdateMode.UnscaledTime;
		toggle.onValueChanged.AddListener(OnValueChange);
	}

	private void OnValueChange(bool isOn)
	{
		if (useAnimation)
		{
			animator.SetBool("IsOn", isOn);
		}
	}
}
