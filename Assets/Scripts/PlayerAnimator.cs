using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Player player;

    private const string IsWalking = "IsWalking";

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(IsWalking, false);
    }

    void Update()
    {
        animator.SetBool(IsWalking, player.IsWalking);
    }
}
