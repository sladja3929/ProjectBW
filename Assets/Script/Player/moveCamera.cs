using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera : MonoBehaviour
{

    public GameObject Player;
    Vector3 cameraPosition;
    float followSpeed = 10;

    public float offsetX = 0.5f;
    public float offsetY = 0.5f;
    public float offsetZ = -4f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosition.x = Player.transform.position.x + offsetX;
        cameraPosition.y = Player.transform.position.y + offsetY;
        cameraPosition.z = Player.transform.position.z + offsetZ;

        //단순 이동
        //transform.position = cameraPosition;

        //딜레이를 가지고 따라가기
        transform.position = Vector3.Lerp(transform.position, cameraPosition, followSpeed * Time.deltaTime);
    }
    private void LateUpdate()
    {
        
    }
}
