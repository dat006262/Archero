using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

using UnityEngine.Networking;

using com.F4A.MobileThird;

namespace com.F4A.AssetBundles
{

	public class F4AAssetBundleDownloader : SingletonMono<F4AAssetBundleDownloader>
    {
        [Serializable]
        public class BundleInfo
        {
            public string Name;
            public bool IsRequired;
            public int FileSize;
        }

        public event Action<string> OnAssetBundleDownloader_LoadComplete;
        public event Action<Exception> OnAssetBundleDownloader_LoadFail;
        public event Action OnAssetBundleDownloader_DownloadRequiredComplete;
        public event Action OnAssetBundleDownloader_LoadRequiredComplete;
        public event Action<string, float, long> OnAssetBundleDownloader_DownloadProgress;

        public string HostUrl;
        public string FilePath;
        public string CdnToken = string.Empty;
        public int BundleVersion;

        public long TotalRequiredFileSize = 0;

        public BundleInfo[] BundleInfos;

        public AssetBundle this[string bundleName]
        {
            get
            {
                if (bundleDict.ContainsKey(bundleName))
                {
                    return bundleDict[bundleName];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (bundleDict.ContainsKey(bundleName))
                {
                    bundleDict[bundleName] = value;
                }
                else
                {
                    bundleDict.Add(bundleName, value);
                }
            }
        }

        protected bool enableCacheMode = true;
        public bool EnableCacheMode
        {
            get
            {
                return enableCacheMode;
            }
            set
            {
                enableCacheMode = value;
            }
        }

        protected Dictionary<string, AssetBundle> bundleDict = new Dictionary<string, AssetBundle>();

        protected int requiredBundleLoadCount;

        protected string gameAssetKey;

        protected Dictionary<string, byte[]> downloadedData = new Dictionary<string, byte[]>();

        //AB = Asset Bundle
        private string currentABFolder = "";

        private Coroutine coroutineDownloadAssetBundle;

        [ContextMenu("Calculate File Size")]
        public void CalculateFileSize()
        {
            TotalRequiredFileSize = BundleInfos.Select(bi =>
            {
                var fileInfo = new FileInfo(Path.Combine(Application.streamingAssetsPath, bi.Name));
                bi.FileSize = (int)fileInfo.Length;

                return bi.IsRequired ? fileInfo.Length : 0;
            }).Sum();
        }

        [ContextMenu("Update Asset Bundle")]
        public void UpdateAssetBundle()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:

                    break;
            }
        }

        private void ProgressUpdateAssetBundleForWindow()
        {

        }

        void Start()
        {
            Caching.compressionEnabled = false;
            OnAssetBundleDownloader_LoadComplete += HandleAssetBundleDownloader_LoadComplete;
        }

        public void Initialize(string downloadUrl, string assetKey)
        {
            HostUrl = downloadUrl;
            this.gameAssetKey = assetKey;
        }

        public void Initialize(string downloadUrl, string filePath, string gameAssetKey)
        {
            HostUrl = downloadUrl;
            FilePath = filePath;
            this.gameAssetKey = gameAssetKey;
        }

        public void InitializeRaw(string downloadUrl, string gameAssetKey, string version, string cdnToken)
        {
            string removeSpecialChar = Regex.Replace(version, "[^0-9]", "");
            Debug.Log("REMOVE " + removeSpecialChar);
            if (string.IsNullOrEmpty(downloadUrl) || string.IsNullOrEmpty(gameAssetKey) || string.IsNullOrEmpty(version))
            {
                Debug.LogError("CAN NOT INITIALIZE RAW");
                return;
            }

            HostUrl = downloadUrl;

            if (!int.TryParse(removeSpecialChar, out BundleVersion))
            {
                Debug.LogError("CAN NOT PARSE BUNDLE VERSION");
            }

            CdnToken = cdnToken;
            this.gameAssetKey = gameAssetKey;
        }

        private void HandleAssetBundleDownloader_LoadComplete(string assetBundle)
        {
            var requiredBundleCount = BundleInfos.Count(bi => bi.IsRequired);
            if (requiredBundleLoadCount < requiredBundleCount)
            {
                requiredBundleLoadCount++;
                if (requiredBundleLoadCount == requiredBundleCount)
                {
                    if (OnAssetBundleDownloader_LoadRequiredComplete != null)
                    {
                        OnAssetBundleDownloader_LoadRequiredComplete();
                    }
                }
            }
        }

        /// <summary>
        /// Start download all asset bundles
        /// Only call after register success
        /// </summary>
        public void StartDownloadRequiredBundles()
        {
            StartCoroutine(DoDownload(true, BundleInfos
                                        .Where(bi => bi.IsRequired)
                                        .Select(bi => bi.Name)
                                        .ToArray()));
        }

        //public void StartDownloadRequireRawBundles()
        //{
        //    StartCoroutine(DoDownloadRaw(true, BundleVersion, BundleInfos
        //                                .Where(bi => bi.IsRequired)
        //                                .Select(bi => bi)
        //                                .ToArray()));
        //}

        public void StartDownloadRequireRawBundles()
        {
            //coroutineDownloadAssetBundle = StartCoroutine(DoDownloadRaw(true, BundleVersion, BundleInfos
            //                             .Where(bi => bi.IsRequired)
            //                             .Select(bi => bi)
            //                             .ToArray()));
            coroutineDownloadAssetBundle = StartCoroutine(DoDownloadRaw_NewMethod(true, BundleVersion, BundleInfos
                                        .Where(bi => bi.IsRequired)
                                        .Select(bi => bi)
                                        .ToArray()));
        }

        public void StopDownloadAllAssetBundles()
        {
            if (coroutineDownloadAssetBundle != null)
            {
                StopCoroutine(coroutineDownloadAssetBundle);
                coroutineDownloadAssetBundle = null;
            }
            if (assetBundlesRequest != null)
            {
                assetBundlesRequest.Abort();
            }
        }

        public void StartDownloadOptionalBundles()
        {
            StartCoroutine(DoDownload(false, BundleInfos
                                        .Where(bi => !bi.IsRequired)
                                        .Select(bi => bi.Name)
                                        .ToArray()));
        }

        private UnityWebRequest assetBundlesRequest;

        protected IEnumerator DoDownloadRaw_NewMethod(bool isRequired, int version, params BundleInfo[] bundles)
        {
            yield return null;

            //string filePath;

            for (int index = 0; index < bundles.Length; index++)
            {
                //filePath = Path.Combine(FilePath, bundles[index].Name);

                string fullPath = string.Format("{0}/{1}{2}", HostUrl, bundles[index].Name, CdnToken);

                Debug.Log("FULL PATH NEW METHOD: " + version);

                assetBundlesRequest = UnityWebRequestAssetBundle.GetAssetBundle(fullPath, (uint)version, 0);

                Coroutine tmpCoroutine = null;
                if (isRequired)
                {
                    tmpCoroutine = StartCoroutine(WatchDownloadingProgress_NewMethod(bundles[index].Name, assetBundlesRequest, index));
                }

#if UNITY_2017_OR_NEWER
	            yield return assetBundlesRequest.SendWebRequest();
#else
	            yield return assetBundlesRequest.Send();
#endif

                if (tmpCoroutine != null)
                {
                    StopCoroutine(tmpCoroutine);
                    tmpCoroutine = null;
                }

#if UNITY_2017_OR_NEWER
                if (assetBundlesRequest.isNetworkError)
                {
                    Debug.LogError(assetBundlesRequest.error);
	            }
#else
	            if (assetBundlesRequest.isNetworkError)
	            {
		            Debug.LogError(assetBundlesRequest.error);
	            }
#endif
                else
                {
                    if (!bundleDict.ContainsKey(bundles[index].Name))
                    {
                        this[bundles[index].Name] = ((DownloadHandlerAssetBundle)assetBundlesRequest.downloadHandler).assetBundle;
                    }

                    if (OnAssetBundleDownloader_LoadComplete != null)
                    {
                        OnAssetBundleDownloader_LoadComplete(bundles[index].Name);
                    }
                }

                assetBundlesRequest.Dispose();
            }

            //Download finish, start decrypt and load asset bundles
            if (OnAssetBundleDownloader_DownloadRequiredComplete != null && isRequired)
            {
                OnAssetBundleDownloader_DownloadRequiredComplete();
            }
        }

        protected IEnumerator DoDownloadRaw(bool isRequired, int version, params BundleInfo[] bundles)
        {
            yield return null;

            string filePath;

            for (int index = 0; index < bundles.Length; index++)
            {
                filePath = Path.Combine(FilePath, bundles[index].Name);
                Debug.Log("File Path " + filePath);
                // Load from cache
                if (enableCacheMode && File.Exists(filePath))
                {
                    WWW assetBundles = WWW.LoadFromCacheOrDownload(filePath, version);
                    yield return assetBundles;
                    this[bundles[index].Name] = assetBundles.assetBundle;
                }
                else
                {
                    string fullPath = string.Format("{0}/{1}{2}", HostUrl, bundles[index].Name, CdnToken);

                    Debug.Log("FULL PATH: " + fullPath);

                    WWW assetBundles = WWW.LoadFromCacheOrDownload(fullPath, version);

                    if (isRequired)
                    {
                        StartCoroutine(WatchDownloadingProgress(bundles[index].Name, assetBundles));
                    }

                    yield return assetBundles;
                    this[bundles[index].Name] = assetBundles.assetBundle;

                    if (OnAssetBundleDownloader_LoadComplete != null)
                    {
                        OnAssetBundleDownloader_LoadComplete(bundles[index].Name);
                    }

                    assetBundles.Dispose();
                }
            }

            //Download finish, start decrypt and load asset bundles
            if (OnAssetBundleDownloader_DownloadRequiredComplete != null && isRequired)
            {
                OnAssetBundleDownloader_DownloadRequiredComplete();
            }
        }

        protected IEnumerator DoDownload(bool isRequired, params string[] bundleNames)
        {
            yield return null;

            string filePath;
            byte[] data;

            for (int index = 0; index < bundleNames.Length; index++)
            {
                filePath = Path.Combine(FilePath, bundleNames[index]);
                Debug.Log("File Path " + filePath);
                // Load from cache
                if (enableCacheMode && File.Exists(filePath))
                {
                    using (var reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                    {
                        data = reader.ReadBytes((int)reader.BaseStream.Length);
                        reader.Close();
                    }

                    if (!downloadedData.ContainsKey(bundleNames[index]))
                    {
                        downloadedData.Add(bundleNames[index], data);
                    }
                }
                // Download and save to cache
                else
                {
                    string fullPath = HostUrl + "/" + bundleNames[index];

                    WWW encryptedBundleRquest = new WWW(fullPath);

                    if (isRequired)
                    {
                        StartCoroutine(WatchDownloadingProgress(bundleNames[index], encryptedBundleRquest));
                    }

                    yield return encryptedBundleRquest;

                    if (encryptedBundleRquest.error != null)
                    {
                        if (OnAssetBundleDownloader_LoadFail != null)
                        {
                            OnAssetBundleDownloader_LoadFail(new Exception(encryptedBundleRquest.error));
                        }
                        yield break;
                    }

                    data = encryptedBundleRquest.bytes;

                    if (enableCacheMode)
                    {
                        using (var stream = File.Open(filePath, FileMode.OpenOrCreate))
                        {
                            stream.Write(data, 0, data.Length);
                            stream.Close();
                        }
                    }

                    if (!downloadedData.ContainsKey(bundleNames[index]))
                    {
                        downloadedData.Add(bundleNames[index], data);
                    }
                }
            }

            //Download finish, start decrypt and load asset bundles
            if (OnAssetBundleDownloader_DownloadRequiredComplete != null && isRequired)
            {
                OnAssetBundleDownloader_DownloadRequiredComplete();
            }

            foreach (var bundle in downloadedData)
            {
                yield return StartCoroutine(DecryptBundle(bundle));
            }

            downloadedData.Clear();
        }

        /// <summary>
        /// Watching progress of this request
        /// </summary>
        /// <param name="www"></param>
        /// <returns></returns>
        protected IEnumerator WatchDownloadingProgress_NewMethod(string bundleName, UnityWebRequest www)
        {
            while (!www.isDone)
            {
                if (OnAssetBundleDownloader_DownloadProgress != null)
                {
                    OnAssetBundleDownloader_DownloadProgress(bundleName, www.downloadProgress, TotalRequiredFileSize);
                }
                yield return null;
            }

            if (OnAssetBundleDownloader_DownloadProgress != null)
            {
                OnAssetBundleDownloader_DownloadProgress(bundleName, 1, TotalRequiredFileSize);
                OnAssetBundleDownloader_DownloadProgress = null;
            }
        }

        /// <summary>
        /// Watching progress of this request
        /// </summary>
        /// <param name="www"></param>
        /// <returns></returns>
        protected IEnumerator WatchDownloadingProgress_NewMethod(string bundleName, UnityWebRequest www, int idBundle)
        {
            while (!www.isDone)
            {
                if (OnAssetBundleDownloader_DownloadProgress != null)
                {
                    OnAssetBundleDownloader_DownloadProgress(bundleName, www.downloadProgress, TotalRequiredFileSize);
                }
                yield return null;
            }

            if (OnAssetBundleDownloader_DownloadProgress != null)
            {
                OnAssetBundleDownloader_DownloadProgress(bundleName, 1, TotalRequiredFileSize);

                if (BundleInfos.Select(b => b.IsRequired).Count() - 1 == idBundle)
                {
                    OnAssetBundleDownloader_DownloadProgress = null;
                }
            }
        }

        /// <summary>
        /// Watching progress of this request
        /// </summary>
        /// <param name="www"></param>
        /// <returns></returns>
        protected IEnumerator WatchDownloadingProgress(string bundleName, WWW www)
        {
            while (!www.isDone)
            {
                if (OnAssetBundleDownloader_DownloadProgress != null)
                {
                    OnAssetBundleDownloader_DownloadProgress(bundleName, www.progress, TotalRequiredFileSize);
                }
                yield return null;
            }

            if (OnAssetBundleDownloader_DownloadProgress != null)
            {
                OnAssetBundleDownloader_DownloadProgress(bundleName, 1, TotalRequiredFileSize);
                OnAssetBundleDownloader_DownloadProgress = null;
            }
        }

        /// <summary>
        /// Decrypt asset bundle
        /// </summary>
        /// <param name="bundlePairObject"></param>
        protected IEnumerator DecryptBundle(object bundlePairObject)
        {
            /*
            var bundlePair = (KeyValuePair<string, byte[]>)bundlePairObject;
            var bundleName = bundlePair.Key;
            var byteData = bundlePair.Value;
            try
            {
                byte[] assetsKey = Convert.FromBase64String(gameAssetKey);

                var rawData = EncryptionUtilities.DecryptBytesResource(byteData, assetsKey);

                UIThreadInvoker.SharedInstance.Invoke(() =>
                {
                    StartCoroutine(CreateBundleInMemory(bundleName, rawData));
                });
            }
            catch (Exception ex)
            {
                if (OnAssetBundleDownloader_LoadFail != null)
                {
                    OnAssetBundleDownloader_LoadFail(ex);
                }
            }
            */
            yield return null;
        }

        int counter = 0;
        private int milestoneCounter = 1;

        /// <summary>
        /// Create bundle in memory for used
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="rawData"></param>
        /// <returns></returns>
        protected IEnumerator CreateBundleInMemory(string bundleName, byte[] rawData)
        {
            AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(rawData);

            while (!assetBundleCreateRequest.isDone)
            {
                counter++;
                if (counter > milestoneCounter)
                {
                    counter = 0;
                }
                yield return new WaitForEndOfFrame();
            }

            //yield return assetBundleCreateRequest;

            this[bundleName] = assetBundleCreateRequest.assetBundle;

            yield return null;

            if (OnAssetBundleDownloader_LoadComplete != null)
            {
                OnAssetBundleDownloader_LoadComplete(bundleName);
            }
        }

        public void UnLoadAllBundles()
        {
            foreach (var pair in bundleDict)
            {
                if (pair.Value != null)
                {
                    pair.Value.Unload(true);
                }
            }

            //if (handler != null)
            //{
            //    handler.Dispose();
            //}
            //if (assetBundlesRequest != null)
            //{
            //    assetBundlesRequest.Dispose();
            //}
            bundleDict.Clear();
            bundleDict = null;
        }

        /*
        public virtual void Load<T>(string bundleName, string assetName, Action<T> onDone) where T : UnityEngine.Object
        {
            if (this[bundleName] == null) return;

            var request = this[bundleName].LoadAssetAsync<T>(assetName);

            if (request == null) return;

            Run.Coroutine(DoLoad(request), () =>
            {
                // Get the asset.
                var go = request.asset;

                if (go != null)
                {
                    onDone(go as T);
                }
                else
                {
                    onDone(null);
                }
            });
        }
        */

        private IEnumerator DoLoad(AssetBundleRequest request)
        {
            yield return request;
        }
    }
}
