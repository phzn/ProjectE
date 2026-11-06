using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200002B RID: 43
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachineTransposer : CinemachineComponentBase
	{
		// Token: 0x060001F8 RID: 504 RVA: 0x0000F8C6 File Offset: 0x0000DAC6
		protected virtual void OnValidate()
		{
			this.m_FollowOffset = this.EffectiveOffset;
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000F8D4 File Offset: 0x0000DAD4
		// (set) Token: 0x060001FA RID: 506 RVA: 0x0000F8DC File Offset: 0x0000DADC
		public bool HideOffsetInInspector { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000F8E8 File Offset: 0x0000DAE8
		public Vector3 EffectiveOffset
		{
			get
			{
				Vector3 followOffset = this.m_FollowOffset;
				if (this.m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
				{
					followOffset.x = 0f;
					followOffset.z = -Mathf.Abs(followOffset.z);
				}
				return followOffset;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000F925 File Offset: 0x0000DB25
		public override bool IsValid
		{
			get
			{
				return base.enabled && base.FollowTarget != null;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000F93D File Offset: 0x0000DB3D
		public override CinemachineCore.Stage Stage
		{
			get
			{
				return CinemachineCore.Stage.Body;
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000F940 File Offset: 0x0000DB40
		public override float GetMaxDampTime()
		{
			Vector3 damping = this.Damping;
			Vector3 angularDamping = this.AngularDamping;
			float num = Mathf.Max(damping.x, Mathf.Max(damping.y, damping.z));
			float num2 = Mathf.Max(angularDamping.x, Mathf.Max(angularDamping.y, angularDamping.z));
			return Mathf.Max(num, num2);
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000F99C File Offset: 0x0000DB9C
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			this.InitPrevFrameStateInfo(ref curState, deltaTime);
			if (this.IsValid)
			{
				Vector3 vector = this.EffectiveOffset;
				Vector3 vector2;
				Quaternion quaternion;
				this.TrackTarget(deltaTime, curState.ReferenceUp, vector, out vector2, out quaternion);
				vector = quaternion * vector;
				curState.ReferenceUp = quaternion * Vector3.up;
				Vector3 followTargetPosition = base.FollowTargetPosition;
				vector2 += this.GetOffsetForMinimumTargetDistance(vector2, vector, curState.RawOrientation * Vector3.forward, curState.ReferenceUp, followTargetPosition);
				curState.RawPosition = vector2 + vector;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000FA25 File Offset: 0x0000DC25
		public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			base.OnTargetObjectWarped(target, positionDelta);
			if (target == base.FollowTarget)
			{
				this.m_PreviousTargetPosition += positionDelta;
			}
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000FA50 File Offset: 0x0000DC50
		public override void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
			base.ForceCameraPosition(pos, rot);
			Quaternion quaternion = ((this.m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp) ? rot : this.GetReferenceOrientation(base.VirtualCamera.State.ReferenceUp));
			this.m_PreviousTargetPosition = pos - quaternion * this.EffectiveOffset;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000FAA0 File Offset: 0x0000DCA0
		protected void InitPrevFrameStateInfo(ref CameraState curState, float deltaTime)
		{
			bool flag = deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid;
			if (this.m_previousTarget != base.FollowTarget || !flag)
			{
				this.m_previousTarget = base.FollowTarget;
				this.m_targetOrientationOnAssign = base.FollowTargetRotation;
			}
			if (!flag)
			{
				this.m_PreviousTargetPosition = base.FollowTargetPosition;
				this.m_PreviousReferenceOrientation = this.GetReferenceOrientation(curState.ReferenceUp);
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000FB14 File Offset: 0x0000DD14
		protected void TrackTarget(float deltaTime, Vector3 up, Vector3 desiredCameraOffset, out Vector3 outTargetPosition, out Quaternion outTargetOrient)
		{
			Quaternion referenceOrientation = this.GetReferenceOrientation(up);
			Quaternion quaternion = referenceOrientation;
			bool flag = deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid;
			if (flag)
			{
				if (this.m_AngularDampingMode == CinemachineTransposer.AngularDampingMode.Quaternion && this.m_BindingMode == CinemachineTransposer.BindingMode.LockToTarget)
				{
					float num = base.VirtualCamera.DetachedFollowTargetDamp(1f, this.m_AngularDamping, deltaTime);
					quaternion = Quaternion.Slerp(this.m_PreviousReferenceOrientation, referenceOrientation, num);
				}
				else if (this.m_BindingMode != CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
				{
					Vector3 vector = (Quaternion.Inverse(this.m_PreviousReferenceOrientation) * referenceOrientation).eulerAngles;
					for (int i = 0; i < 3; i++)
					{
						if (vector[i] > 180f)
						{
							ref Vector3 ptr = ref vector;
							int num2 = i;
							ptr[num2] -= 360f;
						}
						if (Mathf.Abs(vector[i]) < 0.01f)
						{
							vector[i] = 0f;
						}
					}
					vector = base.VirtualCamera.DetachedFollowTargetDamp(vector, this.AngularDamping, deltaTime);
					quaternion = this.m_PreviousReferenceOrientation * Quaternion.Euler(vector);
				}
			}
			this.m_PreviousReferenceOrientation = quaternion;
			Vector3 followTargetPosition = base.FollowTargetPosition;
			Vector3 vector2 = this.m_PreviousTargetPosition;
			Vector3 vector3 = (flag ? this.m_PreviousOffset : desiredCameraOffset);
			if ((desiredCameraOffset - vector3).sqrMagnitude > 0.01f)
			{
				Quaternion quaternion2 = UnityVectorExtensions.SafeFromToRotation(this.m_PreviousOffset.ProjectOntoPlane(up), desiredCameraOffset.ProjectOntoPlane(up), up);
				vector2 = followTargetPosition + quaternion2 * (this.m_PreviousTargetPosition - followTargetPosition);
			}
			this.m_PreviousOffset = desiredCameraOffset;
			Vector3 vector4 = followTargetPosition - vector2;
			if (flag)
			{
				Quaternion quaternion3;
				if (desiredCameraOffset.AlmostZero())
				{
					quaternion3 = base.VcamState.RawOrientation;
				}
				else
				{
					quaternion3 = Quaternion.LookRotation(quaternion * desiredCameraOffset, up);
				}
				Vector3 vector5 = Quaternion.Inverse(quaternion3) * vector4;
				vector5 = base.VirtualCamera.DetachedFollowTargetDamp(vector5, this.Damping, deltaTime);
				vector4 = quaternion3 * vector5;
			}
			vector2 += vector4;
			outTargetPosition = (this.m_PreviousTargetPosition = vector2);
			outTargetOrient = quaternion;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000FD40 File Offset: 0x0000DF40
		protected Vector3 GetOffsetForMinimumTargetDistance(Vector3 dampedTargetPos, Vector3 cameraOffset, Vector3 cameraFwd, Vector3 up, Vector3 actualTargetPos)
		{
			Vector3 vector = Vector3.zero;
			if (base.VirtualCamera.FollowTargetAttachment > 0.9999f)
			{
				cameraOffset = cameraOffset.ProjectOntoPlane(up);
				float num = cameraOffset.magnitude * 0.2f;
				if (num > 0f)
				{
					actualTargetPos = actualTargetPos.ProjectOntoPlane(up);
					dampedTargetPos = dampedTargetPos.ProjectOntoPlane(up);
					Vector3 vector2 = dampedTargetPos + cameraOffset;
					float num2 = Vector3.Dot(actualTargetPos - vector2, (dampedTargetPos - vector2).normalized);
					if (num2 < num)
					{
						Vector3 vector3 = actualTargetPos - dampedTargetPos;
						float magnitude = vector3.magnitude;
						if (magnitude < 0.01f)
						{
							vector3 = -cameraFwd.ProjectOntoPlane(up);
						}
						else
						{
							vector3 /= magnitude;
						}
						vector = vector3 * (num - num2);
					}
					this.m_PreviousTargetPosition += vector;
				}
			}
			return vector;
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000FE20 File Offset: 0x0000E020
		protected Vector3 Damping
		{
			get
			{
				if (this.m_BindingMode == CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp)
				{
					return new Vector3(0f, this.m_YDamping, this.m_ZDamping);
				}
				return new Vector3(this.m_XDamping, this.m_YDamping, this.m_ZDamping);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000FE5C File Offset: 0x0000E05C
		protected Vector3 AngularDamping
		{
			get
			{
				switch (this.m_BindingMode)
				{
				case CinemachineTransposer.BindingMode.LockToTargetOnAssign:
				case CinemachineTransposer.BindingMode.WorldSpace:
				case CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp:
					return Vector3.zero;
				case CinemachineTransposer.BindingMode.LockToTargetWithWorldUp:
					return new Vector3(0f, this.m_YawDamping, 0f);
				case CinemachineTransposer.BindingMode.LockToTargetNoRoll:
					return new Vector3(this.m_PitchDamping, this.m_YawDamping, 0f);
				}
				return new Vector3(this.m_PitchDamping, this.m_YawDamping, this.m_RollDamping);
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000FEDA File Offset: 0x0000E0DA
		public virtual Vector3 GetTargetCameraPosition(Vector3 worldUp)
		{
			if (!this.IsValid)
			{
				return Vector3.zero;
			}
			return base.FollowTargetPosition + this.GetReferenceOrientation(worldUp) * this.EffectiveOffset;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000FF08 File Offset: 0x0000E108
		public Quaternion GetReferenceOrientation(Vector3 worldUp)
		{
			if (this.m_BindingMode == CinemachineTransposer.BindingMode.WorldSpace)
			{
				return Quaternion.identity;
			}
			if (base.FollowTarget != null)
			{
				Quaternion rotation = base.FollowTarget.rotation;
				switch (this.m_BindingMode)
				{
				case CinemachineTransposer.BindingMode.LockToTargetOnAssign:
					return this.m_targetOrientationOnAssign;
				case CinemachineTransposer.BindingMode.LockToTargetWithWorldUp:
				{
					Vector3 vector = (rotation * Vector3.forward).ProjectOntoPlane(worldUp);
					if (!vector.AlmostZero())
					{
						return Quaternion.LookRotation(vector, worldUp);
					}
					break;
				}
				case CinemachineTransposer.BindingMode.LockToTargetNoRoll:
					return Quaternion.LookRotation(rotation * Vector3.forward, worldUp);
				case CinemachineTransposer.BindingMode.LockToTarget:
					return rotation;
				case CinemachineTransposer.BindingMode.SimpleFollowWithWorldUp:
				{
					Vector3 vector2 = (base.FollowTargetPosition - base.VcamState.RawPosition).ProjectOntoPlane(worldUp);
					if (!vector2.AlmostZero())
					{
						return Quaternion.LookRotation(vector2, worldUp);
					}
					break;
				}
				}
			}
			return this.m_PreviousReferenceOrientation.normalized;
		}

		// Token: 0x0400017D RID: 381
		[Tooltip("The coordinate space to use when interpreting the offset from the target.  This is also used to set the camera's Up vector, which will be maintained when aiming the camera.")]
		public CinemachineTransposer.BindingMode m_BindingMode = CinemachineTransposer.BindingMode.LockToTargetWithWorldUp;

		// Token: 0x0400017E RID: 382
		[Tooltip("The distance vector that the transposer will attempt to maintain from the Follow target")]
		public Vector3 m_FollowOffset = Vector3.back * 10f;

		// Token: 0x0400017F RID: 383
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the X-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's x-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_XDamping = 1f;

		// Token: 0x04000180 RID: 384
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the Y-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's y-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_YDamping = 1f;

		// Token: 0x04000181 RID: 385
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the Z-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's z-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_ZDamping = 1f;

		// Token: 0x04000182 RID: 386
		public CinemachineTransposer.AngularDampingMode m_AngularDampingMode;

		// Token: 0x04000183 RID: 387
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to track the target rotation's X angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
		public float m_PitchDamping;

		// Token: 0x04000184 RID: 388
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to track the target rotation's Y angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
		public float m_YawDamping;

		// Token: 0x04000185 RID: 389
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to track the target rotation's Z angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
		public float m_RollDamping;

		// Token: 0x04000186 RID: 390
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to track the target's orientation.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
		public float m_AngularDamping;

		// Token: 0x04000188 RID: 392
		private Vector3 m_PreviousTargetPosition = Vector3.zero;

		// Token: 0x04000189 RID: 393
		private Quaternion m_PreviousReferenceOrientation = Quaternion.identity;

		// Token: 0x0400018A RID: 394
		private Quaternion m_targetOrientationOnAssign = Quaternion.identity;

		// Token: 0x0400018B RID: 395
		private Vector3 m_PreviousOffset;

		// Token: 0x0400018C RID: 396
		private Transform m_previousTarget;

		// Token: 0x0200009E RID: 158
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		public enum BindingMode
		{
			// Token: 0x04000345 RID: 837
			LockToTargetOnAssign,
			// Token: 0x04000346 RID: 838
			LockToTargetWithWorldUp,
			// Token: 0x04000347 RID: 839
			LockToTargetNoRoll,
			// Token: 0x04000348 RID: 840
			LockToTarget,
			// Token: 0x04000349 RID: 841
			WorldSpace,
			// Token: 0x0400034A RID: 842
			SimpleFollowWithWorldUp
		}

		// Token: 0x0200009F RID: 159
		public enum AngularDampingMode
		{
			// Token: 0x0400034C RID: 844
			Euler,
			// Token: 0x0400034D RID: 845
			Quaternion
		}
	}
}
