using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpBtnMgr : MonoBehaviour
{
    public GameObject m_HelpColorObj = null;
    public GameObject m_HelpAOObj = null;

    public Button m_HelpColorBtn = null;
    public Button m_HelpAoBtn = null;

    public Button m_HelpColorDoneBtn = null;
    public Button m_HelpAoDoneBtn = null;
    // Start is called before the first frame update
    void Start()
    {
        if (m_HelpColorBtn != null)
            m_HelpColorBtn.onClick.AddListener(() => 
            {
                m_HelpColorObj.SetActive(true);
            });   
        
        if (m_HelpAoBtn != null)
            m_HelpAoBtn.onClick.AddListener(() => 
            {
                m_HelpAOObj.SetActive(true);
            });

        if (m_HelpColorDoneBtn != null)
            m_HelpColorDoneBtn.onClick.AddListener(() =>
            {
                m_HelpColorObj.SetActive(false);
            });

        if (m_HelpAoDoneBtn != null)
            m_HelpAoDoneBtn.onClick.AddListener(() =>
            {
                m_HelpAOObj.SetActive(false);
            });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
