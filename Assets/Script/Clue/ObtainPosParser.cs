using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObtainPosParser
{
    /*
    도심 100 주택가 101 저택가 102 슬램가 103
    지부 104 항구 105 시장 106 숲 107
    */
    private NpcParser npcParser;

    // 획득 경로 1 파싱 (큰 구역_사이드_컷)
    public string ParsingObtainPos1(string posCode)
    {
        string obtainPos1 = "";
        char[] tempArray = posCode.ToCharArray();

        obtainPos1 += GetMainPos(tempArray[0]);
        obtainPos1 += "_";
        obtainPos1 += GetSidePos(tempArray[1]);
        obtainPos1 += "_";
        obtainPos1 += GetCutPos(tempArray[2]);

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
