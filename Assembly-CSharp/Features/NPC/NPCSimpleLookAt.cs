using System;
using Features.RenderingPrecision;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EEC RID: 3820
	[DefaultExecutionOrder(100)]
	public class NPCSimpleLookAt : MonoBehaviour
	{
		// Token: 0x06007257 RID: 29271 RVA: 0x002E8C83 File Offset: 0x002E6E83
		public void StartLookingAt(Vector3 worldPosition)
		{
			this.EnsureBonesInitialized();
			this._targetPosition = worldPosition;
			this._targetTransform = null;
			this._isLooking = true;
			bool flag = this.debugLog;
		}

		// Token: 0x06007258 RID: 29272 RVA: 0x002E8CA7 File Offset: 0x002E6EA7
		public void StartLookingAt(Transform target)
		{
			this.EnsureBonesInitialized();
			this._targetTransform = target;
			this._isLooking = true;
			bool flag = this.debugLog;
		}

		// Token: 0x06007259 RID: 29273 RVA: 0x002E8CC4 File Offset: 0x002E6EC4
		public void StopLooking()
		{
			this._isLooking = false;
			this._targetTransform = null;
			bool flag = this.debugLog;
		}

		// Token: 0x170012A7 RID: 4775
		// (get) Token: 0x0600725A RID: 29274 RVA: 0x002E8CDB File Offset: 0x002E6EDB
		public bool IsLooking
		{
			get
			{
				return this._isLooking;
			}
		}

		// Token: 0x170012A8 RID: 4776
		// (get) Token: 0x0600725B RID: 29275 RVA: 0x002E8CE3 File Offset: 0x002E6EE3
		// (set) Token: 0x0600725C RID: 29276 RVA: 0x002E8CEB File Offset: 0x002E6EEB
		internal NPCAmbientSuspension ProximitySuspension { get; set; }

		// Token: 0x0600725D RID: 29277 RVA: 0x002E8CF4 File Offset: 0x002E6EF4
		public bool IsLookingAt(Transform target)
		{
			return target != null && this._isLooking && this._targetTransform == target;
		}

		// Token: 0x0600725E RID: 29278 RVA: 0x002E8D15 File Offset: 0x002E6F15
		public void StopLookingAt(Transform expectedTarget)
		{
			if (this.IsLookingAt(expectedTarget))
			{
				this.StopLooking();
			}
		}

		// Token: 0x170012A9 RID: 4777
		// (get) Token: 0x0600725F RID: 29279 RVA: 0x002E8D28 File Offset: 0x002E6F28
		public bool IsNeutral
		{
			get
			{
				return !this._isLooking && this._currentHeadOffset.sqrMagnitude < 0.1f && this._currentNeckOffset.sqrMagnitude < 0.1f && this._currentSpineOffset.sqrMagnitude < 0.1f && this._currentLowerSpineOffset.sqrMagnitude < 0.1f;
			}
		}

		// Token: 0x06007260 RID: 29280 RVA: 0x002E8D87 File Offset: 0x002E6F87
		private void Awake()
		{
			WorldOrigin.Shifted += this.OnWorldOriginShifted;
			this._animator = base.GetComponentInChildren<Animator>();
			this._wander = base.GetComponent<NPCWanderController>();
			this.FindBones();
		}

		// Token: 0x06007261 RID: 29281 RVA: 0x000320C6 File Offset: 0x000302C6
		private void OnEnable()
		{
			NPCSimulationPause.RegisterParticipant(this);
		}

		// Token: 0x06007262 RID: 29282 RVA: 0x002E8DB8 File Offset: 0x002E6FB8
		private void OnDestroy()
		{
			WorldOrigin.Shifted -= this.OnWorldOriginShifted;
		}

		// Token: 0x06007263 RID: 29283 RVA: 0x002E8DCB File Offset: 0x002E6FCB
		private void OnWorldOriginShifted(Vector3 delta)
		{
			this._targetPosition += delta;
		}

		// Token: 0x06007264 RID: 29284 RVA: 0x002E8DDF File Offset: 0x002E6FDF
		private void Start()
		{
			this.CaptureRestPose();
		}

		// Token: 0x06007265 RID: 29285 RVA: 0x002E8DE8 File Offset: 0x002E6FE8
		private void LateUpdate()
		{
			if (this.ProximitySuspension != null && this.ProximitySuspension.IsSuspended)
			{
				return;
			}
			if (NPCSimulationPause.IsPaused)
			{
				return;
			}
			if (!this._hasRestPose)
			{
				return;
			}
			if (this._targetTransform != null)
			{
				if (this.manualPlayerTargetBone != null)
				{
					this._targetPosition = this.manualPlayerTargetBone.position;
				}
				else if (this._targetTransform.CompareTag(this.playerTag))
				{
					if (this._cachedPlayerHead == null)
					{
						Animator componentInChildren = this._targetTransform.GetComponentInChildren<Animator>();
						if (componentInChildren != null && componentInChildren.isHuman)
						{
							this._cachedPlayerHead = componentInChildren.GetBoneTransform(HumanBodyBones.Head);
							if (this._cachedPlayerHead == null)
							{
								this._cachedPlayerHead = componentInChildren.GetBoneTransform(HumanBodyBones.Neck);
							}
						}
						if (this._cachedPlayerHead == null)
						{
							this._cachedPlayerHead = this.FindDeepChild(this._targetTransform, "Head");
						}
					}
					if (this._cachedPlayerHead != null)
					{
						this._targetPosition = this._cachedPlayerHead.position;
					}
					else
					{
						this._targetPosition = this._targetTransform.position + this.targetOffset;
					}
				}
				else
				{
					this._targetPosition = this._targetTransform.position;
				}
			}
			Vector3 vector = this.CalculateLookDirection();
			float num = (this._isLooking ? this.rotationSpeed : this.returnSpeed);
			float simulationDeltaTime = NPCSimulationPause.SimulationDeltaTime;
			float num2 = 1f - Mathf.Exp(-this.smoothing * simulationDeltaTime);
			if (this._isLooking)
			{
				float num3 = (this.enableFullBodyRotation ? (this.spineWeight * 1.5f) : this.spineWeight);
				float num4 = (this.enableFullBodyRotation ? (this.lowerSpineWeight * 2f) : this.lowerSpineWeight);
				Vector3 vector2 = vector * this.headWeight;
				Vector3 vector3 = vector * this.neckWeight;
				Vector3 vector4 = vector * num3;
				Vector3 vector5 = vector * num4;
				this._currentHeadOffset = Vector3.Lerp(this._currentHeadOffset, vector2, num2);
				this._currentNeckOffset = Vector3.Lerp(this._currentNeckOffset, vector3, num2);
				this._currentSpineOffset = Vector3.Lerp(this._currentSpineOffset, vector4, num2);
				this._currentLowerSpineOffset = Vector3.Lerp(this._currentLowerSpineOffset, vector5, num2);
			}
			else
			{
				float num5 = 1f - Mathf.Exp(-this.smoothing * 1.5f * simulationDeltaTime);
				this._currentHeadOffset = Vector3.Lerp(this._currentHeadOffset, Vector3.zero, num5);
				this._currentNeckOffset = Vector3.Lerp(this._currentNeckOffset, Vector3.zero, num5);
				this._currentSpineOffset = Vector3.Lerp(this._currentSpineOffset, Vector3.zero, num5);
				this._currentLowerSpineOffset = Vector3.Lerp(this._currentLowerSpineOffset, Vector3.zero, num5);
				if (this._currentHeadOffset.sqrMagnitude < 0.01f)
				{
					this._currentHeadOffset = Vector3.zero;
				}
				if (this._currentNeckOffset.sqrMagnitude < 0.01f)
				{
					this._currentNeckOffset = Vector3.zero;
				}
				if (this._currentSpineOffset.sqrMagnitude < 0.01f)
				{
					this._currentSpineOffset = Vector3.zero;
				}
				if (this._currentLowerSpineOffset.sqrMagnitude < 0.01f)
				{
					this._currentLowerSpineOffset = Vector3.zero;
				}
			}
			this.ApplyRotations();
		}

		// Token: 0x06007266 RID: 29286 RVA: 0x002E9134 File Offset: 0x002E7334
		private Transform FindDeepChild(Transform parent, string name)
		{
			foreach (object obj in parent)
			{
				Transform transform = (Transform)obj;
				if (transform.name.Contains(name))
				{
					return transform;
				}
				Transform transform2 = this.FindDeepChild(transform, name);
				if (transform2 != null)
				{
					return transform2;
				}
			}
			return null;
		}

		// Token: 0x06007267 RID: 29287 RVA: 0x002E91B0 File Offset: 0x002E73B0
		private void FindBones()
		{
			if (this._animator == null || !this._animator.isHuman)
			{
				if (this.debugLog)
				{
					Debug.LogWarning("[LookAt] " + base.gameObject.name + ": No humanoid animator found!");
				}
				return;
			}
			if (this.headBone == null)
			{
				this.headBone = this._animator.GetBoneTransform(HumanBodyBones.Head);
			}
			if (this.neckBone == null)
			{
				this.neckBone = this._animator.GetBoneTransform(HumanBodyBones.Neck);
			}
			if (this.spineBone == null)
			{
				this.spineBone = this._animator.GetBoneTransform(HumanBodyBones.Chest);
				if (this.spineBone == null)
				{
					this.spineBone = this._animator.GetBoneTransform(HumanBodyBones.UpperChest);
				}
			}
			if (this.lowerSpineBone == null)
			{
				this.lowerSpineBone = this._animator.GetBoneTransform(HumanBodyBones.Spine);
			}
			bool flag = this.debugLog;
		}

		// Token: 0x06007268 RID: 29288 RVA: 0x002E92AC File Offset: 0x002E74AC
		private void CaptureRestPose()
		{
			if (this.headBone != null)
			{
				this._headRestRotation = this.headBone.localRotation;
			}
			if (this.neckBone != null)
			{
				this._neckRestRotation = this.neckBone.localRotation;
			}
			if (this.spineBone != null)
			{
				this._spineRestRotation = this.spineBone.localRotation;
			}
			if (this.lowerSpineBone != null)
			{
				this._lowerSpineRestRotation = this.lowerSpineBone.localRotation;
			}
			this._hasRestPose = this.headBone != null;
			if (this.debugLog)
			{
				bool hasRestPose = this._hasRestPose;
			}
		}

		// Token: 0x06007269 RID: 29289 RVA: 0x002E9358 File Offset: 0x002E7558
		public void ReInitializeAfterMerge()
		{
			this.headBone = null;
			this.neckBone = null;
			this.spineBone = null;
			this.lowerSpineBone = null;
			this._currentHeadOffset = Vector3.zero;
			this._currentNeckOffset = Vector3.zero;
			this._currentSpineOffset = Vector3.zero;
			this._currentLowerSpineOffset = Vector3.zero;
			this._animator = base.GetComponentInChildren<Animator>();
			if (this._animator != null)
			{
				this._animator.Rebind();
			}
			this.FindBones();
			this.CaptureRestPose();
			this._needsBoneRefresh = false;
		}

		// Token: 0x0600726A RID: 29290 RVA: 0x002E93E8 File Offset: 0x002E75E8
		private void EnsureBonesInitialized()
		{
			if (this._needsBoneRefresh || this.headBone == null)
			{
				this._animator = base.GetComponentInChildren<Animator>();
				if (this._animator == null)
				{
					Debug.LogWarning("[LookAt] " + base.gameObject.name + ": No animator found!");
					return;
				}
				this.FindBones();
				if (!this._hasRestPose)
				{
					this.CaptureRestPose();
				}
				this._needsBoneRefresh = false;
			}
		}

		// Token: 0x0600726B RID: 29291 RVA: 0x002E9460 File Offset: 0x002E7660
		private Vector3 CalculateLookDirection()
		{
			if (this.headBone == null)
			{
				return Vector3.zero;
			}
			Vector3 position = this.headBone.position;
			Vector3 vector = this._targetPosition - position;
			Vector3 vector2 = base.transform.InverseTransformDirection(vector.normalized);
			float num = Mathf.Atan2(vector2.x, vector2.z) * 57.29578f;
			float num2 = 0f;
			if (!this.horizontalOnly)
			{
				num2 = -Mathf.Asin(Mathf.Clamp(vector2.y, -1f, 1f)) * 57.29578f;
				num2 = Mathf.Clamp(num2, -this.maxVerticalAngle, this.maxVerticalAngle);
			}
			num = Mathf.Clamp(num, -this.maxHorizontalAngle, this.maxHorizontalAngle);
			return new Vector3(num2, num, 0f);
		}

		// Token: 0x0600726C RID: 29292 RVA: 0x002E9530 File Offset: 0x002E7730
		private void ApplyRotations()
		{
			bool flag = this._wander != null && this._wander.BenchOwnsBodyPose;
			if (!flag && this.lowerSpineBone != null && this.lowerSpineWeight > 0.01f)
			{
				Quaternion quaternion = this._lowerSpineRestRotation * Quaternion.Euler(this._currentLowerSpineOffset);
				this.lowerSpineBone.localRotation = quaternion;
			}
			if (!flag && this.spineBone != null && this.spineWeight > 0.01f)
			{
				Quaternion quaternion2 = this._spineRestRotation * Quaternion.Euler(this._currentSpineOffset);
				this.spineBone.localRotation = quaternion2;
			}
			if (this.neckBone != null && this.neckWeight > 0.01f)
			{
				Quaternion quaternion3 = this._neckRestRotation * Quaternion.Euler(this._currentNeckOffset);
				this.neckBone.localRotation = quaternion3;
			}
			if (this.headBone != null && this.headWeight > 0.01f)
			{
				Quaternion quaternion4 = this._headRestRotation * Quaternion.Euler(this._currentHeadOffset);
				this.headBone.localRotation = quaternion4;
			}
		}

		// Token: 0x0600726D RID: 29293 RVA: 0x002E9658 File Offset: 0x002E7858
		private void OnDrawGizmosSelected()
		{
			if (!this.drawGizmos || this.headBone == null)
			{
				return;
			}
			Gizmos.color = (this._isLooking ? Color.green : Color.gray);
			Gizmos.DrawLine(this.headBone.position, this.headBone.position + this.headBone.forward * 2f);
			if (this._isLooking)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(this._targetPosition, 0.2f);
				Gizmos.DrawLine(this.headBone.position, this._targetPosition);
			}
		}

		// Token: 0x0400757A RID: 30074
		[Header("Bone References (Auto-found if empty)")]
		[Tooltip("Head bone transform. Auto-found from Humanoid rig if empty.")]
		public Transform headBone;

		// Token: 0x0400757B RID: 30075
		[Tooltip("Spine bone for upper body rotation. Auto-found if empty.")]
		public Transform spineBone;

		// Token: 0x0400757C RID: 30076
		[Tooltip("Lower spine bone for additional body rotation. Auto-found if empty.")]
		public Transform lowerSpineBone;

		// Token: 0x0400757D RID: 30077
		[Tooltip("Neck bone for intermediate rotation. Auto-found if empty.")]
		public Transform neckBone;

		// Token: 0x0400757E RID: 30078
		[Header("Targeting")]
		[Tooltip("Offset applied to target position if no head bone is found (e.g., to look at player's head instead of feet)")]
		public Vector3 targetOffset = new Vector3(0f, 1.6f, 0f);

		// Token: 0x0400757F RID: 30079
		[Tooltip("The tag of the player object to find head bone on")]
		public string playerTag = "Player";

		// Token: 0x04007580 RID: 30080
		[Tooltip("Manually assign the player's target bone (e.g., Head or Chest). If set, this overrides auto-detection.")]
		public Transform manualPlayerTargetBone;

		// Token: 0x04007581 RID: 30081
		private Transform _cachedPlayerHead;

		// Token: 0x04007582 RID: 30082
		[Header("Look At Settings")]
		[Tooltip("How fast the head rotates toward target (degrees/second)")]
		[Range(30f, 180f)]
		public float rotationSpeed = 90f;

		// Token: 0x04007583 RID: 30083
		[Tooltip("How fast the head returns to neutral (degrees/second)")]
		[Range(30f, 180f)]
		public float returnSpeed = 60f;

		// Token: 0x04007584 RID: 30084
		[Tooltip("Maximum horizontal angle the head can turn (degrees)")]
		[Range(30f, 90f)]
		public float maxHorizontalAngle = 70f;

		// Token: 0x04007585 RID: 30085
		[Tooltip("Maximum vertical angle the head can turn (degrees)")]
		[Range(15f, 60f)]
		public float maxVerticalAngle = 40f;

		// Token: 0x04007586 RID: 30086
		[Header("Weight Distribution")]
		[Tooltip("How much the head contributes to the look-at (0-1)")]
		[Range(0f, 1f)]
		public float headWeight = 0.7f;

		// Token: 0x04007587 RID: 30087
		[Tooltip("How much the neck contributes to the look-at (0-1)")]
		[Range(0f, 1f)]
		public float neckWeight = 0.3f;

		// Token: 0x04007588 RID: 30088
		[Tooltip("How much the upper spine contributes to the look-at (0-1). Set to 0 for head-only rotation.")]
		[Range(0f, 1f)]
		public float spineWeight;

		// Token: 0x04007589 RID: 30089
		[Tooltip("How much the lower spine contributes to the look-at (0-1). Set to 0 for head-only rotation.")]
		[Range(0f, 1f)]
		public float lowerSpineWeight;

		// Token: 0x0400758A RID: 30090
		[Tooltip("Enable full upper body rotation (spine + shoulders). Useful for actions like phone calls.")]
		public bool enableFullBodyRotation;

		// Token: 0x0400758B RID: 30091
		[Tooltip("Limit vertical rotation (pitch). When true, head only rotates horizontally (yaw).")]
		public bool horizontalOnly = true;

		// Token: 0x0400758C RID: 30092
		[Header("Smoothing")]
		[Tooltip("Smoothing factor for rotation (higher = smoother but slower)")]
		[Range(1f, 20f)]
		public float smoothing = 8f;

		// Token: 0x0400758D RID: 30093
		[Header("Debug")]
		public bool debugLog;

		// Token: 0x0400758E RID: 30094
		public bool drawGizmos;

		// Token: 0x0400758F RID: 30095
		private bool _isLooking;

		// Token: 0x04007590 RID: 30096
		private Vector3 _targetPosition;

		// Token: 0x04007591 RID: 30097
		private Transform _targetTransform;

		// Token: 0x04007592 RID: 30098
		private Vector3 _currentHeadOffset = Vector3.zero;

		// Token: 0x04007593 RID: 30099
		private Vector3 _currentNeckOffset = Vector3.zero;

		// Token: 0x04007594 RID: 30100
		private Vector3 _currentSpineOffset = Vector3.zero;

		// Token: 0x04007595 RID: 30101
		private Vector3 _currentLowerSpineOffset = Vector3.zero;

		// Token: 0x04007596 RID: 30102
		private Quaternion _headRestRotation;

		// Token: 0x04007597 RID: 30103
		private Quaternion _neckRestRotation;

		// Token: 0x04007598 RID: 30104
		private Quaternion _spineRestRotation;

		// Token: 0x04007599 RID: 30105
		private Quaternion _lowerSpineRestRotation;

		// Token: 0x0400759A RID: 30106
		private bool _hasRestPose;

		// Token: 0x0400759B RID: 30107
		private Animator _animator;

		// Token: 0x0400759C RID: 30108
		private NPCWanderController _wander;

		// Token: 0x0400759D RID: 30109
		private bool _needsBoneRefresh;
	}
}
