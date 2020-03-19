using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이름만 Cloud지, 사실상 움직이는 배경물체들에게 모두 있는 스크립트
public class Cloud : MonoBehaviour
{
    // 구름의 Transform 컴포넌트
    private Transform cloudTransform;
    // 구름이 이동할 목적지 위치
    public Vector2 destinationPos;
    // 구름이 리셋될 위치
    public Vector2 resetPos;
    public Vector2 tempLocalpos;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        cloudTransform = GetComponent<Transform>();
        tempLocalpos.y = cloudTransform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (cloudTransform.localPosition.x >= destinationPos.x)
        {
            // 점점 구름을 이동시킴
            tempLocalpos.x = cloudTransform.localPosition.x - Time.deltaTime * speed;
            // 구름 위치 변경 적용
            cloudTransform.localPosition = tempLocalpos;
        }
        else if (cloudTransform.localPosition.x < destinationPos.x)
        {
            cloudTransform.localPosition = resetPos;
        }
    }
}
