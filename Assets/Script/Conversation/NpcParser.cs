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
    /// 서술자 = 1012 / 제렐 = 1013 / 시스템 = 1014 / 조수 = 1015
    /// 
    /// 주택가 - 인물 총 11명
    /// 제린 = 1100 / 미쉘 = 1101 / 제키 = 1102 / 릴리 = 1103
    /// 마드리아 = 1104 / 쉐렌 = 1105 / 바륀 = 1106
    /// 폴 = 1107 / 마리 = 1108 토케 = 1109 / 슈벤 = 1110
    /// 
    /// 항구 - 인물 총 8명
    /// 마릴린 = 1500 / 므피어 = 1501 / 코이체 = 1502
    /// 키로 = 1503 / 악당 1 = 1504 / 악당 2 = 1505 / 표 확인자 = 1506
    /// 감시자 = 1507
    /// 
    /// 항구 - 상호작용하는 오브젝트 총 5개
    /// 가판대 위치 1_항구 = 9500 / 가판대 위치 2_항구 = 9501 / 돛을 거는 기둥_항구 = 9502 / 작은 배_항구 = 9503 / 유람선_항구 = 9504
    /// 
    /// 유람선 안 - 상호작용하는 오브젝트 총 6개
    /// 음식이 있는 테이블_유람선 = 9505 / 커다란 의자_유람선 = 9506 / 이상한 손잡이_유람선 = 9507 / 철장 1_유람선 = 9508 / 철장 2_유람선 = 9509 / 철장 3_유람선 = 9510
    /// 
    /// 지부 - 인물 총 4명
    /// 안내원 = 1400 / 접수원 = 1401 / 마탄 = 1402 / 레누 = 1403
    /// 
    /// 지부 - 상호작용하는 오브젝트 총 16개
    /// 화분_지부 = 9400 / 책장_지부 = 9401 / 카펫_지부 = 9402 / 책상_지부 = 9403 / 사체_지부 = 9404
    /// 달력 = 9405 / 편지 봉투_지부 = 9406 / 티켓_지부 = 9407 / 입양 서류_지부 = 9408 / 금고_지부 = 9409 / 금고속 종이_지부 = 9410
    /// 
    /// 창문_지부2층 = 9411 / 책장_지부2층 = 9412 / 금고_주인공사무실 = 9413 / 창문_주인공사무실 = 9414 / 책장_주인공사무실 = 9415
    /// 
    /// 슬램가 - 인물 총 7명
    /// 디펀 = 1300 / 아킬 = 1301 / 노숙자 = 1302
    /// 아반 = 1303 / 자드 = 1304 / 아울 = 1305 / 사람들 = 1306
    /// 
    /// 슬램가 - 상호작용하는 오브젝트 총 9개
    /// 나무상자 = 9300 / 창문 = 9301 / 종이 = 9302 / 정보상 문 = 9303 / 기둥 = 9307 / 허름한 집 문 = 9308
    /// 쓰레기 더미(왼쪽) = 9304 / 쓰레기 더미(중앙) = 9305 / 쓰레기 더미(오른쪽) = 9306
    /// 
    /// 저택가 안의 별채 -  인물 총 5명
    /// 클로이 = 1207 / 노바 = 1208 / 제니 = 1209
    /// 톰 = 1210 / 캐시 = 1219
    /// 
    /// 저택가 - 인물 총 12명
    /// 문지기 1 = 1200 / 문지기 2 = 1201 / 마야 = 1202 / 총장 집 관리인 = 1203
    /// 주방 하녀 = 1204 / 집사 = 1205 / 청소하는 하인 = 1206 / 청소하는 하녀 1 = 1211 / 청소하는 하인 2 = 1212
    /// 식료품 옮기는 하인 = 1213 / 청소하는 하녀 2 = 1214 / 청소하는 하녀 3 = 1215
    /// 
    /// 저택가 - 상호작용하는 오브젝트 총 4개
    /// 총장의 저택_저택가 = 9200 / 저택의 대문_저택가 = 9201 / 표지판 1_저택가 = 9202 / 표지판 2_저택가 = 9203 / 표지판 3_저택가 = 9248ㅂ
    /// 
    /// 총장의 저택 1층 - 상호작용하는 오브젝트 총 16개
    /// 옆문 = 9204 / 주방 문 = 9205 / 식탁 = 9206 / 창문 = 9207 / 와인 = 9208 / 샹들리에 = 9209 / 도자기 = 9210
    /// 계단 = 9211 / 쇼파 = 9212 / 테이블 = 9213 / 그림 = 9214 / 벽난로 = 9215 / 상자더미 = 9216 / 창고문 = 9217
    /// 총장의 방 문 = 9218 / 화분 = 9219
    /// 
    /// 총장의 저택 2층 - 상호작용하는 오브젝트 총 10개
    /// 계단 = 9220 / 그림 1 = 9221 / 그림 2 = 9222 / 책장 = 9223 / 창문 = 9224 / 옷장 = 9225 / 테이블 = 9226
    /// 책장 = 9227 / 작은 서랍장 = 9228 / 침대 = 9229
    /// 
    /// 자작의 저택 - 상호작용하는 오브젝트 총 19개
    /// 테라스 문 = 9230 / 쇼파 1, 쇼파 2 = 9231 / 테이블 = 9232 / 장식용 검 = 9233 / 사슴 박제 = 9234
    /// 벽난로 = 9235 / 원형계단 = 9236 / 선대 자작 초상화 = 9237 / 현 자작 초상화 = 9238 / 첫번째 방문 = 9239
    /// 자작의 방 문 = 9240 / 자작의 저택 = 9241 / 화분 = 9242 / 창문 = 9243 / 서랍장 = 9244 / 촛불 = 9245 / 침대 = 9246 / 샹들리에 = 9247
    /// 
    /// 시장 - 인물 총 5명
    /// 헤더 = 1600 / 벅 = 1601 / 세실 = 1602 / 브루노 = 1603 / 사람들 무리 = 1604q
    /// 
    /// 도심 - 인물 총 13명
    /// 존 = 1800 / 데이비드 = 1801 / 칼릭스 = 1802
    /// 찰스 = 1803 / 벤 = 1804 / 다리아 = 1805
    /// 르네 = 1806 / 클레오 = 1807 / 수다 떠는 여인 1 = 1808 / 수다 떠는 여인 2 = 1809
    /// 스미스 = 1810 / 찰리 = 1811 / 콜린 = 1812
    /// 
    /// 도심 - 상호작용하는 오브젝트 총 4개
    /// 부동산 = 9800 / 살롱 = 9801 / 카페 = 9802 / 분수 = 9803
    /// 
    /// 부동산 - 상호작용하는 오브젝트 총 6개
    /// 문 = 9804 / 지도 = 9805 / 도자기 1 = 9806 / 도자기 2 = 9807 / 의자 1, 의자 2 = 9808 / 테이블 = 9809
    /// 
    /// 살롱 - 상호작용하는 오브젝트 총 6개
    /// 진열된 옷들 1, 진열된 옷들 2 = 9810 / 샹들리에 = 9811 / 고급액자 = 9812 / 고급차상 = 9813 / 고급의자 1, 고급의자 2 = 9814 / 큰 거울 = 9815
    /// 
    /// 카페 - 상호작용하는 오브젝트 총 5개
    /// 의자 1, 의자 2 = 9816 / 원형 테이블 1, 원형 테이블 2 = 9817 / 카페 의자 1, 카페 의자 2 , 카페 의자 3 = 9818
    /// 카페 테이블 = 9819 / 샹들리에 = 9820
    /// 
    /// 숲 - 상호작용하는 오브젝트 총 11개
    /// 숲의 초입 = 9700 / 나무들 = 9701 / 나무 밑둥 = 9702 / 덤불 = 9703 / 남매의 집 = 9704 / 호수다리 1 = 9705 / 호수다리 2 = 9706
    /// 문 = 9707 / 화로 = 9708 / 나무책상 = 9709 / 2층 침대 = 9710
    /// 
    /// 주택가 - 상호작용하는 오브젝트 총 7개
    /// 주민집 = 9100 / 분수대 = 9101 / 표지판 = 9102 / 나무 = 9103 / 발루아 집 = 9104
    /// 우체통 = 9105 / 레이나 집문 = 9106
    /// 
    /// 레이나 집 - 상호작용하는 오브젝트 총 6개
    /// 쓰레기통 = 9107 / 싱크대 = 9108 / 식탁 = 9109 / 더블침대 = 9110 / 옷장 = 9111 / 이층침대 = 9112
    /// 
    /// 발루아 집 - 상호작용하는 오브젝트 총 6개
    /// 싱크대 = 9113 / 식탁 = 9114 / 옷장 = 9115 / 침대 = 9116 / 책장 = 9117 / 어질러진 책상 = 9118
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
                return "제렐";

            case "1014":
                return "시스템";

            case "1015":
                return "조수";

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

            case "제렐":
                return "1013";

            case "시스템":
                return "1014";

            case "조수":
                return "1015";

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

            case "슈벤":
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
            case "1200":
                return "문지기 1";

            case "1201":
                return "문지기 2";

            case "1202":
                return "마야";

            case "1203":
                return "총장 집 관리인";

            case "1204":
                return "주방 하녀";

            case "1205":
                return "집사";

            case "1206":
                return "청소하는 하인";

            case "1207":
                return "클로이";

            case "1208":
                return "노바";

            case "1209":
                return "제니";

            case "1210":
                return "톰";

            case "1211":
                return "청소하는 하녀 1";

            case "1212":
                return "청소하는 하인 2";

            case "1213":
                return "식료품 옮기는 하인";

            case "1214":
                return "청소하는 하녀 2";

            case "1215":
                return "청소하는 하녀 3";

            case "1219":
                return "캐시";

            default:
                return null;
        }
    }
    public string GetMansionNpcCodeFromName(string characterName)
    {
        switch (characterName)
        {
            case "문지기 1":
                return "1200";

            case "문지기 2":
                return "1201";

            case "마야":
                return "1202";

            case "총장 집 관리인":
                return "1203";

            case "주방 하녀":
                return "1204";

            case "집사":
                return "1205";

            case "청소하는 하인":
                return "1206";

            case "클로이":
                return "1207";

            case "노바":
                return "1208";

            case "제니":
                return "1209";

            case "톰":
                return "1210";

            case "청소하는 하녀 1":
                return "1211";

            case "청소하는 하인 2":
                return "1212";

            case "식료품 옮기는 하인":
                return "1213";

            case "청소하는 하녀 2":
                return "1214";

            case "청소하는 하녀 3":
                return "1215";

            case "캐시":
                return "1219";

            default:
                return null;
        }
    }

    // 2번째 index 값 = 3 -> 슬램가
    public string GetSlumNpcNameFromCode(string characterCode)
    {
        switch (characterCode)
        {
            case "1300":
                return "디펀";

            case "1301":
                return "아킬";

            case "1302":
                return "노숙자";

            case "1303":
                return "아반";

            case "1304":
                return "자드";

            case "1305":
                return "아울";

            case "1306":
                return "사람들";

            default:
                return null;
        }
    }
    public string GetSlumNpcCodeFromName(string characterName)
    {
        switch (characterName)
        {
            case "디펀":
                return "1300";

            case "아킬":
                return "1301";

            case "노숙자":
                return "1302";

            case "아반":
                return "1303";

            case "자드":
                return "1304";

            case "아울":
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
                return "마탄";

            case "1403":
                return "레누";

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

            case "마탄":
                return "1402";

            case "레누":
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
                return "코이체";

            case "1503":
                return "키로";

            case "1504":
                return "악당 1";

            case "1505":
                return "악당 2";

            case "1506":
                return "표 확인자";

            case "1507":
                return "감시자";

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

            case "코이체":
                return "1502";

            case "키로":
                return "1503";

            case "악당 1":
                return "1504";

            case "악당 2":
                return "1505";

            case "표 확인자":
                return "1506";

            case "감시자":
                return "1507";

            default:
                return null;
        }//switch()
    }

    // 2번째 index 값 = 6 -> 시장
    public string GetMarketNpcNameFromCode(string characterCode)
    {
        switch (characterCode)
        {
            case "1600":
                return "헤더";

            case "1601":
                return "벅";

            case "1602":
                return "세실";

            case "1603":
                return "브루노";

            case "1604":
                return "사람들 무리";

            default:
                return null;
        }
    }
    public string GetMarketNpcCodeFromName(string characterName)
    {
        switch (characterName)
        {
            case "헤더":
                return "1600";

            case "벅":
                return "1601";

            case "세실":
                return "1602";

            case "브루노":
                return "1603";

            case "사람들 무리":
                return "1604";

            default:
                return null;
        }
    }

    // 2번째 index 값 = 8 -> 도심
    public string GetDowntownNpcNameFromCode(string characterCode)
    {
        switch (characterCode)
        {
            case "1800":
                return "존";

            case "1801":
                return "데이비드";

            case "1802":
                return "칼릭스";

            case "1803":
                return "찰스";

            case "1804":
                return "벤";

            case "1805":
                return "다리아";

            case "1806":
                return "르네";

            case "1807":
                return "클레오";

            case "1808":
                return "수다 떠는 여인 1";

            case "1809":
                return "수다 떠는 여인 2";

            case "1810":
                return "스미스";

            case "1811":
                return "찰리";

            case "1812":
                return "콜린";

            default:
                return null;
        }
    }
    public string GetDowntownNpcCodeFromName(string characterName)
    {
        switch (characterName)
        {
            case "존":
                return "1800";

            case "데이비드":
                return "1801";

            case "칼릭스":
                return "1802";

            case "찰스":
                return "1803";

            case "벤":
                return "1804";

            case "다리아":
                return "1805";

            case "르네":
                return "1806";

            case "클레오":
                return "1807";

            case "수다 떠는 여인 1":
                return "1808";

            case "수다 떠는 여인 2":
                return "1809";

            case "스미스":
                return "1810";

            case "찰리":
                return "1811";

            case "콜린":
                return "1812";

            default:
                return null;
        }
    }

    /* 사물에 대한 코드 */
    // 코드 -> 이름 일때 _맵 위치 불포함
    public string GetObjectNameFromCode(string objectCode)
    {
        switch (objectCode)
        {
            // 항구
            case "9500":
                return "가판대 위치 1";
            case "9501":
                return "가판대 위치 2";
            case "9502":
                return "돛을 거는 기둥";
            case "9503":
                return "작은 배";
            case "9504":
                return "유람선";

            // 유람선
            case "9505":
                return "음식이 있는 테이블";
            case "9506":
                return "커다란 의자";
            case "9507":
                return "이상한 손잡이";
            case "9508":
                return "철장 1";
            case "9509":
                return "철장 2";
            case "9510":
                return "철장 3";

            // 지부
            case "9400":
                return "화분";
            case "9401":
                return "책장";
            case "9402":
                return "카펫";
            case "9403":
                return "책상";
            case "9404":
                return "사체";
            case "9405":
                return "달력";
            case "9406":
                return "편지 봉투";
            case "9407":
                return "티켓";
            case "9408":
                return "입양 서류";
            case "9409":
                return "금고";
            case "9410":
                return "금고속 종이";
            case "9411":
                return "창문";
            case "9412":
                return "책장";
            case "9413":
                return "금고";
            case "9414":
                return "창문";
            case "9415":
                return "책장";

            // 저택가
            case "9200":
                return "총장의 저택";
            case "9201":
                return "저택의 대문";
            case "9202":
                return "표지판 1";
            case "9203":
                return "표지판 2";
            case "9248":
                return "표지판 3";

            // 총장의 저택 1층
            case "9204":
                return "옆문";
            case "9205":
                return "주방 문";
            case "9206":
                return "식탁";
            case "9207":
                return "창문";
            case "9208":
                return "와인";
            case "9209":
                return "샹들리에";
            case "9210":
                return "도자기";
            case "9211":
                return "계단 1";
            case "9212":
                return "쇼파";
            case "9213":
                return "테이블";
            case "9214":
                return "그림";
            case "9215":
                return "벽난로";
            case "9216":
                return "상자더미";
            case "9217":
                return "창고문";
            case "9218":
                return "총장의 방 문";
            case "9219":
                return "화분";

            // 총장의 저택 2층
            case "9220":
                return "계단 2";
            case "9221":
                return "그림 1";
            case "9222":
                return "그림 2";
            case "9223":
                return "책장 1";
            case "9224":
                return "창문";
            case "9225":
                return "옷장";
            case "9226":
                return "테이블";
            case "9227":
                return "책장 2";
            case "9228":
                return "작은 서랍장";
            case "9229":
                return "침대";

            // 자작의 저택
            case "9230":
                return "테라스 문";
            case "9231":
                return "쇼파 1";
            case "9232":
                return "테이블";
            case "9233":
                return "장식용 검";
            case "9234":
                return "사슴 박제";
            case "9235":
                return "벽난로";
            case "9236":
                return "원형계단";
            case "9237":
                return "선대 자작 초상화";
            case "9238":
                return "현 자작 초상화";
            case "9239":
                return "첫번째 방문";
            case "9240":
                return "자작의 방 문";
            case "9241":
                return "자작의 저택";
            case "9242":
                return "화분";
            case "9243":
                return "창문";
            case "9244":
                return "서랍장";
            case "9245":
                return "촛불";
            case "9246":
                return "침대";
            case "9247":
                return "샹들리에";

            // 슬램가
            case "9300":
                return "나무상자";
            case "9301":
                return "창문";
            case "9302":
                return "종이";
            case "9303":
                return "정보상 문";
            case "9304":
                return "쓰레기 더미(왼쪽)";
            case "9305":
                return "쓰레기 더미(중앙)";
            case "9306":
                return "쓰레기 더미(오른쪽)";
            case "9307":
                return "기둥";
            case "9308":
                return "허름한 집 문";

            // 숲
            case "9700":
                return "숲의 초입";
            case "9701":
                return "나무들";
            case "9702":
                return "나무 밑둥";
            case "9703":
                return "덤불";
            case "9704":
                return "남매의 집";
            case "9705":
                return "호수 다리 1";
            case "9706":
                return "호수 다리 2";
            case "9707":
                return "문";
            case "9708":
                return "화로";
            case "9709":
                return "나무책상";
            case "9710":
                return "2층 침대";

            // 주택가
            case "9100":
                return "주민집";
            case "9101":
                return "분수대";
            case "9102":
                return "표지판";
            case "9103":
                return "나무";
            case "9104":
                return "발루아 집";
            case "9105":
                return "우체통";
            case "9106":
                return "레이나 집문";

            // 레이나 집
            case "9107":
                return "쓰레기통";
            case "9108":
                return "싱크대";
            case "9109":
                return "식탁";
            case "9110":
                return "더블침대";
            case "9111":
                return "옷장";
            case "9112":
                return "이층침대";

            // 발루아 집
            case "9113":
                return "싱크대";
            case "9114":
                return "식탁";
            case "9115":
                return "옷장";
            case "9116":
                return "침대";
            case "9117":
                return "책장";
            case "9118":
                return "어질러진 책상";

            // 도심
            case "9800":
                return "부동산";
            case "9801":
                return "살롱";
            case "9802":
                return "카페";
            case "9803":
                return "분수";

            // 부동산
            case "9804":
                return "문";
            case "9805":
                return "지도";
            case "9806":
                return "도자기 1";
            case "9807":
                return "도자기 2";
            case "9808":
                return "의자 1";
            case "9809":
                return "테이블";

            // 살롱
            case "9810":
                return "진열된 옷들 1";
            case "9811":
                return "샹들리에";
            case "9812":
                return "고급액자";
            case "9813":
                return "고급차상";
            case "9814":
                return "고급의자 1";
            case "9815":
                return "큰 거울";

            // 카페
            case "9816":
                return "의자 1";
            case "9817":
                return "원형 테이블 1";
            case "9818":
                return "카페 의자 1";
            case "9819":
                return "카페 테이블";
            case "9820":
                return "샹들리에";

            default:
                return null;
        }
    }

    // 이름 -> 코드일때 _맵 위치 포함
    public string GetObjectCodeFromName(string objectName)
    {
        switch (objectName)
        {
            // 항구
            case "가판대 위치 1_항구":
                return "9500";
            case "가판대 위치 2_항구":
                return "9501";
            case "돛은 거는 기둥_항구":
                return "9502";
            case "작은 배_항구":
                return "9503";
            case "유람선_항구":
                return "9504";

            // 유람선
            case "음식이 있는 테이블_유람선":
                return "9505";
            case "커다란 의자_유람선":
                return "9506";
            case "이상한 손잡이_유람선":
                return "9507";
            case "철장 1_유람선":
                return "9508";
            case "철장 2_유람선":
                return "9509";
            case "철장 3_유람선":
                return "9510";

            // 지부
            case "화분_지부":
                return "9400";
            case "책장_지부":
                return "9401";
            case "카펫_지부":
                return "9402";
            case "책상_지부":
                if (PlayerManager.instance.NumOfAct.Equals("53"))
                {
                    int num = Random.Range(0, 4);
                    if (num == 0)
                        return "9403";  // 책상에 대한 대화 출력
                    else if (num == 1)
                        return "9406";  // 편지 봉투에 대한 대화 출력
                    else if (num == 2)
                        return "9407";  // 티켓에 대한 대화 출력
                    else if (num == 3)
                        return "9408";  // 입양 서류에 대한 대화 출력
                    else
                        return null;
                }
                else
                {
                    return "9403";
                }
            case "사체_지부":
                return "9404";
            case "달력_지부":
                return "9405";
            case "금고_지부":
                return "9409";
            case "금고속 종이_지부":
                return "9410";
            case "창문_지부2층":
                return "9411";
            case "책장_지부2층":
                return "9412";
            case "금고_주인공사무실":
                return "9413";
            case "창문_주인공사무실":
                return "9414";
            case "책장_주인공사무실":
                return "9415";

            // 저택가
            case "총장의 저택_저택가":
                return "9200";
            case "저택의 대문_저택가":
                return "9201";
            case "표지판 1_저택가":
                return "9202";
            case "표지판 2_저택가":
                return "9203";
            case "표지판 3_저택가":
                return "9248";

            // 총장의 저택 1층
            case "옆문_총장의 저택 1층":
                return "9204";
            case "주방 문_총장의 저택 1층":
                return "9205";
            case "식탁_총장의 저택 1층":
                return "9206";
            case "창문_총장의 저택 1층":
                return "9207";
            case "와인_총장의 저택 1층":
                return "9208";
            case "샹들리에_총장의 저택 1층":
                return "9209";
            case "도자기_총장의 저택 1층":
                return "9210";
            case "계단 1_총장의 저택 1층":
                return "9211";
            case "쇼파_총장의 저택 1층":
                return "9212";
            case "테이블_총장의 저택 1층":
                return "9213";
            case "그림_총장의 저택 1층":
                return "9214";
            case "벽난로_총장의 저택 1층":
                return "9215";
            case "상자더미_총장의 저택 1층":
                return "9216";
            case "창고문_총장의 저택 1층":
                return "9217";
            case "총장의 방 문_총장의 저택 1층":
                return "9218";
            case "화분_총장의 저택 1층":
                return "9219";

            // 총장의 저택 2층
            case "계단 2_총장의 저택 2층":
                return "9220";
            case "그림 1_총장의 저택 2층":
                return "9221";
            case "그림 2_총장의 저택 2층":
                return "9222";
            case "책장 1_총장의 저택 2층":
                return "9223";
            case "창문_총장의 저택 2층":
                return "9224";
            case "옷장_총장의 저택 2층":
                return "9225";
            case "테이블_총장의 저택 2층":
                return "9226";
            case "책장 2_총장의 저택 2층":
                return "9227";
            case "작은 서랍장_총장의 저택 2층":
                return "9228";
            case "침대_총장의 저택 2층":
                return "9229";

            // 자작의 저택
            case "테라스 문_자작의 저택":
                return "9230";
            case "쇼파 1_자작의 저택":
            case "쇼파 2_자작의 저택":
                return "9231";
            case "테이블_자작의 저택":
                return "9232";
            case "장식용 검_자작의 저택":
                return "9233";
            case "사슴 박제_자작의 저택":
                return "9234";
            case "벽난로_자작의 저택":
                return "9235";
            case "원형계단_자작의 저택":
                return "9236";
            case "선대 자작 초상화_자작의 저택":
                return "9237";
            case "현 자작 초상화_자작의 저택":
                return "9238";
            case "첫번째 방문_자작의 저택":
                return "9239";
            case "자작의 방 문_자작의 저택":
                return "9240";
            case "자작의 저택_자작의 저택":
                return "9241";
            case "화분_자작의 저택":
                return "9242";
            case "창문_자작의 저택":
                return "9243";
            case "서랍장_자작의 저택":
                return "9244";
            case "촛불_자작의 저택":
                return "9245";
            case "침대_자작의 저택":
                return "9246";
            case "샹들리에_자작의 저택":
                return "9247";

            // 슬램가
            case "나무상자_슬램가":
                return "9300";
            case "창문_슬램가":
                return "9301";
            case "종이_슬램가":
                return "9302";
            case "정보상 문_슬램가":
                return "9303";
            case "쓰레기 더미(왼쪽)_슬램가":
                return "9304";
            case "쓰레기 더미(중앙)_슬램가":
                return "9305";
            case "쓰레기 더미(오른쪽)_슬램가":
                return "9306";
            case "기둥_슬램가":
                return "9307";
            case "허름한 집 문_슬램가":
                return "9308";

            // 숲
            case "숲의 초입_숲":
                return "9700";
            case "나무들_숲":
                return "9701";
            case "나무 밑둥_숲":
                return "9702";
            case "덤불_숲":
                return "9703";
            case "남매의 집_숲":
                return "9704";
            case "호수 다리 1_숲":
                return "9705";
            case "호수 다리 2_숲":
                return "9706";
            case "문_숲":
                return "9707";
            case "화로_숲":
                return "9708";
            case "나무책상_숲":
                return "9709";
            case "2층 침대_숲":
                return "9710";

            // 주택가
            case "주민집_주택가":
                return "9100";
            case "분수대_주택가":
                return "9101";
            case "표지판_주택가":
                return "9102";
            case "나무_주택가":
                return "9103";
            case "발루아 집_주택가":
                return "9104";
            case "우체통_주택가":
                return "9105";
            case "레이나 집문_주택가":
                return "9106";

            // 레이나 집
            case "쓰레기통_레이나 집":
                return "9107";
            case "싱크대_레이나 집":
                return "9108";
            case "식탁_레이나 집":
                return "9109";
            case "더블침대_레이나 집":
                return "9110";
            case "옷장_레이나 집":
                return "9111";
            case "이층침대_레이나 집":
                return "9112";

            // 발루아 집
            case "싱크대_발루아 집":
                return "9113";
            case "식탁_발루아 집":
                return "9114";
            case "옷장_발루아 집":
                return "9115";
            case "침대_발루아 집":
                return "9116";
            case "책장_발루아 집":
                return "9117";
            case "어질러진 책상_발루아 집":
                return "9118";

            // 도심
            case "부동산_도심":
                return "9800";
            case "살롱_도심":
                return "9801";
            case "카페_도심":
                return "9802";
            case "분수_도심":
                return "9803";

            // 부동산
            case "문_부동산":
                return "9804";
            case "지도_부동산":
                return "9805";
            case "도자기 1_부동산":
                return "9806";
            case "도자기 2_부동산":
                return "9807";
            case "의자 1_부동산":
            case "의자 2_부동산":
                return "9808";
            case "테이블_부동산":
                return "9809";

            // 살롱
            case "진열된 옷들 1_살롱":
            case "진열된 옷들 2_살롱":
                return "9810";
            case "샹들리에_살롱":
                return "9811";
            case "고급액자_살롱":
                return "9812";
            case "고급차상_살롱":
                return "9813";
            case "고급의자 1_살롱":
            case "고급의자 2_살롱":
                return "9814";
            case "큰 거울_살롱":
                return "9815";

            // 카페
            case "의자 1_카페":
            case "의자 2_카페":
                return "9816";
            case "원형 테이블 1_카페":
            case "원형 테이블 2_카페":
                return "9817";
            case "카페 의자 1_카페":
            case "카페 의자 2_카페":
            case "카페 의자 3_카페":
                return "9818";
            case "카페 테이블_카페":
                return "9819";
            case "샹들리에_카페":
                return "9820";

            default:
                return null;
        }
    }

    // 코드 -> 이름 일때 _맵 위치 불포함
    public string GetNpcNameFromCode(string characterCode)
    {
        if (characterCode[0].Equals('1'))
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
                    return GetSlumNpcNameFromCode(characterCode);

                case '4':
                    return GetChapterNpcNameFromCode(characterCode);

                case '5':
                    return GetHarborNpcNameFromCode(characterCode);

                case '6':
                    return GetMarketNpcNameFromCode(characterCode);

                case '8':
                    return GetDowntownNpcNameFromCode(characterCode);

                default:
                    return characterCode;
            }
        }
        else
        {
            return GetObjectNameFromCode(characterCode);
        }
    }

    // 이름 -> 코드일때 _맵 위치 포함
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
        else if ((tempNpcName = GetSlumNpcCodeFromName(characterName)) != null)
            return tempNpcName;
        else if ((tempNpcName = GetChapterNpcCodeFromName(characterName)) != null)
            return tempNpcName;
        else if ((tempNpcName = GetHarborNpcCodeFromName(characterName)) != null)
            return tempNpcName;
        else if ((tempNpcName = GetMarketNpcCodeFromName(characterName)) != null)
            return tempNpcName;
        else if ((tempNpcName = GetDowntownNpcCodeFromName(characterName)) != null)
            return tempNpcName;
        else if ((tempNpcName = GetObjectCodeFromName(characterName)) != null)
            return tempNpcName;
        else
            return characterName;
    }
}
