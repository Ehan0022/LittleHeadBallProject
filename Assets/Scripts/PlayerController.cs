using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Animator animator;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 20f;
    public bool isGrounded = true;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] LayerMask ballLayerMask;
    [SerializeField] Transform foot;
    [SerializeField] Rigidbody2D ballRigidbody;
    [SerializeField] GameObject ball;
    
    [SerializeField] Transform ballDistancePoint;
    [SerializeField] Transform kickCenterPoint;

    public Vector2 areaCenterHead; // Dikdörtgen alanýn merkezi
    public Vector2 areaSizeHead;   // Dikdörtgen alanýn boyutu
    public Vector2 areaCenterFoot; // Dikdörtgen alanýn merkezi
    public Vector2 areaSizeFoot;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();     
    }

    public float forwardRaycastDistance = 0.65f;


    private float xPower;
    private float yPower;
    private float distance;

       // Dikdörtgen alanýn boyutu
    void Update()
    {
        HandleMovement();
        HandleHeadKicks();
        FechCentersForGizmos();
        /*Vector2 startPoint = foot.position + Vector3.up * 0.1f;
        Vector2 endPoint = startPoint + Vector2.right * forwardRaycastDistance;
        Debug.DrawLine(startPoint, endPoint, Color.green);*/

        RaycastHit2D ballHit = Physics2D.Raycast(foot.position + Vector3.up * 0.1f, Vector2.right, forwardRaycastDistance, ballLayerMask);

        if(ballHit.collider != null)
        {
            //Debug.Log("Vurulabilir");
            distance = Vector2.Distance(ballDistancePoint.position, ballHit.collider.gameObject.transform.position);
            //Debug.Log(Vector2.Distance(ballDistancePoint.position, ballHit.collider.gameObject.transform.position));
        }

        //min distance 0.74 max distance 1.01
        xPower = ((distance - 0.74f) / 0.001f) * 0.075f;
        yPower = ((1.0f - distance) / 0.001f) * 0.025f;
            

        /*if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {       
            animator.SetTrigger("Kick");

            if(ballHit.collider != null)
            {
                ballRigidbody.AddForce(new Vector2(xPower, yPower));
            }
        }*/


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            bool isObjectInArea = IsObjectInArea(headKickPoint, Color.green, areaSizeFoot);
            if (isObjectInArea)
            {
                animator.SetTrigger("Kick");
                ballRigidbody.AddForce(new Vector2(15f, 5f));
            }
            else
            {
                animator.SetTrigger("Kick");
            }
        }


    }

     
    int xDir = 0;

    
    private void HandleHeadKicks()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded)
        {
            bool isObjectInArea = IsObjectInArea(kickCenterPoint, Color.red, areaSizeHead);
            if (isObjectInArea)
            {
                animator.SetTrigger("HeadKick");
                HeadKick();
               // Debug.Log("Kafa vuruldu");
            }
            else
            {
                animator.SetTrigger("HeadKick");
               // Debug.Log("Vurulmadý");
            }
        }
    }



    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.D))
            xDir = 1;
        else if (Input.GetKey(KeyCode.A))
            xDir = -1;
        else
            xDir = 0;

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            isGrounded = false;
        }

        rigidbody.velocity = new Vector2(xDir * speed, rigidbody.velocity.y);

        IsGrounded();
    }

    RaycastHit2D floorHit;
    private float groundRaycastDistance = 0.75f;
    private void IsGrounded()
    {
        floorHit = Physics2D.Raycast(foot.position, -Vector2.up, groundRaycastDistance, groundLayerMask);
        if (floorHit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    [SerializeField] Transform headKickPoint;
    
    
    bool IsObjectInArea(Transform pointTransform, Color color, Vector2 areaSize)
    {
        Vector2 areaCenter = pointTransform.position;
        Rect rect = new Rect(areaCenter - areaSize / 2, areaSize);
        RaycastHit2D hit = Physics2D.BoxCast(areaCenter, areaSize, 0f, Vector2.zero, 0f, ballLayerMask);

        // Raycast sonucu kontrol et ve ýþýný çiz
        if (hit.collider != null)
        {
            // Iþýný çiz
            Debug.DrawRay(hit.point, Vector2.up * 0.1f, color, 1f);
            Debug.DrawRay(hit.point, Vector2.right * 0.1f, color, 1f);

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
        Gizmos.DrawWireCube(areaCenterHead, areaSizeHead);
        Gizmos.DrawWireCube(areaCenterFoot, areaSizeFoot);
    }

    private void FechCentersForGizmos()
    {
        areaCenterHead = headKickPoint.position;
        areaCenterFoot = kickCenterPoint.position;
    }

    [SerializeField] private float headKickPower;

    private void HeadKick()
    {
        float ballHeight = ball.transform.position.y;
        float fixedHeight = headKickPoint.position.y;
        if((ballHeight-fixedHeight) > 0f && (ballHeight - fixedHeight) < 0.11f)
        {
            ballRigidbody.AddForce(new Vector2(headKickPower, 1f));
            Debug.Log("Birinci yükseklikten vuruldu");
        }
        else if ((ballHeight - fixedHeight) >= 0.10f && (ballHeight - fixedHeight) < 0.20f)
        {
            ballRigidbody.AddForce(new Vector2(headKickPower, 2f));
            Debug.Log("Ýkinci yükseklikten vuruldu");
        }
        else if ((ballHeight - fixedHeight) >= 0.20f && (ballHeight - fixedHeight) < 0.30f)
        {
            ballRigidbody.AddForce(new Vector2(headKickPower, 3f));
            Debug.Log("Üçüncü yükseklikten vuruldu");
        }
        else if ((ballHeight - fixedHeight) >= 0.30f && (ballHeight - fixedHeight) < 0.45f)
        {
            ballRigidbody.AddForce(new Vector2(headKickPower, 4f));
            Debug.Log("Dördüncü yükseklikten vuruldu");
        }
        else
        {
            ballRigidbody.AddForce(new Vector2(headKickPower,0));
            Debug.Log("Alttan vuruldu");
        }

    }

    private void Kick()
    {

    }



}
