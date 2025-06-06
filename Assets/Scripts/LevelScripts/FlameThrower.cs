using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public bool isActive = false;

    public ParticleSystem flameParticles;
    public Transform checkPoint;
    public AudioSource flameAudio;
    public FlameThrowerController controllerPuzzle;
    public Collider damageTrigger; // Genellikle bir Box Collider - isTrigger

    private void Start()
    {
        flameParticles = GetComponentInChildren<ParticleSystem>();
        flameAudio = GetComponent<AudioSource>();
        SetActive(false);
        damageTrigger=transform.GetComponent<Collider>();
    }

    public void SetActive(bool active)
    {
        isActive = active;

        if (flameParticles != null)
        {
            if (active && !flameParticles.isPlaying)
                flameParticles.Play();
            else if (!active && flameParticles.isPlaying)
                flameParticles.Stop();
        }

        if (flameAudio != null)
        {
            if (active && !flameAudio.isPlaying)
                flameAudio.Play();
            else if (!active && flameAudio.isPlaying)
                flameAudio.Stop();
        }

        new WaitForSeconds(1f);
        if (damageTrigger != null)
        {
            damageTrigger.enabled = active;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            Debug.Log("Player hit by flame!");
            Player3DController controller = other.GetComponent<Player3DController>();
            if (controller != null)
            {
                controllerPuzzle.ResetFlames();
                controller.RespawnOnCheckpoint(checkPoint);
                controller.OnDeath(); // veya Respawn çaðrýsý
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            Debug.Log("Player hit by flame!");
            Player3DController controller = other.GetComponent<Player3DController>();
            if (controller != null)
            {
                controllerPuzzle.ResetFlames();
                controller.RespawnOnCheckpoint(checkPoint);
                controller.OnDeath(); // veya Respawn çaðrýsý
            }
        }
    }
}
