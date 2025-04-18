using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler<OnRecipeEventArgs> OnRecipeSpawned;   // 주문이 들어올 때 발생하는 이벤트
    public event EventHandler<OnRecipeEventArgs> OnRecipeCompleted; // 주문을 서빙했을때 발생하는 이벤트
    public class OnRecipeEventArgs : EventArgs
    {
        public RecipeSO recipeSO; // 서빙에 성공한 RecipeSO
    }

    public event EventHandler<OnDeliveredEventArgs> OnRecipeSuccess;
    public event EventHandler<OnDeliveredEventArgs> OnRecipeFailed;                       // 주문 서빙에 실패했을 때 발생하는 이벤트
    public class OnDeliveredEventArgs : EventArgs
    {
        public DeliveryCounter counter; // 서빙한 DeliveryCounter
    }

    public static DeliveryManager Instance { get; private set; } // 싱글톤 인스턴스

    [SerializeField] private RecipeListSO availableRecipes; // 주문이 들어올수 있는 레시피 리스트
    [SerializeField] private float spawnRecipeTimerMax = 4f; // 주문이 들어오는 시간 간격
    [SerializeField] private int maxWaitingRecipeCount = 4; // 최대 대기 주문 수
    private List<RecipeSO> waitingRecipeSOList; // 주문되어서 대기중인 레시피 리스트
    private float spawnRecipeTimer; // 마지막 주문으로 부터 지난 시간
    private int waitingRecipeCount; // 대기중인 주문 수

    private int successfulRecipeCount; // 성공적으로 서빙한 주문 수

    void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>(); // 주문 대기 리스트 초기화
    }

    void Start()
    {
        spawnRecipeTimer = spawnRecipeTimerMax;
        waitingRecipeCount = 0;
        successfulRecipeCount = 0;
    }

    void Update()
    {
        spawnRecipeTimer -= Time.deltaTime; // 마지막 주문으로 부터 지난 시간 증가
        if (spawnRecipeTimer <= 0)
        {
            spawnRecipeTimer = spawnRecipeTimerMax; // 타이머 초기화

            if (waitingRecipeCount < maxWaitingRecipeCount)
            {
                waitingRecipeCount++; // 대기중인 주문 수 증가

                // 주문이 들어올 수 있는 레시피 리스트에서 랜덤으로 레시피 선택
                RecipeSO orderRecipe = availableRecipes.recipeSOList[UnityEngine.Random.Range(0, availableRecipes.recipeSOList.Count)]; // 랜덤으로 레시피 선택
                Debug.Log("주문이 들어왔습니다: " + orderRecipe.recipeName); // 주문된 레시피 이름 출력
                waitingRecipeSOList.Add(orderRecipe);

                OnRecipeSpawned?.Invoke(this, new OnRecipeEventArgs{
                    recipeSO = orderRecipe
                }); // 주문이 들어왔을 때 발생하는 이벤트 호출
            }
            else 
            {
                spawnRecipeTimer = spawnRecipeTimerMax;
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject, DeliveryCounter deliveryCounter)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            bool plateKitchenObjectMatch = true;

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool ingredientFound = true;
                // 같은 숫자의 재료가 있는 경우
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    if(!plateKitchenObject.GetKitchenObjectSOList().Contains(recipeKitchenObjectSO))
                    {
                        // 접시에 담긴 재료 리스트에 레시피 재료가 없는 경우
                        ingredientFound = false;

                        // 루프 탈출
                        break;
                    }
                }

                if (ingredientFound == false)
                {
                    // 매칭되는 재료가 없는 경우는 주문 실패
                    plateKitchenObjectMatch = false;
                }
            }
            else
            {
                // 같은 숫자의 재료가 없는 경우는 주문 실패
                plateKitchenObjectMatch = false;
            }

            if (plateKitchenObjectMatch) {
                // 주문 성공
                Debug.Log("서빙 완료 : " + waitingRecipeSO.recipeName); // 주문 성공 메시지 출력
                waitingRecipeSOList.RemoveAt(i);                        // 주문 리스트에서 제거
                waitingRecipeCount--;                                   // 대기중인 주문 수 감소
                successfulRecipeCount++;

                OnRecipeCompleted?.Invoke(this, new OnRecipeEventArgs {
                    recipeSO = waitingRecipeSO
                });

                OnRecipeSuccess?.Invoke(this, new OnDeliveredEventArgs {
                    counter = deliveryCounter
                });

            return;                                                // 메소드 종료
            }
        }

        // 서빙 실패
        Debug.Log("서빙 실패! 일치하는 주문 레시피가 없습니다."); // 주문 실패 메시지 출력

        // 주문 실패 이벤트 호출
        OnRecipeFailed?.Invoke(this, new OnDeliveredEventArgs {
            counter = deliveryCounter
        }); 
        return;
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList; // 대기중인 주문 리스트 반환
    }

    public int GetSuccessfulRecipeCount()
    {
        return successfulRecipeCount; // 성공적으로 서빙한 주문 수 반환
    }
}
