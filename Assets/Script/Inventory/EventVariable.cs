using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EventVariable
{
    //public bool isEnter_In_512 = false;                     // 73 시간대에 512맵(항구 1사이드 2컷)에 처음 들어갈 때 발생 (관련 이벤트 248)
    //public int num_Talk_With_1100_1101 = 0;                 // 1100, 1101과 대화한 횟수 (관련 이벤트 203, 204)
    //public int num_Talk_With_1107_1108 = 0;                 // 1107, 1108과 대화한 횟수 (관련 이벤트 213, 214)
    //public bool isInvestigated_FirstJail = false;           // 첫번째 철장이 조사됐는지의 여부 (관련 이벤트 222)
    //public bool isInvestigated_SecondJail = false;          // 두번째 철장이 조사됐는지의 여부 (관련 이벤트 222)
    //public bool isInvestigated_ThridJail = false;           // 세번째 철장이 조사됐는지의 여부 (관련 이벤트 222)
    //public bool isEnter_InformationAgency = false;          // 정보상에 들어간 적 있는지의 여부 (관련 이벤트 237)

    /* EventManager의 PlayEvent 함수에서 이벤트를 다루기 위한 행동 변수들을 정의 */
    // 이벤트의 조건이 "랜덤" 인 것이 정확히 무슨의미인지 확인해야할 필요가 있음.

    public int num_Talk_With_1105;                      // 1105와 대화한 횟수 (관련 이벤트 209)
    public bool isInvestigated_StrangeDoor;             // 이상한 문 오브젝트가 조사됐는지의 여부 (관련 이벤트 221)
    public int num_Try_to_Enter_in_Mansion;             // 자작의 저택 방문 시도 횟수 (관련 이벤트 228,233)
    public int num_Talk_With_1003;                      // 1003(멜리사)와 대화를 진행한 횟수 (관련 이벤트 230, 315)
    public int num_investigation_Raina_house_object;    // 레이나의 집의 오브젝트와 상호작용한 횟수 (관련 이벤트 230)
    public int num_Play_5027_or_5030;                   // 5027 or 5030 대화가 진행된 횟수 (관련 이벤트 230)
    public int num_Talk_With_1601_in_73;                // 73 시간대에 1601과 대화한 횟수 (관련 이벤트 231)
    public int num_Talk_With_1803_1804_in_71;           // 1803,1804와 대화한 횟수 (관련 이벤트 234)
    public int num_Talk_With_1003_in_73;                // 73 시간대에 1003과 대화한 횟수 (관련 이벤트 235, 230)
    public int num_Interrogate_about_case;              // 사건에 관해서 심문한 횟수 (관련 이벤트 236)
    public int num_Enter_or_Investigate_BroSisHouse;    // 남매의 집에 방문 or 조사한 횟수 (관련 이벤트 238)
    public bool isCheckedSecretCode;                    // 암호확인 여부 (관련 이벤트 239)
    public bool isTakenSecretCodeEvent;                 // 암호이벤트 이수 여부 (관련 이벤트 240)
    public bool isInvestigated_Raina_house;             // 레이나 집 수사(오브젝트 조사) 여부 (관련 이벤트 242)
    public int num_Talk_With_1603_in_72;                // 72 시간대에 1603과 대화한 횟수 (관련 이벤트 244)
    public int num_Enter_in_Mansion;                    // 자작의 저택에 들어간 횟수(방문 횟수, 관련 이벤트 245)
    public int num_Talk_With_1202;                      // 1113과 대화한 횟수 (관련 이벤트 246)
    public int num_Talk_With_1205_in_71;                // 71 시간대에 1205와 대화한 횟수 (관련 이벤트 247)
    public bool isPossessed_3A01_3A08_Clues;            // 3A08까지 단서 획득 여부 (관련 이벤트 249, 250)
    public bool isEnter_In_Cruise;                      // 유람선에 들어간 적이 있는지 여부 (관련 이벤트 251)
    public int num_Talk_With_1013;                      // 1013(제렐)과 대화를 한 횟수 (관련 이벤트 306)
    public int num_Talk_With_1003_in_53;                // 사건 3에서 1003(멜리사)와 대화한 횟수 (관련 이벤트 307)
    public int num_Talk_With_1500_in_79;                // 79 시간대에 1500(마릴린)과 대화한 횟수 (관련 이벤트 308)
    public int num_Talk_With_1010;                      // 1010(발루아)와 대화한 횟수 (관련 이벤트 309)
    public bool isEnter_In_Cafe;                        // 카페에 방문한 횟수 (관련 이벤트 310)
    public int num_Talk_With_1109_in_78;                // 78 시간대에 1109와 대화한 횟수 (관련 이벤트 311)
    public int num_Talk_With_1110_in_78;                // 78 시간대에 1110와 대화한 횟수 (관련 이벤트 312)
    public bool isInvestigated_President_Desk_in_54;    // 사건 4에서 총장의 사무실의 책상을 조사한 적이 있는지 여부 (관련 이벤트 313)
    public bool isActivated_4015_Conversation;          // 4015 대화묶음이 실행된적이 있는지 여부 (관련 이벤트 303, 304)

    public EventVariable()
    {
        InitEventVariables();
    }

    /* 이벤트 변수 값 초기화 */
    public void InitEventVariables()
    {
        num_Talk_With_1105 = 0;                      
        isInvestigated_StrangeDoor = false;         
        num_Try_to_Enter_in_Mansion = 0;            
        num_Talk_With_1003 = 0;                     
        num_investigation_Raina_house_object = 0;   
        num_Play_5027_or_5030 = 0;                  
        num_Talk_With_1601_in_73 = 0;               
        num_Talk_With_1803_1804_in_71 = 0;          
        num_Talk_With_1003_in_73 = 0;               
        num_Interrogate_about_case = 0;             
        num_Enter_or_Investigate_BroSisHouse = 0;   
        isCheckedSecretCode = false;            
        isTakenSecretCodeEvent = false;         
        isInvestigated_Raina_house = false;     
        num_Talk_With_1603_in_72 = 0;           
        num_Enter_in_Mansion = 0;               
        num_Talk_With_1202 = 0;                 
        num_Talk_With_1205_in_71 = 0;           
        isPossessed_3A01_3A08_Clues = false;    
        isEnter_In_Cruise = false;
        num_Talk_With_1013 = 0;
        num_Talk_With_1500_in_79 = 0;
        num_Talk_With_1010 = 0;
        isEnter_In_Cafe = false;
        num_Talk_With_1109_in_78 = 0;
        num_Talk_With_1110_in_78 = 0;
        isInvestigated_President_Desk_in_54 = false;
        isActivated_4015_Conversation = false;
    }

    /* Load한 이벤트 변수 값 적용 */
    public void SetEventvariables(JsonData jsonEventVariablesData)
    {
        num_Talk_With_1105 = int.Parse(jsonEventVariablesData[0].ToString());

        if (jsonEventVariablesData[1].ToString().Equals("true"))
            isInvestigated_StrangeDoor = true;
        else
            isInvestigated_StrangeDoor = false;

        num_Try_to_Enter_in_Mansion = int.Parse(jsonEventVariablesData[2].ToString());
        num_Talk_With_1003 = int.Parse(jsonEventVariablesData[3].ToString());
        num_investigation_Raina_house_object = int.Parse(jsonEventVariablesData[4].ToString());
        num_Play_5027_or_5030 = int.Parse(jsonEventVariablesData[5].ToString());
        num_Talk_With_1601_in_73 = int.Parse(jsonEventVariablesData[6].ToString());
        num_Talk_With_1803_1804_in_71 = int.Parse(jsonEventVariablesData[7].ToString());
        num_Talk_With_1003_in_73 = int.Parse(jsonEventVariablesData[8].ToString());
        num_Interrogate_about_case = int.Parse(jsonEventVariablesData[9].ToString());
        num_Enter_or_Investigate_BroSisHouse = int.Parse(jsonEventVariablesData[10].ToString());
        
        if (jsonEventVariablesData[11].ToString().Equals("true"))
            isCheckedSecretCode = true;
        else
            isCheckedSecretCode = false;
        
        if (jsonEventVariablesData[12].ToString().Equals("true"))
            isTakenSecretCodeEvent = true;
        else
            isTakenSecretCodeEvent = false;
        
        if (jsonEventVariablesData[13].ToString().Equals("true"))
            isInvestigated_Raina_house = true;
        else
            isInvestigated_Raina_house = false;

        num_Talk_With_1603_in_72 = int.Parse(jsonEventVariablesData[14].ToString());
        num_Enter_in_Mansion = int.Parse(jsonEventVariablesData[15].ToString());
        num_Talk_With_1202 = int.Parse(jsonEventVariablesData[16].ToString());
        num_Talk_With_1205_in_71 = int.Parse(jsonEventVariablesData[17].ToString());
        
        if (jsonEventVariablesData[18].ToString().Equals("true"))
            isPossessed_3A01_3A08_Clues = true;
        else
            isPossessed_3A01_3A08_Clues = false;
        
        if (jsonEventVariablesData[19].ToString().Equals("true"))
            isEnter_In_Cruise = true;
        else
            isEnter_In_Cruise = false;

        num_Talk_With_1013 = int.Parse(jsonEventVariablesData[20].ToString());
        num_Talk_With_1003_in_53 = int.Parse(jsonEventVariablesData[21].ToString());
        num_Talk_With_1500_in_79 = int.Parse(jsonEventVariablesData[22].ToString());
        num_Talk_With_1010 = int.Parse(jsonEventVariablesData[23].ToString());

        if (jsonEventVariablesData[24].ToString().Equals("true"))
            isEnter_In_Cafe = true;
        else
            isEnter_In_Cafe = false;

        num_Talk_With_1109_in_78 = int.Parse(jsonEventVariablesData[25].ToString());
        num_Talk_With_1110_in_78 = int.Parse(jsonEventVariablesData[26].ToString());

        if (jsonEventVariablesData[27].ToString().Equals("true"))
            isInvestigated_President_Desk_in_54 = true;
        else
            isInvestigated_President_Desk_in_54 = false;

        if (jsonEventVariablesData[28].ToString().Equals("true"))
            isActivated_4015_Conversation = true;
        else
            isActivated_4015_Conversation = false;

    }

    /* 이벤트 관련 변수 Save 파일 로드하고, 값 설정하기 */
    public void LoadEventVariables()
    {
        string loadDataPath = Application.streamingAssetsPath + "/Data/PlayerEventVariable.json";
        string tempJsonString = File.ReadAllText(loadDataPath);

        if (GameManager.instance.isEncrypted)
            tempJsonString = GameManager.instance.DecryptData(tempJsonString);

        JsonData jsonData = JsonMapper.ToObject(tempJsonString);
        SetEventvariables(jsonData);
    }

    /* 이벤트 관련 변수 Save 파일 만들기 */
    public void SaveEventVariables()
    {
        string saveDataPath = Application.streamingAssetsPath + "/Data/PlayerEventVariable.json";
        // EventVariable 클래스 통째로 json화
        JsonData tempJsonData = JsonMapper.ToJson(this);

        if (GameManager.instance.isEncrypted)
        {
            string tempStringData = GameManager.instance.EncryptData(tempJsonData.ToString());
            File.WriteAllText(saveDataPath, tempStringData);
        }
        else
        {
            File.WriteAllText(saveDataPath, tempJsonData.ToString(), System.Text.Encoding.UTF8);
        }

    }
}
