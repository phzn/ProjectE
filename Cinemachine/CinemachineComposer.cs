using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000022 RID: 34
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachineComposer : CinemachineComponentBase
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000C3FE File Offset: 0x0000A5FE
		public override bool IsValid
		{
			get
			{
				return base.enabled && base.LookAtTarget != null;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000C416 File Offset: 0x0000A616
		public override CinemachineCore.Stage Stage
		{
			get
			{
				return CinemachineCore.Stage.Aim;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000C419 File Offset: 0x0000A619
		// (set) Token: 0x06000196 RID: 406 RVA: 0x0000C421 File Offset: 0x0000A621
		public Vector3 TrackedPoint { get; private set; }

		// Token: 0x06000197 RID: 407 RVA: 0x0000C42C File Offset: 0x0000A62C
		protected virtual Vector3 GetLookAtPointAndSetTrackedPoint(Vector3 lookAt, Vector3 up, float deltaTime)
		{
			Vector3 vector = lookAt;
			if (base.LookAtTarget != null)
			{
				vector += base.LookAtTargetRotation * this.m_TrackedObjectOffset;
			}
			if (this.m_LookaheadTime < 0.0001f)
			{
				this.TrackedPoint = vector;
			}
			else
			{
				bool flag = base.VirtualCamera.LookAtTargetChanged || !base.VirtualCamera.PreviousStateIsValid;
				this.m_Predictor.Smoothing = this.m_LookaheadSmoothing;
				this.m_Predictor.AddPosition(vector, flag ? (-1f) : deltaTime, this.m_LookaheadTime);
				Vector3 vector2 = this.m_Predictor.PredictPositionDelta(this.m_LookaheadTime);
				if (this.m_LookaheadIgnoreY)
				{
					vector2 = vector2.ProjectOntoPlane(up);
				}
				this.TrackedPoint = vector + vector2;
			}
			return vector;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000C4F4 File Offset: 0x0000A6F4
		public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			base.OnTargetObjectWarped(target, positionDelta);
			if (target == base.LookAtTarget)
			{
				this.m_CameraPosPrevFrame += positionDelta;
				this.m_LookAtPrevFrame += positionDelta;
				this.m_Predictor.ApplyTransformDelta(positionDelta);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000C547 File Offset: 0x0000A747
		public override void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
			base.ForceCameraPosition(pos, rot);
			this.m_CameraPosPrevFrame = pos;
			this.m_CameraOrientationPrevFrame = rot;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000C55F File Offset: 0x0000A75F
		public override float GetMaxDampTime()
		{
			return Mathf.Max(this.m_HorizontalDamping, this.m_VerticalDamping);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000C572 File Offset: 0x0000A772
		public override void PrePipelineMutateCameraState(ref CameraState curState, float deltaTime)
		{
			if (this.IsValid && curState.HasLookAt)
			{
				curState.ReferenceLookAt = this.GetLookAtPointAndSetTrackedPoint(curState.ReferenceLookAt, curState.ReferenceUp, deltaTime);
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000C5A0 File Offset: 0x0000A7A0
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			if (!this.IsValid || !curState.HasLookAt)
			{
				return;
			}
			if (!(this.TrackedPoint - curState.ReferenceLookAt).AlmostZero())
			{
				Vector3 vector = Vector3.Lerp(curState.CorrectedPosition, curState.ReferenceLookAt, 0.5f);
				Vector3 vector2 = curState.ReferenceLookAt - vector;
				Vector3 vector3 = this.TrackedPoint - vector;
				if (Vector3.Dot(vector2, vector3) < 0f)
				{
					float num = Vector3.Distance(curState.ReferenceLookAt, vector) / Vector3.Distance(curState.ReferenceLookAt, this.TrackedPoint);
					this.TrackedPoint = Vector3.Lerp(curState.ReferenceLookAt, this.TrackedPoint, num);
				}
			}
			float magnitude = (this.TrackedPoint - curState.CorrectedPosition).magnitude;
			if (magnitude < 0.0001f)
			{
				if (deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
				{
					curState.RawOrientation = this.m_CameraOrientationPrevFrame;
				}
				return;
			}
			this.mCache.UpdateCache(curState.Lens, this.SoftGuideRect, this.HardGuideRect, magnitude);
			Quaternion quaternion = curState.RawOrientation;
			if (deltaTime < 0f || !base.VirtualCamera.PreviousStateIsValid)
			{
				quaternion = Quaternion.LookRotation(quaternion * Vector3.forward, curState.ReferenceUp);
				Rect mFovSoftGuideRect = this.mCache.mFovSoftGuideRect;
				if (this.m_CenterOnActivate)
				{
					mFovSoftGuideRect = new Rect(mFovSoftGuideRect.center, Vector2.zero);
				}
				this.RotateToScreenBounds(ref curState, mFovSoftGuideRect, curState.ReferenceLookAt, ref quaternion, this.mCache.mFov, this.mCache.mFovH, -1f);
			}
			else
			{
				Vector3 vector4 = this.m_LookAtPrevFrame - this.m_CameraPosPrevFrame;
				if (vector4.AlmostZero())
				{
					quaternion = Quaternion.LookRotation(this.m_CameraOrientationPrevFrame * Vector3.forward, curState.ReferenceUp);
				}
				else
				{
					vector4 = Quaternion.Euler(curState.PositionDampingBypass) * vector4;
					quaternion = Quaternion.LookRotation(vector4, curState.ReferenceUp);
					quaternion = quaternion.ApplyCameraRotation(-this.m_ScreenOffsetPrevFrame, curState.ReferenceUp);
				}
				this.RotateToScreenBounds(ref curState, this.mCache.mFovSoftGuideRect, this.TrackedPoint, ref quaternion, this.mCache.mFov, this.mCache.mFovH, deltaTime);
				if (deltaTime < 0f || base.VirtualCamera.LookAtTargetAttachment > 0.9999f)
				{
					this.RotateToScreenBounds(ref curState, this.mCache.mFovHardGuideRect, curState.ReferenceLookAt, ref quaternion, this.mCache.mFov, this.mCache.mFovH, -1f);
				}
			}
			this.m_CameraPosPrevFrame = curState.CorrectedPosition;
			this.m_LookAtPrevFrame = this.TrackedPoint;
			this.m_CameraOrientationPrevFrame = quaternion.Normalized();
			this.m_ScreenOffsetPrevFrame = this.m_CameraOrientationPrevFrame.GetCameraRotationToTarget(this.m_LookAtPrevFrame - curState.CorrectedPosition, curState.ReferenceUp);
			curState.RawOrientation = this.m_CameraOrientationPrevFrame;
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0000C887 File Offset: 0x0000AA87
		// (set) Token: 0x0600019E RID: 414 RVA: 0x0000C8C0 File Offset: 0x0000AAC0
		internal Rect SoftGuideRect
		{
			get
			{
				return new Rect(this.m_ScreenX - this.m_DeadZoneWidth / 2f, this.m_ScreenY - this.m_DeadZoneHeight / 2f, this.m_DeadZoneWidth, this.m_DeadZoneHeight);
			}
			set
			{
				this.m_DeadZoneWidth = Mathf.Clamp(value.width, 0f, 2f);
				this.m_DeadZoneHeight = Mathf.Clamp(value.height, 0f, 2f);
				this.m_ScreenX = Mathf.Clamp(value.x + this.m_DeadZoneWidth / 2f, -0.5f, 1.5f);
				this.m_ScreenY = Mathf.Clamp(value.y + this.m_DeadZoneHeight / 2f, -0.5f, 1.5f);
				this.m_SoftZoneWidth = Mathf.Max(this.m_SoftZoneWidth, this.m_DeadZoneWidth);
				this.m_SoftZoneHeight = Mathf.Max(this.m_SoftZoneHeight, this.m_DeadZoneHeight);
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0000C988 File Offset: 0x0000AB88
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x0000CA10 File Offset: 0x0000AC10
		internal Rect HardGuideRect
		{
			get
			{
				Rect rect = new Rect(this.m_ScreenX - this.m_SoftZoneWidth / 2f, this.m_ScreenY - this.m_SoftZoneHeight / 2f, this.m_SoftZoneWidth, this.m_SoftZoneHeight);
				rect.position += new Vector2(this.m_BiasX * (this.m_SoftZoneWidth - this.m_DeadZoneWidth), this.m_BiasY * (this.m_SoftZoneHeight - this.m_DeadZoneHeight));
				return rect;
			}
			set
			{
				this.m_SoftZoneWidth = Mathf.Clamp(value.width, 0f, 2f);
				this.m_SoftZoneHeight = Mathf.Clamp(value.height, 0f, 2f);
				this.m_DeadZoneWidth = Mathf.Min(this.m_DeadZoneWidth, this.m_SoftZoneWidth);
				this.m_DeadZoneHeight = Mathf.Min(this.m_DeadZoneHeight, this.m_SoftZoneHeight);
			}
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000CA84 File Offset: 0x0000AC84
		private void RotateToScreenBounds(ref CameraState state, Rect screenRect, Vector3 trackedPoint, ref Quaternion rigOrientation, float fov, float fovH, float deltaTime)
		{
			Vector3 vector = trackedPoint - state.CorrectedPosition;
			Vector2 cameraRotationToTarget = rigOrientation.GetCameraRotationToTarget(vector, state.ReferenceUp);
			this.ClampVerticalBounds(ref screenRect, vector, state.ReferenceUp, fov);
			float num = (screenRect.yMin - 0.5f) * fov;
			float num2 = (screenRect.yMax - 0.5f) * fov;
			if (cameraRotationToTarget.x < num)
			{
				cameraRotationToTarget.x -= num;
			}
			else if (cameraRotationToTarget.x > num2)
			{
				cameraRotationToTarget.x -= num2;
			}
			else
			{
				cameraRotationToTarget.x = 0f;
			}
			num = (screenRect.xMin - 0.5f) * fovH;
			num2 = (screenRect.xMax - 0.5f) * fovH;
			if (cameraRotationToTarget.y < num)
			{
				cameraRotationToTarget.y -= num;
			}
			else if (cameraRotationToTarget.y > num2)
			{
				cameraRotationToTarget.y -= num2;
			}
			else
			{
				cameraRotationToTarget.y = 0f;
			}
			if (deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
			{
				cameraRotationToTarget.x = base.VirtualCamera.DetachedLookAtTargetDamp(cameraRotationToTarget.x, this.m_VerticalDamping, deltaTime);
				cameraRotationToTarget.y = base.VirtualCamera.DetachedLookAtTargetDamp(cameraRotationToTarget.y, this.m_HorizontalDamping, deltaTime);
			}
			rigOrientation = rigOrientation.ApplyCameraRotation(cameraRotationToTarget, state.ReferenceUp);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000CBEC File Offset: 0x0000ADEC
		private bool ClampVerticalBounds(ref Rect r, Vector3 dir, Vector3 up, float fov)
		{
			float num = UnityVectorExtensions.Angle(dir, up);
			float num2 = fov / 2f + 1f;
			if (num < num2)
			{
				float num3 = 1f - (num2 - num) / fov;
				if (r.yMax > num3)
				{
					r.yMin = Mathf.Min(r.yMin, num3);
					r.yMax = Mathf.Min(r.yMax, num3);
					return true;
				}
			}
			if (num > 180f - num2)
			{
				float num4 = (num - (180f - num2)) / fov;
				if (num4 > r.yMin)
				{
					r.yMin = Mathf.Max(r.yMin, num4);
					r.yMax = Mathf.Max(r.yMax, num4);
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000106 RID: 262
		[Tooltip("Target offset from the target object's center in target-local space. Use this to fine-tune the tracking target position when the desired area is not the tracked object's center.")]
		public Vector3 m_TrackedObjectOffset = Vector3.zero;

		// Token: 0x04000107 RID: 263
		[Space]
		[Tooltip("This setting will instruct the composer to adjust its target offset based on the motion of the target.  The composer will look at a point where it estimates the target will be this many seconds into the future.  Note that this setting is sensitive to noisy animation, and can amplify the noise, resulting in undesirable camera jitter.  If the camera jitters unacceptably when the target is in motion, turn down this setting, or animate the target more smoothly.")]
		[Range(0f, 1f)]
		public float m_LookaheadTime;

		// Token: 0x04000108 RID: 264
		[Tooltip("Controls the smoothness of the lookahead algorithm.  Larger values smooth out jittery predictions and also increase prediction lag")]
		[Range(0f, 30f)]
		public float m_LookaheadSmoothing;

		// Token: 0x04000109 RID: 265
		[Tooltip("If checked, movement along the Y axis will be ignored for lookahead calculations")]
		public bool m_LookaheadIgnoreY;

		// Token: 0x0400010A RID: 266
		[Space]
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to follow the target in the screen-horizontal direction. Small numbers are more responsive, rapidly orienting the camera to keep the target in the dead zone. Larger numbers give a more heavy slowly responding camera. Using different vertical and horizontal settings can yield a wide range of camera behaviors.")]
		public float m_HorizontalDamping = 0.5f;

		// Token: 0x0400010B RID: 267
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to follow the target in the screen-vertical direction. Small numbers are more responsive, rapidly orienting the camera to keep the target in the dead zone. Larger numbers give a more heavy slowly responding camera. Using different vertical and horizontal settings can yield a wide range of camera behaviors.")]
		public float m_VerticalDamping = 0.5f;

		// Token: 0x0400010C RID: 268
		[Space]
		[Range(-0.5f, 1.5f)]
		[Tooltip("Horizontal screen position for target. The camera will rotate to position the tracked object here.")]
		public float m_ScreenX = 0.5f;

		// Token: 0x0400010D RID: 269
		[Range(-0.5f, 1.5f)]
		[Tooltip("Vertical screen position for target, The camera will rotate to position the tracked object here.")]
		public float m_ScreenY = 0.5f;

		// Token: 0x0400010E RID: 270
		[Range(0f, 2f)]
		[Tooltip("Camera will not rotate horizontally if the target is within this range of the position.")]
		public float m_DeadZoneWidth;

		// Token: 0x0400010F RID: 271
		[Range(0f, 2f)]
		[Tooltip("Camera will not rotate vertically if the target is within this range of the position.")]
		public float m_DeadZoneHeight;

		// Token: 0x04000110 RID: 272
		[Range(0f, 2f)]
		[Tooltip("When target is within this region, camera will gradually rotate horizontally to re-align towards the desired position, depending on the damping speed.")]
		public float m_SoftZoneWidth = 0.8f;

		// Token: 0x04000111 RID: 273
		[Range(0f, 2f)]
		[Tooltip("When target is within this region, camera will gradually rotate vertically to re-align towards the desired position, depending on the damping speed.")]
		public float m_SoftZoneHeight = 0.8f;

		// Token: 0x04000112 RID: 274
		[Range(-0.5f, 0.5f)]
		[Tooltip("A non-zero bias will move the target position horizontally away from the center of the soft zone.")]
		public float m_BiasX;

		// Token: 0x04000113 RID: 275
		[Range(-0.5f, 0.5f)]
		[Tooltip("A non-zero bias will move the target position vertically away from the center of the soft zone.")]
		public float m_BiasY;

		// Token: 0x04000114 RID: 276
		[Tooltip("Force target to center of screen when this camera activates.  If false, will clamp target to the edges of the dead zone")]
		public bool m_CenterOnActivate = true;

		// Token: 0x04000116 RID: 278
		private Vector3 m_CameraPosPrevFrame = Vector3.zero;

		// Token: 0x04000117 RID: 279
		private Vector3 m_LookAtPrevFrame = Vector3.zero;

		// Token: 0x04000118 RID: 280
		private Vector2 m_ScreenOffsetPrevFrame = Vector2.zero;

		// Token: 0x04000119 RID: 281
		private Quaternion m_CameraOrientationPrevFrame = Quaternion.identity;

		// Token: 0x0400011A RID: 282
		internal PositionPredictor m_Predictor = new PositionPredictor();

		// Token: 0x0400011B RID: 283
		private CinemachineComposer.FovCache mCache;

		// Token: 0x02000093 RID: 147
		private struct FovCache
		{
			// Token: 0x0600043D RID: 1085 RVA: 0x00018DB4 File Offset: 0x00016FB4
			public void UpdateCache(LensSettings lens, Rect softGuide, Rect hardGuide, float targetDistance)
			{
				bool flag = this.mAspect != lens.Aspect || softGuide != this.mSoftGuideRect || hardGuide != this.mHardGuideRect;
				if (lens.Orthographic)
				{
					float num = Mathf.Abs(lens.OrthographicSize / targetDistance);
					if (this.mOrthoSizeOverDistance == 0f || Mathf.Abs(num - this.mOrthoSizeOverDistance) / this.mOrthoSizeOverDistance > this.mOrthoSizeOverDistance * 0.01f)
					{
						flag = true;
					}
					if (flag)
					{
						this.mFov = 114.59156f * Mathf.Atan(num);
						this.mFovH = 114.59156f * Mathf.Atan(lens.Aspect * num);
						this.mOrthoSizeOverDistance = num;
					}
				}
				else
				{
					float fieldOfView = lens.FieldOfView;
					if (this.mFov != fieldOfView)
					{
						flag = true;
					}
					if (flag)
					{
						this.mFov = fieldOfView;
						double num2 = 2.0 * Math.Atan(Math.Tan((double)(this.mFov * 0.017453292f / 2f)) * (double)lens.Aspect);
						this.mFovH = (float)(57.295780181884766 * num2);
						this.mOrthoSizeOverDistance = 0f;
					}
				}
				if (flag)
				{
					this.mFovSoftGuideRect = this.ScreenToFOV(softGuide, this.mFov, this.mFovH, lens.Aspect);
					this.mSoftGuideRect = softGuide;
					this.mFovHardGuideRect = this.ScreenToFOV(hardGuide, this.mFov, this.mFovH, lens.Aspect);
					this.mHardGuideRect = hardGuide;
					this.mAspect = lens.Aspect;
				}
			}

			// Token: 0x0600043E RID: 1086 RVA: 0x00018F3C File Offset: 0x0001713C
			private Rect ScreenToFOV(Rect rScreen, float fov, float fovH, float aspect)
			{
				Rect rect = new Rect(rScreen);
				Matrix4x4 inverse = Matrix4x4.Perspective(fov, aspect, 0.0001f, 2f).inverse;
				Vector3 vector = inverse.MultiplyPoint(new Vector3(0f, rect.yMin * 2f - 1f, 0.5f));
				vector.z = -vector.z;
				float num = UnityVectorExtensions.SignedAngle(Vector3.forward, vector, Vector3.left);
				rect.yMin = (fov / 2f + num) / fov;
				vector = inverse.MultiplyPoint(new Vector3(0f, rect.yMax * 2f - 1f, 0.5f));
				vector.z = -vector.z;
				num = UnityVectorExtensions.SignedAngle(Vector3.forward, vector, Vector3.left);
				rect.yMax = (fov / 2f + num) / fov;
				vector = inverse.MultiplyPoint(new Vector3(rect.xMin * 2f - 1f, 0f, 0.5f));
				vector.z = -vector.z;
				num = UnityVectorExtensions.SignedAngle(Vector3.forward, vector, Vector3.up);
				rect.xMin = (fovH / 2f + num) / fovH;
				vector = inverse.MultiplyPoint(new Vector3(rect.xMax * 2f - 1f, 0f, 0.5f));
				vector.z = -vector.z;
				num = UnityVectorExtensions.SignedAngle(Vector3.forward, vector, Vector3.up);
				rect.xMax = (fovH / 2f + num) / fovH;
				return rect;
			}

			// Token: 0x04000318 RID: 792
			public Rect mFovSoftGuideRect;

			// Token: 0x04000319 RID: 793
			public Rect mFovHardGuideRect;

			// Token: 0x0400031A RID: 794
			public float mFovH;

			// Token: 0x0400031B RID: 795
			public float mFov;

			// Token: 0x0400031C RID: 796
			private float mOrthoSizeOverDistance;

			// Token: 0x0400031D RID: 797
			private float mAspect;

			// Token: 0x0400031E RID: 798
			private Rect mSoftGuideRect;

			// Token: 0x0400031F RID: 799
			private Rect mHardGuideRect;
		}
	}
}
