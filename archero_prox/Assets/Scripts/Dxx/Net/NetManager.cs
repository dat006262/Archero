




using Dxx.Util;
using GameProtocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;

namespace Dxx.Net
{
	public class NetManager
	{
		private static bool _netconnect = true;

		private static bool _netcurrent;

		private static long _nettime = 0L;

		private static long _localtime = 0L;

		public const int TimeOut = 10;

		private static List<NetCacheOne> mTemps = new List<NetCacheOne>();

		private static NetCaches _NetCache = null;

		//@TODO
        public static bool IsNetConnect => true;
        //public static bool IsNetConnect => Application.internetReachability != NetworkReachability.NotReachable;

		public static long NetTime => _nettime;

		public static long LocalTime
		{
			get
			{
				if (_localtime == 0)
				{
					_localtime = Utils.GetLocalTime();
				}
				return _localtime;
			}
		}

		public static float unitytime
		{
			get;
			private set;
		} = 0f;


		public static int PingCount
		{
			get;
			private set;
		}

		public static NetCaches mNetCache
		{
			get
			{
				if (_NetCache == null)
				{
					check(0uL);
					NetCaches.DeleteFile(0uL);
					if (LocalSave.Instance.GetServerUserID() != 0)
					{
						check(LocalSave.Instance.GetServerUserID());
					}
					_NetCache = new NetCaches();
					_NetCache.serveruserid = LocalSave.Instance.GetServerUserID();
					_NetCache.mList = mTemps;
					_NetCache.Refresh();
				}
				return _NetCache;
			}
		}

		public static void SetNetTime(long time)
		{
			_nettime = time;
			RefreshUnityTime();
		}

		public static void RefreshUnityTime()
		{
			unitytime = Time.realtimeSinceStartup;
		}

		public static void StartPing()
		{
		}

		public static void UpdateNetConnect()
		{
			_netcurrent = IsNetConnect;
			if (_netcurrent != _netconnect)
			{
				_netconnect = _netcurrent;
				if (IsNetConnect)
				{
					NetCaches.DeleteFile(0uL);
					LocalSave.Instance.DoLogin(SendType.eLoop, null);
				}
				Facade.Instance.SendNotification("PUB_NETCONNECT_UPDATE");
			}
		}

		public static void SendInternal<T1>(T1 packet, SendType sendtype, Action<NetResponse> callback) where T1 : CProtocolBase
		{
            Debug.LogFormat("@LOG SendInternal 1 m_strUserID:{0},m_strAccessToken:{0}", packet.m_strUserID, packet.m_strAccessToken);
			HTTPSendClient hTTPSendClient = GameNode.m_Net.AddComponent<HTTPSendClient>();
			hTTPSendClient.StartSend(packet, sendtype, callback);
		}

		public static void SendInternal<T1>(T1 packet, SendType sendtype, int count, int time, Action<NetResponse> callback) where T1 : CProtocolBase
		{
            Debug.LogFormat("@LOG SendInternal 2 m_strUserID:{0},m_strAccessToken:{0}", packet.m_strUserID, packet.m_strAccessToken);
            HTTPSendClient hTTPSendClient = GameNode.m_Net.AddComponent<HTTPSendClient>();
			hTTPSendClient.StartSend(packet, sendtype, count, time, callback);
		}

        public static void SendInternal(NetCacheOne senddata, Action<NetResponse> callback)
        {
            Debug.LogFormat("@LOG SendInternal 3 m_strUserID:{0},m_strAccessToken:{0}", senddata.data.m_strUserID, senddata.data.m_strAccessToken);
            HTTPSendClient hTTPSendClient = GameNode.m_Net.AddComponent<HTTPSendClient>();
            hTTPSendClient.StartSend(senddata, callback);
        }

		public static CReqItemPacket GetItemPacket(List<Drop_DropModel.DropData> list, bool addequipmust = false)
		{
			List<Drop_DropModel.DropData> dropEquips = LocalModelManager.Instance.Drop_Drop.GetDropEquips(list);
			CReqItemPacket cReqItemPacket = new CReqItemPacket();
			cReqItemPacket.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
			cReqItemPacket.m_nCoinAmount = (uint)LocalModelManager.Instance.Drop_Drop.GetDropGold(list);
			cReqItemPacket.m_nDiamondAmount = (uint)LocalModelManager.Instance.Drop_Drop.GetDropDiamond(list);
			cReqItemPacket.m_nLife = (ushort)LocalModelManager.Instance.Drop_Drop.GetDropKey(list);
			cReqItemPacket.m_nExperince = (ushort)LocalModelManager.Instance.Drop_Drop.GetDropExp(list);
			cReqItemPacket.m_nNormalDiamondItem = (ushort)LocalModelManager.Instance.Drop_Drop.GetDropDiamondBoxNormal(list);
			cReqItemPacket.m_nLargeDiamondItem = (ushort)LocalModelManager.Instance.Drop_Drop.GetDropDiamondBoxLarge(list);
			cReqItemPacket.arrayEquipItems = new CEquipmentItem[dropEquips.Count];
			int i = 0;
			for (int count = dropEquips.Count; i < count; i++)
			{
				cReqItemPacket.arrayEquipItems[i] = new CEquipmentItem();
				cReqItemPacket.arrayEquipItems[i].m_nUniqueID = Utils.GenerateUUID();
				cReqItemPacket.arrayEquipItems[i].m_nEquipID = (uint)dropEquips[i].id;
				cReqItemPacket.arrayEquipItems[i].m_nLevel = 1u;
				cReqItemPacket.arrayEquipItems[i].m_nFragment = (uint)dropEquips[i].count;
				if (addequipmust)
				{
					LocalSave.Instance.AddProp(cReqItemPacket.arrayEquipItems[i]);
				}
			}
			return cReqItemPacket;
		}

		private static void check(ulong serveruserid)
		{
			string fileName = NetCaches.GetFileName(serveruserid);
			string xmlFileString = FileUtils.GetXmlFileString(fileName);
			if (!string.IsNullOrEmpty(xmlFileString))
			{
				try
				{
					JObject jObject = JObject.Parse(xmlFileString);
					JArray jArray = null;
					jArray = JArray.Parse(jObject["mList"].ToString());
					int i = 0;
					for (int count = jArray.Count; i < count; i++)
					{
						JObject jObject2 = JObject.Parse(jArray[i].ToString());
						JObject jObject3 = JObject.Parse(jObject2["data"].ToString());
						NetCacheOne netCacheOne = new NetCacheOne();
						netCacheOne.trycount = 0;
						netCacheOne.sendcode = jObject2["sendcode"].ToObject<ushort>();
						bool flag = true;
						if (netCacheOne.sendcode == 7)
						{
							try
							{
								CReqItemPacket cReqItemPacket = (CReqItemPacket)(netCacheOne.data = JsonConvert.DeserializeObject<CReqItemPacket>(jObject3.ToString()));
								flag = true;
							}
							catch
							{
								flag = false;
								SdkManager.Bugly_Report("NetManager_Cache", Utils.FormatString("GameOver DeserializeObject failed."));
							}
						}
						else if (netCacheOne.sendcode == 13)
						{
							try
							{
								CLifeTransPacket cLifeTransPacket = (CLifeTransPacket)(netCacheOne.data = JsonConvert.DeserializeObject<CLifeTransPacket>(jObject3.ToString()));
								flag = true;
							}
							catch
							{
								flag = false;
								SdkManager.Bugly_Report("NetManager_Cache", Utils.FormatString("key DeserializeObject failed."));
							}
						}
						else if (netCacheOne.sendcode == 15)
						{
							try
							{
								CInAppPurchase cInAppPurchase = (CInAppPurchase)(netCacheOne.data = JsonConvert.DeserializeObject<CInAppPurchase>(jObject3.ToString()));
								flag = true;
							}
							catch
							{
								flag = false;
								SdkManager.Bugly_Report("NetManager_Cache", Utils.FormatString("iap DeserializeObject failed."));
							}
						}
						if (flag)
						{
							mTemps.Add(netCacheOne);
						}
					}
				}
				catch
				{
				}
			}
		}
	}
}
