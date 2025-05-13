




using DG.Tweening;
using Dxx.Util;
using System.Collections;
using TableTool;
using UnityEngine;

public class CharUIHeroCtrl : MonoBehaviour
{
	public GameObject child;

	public GameObject[] petsparent;

	private BodyMask m_Body;

	private GameObject weaponobj;

	private GameObject[] pets = new GameObject[2];

	private int weaponid = -1;

	private int clothid = -1;

	private int[] petsid = new int[2]
	{
		-1,
		-1
	};

	private SequencePool mSeqPool = new SequencePool();

	private bool bChildShow;

	public void Show(bool value)
	{
		if (bChildShow != value)
		{
			bChildShow = value;
			if (!value)
			{
				mSeqPool.Clear();
				child.SetActive(value);
			}
			else
			{
				mSeqPool.Get().AppendInterval(1f).AppendCallback(delegate
				{
					child.SetActive(value: true);
					InitCloth(clothid);
					InitWeapon(weaponid);
					InitPet(0, petsid[0]);
					InitPet(1, petsid[1]);
				});
			}
		}
	}

	public void InitWeapon(int weaponid)
	{
		if (this.weaponid == weaponid)
		{
			return;
		}
		if (weaponobj != null)
		{
			UnityEngine.Object.Destroy(weaponobj);
			weaponobj = null;
		}
		this.weaponid = weaponid;
		if (!(m_Body == null))
		{
			GameObject gameObject = GameLogic.EffectGet(Utils.GetString("Game/WeaponHand/WeaponHand", weaponid));
			if ((bool)gameObject)
			{
				Weapon_weapon beanById = LocalModelManager.Instance.Weapon_weapon.GetBeanById(weaponid);
				gameObject.transform.parent = WeaponBase.GetWeaponNode(m_Body, beanById.WeaponNode);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localRotation = Quaternion.identity;
				weaponobj = gameObject;
				MeshRenderer[] componentsInChildren = weaponobj.GetComponentsInChildren<MeshRenderer>(includeInactive: true);
				weaponobj.ChangeChildLayer(LayerManager.UI);
			}
		}
	}

	public void InitCloth(int clothid)
	{
		if (!((this.clothid == clothid) & (m_Body != null)))
		{
			this.clothid = clothid;
			if (m_Body != null)
			{
				UnityEngine.Object.Destroy(m_Body.gameObject);
				m_Body = null;
			}
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(init_cloth());
			}
		}
	}

	private IEnumerator init_cloth()
	{
		yield return new WaitForSeconds(0.7f);
		string bodystring4 = LocalModelManager.Instance.Character_Char.GetBeanById(1001).ModelID;
		string bodystring2 = GetBodyString(clothid.ToString());
		string bodystring3 = string.Empty;
		if (clothid > 0)
		{
			bodystring3 = bodystring2;
		}
		else
		{
			bodystring3 = GetBodyString(bodystring4);
		}
		LoadSyncCtrl.Load(bodystring3, delegate(GameObject o2)
		{
			if (o2 == null)
			{
				SdkManager.Bugly_Report("CharUIHeroCtrl", Utils.FormatString("bodystring:{0} is create failed!", bodystring3));
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(o2);
			gameObject.SetParentNormal(child);
			gameObject.ChangeChildLayer(LayerManager.UI);
			if (m_Body != null)
			{
				UnityEngine.Object.Destroy(m_Body.gameObject);
				m_Body = null;
			}
			m_Body = gameObject.GetComponent<BodyMask>();
			int num = weaponid;
			weaponid = -1;
			InitWeapon(num);
		});
	}

	private string GetBodyString(string value)
	{
		return Utils.FormatString("Game/Models/{0}", value);
	}

	public void InitPet(int index, int petid)
	{
		if (index < 0 || index >= petsid.Length)
		{
			SdkManager.Bugly_Report("CharUIHeroCtrl", Utils.FormatString("InitPet index:{0} petid:{1} is out of range.", index, petid));
		}
		else if (petsid[index] != petid)
		{
			petsid[index] = petid;
			if (pets[index] != null)
			{
				UnityEngine.Object.Destroy(pets[index]);
			}
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(init_pet(index));
			}
		}
	}

	private IEnumerator init_pet(int index)
	{
		yield return new WaitForSeconds(0.9f);
		Skill_skill skill = LocalModelManager.Instance.Skill_skill.GetBeanById(petsid[index]);
		if (skill != null)
		{
			Character_Char chardata = LocalModelManager.Instance.Character_Char.GetBeanById(int.Parse(skill.Args[0]));
			Debugger.Log("chardata pet " + chardata.CharID);
			string bodystring = GetBodyString(chardata.ModelID);
			LoadSyncCtrl.Load(bodystring, delegate(GameObject o2)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(o2);
				gameObject.SetParentNormal(petsparent[index]);
				gameObject.ChangeChildLayer(LayerManager.UI);
				gameObject.transform.localScale = Vector3.one * chardata.BodyScale;
				BodyMask component = gameObject.GetComponent<BodyMask>();
				component.SetTextureWithoutInit(chardata.TextureID);
				pets[index] = gameObject;
			});
		}
	}
}
