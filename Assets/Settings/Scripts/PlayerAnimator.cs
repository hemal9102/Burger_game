using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    [SerializeField] private Player player;
    private const string IS_WALKING = "IsWalking";
    private Animator Animator;
    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Animator.SetBool(IS_WALKING, player.IsWalking());

    }

}
