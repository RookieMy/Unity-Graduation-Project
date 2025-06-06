using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Transform door; // Kapý objesi
    public Vector3 openPosition; // Kapýnýn açýk pozisyonu
    private Vector3 closedPosition; // Kapýnýn kapalý pozisyonu

    public float speed = 2f; // Açýlma/Kapanma hýzý
    public bool isOpen = false; // Kapýnýn durumu

    void Start()
    {
        // Kapýnýn baþlangýç pozisyonunu kaydet
        closedPosition = door.localPosition;
    }

    public void ToggleDoor()
    {
        if (!isOpen)
        {
            // Kapýnýn açýk mý kapalý mý olduðunu kontrol et ve durumu deðiþtir
            isOpen = !isOpen;
            StopAllCoroutines(); // Eski bir hareket varsa durdur
            if (isOpen)
            {
                StartCoroutine(SmoothMove(door.localPosition, openPosition)); // Kapýyý aç
            }
            else
            {
                StartCoroutine(SmoothMove(door.localPosition, closedPosition)); // Kapýyý kapat
            }
        }
    }

    private System.Collections.IEnumerator SmoothMove(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;
        while (elapsedTime < speed)
        {
            door.localPosition = Vector3.Lerp(start, end, elapsedTime / speed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        door.localPosition = end; // Pozisyonu tam olarak yerine oturt
    }
}
