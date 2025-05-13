




using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using PathologicalGames;

using System;

namespace com.F4A.MobileThird
{
	using Newtonsoft.Json;
	using System.IO;
    public enum ETypeStarMapLevel
    {
        Solid,
        Separated,
    }

    public class MapViewManager : SingletonMono<MapViewManager>
    {
        /// <summary>
        /// Add event when user click button level
        /// object sender, LevelReachedEventArgs (EventArgs) args
        /// </summary>
        public static Action<object, LevelReachedEventArgs> OnLevelClicked;
        /// <summary>
        /// params: total level, dicIndexTarget
        /// </summary>
        public static Action<int, Dictionary<int, int>> OnLoadTargetLevel;

        [Header("Action: OnLevelClicked")]
        [Header("Action: OnLoadTargetLevel")]

        [SerializeField]
        private SpawnPool levelsPool = null;
        [SerializeField]
        private GameObject levelMapPrefab = null;
        [SerializeField]
        private MapCameraController mapCameraController = null;


        [SerializeField]
        private Button btnGotoLastLevelUnlock = null;
        [SerializeField]
        private DOTweenPath MapPath = null;
		public int TotalLevel(){
			return Mathf.Min(maxLevel, MapPath.wps.Count);
		}
	    [SerializeField]
	    private int maxLevel = 1000;
	    [SerializeField]
	    private Vector3 wayPointDistance = new Vector3(0, 0.25f, 0);


        [SerializeField]
        private Transform characterMap = null;

        public ETypeStarMapLevel TypeStarMapLevel = ETypeStarMapLevel.Separated;
        [SerializeField]
        private Sprite[] targetSprites = null;
        private bool hasTargetSprite = false;

        private Dictionary<int, LevelMapController> dicLevelMap = new Dictionary<int, LevelMapController>();
        [HideInInspector]
        public Dictionary<int, int> dicIndexTarget = new Dictionary<int, int>();
		
	    [SerializeField]
        private int levelIndexCharacter = 1;

		private float positionYCameraMap = 0;

        private void Awake()
        {
            if (!mapCameraController)
                mapCameraController = GetComponent<MapCameraController>();
            hasTargetSprite = targetSprites != null && targetSprites.Length > 0;
            if (btnGotoLastLevelUnlock)
            {
                btnGotoLastLevelUnlock.onClick.RemoveAllListeners();
                btnGotoLastLevelUnlock.onClick.AddListener(HandleBtnGotoLastLevelUnlock_Click);
            }
        }

        private void OnEnable()
	    {
		    Debug.Log("MapViewManager OnEnable");
            //levelIndexCharacter = GetLastUnlockedLevel(true);
            //characterMap.position = MapPath.wps[levelIndexCharacter - 1];

			positionYCameraMap = mapCameraController.transform.position.y;

            DOVirtual.DelayedCall(0.15f, () => {
                if(hasTargetSprite && dicIndexTarget.Count == 0)
                {
                    if (MapViewManager.OnLoadTargetLevel != null)
                        MapViewManager.OnLoadTargetLevel(TotalLevel(), dicIndexTarget);
                }

	            levelIndexCharacter = GetLastUnlockedLevelAndSetCamera();
                characterMap.transform.position = GetWayPoint(GetLevelPoint(levelIndexCharacter));

				positionYCameraMap = mapCameraController.transform.position.y;
            });

			DOVirtual.DelayedCall (0.25f, () => {
				RefreshLevelOnMap();
			}).SetLoops (-1);
        }
        
        public void RefreshLevelOnMap()
        {
            Vector3 posCamera = mapCameraController.transform.position;
            //levelsPool.DespawnAll();
	        //dicLevelMap.Clear();
            
	        int lastUnlock = GetLastUnlockedLevel();

            for (int i = 0; i < TotalLevel(); i++)
            {
                int number = i + 1;
                Vector3 pos = MapPath.wps[i];
                if (posCamera.y - 12 < pos.y && pos.y < posCamera.y + 12)
                {
                	var stars = SmartMapViewUtils.GetStarsLevel(number);
                	var islocked = SmartMapViewUtils.IsLevelLocked(number);;
                    if (!dicLevelMap.ContainsKey(number))
                    {
                        LevelMapController level = SpawnLevelMapController();
                        if (level)
                        {
                            if (hasTargetSprite && dicIndexTarget.ContainsKey(number))
	                            level.Setup(pos, number, targetSprites[dicIndexTarget[number]], stars, islocked);
                            else
	                            level.Setup(pos, number, null, stars, islocked);
                        }

                        dicLevelMap[number] = level;
                    }
                    else{
                    	LevelMapController level = dicLevelMap[number];
                    	level.UpdateState(stars, islocked);
                    }
                }
                else if (dicLevelMap.ContainsKey(number))
                {
                    LevelMapController level = dicLevelMap[number];
                    dicLevelMap.Remove(number);
                    DespawnLevelMapController(level.transform);
                }
            }

            Vector3 posLastUnlock = MapPath.wps[lastUnlock - 1];

            if (btnGotoLastLevelUnlock)
            {
                if (posCamera.y - 5 > posLastUnlock.y || posCamera.y + 5 < posLastUnlock.y)
                    btnGotoLastLevelUnlock.gameObject.SetActive(true);
                else
                    btnGotoLastLevelUnlock.gameObject.SetActive(false);
            }
        }

		/// <summary>
		/// Gets the last unlocked level.
		/// </summary>
		/// <returns>The last unlocked level.</returns>
		/// <param name="isSetCamera">If set to <c>true</c> is set camera.</param>
        public int GetLastUnlockedLevelAndSetCamera()
        {
            int lastUnlock = GetLastUnlockedLevel();
	        int index = lastUnlock >= TotalLevel() ? TotalLevel() - 1 : lastUnlock;
            Vector2 pos = new Vector2(0, MapPath.wps[index].y <= 0 ? 0 : MapPath.wps[index].y);
            mapCameraController.SetPosition(pos);
            return lastUnlock;
        }
		
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <returns></returns>
        public int GetLastUnlockedLevel()
        {
            int lastUnlock = 1;
            for (int i = 2; i <= TotalLevel(); i++)
            {
                if (!SmartMapViewUtils.IsLevelLocked(i))
                {
                    lastUnlock = i;
                }
            }
            SmartMapViewUtils.SetLastLevelUnlock(lastUnlock);
            return lastUnlock;
        }

	    /// <summary>
	    /// get position way point, using for character
	    /// </summary>
	    /// <param name="pointLevel"></param>
	    /// <returns></returns>
        private Vector3 GetWayPoint(Vector3 pointLevel)
        {
            return pointLevel + wayPointDistance;
        }
	    
	    /// <summary>
	    /// Get position of level
	    /// </summary>
	    /// <param name="level"></param>
	    /// <returns></returns>
        private Vector3 GetLevelPoint(int level){
            return MapPath.wps[level - 1];
        }
		
	    /// <summary>
	    /// 
	    /// </summary>
	    /// <param name="level"></param>
	    /// <returns></returns>
        public float SetPositionCharacterMap(int level)
	    {
            if(levelIndexCharacter != level)
            {
                List<Vector3> paths = new List<Vector3>();
                if(levelIndexCharacter < level)
                {
                    for (int i = levelIndexCharacter; i <= level; i++)
                    {
                        paths.Add(GetWayPoint(GetLevelPoint(i)));
                    }
                }
                else
                {
                    for (int i = levelIndexCharacter; i >= level; i--)
                    {
                        paths.Add(GetWayPoint(GetLevelPoint(i)));
                    }
                }

                float duration = Mathf.Abs(level - levelIndexCharacter) * 0.15f;
                duration = (duration > 0.45f) ? 0.45f : duration;
                characterMap.DOPath(paths.ToArray(), duration, PathType.CatmullRom);
                levelIndexCharacter = level;

                return duration;
            }
            return 0.05f;
        }

        private LevelMapController SpawnLevelMapController()
        {
            LevelMapController level = levelsPool.Spawn(levelMapPrefab).GetComponent<LevelMapController>();
            return level;
        }

        private void DespawnLevelMapController(Transform xtransform)
        {
            if (levelsPool.IsSpawned(xtransform))
                levelsPool.Despawn(xtransform);
            else
                xtransform.gameObject.SetActive(false);
        }

        public void HandleBtnGotoLastLevelUnlock_Click(){
            GetLastUnlockedLevelAndSetCamera();
        }

#region CHECK
#if UNITY_EDITOR
        [ContextMenu("Unlock All Level")]
        public void UnlockAllLevel()
        {
            for(int i = 0; i < TotalLevel(); i++)
            {
				PlayerPrefs.SetInt(SmartMapViewUtils.KeyLevelStars(i + 1), 1);
            }
        }

        [ContextMenu("Clear All Level")]
        public void ClearAllLevel()
        {
            for (int i = 0; i < TotalLevel(); i++)
            {
				PlayerPrefs.DeleteKey(SmartMapViewUtils.KeyLevelStars(i + 1));
            }
        }
        
	    [SerializeField]
	    private int unlockToLevel= 10;
	    [ContextMenu("Unlock To Levels")]
	    public void UnlockToLevels()
	    {
		    for (int i = 0; i < unlockToLevel; i++)
		    {
			    PlayerPrefs.SetInt(SmartMapViewUtils.KeyLevelStars(i + 1), 1);
		    }
	    }
	    
	    Dictionary<string, object> dicInfo = new Dictionary<string, object>();
	    [ContextMenu("Save Map")]
	    public void SaveMap(){
	    	dicInfo.Clear ();
	    	if(MapPath != null){
	    		Vector3[] points = MapPath.wps.ToArray();
	    		dicInfo["points"] = points;
	    		
	    		var str = JsonConvert.SerializeObject (dicInfo);
		    	string pathDirectory = Path.Combine(Application.dataPath, @"Common/Data");
		    	DMCMobileUtils.CreateDirectory(pathDirectory);

		    	string path = Path.Combine(pathDirectory, @"MapInfo.txt");
		    	StreamWriter file = new System.IO.StreamWriter (path);
		    	file.WriteLine (str);
		    	file.Close ();

		    	UnityEditor.AssetDatabase.Refresh ();
	    	}
	    }
	    
	    [ContextMenu("Load Map")]
	    public void LoadMap(){
	    	string path = Application.dataPath + "/Common/Data/MapInfo.txt";
	    	string text = System.IO.File.ReadAllText (path);
	    	dicInfo.Clear ();
		    dicInfo = JsonConvert.DeserializeObject<Dictionary<string, object>> (text);
		    if(dicInfo.ContainsKey("points")){
		    	var points = JsonConvert.DeserializeObject<Vector3[]>(dicInfo["points"].ToString());
		    	int lenght = points.Length;
		    	MapPath.wps.Clear();
		    	for(int i = 0; i < lenght; i++)
		    		MapPath.wps.Add(points[i]);
		    }
	    }
	    #endif
#endregion
    }
}
