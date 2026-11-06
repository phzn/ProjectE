using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.U2D;

namespace UnityEngine.Experimental.Rendering.Universal
{
	// Token: 0x02000006 RID: 6
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Rendering/2D/Pixel Perfect Camera")]
	[RequireComponent(typeof(Camera))]
	[MovedFrom("UnityEngine.Experimental.Rendering")]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@latest/index.html?subfolder=/manual/2d-pixelperfect.html%23properties")]
	public class PixelPerfectCamera : MonoBehaviour, IPixelPerfectCamera, ISerializationCallbackReceiver
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000223A File Offset: 0x0000043A
		// (set) Token: 0x0600000E RID: 14 RVA: 0x00002242 File Offset: 0x00000442
		public PixelPerfectCamera.CropFrame cropFrame
		{
			get
			{
				return this.m_CropFrame;
			}
			set
			{
				this.m_CropFrame = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000224B File Offset: 0x0000044B
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002253 File Offset: 0x00000453
		public PixelPerfectCamera.GridSnapping gridSnapping
		{
			get
			{
				return this.m_GridSnapping;
			}
			set
			{
				this.m_GridSnapping = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000225C File Offset: 0x0000045C
		public float orthographicSize
		{
			get
			{
				return this.m_Internal.orthoSize;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002269 File Offset: 0x00000469
		// (set) Token: 0x06000013 RID: 19 RVA: 0x00002271 File Offset: 0x00000471
		public int assetsPPU
		{
			get
			{
				return this.m_AssetsPPU;
			}
			set
			{
				this.m_AssetsPPU = ((value > 0) ? value : 1);
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002281 File Offset: 0x00000481
		// (set) Token: 0x06000015 RID: 21 RVA: 0x00002289 File Offset: 0x00000489
		public int refResolutionX
		{
			get
			{
				return this.m_RefResolutionX;
			}
			set
			{
				this.m_RefResolutionX = ((value > 0) ? value : 1);
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002299 File Offset: 0x00000499
		// (set) Token: 0x06000017 RID: 23 RVA: 0x000022A1 File Offset: 0x000004A1
		public int refResolutionY
		{
			get
			{
				return this.m_RefResolutionY;
			}
			set
			{
				this.m_RefResolutionY = ((value > 0) ? value : 1);
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000022B1 File Offset: 0x000004B1
		// (set) Token: 0x06000019 RID: 25 RVA: 0x000022BC File Offset: 0x000004BC
		[Obsolete("Use gridSnapping instead", false)]
		public bool upscaleRT
		{
			get
			{
				return this.m_GridSnapping == PixelPerfectCamera.GridSnapping.UpscaleRenderTexture;
			}
			set
			{
				this.m_GridSnapping = (value ? PixelPerfectCamera.GridSnapping.UpscaleRenderTexture : PixelPerfectCamera.GridSnapping.None);
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001A RID: 26 RVA: 0x000022CB File Offset: 0x000004CB
		// (set) Token: 0x0600001B RID: 27 RVA: 0x000022D6 File Offset: 0x000004D6
		[Obsolete("Use gridSnapping instead", false)]
		public bool pixelSnapping
		{
			get
			{
				return this.m_GridSnapping == PixelPerfectCamera.GridSnapping.PixelSnapping;
			}
			set
			{
				this.m_GridSnapping = (value ? PixelPerfectCamera.GridSnapping.PixelSnapping : PixelPerfectCamera.GridSnapping.None);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001C RID: 28 RVA: 0x000022E5 File Offset: 0x000004E5
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002304 File Offset: 0x00000504
		[Obsolete("Use cropFrame instead", false)]
		public bool cropFrameX
		{
			get
			{
				return this.m_CropFrame == PixelPerfectCamera.CropFrame.StretchFill || this.m_CropFrame == PixelPerfectCamera.CropFrame.Windowbox || this.m_CropFrame == PixelPerfectCamera.CropFrame.Pillarbox;
			}
			set
			{
				if (value)
				{
					if (this.m_CropFrame == PixelPerfectCamera.CropFrame.None)
					{
						this.m_CropFrame = PixelPerfectCamera.CropFrame.Pillarbox;
						return;
					}
					if (this.m_CropFrame == PixelPerfectCamera.CropFrame.Letterbox)
					{
						this.m_CropFrame = PixelPerfectCamera.CropFrame.Windowbox;
						return;
					}
				}
				else
				{
					if (this.m_CropFrame == PixelPerfectCamera.CropFrame.Pillarbox)
					{
						this.m_CropFrame = PixelPerfectCamera.CropFrame.None;
						return;
					}
					if (this.m_CropFrame == PixelPerfectCamera.CropFrame.Windowbox || this.m_CropFrame == PixelPerfectCamera.CropFrame.StretchFill)
					{
						this.m_CropFrame = PixelPerfectCamera.CropFrame.Letterbox;
					}
				}
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000235F File Offset: 0x0000055F
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00002380 File Offset: 0x00000580
		[Obsolete("Use cropFrame instead", false)]
		public bool cropFrameY
		{
			get
			{
				return this.m_CropFrame == PixelPerfectCamera.CropFrame.StretchFill || this.m_CropFrame == PixelPerfectCamera.CropFrame.Windowbox || this.m_CropFrame == PixelPerfectCamera.CropFrame.Letterbox;
			}
			set
			{
				if (value)
				{
					if (this.m_CropFrame == PixelPerfectCamera.CropFrame.None)
					{
						this.m_CropFrame = PixelPerfectCamera.CropFrame.Letterbox;
						return;
					}
					if (this.m_CropFrame == PixelPerfectCamera.CropFrame.Pillarbox)
					{
						this.m_CropFrame = PixelPerfectCamera.CropFrame.Windowbox;
						return;
					}
				}
				else
				{
					if (this.m_CropFrame == PixelPerfectCamera.CropFrame.Letterbox)
					{
						this.m_CropFrame = PixelPerfectCamera.CropFrame.None;
						return;
					}
					if (this.m_CropFrame == PixelPerfectCamera.CropFrame.Windowbox || this.m_CropFrame == PixelPerfectCamera.CropFrame.StretchFill)
					{
						this.m_CropFrame = PixelPerfectCamera.CropFrame.Pillarbox;
					}
				}
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000023DB File Offset: 0x000005DB
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000023E6 File Offset: 0x000005E6
		[Obsolete("Use cropFrame instead", false)]
		public bool stretchFill
		{
			get
			{
				return this.m_CropFrame == PixelPerfectCamera.CropFrame.StretchFill;
			}
			set
			{
				if (value)
				{
					this.m_CropFrame = PixelPerfectCamera.CropFrame.StretchFill;
					return;
				}
				this.m_CropFrame = PixelPerfectCamera.CropFrame.Windowbox;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000023FC File Offset: 0x000005FC
		public int pixelRatio
		{
			get
			{
				if (!this.m_CinemachineCompatibilityMode)
				{
					return this.m_Internal.zoom;
				}
				if (this.m_GridSnapping == PixelPerfectCamera.GridSnapping.UpscaleRenderTexture)
				{
					return this.m_Internal.zoom * this.m_Internal.cinemachineVCamZoom;
				}
				return this.m_Internal.cinemachineVCamZoom;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002449 File Offset: 0x00000649
		public bool requiresUpscalePass
		{
			get
			{
				return this.m_Internal.requiresUpscaling;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002458 File Offset: 0x00000658
		public Vector3 RoundToPixel(Vector3 position)
		{
			float unitsPerPixel = this.m_Internal.unitsPerPixel;
			if (unitsPerPixel == 0f)
			{
				return position;
			}
			Vector3 vector;
			vector.x = Mathf.Round(position.x / unitsPerPixel) * unitsPerPixel;
			vector.y = Mathf.Round(position.y / unitsPerPixel) * unitsPerPixel;
			vector.z = Mathf.Round(position.z / unitsPerPixel) * unitsPerPixel;
			return vector;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000024BE File Offset: 0x000006BE
		public float CorrectCinemachineOrthoSize(float targetOrthoSize)
		{
			this.m_CinemachineCompatibilityMode = true;
			if (this.m_Internal == null)
			{
				return targetOrthoSize;
			}
			return this.m_Internal.CorrectCinemachineOrthoSize(targetOrthoSize);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000024DD File Offset: 0x000006DD
		internal FilterMode finalBlitFilterMode
		{
			get
			{
				if (this.m_FilterMode != PixelPerfectCamera.PixelPerfectFilterMode.RetroAA)
				{
					return FilterMode.Point;
				}
				return FilterMode.Bilinear;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000024EA File Offset: 0x000006EA
		internal Vector2Int offscreenRTSize
		{
			get
			{
				return new Vector2Int(this.m_Internal.offscreenRTWidth, this.m_Internal.offscreenRTHeight);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002508 File Offset: 0x00000708
		private Vector2Int cameraRTSize
		{
			get
			{
				RenderTexture targetTexture = this.m_Camera.targetTexture;
				if (!(targetTexture == null))
				{
					return new Vector2Int(targetTexture.width, targetTexture.height);
				}
				return new Vector2Int(Screen.width, Screen.height);
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x0000254C File Offset: 0x0000074C
		private void PixelSnap()
		{
			Vector3 position = this.m_Camera.transform.position;
			Vector3 vector = this.RoundToPixel(position) - position;
			vector.z = -vector.z;
			Matrix4x4 inverse = Matrix4x4.TRS(position + vector, Quaternion.identity, Vector3.one).inverse;
			Matrix4x4 inverse2 = Matrix4x4.Rotate(this.m_Camera.transform.rotation).inverse;
			Matrix4x4 matrix4x = Matrix4x4.Scale(new Vector3(1f, 1f, -1f));
			this.m_Camera.worldToCameraMatrix = matrix4x * inverse2 * inverse;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000025F9 File Offset: 0x000007F9
		private void Awake()
		{
			this.m_Camera = base.GetComponent<Camera>();
			this.m_Internal = new PixelPerfectCameraInternal(this);
			this.UpdateCameraProperties();
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000261C File Offset: 0x0000081C
		private void UpdateCameraProperties()
		{
			Vector2Int cameraRTSize = this.cameraRTSize;
			this.m_Internal.CalculateCameraProperties(cameraRTSize.x, cameraRTSize.y);
			if (this.m_Internal.useOffscreenRT)
			{
				this.m_Camera.pixelRect = this.m_Internal.CalculateFinalBlitPixelRect(cameraRTSize.x, cameraRTSize.y);
				return;
			}
			this.m_Camera.rect = new Rect(0f, 0f, 1f, 1f);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x000026A0 File Offset: 0x000008A0
		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (camera == this.m_Camera)
			{
				this.UpdateCameraProperties();
				this.PixelSnap();
				if (!this.m_CinemachineCompatibilityMode)
				{
					this.m_Camera.orthographicSize = this.m_Internal.orthoSize;
				}
				PixelPerfectRendering.pixelSnapSpacing = this.m_Internal.unitsPerPixel;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000026F5 File Offset: 0x000008F5
		private void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (camera == this.m_Camera)
			{
				PixelPerfectRendering.pixelSnapSpacing = 0f;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000270F File Offset: 0x0000090F
		private void OnEnable()
		{
			this.m_CinemachineCompatibilityMode = false;
			RenderPipelineManager.beginCameraRendering += this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering += this.OnEndCameraRendering;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000273C File Offset: 0x0000093C
		internal void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= this.OnBeginCameraRendering;
			RenderPipelineManager.endCameraRendering -= this.OnEndCameraRendering;
			this.m_Camera.rect = new Rect(0f, 0f, 1f, 1f);
			this.m_Camera.ResetWorldToCameraMatrix();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000279A File Offset: 0x0000099A
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000279C File Offset: 0x0000099C
		public void OnAfterDeserialize()
		{
		}

		// Token: 0x0400000A RID: 10
		[SerializeField]
		private int m_AssetsPPU = 100;

		// Token: 0x0400000B RID: 11
		[SerializeField]
		private int m_RefResolutionX = 320;

		// Token: 0x0400000C RID: 12
		[SerializeField]
		private int m_RefResolutionY = 180;

		// Token: 0x0400000D RID: 13
		[SerializeField]
		private PixelPerfectCamera.CropFrame m_CropFrame;

		// Token: 0x0400000E RID: 14
		[SerializeField]
		private PixelPerfectCamera.GridSnapping m_GridSnapping;

		// Token: 0x0400000F RID: 15
		[SerializeField]
		private PixelPerfectCamera.PixelPerfectFilterMode m_FilterMode;

		// Token: 0x04000010 RID: 16
		private Camera m_Camera;

		// Token: 0x04000011 RID: 17
		private PixelPerfectCameraInternal m_Internal;

		// Token: 0x04000012 RID: 18
		private bool m_CinemachineCompatibilityMode;

		// Token: 0x02000146 RID: 326
		public enum CropFrame
		{
			// Token: 0x0400093E RID: 2366
			None,
			// Token: 0x0400093F RID: 2367
			Pillarbox,
			// Token: 0x04000940 RID: 2368
			Letterbox,
			// Token: 0x04000941 RID: 2369
			Windowbox,
			// Token: 0x04000942 RID: 2370
			StretchFill
		}

		// Token: 0x02000147 RID: 327
		public enum GridSnapping
		{
			// Token: 0x04000944 RID: 2372
			None,
			// Token: 0x04000945 RID: 2373
			PixelSnapping,
			// Token: 0x04000946 RID: 2374
			UpscaleRenderTexture
		}

		// Token: 0x02000148 RID: 328
		public enum PixelPerfectFilterMode
		{
			// Token: 0x04000948 RID: 2376
			RetroAA,
			// Token: 0x04000949 RID: 2377
			Point
		}

		// Token: 0x02000149 RID: 329
		private enum ComponentVersions
		{
			// Token: 0x0400094B RID: 2379
			Version_Unserialized,
			// Token: 0x0400094C RID: 2380
			Version_1
		}
	}
}
