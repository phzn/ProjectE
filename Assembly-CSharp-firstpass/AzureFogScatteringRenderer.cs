using System;

namespace UnityEngine.AzureSky
{
	// Token: 0x02000014 RID: 20
	[ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Azure[Sky] Dynamic Skybox/Azure Fog Scattering Renderer")]
	public sealed class AzureFogScatteringRenderer : MonoBehaviour
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002C4F File Offset: 0x00000E4F
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002C57 File Offset: 0x00000E57
		public Camera camera
		{
			get
			{
				return this.m_camera;
			}
			set
			{
				this.m_camera = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002C60 File Offset: 0x00000E60
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002C68 File Offset: 0x00000E68
		public Material fogRendererMaterial
		{
			get
			{
				return this.m_fogRendererMaterial;
			}
			set
			{
				this.m_fogRendererMaterial = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002C71 File Offset: 0x00000E71
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002C79 File Offset: 0x00000E79
		public Material fogComputationMaterial
		{
			get
			{
				return this.m_fogComputationMaterial;
			}
			set
			{
				this.m_fogComputationMaterial = value;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002C82 File Offset: 0x00000E82
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002C8A File Offset: 0x00000E8A
		public RenderTexture fogScatteringRT
		{
			get
			{
				return this.m_fogScatteringRT;
			}
			set
			{
				this.m_fogScatteringRT = value;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002C93 File Offset: 0x00000E93
		public int fogRenderTextureWidth
		{
			get
			{
				return this.m_fogRenderTextureWidth;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002C9B File Offset: 0x00000E9B
		public int fogRenderTextureHeight
		{
			get
			{
				return this.m_fogRenderTextureHeight;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002CA3 File Offset: 0x00000EA3
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00002CAB File Offset: 0x00000EAB
		public float renderScale
		{
			get
			{
				return this.m_renderScale;
			}
			set
			{
				this.m_renderScale = Mathf.Clamp(value, 0.25f, 1f);
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002CC3 File Offset: 0x00000EC3
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00002CCB File Offset: 0x00000ECB
		public int updateEveryNFrames
		{
			get
			{
				return this.m_updateEveryNFrames;
			}
			set
			{
				this.m_updateEveryNFrames = Mathf.Max(1, value);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00002CDA File Offset: 0x00000EDA
		private void Start()
		{
			this.m_camera = base.GetComponent<Camera>();
			this.SetFogScatteringResolution(this.m_fogRenderTextureWidth, this.m_fogRenderTextureHeight);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002CFA File Offset: 0x00000EFA
		private void OnEnable()
		{
			this.m_camera = base.GetComponent<Camera>();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002D08 File Offset: 0x00000F08
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			this.m_camera.depthTextureMode |= DepthTextureMode.Depth;
			int num = Mathf.Max(1, Mathf.RoundToInt((float)source.width * this.m_renderScale));
			int num2 = Mathf.Max(1, Mathf.RoundToInt((float)source.height * this.m_renderScale));
			if (this.m_fogScatteringRT == null || this.m_lastSourceWidth != num || this.m_lastSourceHeight != num2)
			{
				this.SetFogScatteringResolution(num, num2);
				this.m_lastSourceWidth = num;
				this.m_lastSourceHeight = num2;
			}
			if (this.m_fogComputationMaterial == null || this.m_fogRendererMaterial == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.m_camera.CalculateFrustumCorners(this.m_viewRect, this.m_camera.farClipPlane, this.m_camera.stereoActiveEye, this.m_frustumCorners);
			this.m_frustumCornersArray = Matrix4x4.identity;
			this.m_frustumCornersArray.SetRow(0, this.m_camera.transform.TransformVector(this.m_frustumCorners[0]));
			this.m_frustumCornersArray.SetRow(2, this.m_camera.transform.TransformVector(this.m_frustumCorners[1]));
			this.m_frustumCornersArray.SetRow(3, this.m_camera.transform.TransformVector(this.m_frustumCorners[2]));
			this.m_frustumCornersArray.SetRow(1, this.m_camera.transform.TransformVector(this.m_frustumCorners[3]));
			this.m_fogComputationMaterial.SetMatrix(AzureShaderUniforms.FrustumCornersMatrix, this.m_frustumCornersArray);
			this.m_fogRendererMaterial.SetMatrix(AzureShaderUniforms.FrustumCornersMatrix, this.m_frustumCornersArray);
			if (this.m_updateEveryNFrames <= 1 || Time.frameCount % this.m_updateEveryNFrames == 0)
			{
				Graphics.Blit(null, this.m_fogScatteringRT, this.m_fogComputationMaterial, 0);
			}
			this.m_fogRendererMaterial.SetTexture(AzureShaderUniforms.FogScatteringDataTexture, this.m_fogScatteringRT);
			Graphics.Blit(source, destination, this.m_fogRendererMaterial, 0);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002F24 File Offset: 0x00001124
		public void SetFogScatteringResolution(int width, int height)
		{
			this.m_fogRenderTextureWidth = width;
			this.m_fogRenderTextureHeight = height;
			if (this.m_fogScatteringRT != null)
			{
				this.m_fogScatteringRT.Release();
				Object.DestroyImmediate(this.m_fogScatteringRT);
			}
			this.m_fogScatteringRT = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default)
			{
				name = "Fog Scattering RT",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			this.m_fogScatteringRT.Create();
		}

		// Token: 0x0400005F RID: 95
		private Camera m_camera;

		// Token: 0x04000060 RID: 96
		[SerializeField]
		private Material m_fogRendererMaterial;

		// Token: 0x04000061 RID: 97
		[SerializeField]
		private Material m_fogComputationMaterial;

		// Token: 0x04000062 RID: 98
		[SerializeField]
		private RenderTexture m_fogScatteringRT;

		// Token: 0x04000063 RID: 99
		[SerializeField]
		private int m_fogRenderTextureWidth = 1920;

		// Token: 0x04000064 RID: 100
		[SerializeField]
		private int m_fogRenderTextureHeight = 1080;

		// Token: 0x04000065 RID: 101
		[SerializeField]
		[Range(0.25f, 1f)]
		private float m_renderScale = 0.5f;

		// Token: 0x04000066 RID: 102
		[SerializeField]
		[Min(1f)]
		private int m_updateEveryNFrames = 2;

		// Token: 0x04000067 RID: 103
		private int m_lastSourceWidth;

		// Token: 0x04000068 RID: 104
		private int m_lastSourceHeight;

		// Token: 0x04000069 RID: 105
		private Vector3[] m_frustumCorners = new Vector3[4];

		// Token: 0x0400006A RID: 106
		private Rect m_viewRect = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x0400006B RID: 107
		private Matrix4x4 m_frustumCornersArray = Matrix4x4.identity;
	}
}
