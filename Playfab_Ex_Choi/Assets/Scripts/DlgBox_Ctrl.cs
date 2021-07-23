using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DlgBox_Ctrl : MonoBehaviour
{
    public delegate void DLT_Response(); //<--- 델리게이트 데이터(타입)형 하나 선언
    static DLT_Response DltMethod;  //<--- 델리게이트 변수 선언(소캣 역활)

    public Button m_OK_Btn = null;
    public Button m_Close_Btn = null;
    public Button m_Cancel_Btn = null;
    public Text m_Contents_Txt = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_OK_Btn != null)   //구매 OK버튼 눌렀을 경우
            m_OK_Btn.onClick.AddListener(() => 
            {
                if (DltMethod != null)
                    DltMethod();

                //this.gameObject.SetActive(false);
                Destroy(gameObject);
            });

        if (m_Close_Btn != null)
            m_Close_Btn.onClick.AddListener(() => 
            {
                //this.gameObject.SetActive(false);
                Destroy(gameObject);
            });

        if (m_Cancel_Btn != null)
            m_Cancel_Btn.onClick.AddListener(() => 
            {
                //this.gameObject.SetActive(false);
                Destroy(gameObject);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMessage(string a_Mess, DLT_Response a_DltMtd = null)
    {
        m_Contents_Txt.text = a_Mess;
        DltMethod = a_DltMtd;
    }
}
