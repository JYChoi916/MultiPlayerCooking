using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter; // PlatesCounter
    [SerializeField] private Transform counterTopPoint; // 접시가 놓일 위치
    [SerializeField] private Transform plateVisualPrefab; // 접시 비주얼 프리팹

    private List<GameObject> plateVisualGameObjectList; // 접시 비주얼 리스트

    void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>(); // 접시 비주얼 리스트 초기화
    }

    void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned; // 접시 생성 이벤트 구독
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved; // 접시 제거 이벤트 구독
    }

    private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint); // 접시 비주얼 생성
        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count ,0);
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject); // 리스트에 추가
    }

    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        int lastIndex = plateVisualGameObjectList.Count - 1; // 마지막 인덱스
        GameObject plateVisualGameObject = plateVisualGameObjectList[lastIndex]; // 마지막 접시
        plateVisualGameObjectList.RemoveAt(lastIndex); // 리스트에서 제거
        Destroy(plateVisualGameObject); // 마지막 접시 오브젝트 파괴
    }
}
