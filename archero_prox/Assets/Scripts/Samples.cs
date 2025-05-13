




using GameProtocol;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Samples : MonoBehaviour
{
	private void Start()
	{
		Button component = GetComponent<Button>();
		component.onClick.AddListener(delegate
		{
			StartCoroutine(testNetwork());
		});
	}

	private IEnumerator testNetwork()
	{
		uint nHTTPPort = 4433u;
		byte[] postUserLoginData = new CUserLoginPacket
		{
			m_strUserID = "0F24582A64D26",
			m_strDeviceID = SystemInfo.deviceUniqueIdentifier
		}.buildPacket();
		string hexString2 = BitConverter.ToString(postUserLoginData);
		hexString2 = hexString2.Replace("-", string.Empty);
		UnityEngine.Debug.LogFormat("postUserLoginData[{0}: {1}]", postUserLoginData.Length, hexString2);
		UnityWebRequest loginRequest = UnityWebRequest.Put("https://o2matrix.top:" + Convert.ToString(nHTTPPort), postUserLoginData);
		loginRequest.method = "PUT";
		yield return loginRequest.SendWebRequest();
		string loginResp2 = BitConverter.ToString(loginRequest.downloadHandler.data);
		loginResp2 = loginResp2.Replace("-", string.Empty);
		UnityEngine.Debug.LogFormat("loginResp[{0}]: {1}", loginRequest.downloadHandler.data.Length, loginResp2);
		CCommonRespMsg retMsg = new CCommonRespMsg();
		CRespUserLoginPacket respUserLoginPacket = new CRespUserLoginPacket();
		CustomBinaryReader reader = new CustomBinaryReader(new MemoryStream(loginRequest.downloadHandler.data));
		byte headerTag = reader.ReadByte();
		ushort code = reader.ReadUInt16();
		ushort len = reader.ReadUInt16();
		UnityEngine.Debug.LogFormat("headerTag[{0}] code[{1}] len[{2}]", headerTag, code, len);
		if (headerTag == 13)
		{
			switch (code)
			{
			case 8:
				respUserLoginPacket.ReadFromStream(reader);
				break;
			case 6:
				retMsg.ReadFromStream(reader);
				break;
			}
		}
	}

	private void Update()
	{
	}
}
