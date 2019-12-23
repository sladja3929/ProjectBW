using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 고유한 character code 설정하는 class */
public class NpcParser
{
    /// <summary>
    /// 1208 update
    /// 
    /// 주요 캐릭터 - 총 14명
    /// 메르테 = 1000 / 안드렌 = 1001 / 체스미터 = 1002 / 멜리사 = 1003
    /// 프란체티 = 1004 / 레이나 = 1005 / 더글라스 = 1006 / 에고이스모 륑 = 1007
    /// 메그 륑 = 1008 / 아놀드 = 1009 / 발루아 = 1010 / 도모니아 여왕 = 1011
    /// 서술자 = 1012 / 제렐(전 수사관) = 1013
    /// 
    /// 주택가 - 인물 총 11명
    /// 제린 = 1100 / 미쉘 = 1101 / 제키 = 1102 / 릴리 = 1103
    /// 마드리아 = 1104 / 쉐렌 = 1105 / 바륀 = 1106
    /// 폴 = 1107 / 마리 = 1108 토케 = 1109 / 슈벤 = 1110
    /// 
    /// 항구 - 인물 총 8명
    /// 마릴린 = 1500 / 므피어 = 1501 / 엑스트라 어부 = 1502
    /// 아이 = 1503 / 악당1 = 1504 / 악당2 = 1505 / 유람선 직원 = 1506
    /// 유람선 감금층 직원 = 1507
    /// 
    /// 지부 - 인물 총 4명
    /// 안내원 = 1400 / 접수원 = 1401 / 공사를 알리는 직원 = 1402
    /// 쇼파1 직원 = 1403
    /// 
    /// 슬램가 - 인물 총 7명
    /// 창고 지킴이 = 1300 / 조무래기 = 1301 / 노숙자 = 1302
    /// 어벙이 = 1303 / 짜증이 = 1304 / 바텐더 = 1305 / 사람들 = 1306
    /// 
    /// 저택가 안의 별채 -  인물 총 5명
    /// 왼쪽 아이 = 1215 / 오른쪽 아이 = 1216 / 여자 아이 = 1217
    /// 남자 아이 = 1218 / 어린 아이(여자) = 1219
    /// 
    /// </summary>
    /// <param name="characterCode"></param>
    /// <returns></returns>


    // 2번째 index 값 = 0 -> 메인
    public string GetMainNpcNameFromCode(string characterCode)
    {
        switch (characterCode)
        {
            case "1000":
                return "메르테";

            case "1001":
                return "안드렌";

            case "1002":
                return "체스미터";

            case "1003":
                return "멜리사";

            case "1004":
                return "프란체티";

            case "1005":
                return "레이나";

            case "1006":
                return "더글라스";

            case "1007":
                return "에고이스모 륑";

            case "1008":
                return "메그 륑";

            case "1009":
                return "아놀드";

            case "1010":
                return "발루아";

            case "1011":
                return "도모니아 여왕";

            case "1012":
                return "서술자";

            case "1013":
                return "제렐(전 수사관)";

            default:
                return null;
        }//switch()
    }
    public string GetMainNpcCodeFromName(string characterName)
    {
        switch (characterName)
        {
            case "메르테":
                return "1000";

            case "안드렌":
                return "1001";

            case "체스미터":
                return "1002";

            case "멜리사":
                return "1003";

            case "프란체티":
                return "1004";

            case "레이나":
                return "1005";

            case "더글라스":
                return "1006";

            case "에고이스모 륑":
                return "1007";

            case "메그 륑":
                return "1008";

            case "아놀드":
                return "1009";

            case "발루아":
                return "1010";

            case "도모니아 여왕":
                return "1011";

            case "서술자":
                return "1012";

            case "제렐(전 수사관)":
                return "1013";

            default:
                return null;
        }//switch()
    }

    // 2번째 index 값 = 1 -> 주택가
    public string GetVillageNpcNameFromCode(string characterCode)
    {
        switch (characterCode)
        {
            case "1100":
                return "제린";

            case "1101":
                return "미쉘";

            case "1102":
                return "제키";

            case "1103":
                return "릴리";

            case "1104":
                return "마드리아";

            case "1105":
                return "쉐렌";

            case "1106":
                return "바륀";

            case "1107":
                return "폴";

            case "1108":
                return "마리";

            case "1109":
                return "토케";

            case "1110":
                return "슈벤";

            default:
                return null;
        }//switch()
    }
    public string GetVillageNpcCodeFromName(string characterName)
    {
        switch (characterName)
        {
            case "제린":
                return "1100";

            case "미쉘":
                return "1101";

            case "제키":
                return "1102";

            case "릴리":
                return "1103";

            case "마드리아":
                return "1104";

            case "쉐렌":
                return "1105";

            case "바륀":
                return "1106";

            case "폴":
                return "1107";

            case "마리":
                return "1108";

            case "토케":
                return "1109";

            case "슈벤" +
            "":
                return "1110";

            default:
                return null;
        }//switch()
    }

    // 2번째 index 값 = 2 -> 저택가(안의 별채 포함)
    public string GetMansionNpcNameFromCode(string characterCode)
    {
        switch (characterCode)
        {
            case "1215":
                return "왼쪽 아이";

            case "1216":
                return "오른쪽 아이";

            case "1217":
                return "여자 아이";

            case "1218":
                return "남자 아이";

            case "1219":
                return "어린 아이(여자)";

            default:
                return null;
        }
    }
    public string GetMansionNpcCodeFromName(string characterName)
    {
        switch (characterName)
        {
            case "왼쪽 아이":
                return "1215";

            case "오른쪽 아이":
                return "1216";

            case "여자 아이":
                return "1217";

            case "남자 아이":
                return "1218";

            case "어린 아이(여자)":
                return "1219";

            default:
                return null;
        }
    }

    // 2번째 index 값 = 3 -> 슬램가
    public string GetSlamNpcNameFromCode(string characterCode)
    {
        switch (characterCode)
        {
            case "1300":
                return "창고 지킴이";

            case "1301":
                return "조무래기";

            case "1302":
                return "노숙자";

            case "1303":
                return "어벙이";

            case "1304":
                return "짜증이";

            case "1305":
                return "바텐더";

            case "1306":
                return "사람들";

            default:
                return null;
        }
    }
    public string GetSlamNpcCodeFromName(string characterName)
    {
        switch (characterName)
        {
            case "창고 지킴이":
                return "1300";

            case "조무래기":
                return "1301";

            case "노숙자":
                return "1302";

            case "어벙이":
                return "1303";

            case "짜증이":
                return "1304";

            case "바텐더":
                return "1305";

            case "사람들":
                return "1306";

            default:
                return null;
        }
    }

    // 2번째 index 값 = 4 -> 지부
    public string GetChapterNpcNameFromCode(string characterCode)
    {
        switch (characterCode)
        {
            case "1400":
                return "안내원";

            case "1401":
                return "접수원";

            case "1402":
                return "공사를 알리는 직원";

            case "1403":
                return "쇼파1 직원";

            default:
                return null;
        }
    }
    public string GetChapterNpcCodeFromName(string characterName)
    {
        switch (characterName)
        {
            case "안내원":
                return "1400";

            case "접수원":
                return "1401";

            case "공사를 알리는 직원":
                return "1402";

            case "쇼파1 직원":
                return "1403";

            default:
                return null;
        }
    }

    // 2번째 index 값 = 5 -> 항구
    public string GetHarborNpcNameFromCode(string characterCode)
    {
        switch (characterCode)
        {
            case "1500":
                return "마릴린";

            case "1501":
                return "므피어";

            case "1502":
                return "엑스트라 어부";

            case "1503":
                return "아이";

            case "1504":
                return "악당1";

            case "1505":
                return "악당2";

            case "1506":
                return "유람선 직원";

            case "1507":
                return "유람선 감금층 직원";

            default:
                return null;
        }//switch()
    }
    public string GetHarborNpcCodeFromName(string characterName)
    {
        switch (characterName)
        {
            case "마릴린":
                return "1500";

            case "므피어":
                return "1501";

            case "엑스트라 어부":
                return "1502";

            case "아이":
                return "1503";

            case "악당1":
                return "1504";

            case "악당2":
                return "1505";

            case "유람선 직원":
                return "1506";

            case "유람선 감금층 직원":
                return "1507";

            default:
                return null;
        }//switch()
    }


    public string GetNpcNameFromCode(string characterCode)
    {
        switch (characterCode[1])
        {
            case '0':
                return GetMainNpcNameFromCode(characterCode);

            case '1':
                return GetVillageNpcNameFromCode(characterCode);

            case '2':
                return GetMansionNpcNameFromCode(characterCode);

            case '3':
                return GetSlamNpcNameFromCode(characterCode);

            case '4':
                return GetChapterNpcNameFromCode(characterCode);

            case '5':
                return GetHarborNpcNameFromCode(characterCode);

            default:
                return null;
        }
    }

    public string GetNpcCodeFromName(string characterName)
    {
        //만약... 동명이인이 있다면, 어떤 장소에 있던 캐릭터인지 예외처리 해줘야함.
        string tempNpcName = "";

        if ((tempNpcName = GetMainNpcCodeFromName(characterName)) != null)
            return tempNpcName;
        else if ((tempNpcName = GetVillageNpcCodeFromName(characterName)) != null)
            return tempNpcName;
        else if ((tempNpcName = GetMansionNpcCodeFromName(characterName)) != null)
            return tempNpcName;
        else if ((tempNpcName = GetChapterNpcCodeFromName(characterName)) != null)
            return tempNpcName;
        else if ((tempNpcName = GetHarborNpcCodeFromName(characterName)) != null)
            return tempNpcName;
        else
            return null;
    }
}
