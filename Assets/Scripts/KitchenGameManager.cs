using System;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour
{
    public event EventHandler OnStateChanged; // 상태 변경 이벤트
    public event EventHandler OnGamePaused; // 게임 일시 정지 이벤트
    public event EventHandler OnGameUnpaused; // 게임 일시 정지 이벤트

    public static KitchenGameManager Instance { get; private set; } // 싱글톤 인스턴스

    private bool isGamePaused;

    private enum State {
        WaitingForStart,
        CountdownStart,
        GamePlaying,
        GameOver
    }

    private State state;
    private float waitingToStartTimer = 1f; 
    private float countdownToStartTimer = 3f;
    private float gamePlayingTimer;
    [SerializeField] private float gamePlayingTimerMax = 60f; // 게임 플레이 최대 시간

    void Awake()
    {
        Instance = this;
        state = State.WaitingForStart;
    }

    void Start()
    {
        gamePlayingTimer = gamePlayingTimerMax; // 게임 플레이 타이머 초기화
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction; // 일시 정지 액션 이벤트 구독
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    void Update()
    {
        switch(state)
        {
            case State.WaitingForStart:
                waitingToStartTimer -= Time.deltaTime; // 대기 타이머 감소
                if (waitingToStartTimer < 0f) // 타이머가 0 이하가 되면
                {
                    state = State.CountdownStart; // 카운트다운 시작 상태로 변경
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.CountdownStart:
                countdownToStartTimer -= Time.deltaTime; // 카운트다운 타이머 감소
                if (countdownToStartTimer < 0f) // 타이머가 0 이하가 되면
                {
                    state = State.GamePlaying; // 게임 플레이 상태로 변경
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime; // 게임 플레이 타이머 감소
                if (gamePlayingTimer < 0f)
                {
                    state = State.GameOver; // 게임 오버 상태로 변경
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying; // 현재 상태가 게임 플레이 상태인지 확인
    }

    public bool IsCountdownToStart()
    {
        return state == State.CountdownStart; // 현재 상태가 카운트다운 시작 상태인지 확인
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer; // 카운트다운 타이머 반환
    }

    public bool IsGameOver()
    {
        return state == State.GameOver; // 현재 상태가 게임 오버 상태인지 확인
    }

    public float GetGamePlayingTimerNormalized()
    {
        return gamePlayingTimer / gamePlayingTimerMax;
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty); // 게임 일시 정지 이벤트 발생
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty); // 게임 재개 이벤트 발생
        }
    }
}
