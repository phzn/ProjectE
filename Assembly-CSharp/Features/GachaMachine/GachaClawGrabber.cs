using System;
using UnityEngine;

namespace Features.GachaMachine
{
	// Token: 0x020011DC RID: 4572
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(32000)]
	public sealed class GachaClawGrabber : MonoBehaviour
	{
		// Token: 0x1700167B RID: 5755
		// (get) Token: 0x06008CBD RID: 36029 RVA: 0x003977E2 File Offset: 0x003959E2
		public bool IsHolding
		{
			get
			{
				return this._heldBody != null;
			}
		}

		// Token: 0x1700167C RID: 5756
		// (get) Token: 0x06008CBE RID: 36030 RVA: 0x003977F0 File Offset: 0x003959F0
		public Rigidbody HeldBody
		{
			get
			{
				return this._heldBody;
			}
		}

		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x06008CBF RID: 36031 RVA: 0x003977F8 File Offset: 0x003959F8
		public Vector3 ContactPointWorld
		{
			get
			{
				return this.ResolveContactPoint();
			}
		}

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x06008CC0 RID: 36032 RVA: 0x00397800 File Offset: 0x00395A00
		public bool HasCielSession
		{
			get
			{
				return this._cielRig != null && this._cielRig.IsActive;
			}
		}

		// Token: 0x06008CC1 RID: 36033 RVA: 0x0039781D File Offset: 0x00395A1D
		public bool IsSessionCielCollider(Collider candidate)
		{
			return this._cielRig != null && this._cielRig.IsRigCollider(candidate);
		}

		// Token: 0x06008CC2 RID: 36034 RVA: 0x0039783B File Offset: 0x00395A3B
		internal void Bind(GachaClawRig rig, Transform clawMotor)
		{
			this._rig = rig;
			this._clawMotor = clawMotor;
		}

		// Token: 0x06008CC3 RID: 36035 RVA: 0x0039784C File Offset: 0x00395A4C
		public void ConfigureSession(GachaMachineController controller, Transform playerRoot, Animator playerAnimator)
		{
			this.ClearSession();
			this._controller = controller;
			this._playerRoot = playerRoot;
			this._playerAnimator = playerAnimator;
			if (this._playerRoot == null)
			{
				return;
			}
			this._cielRig = GachaCielCollisionRig.GetOrCreate(this._playerRoot);
			if (this._cielRig == null)
			{
				return;
			}
			this._cielRig.BindMachineController(this._controller);
			if (!this._cielRig.Acquire(this, this._playerAnimator))
			{
				this._cielRig = null;
			}
		}

		// Token: 0x06008CC4 RID: 36036 RVA: 0x003978CF File Offset: 0x00395ACF
		public void ClearSession()
		{
			this.Release();
			if (this._cielRig != null)
			{
				this._cielRig.Release(this);
			}
			this._cielRig = null;
			this._controller = null;
			this._playerRoot = null;
			this._playerAnimator = null;
		}

		// Token: 0x06008CC5 RID: 36037 RVA: 0x0039790D File Offset: 0x00395B0D
		public void SetControlEnabled(bool enabled)
		{
			this._controlEnabled = enabled;
			if (!enabled)
			{
				this.Release();
			}
		}

		// Token: 0x06008CC6 RID: 36038 RVA: 0x0039791F File Offset: 0x00395B1F
		public void SetClosure01(float closure01)
		{
			this._closure = Mathf.Clamp01(closure01);
			if (this._closure <= this.releaseClosure)
			{
				this.Release();
			}
		}

		// Token: 0x06008CC7 RID: 36039 RVA: 0x00397941 File Offset: 0x00395B41
		public void SetGripBlocked(bool gripBlocked)
		{
			this._gripBlocked = gripBlocked;
		}

		// Token: 0x06008CC8 RID: 36040 RVA: 0x0039794C File Offset: 0x00395B4C
		private void LateUpdate()
		{
			if (!this._controlEnabled || this._clawMotor == null || this.IsHolding)
			{
				return;
			}
			if ((this._closure >= this.acquireClosure || (this._gripBlocked && this._closure >= this.releaseClosure)) && Time.unscaledTime >= this._nextAcquireTime)
			{
				this._nextAcquireTime = Time.unscaledTime + this.reacquireInterval;
				this.TryAcquireDynamicBody(this.ResolveContactPoint());
			}
		}

		// Token: 0x06008CC9 RID: 36041 RVA: 0x003979D4 File Offset: 0x00395BD4
		private void FixedUpdate()
		{
			if (this._heldBody == null && this._joint == null)
			{
				return;
			}
			if (this._joint == null)
			{
				this.Release();
				return;
			}
			if (this._jointAnchorBody == null)
			{
				return;
			}
			this._jointAnchorBody.MovePosition(this.ResolveContactPoint());
			this._jointAnchorBody.MoveRotation(this._clawMotor.rotation);
		}

		// Token: 0x06008CCA RID: 36042 RVA: 0x00397A4C File Offset: 0x00395C4C
		private bool TryAcquireDynamicBody(Vector3 contact)
		{
			int num = Physics.OverlapSphereNonAlloc(contact, this.acquisitionRadius, this._overlapBuffer, this.grabLayers, QueryTriggerInteraction.Ignore);
			Rigidbody rigidbody = null;
			Vector3 vector = contact;
			float num2 = float.PositiveInfinity;
			for (int i = 0; i < num; i++)
			{
				Collider collider = this._overlapBuffer[i];
				if (!(collider == null) && !collider.transform.IsChildOf(base.transform) && (!(this._cielRig != null) || !this._cielRig.IsRigCollider(collider)) && (!(this._playerRoot != null) || !collider.transform.IsChildOf(this._playerRoot)))
				{
					Rigidbody attachedRigidbody = collider.attachedRigidbody;
					if (!(attachedRigidbody == null) && !attachedRigidbody.isKinematic && attachedRigidbody.detectCollisions)
					{
						Vector3 vector2 = collider.ClosestPoint(contact);
						float sqrMagnitude = (vector2 - contact).sqrMagnitude;
						if (sqrMagnitude < num2)
						{
							num2 = sqrMagnitude;
							rigidbody = attachedRigidbody;
							vector = vector2;
						}
					}
				}
			}
			if (rigidbody == null)
			{
				return false;
			}
			this.CreateDynamicJoint(rigidbody, vector);
			return this.IsHolding;
		}

		// Token: 0x06008CCB RID: 36043 RVA: 0x00397B70 File Offset: 0x00395D70
		private void CreateDynamicJoint(Rigidbody body, Vector3 worldContact)
		{
			this.EnsureJointAnchor();
			if (this._jointAnchorBody == null)
			{
				return;
			}
			this._jointAnchorBody.position = this.ResolveContactPoint();
			this._jointAnchorBody.rotation = this._clawMotor.rotation;
			this._joint = this._jointAnchorBody.gameObject.AddComponent<FixedJoint>();
			this._joint.connectedBody = body;
			this._joint.autoConfigureConnectedAnchor = false;
			this._joint.anchor = Vector3.zero;
			this._joint.connectedAnchor = body.transform.InverseTransformPoint(worldContact);
			this._joint.enableCollision = false;
			this._joint.enablePreprocessing = true;
			this._joint.breakForce = this.jointBreakForce;
			this._joint.breakTorque = this.jointBreakTorque;
			this._heldBody = body;
			this.LogAcquired(body.name);
		}

		// Token: 0x06008CCC RID: 36044 RVA: 0x00397C5B File Offset: 0x00395E5B
		public void Release()
		{
			if (this._joint != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._joint);
				}
				else
				{
					Object.DestroyImmediate(this._joint);
				}
			}
			this._joint = null;
			this._heldBody = null;
		}

		// Token: 0x06008CCD RID: 36045 RVA: 0x00397C98 File Offset: 0x00395E98
		private void EnsureJointAnchor()
		{
			if (this._jointAnchorBody != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("__GachaClawJointAnchor")
			{
				hideFlags = HideFlags.DontSave,
				layer = base.gameObject.layer
			};
			this._jointAnchorBody = gameObject.AddComponent<Rigidbody>();
			this._jointAnchorBody.isKinematic = true;
			this._jointAnchorBody.useGravity = false;
			this._jointAnchorBody.detectCollisions = false;
			this._jointAnchorBody.interpolation = RigidbodyInterpolation.Interpolate;
			this._jointAnchorBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
		}

		// Token: 0x06008CCE RID: 36046 RVA: 0x00397D20 File Offset: 0x00395F20
		private Vector3 ResolveContactPoint()
		{
			if (!(this._clawMotor != null))
			{
				return base.transform.position;
			}
			return this._clawMotor.TransformPoint(this.contactCenterLocal);
		}

		// Token: 0x06008CCF RID: 36047 RVA: 0x00397D50 File Offset: 0x00395F50
		private void LogAcquired(string target)
		{
			if (this.debugLogs)
			{
				string text = "[GachaClawGrabber] {0} claw grabbed '{1}'.";
				GachaClawRig rig = this._rig;
				Debug.Log(string.Format(text, (rig != null) ? new GachaMachineSide?(rig.Side) : null, target), this);
			}
		}

		// Token: 0x06008CD0 RID: 36048 RVA: 0x00397D9A File Offset: 0x00395F9A
		private void OnJointBreak(float breakForce)
		{
			this.Release();
		}

		// Token: 0x06008CD1 RID: 36049 RVA: 0x00397DA2 File Offset: 0x00395FA2
		private void OnDisable()
		{
			this._controlEnabled = false;
			this.ClearSession();
		}

		// Token: 0x06008CD2 RID: 36050 RVA: 0x00397DB4 File Offset: 0x00395FB4
		private void OnDestroy()
		{
			this.ClearSession();
			if (this._jointAnchorBody == null)
			{
				return;
			}
			GameObject gameObject = this._jointAnchorBody.gameObject;
			this._jointAnchorBody = null;
			if (Application.isPlaying)
			{
				Object.Destroy(gameObject);
				return;
			}
			Object.DestroyImmediate(gameObject);
		}

		// Token: 0x06008CD3 RID: 36051 RVA: 0x00397E00 File Offset: 0x00396000
		private void OnValidate()
		{
			this.acquisitionRadius = Mathf.Max(0.01f, this.acquisitionRadius);
			this.acquireClosure = Mathf.Clamp01(this.acquireClosure);
			this.releaseClosure = Mathf.Clamp(this.releaseClosure, 0f, this.acquireClosure);
			this.reacquireInterval = Mathf.Max(0.02f, this.reacquireInterval);
			this.jointBreakForce = Mathf.Max(1f, this.jointBreakForce);
			this.jointBreakTorque = Mathf.Max(1f, this.jointBreakTorque);
		}

		// Token: 0x06008CD4 RID: 36052 RVA: 0x00397E94 File Offset: 0x00396094
		private void OnDrawGizmosSelected()
		{
			if (!this.drawAcquisitionVolume)
			{
				return;
			}
			Color color = Gizmos.color;
			Gizmos.color = (this.IsHolding ? new Color(0.3f, 1f, 0.45f, 0.9f) : new Color(1f, 0.75f, 0.15f, 0.8f));
			Gizmos.DrawWireSphere(this.ResolveContactPoint(), this.acquisitionRadius);
			Gizmos.color = color;
		}

		// Token: 0x04008FA7 RID: 36775
		[Header("Acquisition")]
		[Tooltip("Centre between the finger tips, in GMClawMotor local space (+Y is down).")]
		[SerializeField]
		private Vector3 contactCenterLocal = new Vector3(0f, 0.18f, 0f);

		// Token: 0x04008FA8 RID: 36776
		[SerializeField]
		[Min(0.01f)]
		private float acquisitionRadius = 0.15f;

		// Token: 0x04008FA9 RID: 36777
		[SerializeField]
		[Range(0f, 1f)]
		private float acquireClosure = 0.82f;

		// Token: 0x04008FAA RID: 36778
		[SerializeField]
		[Range(0f, 1f)]
		private float releaseClosure = 0.42f;

		// Token: 0x04008FAB RID: 36779
		[SerializeField]
		private LayerMask grabLayers = -1;

		// Token: 0x04008FAC RID: 36780
		[SerializeField]
		[Min(0.02f)]
		private float reacquireInterval = 0.08f;

		// Token: 0x04008FAD RID: 36781
		[Header("Dynamic Body Joint")]
		[Tooltip("High enough to tear a toy out of the packed 18-toy pile burying Ciel without dropping it.")]
		[SerializeField]
		[Min(1f)]
		private float jointBreakForce = 900f;

		// Token: 0x04008FAE RID: 36782
		[SerializeField]
		[Min(1f)]
		private float jointBreakTorque = 450f;

		// Token: 0x04008FAF RID: 36783
		[Header("Debug")]
		[SerializeField]
		private bool debugLogs;

		// Token: 0x04008FB0 RID: 36784
		[SerializeField]
		private bool drawAcquisitionVolume;

		// Token: 0x04008FB1 RID: 36785
		private readonly Collider[] _overlapBuffer = new Collider[48];

		// Token: 0x04008FB2 RID: 36786
		private GachaClawRig _rig;

		// Token: 0x04008FB3 RID: 36787
		private Transform _clawMotor;

		// Token: 0x04008FB4 RID: 36788
		private GachaMachineController _controller;

		// Token: 0x04008FB5 RID: 36789
		private Transform _playerRoot;

		// Token: 0x04008FB6 RID: 36790
		private Animator _playerAnimator;

		// Token: 0x04008FB7 RID: 36791
		private GachaCielCollisionRig _cielRig;

		// Token: 0x04008FB8 RID: 36792
		private Rigidbody _jointAnchorBody;

		// Token: 0x04008FB9 RID: 36793
		private FixedJoint _joint;

		// Token: 0x04008FBA RID: 36794
		private Rigidbody _heldBody;

		// Token: 0x04008FBB RID: 36795
		private bool _gripBlocked;

		// Token: 0x04008FBC RID: 36796
		private float _closure;

		// Token: 0x04008FBD RID: 36797
		private float _nextAcquireTime;

		// Token: 0x04008FBE RID: 36798
		private bool _controlEnabled;
	}
}
