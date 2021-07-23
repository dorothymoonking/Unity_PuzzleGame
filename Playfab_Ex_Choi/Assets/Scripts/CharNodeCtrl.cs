using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CrState
{
    Lock,
    BeforeBuy,
    Active,
}

//m_Level이 0보다 작거나 같은 경우
//{
//  if( 0보다 작거나 같은 경우 상태가 처음 나온 상태 )
//  {
//      구입가능 표시   BeforeBuy
//  }
//  else
//  {
//      Lock 구매 불가능
//  }
//}
//
//0 < m_Level    Active

public class CharNodeCtrl : MonoBehaviour
{
    [HideInInspector] public CharType m_CrType = CharType.Char_0;
    //[HideInInspector] public int m_Level = 0; //레벨이 0이면 Lock 상태로 볼 것이다.
    [HideInInspector] public CrState m_CrState = CrState.Lock;

    public Image m_CrIconImg;
    public Text m_HelpText;
    public Text m_BuyText;
    public Image BuyDoneImg;
    public Button m_BtnCom = null;
    // Start is called before the first frame update
    void Start()
    {
        if(m_BtnCom != null)
        {
            m_BtnCom.onClick.AddListener(() => 
            {
                StoreMgr a_StoreMgr = null;
                GameObject a_StoreObj = GameObject.Find("Store_Mgr");
                if (a_StoreObj != null)
                    a_StoreMgr = a_StoreObj.GetComponent<StoreMgr>();
                if (a_StoreMgr != null)
                    a_StoreMgr.BuyCharItem(m_CrType);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitData(CharType a_CrType)
    {
        if (a_CrType < CharType.Char_0 || CharType.CrCount <= a_CrType)
            return;

        m_CrType = a_CrType;
        m_CrIconImg.sprite = GlobalValue.m_CrDataList[(int)a_CrType].m_IconImg;

        m_CrIconImg.GetComponent<RectTransform>().sizeDelta
            = new Vector2(GlobalValue.m_CrDataList[(int)a_CrType].m_IconSize.x * 135.0f, 135.0f);
        m_HelpText.text = GlobalValue.m_CrDataList[(int)a_CrType].m_SkillExp;
    }//public void InitData(CharType a_CrType)

    public void SetState(CrState a_CrState, int a_Price, int a_Lv = 0)
    {
        m_CrState = a_CrState;
        if (a_CrState == CrState.Lock) //잠긴 상태
        {
            m_CrIconImg.color = new Color32(0, 0, 0, 185);
            m_HelpText.gameObject.SetActive(false);
            m_BuyText.text = a_Price.ToString() + " 골드";
        }
        else if (a_CrState == CrState.BeforeBuy) //구매 가능 상태
        {
            m_CrIconImg.color = new Color32(255, 255, 255, 120); //new Color32(110, 110, 110, 255);
            m_HelpText.gameObject.SetActive(true);
            m_HelpText.color = new Color32(180, 180, 180, 255);
            m_BuyText.text = a_Price.ToString() + " 골드";
        }
        else if (a_CrState == CrState.Active) //활성화 상태
        {
            m_CrIconImg.color = new Color32(255, 255, 255, 255);
            m_HelpText.gameObject.SetActive(true);
            m_HelpText.color = new Color32(255, 255, 255, 255);
            int a_CacPrice = a_Price + (a_Price * (a_Lv - 1));
            m_BuyText.text = a_Price.ToString() + " 골드";
            BuyDoneImg.gameObject.SetActive(true);
            m_BtnCom.enabled = false;
        }
    }//public void SetState(CrState a_CrState, int a_Price, int a_Lv = 0)

}
