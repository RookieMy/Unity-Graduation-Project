using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Transform door; // Kap� objesi
    public Vector3 openPosition; // Kap�n�n a��k pozisyonu
    private Vector3 closedPosition; // Kap�n�n kapal� pozisyonu

    public float speed = 2f; // A��lma/Kapanma h�z�
    public bool isOpen = false; // Kap�n�n durumu

    void Start()
    {
        // Kap�n�n ba�lang�� pozisyonunu kaydet
        closedPosition = door.localPosition;
    }

    public void ToggleDoor()
    {
        if (!isOpen)
        {
            // Kap�n�n a��k m� kapal� m� oldu�unu kontrol et ve durumu de�i�tir
            isOpen = !isOpen;
            StopAllCoroutines(); // Eski bir hareket varsa durdur
            if (isOpen)
            {
                StartCoroutine(SmoothMove(door.localPosition, openPosition)); // Kap�y� a�
            }
            else
            {
                StartCoroutine(SmoothMove(door.localPosition, closedPosition)); // Kap�y� kapat
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
