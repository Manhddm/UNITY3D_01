
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    [Header("Movement data")]
    public float moveSpeed = 2f;
    public float rotateSpeed = 1f;
    
    private float _verticalInput;
    private float _horizontalInput;
    [FormerlySerializedAs("_gunPoint")]
    [Header(("Gun data"))] 
    [SerializeField]private Transform gunPoint;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [Header("Tower data")]
    public GameObject tankTower;
    public float towerRotation;
    [Header("Aim data")]
    public LayerMask whatIsAim;
    public Transform aimTransform;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateAim();
        CheckInput();
    }

    private void CheckInput()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
        _verticalInput = Input.GetAxis("Vertical");
        if (_verticalInput >= 0)
        {
            _horizontalInput = Input.GetAxis("Horizontal");
        }
        else
        {
            _horizontalInput = -Input.GetAxis("Horizontal");
        }
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyBodyRotation();
        ApplyTowerRotation();
        
       
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = gunPoint.forward * bulletSpeed;
        Destroy(bullet, 7f);
    }

    private void UpdateAim()
    {
        if (Camera.main != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsAim))
            {
                float fixedY = aimTransform.position.y;
                aimTransform.position = new Vector3(hit.point.x, fixedY, hit.point.z);  
          
            }
        }
    }

    private void ApplyMovement()
    {
        Vector3 movemenet = transform.forward * (moveSpeed * _verticalInput);
        rb.velocity = movemenet;
    }

    private void ApplyBodyRotation()
    {
        transform.Rotate(0, _horizontalInput * rotateSpeed, 0);
    }

    private void ApplyTowerRotation()
    {
        Vector3 direction = aimTransform.position - transform.position;
        direction.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        tankTower.transform.rotation = Quaternion.RotateTowards(tankTower.transform.rotation, targetRotation, rotateSpeed);
    }
}
