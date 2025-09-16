




using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPSendClient : MonoBehaviour
{
    private class CArcherSSLCertVerify : CertificateHandler
    {
        private static string[] PUB_KEYS = new string[1]
        {
            "3082010A028201010091EBF42D6BFAB6BB6C00EAD9BC33172FDB94C6E977B357581CD1E3D3F21DAA2E4E0FE9653BE457E75D9D48BF9CD610CBDBFA33E89B85B3F0747781E61481B5867D5D101B8C3AFBFA619D0FFE443E4A4397832CFCEC81BAA1BE7A4E9602A416473DCB4A80517CA553522967282D3676A1C9CEE03E34DC38DBD7DDF59D9652D4861540B7CC9F5FBFD5A13A09042E42440A42DB234121528874551D8BBB26C589AA39D8928960EC9EB2D27111B6FB1FAF87759622319A332C94F89DE32056F73DAA0A4DB02A388B1389B1CCE7B9D9767E0A45A785EDEB090F0C29A1877E098588AF9194011C50F0744F6B98F81482374C84FD1402B1BA16ED7294E151AB4DE492A50203010001"
        };

        private static string[] CERT_HASHS = new string[1]
        {
            "54239C1DC87F77A0EBF1AE2EB25A485DF621072A"
        };

        protected override bool ValidateCertificate(byte[] certificateData)
        {
            X509Certificate2 x509Certificate = new X509Certificate2(certificateData);
            string publicKeyString = x509Certificate.GetPublicKeyString();
            string certHashString = x509Certificate.GetCertHashString();
            for (int i = 0; i < PUB_KEYS.Length; i++)
            {
                if (publicKeyString.Equals(PUB_KEYS[i]) && certHashString.Equals(CERT_HASHS[i]))
                {
                    return true;
                }
            }
            UnityEngine.Debug.LogErrorFormat("!!!!!!!!!SSL[{0}][{1}] Certificate Info Verify Failed!!!!!!!!!", publicKeyString, certHashString);
            return false;
        }
    }

    private const string SHA_KEY = "80E232B550B746E8B6CD0894C03913B519606";

    public ushort sendcode;

    public SendType mSendType;

    private int sendcount = 10;

    private Dictionary<int, UnityWebRequest> uwrlist = new Dictionary<int, UnityWebRequest>();

    private Dictionary<int, float> starttimes = new Dictionary<int, float>();

    private float starttime;

    private bool bShowMask;

    private float mStartTime;

    private byte[] receive;

    private int sendlastcount = 1;

    private int timeout = 10;

    private int counter = 2;

    private string mIP = string.Empty;

    private List<byte> sha_list = new List<byte>();

    private bool IsForce => mSendType == SendType.eCacheForce || mSendType == SendType.eForceLoop || mSendType == SendType.eForceOnce;

    private bool IsCache => mSendType == SendType.eCache || mSendType == SendType.eCacheForce;

    private bool IsLoop => mSendType == SendType.eForceLoop || mSendType == SendType.eLoop;

    public void StartSend<T1>(T1 packet, SendType sendtype, Action<NetResponse> callback) where T1 : CProtocolBase
    {
        //StartSend(packet, sendtype, 2, 10, callback);
        StartSend(packet, sendtype, 1, 1, callback);
    }

    public void StartSend<T1>(T1 packet, SendType sendtype, int count, int time, Action<NetResponse> callback) where T1 : CProtocolBase
    {
        mStartTime = Time.realtimeSinceStartup;
        mSendType = sendtype;
        timeout = time;
        this.counter = count;
        start_send(new NetCacheOne
        {
            sendcode = packet.GetMsgType,
            data = packet
        }, callback);
    }

    public void StartSend(NetCacheOne senddata, Action<NetResponse> callback)
    {
        mStartTime = Time.realtimeSinceStartup;
        mSendType = SendType.eCache;
        start_send(senddata, callback);
    }

    private void start_send(NetCacheOne senddata, Action<NetResponse> callback)
    {
        ////@TODO work to http
        //for (int i = 0; i < counter; i++)
        //{
        //    StartCoroutine(send_delay(i * timeout, senddata, i, callback));
        //}

        callback.Invoke(new NetResponse());
    }

    private IEnumerator send_delay(int delaytime, NetCacheOne senddata, int index, Action<NetResponse> callback)
    {
        yield return new WaitForSecondsRealtime(delaytime);
        StartCoroutine(sendInternal(senddata, index, callback));
    }

    private float set_starttime(int index)
    {
        if (!starttimes.ContainsKey(index))
        {
            starttimes.Add(index, 0f);
        }
        starttimes[index] = Time.realtimeSinceStartup;
        return starttimes[index];
    }

    private float get_starttime(int index)
    {
        float value = 0f;
        if (starttimes.TryGetValue(index, out value))
        {
            return value;
        }
        return set_starttime(index);
    }

    private IProtocol CreateProtocol(ushort code, CustomBinaryReader reader)
    {
        switch (code)
        {
            case 8:
                return new CRespUserLoginPacket();
            case 4:
                return new CRespMailList();
            case 10:
                return new CRespDimaonToCoin();
            case 15:
                return new CRespInAppPurchase();
            case 17:
                return new CRespFirstRewardInfo();
            case 7:
            case 11:
            case 18:
                return new CRespItemPacket();
            default:
                return null;
        }
    }

    private void DoResponse(ushort code, byte[] postbytes, IProtocol data)
    {
        Debugger.Log(Utils.FormatString("静默处理 code:{0} class:{1}", code, data.GetType().FullName));
        switch (code)
        {
            case 4:
                break;
            case 5:
            case 6:
                break;
            case 8:
                break;
            case 7:
                {
                    UnityEngine.Debug.LogError("宝箱开启缓存请求 静默处理");
                    CustomBinaryReader customBinaryReader = new CustomBinaryReader(new MemoryStream(postbytes));
                    byte b = customBinaryReader.ReadByte();
                    ushort num = customBinaryReader.ReadUInt16();
                    ushort num2 = customBinaryReader.ReadUInt16();
                    if (num != code)
                    {
                        break;
                    }
                    IProtocol protocol = CreateProtocol(code, customBinaryReader);
                    if (protocol != null)
                    {
                        protocol.ReadFromStream(customBinaryReader);
                        CReqItemPacket cReqItemPacket = protocol as CReqItemPacket;
                        Debugger.Log(Utils.FormatString("金币:{0} 经验:{1} 装备数量:{2}", cReqItemPacket.m_nCoinAmount, cReqItemPacket.m_nExperince, cReqItemPacket.arrayEquipItems.Length));
                        for (int i = 0; i < cReqItemPacket.arrayEquipItems.Length; i++)
                        {
                            Debugger.Log(Utils.FormatString("装备[{0}] id:{1} count:{2}", i, cReqItemPacket.arrayEquipItems[i].m_nEquipID, cReqItemPacket.arrayEquipItems[i].m_nFragment));
                        }
                    }
                    break;
                }
        }
    }

    private string GetSHA256(long time, byte[] body)
    {
        sha_list.Clear();
        sha_list.AddRange(Encoding.Default.GetBytes("80E232B550B746E8B6CD0894C03913B519606"));
        sha_list.AddRange(Encoding.Default.GetBytes(time.ToString()));
        sha_list.AddRange(body);
        byte[] array = SHA256.Create().ComputeHash(sha_list.ToArray());
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }

    private IEnumerator sendInternal(NetCacheOne senddata, int index, Action<NetResponse> callback)
    {
        Debug.Log("@LOG HTTPSendClient.sendInternal");
        if (!NetManager.IsNetConnect)
        {
            Debug.Log("无网络 " + senddata.sendcode + " cache " + IsCache);
            if (IsForce)
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
            }
            callback?.Invoke(new NetResponse());
            CacheError(senddata, reduce_count: false);
            DeInit();
            yield break;
        }
        if (IsForce && !bShowMask)
        {
            bShowMask = true;
            WindowUI.ShowMask(value: true);
            WindowUI.ShowNetDoing(value: true);
        }
        if (sendcount < 0)
        {
            if (IsForce)
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
            }
            callback?.Invoke(new NetResponse());
            CacheError(senddata, reduce_count: true);
            DeInit();
            yield break;
        }
        sendcode = senddata.sendcode;
        byte[] postbytes = senddata.data.buildPacket();
        starttime = Time.realtimeSinceStartup;
        UnityWebRequest _uwr2 = null;
        if (!uwrlist.TryGetValue(index, out _uwr2))
        {
            _uwr2 = UnityWebRequest.Put(NetConfig.GetPath(sendcode, mIP), postbytes);
            uwrlist.Add(index, _uwr2);
        }
        else
        {
            KillRequest(_uwr2);
            _uwr2 = UnityWebRequest.Put(NetConfig.GetPath(sendcode, mIP), postbytes);
            uwrlist[index] = _uwr2;
        }
        _uwr2.timeout = (counter - index) * timeout;
        _uwr2.method = "PUT";
        _uwr2.certificateHandler = new CArcherSSLCertVerify();
        starttime = Time.realtimeSinceStartup;
        set_starttime(index);
        long time = Utils.GetTimeStamp();
        _uwr2.SetRequestHeader("HabbyTime", time.ToString());
        _uwr2.SetRequestHeader("HabbyCheck", GetSHA256(time, postbytes));
        if (!string.IsNullOrEmpty(mIP))
        {
            _uwr2.SetRequestHeader("host", mIP);
        }
        _uwr2.SendWebRequest();
        while (!check_done(index))
        {
            yield return null;
        }
        if (isTimeOut(index) || _uwr2.responseCode != 200)
        {
            if (isTimeOut(index))
            {
                Debugger.Log(Debugger.Tag.eHTTP, "超时 sendcode = " + sendcode + " response " + _uwr2.responseCode + " isdone " + _uwr2.isDone);
            }
            else
            {
                Debugger.Log(Debugger.Tag.eHTTP, "返回数据错误 sendcode = " + sendcode + " response " + _uwr2.responseCode + " isdone " + _uwr2.isDone);
            }
            if (IsLoop || !isTimeOut(index))
            {
                mIP = NetConfig.RandomIP();
                DeInitBefore();
                StartCoroutine(sendInternal(senddata, index, callback));
                yield break;
            }
            if (IsForce && isTimeOut(index))
            {
                SdkManager.send_event_http(Utils.FormatString("send timeout sendcode : {0}", sendcode));
#if SHOW_NETERR
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
#endif
            }
            callback?.Invoke(new NetResponse());
            CacheError(senddata, reduce_count: false);
            DeInit();
            yield break;
        }
        receive = _uwr2.downloadHandler.data;
        if (_uwr2.GetResponseHeader("Habby") == "archero_zip")
        {
            receive = GZipHelper.Depress(receive);
        }
        CustomBinaryReader reader = new CustomBinaryReader(new MemoryStream(receive));
        byte headerTag = reader.ReadByte();
        if (headerTag == 13)
        {
            ushort num = reader.ReadUInt16();
            reader.ReadUInt16();
            if (num == senddata.sendcode)
            {
                NetResponse netResponse = new NetResponse();
                netResponse.data = CreateProtocol(num, reader);
                if (netResponse.data == null)
                {
                    SdkManager.Bugly_Report(Utils.FormatString("HTTPSendClient.SendCode:{0} ", senddata.sendcode), Utils.FormatString("DoReader code;{0} dont deal.", num));
                    SdkManager.send_event_http(Utils.FormatString("DoReader code : {0} dont deal.", num));
                    callback?.Invoke(new NetResponse());
                    CacheError(senddata, reduce_count: true);
                    DeInit();
                    yield break;
                }
                try
                {
                    netResponse.data.ReadFromStream(reader);
                }
                catch
                {
                    SdkManager.Bugly_Report("HttpSendClient", Utils.FormatString("read {0} stream error", num));
                    netResponse.data = null;
                    SdkManager.send_event_http(Utils.FormatString("readfromstream error code : {0}", num));
                }
                try
                {
                    if (callback != null)
                    {
                        callback(netResponse);
                    }
                    else
                    {
                        DoResponse(num, postbytes, netResponse.data);
                    }
                }
                catch
                {
                    SdkManager.send_event_http(Utils.FormatString("readfromstream success but callback error code : {0}", num));
#if SHOW_NETERR
                    CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
#endif
                }
            }
            else
            {
                if (num != 6)
                {
                    DeInitBefore();
                    sendcount--;
                    StartCoroutine(sendInternal(senddata, index, callback));
                    yield break;
                }
                CCommonRespMsg cCommonRespMsg = new CCommonRespMsg();
                try
                {
                    cCommonRespMsg.ReadFromStream(reader);
                }
                catch
                {
                    SdkManager.Bugly_Report("HttpSendClient", Utils.FormatString("MSG_RESP_RETURN_MESSAGE read {0} stream error", num));
                    SdkManager.send_event_http(Utils.FormatString("MSG_RESP_RETURN_MESSAGE read {0} steam error", num));
                }
                NetResponse netResponse2 = new NetResponse();
                netResponse2.error = cCommonRespMsg;
                try
                {
                    callback?.Invoke(netResponse2);
                }
                catch
                {
                    string text = Utils.FormatString("MSG_RESP_RETURN_MESSAGE callback error code : {0}", num);
                    if (cCommonRespMsg != null)
                    {
                        text += Utils.FormatString(" resp code : {0} info : {1}", cCommonRespMsg.m_nStatusCode, cCommonRespMsg.m_strInfo);
                    }
                    SdkManager.send_event_http(text);
#if SHOW_NETERR
                    CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
#endif
                }
                if (cCommonRespMsg.m_nStatusCode != 0 && cCommonRespMsg.m_nStatusCode != 1)
                {
                    bool reduce_count = true;
                    if (cCommonRespMsg.m_nStatusCode < 0)
                    {
                        reduce_count = false;
                        if (!GameLogic.InGame)
                        {
                            CInstance<TipsUIManager>.Instance.Show(Utils.FormatString("发送：{0} 通用信息错误：{1}", sendcode, cCommonRespMsg.m_nStatusCode));
                        }
                    }
                    SdkManager.Bugly_Report(Utils.FormatString("HTTPSendClient.SendCode:{0} ", senddata.sendcode), Utils.FormatString("通用信息返回 : receivecode:{0} statecode:{1}, info:{2}", num, (short)cCommonRespMsg.m_nStatusCode, cCommonRespMsg.m_strInfo));
                    SdkManager.send_event_http(Utils.FormatString("sendcode : {0} normal response error code : {1} info : {2}", sendcode, cCommonRespMsg.m_nStatusCode, cCommonRespMsg.m_strInfo));
                    CacheError(senddata, reduce_count);
                    DeInit();
                    yield break;
                }
            }
            DeInit();
        }
        else
        {
            DeInitBefore();
            sendcount--;
            StartCoroutine(sendInternal(senddata, index, callback));
        }
    }

    private bool check_done(int index)
    {
        if (isTimeOut(index))
        {
            return true;
        }
        UnityWebRequest value = null;
        if (uwrlist.TryGetValue(index, out value) && value != null && value.isDone)
        {
            return true;
        }
        return false;
    }

    private float get_timeout(int index)
    {
        UnityWebRequest value = null;
        if (uwrlist.TryGetValue(index, out value))
        {
            return value.timeout;
        }
        return 0f;
    }

    private bool isTimeOut(int index)
    {
        return Time.realtimeSinceStartup - get_starttime(index) >= get_timeout(index);
    }

    private void CacheError(NetCacheOne data, bool reduce_count)
    {
        if (IsCache)
        {
            NetManager.mNetCache.Add(data, reduce_count);
        }
    }

    private void DeInit()
    {
        DeInitBefore();
        UnityEngine.Object.Destroy(this);
    }

    private void DeInitBefore()
    {
        if (IsForce && bShowMask)
        {
            bShowMask = false;
            WindowUI.ShowMask(value: false);
            WindowUI.ShowNetDoing(value: false);
        }
        StopAllCoroutines();
        Dictionary<int, UnityWebRequest>.Enumerator enumerator = uwrlist.GetEnumerator();
        while (enumerator.MoveNext())
        {
            KillRequest(enumerator.Current.Value);
        }
        uwrlist.Clear();
    }

    private void KillRequest(UnityWebRequest request)
    {
        try
        {
            if (request != null)
            {
                request.Abort();
                request.Dispose();
                request = null;
            }
        }
        catch
        {
        }
    }
}
