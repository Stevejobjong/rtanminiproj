using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelBackground : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float scrollAmount;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector3 moveDirection;

    private void Update()
    {
        //배경 이미지가 moveDirection 방향으로 moveSpeed 속도만큼 이동
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        //배경이 화면 밖으로 나가면 반복되도록 위치 조정
        if (transform.position.x <= -scrollAmount)
        {
            transform.position = target.position - moveDirection * scrollAmount;
        }
    }
}
