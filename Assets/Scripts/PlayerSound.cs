using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private float footstepTimerMax = 0.3f; // 발소리 간격

    private float footstepTimer;
    private Player player;

    void Awake()
    {
        player = GetComponent<Player>();   
    }

    void Start()
    {
        footstepTimer = footstepTimerMax; // 발소리 타이머 초기화
    }

    void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0f)
        {
            if (player.IsWalking)
            {
                footstepTimer = footstepTimerMax; // 발소리 타이머 초기화
                SoundManager.Instance.PlayFootstepSound(player.transform.position); // 발소리 재생
            }
        }
    }
}
