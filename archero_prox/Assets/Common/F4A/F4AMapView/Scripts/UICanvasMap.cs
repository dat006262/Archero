




using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace com.F4A.MobileThird
{
    public class UICanvasMap : MonoBehaviour
    {
        public static Action OnAddGemClick;
        public static Action OnAddLifeClick;
        public static Action<int> OnChangeGem;
        public static Action<int> OnChangeLife;

        [Header("Action: OnAddLifeClick")]
        [Header("Action: OnAddGemClick")]
        [Header("Action: OnChangeGem")]
        [Header("Action: OnChangeLife")]

        [Space(20)]
        [Header("Life Panel")]
        [SerializeField]
        private Text lifeText = null;
        [SerializeField]
        private Text lifesTimerText = null;
        [SerializeField]
        private int lifeCurrent = 0;
        public int LifeCurrent
        {
            set {
                lifeCurrent = value;
                if (OnChangeLife != null)
                    OnChangeLife(value);
            }
            get { return lifeCurrent; }
        }


        [Header("Gem Panel")]
        [SerializeField]
        private GameObject gemGO = null;
        [SerializeField]
        private Text gemText = null;
        [SerializeField]
        private bool isUseGem = true;
        [SerializeField]
        private int gemCurrent = 0;
        public int GemCurrent
        {
            set
            {
                gemCurrent = value;
                if (OnChangeGem != null)
                    OnChangeGem(value);
            }
            get { return gemCurrent; }
        }

        private void Awake()
        {
            gemGO.SetActive(isUseGem);
	        LifeCurrent = SmartMapViewUtils.GetLife(SmartMapViewConfig.Instance.DefaultLife);
	        GemCurrent = SmartMapViewUtils.GetGem(SmartMapViewConfig.Instance.gemDefault);
        }

		private void Start()
		{
			
		}

		public void HandleBtnAddLife_Click()
        {
            if (OnAddLifeClick != null)
                OnAddLifeClick();
        }

        public void HandleBtnAddGem_Click()
        {
            if (OnAddGemClick != null)
                OnAddGemClick();
        }
    }
}
