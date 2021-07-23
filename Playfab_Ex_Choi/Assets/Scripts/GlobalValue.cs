using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharType
{ 
    Char_0 = 0,
    Char_1, 
    Char_2, 
    Char_3, 
    Char_4, 
    Char_5,
    CrCount
}

public class CharInfo   //각 Item 정보
{
    public string m_Name = "";      //캐릭터 이름
    public CharType m_CrType = CharType.Char_0;
    public Vector2 m_IconSize = Vector2.one; //아이콘의 가로 사이즈, 세로 사이즈
    public int m_Price = 500;       //아이템의 기본 가격
    public int m_Level = 0;
    //그전엔 Lock, 레벨0 이면 아직 구매 안됨(구매가 완료되면 레벨 1부터)
    //public int m_MaxUsable = 1;
    //사용할 수 있는 최대 스킬 카운트는 Level과 같다.
    public string m_SkillExp = "";  //스킬 효과 설명
    public Sprite m_IconImg = null;
    public string m_ImgName = "";
    //< ChrItem_0 >                         
    //Lv0->Lv1 : 500 (구입)                   m_Price = 500; //기본가격       
    //Lv1->Lv2 : 250:   ChrItem_0_UpLv2      m_UpPrice = 250; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) 가격 필요
    //Lv2->Lv3 : 500:   ChrItem_0_UpLv3
    //Lv3->Lv4 : 750:   ChrItem_0_UpLv4
    //Lv4->Lv5 : 1000:  ChrItem_0_UpLv5

    //< ChrItem_1 >   
    //Lv0->Lv1 : 1000 (구입)                 m_Price = 1000; //기본가격
    //Lv1->Lv2 : 500:    ChrItem_1_UpLv2    m_UpPrice = 500;
    //Lv2->Lv3 : 1000:   ChrItem_1_UpLv3
    //Lv3->Lv4 : 1500:   ChrItem_1_UpLv4
    //Lv4->Lv5 : 2000:   ChrItem_1_UpLv5

    //< ChrItem_2 >  
    //Lv0->Lv1 : 2000 (구입)                  m_Price = 2000; //기본가격
    //Lv1->Lv2 : 1000:    ChrItem_2_UpLv2    m_UpPrice = 1000;
    //Lv2->Lv3 : 2000:    ChrItem_2_UpLv3
    //Lv3->Lv4 : 3000:    ChrItem_2_UpLv4
    //Lv4->Lv5 : 4000:    ChrItem_2_UpLv5

    //< ChrItem_3 >  
    //Lv0->Lv1 : 4000 (구입)                   m_Price = 4000; //기본가격
    //Lv1->Lv2 : 2000:    ChrItem_3_UpLv2     m_UpPrice = 2000; 
    //Lv2->Lv3 : 4000:    ChrItem_3_UpLv3
    //Lv3->Lv4 : 6000:    ChrItem_3_UpLv4
    //Lv4->Lv5 : 8000:    ChrItem_3_UpLv5

    //< ChrItem_4 >  
    //Lv0->Lv1 : 8000 (구입)                    m_Price = 8000; //기본가격
    //Lv1->Lv2 : 4000:    ChrItem_4_UpLv2      m_UpPrice = 4000; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) 가격 필요
    //Lv2->Lv3 : 8000:    ChrItem_4_UpLv3
    //Lv3->Lv4 : 12000:   ChrItem_4_UpLv4
    //Lv4->Lv5 : 16000:   ChrItem_4_UpLv5

    //< ChrItem_5 >  
    //Lv0->Lv1 : 15000 (구입)                   m_Price = 15000; //기본가격
    //Lv1->Lv2 : 8000:    ChrItem_5_UpLv2      m_UpPrice = 8000; //Lv1->Lv2  (m_UpPrice + (m_UpPrice * (m_Level - 1)) 가격 필요
    //Lv2->Lv3 : 16000:   ChrItem_5_UpLv3
    //Lv3->Lv4 : 24000:   ChrItem_5_UpLv4
    //Lv4->Lv5 : 32000:   ChrItem_5_UpLv5

    public void SetType(CharType a_CrType)
    {
        m_CrType = a_CrType;
        if (a_CrType == CharType.Char_0)
        {
            m_Name = "강아지";
            m_IconSize.x = 0.766f;   //세로에 대한 가로 비율
            m_IconSize.y = 1.0f;     //세로를 기준으로 잡을 것이기 때문에 그냥 1.0f

            m_Price = 500; //기본가격
            m_SkillExp = "두뇌 Lv.1";
            m_IconImg = Resources.Load("Image/m0011", typeof(Sprite)) as Sprite;
            m_ImgName = "Image/m0011";
        }
        else if (a_CrType == CharType.Char_1)
        {
            m_Name = "울버독";
            m_IconSize.x = 0.81f;    //세로에 대한 가로 비율
            m_IconSize.y = 1.0f;     //세로를 기준으로 잡을 것이기 때문에 그냥 1.0f

            m_Price = 1000; //기본가격
            m_SkillExp = "두뇌 Lv.2";
            m_IconImg = Resources.Load("Image/m0367", typeof(Sprite)) as Sprite;
            m_ImgName = "Image/m0367";
        }
        else if (a_CrType == CharType.Char_2)
        {
            m_Name = "구미호";
            m_IconSize.x = 0.946f;     //세로에 대한 가로 비율
            m_IconSize.y = 1.0f;     //세로를 기준으로 잡을 것이기 때문에 그냥 1.0f

            m_Price = 2000; //기본가격
            m_SkillExp = "두뇌 Lv.3";
            m_IconImg = Resources.Load("Image/m0054", typeof(Sprite)) as Sprite;
            m_ImgName = "Image/m0054";
        }
        else if (a_CrType == CharType.Char_3)
        {
            m_Name = "야옹이";
            m_IconSize.x = 0.93f;     //세로에 대한 가로 비율
            m_IconSize.y = 1.0f;     //세로를 기준으로 잡을 것이기 때문에 그냥 1.0f

            m_Price = 4000; //기본가격
            m_SkillExp = "두뇌 Lv.4";
            m_IconImg = Resources.Load("Image/m0423", typeof(Sprite)) as Sprite;
            m_ImgName = "Image/m0423";
        }
        else if (a_CrType == CharType.Char_4)
        {
            m_Name = "드래곤";
            m_IconSize.x = 0.93f;     //세로에 대한 가로 비율
            m_IconSize.y = 1.0f;     //세로를 기준으로 잡을 것이기 때문에 그냥 1.0f

            m_Price = 8000; //기본가격
            m_SkillExp = "두뇌 Lv.5";
            m_IconImg = Resources.Load("Image/m0244", typeof(Sprite)) as Sprite;
            m_ImgName = "Image/m0244";
        }
        else if (a_CrType == CharType.Char_5)
        {
            m_Name = "팅커벨";
            m_IconSize.x = 0.93f;     //세로에 대한 가로 비율
            m_IconSize.y = 1.0f;     //세로를 기준으로 잡을 것이기 때문에 그냥 1.0f

            m_Price = 15000; //기본가격
            m_SkillExp = "두뇌 Lv.Max)";
            m_IconImg = Resources.Load("Image/m0172", typeof(Sprite)) as Sprite;
            m_ImgName = "Image/m0172";
        }
    }
}


public class GlobalValue
{
    public static string g_Unique_ID = "";  //유저의 고유번호

    public static string g_NickName = "";   //유저의 별명
    public static int g_BestScore = 0;      //게임점수 숫자맞추기
    public static int g_BestScore2 = 0;      //게임점수 사칙연산

    public static int g_UserGold = 0;       //게임머니
    public static int g_Rank = 0;           //유저의 랭킹
    public static int g_TotalScore = 0;
    public static string g_UserImgName = "Image/egg";
    //누적 골드는 별도의 필드 추가
    //캐릭터 아이템 데이터 리스트(서버에서는 보유 아이템은 배열로 레벨만 갖고 있으면 된다.
    //레벨0 이면 아직 보유 안한 것으로...)
    public static List<CharInfo> m_CrDataList = new List<CharInfo>();

    public static void InitDate()
    {
        if (0 < m_CrDataList.Count)
            return;

        CharInfo a_CrItemNd;
        for(int ii = 0; ii < (int)CharType.CrCount; ii++)
        {
            a_CrItemNd = new CharInfo();
            a_CrItemNd.SetType((CharType)ii);
            m_CrDataList.Add(a_CrItemNd);
        }
    }
}
