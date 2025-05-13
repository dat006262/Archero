




using DG.Tweening;
using Dxx.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace Dxx.Net
{
	[Serializable]
	public class NetCaches : LocalSaveBase
	{
		public ulong serveruserid;

		public List<NetCacheOne> mList = new List<NetCacheOne>();

		[JsonIgnore]
		private const float mCheckDelay = 0.1f;

		[JsonIgnore]
		private float mTime = -1.1f;

		[JsonIgnore]
		private int mCount;

		[JsonIgnore]
		private NetCacheOne mCurrent;

		[JsonIgnore]
		private bool bSendLogin;

		[JsonIgnore]
		private float mSendLoginStartTime;

		[JsonIgnore]
		private bool bCurrentSendOver = true;

		[JsonIgnore]
		public bool IsEmpty => mList.Count == 0;

		public static string GetFileName(ulong serveruserid)
		{
			return Utils.FormatString("{0}-{1}", serveruserid, "netcache.txt");
		}

		public static void DeleteFile(ulong serveruserid)
		{
			string fullPath = FileUtils.GetFullPath(GetFileName(serveruserid));
			if (File.Exists(fullPath))
			{
				File.Delete(fullPath);
			}
		}

		protected override void OnRefresh()
		{
			LocalSave.Instance.DoThreadSave(LocalSave.EThreadWriteType.eNet);
		}

		public void Init()
		{
			ServicePointManager.DefaultConnectionLimit = 50;
			DOTween.Sequence().AppendInterval(0.2f).AppendCallback(OnUpdate)
				.SetLoops(-1);
		}

		public void Add(NetCacheOne data, bool reduce_count)
		{
			if (data.trycount <= 20)
			{
				if (reduce_count)
				{
					data.trycount++;
				}
				ulong serverUserID = LocalSave.Instance.GetServerUserID();
				if (serveruserid != serverUserID)
				{
					Clear();
					return;
				}
				mList.Add(data);
				Refresh();
			}
		}

		public void Remove(NetCacheOne data)
		{
			if (mList.Contains(data))
			{
				mList.Remove(data);
				if (IsEmpty)
				{
					LocalSave.Instance.mEquip.check_invalid();
				}
				Refresh();
			}
		}

		public void Copy(NetCaches data)
		{
			data.mList = mList;
		}

		private void Clear()
		{
			mList.Clear();
			ulong serverUserID = LocalSave.Instance.GetServerUserID();
			FileUtils.GetXml<NetCaches>(GetFileName(serverUserID)).Copy(this);
			serveruserid = serverUserID;
			Refresh();
		}

		private void OnUpdate()
		{
			if (serveruserid == 0)
			{
				serveruserid = LocalSave.Instance.GetServerUserID();
				NetManager.UpdateNetConnect();
				return;
			}
			if (serveruserid != 0 && serveruserid != LocalSave.Instance.GetServerUserID())
			{
				Clear();
				return;
			}
			if (bSendLogin && Time.unscaledTime - mSendLoginStartTime > 0.1f)
			{
				if (mList.Count == 0)
				{
					LocalSave.Instance.DoLogin(SendType.eUDP, null);
				}
				bSendLogin = false;
			}
			if (GameLogic.InGame || !(Time.unscaledTime - mTime > 0.1f))
			{
				return;
			}
			NetManager.UpdateNetConnect();
			if (NetManager.IsNetConnect && bCurrentSendOver)
			{
				mCount = mList.Count;
				if (mCount > 0)
				{
					bCurrentSendOver = false;
					mCurrent = mList[0];
					mList.RemoveAt(0);
					Refresh();
					NetManager.SendInternal(mCurrent, delegate
					{
						bCurrentSendOver = true;
						if (mList.Count == 0)
						{
							mSendLoginStartTime = Time.unscaledTime;
							bSendLogin = true;
						}
					});
				}
			}
			mTime = Time.unscaledTime;
		}
	}
}
