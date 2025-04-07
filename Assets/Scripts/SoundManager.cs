using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; } // 싱글톤 인스턴스
    private void Awake()
    {
        Instance = this; // 싱글톤 인스턴스 초기화
    }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO; // 오디오 클립 참조 스크립터블 오브젝트

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess; // 레시피 성공 이벤트 구독
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed; // 레시피 실패 이벤트 구독
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut; // 재료 자르기 이벤트 구독
        Player.Instance.OnPickedSomething += Player_OnPickedSomething; // 플레이어가 오브젝트를 집었을 때 이벤트 구독
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere; // 오브젝트가 카운터에 놓였을 때 이벤트 구독
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed; // 오브젝트가 쓰레기통에 버려졌을 때 이벤트 구독
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter; // 이벤트 발생한 TrashCounter
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position); // 오브젝트 버리기 사운드 재생
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter; // 이벤트 발생한 BaseCounter
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position); // 오브젝트 놓기 사운드 재생
    }

    private void Player_OnPickedSomething(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position); // 오브젝트 집기 사운드 재생
    }

    private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter; // 이벤트 발생한 CuttingCounter
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, DeliveryManager.OnDeliveredEventArgs e)
    {
        
        PlaySound(audioClipRefsSO.deliverySuccess, e.counter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, DeliveryManager.OnDeliveredEventArgs e)
    {
        DeliveryCounter deliveryCounter = sender as DeliveryCounter;
        PlaySound(audioClipRefsSO.deliveryFail, e.counter.transform.position);
    }

    private void PlaySound(AudioClip[] clips, Vector3 position, float volume = 1f) 
    {
        PlaySound(clips[UnityEngine.Random.Range(0, clips.Length)], position, volume);
    }

    private void PlaySound(AudioClip clip, Vector3 position, float volume = 1f) 
    {
        AudioSource.PlayClipAtPoint(clip, position, volume); // 지정된 위치에서 오디오 클립 재생
    }

    public void PlayFootstepSound(Vector3 position) 
    {
        PlaySound(audioClipRefsSO.footstep, position); // 발소리 재생
    }
}
