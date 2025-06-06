using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public float walkInterval = 0.5f;
    public float runInterval = 0.3f;

    private Player3DController controller;
    private float stepTimer = 0f;

    void Start()
    {
        controller = GetComponent<Player3DController>();
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        var state = controller.CurrentState;

        bool canStep = false;
        float currentInterval = walkInterval;

        if (state == Player3DController.MovementState.Walking)
        {
            canStep = true;
            currentInterval = walkInterval;
        }
        else if (state == Player3DController.MovementState.Running)
        {
            canStep = true;
            currentInterval = runInterval;
        }

        if (canStep)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= currentInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f; // hareket etmiyorsa sýfýrla
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length > 0)
        {
            AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}
