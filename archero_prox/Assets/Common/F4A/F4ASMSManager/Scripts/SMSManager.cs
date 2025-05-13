




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using System.Linq;

namespace com.F4A.MobileThird
{
	public enum ESMSCountry : int
	{
		None = 0,
	    Thaidland = 1,
		Malaysia = 2,
	//Vietnam = 3,
	}
    
	public enum ESMSHomeNetwork : int
	{
		None = 0,
		Celcom = 1,
		Digi = 2,
		UMobile = 3,
		Maxis = 4,
		
		//Viettel = 5, // vn, only test
	}
	
	public enum ETypeLoadConfigSMS : int
	{
		None = 0,
		FileLocal = 1,
		FileServer = 2,
	}
	
	[System.Serializable]
	public class SMSConfigData
	{
		public bool enable = false;
		public ESMSCountry country = ESMSCountry.None;
		public string url = "";
		[JsonProperty("network_configs")]
		public SMSNetworkData[] networkConfigs;
	}
	
	[System.Serializable]
	public class SMSNetworkData
	{
		[JsonProperty("network")]
		public ESMSHomeNetwork homeNetwork = ESMSHomeNetwork.None;
		[JsonProperty("phone_service")]
		public string phoneServices = "";
		public string url = "";
	}
    
    public class SMSManager : SingletonMono<SMSManager>
	{
		[SerializeField]
		private ETypeLoadConfigSMS typeLoadConfigSMS = ETypeLoadConfigSMS.None;
		[SerializeField]
		private string urlConfigSMS = "URL config sms";
		[SerializeField]
		private TextAsset fileConfigSMS = null;
		
		public SMSConfigData[] configData = null;
		
		#region Unity Method
		// Awake is called when the script instance is being loaded.
        private void Start()
		{
            F4ACoreManager.OnDownloadF4AConfigCompleted += F4ACoreManager_OnDownloadF4AConfigCompleted;
		}

		private void OnDestroy()
		{
            F4ACoreManager.OnDownloadF4AConfigCompleted -= F4ACoreManager_OnDownloadF4AConfigCompleted;
		}

        #endregion

        #region Handles, Events
        private void F4ACoreManager_OnDownloadF4AConfigCompleted(F4AConfigData configData, bool success)
        {
            if (configData != null && !string.IsNullOrEmpty(configData.urlConfigSms))
            {
                urlConfigSMS = F4ACoreManager.Instance.ConfigData.urlConfigSms;
            }
        }
        #endregion

        private void GetDataConfigSMS()
        {
            string str = "";
            if (typeLoadConfigSMS == ETypeLoadConfigSMS.FileLocal)
            {
                str = fileConfigSMS.text;
            }
            else if (typeLoadConfigSMS == ETypeLoadConfigSMS.FileServer)
            {

            }

            if (!string.IsNullOrEmpty(str))
            {
                PlayerPrefs.SetString("SMSConfigData", str);
                configData = JsonConvert.DeserializeObject<SMSConfigData[]>(str);
            }
        }


		private ESMSCountry GetSimCountryFromSimCountryIso(string countryIso)
		{
			if(string.IsNullOrEmpty(countryIso))
				return ESMSCountry.None;
			countryIso = countryIso.ToLower();
			switch(countryIso){
				case "my":
					return ESMSCountry.Malaysia;
				case "th":
					return ESMSCountry.Thaidland;
				default:
					return ESMSCountry.None;
			}
		}
		
		private ESMSHomeNetwork GetHomeNetworkFromNetworkOperator(string operatorName){
			if(string.IsNullOrEmpty(operatorName))
				return ESMSHomeNetwork.None;
			operatorName = operatorName.ToLower();
			if(operatorName.Contains("celcom")){
				return ESMSHomeNetwork.Celcom;
			}
			else if(operatorName.Contains("digi")){
				return ESMSHomeNetwork.Digi;
			}
			else if(operatorName.Contains("maxis")){
				return ESMSHomeNetwork.Maxis;
			}
			else if(operatorName.Contains("umobile") || operatorName.Contains("u mobile")){
				return ESMSHomeNetwork.UMobile;
			}
			else{
				return ESMSHomeNetwork.None;
			}
		}

        public void SendRequestMessage()
        {
#if UNITY_ANDROID
            var country = GetSimCountryFromSimCountryIso(AndroidNativeFunctions.GetSimCountryIso());
            var network = GetHomeNetworkFromNetworkOperator(AndroidNativeFunctions.GetNetworkOperatorName());
            var countryData = configData.Where(d => d.country == country).FirstOrDefault();
            if (countryData != null)
            {
                var networkData = countryData.networkConfigs.Where(n => n.homeNetwork == network).FirstOrDefault();
                if (networkData != null)
                {
                    // @TODO: send message to phone service
                }
            }
#elif UNITY_IOS
#else
#endif
        }
    }
}
