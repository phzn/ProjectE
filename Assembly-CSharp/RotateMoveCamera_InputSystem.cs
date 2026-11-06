using System;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000149 RID: 329
[RequireComponent(typeof(Camera))]
public class RotateMoveCamera_InputSystem : MonoBehaviour
{
	// Token: 0x06000A70 RID: 2672 RVA: 0x00047528 File Offset: 0x00045728
	private void Awake()
	{
		this.cam = base.GetComponent<Camera>();
		this.moveAction = this.inputActions.FindActionMap("Player", false).FindAction("Move", false);
		this.lookAction = this.inputActions.FindActionMap("Player", false).FindAction("Look", false);
		this.moveAction.Enable();
		this.lookAction.Enable();
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0004759B File Offset: 0x0004579B
	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x000475A9 File Offset: 0x000457A9
	private void Update()
	{
		this.Rotate();
		this.Move();
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x000475B8 File Offset: 0x000457B8
	private void Rotate()
	{
		this.lookInput = this.lookAction.ReadValue<Vector2>();
		this.rotationX += this.lookInput.x * this.sensitivityX * Time.deltaTime;
		this.rotationY += this.lookInput.y * this.sensitivityY * Time.deltaTime;
		this.rotationY = Mathf.Clamp(this.rotationY, this.minY, this.maxY);
		base.transform.localEulerAngles = new Vector3(-this.rotationY, this.rotationX, 0f);
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x00047660 File Offset: 0x00045860
	private void Move()
	{
		this.moveInput = this.moveAction.ReadValue<Vector2>();
		Vector3 vector = base.transform.forward * this.moveInput.y + base.transform.right * this.moveInput.x;
		vector *= this.moveSpeed * Time.deltaTime;
		base.transform.position += vector;
	}

	// Token: 0x040009AB RID: 2475
	[Header("Rotación")]
	public float minX = -360f;

	// Token: 0x040009AC RID: 2476
	public float maxX = 360f;

	// Token: 0x040009AD RID: 2477
	public float minY = -89f;

	// Token: 0x040009AE RID: 2478
	public float maxY = 89f;

	// Token: 0x040009AF RID: 2479
	public float sensitivityX = 100f;

	// Token: 0x040009B0 RID: 2480
	public float sensitivityY = 100f;

	// Token: 0x040009B1 RID: 2481
	[Header("Movimiento")]
	public float moveSpeed = 5f;

	// Token: 0x040009B2 RID: 2482
	private float rotationY;

	// Token: 0x040009B3 RID: 2483
	private float rotationX;

	// Token: 0x040009B4 RID: 2484
	private Vector2 moveInput;

	// Token: 0x040009B5 RID: 2485
	private Vector2 lookInput;

	// Token: 0x040009B6 RID: 2486
	private Camera cam;

	// Token: 0x040009B7 RID: 2487
	[SerializeField]
	private InputActionAsset inputActions;

	// Token: 0x040009B8 RID: 2488
	private InputAction moveAction;

	// Token: 0x040009B9 RID: 2489
	private InputAction lookAction;
}
