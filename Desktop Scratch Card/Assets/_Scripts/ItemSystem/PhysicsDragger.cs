using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PhysicsDragger : MonoBehaviour
{
    [SerializeField] private float forceMultiplier = 500f;
    [SerializeField] private float maxForce = 1000f;
    [SerializeField] private float dampingFactor = 4f;
    
    private Rigidbody2D rb;
    private Camera mainCamera;
    private Vector2 targetPosition;
    private Vector2 grabOffset;    // 抓取点相对物体中心的偏移
    private bool isDragging = false;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        rb.constraints = RigidbodyConstraints2D.None;

        // 计算抓取点相对物体中心的偏移
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        grabOffset = mousePos - (Vector2)rb.position;
        
        // 将偏移转换为物体本地坐标系
        float angle = -rb.rotation * Mathf.Deg2Rad;  // 转换为弧度
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        grabOffset = new Vector2(
            grabOffset.x * cos - grabOffset.y * sin,
            grabOffset.x * sin + grabOffset.y * cos
        );
    }

    private void OnMouseDrag()
    {
        targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        isDragging = false;
    }

    private void FixedUpdate()
    {
        if (!isDragging) return;

        // 计算力的作用点（在世界空间中）
        float angle = rb.rotation * Mathf.Deg2Rad;  // 转换为弧度
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        Vector2 rotatedOffset = new Vector2(
            grabOffset.x * cos - grabOffset.y * sin,
            grabOffset.x * sin + grabOffset.y * cos
        );
        Vector2 grabPoint = (Vector2)rb.position + rotatedOffset;
        
        // 计算目标位置和实际抓取点的差异
        Vector2 difference = targetPosition - grabPoint;
        
        // 计算期望速度
        Vector2 desiredVelocity = difference * forceMultiplier * Time.fixedDeltaTime;
        
        // 计算抓取点的实际速度
        Vector2 grabPointVelocity = rb.GetPointVelocity(grabPoint);
        
        // 计算速度差
        Vector2 velocityDiff = desiredVelocity - grabPointVelocity;
        
        // 应用阻尼力
        Vector2 force = velocityDiff * dampingFactor;
        force = Vector2.ClampMagnitude(force, maxForce);
        
        // 在抓取点施加力
        rb.AddForceAtPosition(force * rb.mass, grabPoint);

        // Debug可视化
        Debug.DrawLine(grabPoint, grabPoint + force.normalized, Color.red);
    }
}