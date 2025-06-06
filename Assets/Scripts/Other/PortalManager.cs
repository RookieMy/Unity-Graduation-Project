using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PortalManager : MonoBehaviour
{
    public Transform[] portals;

    public Animator timerTextPanel;
    public TextMeshProUGUI timerText;
    public Transform currentPortal;
    private bool isTeleporting;

    private void Update()
    {
        
        foreach(var portal in portals)
        {
            if (portal.parent.gameObject.activeSelf)
                currentPortal = portal;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isTeleporting)
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        if(currentPortal!=null)
        {
            player.position = currentPortal.position;
            player.rotation = currentPortal.rotation;
            player.transform.GetComponent<Player3DController>().OnTeleport();
            StartCoroutine(TeleportCooldown());

        }
    }

    private System.Collections.IEnumerator TeleportCooldown()
    {
        isTeleporting = true;
        yield return new WaitForSeconds(.5f);
        isTeleporting = false;
        
        
    }

    
}
