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
    
    [SerializeField] Transform ballDistancePoint;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }

    public float forwardRaycastDistance = 0.65f;


    private float xPower;
    private float yPower;
    private float distance;

    public Vector2 areaCenter; // Dikdörtgen alanýn merkezi
    public Vector2 areaSize;   // Dikdörtgen alanýn boyutu
    void Update()
    {
        HandleMovement();

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
            

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {       
            animator.SetTrigger("Kick");

            if(ballHit.collider != null)
            {
                ballRigidbody.AddForce(new Vector2(xPower, yPower));
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && !isGrounded)
        {
            bool isObjectInArea = IsObjectInArea();
            if (isObjectInArea)
            {
                animator.SetTrigger("HeadKick");
                HeadKick();
                Debug.Log("Kafa vuruldu");              
            }
            else
            {
                animator.SetTrigger("HeadKick");
                Debug.Log("Vurulmadý");
            }
        }

    }

     
    int xDir = 0;
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
    
    
    bool IsObjectInArea()
    {
        areaCenter = headKickPoint.position;
        Rect rect = new Rect(areaCenter - areaSize / 2, areaSize);
        RaycastHit2D hit = Physics2D.BoxCast(areaCenter, areaSize, 0f, Vector2.zero, 0f, ballLayerMask);

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

    [SerializeField] private float headKickPower;
    private void HeadKick()
    {
        ballRigidbody.AddForce(new Vector2(headKickPower, 0f));
    }



}
