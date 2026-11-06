using System;
using System.Collections;
using Invector;
using Invector.vCharacterController;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E6 RID: 230
[vClassHeader("Parachute Controller", true, "icon_v2", false, "")]
public class vParachuteController : vMonoBehaviour
{
	// Token: 0x06000920 RID: 2336 RVA: 0x0004242B File Offset: 0x0004062B
	private IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		this.tpInput = base.GetComponentInParent<vThirdPersonInput>();
		if (this.tpInput)
		{
			this.tpInput.onUpdate -= this.UpdateParachute;
			this.tpInput.onUpdate += this.UpdateParachute;
			this.tpInput.onFixedUpdate -= this.FixedUpdateParachute;
			this.tpInput.onFixedUpdate += this.FixedUpdateParachute;
			this.originalFreeSpeedRotation = this.tpInput.cc.freeSpeed.rotationSpeed;
			this.startPivotPosition = this.parachutePivot.transform.localPosition;
			this.startPivotEuler = this.parachutePivot.transform.localEulerAngles;
			this.parachuteTilt.transform.localPosition = Vector3.zero;
			this.parachuteTilt.transform.localEulerAngles = Vector3.zero;
			this.parachuteTilt.SetActive(false);
			this.parachutePivot.centerOfMass = this.parachutePivot.transform.InverseTransformPoint(this.tpInput.transform.position);
		}
		this._startAnimationHash = Animator.StringToHash(this.anim_OpenParachute);
		if (this.canUseParachute)
		{
			this.onCanUseParachuteEnable.Invoke();
		}
		else
		{
			this.onCanUseParachuteDisable.Invoke();
		}
		yield break;
	}

	// Token: 0x06000921 RID: 2337 RVA: 0x0004243A File Offset: 0x0004063A
	public virtual void OpenParachute()
	{
		if (this.openParachuteConditions)
		{
			base.StartCoroutine(this.OpenParachuteRoutine());
		}
	}

	// Token: 0x17000201 RID: 513
	// (get) Token: 0x06000922 RID: 2338 RVA: 0x00042454 File Offset: 0x00040654
	public bool openParachuteConditions
	{
		get
		{
			return this.canUseParachute && !this.usingParachute && !this.tpInput.cc.ragdolled && !this.tpInput.cc.isRolling && !this.tpInput.cc.customAction && this.tpInput.cc.groundDistance > this.minHeightToOpenParachute && this.timerToReOpen < Time.time;
		}
	}

	// Token: 0x06000923 RID: 2339 RVA: 0x000424CE File Offset: 0x000406CE
	public virtual void ExitParachuteImmedite()
	{
		if (!this.usingParachute)
		{
			return;
		}
		this.actionStateToExit = 0;
		this.CloseParachute();
	}

	// Token: 0x06000924 RID: 2340 RVA: 0x000424E6 File Offset: 0x000406E6
	public virtual void ExitParachute()
	{
		if (!this.usingParachute)
		{
			return;
		}
		this.actionStateToExit = 1;
		this.CloseParachute();
	}

	// Token: 0x17000202 RID: 514
	// (get) Token: 0x06000925 RID: 2341 RVA: 0x000424FE File Offset: 0x000406FE
	// (set) Token: 0x06000926 RID: 2342 RVA: 0x00042506 File Offset: 0x00040706
	public bool canUseParachute
	{
		get
		{
			return this._canUseParachute;
		}
		set
		{
			if (this._canUseParachute != value)
			{
				if (value)
				{
					this.onCanUseParachuteEnable.Invoke();
				}
				else
				{
					this.onCanUseParachuteDisable.Invoke();
				}
				this._canUseParachute = value;
			}
		}
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x00042534 File Offset: 0x00040734
	protected virtual void UpdateParachute()
	{
		if (this.openCloseParachute.GetButtonDown())
		{
			if (this.openParachuteConditions)
			{
				this.OpenParachute();
			}
			else if (this.usingParachute)
			{
				this.ExitParachuteImmedite();
			}
		}
		if (this.usingParachute && this.tpInput.cc.ragdolled)
		{
			this.ExitParachuteImmedite();
		}
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x0004258C File Offset: 0x0004078C
	protected virtual void FixedUpdateParachute()
	{
		if (this.canMove)
		{
			float axis = this.tpInput.horizontalInput.GetAxis();
			float axis2 = this.tpInput.verticalInput.GetAxis();
			Vector3 vector = new Vector3(axis, 0f, axis2);
			Vector3 vector2 = vector;
			this.tpInput.cameraMain.transform.TransformDirection(vector);
			this.tpInput.animator.SetFloat("InputHorizontal", vector.x, (vector.magnitude > 0.1f) ? 1f : 2f, Time.deltaTime);
			this.tpInput.animator.SetFloat("InputVertical", vector.z, (vector.magnitude > 0.1f) ? 1f : 2f, Time.deltaTime);
			this.parachutePivot.transform.forward.y = 0f;
			this.currentHorizontalSpeed = Mathf.Lerp(this.currentHorizontalSpeed, Mathf.Lerp(this.speedPitchBack, this.speedPitchForward, vector.z), this.acceleration * Time.deltaTime);
			this.horizontalForce = this.parachutePivot.transform.forward.normalized * this.currentHorizontalSpeed;
			this.currentDrag = Mathf.Lerp(this.dragPitchBack, this.dragPitchForward, vector.z) * ((vector.z < 0f) ? Mathf.Lerp(1f, -this.parachutePivot.velocity.y, -vector.z) : 1f);
			this.parachutePivot.AddForceAtPosition(Vector3.up * this.currentDrag, base.transform.position);
			this.tpInput.cc.heightReached = this.tpInput.transform.position.y;
			this.tpInput.cc.verticalVelocity = this.parachutePivot.velocity.y;
			float num = this.rollAngle * -vector2.x;
			float num2 = ((vector2.z > 0f) ? (this.pitchAngle.y * vector2.z) : (this.pitchAngle.x * -vector2.z));
			this.forwardBackTiltRotation = Quaternion.Lerp(this.forwardBackTiltRotation, Quaternion.AngleAxis(num2, Vector3.right), this.pitchSpeed * Time.deltaTime);
			this.sidesTiltRotation = Quaternion.Lerp(this.sidesTiltRotation, Quaternion.AngleAxis(num, Vector3.forward), this.rollSpeed * Time.deltaTime);
			this.parachuteTilt.transform.localRotation = this.sidesTiltRotation * this.forwardBackTiltRotation;
			this.parachutePivot.transform.Rotate(Vector3.up, vector2.x * this.rotationYSpeed * Time.deltaTime);
			if (this.autoAlignCamera)
			{
				if ((double)vector.magnitude > 0.5 && Mathf.Abs(this.tpInput.rotateCameraXInput.GetAxis()) < 0.5f && Mathf.Abs(this.tpInput.rotateCameraYInput.GetAxis()) < 0.5f)
				{
					if (this.autoAlignWeight < 1f)
					{
						this.autoAlignWeight += Time.deltaTime * this.autoAlignSpeed;
					}
					this.tpInput.tpCamera.mouseY = Mathf.LerpAngle(this.tpInput.tpCamera.mouseY, this.parachuteTilt.transform.eulerAngles.NormalizeAngle().x, this.autoAlignWeight);
					this.tpInput.tpCamera.mouseX = Mathf.LerpAngle(this.tpInput.tpCamera.mouseX, this.parachutePivot.transform.eulerAngles.NormalizeAngle().y, this.autoAlignWeight);
				}
				else
				{
					this.autoAlignWeight = 0f;
				}
			}
			if (!this.inForwardCollidion)
			{
				Vector3 velocity = this.parachutePivot.velocity;
				velocity.Set(this.horizontalForce.x, velocity.y, this.horizontalForce.z);
				this.parachutePivot.velocity = velocity;
				return;
			}
			this.currentHorizontalSpeed = 0f;
		}
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x000429EB File Offset: 0x00040BEB
	protected virtual IEnumerator OpenParachuteRoutine()
	{
		this.onStartOpen.Invoke();
		this.usingParachute = true;
		this.parachuteTilt.transform.localPosition = Vector3.zero;
		this.parachuteTilt.transform.localEulerAngles = Vector3.zero;
		this.forwardBackTiltRotation = Quaternion.identity;
		this.sidesTiltRotation = Quaternion.identity;
		this.tpInput.cc.heightReached = base.transform.position.y;
		this.tpInput.cc.disableCheckGround = true;
		this.tpInput.SetLockAllInput(true);
		this.tpInput.cc.isCrouching = false;
		this.tpInput.cc.lockMovement = true;
		this.tpInput.cc.lockRotation = true;
		this.tpInput.animator.SetInteger("ActionState", 1);
		this.tpInput.animator.CrossFadeInFixedTime(this._startAnimationHash, 0.25f);
		Vector3 vel = this.tpInput.cc._rigidbody.velocity;
		while (!this.tpInput.cc.baseLayerInfo.IsName(this.parachuteStateName))
		{
			yield return null;
		}
		this.onOpen.Invoke();
		vel.y = 0f;
		this.horizontalForce = vel;
		this.currentDrag = 0f;
		this.currentHorizontalSpeed = 0f;
		this.tpInput.cc._rigidbody.isKinematic = true;
		this.parachutePivot.transform.parent = null;
		this.parachuteTilt.SetActive(true);
		this.tpInput.transform.parent = this.parachuteTilt.transform;
		this.tpInput.transform.localEulerAngles = Vector3.zero;
		this.tpInput.transform.localPosition = new Vector3(0f, this.tpInput.transform.localPosition.y, 0f);
		this.parachutePivot.isKinematic = false;
		this.tpInput.cc.disableAnimations = true;
		this.tpInput.cc.lockSetMoveSpeed = true;
		this.tpInput.cc.moveSpeed = this.speedPitchBack;
		this.tpInput.cc.freeSpeed.rotationSpeed = this.rotationYSpeed;
		this.tpInput.cc._rigidbody.useGravity = false;
		this.tpInput.cc._capsuleCollider.isTrigger = false;
		this.canMove = true;
		yield return new WaitForEndOfFrame();
		if (this.cameraTarget)
		{
			this.tpInput.tpCamera.SetTarget(this.cameraTarget);
		}
		if (!string.IsNullOrEmpty(this.cameraState))
		{
			this.tpInput.ChangeCameraState(this.cameraState, true);
		}
		yield break;
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x000429FC File Offset: 0x00040BFC
	protected virtual void CloseParachute()
	{
		if (!this.usingParachute)
		{
			return;
		}
		this.timerToReOpen = Time.time + this.minTimeToReOpen;
		this.usingParachute = false;
		this.canMove = false;
		this.tpInput.transform.parent = null;
		this.parachuteTilt.SetActive(false);
		this.parachutePivot.isKinematic = true;
		this.parachutePivot.transform.parent = this.tpInput.transform;
		this.parachutePivot.transform.localPosition = this.startPivotPosition;
		this.parachutePivot.transform.localEulerAngles = this.startPivotEuler;
		this.parachuteTilt.transform.localPosition = Vector3.zero;
		this.parachuteTilt.transform.localEulerAngles = Vector3.zero;
		Vector3 forward = this.tpInput.transform.forward;
		forward.y = 0f;
		this.tpInput.transform.rotation = Quaternion.LookRotation(forward);
		this.tpInput.cc.disableAnimations = false;
		this.tpInput.cc.lockSetMoveSpeed = false;
		if (this.actionStateToExit == 1)
		{
			this.tpInput.animator.CrossFadeInFixedTime(this.anim_ExitParachute, 0.1f);
		}
		this.tpInput.animator.SetInteger("ActionState", 0);
		this.tpInput.cc._rigidbody.drag = 0f;
		this.tpInput.cc.disableCheckGround = false;
		if (this.actionStateToExit != 1)
		{
			this.tpInput.cc.isGrounded = false;
			this.tpInput.cc.animator.SetBool("IsGrounded", false);
		}
		if (!this.tpInput.cc.ragdolled)
		{
			this.tpInput.cc.verticalVelocity = 0f;
			this.tpInput.cc._rigidbody.isKinematic = false;
			this.tpInput.cc._rigidbody.useGravity = true;
		}
		this.tpInput.cc.freeSpeed.rotationSpeed = this.originalFreeSpeedRotation;
		this.tpInput.cc.lockMovement = false;
		this.tpInput.cc.lockRotation = false;
		this.tpInput.ResetCameraState();
		this.tpInput.SetLockAllInput(false);
		this.tpInput.tpCamera.SetTarget(this.tpInput.transform);
		this.onClose.Invoke();
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00042C88 File Offset: 0x00040E88
	protected void OnCollisionEnter(Collision collision)
	{
		this.CheckCollision(collision);
	}

	// Token: 0x0600092C RID: 2348 RVA: 0x00042C88 File Offset: 0x00040E88
	protected void OnCollisionStay(Collision collision)
	{
		this.CheckCollision(collision);
	}

	// Token: 0x0600092D RID: 2349 RVA: 0x00042C94 File Offset: 0x00040E94
	protected virtual void CheckCollision(Collision collision)
	{
		if (!this.usingParachute)
		{
			return;
		}
		if (Vector3.Angle(Vector3.up, collision.contacts[0].normal) < this.tpInput.cc.slopeLimit)
		{
			this.ExitParachute();
		}
		if (Vector3.Angle(-base.transform.forward, collision.contacts[0].normal) < 45f)
		{
			this.inForwardCollidion = true;
			return;
		}
		this.inForwardCollidion = false;
	}

	// Token: 0x0600092E RID: 2350 RVA: 0x00042D19 File Offset: 0x00040F19
	protected void OnCollisionExit(Collision collision)
	{
		this.inForwardCollidion = false;
	}

	// Token: 0x04000838 RID: 2104
	[vEditorToolbar("Input/Movement", false, "", false, false)]
	[vReadOnly(true)]
	public bool canMove;

	// Token: 0x04000839 RID: 2105
	[vReadOnly(true)]
	public bool inForwardCollidion;

	// Token: 0x0400083A RID: 2106
	[vReadOnly(true)]
	public bool usingParachute;

	// Token: 0x0400083B RID: 2107
	[vReadOnly(true)]
	[SerializeField]
	protected float currentDrag;

	// Token: 0x0400083C RID: 2108
	[SerializeField]
	protected bool _canUseParachute = true;

	// Token: 0x0400083D RID: 2109
	public Rigidbody parachutePivot;

	// Token: 0x0400083E RID: 2110
	public GameObject parachuteTilt;

	// Token: 0x0400083F RID: 2111
	public GenericInput openCloseParachute = new GenericInput("Space", "A", "A");

	// Token: 0x04000840 RID: 2112
	[Range(2f, 100f)]
	public float minHeightToOpenParachute = 2f;

	// Token: 0x04000841 RID: 2113
	[Range(0.5f, 5f)]
	public float minTimeToReOpen = 0.5f;

	// Token: 0x04000842 RID: 2114
	[Header("Vertical Movement")]
	[Range(1f, 10f)]
	public float dragPitchBack = 9f;

	// Token: 0x04000843 RID: 2115
	[Range(1f, 10f)]
	public float dragPitchForward = 5f;

	// Token: 0x04000844 RID: 2116
	[Header("Horizontal Movement")]
	public float acceleration = 2f;

	// Token: 0x04000845 RID: 2117
	public float speedPitchBack = 20f;

	// Token: 0x04000846 RID: 2118
	public float speedPitchForward = 10f;

	// Token: 0x04000847 RID: 2119
	[vHelpBox("Roll(z) uses Horizontal Input, Pitch uses Vertical Input (Back (-1) and Forward (1))", vHelpBoxAttribute.MessageType.None)]
	[Header("Roll(Z)/Pitch(X) Rotation")]
	public float rollSpeed = 2f;

	// Token: 0x04000848 RID: 2120
	public float rollAngle = 35f;

	// Token: 0x04000849 RID: 2121
	public float pitchSpeed = 1f;

	// Token: 0x0400084A RID: 2122
	[vMinMax(minLimit = -45f, maxLimit = 80f)]
	public Vector2 pitchAngle = new Vector2(-20f, 60f);

	// Token: 0x0400084B RID: 2123
	[Header("Yaw(Y) Rotation")]
	public float rotationYSpeed = 60f;

	// Token: 0x0400084C RID: 2124
	[vEditorToolbar("Camera", false, "", false, false)]
	[Header("Camera")]
	public string cameraState = "Parachute";

	// Token: 0x0400084D RID: 2125
	public Transform cameraTarget;

	// Token: 0x0400084E RID: 2126
	public bool autoAlignCamera = true;

	// Token: 0x0400084F RID: 2127
	public float autoAlignSpeed = 0.1f;

	// Token: 0x04000850 RID: 2128
	[vEditorToolbar("Animator", false, "", false, false)]
	[Tooltip("Used to check when is in parachute state")]
	public string parachuteStateName = "Parachute";

	// Token: 0x04000851 RID: 2129
	[Tooltip("Used to play the first animation of the parachute state")]
	public string anim_OpenParachute = "OpenParachute";

	// Token: 0x04000852 RID: 2130
	[Tooltip("Used to play the first animation of the parachute state")]
	public string anim_ExitParachute = "ExitParachute";

	// Token: 0x04000853 RID: 2131
	[vEditorToolbar("Events", false, "", false, false)]
	public UnityEvent onCanUseParachuteEnable;

	// Token: 0x04000854 RID: 2132
	[vEditorToolbar("Events", false, "", false, false)]
	public UnityEvent onCanUseParachuteDisable;

	// Token: 0x04000855 RID: 2133
	public UnityEvent onStartOpen;

	// Token: 0x04000856 RID: 2134
	public UnityEvent onOpen;

	// Token: 0x04000857 RID: 2135
	public UnityEvent onClose;

	// Token: 0x04000858 RID: 2136
	protected Quaternion forwardBackTiltRotation;

	// Token: 0x04000859 RID: 2137
	protected Quaternion sidesTiltRotation;

	// Token: 0x0400085A RID: 2138
	protected Vector3 startPivotPosition;

	// Token: 0x0400085B RID: 2139
	protected Vector3 startPivotEuler;

	// Token: 0x0400085C RID: 2140
	protected Vector3 horizontalForce;

	// Token: 0x0400085D RID: 2141
	protected float currentHorizontalSpeed;

	// Token: 0x0400085E RID: 2142
	protected float originalFreeSpeedRotation;

	// Token: 0x0400085F RID: 2143
	protected int actionStateToExit;

	// Token: 0x04000860 RID: 2144
	protected vThirdPersonInput tpInput;

	// Token: 0x04000861 RID: 2145
	protected float autoAlignWeight;

	// Token: 0x04000862 RID: 2146
	protected float timerToReOpen;

	// Token: 0x04000863 RID: 2147
	protected int _startAnimationHash;

	// Token: 0x04000864 RID: 2148
	private const int actState_default = 1;

	// Token: 0x04000865 RID: 2149
	private const int actState_exitOnGround = 1;

	// Token: 0x04000866 RID: 2150
	private const int actState_exitImmediate = 0;
}
