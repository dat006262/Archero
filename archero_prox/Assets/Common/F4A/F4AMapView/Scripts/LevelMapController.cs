




using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace com.F4A.MobileThird
{
    public class LevelMapController : MonoBehaviour
    {
        [SerializeField]
        private int Number = 1;
        [SerializeField]
        private Text textNumber = null;
        [SerializeField]
        private Transform Lock;
        [SerializeField]
        private SpriteRenderer targetLevel = null;
        [SerializeField]
        private Transform StarsSolidParent;
        [SerializeField]
        private Transform[] StarsSolid;
        [SerializeField]
        private Transform StarsSeparatedParent;
        [SerializeField]
        private Transform[] StarsSeparated;

        [SerializeField]
        private float OverScale = 1.05f, ClickScale = 0.95f;

        private Vector3 _originalScale;
        private bool _isScaled;

        [SerializeField]
        private int StarsCount = 0;
        [SerializeField]
        private bool IsLocked = true;

        private void Awake()
        {
            _originalScale = transform.localScale;
        }

	    public void Setup(Vector3 pos, int numberLevel, Sprite spriteTarget, int starsCount, bool isLocked)
        {
            gameObject.SetActive(true);
            transform.position = pos;
            this.Number = numberLevel;
            textNumber.text = numberLevel.ToString();

            if(targetLevel && spriteTarget)
                targetLevel.sprite = spriteTarget;

            if (MapViewManager.Instance.TypeStarMapLevel == ETypeStarMapLevel.Solid)
            {
                StarsSolidParent.gameObject.SetActive(true);
                StarsSeparatedParent.gameObject.SetActive(false);
            }
            else
            {
                StarsSolidParent.gameObject.SetActive(false);
                StarsSeparatedParent.gameObject.SetActive(true);
            }
	        UpdateState(starsCount, isLocked);
        }

        public void UpdateState(int starsCount, bool isLocked)
        {
            StarsCount = starsCount;
            IsLocked = isLocked;
            Lock.gameObject.SetActive(isLocked);

            if(MapViewManager.Instance.TypeStarMapLevel == ETypeStarMapLevel.Separated)
            {
                for(int i = 0; i< StarsSeparated.Length; i++)
                    StarsSeparated[i].gameObject.SetActive(starsCount > i);
            }
            else
            {
                for (int i = 0; i < StarsSolid.Length; i++)
                    StarsSolid[i].gameObject.SetActive(starsCount == i);
            }
        }

        #region CLICK

        public void OnMouseEnter()
        {
            Scale(OverScale);
        }

        public void OnMouseDown()
        {
            Scale(ClickScale);
        }

        public void OnMouseExit()
        {
            ResetScale();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnMouseUpAsButton()
        {
            ResetScale();
            if (!IsLocked)
            {
                float duration = MapViewManager.Instance.SetPositionCharacterMap(this.Number);
                DOVirtual.DelayedCall(duration, () =>
                {
                    if(MapViewManager.OnLevelClicked != null)
                        MapViewManager.OnLevelClicked(this, new LevelReachedEventArgs(Number));
                });
            }
        }

        private void OnDisable()
        {
            ResetScale();
        }

        private void Scale(float scaleValue)
        {
            transform.localScale = _originalScale * scaleValue;
            _isScaled = true;
        }

        private void ResetScale()
        {
            if (_isScaled)
            {
                transform.localScale = _originalScale;
            }
        }

        #endregion

        private bool IsLevelExtraGem()
        {
            return (this.Number % 5 == 0);
        }
    }
}
