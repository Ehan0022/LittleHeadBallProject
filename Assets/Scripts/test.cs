using UnityEngine;

public class test : MonoBehaviour
{
    public Vector2 areaCenter; // Dikd�rtgen alan�n merkezi
    public Vector2 areaSize;   // Dikd�rtgen alan�n boyutu
    public LayerMask layerMask; // Kontrol edilecek layerlar

    void Update()
    {
        bool isObjectInArea = IsObjectInArea();
        if (isObjectInArea)
        {
            Debug.Log("Objenin alan i�inde!");
        }
        else
        {
            Debug.Log("Objenin alan d���nda.");
        }
    }

    bool IsObjectInArea()
    {
        Rect rect = new Rect(areaCenter - areaSize / 2, areaSize);
        RaycastHit2D hit = Physics2D.BoxCast(areaCenter, areaSize, 0f, Vector2.zero, 0f, layerMask);

        // Raycast sonucu kontrol et ve ���n� �iz
        if (hit.collider != null)
        {
            // I��n� �iz
            Debug.DrawRay(hit.point, Vector2.up * 0.1f, Color.red, 1f);
            Debug.DrawRay(hit.point, Vector2.right * 0.1f, Color.red, 1f);

            // �arp��ma noktas�n�n dikd�rtgen i�inde olup olmad���n� kontrol et
            if (rect.Contains(hit.point))
            {
                return true;
            }
        }
        return false;
    }

    
    private void OnDrawGizmos()
    {
        // Gizmos ile dikd�rtgen alan� �iz
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}

