using UnityEngine;

public class TrapFlame : MonoBehaviour
{
    public ParticleSystem flameEffect;
    public Collider flameTrigger; // Trigger collider (ayný objede olabilir)
    public float activeTime = 3f;
    public float inactiveTime = 2f;

    public AudioSource flameAudio;
    public Light flameLight;

    private float timer;
    private bool isActive = false;
    public Transform checkPoint;

    void Start()
    {
        if (flameEffect == null)
            flameEffect = GetComponentInChildren<ParticleSystem>();
        if (flameTrigger == null)
            flameTrigger = GetComponent<Collider>();
        if (flameAudio == null)
            flameAudio = GetComponentInChildren<AudioSource>();
        if (flameLight == null)
            flameLight = GetComponentInChildren<Light>();

        flameEffect.Stop();
        flameTrigger.enabled = false;
        flameAudio.Stop();
        flameLight.enabled = false;

        timer = inactiveTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (!isActive && timer <= 0f)
        {
            // Ateþi baþlat
            flameEffect.Play();
            flameTrigger.enabled = true;
            if (flameAudio) flameAudio.Play();
            if (flameLight) flameLight.enabled = true;

            isActive = true;
            timer = activeTime;
        }
        else if (isActive && timer <= 0f)
        {
            // Ateþi durdur
            flameEffect.Stop();
            flameTrigger.enabled = false;
            if (flameAudio) flameAudio.Stop();
            if (flameLight) flameLight.enabled = false;

            isActive = false;
            timer = inactiveTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            other.GetComponent<Player3DController>().RespawnOnCheckpoint(checkPoint);
        }
    }
}
