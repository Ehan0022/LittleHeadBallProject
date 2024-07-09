using UnityEngine;

public class test : MonoBehaviour
{
    public Vector2 areaCenter; // Dikdörtgen alanýn merkezi
    public Vector2 areaSize;   // Dikdörtgen alanýn boyutu
    public LayerMask layerMask; // Kontrol edilecek layerlar

    void Update()
    {
        bool isObjectInArea = IsObjectInArea();
        if (isObjectInArea)
        {
            Debug.Log("Objenin alan içinde!");
        }
        else
        {
            Debug.Log("Objenin alan dýþýnda.");
        }
    }

    bool IsObjectInArea()
    {
        Rect rect = new Rect(areaCenter - areaSize / 2, areaSize);
        RaycastHit2D hit = Physics2D.BoxCast(areaCenter, areaSize, 0f, Vector2.zero, 0f, layerMask);

        // Raycast sonucu kontrol et ve ýþýný çiz
        if (hit.collider != null)
        {
            // Iþýný çiz
            Debug.DrawRay(hit.point, Vector2.up * 0.1f, Color.red, 1f);
            Debug.DrawRay(hit.point, Vector2.right * 0.1f, Color.red, 1f);

            // Çarpýþma noktasýnýn dikdörtgen içinde olup olmadýðýný kontrol et
            if (rect.Contains(hit.point))
            {
                return true;
            }
        }
        return false;
    }

    
    private void OnDrawGizmos()
    {
        // Gizmos ile dikdörtgen alaný çiz
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(areaCenter, areaSize);
    }
}

