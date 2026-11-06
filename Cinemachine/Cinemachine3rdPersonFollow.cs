using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000020 RID: 32
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class Cinemachine3rdPersonFollow : CinemachineComponentBase
	{
		// Token: 0x0600017F RID: 383 RVA: 0x0000BC28 File Offset: 0x00009E28
		private void OnValidate()
		{
			this.CameraSide = Mathf.Clamp(this.CameraSide, -1f, 1f);
			this.Damping.x = Mathf.Max(0f, this.Damping.x);
			this.Damping.y = Mathf.Max(0f, this.Damping.y);
			this.Damping.z = Mathf.Max(0f, this.Damping.z);
			this.CameraRadius = Mathf.Max(0.001f, this.CameraRadius);
			this.DampingIntoCollision = Mathf.Max(0f, this.DampingIntoCollision);
			this.DampingFromCollision = Mathf.Max(0f, this.DampingFromCollision);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000BCF4 File Offset: 0x00009EF4
		private void Reset()
		{
			this.ShoulderOffset = new Vector3(0.5f, -0.4f, 0f);
			this.VerticalArmLength = 0.4f;
			this.CameraSide = 1f;
			this.CameraDistance = 2f;
			this.Damping = new Vector3(0.1f, 0.5f, 0.3f);
			this.CameraCollisionFilter = 0;
			this.CameraRadius = 0.2f;
			this.DampingIntoCollision = 0f;
			this.DampingFromCollision = 2f;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000BD83 File Offset: 0x00009F83
		private void OnDestroy()
		{
			RuntimeUtility.DestroyScratchCollider();
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000BD8A File Offset: 0x00009F8A
		public override bool IsValid
		{
			get
			{
				return base.enabled && base.FollowTarget != null;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000BDA2 File Offset: 0x00009FA2
		public override CinemachineCore.Stage Stage
		{
			get
			{
				return CinemachineCore.Stage.Body;
			}
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000BDA8 File Offset: 0x00009FA8
		public override float GetMaxDampTime()
		{
			return Mathf.Max(Mathf.Max(this.DampingIntoCollision, this.DampingFromCollision), Mathf.Max(this.Damping.x, Mathf.Max(this.Damping.y, this.Damping.z)));
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000BDF6 File Offset: 0x00009FF6
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			if (this.IsValid)
			{
				if (!base.VirtualCamera.PreviousStateIsValid)
				{
					deltaTime = -1f;
				}
				this.PositionCamera(ref curState, deltaTime);
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000BE1C File Offset: 0x0000A01C
		public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			base.OnTargetObjectWarped(target, positionDelta);
			if (target == base.FollowTarget)
			{
				this.m_PreviousFollowTargetPosition += positionDelta;
			}
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000BE48 File Offset: 0x0000A048
		private void PositionCamera(ref CameraState curState, float deltaTime)
		{
			Vector3 referenceUp = curState.ReferenceUp;
			Vector3 followTargetPosition = base.FollowTargetPosition;
			Quaternion followTargetRotation = base.FollowTargetRotation;
			Vector3 vector = followTargetRotation * Vector3.forward;
			Quaternion heading = Cinemachine3rdPersonFollow.GetHeading(followTargetRotation, referenceUp);
			if (deltaTime < 0f)
			{
				this.m_DampingCorrection = Vector3.zero;
				this.m_CamPosCollisionCorrection = 0f;
			}
			else
			{
				this.m_DampingCorrection += Quaternion.Inverse(heading) * (this.m_PreviousFollowTargetPosition - followTargetPosition);
				this.m_DampingCorrection -= base.VirtualCamera.DetachedFollowTargetDamp(this.m_DampingCorrection, this.Damping, deltaTime);
			}
			this.m_PreviousFollowTargetPosition = followTargetPosition;
			Vector3 vector2 = followTargetPosition;
			Vector3 vector3;
			Vector3 vector4;
			this.GetRawRigPositions(vector2, followTargetRotation, heading, out vector3, out vector4);
			Vector3 vector5 = vector4 - vector * (this.CameraDistance - this.m_DampingCorrection.z);
			float num = 0f;
			Vector3 vector6 = this.ResolveCollisions(vector2, vector4, -1f, this.CameraRadius * 1.05f, ref num);
			vector5 = this.ResolveCollisions(vector6, vector5, deltaTime, this.CameraRadius, ref this.m_CamPosCollisionCorrection);
			curState.RawPosition = vector5;
			curState.RawOrientation = followTargetRotation;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000BF7C File Offset: 0x0000A17C
		public void GetRigPositions(out Vector3 root, out Vector3 shoulder, out Vector3 hand)
		{
			Vector3 referenceUp = base.VirtualCamera.State.ReferenceUp;
			Quaternion followTargetRotation = base.FollowTargetRotation;
			Quaternion heading = Cinemachine3rdPersonFollow.GetHeading(followTargetRotation, referenceUp);
			root = this.m_PreviousFollowTargetPosition;
			this.GetRawRigPositions(root, followTargetRotation, heading, out shoulder, out hand);
			float num = 0f;
			hand = this.ResolveCollisions(root, hand, -1f, this.CameraRadius * 1.05f, ref num);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000BFF8 File Offset: 0x0000A1F8
		internal static Quaternion GetHeading(Quaternion targetRot, Vector3 up)
		{
			Vector3 vector = targetRot * Vector3.forward;
			Vector3 vector2 = Vector3.Cross(up, Vector3.Cross(vector.ProjectOntoPlane(up), up));
			if (vector2.AlmostZero())
			{
				vector2 = Vector3.Cross(targetRot * Vector3.right, up);
			}
			return Quaternion.LookRotation(vector2, up);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000C048 File Offset: 0x0000A248
		private void GetRawRigPositions(Vector3 root, Quaternion targetRot, Quaternion heading, out Vector3 shoulder, out Vector3 hand)
		{
			Vector3 shoulderOffset = this.ShoulderOffset;
			shoulderOffset.x = Mathf.Lerp(-shoulderOffset.x, shoulderOffset.x, this.CameraSide);
			shoulderOffset.x += this.m_DampingCorrection.x;
			shoulderOffset.y += this.m_DampingCorrection.y;
			shoulder = root + heading * shoulderOffset;
			hand = shoulder + targetRot * new Vector3(0f, this.VerticalArmLength, 0f);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000C0EC File Offset: 0x0000A2EC
		private Vector3 ResolveCollisions(Vector3 root, Vector3 tip, float deltaTime, float cameraRadius, ref float collisionCorrection)
		{
			if (this.CameraCollisionFilter.value == 0)
			{
				return tip;
			}
			Vector3 vector = tip - root;
			float magnitude = vector.magnitude;
			if (magnitude < 0.0001f)
			{
				return tip;
			}
			vector /= magnitude;
			Vector3 vector2 = tip;
			float num = 0f;
			RaycastHit raycastHit;
			if (RuntimeUtility.SphereCastIgnoreTag(root, cameraRadius, vector, out raycastHit, magnitude, this.CameraCollisionFilter, in this.IgnoreTag))
			{
				num = (raycastHit.point + raycastHit.normal * cameraRadius - tip).magnitude;
			}
			collisionCorrection += ((deltaTime < 0f) ? (num - collisionCorrection) : Damper.Damp(num - collisionCorrection, (num > collisionCorrection) ? this.DampingIntoCollision : this.DampingFromCollision, deltaTime));
			if (collisionCorrection > 0.0001f)
			{
				vector2 -= vector * collisionCorrection;
			}
			return vector2;
		}

		// Token: 0x040000F2 RID: 242
		[Tooltip("How responsively the camera tracks the target.  Each axis (camera-local) can have its own setting.  Value is the approximate time it takes the camera to catch up to the target's new position.  Smaller values give a more rigid effect, larger values give a squishier one")]
		public Vector3 Damping;

		// Token: 0x040000F3 RID: 243
		[Header("Rig")]
		[Tooltip("Position of the shoulder pivot relative to the Follow target origin.  This offset is in target-local space")]
		public Vector3 ShoulderOffset;

		// Token: 0x040000F4 RID: 244
		[Tooltip("Vertical offset of the hand in relation to the shoulder.  Arm length will affect the follow target's screen position when the camera rotates vertically")]
		public float VerticalArmLength;

		// Token: 0x040000F5 RID: 245
		[Tooltip("Specifies which shoulder (left, right, or in-between) the camera is on")]
		[Range(0f, 1f)]
		public float CameraSide;

		// Token: 0x040000F6 RID: 246
		[Tooltip("How far behind the hand the camera will be placed")]
		public float CameraDistance;

		// Token: 0x040000F7 RID: 247
		[Header("Obstacles")]
		[Tooltip("Camera will avoid obstacles on these layers")]
		public LayerMask CameraCollisionFilter;

		// Token: 0x040000F8 RID: 248
		[TagField]
		[Tooltip("Obstacles with this tag will be ignored.  It is a good idea to set this field to the target's tag")]
		public string IgnoreTag = string.Empty;

		// Token: 0x040000F9 RID: 249
		[Tooltip("Specifies how close the camera can get to obstacles")]
		[Range(0f, 1f)]
		public float CameraRadius;

		// Token: 0x040000FA RID: 250
		[Range(0f, 10f)]
		[Tooltip("How gradually the camera moves to correct for occlusions.  Higher numbers will move the camera more gradually.")]
		public float DampingIntoCollision;

		// Token: 0x040000FB RID: 251
		[Range(0f, 10f)]
		[Tooltip("How gradually the camera returns to its normal position after having been corrected by the built-in collision resolution system.  Higher numbers will move the camera more gradually back to normal.")]
		public float DampingFromCollision;

		// Token: 0x040000FC RID: 252
		private Vector3 m_PreviousFollowTargetPosition;

		// Token: 0x040000FD RID: 253
		private Vector3 m_DampingCorrection;

		// Token: 0x040000FE RID: 254
		private float m_CamPosCollisionCorrection;
	}
}
