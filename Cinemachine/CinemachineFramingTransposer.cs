using System;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	// Token: 0x02000023 RID: 35
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachineFramingTransposer : CinemachineComponentBase
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000CD36 File Offset: 0x0000AF36
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x0000CD70 File Offset: 0x0000AF70
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000CE38 File Offset: 0x0000B038
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x0000CEC0 File Offset: 0x0000B0C0
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

		// Token: 0x060001A8 RID: 424 RVA: 0x0000CF34 File Offset: 0x0000B134
		private void OnValidate()
		{
			this.m_CameraDistance = Mathf.Max(this.m_CameraDistance, 0.01f);
			this.m_DeadZoneDepth = Mathf.Max(this.m_DeadZoneDepth, 0f);
			this.m_GroupFramingSize = Mathf.Max(0.001f, this.m_GroupFramingSize);
			this.m_MaxDollyIn = Mathf.Max(0f, this.m_MaxDollyIn);
			this.m_MaxDollyOut = Mathf.Max(0f, this.m_MaxDollyOut);
			this.m_MinimumDistance = Mathf.Max(0f, this.m_MinimumDistance);
			this.m_MaximumDistance = Mathf.Max(this.m_MinimumDistance, this.m_MaximumDistance);
			this.m_MinimumFOV = Mathf.Max(1f, this.m_MinimumFOV);
			this.m_MaximumFOV = Mathf.Clamp(this.m_MaximumFOV, this.m_MinimumFOV, 179f);
			this.m_MinimumOrthoSize = Mathf.Max(0.01f, this.m_MinimumOrthoSize);
			this.m_MaximumOrthoSize = Mathf.Max(this.m_MinimumOrthoSize, this.m_MaximumOrthoSize);
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0000D03B File Offset: 0x0000B23B
		public override bool IsValid
		{
			get
			{
				return base.enabled && base.FollowTarget != null;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000D053 File Offset: 0x0000B253
		public override CinemachineCore.Stage Stage
		{
			get
			{
				return CinemachineCore.Stage.Body;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000D056 File Offset: 0x0000B256
		public override bool BodyAppliesAfterAim
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000D059 File Offset: 0x0000B259
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000D061 File Offset: 0x0000B261
		public Vector3 TrackedPoint { get; private set; }

		// Token: 0x060001AE RID: 430 RVA: 0x0000D06A File Offset: 0x0000B26A
		public override void OnTargetObjectWarped(Transform target, Vector3 positionDelta)
		{
			base.OnTargetObjectWarped(target, positionDelta);
			if (target == base.FollowTarget)
			{
				this.m_PreviousCameraPosition += positionDelta;
				this.m_Predictor.ApplyTransformDelta(positionDelta);
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000D0A0 File Offset: 0x0000B2A0
		public override void ForceCameraPosition(Vector3 pos, Quaternion rot)
		{
			base.ForceCameraPosition(pos, rot);
			this.m_PreviousCameraPosition = pos;
			this.m_prevRotation = rot;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000D0B8 File Offset: 0x0000B2B8
		public override float GetMaxDampTime()
		{
			return Mathf.Max(this.m_XDamping, Mathf.Max(this.m_YDamping, this.m_ZDamping));
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000D0D8 File Offset: 0x0000B2D8
		public override bool OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime, ref CinemachineVirtualCameraBase.TransitionParams transitionParams)
		{
			if (fromCam != null && transitionParams.m_InheritPosition && !CinemachineCore.Instance.IsLiveInBlend(base.VirtualCamera))
			{
				this.m_PreviousCameraPosition = fromCam.State.RawPosition;
				this.m_prevRotation = fromCam.State.RawOrientation;
				this.m_InheritingPosition = true;
				return true;
			}
			return false;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000D130 File Offset: 0x0000B330
		private Rect ScreenToOrtho(Rect rScreen, float orthoSize, float aspect)
		{
			return new Rect
			{
				yMax = 2f * orthoSize * (1f - rScreen.yMin - 0.5f),
				yMin = 2f * orthoSize * (1f - rScreen.yMax - 0.5f),
				xMin = 2f * orthoSize * aspect * (rScreen.xMin - 0.5f),
				xMax = 2f * orthoSize * aspect * (rScreen.xMax - 0.5f)
			};
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000D1C8 File Offset: 0x0000B3C8
		private Vector3 OrthoOffsetToScreenBounds(Vector3 targetPos2D, Rect screenRect)
		{
			Vector3 zero = Vector3.zero;
			if (targetPos2D.x < screenRect.xMin)
			{
				zero.x += targetPos2D.x - screenRect.xMin;
			}
			if (targetPos2D.x > screenRect.xMax)
			{
				zero.x += targetPos2D.x - screenRect.xMax;
			}
			if (targetPos2D.y < screenRect.yMin)
			{
				zero.y += targetPos2D.y - screenRect.yMin;
			}
			if (targetPos2D.y > screenRect.yMax)
			{
				zero.y += targetPos2D.y - screenRect.yMax;
			}
			return zero;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x0000D27C File Offset: 0x0000B47C
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x0000D284 File Offset: 0x0000B484
		public Bounds LastBounds { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000D28D File Offset: 0x0000B48D
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x0000D295 File Offset: 0x0000B495
		public Matrix4x4 LastBoundsMatrix { get; private set; }

		// Token: 0x060001B8 RID: 440 RVA: 0x0000D2A0 File Offset: 0x0000B4A0
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			LensSettings lens = curState.Lens;
			Vector3 vector = base.FollowTargetPosition + base.FollowTargetRotation * this.m_TrackedObjectOffset;
			bool flag = deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid;
			if (!flag || base.VirtualCamera.FollowTargetChanged)
			{
				this.m_Predictor.Reset();
			}
			if (!flag)
			{
				this.m_PreviousCameraPosition = curState.RawPosition;
				this.m_prevFOV = (lens.Orthographic ? lens.OrthographicSize : lens.FieldOfView);
				this.m_prevRotation = curState.RawOrientation;
				if (!this.m_InheritingPosition && this.m_CenterOnActivate)
				{
					this.m_PreviousCameraPosition = base.FollowTargetPosition + curState.RawOrientation * Vector3.back * this.m_CameraDistance;
				}
			}
			if (!this.IsValid)
			{
				this.m_InheritingPosition = false;
				return;
			}
			float fieldOfView = lens.FieldOfView;
			ICinemachineTargetGroup abstractFollowTargetGroup = base.AbstractFollowTargetGroup;
			bool flag2 = abstractFollowTargetGroup != null && this.m_GroupFramingMode != CinemachineFramingTransposer.FramingMode.None && !abstractFollowTargetGroup.IsEmpty;
			if (flag2)
			{
				vector = this.ComputeGroupBounds(abstractFollowTargetGroup, ref curState);
			}
			this.TrackedPoint = vector;
			if (this.m_LookaheadTime > 0.0001f)
			{
				this.m_Predictor.Smoothing = this.m_LookaheadSmoothing;
				this.m_Predictor.AddPosition(vector, deltaTime, this.m_LookaheadTime);
				Vector3 vector2 = this.m_Predictor.PredictPositionDelta(this.m_LookaheadTime);
				if (this.m_LookaheadIgnoreY)
				{
					vector2 = vector2.ProjectOntoPlane(curState.ReferenceUp);
				}
				Vector3 vector3 = vector + vector2;
				if (flag2)
				{
					Bounds lastBounds = this.LastBounds;
					lastBounds.center += this.LastBoundsMatrix.MultiplyPoint3x4(vector2);
					this.LastBounds = lastBounds;
				}
				this.TrackedPoint = vector3;
			}
			if (!curState.HasLookAt)
			{
				curState.ReferenceLookAt = vector;
			}
			float num = this.m_CameraDistance;
			bool orthographic = lens.Orthographic;
			float num2 = (flag2 ? this.GetTargetHeight(this.LastBounds.size / this.m_GroupFramingSize) : 0f);
			num2 = Mathf.Max(num2, 0.01f);
			if (!orthographic && flag2)
			{
				float z = this.LastBounds.extents.z;
				float z2 = this.LastBounds.center.z;
				if (z2 > z)
				{
					num2 = Mathf.Lerp(0f, num2, (z2 - z) / z2);
				}
				if (this.m_AdjustmentMode != CinemachineFramingTransposer.AdjustmentMode.ZoomOnly)
				{
					num = num2 / (2f * Mathf.Tan(fieldOfView * 0.017453292f / 2f));
					num = Mathf.Clamp(num, this.m_MinimumDistance, this.m_MaximumDistance);
					float num3 = num - this.m_CameraDistance;
					num3 = Mathf.Clamp(num3, -this.m_MaxDollyIn, this.m_MaxDollyOut);
					num = this.m_CameraDistance + num3;
				}
			}
			Quaternion rawOrientation = curState.RawOrientation;
			if (flag && this.m_TargetMovementOnly)
			{
				Quaternion quaternion = rawOrientation * Quaternion.Inverse(this.m_prevRotation);
				this.m_PreviousCameraPosition = this.TrackedPoint + quaternion * (this.m_PreviousCameraPosition - this.TrackedPoint);
			}
			this.m_prevRotation = rawOrientation;
			if (flag2)
			{
				if (orthographic)
				{
					num2 = Mathf.Clamp(num2 / 2f, this.m_MinimumOrthoSize, this.m_MaximumOrthoSize);
					if (flag)
					{
						num2 = this.m_prevFOV + base.VirtualCamera.DetachedFollowTargetDamp(num2 - this.m_prevFOV, this.m_ZDamping, deltaTime);
					}
					this.m_prevFOV = num2;
					lens.OrthographicSize = Mathf.Clamp(num2, this.m_MinimumOrthoSize, this.m_MaximumOrthoSize);
					curState.Lens = lens;
				}
				else if (this.m_AdjustmentMode != CinemachineFramingTransposer.AdjustmentMode.DollyOnly)
				{
					float z3 = (Quaternion.Inverse(curState.RawOrientation) * (vector - curState.RawPosition)).z;
					float num4 = 179f;
					if (z3 > 0.0001f)
					{
						num4 = 2f * Mathf.Atan(num2 / (2f * z3)) * 57.29578f;
					}
					num4 = Mathf.Clamp(num4, this.m_MinimumFOV, this.m_MaximumFOV);
					if (flag)
					{
						num4 = this.m_prevFOV + base.VirtualCamera.DetachedFollowTargetDamp(num4 - this.m_prevFOV, this.m_ZDamping, deltaTime);
					}
					this.m_prevFOV = num4;
					lens.FieldOfView = num4;
					curState.Lens = lens;
				}
			}
			Vector3 previousCameraPosition = this.m_PreviousCameraPosition;
			Quaternion quaternion2 = Quaternion.Inverse(rawOrientation);
			Vector3 vector4 = quaternion2 * previousCameraPosition;
			Vector3 vector5 = quaternion2 * this.TrackedPoint - vector4;
			Vector3 vector6 = vector5;
			Vector3 vector7 = Vector3.zero;
			float num5 = Mathf.Max(0.01f, num - this.m_DeadZoneDepth / 2f);
			float num6 = Mathf.Max(num5, num + this.m_DeadZoneDepth / 2f);
			float num7 = Mathf.Min(vector5.z, vector6.z);
			if (num7 < num5)
			{
				vector7.z = num7 - num5;
			}
			if (num7 > num6)
			{
				vector7.z = num7 - num6;
			}
			float num8 = (lens.Orthographic ? lens.OrthographicSize : (Mathf.Tan(0.5f * fieldOfView * 0.017453292f) * (num7 - vector7.z)));
			Rect rect = this.ScreenToOrtho(this.SoftGuideRect, num8, lens.Aspect);
			if (!flag)
			{
				Rect rect2 = rect;
				if (this.m_CenterOnActivate && !this.m_InheritingPosition)
				{
					rect2 = new Rect(rect2.center, Vector2.zero);
				}
				vector7 += this.OrthoOffsetToScreenBounds(vector5, rect2);
			}
			else
			{
				vector7 += this.OrthoOffsetToScreenBounds(vector5, rect);
				vector7 = base.VirtualCamera.DetachedFollowTargetDamp(vector7, new Vector3(this.m_XDamping, this.m_YDamping, this.m_ZDamping), deltaTime);
				if (!this.m_UnlimitedSoftZone && (deltaTime < 0f || base.VirtualCamera.FollowTargetAttachment > 0.9999f))
				{
					Rect rect3 = this.ScreenToOrtho(this.HardGuideRect, num8, lens.Aspect);
					Vector3 vector8 = quaternion2 * vector - vector4;
					vector7 += this.OrthoOffsetToScreenBounds(vector8 - vector7, rect3);
				}
			}
			curState.RawPosition = previousCameraPosition + rawOrientation * vector7;
			this.m_PreviousCameraPosition = curState.RawPosition;
			this.m_InheritingPosition = false;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000D908 File Offset: 0x0000BB08
		private float GetTargetHeight(Vector2 boundsSize)
		{
			CameraState cameraState;
			switch (this.m_GroupFramingMode)
			{
			case CinemachineFramingTransposer.FramingMode.Horizontal:
			{
				float x = boundsSize.x;
				cameraState = base.VcamState;
				return x / cameraState.Lens.Aspect;
			}
			case CinemachineFramingTransposer.FramingMode.Vertical:
				return boundsSize.y;
			}
			float x2 = boundsSize.x;
			cameraState = base.VcamState;
			return Mathf.Max(x2 / cameraState.Lens.Aspect, boundsSize.y);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000D978 File Offset: 0x0000BB78
		private Vector3 ComputeGroupBounds(ICinemachineTargetGroup group, ref CameraState curState)
		{
			Vector3 vector = curState.RawPosition;
			Vector3 vector2 = curState.RawOrientation * Vector3.forward;
			this.LastBoundsMatrix = Matrix4x4.TRS(vector, curState.RawOrientation, Vector3.one);
			Bounds bounds = group.GetViewSpaceBoundingBox(this.LastBoundsMatrix);
			Vector3 vector3 = this.LastBoundsMatrix.MultiplyPoint3x4(bounds.center);
			float z = bounds.extents.z;
			if (!curState.Lens.Orthographic)
			{
				float z2 = (Quaternion.Inverse(curState.RawOrientation) * (vector3 - vector)).z;
				vector = vector3 - vector2 * (Mathf.Max(z2, z) + z);
				bounds = CinemachineFramingTransposer.GetScreenSpaceGroupBoundingBox(group, ref vector, curState.RawOrientation);
				this.LastBoundsMatrix = Matrix4x4.TRS(vector, curState.RawOrientation, Vector3.one);
				vector3 = this.LastBoundsMatrix.MultiplyPoint3x4(bounds.center);
			}
			this.LastBounds = bounds;
			return vector3 - vector2 * z;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000DA7C File Offset: 0x0000BC7C
		private static Bounds GetScreenSpaceGroupBoundingBox(ICinemachineTargetGroup group, ref Vector3 pos, Quaternion orientation)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(pos, orientation, Vector3.one);
			Vector2 vector;
			Vector2 vector2;
			Vector2 vector3;
			group.GetViewSpaceAngularBounds(matrix4x, out vector, out vector2, out vector3);
			Vector2 vector4 = (vector + vector2) / 2f;
			Quaternion quaternion = Quaternion.identity.ApplyCameraRotation(new Vector2(-vector4.x, vector4.y), Vector3.up);
			pos = quaternion * new Vector3(0f, 0f, (vector3.y + vector3.x) / 2f);
			pos.z = 0f;
			pos = matrix4x.MultiplyPoint3x4(pos);
			matrix4x = Matrix4x4.TRS(pos, orientation, Vector3.one);
			group.GetViewSpaceAngularBounds(matrix4x, out vector, out vector2, out vector3);
			float num = vector3.y + vector3.x;
			Vector2 vector5 = new Vector2(89.5f, 89.5f);
			if (vector3.x > 0f)
			{
				vector5 = Vector2.Max(vector2, vector.Abs());
				vector5 = Vector2.Min(vector5, new Vector2(89.5f, 89.5f));
			}
			vector5 *= 0.017453292f;
			return new Bounds(new Vector3(0f, 0f, num / 2f), new Vector3(Mathf.Tan(vector5.y) * num, Mathf.Tan(vector5.x) * num, vector3.y - vector3.x));
		}

		// Token: 0x0400011C RID: 284
		[Tooltip("Offset from the Follow Target object (in target-local co-ordinates).  The camera will attempt to frame the point which is the target's position plus this offset.  Use it to correct for cases when the target's origin is not the point of interest for the camera.")]
		public Vector3 m_TrackedObjectOffset;

		// Token: 0x0400011D RID: 285
		[Tooltip("This setting will instruct the composer to adjust its target offset based on the motion of the target.  The composer will look at a point where it estimates the target will be this many seconds into the future.  Note that this setting is sensitive to noisy animation, and can amplify the noise, resulting in undesirable camera jitter.  If the camera jitters unacceptably when the target is in motion, turn down this setting, or animate the target more smoothly.")]
		[Range(0f, 1f)]
		[Space]
		public float m_LookaheadTime;

		// Token: 0x0400011E RID: 286
		[Tooltip("Controls the smoothness of the lookahead algorithm.  Larger values smooth out jittery predictions and also increase prediction lag")]
		[Range(0f, 30f)]
		public float m_LookaheadSmoothing;

		// Token: 0x0400011F RID: 287
		[Tooltip("If checked, movement along the Y axis will be ignored for lookahead calculations")]
		public bool m_LookaheadIgnoreY;

		// Token: 0x04000120 RID: 288
		[Space]
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the X-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's x-axis offset.  Larger numbers give a more heavy slowly responding camera.  Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_XDamping = 1f;

		// Token: 0x04000121 RID: 289
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the Y-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's y-axis offset.  Larger numbers give a more heavy slowly responding camera.  Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_YDamping = 1f;

		// Token: 0x04000122 RID: 290
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the Z-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's z-axis offset.  Larger numbers give a more heavy slowly responding camera.  Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_ZDamping = 1f;

		// Token: 0x04000123 RID: 291
		[Tooltip("If set, damping will apply  only to target motion, but not to camera rotation changes.  Turn this on to get an instant response when the rotation changes.  ")]
		public bool m_TargetMovementOnly = true;

		// Token: 0x04000124 RID: 292
		[Space]
		[Range(-0.5f, 1.5f)]
		[Tooltip("Horizontal screen position for target. The camera will move to position the tracked object here.")]
		public float m_ScreenX = 0.5f;

		// Token: 0x04000125 RID: 293
		[Range(-0.5f, 1.5f)]
		[Tooltip("Vertical screen position for target, The camera will move to position the tracked object here.")]
		public float m_ScreenY = 0.5f;

		// Token: 0x04000126 RID: 294
		[Tooltip("The distance along the camera axis that will be maintained from the Follow target")]
		public float m_CameraDistance = 10f;

		// Token: 0x04000127 RID: 295
		[Space]
		[Range(0f, 2f)]
		[Tooltip("Camera will not move horizontally if the target is within this range of the position.")]
		public float m_DeadZoneWidth;

		// Token: 0x04000128 RID: 296
		[Range(0f, 2f)]
		[Tooltip("Camera will not move vertically if the target is within this range of the position.")]
		public float m_DeadZoneHeight;

		// Token: 0x04000129 RID: 297
		[Tooltip("The camera will not move along its z-axis if the Follow target is within this distance of the specified camera distance")]
		[FormerlySerializedAs("m_DistanceDeadZoneSize")]
		public float m_DeadZoneDepth;

		// Token: 0x0400012A RID: 298
		[Space]
		[Tooltip("If checked, then then soft zone will be unlimited in size.")]
		public bool m_UnlimitedSoftZone;

		// Token: 0x0400012B RID: 299
		[Range(0f, 2f)]
		[Tooltip("When target is within this region, camera will gradually move horizontally to re-align towards the desired position, depending on the damping speed.")]
		public float m_SoftZoneWidth = 0.8f;

		// Token: 0x0400012C RID: 300
		[Range(0f, 2f)]
		[Tooltip("When target is within this region, camera will gradually move vertically to re-align towards the desired position, depending on the damping speed.")]
		public float m_SoftZoneHeight = 0.8f;

		// Token: 0x0400012D RID: 301
		[Range(-0.5f, 0.5f)]
		[Tooltip("A non-zero bias will move the target position horizontally away from the center of the soft zone.")]
		public float m_BiasX;

		// Token: 0x0400012E RID: 302
		[Range(-0.5f, 0.5f)]
		[Tooltip("A non-zero bias will move the target position vertically away from the center of the soft zone.")]
		public float m_BiasY;

		// Token: 0x0400012F RID: 303
		[Tooltip("Force target to center of screen when this camera activates.  If false, will clamp target to the edges of the dead zone")]
		public bool m_CenterOnActivate = true;

		// Token: 0x04000130 RID: 304
		[Space]
		[Tooltip("What screen dimensions to consider when framing.  Can be Horizontal, Vertical, or both")]
		[FormerlySerializedAs("m_FramingMode")]
		public CinemachineFramingTransposer.FramingMode m_GroupFramingMode = CinemachineFramingTransposer.FramingMode.HorizontalAndVertical;

		// Token: 0x04000131 RID: 305
		[Tooltip("How to adjust the camera to get the desired framing.  You can zoom, dolly in/out, or do both.")]
		public CinemachineFramingTransposer.AdjustmentMode m_AdjustmentMode;

		// Token: 0x04000132 RID: 306
		[Tooltip("The bounding box of the targets should occupy this amount of the screen space.  1 means fill the whole screen.  0.5 means fill half the screen, etc.")]
		public float m_GroupFramingSize = 0.8f;

		// Token: 0x04000133 RID: 307
		[Tooltip("The maximum distance toward the target that this behaviour is allowed to move the camera.")]
		public float m_MaxDollyIn = 5000f;

		// Token: 0x04000134 RID: 308
		[Tooltip("The maximum distance away the target that this behaviour is allowed to move the camera.")]
		public float m_MaxDollyOut = 5000f;

		// Token: 0x04000135 RID: 309
		[Tooltip("Set this to limit how close to the target the camera can get.")]
		public float m_MinimumDistance = 1f;

		// Token: 0x04000136 RID: 310
		[Tooltip("Set this to limit how far from the target the camera can get.")]
		public float m_MaximumDistance = 5000f;

		// Token: 0x04000137 RID: 311
		[Range(1f, 179f)]
		[Tooltip("If adjusting FOV, will not set the FOV lower than this.")]
		public float m_MinimumFOV = 3f;

		// Token: 0x04000138 RID: 312
		[Range(1f, 179f)]
		[Tooltip("If adjusting FOV, will not set the FOV higher than this.")]
		public float m_MaximumFOV = 60f;

		// Token: 0x04000139 RID: 313
		[Tooltip("If adjusting Orthographic Size, will not set it lower than this.")]
		public float m_MinimumOrthoSize = 1f;

		// Token: 0x0400013A RID: 314
		[Tooltip("If adjusting Orthographic Size, will not set it higher than this.")]
		public float m_MaximumOrthoSize = 5000f;

		// Token: 0x0400013B RID: 315
		private const float kMinimumCameraDistance = 0.01f;

		// Token: 0x0400013C RID: 316
		private const float kMinimumGroupSize = 0.01f;

		// Token: 0x0400013D RID: 317
		private Vector3 m_PreviousCameraPosition = Vector3.zero;

		// Token: 0x0400013E RID: 318
		internal PositionPredictor m_Predictor = new PositionPredictor();

		// Token: 0x04000140 RID: 320
		private bool m_InheritingPosition;

		// Token: 0x04000141 RID: 321
		private float m_prevFOV;

		// Token: 0x04000142 RID: 322
		private Quaternion m_prevRotation;

		// Token: 0x02000094 RID: 148
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		public enum FramingMode
		{
			// Token: 0x04000321 RID: 801
			Horizontal,
			// Token: 0x04000322 RID: 802
			Vertical,
			// Token: 0x04000323 RID: 803
			HorizontalAndVertical,
			// Token: 0x04000324 RID: 804
			None
		}

		// Token: 0x02000095 RID: 149
		public enum AdjustmentMode
		{
			// Token: 0x04000326 RID: 806
			ZoomOnly,
			// Token: 0x04000327 RID: 807
			DollyOnly,
			// Token: 0x04000328 RID: 808
			DollyThenZoom
		}
	}
}
