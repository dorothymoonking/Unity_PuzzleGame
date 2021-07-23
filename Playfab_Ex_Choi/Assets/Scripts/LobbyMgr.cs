//#define AutoRestore

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

//UnityEditor에서 사용자 디파인 셋팅
//edit -> Player Setting -> Other Setting -> Scripting Define Symbols
//AutoRetore; AutoRestore_Off // ; 으로 구별한다.

public class LobbyMgr : MonoBehaviour
{
    public Text MyInfo_Text = null;
    public Text Ranking_Text = null;

    public Button m_StoreBtn = null;
    public Button m_GameStartBtn = null;
    public Button m_LogOutBtn = null;
    public Button m_ColorTextBtn = null;
    public Button ArithmeticOperationBtn = null;

    public Image m_UserImg = null;

    int m_My_Rank = 0;

    float RestoreTimer = 0.0f;
    public Button RestRk_Btn; //Restore Ranking Btn
    float DelayGetLB = 3.0f;  //로비 진입 후 3.0f초 뒤에 리더보드 한번 더 로딩하기...

    float ShowMsTimer = 0.0f;
    public Text MessageText;

    // Start is called before the first frame update
    void Start()
    {
        GlobalValue.InitDate();

        Ranking_Text.text = "";

        if (m_StoreBtn != null)
            m_StoreBtn.onClick.AddListener(() => 
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Store");
            });

        if (m_LogOutBtn != null)
            m_LogOutBtn.onClick.AddListener(() => 
            {
                GlobalValue.g_Unique_ID = "";
                GlobalValue.g_NickName = "";
                GlobalValue.g_BestScore = 0;
                GlobalValue.g_UserGold = 0;
                GlobalValue.g_TotalScore = 0;
                //GlobalValue.m_CrDataList.Clear();
                PlayFabClientAPI.ForgetAllCredentials(); //Playfab -> LogOut
                UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
            });

        if (m_GameStartBtn != null)
            m_GameStartBtn.onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");        
            });

        if (m_ColorTextBtn != null)
            m_ColorTextBtn.onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("ColorText");
            }); 
        
        if (ArithmeticOperationBtn != null)
            ArithmeticOperationBtn.onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("ArithmeticOperation");
            });

        if(m_UserImg != null)
            m_UserImg.sprite = Resources.Load(GlobalValue.g_UserImgName, typeof(Sprite)) as Sprite;

#if AutoRestore
        //----- 자동 리셋인경우
        RestoreTimer = 10.0f; //<--- 자동 리셋 //초기 딜레이 = 3.0f + 7.0f;
        if (RestRk_Btn != null)
            RestRk_Btn.gameObject.SetActive(false);
        //-----
#else
        //-----수동 리셋인 경우
        if (RestRk_Btn != null) //<--- 수동 리셋
            RestRk_Btn.onClick.AddListener(RestorRank);
        //-----
#endif
        //GetMyRanking(); //내 등수 불러오기...
        GetLeaderboard();

        RefreshMyInfo();    //우선 셋팅해 놓고 내 등수 불러온 후 다시 갱신
    }

    // Update is called once per frame
    void Update()
    {
        if(0.0f < DelayGetLB)
        {
            DelayGetLB -= Time.deltaTime;
            if(DelayGetLB <= 0.0f)
            {
                GetLeaderboard();
            }
        }

#if AutoRestore
        //----- 자동 리셋인 경우
        RestoreTimer -= Time.deltaTime;
        if(RestoreTimer <= 0.0f)
        {
            GetLeaderboard();
            RestoreTimer = 7.0f;
        }
        //-----
#else
        if (0.0 < RestoreTimer)
        {
            RestoreTimer -= Time.deltaTime;
            if (RestoreTimer <= 0)
                RestoreTimer = 0.0f;
        }
#endif
        if(0.0f < ShowMsTimer)
        {
            ShowMsTimer -= Time.deltaTime;
            if(ShowMsTimer <= 0.0f)
                MessageOnOff(false);    //메시지 끄기
        }
    }

    void RefreshMyInfo()
    {
        MyInfo_Text.text = "내정보 : 별명(" + GlobalValue.g_NickName + ")" +
            " : 순위(" + m_My_Rank + "등)" +
            " : (점수" + GlobalValue.g_TotalScore.ToString() + "점)" +
            " : 게임머니(" + GlobalValue.g_UserGold.ToString() + ")"; // "골드)";
    }

    public void GetLeaderboard() //순위 불러오기...
    {
        var request = new GetLeaderboardRequest
        {
            StartPosition = 0,      //0번인덱스 즉 1등 부터
            StatisticName = "ToTalScore",
            MaxResultsCount = 10,   //10명까지
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true //닉네임
            }
        };


        PlayFabClientAPI.GetLeaderboard(request,
            (result) =>
            {
                //랭킹 리스트 받아오기 성공
                if (Ranking_Text == null)
                    return;

                //랭킹 리스트 받아오기 성공
                Ranking_Text.text = "";

                for (int i = 0; i < result.Leaderboard.Count; i++)
                {
                    var curBoard = result.Leaderboard[i];

                    //등수 안에 내가 있다면 색 표시
                    if (curBoard.PlayFabId == GlobalValue.g_Unique_ID)
                        Ranking_Text.text += "<color=#00ff00>";

                    Ranking_Text.text += (i + 1).ToString() + "등 : " +
                    curBoard.DisplayName + " : " +
                    curBoard.StatValue + "점\n";
                    // " : " + curBoard.PlayFabId + "\n"; 

                    //등수 안에 내가 있다면 색 표시
                    if (curBoard.PlayFabId == GlobalValue.g_Unique_ID)
                        Ranking_Text.text += "</color>";
                }//for(int i = 0; i < result.Leaderboard.Count; i++)

                GetMyRanking(); //리더보드 등수를 불러온 직 후 내 등수를 볼러 온다.
            },
            (error) => Debug.Log("리더보드 불러오기 실패")
        );
    }

    void GetMyRanking() //나의 등수 불러 오기...
    {
        //원래 GetLeaderboardAroundPlayer는 
        //특정 PlayFabId 주변으로 리스트를 불러오는 함수이다.
        var request = new GetLeaderboardAroundPlayerRequest
        {
            //PlayFabId = GlobalValue.g_Unique_ID,
            //지정하지 않으면 내 등수를 얻어오게 된다.
            StatisticName = "ToTalScore",
            MaxResultsCount = 1,    //한명에 정보만 받아오라는 뜻
            //ProfileConstraints = new PlayerProfileViewConstraints()
            //{ ShowDisplayName = true }
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request,
            (result) =>
            {
                if (MyInfo_Text == null)
                    return;
                //if(0 < result.Leaderboard.Count)
                //{
                //    var curBoard = result.Leaderboard[i];
                //    m_My_Rank = curBoard.Position + 1;  //내 등수 가져오기...
                //} //또는 아래 처럼 받아온다(하나의 아이디만 가져왔을때 가능).

                for(int i = 0; i < result.Leaderboard.Count; i++)
                {
                    //함수 이름이 Around이니까 여려명을 받을 수도 있지만 난 한명만 요청함
                    var curBoard = result.Leaderboard[i];
                    m_My_Rank = curBoard.Position + 1;  //내 등수 가져오기...
                    GlobalValue.g_TotalScore = curBoard.StatValue;
                    GlobalValue.g_Rank = m_My_Rank;
                    break;
                }

                RefreshMyInfo();    //UI 갱신
            },
            (error)=> Debug.Log("내 등수 불러오기 실패")
        );
    }//void GetMyRanking()

    void RestorRank()
    {
        if(0.0f < RestoreTimer)
        {
            MessageOnOff(true, "최소 7초 주기로만 갱신됩니다.");
            return;
        }

        DelayGetLB = 0.0f; //딜레이 로딩 즉시 취소하고 수동로딩 우선으로...
        GetLeaderboard();

        RestoreTimer = 7.0f;
    }//void RestorRank()

    void MessageOnOff(bool isOn = true, string Mess = "")
    {
        if(isOn == true)
        {
            MessageText.text = Mess;
            MessageText.gameObject.SetActive(true);
            ShowMsTimer = 5.0f;
        }
        else
        {
            MessageText.text = "";
            MessageText.gameObject.SetActive(false);
        }
    }//void MessageOnOff(bool isOn = true, string Mess = "")
}
