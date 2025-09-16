




using Dxx.Net;
using Dxx.Util;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	public enum ButtonType
	{
		ButtonCtrl_Scale = 0,
		ButtonCtrl_Down20 = 11,
		ButtonCtrl_Down15 = 12,
		ButtonCtrl_Down10 = 13,
		ButtonCtrl_Static = 100,
		ButtonCtrl_ScaleDown20 = 51,
		ButtonCtrl_ScaleDown15 = 52,
		ButtonCtrl_ScaleDown10 = 53
	}

	private static Material _gray;

	private string ButtonCtrl_DownString = "ButtonCtrl_Scale_Down";

	private string ButtonCtrl_UpString = "ButtonCtrl_Scale_Up";

	private string ButtonCtrl_DisableString = "ButtonCtrl_Scale_Disable";

	[SerializeField]
	private ButtonType mType;

	public Action onClick;

	public Action onDown;

	public Action onDisable;

	private bool bDown;

	private bool bEnter;

	private long scrollCount;

	private Animator ani;

	[SerializeField]
	protected SoundButtonType Button_ClickSound = SoundButtonType.eButton_Small;

	protected bool bEnable = true;

	private Image[] mImages;

	private Text[] mTexts;

	private Color[] mTextsColor;

	private bool bDepondNet;

	private string disable_tips = string.Empty;

	public static Material GrayMaterial
	{
		get
		{
			if (_gray == null)
			{
				_gray = ResourceManager.Load<Material>("UIMaterial/GrayMaterial");
			}
			return _gray;
		}
	}

	private void Awake()
	{
		AddClip();
		mImages = GetComponentsInChildren<Image>(includeInactive: true);
		mTexts = GetComponentsInChildren<Text>(includeInactive: true);
		UpdateTextsColor();
		OnAwake();
	}

	protected virtual void OnAwake()
	{
	}

	private void OnEnable()
	{
		EnableReset();
	}

	public virtual void SetEnable(bool value)
	{
		if (bEnable != value)
		{
			bEnable = value;
			SetGray(value);
		}
	}

	public void SetGray(bool value)
	{
		SetImageMaterial((!value) ? GrayMaterial : null);
		SetTextsColor(value);
	}

	public void SetDepondNet(bool value)
	{
		bDepondNet = value;
	}

	private void SetImageMaterial(Material mat)
	{
		int i = 0;
		for (int num = mImages.Length; i < num; i++)
		{
			mImages[i].material = mat;
		}
	}

	private void UpdateTextsColor()
	{
		mTextsColor = new Color[mTexts.Length];
		int i = 0;
		for (int num = mTexts.Length; i < num; i++)
		{
			mTextsColor[i] = mTexts[i].color;
		}
	}

	private void SetTextsColor(bool value)
	{
		int i = 0;
		for (int num = mTextsColor.Length; i < num; i++)
		{
			mTexts[i].color = ((!value) ? Color.white : mTextsColor[i]);
		}
	}

	public void SetDisableTips(string tips)
	{
		disable_tips = tips;
	}

	private void AddClip()
	{
		if (mType != ButtonType.ButtonCtrl_Static)
		{
			Animator component = GetComponent<Animator>();
			if ((bool)component)
			{
				ani = component;
				ani.runtimeAnimatorController = null;
			}
			else
			{
				ani = base.gameObject.AddComponent<Animator>();
			}
			AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController();
			animatorOverrideController.runtimeAnimatorController = ResourceManager.Load<RuntimeAnimatorController>("Game/UI/Button/ButtonPlayCtrl");
			string @string = Utils.GetString("Game/UI/Button/", mType.ToString(), "_Down");
			animatorOverrideController[ButtonCtrl_DownString] = ResourceManager.Load<AnimationClip>(@string);
			ButtonCtrl_DownString = @string;
			string string2 = Utils.GetString("Game/UI/Button/", mType.ToString(), "_Up");
			animatorOverrideController[ButtonCtrl_UpString] = ResourceManager.Load<AnimationClip>(string2);
			ButtonCtrl_UpString = string2;
			animatorOverrideController["ButtonCtrl_Disable"] = ResourceManager.Load<AnimationClip>("Game/UI/Button/ButtonCtrl_Disable");
			animatorOverrideController.name = Utils.FormatString("ButtonPlayCtrlRunTime_{0}", mType.ToString());
			ani.runtimeAnimatorController = animatorOverrideController;
			ani.updateMode = AnimatorUpdateMode.UnscaledTime;
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		bDown = true;
		if (onDown != null)
		{
			onDown();
			OnDownVirtual();
		}
		PlayDown();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (bEnter && bDown)
		{
			PlayUp();
			if (bEnable && !eventData.dragging)
			{
				if (Button_ClickSound != 0 && (bool)GameLogic.Hold && (bool)GameLogic.Hold.Sound)
				{
					GameLogic.Hold.Sound.PlayUI((int)Button_ClickSound);
				}
				if (onClick != null)
				{
					StartCoroutine(startI(onClick));
				}
			}
		}
		bDown = false;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		bEnter = true;
		if (bDown)
		{
			PlayDown();
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		bEnter = false;
		if (bDown)
		{
			PlayUp();
		}
	}

	private IEnumerator startI(Action action)
	{
		if (action == null)
		{
			yield break;
		}
		yield return null;
		yield return null;
		Debug.LogFormat("ButtonCtrl startI bDepondNet:{0},IsNetConnect:{1}", bDepondNet, NetManager.IsNetConnect);
        if (bDepondNet)
		{
            if (NetManager.IsNetConnect)
			{
				action();
			}
			else
			{
				CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
			}
		}
		else
		{
			action();
		}
	}

	private IEnumerator startI2(Action<ButtonCtrl> action)
	{
		if (action != null)
		{
			yield return null;
			yield return null;
			action(this);
		}
	}

	private void PlayDown()
	{
		if (!ani)
		{
			return;
		}
		if (bEnable)
		{
			ani.Play("ButtonCtrl_Down");
			return;
		}
		if (!string.IsNullOrEmpty(disable_tips))
		{
			CInstance<TipsUIManager>.Instance.Show(disable_tips);
		}
		ani.Play("ButtonCtrl_Disable");
		if (onDisable != null)
		{
			onDisable();
		}
	}

	private void PlayUp()
	{
		if ((bool)ani && bEnable)
		{
			ani.Play("ButtonCtrl_Up");
		}
	}

	protected virtual void OnDownVirtual()
	{
	}

	protected virtual void OnUpVirtual()
	{
	}

	private void EnableReset()
	{
		if ((bool)ani && bEnable)
		{
			ani.Play("ButtonCtrl_Up", -1, 1f);
		}
	}
}
