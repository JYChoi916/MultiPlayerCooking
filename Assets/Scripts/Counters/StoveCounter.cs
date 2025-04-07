using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged; // 진행도 변경 이벤트

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;   // 상태 변경 이벤트
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State 
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private FryingRecipeSO fryingRecipeSO;
    private float fryingTimer = 0f;
    private BurningRecipeSO burningRecipeSO;
    private float burningTimer = 0f;


    void Start()
    {
        state = State.Idle;
        fryingTimer = 0f;
    }

    void Update()
    {
        if (HasKitchenObject())
        {
            switch(state)
            {
                case State.Idle:
                    // 대기 상태
                    break;
                case State.Frying:
                    // 조리 중 상태
                    fryingTimer += Time.deltaTime;
                    
                    // 타이머가 최대값에 도달하면
                    if (fryingTimer >= fryingRecipeSO.fryingTimerMax)
                    {
                        fryingTimer = 0f; // 타이머 초기화
                        GetKitchenObject().DestroySelf();   // 현재 주방 오브젝트를 파괴하고
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this); // 프라이된 오브젝트를 생성

                        burningRecipeSO = GetCorrectBurningRecipe(fryingRecipeSO.output); // 조리된 오브젝트에 맞는 BurningRecipeSO를 찾음
                        state = State.Fried; // 조리 완료 상태로 변경
                        burningTimer = 0f; // 타이머 초기화

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });
                    }

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressAmount = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                    break;
                case State.Fried:
                    // 조리 완료 상태
                    burningTimer += Time.deltaTime;
                    if (burningTimer >= burningRecipeSO.burningTimerMax)
                    {
                        // 타버린 상태
                        GetKitchenObject().DestroySelf(); // 다 익은 오브젝트를 파괴하고
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this); // 타버린 오브젝트를 생성
                        state = State.Burned; // 타버린 상태로 변경
                        burningTimer = 0f; // 타이머 초기화
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
                            state = state
                        });
                    }

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressAmount = burningTimer / burningRecipeSO.burningTimerMax
                    });                     
                    break;
                case State.Burned:
                    // 타버린 상태
                    break;
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // 이 카운터가 오브젝트를 가지고 있지 않다면
            if (player.HasKitchenObject())
            {
                // 플레이어가 오브젝트를 가지고 있다면
                // 플레이어의 주방 오브젝트를 이 카운터의 자식으로 설정
                player.GetKitchenObject().SetKitchenObjectParent(this);

                fryingRecipeSO = GetCorrectFryingRecipe(GetKitchenObject().GetKitchenObjectSO());
                burningRecipeSO = GetCorrectBurningRecipe(GetKitchenObject().GetKitchenObjectSO());
                if (fryingRecipeSO != null)
                {
                    state = State.Frying; // 조리 중 상태로 변경
                    fryingTimer = 0f;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressAmount = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });
                }
                else if (burningRecipeSO != null)
                {
                    state = State.Fried; // 익힌 상태로 변경
                    burningTimer = 0f;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                        progressAmount = burningTimer / burningRecipeSO.burningTimerMax
                    });
                }
                else
                {
                    state = State.Idle; // 조리 중 상태가 아님
                    Debug.Log("Not a Fryable Object!");
                }
            }
        }
        else {
            // 이 카운터가 오브젝트를 가지고 있다면
            if (player.HasKitchenObject())
            {
                // 플레이어가 오브젝트를 가지고 있다면
                // 접시인지 확인후
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // 플레이어가 접시를 가지고 있다면
                    // 접시에 올려 보고
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // 제대로 접시에 올려졌다면
                        GetKitchenObject().DestroySelf(); // 카운터에 있는 오브젝트를 파괴
                        state = State.Idle;
                        fryingTimer = 0f; // 타이머 초기화
                        burningTimer = 0f; // 타이머 초기화

                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                            progressAmount = 0
                        });
                    }
                }
                else
                {
                    // 플레이가 들고 있는게 접시가 아니다!

                    // 그럼 카운터에 있는 오브젝트는 접시인가?
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // 접시가 맞다면
                        // 플레이어가 가지고 있는 오브젝트를 접시에 올려보고
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            // 제대로 접시에 올려졌다면
                            // 플레이어가 가지고 있는 오브젝트를 파괴
                            player.GetKitchenObject().DestroySelf();
                            state = State.Idle;
                            fryingTimer = 0f; // 타이머 초기화
                            burningTimer = 0f; // 타이머 초기화

                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                                progressAmount = 0
                            });
                        }
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                fryingTimer = 0f; // 타이머 초기화
                burningTimer = 0f; // 타이머 초기화

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressAmount = 0
                });
            }
        }

        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
            state = state
        });
    }

    // 주방 오브젝트에 맞는 조리법 ScriptableObject를 찾는 메서드
    private FryingRecipeSO GetCorrectFryingRecipe(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
        {
            if (fryingRecipeSO.input == inputKitchenObjectSO)
            {
                return fryingRecipeSO;
            }
        }

        return null;
    }

    private BurningRecipeSO GetCorrectBurningRecipe(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }

        return null;
    }
}
