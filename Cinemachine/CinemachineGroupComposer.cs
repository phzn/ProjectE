using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000024 RID: 36
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("")]
	[SaveDuringPlay]
	public class CinemachineGroupComposer : CinemachineComposer
	{
		// Token: 0x060001BD RID: 445 RVA: 0x0000DCF8 File Offset: 0x0000BEF8
		private void OnValidate()
		{
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

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001BE RID: 446 RVA: 0x0000DDD3 File Offset: 0x0000BFD3
		// (set) Token: 0x060001BF RID: 447 RVA: 0x0000DDDB File Offset: 0x0000BFDB
		public Bounds LastBounds { get; private set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000DDE4 File Offset: 0x0000BFE4
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x0000DDEC File Offset: 0x0000BFEC
		public Matrix4x4 LastBoundsMatrix { get; private set; }

		// Token: 0x060001C2 RID: 450 RVA: 0x0000DDF5 File Offset: 0x0000BFF5
		public override float GetMaxDampTime()
		{
			return Mathf.Max(base.GetMaxDampTime(), this.m_FrameDamping);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000DE08 File Offset: 0x0000C008
		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
			ICinemachineTargetGroup abstractLookAtTargetGroup = base.AbstractLookAtTargetGroup;
			if (abstractLookAtTargetGroup == null)
			{
				base.MutateCameraState(ref curState, deltaTime);
				return;
			}
			if (!this.IsValid || !curState.HasLookAt)
			{
				this.m_prevFramingDistance = 0f;
				this.m_prevFOV = 0f;
				return;
			}
			bool orthographic = curState.Lens.Orthographic;
			bool flag = !orthographic && this.m_AdjustmentMode > CinemachineGroupComposer.AdjustmentMode.ZoomOnly;
			Vector3 referenceUp = curState.ReferenceUp;
			Vector3 vector = curState.RawPosition;
			Vector3 vector2 = abstractLookAtTargetGroup.Sphere.position;
			Vector3 vector3 = vector2 - vector;
			float magnitude = vector3.magnitude;
			if (magnitude < 0.0001f)
			{
				return;
			}
			vector3 /= magnitude;
			this.LastBoundsMatrix = Matrix4x4.TRS(vector, Quaternion.LookRotation(vector3, referenceUp), Vector3.one);
			Bounds bounds;
			if (orthographic)
			{
				bounds = abstractLookAtTargetGroup.GetViewSpaceBoundingBox(this.LastBoundsMatrix);
				vector2 = this.LastBoundsMatrix.MultiplyPoint3x4(bounds.center);
				vector3 = (vector2 - vector).normalized;
				this.LastBoundsMatrix = Matrix4x4.TRS(vector, Quaternion.LookRotation(vector3, referenceUp), Vector3.one);
				bounds = abstractLookAtTargetGroup.GetViewSpaceBoundingBox(this.LastBoundsMatrix);
				this.LastBounds = bounds;
			}
			else
			{
				bounds = CinemachineGroupComposer.GetScreenSpaceGroupBoundingBox(abstractLookAtTargetGroup, this.LastBoundsMatrix, out vector3);
				this.LastBoundsMatrix = Matrix4x4.TRS(vector, Quaternion.LookRotation(vector3, referenceUp), Vector3.one);
				this.LastBounds = bounds;
				vector2 = vector + vector3 * bounds.center.z;
			}
			float z = bounds.extents.z;
			float num = this.GetTargetHeight(bounds.size / this.m_GroupFramingSize);
			if (orthographic)
			{
				num = Mathf.Clamp(num / 2f, this.m_MinimumOrthoSize, this.m_MaximumOrthoSize);
				if (deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
				{
					num = this.m_prevFOV + base.VirtualCamera.DetachedLookAtTargetDamp(num - this.m_prevFOV, this.m_FrameDamping, deltaTime);
				}
				this.m_prevFOV = num;
				LensSettings lens = curState.Lens;
				lens.OrthographicSize = Mathf.Clamp(num, this.m_MinimumOrthoSize, this.m_MaximumOrthoSize);
				curState.Lens = lens;
			}
			else
			{
				float z2 = bounds.center.z;
				if (z2 > z)
				{
					num = Mathf.Lerp(0f, num, (z2 - z) / z2);
				}
				if (flag)
				{
					float num2 = Mathf.Clamp(z + num / (2f * Mathf.Tan(curState.Lens.FieldOfView * 0.017453292f / 2f)), z + this.m_MinimumDistance, z + this.m_MaximumDistance) - Vector3.Distance(curState.RawPosition, vector2);
					num2 = Mathf.Clamp(num2, -this.m_MaxDollyIn, this.m_MaxDollyOut);
					if (deltaTime >= 0f && base.VirtualCamera.PreviousStateIsValid)
					{
						float num3 = num2 - this.m_prevFramingDistance;
						num3 = base.VirtualCamera.DetachedLookAtTargetDamp(num3, this.m_FrameDamping, deltaTime);
						num2 = this.m_prevFramingDistance + num3;
					}
					this.m_prevFramingDistance = num2;
					curState.PositionCorrection -= vector3 * num2;
					vector -= vector3 * num2;
				}
				if (this.m_AdjustmentMode != CinemachineGroupComposer.AdjustmentMode.DollyOnly)
				{
					float num4 = (vector2 - vector).magnitude - z;
					float num5 = 179f;
					if (num4 > 0.0001f)
					{
						num5 = 2f * Mathf.Atan(num / (2f * num4)) * 57.29578f;
					}
					num5 = Mathf.Clamp(num5, this.m_MinimumFOV, this.m_MaximumFOV);
					if (deltaTime >= 0f && this.m_prevFOV != 0f && base.VirtualCamera.PreviousStateIsValid)
					{
						num5 = this.m_prevFOV + base.VirtualCamera.DetachedLookAtTargetDamp(num5 - this.m_prevFOV, this.m_FrameDamping, deltaTime);
					}
					this.m_prevFOV = num5;
					LensSettings lens2 = curState.Lens;
					lens2.FieldOfView = num5;
					curState.Lens = lens2;
				}
			}
			curState.ReferenceLookAt = this.GetLookAtPointAndSetTrackedPoint(vector2, curState.ReferenceUp, deltaTime);
			base.MutateCameraState(ref curState, deltaTime);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000E240 File Offset: 0x0000C440
		private float GetTargetHeight(Vector2 boundsSize)
		{
			CameraState cameraState;
			switch (this.m_FramingMode)
			{
			case CinemachineGroupComposer.FramingMode.Horizontal:
			{
				float num = Mathf.Max(0.0001f, boundsSize.x);
				cameraState = base.VcamState;
				return num / cameraState.Lens.Aspect;
			}
			case CinemachineGroupComposer.FramingMode.Vertical:
				return Mathf.Max(0.0001f, boundsSize.y);
			}
			float num2 = Mathf.Max(0.0001f, boundsSize.x);
			cameraState = base.VcamState;
			return Mathf.Max(num2 / cameraState.Lens.Aspect, Mathf.Max(0.0001f, boundsSize.y));
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x0000E2D8 File Offset: 0x0000C4D8
		private static Bounds GetScreenSpaceGroupBoundingBox(ICinemachineTargetGroup group, Matrix4x4 observer, out Vector3 newFwd)
		{
			Vector2 vector;
			Vector2 vector2;
			Vector2 vector3;
			group.GetViewSpaceAngularBounds(observer, out vector, out vector2, out vector3);
			Vector2 vector4 = (vector + vector2) / 2f;
			newFwd = Quaternion.identity.ApplyCameraRotation(new Vector2(-vector4.x, vector4.y), Vector3.up) * Vector3.forward;
			newFwd = observer.MultiplyVector(newFwd);
			float num = vector3.y + vector3.x;
			Vector2 vector5 = Vector2.Min(vector2 - vector4, new Vector2(89.5f, 89.5f)) * 0.017453292f;
			return new Bounds(new Vector3(0f, 0f, num / 2f), new Vector3(Mathf.Tan(vector5.y) * num, Mathf.Tan(vector5.x) * num, vector3.y - vector3.x));
		}

		// Token: 0x04000145 RID: 325
		[Space]
		[Tooltip("The bounding box of the targets should occupy this amount of the screen space.  1 means fill the whole screen.  0.5 means fill half the screen, etc.")]
		public float m_GroupFramingSize = 0.8f;

		// Token: 0x04000146 RID: 326
		[Tooltip("What screen dimensions to consider when framing.  Can be Horizontal, Vertical, or both")]
		public CinemachineGroupComposer.FramingMode m_FramingMode = CinemachineGroupComposer.FramingMode.HorizontalAndVertical;

		// Token: 0x04000147 RID: 327
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to frame the group. Small numbers are more responsive, rapidly adjusting the camera to keep the group in the frame.  Larger numbers give a more heavy slowly responding camera.")]
		public float m_FrameDamping = 2f;

		// Token: 0x04000148 RID: 328
		[Tooltip("How to adjust the camera to get the desired framing.  You can zoom, dolly in/out, or do both.")]
		public CinemachineGroupComposer.AdjustmentMode m_AdjustmentMode;

		// Token: 0x04000149 RID: 329
		[Tooltip("The maximum distance toward the target that this behaviour is allowed to move the camera.")]
		public float m_MaxDollyIn = 5000f;

		// Token: 0x0400014A RID: 330
		[Tooltip("The maximum distance away the target that this behaviour is allowed to move the camera.")]
		public float m_MaxDollyOut = 5000f;

		// Token: 0x0400014B RID: 331
		[Tooltip("Set this to limit how close to the target the camera can get.")]
		public float m_MinimumDistance = 1f;

		// Token: 0x0400014C RID: 332
		[Tooltip("Set this to limit how far from the target the camera can get.")]
		public float m_MaximumDistance = 5000f;

		// Token: 0x0400014D RID: 333
		[Range(1f, 179f)]
		[Tooltip("If adjusting FOV, will not set the FOV lower than this.")]
		public float m_MinimumFOV = 3f;

		// Token: 0x0400014E RID: 334
		[Range(1f, 179f)]
		[Tooltip("If adjusting FOV, will not set the FOV higher than this.")]
		public float m_MaximumFOV = 60f;

		// Token: 0x0400014F RID: 335
		[Tooltip("If adjusting Orthographic Size, will not set it lower than this.")]
		public float m_MinimumOrthoSize = 1f;

		// Token: 0x04000150 RID: 336
		[Tooltip("If adjusting Orthographic Size, will not set it higher than this.")]
		public float m_MaximumOrthoSize = 5000f;

		// Token: 0x04000151 RID: 337
		private float m_prevFramingDistance;

		// Token: 0x04000152 RID: 338
		private float m_prevFOV;

		// Token: 0x02000096 RID: 150
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		public enum FramingMode
		{
			// Token: 0x0400032A RID: 810
			Horizontal,
			// Token: 0x0400032B RID: 811
			Vertical,
			// Token: 0x0400032C RID: 812
			HorizontalAndVertical
		}

		// Token: 0x02000097 RID: 151
		public enum AdjustmentMode
		{
			// Token: 0x0400032E RID: 814
			ZoomOnly,
			// Token: 0x0400032F RID: 815
			DollyOnly,
			// Token: 0x04000330 RID: 816
			DollyThenZoom
		}
	}
}
