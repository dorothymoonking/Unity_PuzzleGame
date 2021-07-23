using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

enum NumOrMathSign
{
    NumType = 0,    //정답을 숫자로
    MathSignType,    //정답을 기호로
    Count
}

enum NumTypeSet
{
    FirstType = 0,
    SecondType,
    AnswerType,
    Count
}

enum MathSign
{
    Addition = 0,       //덧셈
    Subtraction,        //뺄셈
    Multiplication,     //곱하기
    Division,           //나누기
    Count
}

public class ArithmeticOperationMgr : MonoBehaviour
{
    NumOrMathSign m_NumOrMathSignType = NumOrMathSign.NumType;
    MathSign m_MathSign = MathSign.Addition;
    NumTypeSet m_NumTypeSet = NumTypeSet.FirstType;
    string[] m_MathSignStr = { "＋", "－", "×", "÷" };
    string m_AnswerStr = "";  //사칙연산 세팅용 변수

    int[] m_Num = new int[2]; // 사칙연산에 필요한 숫자 세팅용 변수
    float m_Answer = 0;         // 세팅한 숫자로 나온 정답.
    int SetRanBtn = 0;        // 정답이 들어있는 버튼 저장용 변수
    int m_Score = 0;

    float m_Timer = 3.0f;
    float m_GameTime = 30.0f;

    string[] m_SetProblemStr = new string[4]; //유저가 풀어야할 문제

    public Button[] m_Btn = new Button[3];
    public Text m_MainTxt = null;
    public Text Ranking_Text = null;
    public Text m_Score_Text = null;
    public Text GameTime_Text = null;
    public Text Timer_Text = null;

    public Button DoneBtn = null;
    public GameObject GameOverPanel = null;
    public Text EndScoreTxt = null;

    public Button CloseBtn = null;
    // Start is called before the first frame update
    void Start()
    {
        GetLeaderboard();

        if (DoneBtn != null)
            DoneBtn.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
            });
        
        if (CloseBtn != null)
            CloseBtn.onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
            });

        SetProblemType();
        SetBtn();

    }

    // Update is called once per frame
    void Update()
    {
        if(0.0 < m_Timer)
        {
            m_Timer -= Time.deltaTime;
            int Timertxt = (int)m_Timer;
            Timer_Text.text = Timertxt.ToString();
            if (m_Timer <= 0.0f)
            {
                m_Timer = 0.0f;
                m_Score -= 50;
                if (m_Score <= 0)
                    m_Score = 0;
                m_Score_Text.text = "Score : " + m_Score;
                m_Timer = 3.0f;
            }
        }

        if (0.0 < m_GameTime)
        {
            m_GameTime -= Time.deltaTime;
            int GameTimetxt = (int)m_GameTime;
            GameTime_Text.text = GameTimetxt.ToString();
            if (m_GameTime <= 0.0f)
            {
                m_GameTime = 0.0f;
                EndGame();
            }
        }
    }

    void SetProblemType()
    {
        //숫자가 정답인가? 기호가 정답인가? 세팅
        int _Num = (int)NumOrMathSign.Count;
        m_NumOrMathSignType = (NumOrMathSign)Random.Range(0, _Num);

        //숫자가 정답이라면 어느 부분을 가릴것인가.
        _Num = (int)NumTypeSet.Count;
        m_NumTypeSet = (NumTypeSet)Random.Range(0, _Num);

        //사칙연산에 필요한 정답 세팅
        for (int ii = 0; ii < m_Num.Length; ii++)
        {
            m_Num[ii] = Random.Range(1, 11);
        }

        //기호 세팅(+ - × ÷)
        _Num = (int)MathSign.Count;
        m_MathSign = (MathSign)Random.Range(0, _Num);
        m_AnswerStr = m_MathSignStr[(int)m_MathSign]; //기호문자 저장

        m_SetProblemStr[3] = m_AnswerStr;

        SetMainProblem();
    }//void SetProblemType()

    void SetMainProblem()
    {
        //문제 계산
        if (m_MathSign == MathSign.Addition)
        {
            m_Answer = m_Num[0] + m_Num[1];
        }

        else if (m_MathSign == MathSign.Subtraction)
        {
            ChangeNum();
            m_Answer = m_Num[0] - m_Num[1];
        }

        else if (m_MathSign == MathSign.Multiplication)
        {
            ChangeNum();
            m_Answer = m_Num[0] * m_Num[1];
        }

        else if (m_MathSign == MathSign.Division)
        {
            ChangeNum();
            m_Answer = (float)m_Num[0] / (float)m_Num[1];
        }

        for (int ii = 0; ii < 2; ii++)
        {
            m_SetProblemStr[ii] = m_Num[ii].ToString();
        }
        m_SetProblemStr[2] = string.Format("{0:0.##}", m_Answer);
        SetAnswer();
    }//void SetMainProblem()

    void ChangeNum()
    {
        if (m_Num[0] > m_Num[1])
            return;

        int Temp = m_Num[0];
        m_Num[0] = m_Num[1];
        m_Num[1] = Temp;
    }//void ChangeNum()

    void SetAnswer()
    {
        for(int ii = 0; ii < 4; ii ++)
        {
            Debug.Log(m_SetProblemStr[ii]);
        }
        SetRanBtn = Random.Range(0, m_Btn.Length);

        if (m_NumOrMathSignType == NumOrMathSign.NumType)
        {
            float fIdx = 0;
            int iIdx = 0;
            int _Ran;
            for (int ii = 0; ii < m_Btn.Length; ii++)
            {
                if (ii == SetRanBtn)
                {
                    m_Btn[ii].GetComponentInChildren<Text>().text = m_SetProblemStr[(int)m_NumTypeSet];
                    continue;
                }

                _Ran = Random.Range(0, 2);
                if (m_MathSign == MathSign.Division)
                {
                    if (float.TryParse(m_SetProblemStr[(int)m_NumTypeSet], out fIdx) == false)
                        Debug.Log("오류 발생");

                    if (_Ran == 0)
                        fIdx = fIdx + Random.Range(1, 5);

                    else
                    {
                        fIdx = fIdx - Random.Range(1, 5);
                        if (fIdx <= 0.0f)
                            fIdx = 0.0f;
                    }
                    string _Str = string.Format("{0:0.##}", fIdx);
                    m_Btn[ii].GetComponentInChildren<Text>().text = _Str;
                }

                else
                {
                    if (int.TryParse(m_SetProblemStr[(int)m_NumTypeSet], out iIdx) == false)
                        Debug.Log("오류 발생");
                    if (_Ran == 0)
                        iIdx = iIdx + Random.Range(1, 6);
                    else
                    {
                        iIdx = iIdx - Random.Range(1, 6);
                        if (iIdx <= 0)
                            iIdx = 0;
                    }
                    m_Btn[ii].GetComponentInChildren<Text>().text = iIdx.ToString();
                }
            }


            for (int i = 0; i < 3; i++)
            {
                if (i == (int)m_NumTypeSet)
                {
                    m_SetProblemStr[i] = "ㅁ";
                    break;
                }
            }

            SetMainText();
        }//if (m_NumOrMathSignType == NumOrMathSign.NumType)

        else if (m_NumOrMathSignType == NumOrMathSign.MathSignType)
        {
            int _Idx = (int)m_MathSign;
            bool _Check = false;
            for (int ii = 0; ii < m_Btn.Length; ii++)
            {
                if (ii == SetRanBtn)
                {
                    m_Btn[ii].GetComponentInChildren<Text>().text = m_MathSignStr[(int)m_MathSign];
                    continue;
                }

                if (_Idx == (int)MathSign.Division)
                {
                    if (_Check == false)
                    {
                        m_Btn[ii].GetComponentInChildren<Text>().text = m_MathSignStr[_Idx - 1];
                        _Check = !_Check;
                    }
                    else
                    {
                        m_Btn[ii].GetComponentInChildren<Text>().text = m_MathSignStr[_Idx - 2];
                    }
                }

                else if (_Idx == (int)MathSign.Addition)
                {
                    if (_Check == false)
                    {
                        m_Btn[ii].GetComponentInChildren<Text>().text = m_MathSignStr[_Idx + 1];
                        _Check = !_Check;
                    }
                    else
                    {
                        m_Btn[ii].GetComponentInChildren<Text>().text = m_MathSignStr[_Idx + 2];
                    }
                }

                else
                {
                    if (_Check == false)
                    {
                        m_Btn[ii].GetComponentInChildren<Text>().text = m_MathSignStr[_Idx - 1];
                        _Check = !_Check;
                    }
                    else
                    {
                        m_Btn[ii].GetComponentInChildren<Text>().text = m_MathSignStr[_Idx + 1];
                    }
                }
            }

            m_SetProblemStr[3] = "ㅁ";

            SetMainText();
        }//else if (m_NumOrMathSignType == NumOrMathSign.MathSignType)

    }//void SetAnswer()

    void SetMainText()
    {
        m_MainTxt.text = m_SetProblemStr[0] + " " +
                         m_SetProblemStr[3] + " " +
                         m_SetProblemStr[1] + " = " +
                         m_SetProblemStr[2];
    }

    void SetBtn()
    {
        for (int ii = 0; ii < m_Btn.Length; ii++)
        {
            int Index = ii;
            if (m_Btn[ii] != null)
                m_Btn[ii].onClick.AddListener(() =>
                {
                    Judgment(Index);
                });
        }
    }

    void Judgment(int _Idx)
    {
        Debug.Log(_Idx + " : " + SetRanBtn);
        if (_Idx == SetRanBtn)
        {
            //정답
            Debug.Log("정답");
            m_Score += 100;
        }
        else
        {
            Debug.Log("오답");
            m_Score -= 50;
            if (m_Score <= 0)
                m_Score = 0;
        }

        m_Timer = 3.0f;
        m_Score_Text.text = "Scroe : " + m_Score;
        SetProblemType();
    }

    void EndGame()
    {
        ScoreSet(m_Score);
        Time.timeScale = 0;
        if (GameOverPanel != null)
            GameOverPanel.SetActive(true);

        EndScoreTxt.text = "Score : " + m_Score;
    }

    void ScoreSet(int _Score)
    {
        if (GlobalValue.g_BestScore2 < _Score)
            GlobalValue.g_BestScore2 = _Score;

        GlobalValue.g_TotalScore += _Score;
        GlobalValue.g_UserGold += _Score;
        UpdateScoreCo();
        UpdateGoldCo();
    }

    void UpdateScoreCo() 
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            //BestScore, BestLevel
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "ToTalScore",
                                       Value = GlobalValue.g_TotalScore },
                new StatisticUpdate { StatisticName = "BestScore2",
                                       Value = GlobalValue.g_BestScore2 }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(
                request,

                (result) =>
                {
                    //업데이트 성공 처리
                    Debug.Log("점수 저장 성공");
                },

                (error) =>
                {
                    //업데이트 실패시 응답 함수
                    Debug.Log("점수 저장 실패");
                }
            );
    }
    void UpdateGoldCo() 
    {
        Dictionary<string, string> a_Gold = new Dictionary<string, string>();
        string a_MkKey = "";
        a_Gold.Clear();
        a_Gold.Add("UserGold", GlobalValue.g_UserGold.ToString()); //Dictionary 노드 추가 방법
        var request = new UpdateUserDataRequest
        {
            Data = a_Gold
        };

        PlayFabClientAPI.UpdateUserData(
                request,
                (result) =>
                {
                    //업데이트 성공 처리
                    Debug.Log("골드 저장 성공");
                },

                (error) =>
                {
                    //업데이트 실패시 응답 함수
                    Debug.Log("골드 저장 실패");
                }
            );
    }

    public void GetLeaderboard() //순위 불러오기...
    {
        var request = new GetLeaderboardRequest
        {
            StartPosition = 0,      //0번인덱스 즉 1등 부터
            StatisticName = "BestScore2",
            MaxResultsCount = 1,   //10명까지
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

            },
            (error) => Debug.Log("리더보드 불러오기 실패")
        );
    }

}
