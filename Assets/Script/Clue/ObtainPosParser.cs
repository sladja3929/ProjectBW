using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainPosParser
{
    /*
    도심 100 주택가 101 저택가 102 슬램가 103
    지부 104 항구 105 시장 106 숲 107
    */

    //건물안에 해당하는 코드를 포함시켜야함.(1208)
    // 방 안 번호 (시나리오 폴더의 건물 번호.docx 파일 참고
    // 큰 구역 번호 + 큰 구역 사이드 + 컷 + 건물 번호 + 건물 내의 사이드
    // + 해당 건물 사이드 내의 왼쪽에서부터 몇 번째 방인지
    // ex) 지부_2층_2컷의 화분의 위치 값 = 411d22

    /* 건물 번호
     주택가 -> 레이나의 집 = a, 발루아의 집 = b
     슬램가 -> 정보상 = c
     지부 -> 지부 = d
     저택가 -> 총장의 저택 = e, 별채 = f, 자작의 저택 = g
     도심 -> 살롱 = h, 부동산 i, 카페 = j
     숲 -> 남매의 집 = k
     항구 -> 유람선 = l
     */
    
    private NpcParser npcParser;

    // 획득 경로 1 파싱 (큰 구역_사이드_컷)

    /* 1208 */
    // 건물 안에서 발견할 경우, 건물 안의 정보까지 추가되어야 함.
    // -> 주인공의 위치를 계속 저장해놓고 그 장소를 계속 변화하면서 쓰는건 어떨까?

    public string ParsingObtainPos1(string posCode)
    {
        string obtainPos1 = "";
        char[] tempArray = posCode.ToCharArray();

        obtainPos1 += GetMainPos(tempArray[0]);
        obtainPos1 += "_";
        obtainPos1 += GetSidePos(tempArray[1]);
        obtainPos1 += "_";
        obtainPos1 += GetCutPos(tempArray[2]);

        /* 건물 안이라면? */

        return obtainPos1;
    }

    // 획득 경로 2 파싱 (오브젝트 or 캐릭터명)
    public string ParsingObtainPos2(string posCode)
    {
        npcParser = new NpcParser();
        string obtainPos2 = "";

        // , 가 있다면 여러개 존재.
        if (posCode.Contains(","))
        {
            string[] nameArr = posCode.Split(',');
            for (int i = 0; i < nameArr.Length; i++)
            {
                obtainPos2 += npcParser.GetNpcNameFromCode(nameArr[i]);

                if (i != nameArr.Length - 1)
                    obtainPos2 += "&";
            }
        }
        else
        {
            // , 가 없으면 1개
            obtainPos2 += npcParser.GetNpcNameFromCode(posCode);
        }

        return obtainPos2;
    }

    private string GetCutPos(char cutPosCode)
    {
        string cutPos = "";

        switch (cutPosCode)
        {
            case '1':
                cutPos = "컷1";
                break;

            case '2':
                cutPos = "컷2";
                break;

            case '3':
                cutPos = "컷3";
                break;

            default:
                cutPos = "???";
                break;
        }

        return cutPos;
    }

    private string GetSidePos(char sidePosCode)
    {
        string sidePos = "";

        switch (sidePosCode)
        {
            case '1':
                sidePos = "사이드1";
                break;

            case '2':
                sidePos = "사이드2";
                break;

            case '3':
                sidePos = "사이드3";
                break;

            default:
                sidePos = "???";
                break;
        }

        return sidePos;
    }

    private string GetMainPos(char mainPosCode)
    {
        string mainPos = "";

        switch (mainPosCode)
        {
            case '0':
                mainPos = "도심";
                break;

            case '1':
                mainPos = "주택가";
                break;

            case '2':
                mainPos = "저택가";
                break;

            case '3':
                mainPos = "슬램가";
                break;

            case '4':
                mainPos = "지부";
                break;

            case '5':
                mainPos = "항구";
                break;

            case '6':
                mainPos = "시장";
                break;

            case '7':
                mainPos = "숲";
                break;

            default:
                mainPos = "알수 없는 지역";
                break;
        }//switch

        return mainPos;
    }

}
