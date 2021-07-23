using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class InGameMgr : MonoBehaviour
{
    public Text m_ScoreTxt = null;
    public Button m_AddScoreBtn = null;
    private Button m_ScoreBtnObj = null;

    public Text m_UserGoldTxt = null;
    public Button m_AddGoldBtn = null;
    public Button m_RemoveGoldBtn = null;
    private Button m_GoldBtnObj = null;

    public Button GoLobbyBtn = null;
    public Text m_UserInfoTxt = null;

    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.InitDate();

        if (m_AddGoldBtn != null)
            m_AddGoldBtn.onClick.AddListener(() => 
            {
                m_GoldBtnObj = m_AddGoldBtn;

                //GlobalValue.g_UserGold += 15000;
                //m_UserGoldTxt.text = "보유골드(" + GlobalValue.g_UserGold + ")";
                UpdateGoldCo();
            });

        if (m_AddScoreBtn != null)
            m_ScoreBtnObj = m_AddScoreBtn;

        if (m_RemoveGoldBtn != null)
            m_RemoveGoldBtn.onClick.AddListener(() => 
            {
                m_GoldBtnObj = m_RemoveGoldBtn;
                UpdateGoldCo();
            });

        if (GoLobbyBtn != null)
            GoLobbyBtn.onClick.AddListener(()=>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
            });

        if (m_AddScoreBtn != null)
            m_AddScoreBtn.onClick.AddListener(() => 
            {
                UpdateScoreCo();
                //GlobalValue.g_BestScore += Random.Range(5, 11);
            });

        //----- Refrash Info
        if (m_UserInfoTxt != null)
            m_UserInfoTxt.text = "내정보 : 별명("+ GlobalValue.g_NickName + ") : 순위(" + GlobalValue.g_Rank.ToString() + ")";

        if (m_ScoreTxt != null)
            m_ScoreTxt.text = "최고점수(" + GlobalValue.g_BestScore.ToString() + ")";

        if (m_UserGoldTxt != null)
            m_UserGoldTxt.text = "보유골드(" + GlobalValue.g_UserGold.ToString() + ")";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateGoldCo()
    {
        if (m_GoldBtnObj.enabled == false)
            return;

        if (GlobalValue.g_Unique_ID == "")
            return;

        if (m_GoldBtnObj == m_AddGoldBtn)
            GlobalValue.g_UserGold += 15000;

        else if (m_GoldBtnObj == m_RemoveGoldBtn)
        {
            GlobalValue.g_UserGold -= 15000;
            if (GlobalValue.g_UserGold <= 0)
                GlobalValue.g_UserGold = 0;
        }

        m_UserGoldTxt.text = "보유골드(" + GlobalValue.g_UserGold + ")";

        //< 플레이어 데이터(타이틀) >값 활용 코드
        var request = new UpdateUserDataRequest()
        {
            //Permission = UserDatePermission.Private, //디폴트값
            //Permission = UserDatePermission.Public,
            //Public 공개 설정 : 다른 유저들이 볼 수도 있게 하는 옵션
            //Private 비공개 설정(기본설정임) : 나만 접근할 수 있는 값의 속성으로 변경
            Data = new Dictionary<string, string>()
                //{ {"A", "AA"}, {"B", "BB"} };
                {
                    { "UserGold", GlobalValue.g_UserGold.ToString() }
                }
        };

        m_GoldBtnObj.enabled = false;
        //PlayFabClientAPI.UpdateUserData(request, UpdateSuccess, UpdateFailure);
        PlayFabClientAPI.UpdateUserData(request, 
            (result) => 
            {
                m_GoldBtnObj.enabled = true;
                //StartText.text = "데이터 저장 성공";
            },
            (error) => 
            {
                m_GoldBtnObj.enabled = true;
                //StartText.text = "데이터 저장 실패";
                //성공하든 실패하든 로비로 나가는 프로세스는 계속 진행된다.
            });
    }

    void UpdateScoreCo()
    {
        if (m_ScoreBtnObj.enabled == false)
            return;

        if (GlobalValue.g_Unique_ID == "")
            return;

        GlobalValue.g_BestScore += Random.Range(5, 11);

        if (m_ScoreTxt != null)
            m_ScoreTxt.text = "최고점수(" + GlobalValue.g_BestScore.ToString() + ")";


        var request =  new UpdatePlayerStatisticsRequest
        {
            //BestScore, BestLevel
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "BestScore",
                                       Value = GlobalValue.g_BestScore },
                //new StatisticUpdate { StatisticName = "BestLevel",
                //                       Value = GlobalValue.g_BestScore }
            }
        };

        m_ScoreBtnObj.enabled = false;
        PlayFabClientAPI.UpdatePlayerStatistics(
                request,

                (result) =>
                {
                    //업데이트 성공 처리
                    m_ScoreBtnObj.enabled = true;
                },

                (error) =>
                {
                    //업데이트 실패시 응답 함수
                    m_ScoreBtnObj.enabled = true;
                }
            );
    }//void UpdateScoreCo()
}
