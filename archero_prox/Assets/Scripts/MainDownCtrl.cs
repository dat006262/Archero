




using Dxx.Util;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.UI;

public class MainDownCtrl : MonoBehaviour
{
    private const int PageCount = 5;

    private GameObject[] locksimage = new GameObject[5];

    private GameObject[] images = new GameObject[5];

    private ButtonCtrl[] buttons = new ButtonCtrl[5];

    private Text[] texts = new Text[5];

    private bool[] locks = new bool[5]
    {
        true,
        true,
        true,
        true,
        true
    };

    private RedNodeCtrl[] mReds = new RedNodeCtrl[5];

    private ScrollRectBase mScrollRect;

    private Transform bottomline;

    private bool bInit;

    private void Awake()
    {
        init();
    }

    private void Start()
    {
        mReds[0].SetType(RedNodeType.eRedCount);
        mReds[3].SetType(RedNodeType.eGreenUp);
    }

    private void init()
    {
        if (bInit)
        {
            return;
        }
        bInit = true;
        //@TODO BUTTON BAR BOTTOM IN MAIN GAME
        for (int i = 0; i < 5; i++)
        {
            if (i != 2)
            {
                locksimage[i] = base.transform.Find(Utils.FormatString("Button_{0}/child/child/Button/fg/Lock", i)).gameObject;
                images[i] = base.transform.Find(Utils.FormatString("Button_{0}/child/child/Button/fg/Image", i)).gameObject;
                buttons[i] = base.transform.Find(Utils.FormatString("Button_{0}/child/child/Button", i)).GetComponent<ButtonCtrl>();
                locks[i] = true;
            }
            else
            {
                locks[i] = false;
            }
            texts[i] = base.transform.Find(Utils.FormatString("Button_{0}/child/child/Button/fg/Text", i)).GetComponent<Text>();
            Transform transform = base.transform.Find(Utils.FormatString("Button_{0}/child/child/Button/fg/RedNode", i));
            if ((bool)transform)
            {
                mReds[i] = transform.GetComponent<RedNodeCtrl>();
            }
        }
        float bottomHeight = PlatformHelper.GetBottomHeight();
        (base.transform as RectTransform).anchoredPosition = new Vector2(0f, bottomHeight);
        bottomline = base.transform.Find("Bottom");
        if ((bool)bottomline)
        {
            RectTransform rectTransform = bottomline as RectTransform;
            rectTransform.anchoredPosition = new Vector2(0f, (0f - bottomHeight) / GameLogic.WidthScaleAll);
        }
    }

    public void SetScrollRect(ScrollRectBase scroll)
    {
        mScrollRect = scroll;
    }

    public void UpdateUI()
    {
        bool flag = false;
        for (int i = 0; i < 5; i++)
        {
            bool flag2 = false;
            switch (i)
            {
                case 1:
                    flag2 = (GameLogic.Hold.Guide.mEquip.process == 0);
                    Facade.Instance.SendNotification("MainUI_EquipRedCountUpdate");
                    break;
                case 3:
                    flag2 = (GameLogic.Hold.Guide.mCard.process == 0);
                    Facade.Instance.SendNotification("MainUI_CardRedCountUpdate");
                    break;
                case 4:
                    flag2 = false;
                    break;
                default:
                    flag2 = false;
                    break;
                case 0:
                    break;
            }
            UpdateLock(i, flag2);
            if (flag2)
            {
                flag = true;
            }
        }
        mScrollRect.DragDisableForce = false;
        mScrollRect.SetLocks(locks);
    }

    public void UpdateLock(int index)
    {
        init();
        bool @lock = false;
        switch (index)
        {
            case 1:
#if !ENABLE_TEST_GAME
                @lock = (GameLogic.Hold.Guide.mEquip.process == 0);
#endif
                break;
            case 3:
#if !ENABLE_TEST_GAME
                @lock = (GameLogic.Hold.Guide.mCard.process == 0);
#endif
                break;
            case 4:
                @lock = false;
                break;
            default:
                @lock = false;
                break;
            case 0:
                break;
        }
        UpdateLock(index, @lock);
        mScrollRect.SetLocks(locks);
    }

    private void UpdateLock(int index, bool _lock)
    {
        init();
        if (index != 2)
        {
            if (locksimage[index] != null && locksimage[index].activeSelf != _lock)
            {
                locksimage[index].SetActive(_lock);
            }
            if (images[index] != null && images[index].activeSelf == _lock)
            {
                images[index].SetActive(!_lock);
            }
            locks[index] = _lock;
            if (buttons[index] != null)
            {
                buttons[index].SetEnable(!_lock);
            }
        }
    }

    public bool GetLock(int index)
    {
        return locks[index];
    }

    public void SetRedNodeType(int index, RedNodeType type)
    {
        init();
        if (mReds[index] != null)
        {
            mReds[index].SetType(type);
        }
    }

    public void SetRedCount(int index, int count)
    {
        init();
        if (GetLock(index))
        {
            count = 0;
        }
        if (mReds[index] != null)
        {
            mReds[index].Value = count;
        }
    }

    public void OnLanguageChange()
    {
        texts[0].text = GameLogic.Hold.Language.GetLanguageByTID("Main_Shop");
        texts[1].text = GameLogic.Hold.Language.GetLanguageByTID("Main_Equip");
        texts[2].text = GameLogic.Hold.Language.GetLanguageByTID("Main_Battle");
        texts[3].text = GameLogic.Hold.Language.GetLanguageByTID("Main_Talent");
        texts[4].text = GameLogic.Hold.Language.GetLanguageByTID("设置_标题");
    }
}
