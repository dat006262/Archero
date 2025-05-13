




using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class AttributeCtrlBase
{
	protected EntityBase m_Entity;

	private string _className;

	private int _classid;

	private List<Goods_goods.GoodData> list = new List<Goods_goods.GoodData>();

	private GameObject effect;

	private bool bInit;

	public string ClassName => _className;

	public int ClassID => _classid;

	public Skill_alone m_Data
	{
		get;
		private set;
	}

	public Skill_skill m_SkillData
	{
		get;
		private set;
	}

	private void ExcuteAttributes()
	{
		string[] attributes = m_Data.Attributes;
		int i = 0;
		for (int num = attributes.Length; i < num; i++)
		{
			list.Add(Goods_goods.GetGoodData(attributes[i]));
		}
	}

	private void InstallAttrs(int symbol)
	{
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			Goods_goods.GoodData goodData = list[i];
			long value = goodData.value * symbol;
			m_Entity.m_EntityData.ExcuteAttributes(goodData.goodType, value);
		}
	}

	public void Install(EntityBase entity, Skill_skill skilldata, Skill_alone skill, params object[] args)
	{
		bInit = true;
		_className = GetType().ToString();
		string s = ClassName.Substring(ClassName.Length - 4, 4);
		int.TryParse(s, out _classid);
		m_Entity = entity;
		m_Data = skill;
		m_SkillData = skilldata;
		ExcuteAttributes();
		CreateEffect();
		InstallAttrs(1);
		if (args.Length > 0)
		{
			OnInstall(args);
		}
		else
		{
			OnInstall();
		}
	}

	private void CreateEffect()
	{
		Fx_fx beanById = LocalModelManager.Instance.Fx_fx.GetBeanById(m_Data.CreateEffectID);
		if (beanById != null)
		{
			effect = GameLogic.EffectGet(beanById.Path);
			effect.transform.parent = m_Entity.GetKetNode(beanById.Node);
			effect.transform.localPosition = Vector3.zero;
			effect.transform.localRotation = Quaternion.identity;
			effect.transform.localScale = Vector3.one;
		}
	}

	private void RemoveEffect()
	{
		GameLogic.EffectCache(effect);
	}

	protected virtual void OnInstall(params object[] args)
	{
		OnInstall();
	}

	protected virtual void OnInstall()
	{
	}

	public void Uninstall()
	{
		if (bInit)
		{
			bInit = false;
			RemoveEffect();
			InstallAttrs(-1);
			OnUninstall();
		}
	}

	protected virtual void OnUninstall()
	{
	}
}
