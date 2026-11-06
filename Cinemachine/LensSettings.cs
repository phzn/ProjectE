using System;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x02000048 RID: 72
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[Serializable]
	public struct LensSettings
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0001437A File Offset: 0x0001257A
		// (set) Token: 0x06000332 RID: 818 RVA: 0x00014397 File Offset: 0x00012597
		public bool Orthographic
		{
			get
			{
				return this.ModeOverride == LensSettings.OverrideModes.Orthographic || (this.ModeOverride == LensSettings.OverrideModes.None && this.m_OrthoFromCamera);
			}
			set
			{
				this.m_OrthoFromCamera = value;
				this.ModeOverride = (value ? LensSettings.OverrideModes.Orthographic : LensSettings.OverrideModes.Perspective);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000333 RID: 819 RVA: 0x000143AD File Offset: 0x000125AD
		// (set) Token: 0x06000334 RID: 820 RVA: 0x000143B5 File Offset: 0x000125B5
		public Vector2 SensorSize
		{
			get
			{
				return this.m_SensorSize;
			}
			set
			{
				this.m_SensorSize = value;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000335 RID: 821 RVA: 0x000143BE File Offset: 0x000125BE
		public float Aspect
		{
			get
			{
				if (this.SensorSize.y != 0f)
				{
					return this.SensorSize.x / this.SensorSize.y;
				}
				return 1f;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000336 RID: 822 RVA: 0x000143EF File Offset: 0x000125EF
		// (set) Token: 0x06000337 RID: 823 RVA: 0x0001440C File Offset: 0x0001260C
		public bool IsPhysicalCamera
		{
			get
			{
				return this.ModeOverride == LensSettings.OverrideModes.Physical || (this.ModeOverride == LensSettings.OverrideModes.None && this.m_PhysicalFromCamera);
			}
			set
			{
				this.m_PhysicalFromCamera = value;
				this.ModeOverride = (value ? LensSettings.OverrideModes.Physical : LensSettings.OverrideModes.Perspective);
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00014424 File Offset: 0x00012624
		public static LensSettings FromCamera(Camera fromCamera)
		{
			LensSettings @default = LensSettings.Default;
			if (fromCamera != null)
			{
				@default.FieldOfView = fromCamera.fieldOfView;
				@default.OrthographicSize = fromCamera.orthographicSize;
				@default.NearClipPlane = fromCamera.nearClipPlane;
				@default.FarClipPlane = fromCamera.farClipPlane;
				@default.LensShift = fromCamera.lensShift;
				@default.GateFit = fromCamera.gateFit;
				@default.FocusDistance = fromCamera.focusDistance;
				@default.SnapshotCameraReadOnlyProperties(fromCamera);
			}
			return @default;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000144A4 File Offset: 0x000126A4
		public void SnapshotCameraReadOnlyProperties(Camera camera)
		{
			this.m_OrthoFromCamera = false;
			this.m_PhysicalFromCamera = false;
			if (camera != null && this.ModeOverride == LensSettings.OverrideModes.None)
			{
				this.m_OrthoFromCamera = camera.orthographic;
				this.m_PhysicalFromCamera = camera.usePhysicalProperties;
				this.m_SensorSize = camera.sensorSize;
				this.GateFit = camera.gateFit;
			}
			if (this.IsPhysicalCamera)
			{
				if (camera != null && this.m_SensorSize == Vector2.zero)
				{
					this.m_SensorSize = camera.sensorSize;
					this.GateFit = camera.gateFit;
					return;
				}
			}
			else
			{
				if (camera != null)
				{
					this.m_SensorSize = new Vector2(camera.aspect, 1f);
				}
				this.LensShift = Vector2.zero;
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00014568 File Offset: 0x00012768
		public void SnapshotCameraReadOnlyProperties(ref LensSettings lens)
		{
			if (this.ModeOverride == LensSettings.OverrideModes.None)
			{
				this.m_OrthoFromCamera = lens.Orthographic;
				this.m_SensorSize = lens.m_SensorSize;
				this.m_PhysicalFromCamera = lens.IsPhysicalCamera;
			}
			if (!this.IsPhysicalCamera)
			{
				this.LensShift = Vector2.zero;
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000145B4 File Offset: 0x000127B4
		public LensSettings(float verticalFOV, float orthographicSize, float nearClip, float farClip, float dutch)
		{
			this = default(LensSettings);
			this.FieldOfView = verticalFOV;
			this.OrthographicSize = orthographicSize;
			this.NearClipPlane = nearClip;
			this.FarClipPlane = farClip;
			this.Dutch = dutch;
			this.m_SensorSize = new Vector2(1f, 1f);
			this.GateFit = Camera.GateFitMode.Horizontal;
			this.FocusDistance = 10f;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x00014614 File Offset: 0x00012814
		public static LensSettings Lerp(LensSettings lensA, LensSettings lensB, float t)
		{
			t = Mathf.Clamp01(t);
			LensSettings lensSettings = ((t < 0.5f) ? lensA : lensB);
			lensSettings.FarClipPlane = Mathf.Lerp(lensA.FarClipPlane, lensB.FarClipPlane, t);
			lensSettings.NearClipPlane = Mathf.Lerp(lensA.NearClipPlane, lensB.NearClipPlane, t);
			lensSettings.FieldOfView = Mathf.Lerp(lensA.FieldOfView, lensB.FieldOfView, t);
			lensSettings.OrthographicSize = Mathf.Lerp(lensA.OrthographicSize, lensB.OrthographicSize, t);
			lensSettings.Dutch = Mathf.Lerp(lensA.Dutch, lensB.Dutch, t);
			lensSettings.m_SensorSize = Vector2.Lerp(lensA.m_SensorSize, lensB.m_SensorSize, t);
			lensSettings.LensShift = Vector2.Lerp(lensA.LensShift, lensB.LensShift, t);
			lensSettings.FocusDistance = Mathf.Lerp(lensA.FocusDistance, lensB.FocusDistance, t);
			return lensSettings;
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00014700 File Offset: 0x00012900
		public void Validate()
		{
			this.FarClipPlane = Mathf.Max(this.FarClipPlane, this.NearClipPlane + 0.001f);
			this.FieldOfView = Mathf.Clamp(this.FieldOfView, 0.01f, 179f);
			this.m_SensorSize.x = Mathf.Max(this.m_SensorSize.x, 0.1f);
			this.m_SensorSize.y = Mathf.Max(this.m_SensorSize.y, 0.1f);
			this.FocusDistance = Mathf.Max(this.FocusDistance, 0.01f);
		}

		// Token: 0x0400021D RID: 541
		public static LensSettings Default = new LensSettings(40f, 10f, 0.1f, 5000f, 0f);

		// Token: 0x0400021E RID: 542
		[Range(1f, 179f)]
		[Tooltip("This is the camera view in degrees. Display will be in vertical degress, unless the associated camera has its FOV axis setting set to Horizontal, in which case display will be in horizontal degress.  Internally, it is always vertical degrees.  For cinematic people, a 50mm lens on a super-35mm sensor would equal a 19.6 degree FOV")]
		public float FieldOfView;

		// Token: 0x0400021F RID: 543
		[Tooltip("When using an orthographic camera, this defines the half-height, in world coordinates, of the camera view.")]
		public float OrthographicSize;

		// Token: 0x04000220 RID: 544
		[Tooltip("This defines the near region in the renderable range of the camera frustum. Raising this value will stop the game from drawing things near the camera, which can sometimes come in handy.  Larger values will also increase your shadow resolution.")]
		public float NearClipPlane;

		// Token: 0x04000221 RID: 545
		[Tooltip("This defines the far region of the renderable range of the camera frustum. Typically you want to set this value as low as possible without cutting off desired distant objects")]
		public float FarClipPlane;

		// Token: 0x04000222 RID: 546
		[Range(-180f, 180f)]
		[Tooltip("Camera Z roll, or tilt, in degrees.")]
		public float Dutch;

		// Token: 0x04000223 RID: 547
		[Tooltip("Allows you to select a different camera mode to apply to the Camera component when Cinemachine activates this Virtual Camera.  The changes applied to the Camera component through this setting will remain after the Virtual Camera deactivation.")]
		public LensSettings.OverrideModes ModeOverride;

		// Token: 0x04000224 RID: 548
		public Vector2 LensShift;

		// Token: 0x04000225 RID: 549
		public Camera.GateFitMode GateFit;

		// Token: 0x04000226 RID: 550
		public float FocusDistance;

		// Token: 0x04000227 RID: 551
		[SerializeField]
		private Vector2 m_SensorSize;

		// Token: 0x04000228 RID: 552
		private bool m_OrthoFromCamera;

		// Token: 0x04000229 RID: 553
		private bool m_PhysicalFromCamera;

		// Token: 0x020000B9 RID: 185
		public enum OverrideModes
		{
			// Token: 0x040003B7 RID: 951
			None,
			// Token: 0x040003B8 RID: 952
			Orthographic,
			// Token: 0x040003B9 RID: 953
			Perspective,
			// Token: 0x040003BA RID: 954
			Physical
		}
	}
}
