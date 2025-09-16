




#if ENABLE_AMAZON
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
#endif
using System.Collections;
using System.IO;
using UnityEngine;

public class IAMazonS3Manager : MonoBehaviour
{
	public const string Folder_Excel = "data/excel";

	public const string Folder_TiledMap = "data/tiledmap";

	public const string Folder_Config = "data/config";

	public const string Config_Game = "game_config.json";

	public const string Bucket_ExcelData = "archer-data";

#if ENABLE_AMAZON
	private IAmazonS3 mClient;
#endif

	public static IAMazonS3Manager Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
#if ENABLE_AMAZON
		UnityInitializer.AttachToGameObject(base.gameObject);
#endif
		Instance = this;
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(60f);
		Init();
	}

	private void Init()
	{
#if ENABLE_AMAZON
		mClient = new AmazonS3Client(new BasicAWSCredentials("AKIAJ2NYXN7OON5NRXEQ", "YuCsrrlAUNhKEKehqCjeIW9dxbISt45o9HXQEQJQ"), RegionEndpoint.GetBySystemName(RegionEndpoint.USEast2.SystemName));
		ListObjectsRequest listObjectsRequest = new ListObjectsRequest();
		listObjectsRequest.BucketName = "archer-data";
		ListObjectsRequest request = listObjectsRequest;
		mClient.ListObjectsAsync(request, delegate(AmazonServiceResult<ListObjectsRequest, ListObjectsResponse> responseObject)
		{
			IAMazonS3Manager iAMazonS3Manager = this;
			string str = string.Empty;
			if (responseObject.Exception == null)
			{
				str += "Got Response \nPrinting now \n";
				responseObject.Response.S3Objects.ForEach(delegate(S3Object o)
				{
					str += $"{o.Key}\n";
					if (o.Key.Length > 0 && o.Key.Substring(o.Key.Length - 1, 1) != "/")
					{
						iAMazonS3Manager.DownloadObject("archer-data", o.Key, o.ETag);
					}
				});
			}
			else
			{
				str = str + "Got Exception \n " + responseObject.Exception;
			}
		});
#endif
	}

	public void ClearFileName(string filename)
	{
		PlayerPrefs.SetString(GetTagName(filename), string.Empty);
	}

	private string GetTagName(string filename)
	{
		return filename + "1";
	}

	private string GetLocalTag(string filename)
	{
		return PlayerPrefs.GetString(GetTagName(filename));
	}

	private void SetLocalTag(string filename, string tag)
	{
		PlayerPrefs.SetString(GetTagName(filename), tag);
	}

	private void DownloadObject(string bucketname, string filename, string tag)
	{
		string localTag = GetLocalTag(filename);
		if (localTag.CompareTo(tag) != 0)
		{
#if ENABLE_AMAZON
			GetObjectRequest getObjectRequest = new GetObjectRequest();
			getObjectRequest.BucketName = bucketname;
			getObjectRequest.Key = filename;
			getObjectRequest.EtagToNotMatch = GetLocalTag(filename);
			GetObjectRequest request = getObjectRequest;
			mClient.GetObjectAsync(request, delegate(AmazonServiceResult<GetObjectRequest, GetObjectResponse> responseObj)
			{
				if (responseObj != null)
				{
					byte[] info = null;
					GetObjectResponse response = responseObj.Response;
					if (response.ResponseStream != null)
					{
						using (BinaryReader binaryReader = new BinaryReader(response.ResponseStream))
						{
							info = binaryReader.ReadBytes((int)response.ResponseStream.Length);
						}
						SetLocalTag(filename, tag);
						FileUtils.CreateFileOverride(filename, info);
					}
				}
			});
#endif
		}
	}
}
