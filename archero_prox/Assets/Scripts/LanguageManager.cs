




using Dxx.Util;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

public class LanguageManager
{
	public const string CN_s = "CN_s";

	public const string CN_t = "CN_t";

	public const string EN = "EN";

	public const string FR = "FR";

	public const string DE = "DE";

	public const string ID = "ID";

	public const string JP = "JP";

	public const string KR = "KR";

	public const string PT_BR = "PT_BR";

	public const string RU = "RU";

	public const string ES_ES = "ES_ES";

	public static Dictionary<string, string> languagedic = new Dictionary<string, string>
	{
		{
			"EN",
			"English"
		},
		{
			"CN_s",
			"简体中文"
		},
		{
			"KR",
			"한국어"
		},
		{
			"JP",
			"日本語"
		},
		{
			"FR",
			"Français"
		},
		{
			"DE",
			"Deutsch"
		},
		{
			"ES_ES",
			"Español"
		},
		{
			"PT_BR",
			"Português"
		},
		{
			"ID",
			"Bahasa Indonesia"
		},
		{
			"RU",
			"русский"
		},
		{
			"CN_t",
			"繁體中文"
		}
	};

	public static Dictionary<SystemLanguage, string> m_LanguageIDMap = new Dictionary<SystemLanguage, string>
	{
		{
			SystemLanguage.Chinese,
			"CN_s"
		},
		{
			SystemLanguage.ChineseSimplified,
			"CN_s"
		},
		{
			SystemLanguage.ChineseTraditional,
			"CN_t"
		},
		{
			SystemLanguage.English,
			"EN"
		},
		{
			SystemLanguage.Afrikaans,
			"EN"
		},
		{
			SystemLanguage.Arabic,
			"EN"
		},
		{
			SystemLanguage.Basque,
			"EN"
		},
		{
			SystemLanguage.Belarusian,
			"RU"
		},
		{
			SystemLanguage.Bulgarian,
			"EN"
		},
		{
			SystemLanguage.Catalan,
			"EN"
		},
		{
			SystemLanguage.Czech,
			"EN"
		},
		{
			SystemLanguage.Danish,
			"EN"
		},
		{
			SystemLanguage.Dutch,
			"EN"
		},
		{
			SystemLanguage.Estonian,
			"EN"
		},
		{
			SystemLanguage.Faroese,
			"EN"
		},
		{
			SystemLanguage.Finnish,
			"EN"
		},
		{
			SystemLanguage.French,
			"FR"
		},
		{
			SystemLanguage.German,
			"DE"
		},
		{
			SystemLanguage.Greek,
			"EN"
		},
		{
			SystemLanguage.Hebrew,
			"EN"
		},
		{
			SystemLanguage.Hungarian,
			"EN"
		},
		{
			SystemLanguage.Icelandic,
			"EN"
		},
		{
			SystemLanguage.Indonesian,
			"ID"
		},
		{
			SystemLanguage.Italian,
			"EN"
		},
		{
			SystemLanguage.Japanese,
			"JP"
		},
		{
			SystemLanguage.Korean,
			"KR"
		},
		{
			SystemLanguage.Latvian,
			"EN"
		},
		{
			SystemLanguage.Lithuanian,
			"EN"
		},
		{
			SystemLanguage.Norwegian,
			"EN"
		},
		{
			SystemLanguage.Polish,
			"EN"
		},
		{
			SystemLanguage.Portuguese,
			"PT_BR"
		},
		{
			SystemLanguage.Romanian,
			"EN"
		},
		{
			SystemLanguage.Russian,
			"RU"
		},
		{
			SystemLanguage.SerboCroatian,
			"EN"
		},
		{
			SystemLanguage.Slovak,
			"EN"
		},
		{
			SystemLanguage.Slovenian,
			"EN"
		},
		{
			SystemLanguage.Spanish,
			"ES_ES"
		},
		{
			SystemLanguage.Swedish,
			"EN"
		},
		{
			SystemLanguage.Thai,
			"EN"
		},
		{
			SystemLanguage.Turkish,
			"EN"
		},
		{
			SystemLanguage.Ukrainian,
			"EN"
		},
		{
			SystemLanguage.Unknown,
			"EN"
		},
		{
			SystemLanguage.Vietnamese,
			"EN"
		}
	};

	private SystemLanguage CurrentLanguage;

	private Dictionary<string, Language_lauguage> m_LanguageList;

	private int argsLength;

	private int geti;

	private string containstring;

	private string currentstring;

	private Dictionary<int, string> mStageIndexs = new Dictionary<int, string>
	{
		{
			1,
			"I"
		},
		{
			2,
			"II"
		},
		{
			3,
			"III"
		},
		{
			4,
			"IV"
		},
		{
			5,
			"V"
		},
		{
			6,
			"VI"
		},
		{
			7,
			"VII"
		},
		{
			8,
			"VIII"
		},
		{
			9,
			"IX"
		},
		{
			10,
			"X"
		},
		{
			11,
			"XI"
		},
		{
			12,
			"XII"
		}
	};

	public string Level => GetLanguageByTID("EquipUI_Level");

	public string Count => GetLanguageByTID("EquipUI_Count");

	public LanguageManager()
	{
		m_LanguageList = LocalModelManager.Instance.Language_lauguage.GetBeanDic();
		int @int = PlayerPrefsEncrypt.GetInt("LocalLanguage", -1);
		if (@int < 0)
		{
			CurrentLanguage = Application.systemLanguage;
			if (CurrentLanguage == SystemLanguage.Chinese)
			{
				CurrentLanguage = SystemLanguage.ChineseSimplified;
			}
		}
		else
		{
			CurrentLanguage = (SystemLanguage)@int;
		}
	}

	public string GetLanguageByTID(string tid, params object[] args)
	{
		argsLength = args.Length;
		Language_lauguage value = null;
		m_LanguageList.TryGetValue(tid, out value);
		if (value == null)
		{
			SdkManager.Bugly_Report("GetLanguageByTID", Utils.FormatString("TID:[{0}] is not invalid.", tid));
			return string.Empty;
		}
		currentstring = GetString(value);
		for (geti = 1; geti <= argsLength; geti++)
		{
			containstring = Utils.GetString("%s", geti);
			if (currentstring.Contains(containstring))
			{
				currentstring = currentstring.Replace(containstring, args[geti - 1].ToString());
			}
			else
			{
				SdkManager.Bugly_Report("LanguageManager.GetLanguageByTID", CurrentLanguage.ToString() + " tid:" + tid + " dont have " + containstring);
			}
		}
		if (currentstring.Contains(Utils.GetString("%s", geti)))
		{
			SdkManager.Bugly_Report("LanguageManager.GetLanguageByTID", " tid:" + tid + " need more args!!!!");
		}
		currentstring = currentstring.Replace("\\n", "\n");
		return currentstring;
	}

	public string GetEquipSpecialInfo(int equipid)
	{
		string key = Utils.FormatString("装备特性描述{0}", equipid);
		string result = string.Empty;
		Language_lauguage value = null;
		if (m_LanguageList.TryGetValue(key, out value))
		{
			result = GetString(value);
		}
		return result;
	}

	public string GetSkillName(int skillId)
	{
		int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId).SkillIcon;
		return GetLanguageByTID(Utils.GetString("技能名称", skillIcon));
	}

	public string GetSkillContent(int skillId)
	{
		int skillIcon = LocalModelManager.Instance.Skill_skill.GetBeanById(skillId).SkillIcon;
		return GetLanguageByTID(Utils.GetString("技能描述", skillIcon));
	}

	private string GetString(Language_lauguage language)
	{
		if (!m_LanguageIDMap.ContainsKey(CurrentLanguage))
		{
			return language.EN;
		}
		switch (m_LanguageIDMap[CurrentLanguage])
		{
		case "CN_s":
			return language.CN_s;
		case "CN_t":
			return language.CN_t;
		case "EN":
			return language.EN;
		case "FR":
			return language.FR;
		case "DE":
			return language.DE;
		case "ID":
			return language.ID;
		case "JP":
			return language.JP;
		case "KR":
			return language.KR;
		case "PT_BR":
			return language.PT_BR;
		case "RU":
			return language.RU;
		case "ES_ES":
			return language.ES_ES;
		default:
			return string.Empty;
		}
	}

	public void ChangeLanguage(string language)
	{
		Dictionary<SystemLanguage, string>.Enumerator enumerator = m_LanguageIDMap.GetEnumerator();
		do
		{
			if (!enumerator.MoveNext())
			{
				return;
			}
		}
		while (!(enumerator.Current.Value == language));
		CurrentLanguage = enumerator.Current.Key;
		UnityEngine.Debug.Log("change language -> " + CurrentLanguage);
		PlayerPrefsEncrypt.SetInt("LocalLanguage", (int)CurrentLanguage);
		Facade.Instance.SendNotification("PUB_LANGUAGE_UPDATE");
	}

	public SystemLanguage GetLanguage()
	{
		return CurrentLanguage;
	}

	public string GetLanguageString()
	{
		if (m_LanguageIDMap.ContainsKey(CurrentLanguage))
		{
			return m_LanguageIDMap[CurrentLanguage];
		}
		return "EN";
	}

	public string GetRomanNumber(int value)
	{
		string value2 = string.Empty;
		if (mStageIndexs.TryGetValue(value, out value2))
		{
			return value2;
		}
		return string.Empty;
	}

	public string GetSecond(int second)
	{
		return GetLanguageByTID("倒计时_s", second);
	}

	public string GetStageLayer(int MaxLevel)
	{
		LocalSave.Instance.mStage.GetLayerBoxStageLayer(MaxLevel, out int stage, out int layer);
		return GetLanguageByTID("stagelist_stage_layer", stage, layer);
	}
}
