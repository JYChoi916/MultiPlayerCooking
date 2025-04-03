using System;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned; // 접시 생성 이벤트
    public event EventHandler OnPlateRemoved; // 접시 제거 이벤트

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO; // 접시 오브젝트
    [SerializeField] private int platesSpawnedAmountMax = 4; // 최대 생성 가능한 접시 수
    [SerializeField] private float spawnPlateTimerMax = 4f; // 접시 생성 주기

    private float spawnPlateTimer;
    private int platesSpawnedAmount; // 생성된 접시 수

    void Update()
    {
        if (platesSpawnedAmount < platesSpawnedAmountMax)
        {
            spawnPlateTimer += Time.deltaTime;
            if (spawnPlateTimer >= spawnPlateTimerMax)
            {
                spawnPlateTimer = 0f; // 타이머 초기화
                platesSpawnedAmount++;

                OnPlateSpawned?.Invoke(this, EventArgs.Empty); // 접시 생성 이벤트 발생
            }
        }
    }

    public override void Interact(Player player)
    {
        // 플레이어가 손에 주방오브젝트를 들고 있지 않은 경우
        if (!player.HasKitchenObject())
        {
            if (platesSpawnedAmount > 0)
            {
                platesSpawnedAmount--; // 생선된 접시 수 감소
                
                // 쌓여둔 접시 비주얼에서 하나 제거
                OnPlateRemoved?.Invoke(this, EventArgs.Empty); // 접시 제거 이벤트 발생

                // 플레이어에게 접시 생성
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            }
        }
    }
}
