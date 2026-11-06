using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x0200000B RID: 11
	public class XRPass
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00003589 File Offset: 0x00001789
		public XRPass()
		{
			this.m_Views = new List<XRView>(2);
			this.m_OcclusionMesh = new XROcclusionMesh(this);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000035A9 File Offset: 0x000017A9
		public static XRPass CreateDefault(XRPassCreateInfo createInfo)
		{
			XRPass xrpass = GenericPool<XRPass>.Get();
			xrpass.InitBase(createInfo);
			return xrpass;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000035B7 File Offset: 0x000017B7
		public virtual void Release()
		{
			GenericPool<XRPass>.Release(this);
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000035BF File Offset: 0x000017BF
		public bool enabled
		{
			get
			{
				return this.viewCount > 0;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000035CA File Offset: 0x000017CA
		public bool supportsFoveatedRendering
		{
			get
			{
				return this.enabled && this.foveatedRenderingInfo != IntPtr.Zero && XRSystem.foveatedRenderingCaps > FoveatedRenderingCaps.None;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000037 RID: 55 RVA: 0x000035F0 File Offset: 0x000017F0
		// (set) Token: 0x06000038 RID: 56 RVA: 0x000035F8 File Offset: 0x000017F8
		public bool copyDepth { get; private set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00003601 File Offset: 0x00001801
		// (set) Token: 0x0600003A RID: 58 RVA: 0x00003609 File Offset: 0x00001809
		public int multipassId { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00003612 File Offset: 0x00001812
		// (set) Token: 0x0600003C RID: 60 RVA: 0x0000361A File Offset: 0x0000181A
		public int cullingPassId { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003623 File Offset: 0x00001823
		// (set) Token: 0x0600003E RID: 62 RVA: 0x0000362B File Offset: 0x0000182B
		public RenderTargetIdentifier renderTarget { get; private set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00003634 File Offset: 0x00001834
		// (set) Token: 0x06000040 RID: 64 RVA: 0x0000363C File Offset: 0x0000183C
		public RenderTextureDescriptor renderTargetDesc { get; private set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00003645 File Offset: 0x00001845
		// (set) Token: 0x06000042 RID: 66 RVA: 0x0000364D File Offset: 0x0000184D
		public ScriptableCullingParameters cullingParams { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00003656 File Offset: 0x00001856
		public int viewCount
		{
			get
			{
				return this.m_Views.Count;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003663 File Offset: 0x00001863
		public bool singlePassEnabled
		{
			get
			{
				return this.viewCount > 1;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000045 RID: 69 RVA: 0x0000366E File Offset: 0x0000186E
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00003676 File Offset: 0x00001876
		public IntPtr foveatedRenderingInfo { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000047 RID: 71 RVA: 0x0000367F File Offset: 0x0000187F
		public bool isHDRDisplayOutputActive
		{
			get
			{
				HDROutputSettings hdrOutputSettings = XRSystem.GetActiveDisplay().hdrOutputSettings;
				return hdrOutputSettings != null && hdrOutputSettings.active;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003696 File Offset: 0x00001896
		public ColorGamut hdrDisplayOutputColorGamut
		{
			get
			{
				HDROutputSettings hdrOutputSettings = XRSystem.GetActiveDisplay().hdrOutputSettings;
				if (hdrOutputSettings == null)
				{
					return ColorGamut.sRGB;
				}
				return hdrOutputSettings.displayColorGamut;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000049 RID: 73 RVA: 0x000036B0 File Offset: 0x000018B0
		public HDROutputUtils.HDRDisplayInformation hdrDisplayOutputInformation
		{
			get
			{
				HDROutputSettings hdrOutputSettings = XRSystem.GetActiveDisplay().hdrOutputSettings;
				int num = ((hdrOutputSettings != null) ? hdrOutputSettings.maxFullFrameToneMapLuminance : (-1));
				HDROutputSettings hdrOutputSettings2 = XRSystem.GetActiveDisplay().hdrOutputSettings;
				int num2 = ((hdrOutputSettings2 != null) ? hdrOutputSettings2.maxToneMapLuminance : (-1));
				HDROutputSettings hdrOutputSettings3 = XRSystem.GetActiveDisplay().hdrOutputSettings;
				int num3 = ((hdrOutputSettings3 != null) ? hdrOutputSettings3.minToneMapLuminance : (-1));
				HDROutputSettings hdrOutputSettings4 = XRSystem.GetActiveDisplay().hdrOutputSettings;
				return new HDROutputUtils.HDRDisplayInformation(num, num2, num3, (hdrOutputSettings4 != null) ? hdrOutputSettings4.paperWhiteNits : 160f);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004A RID: 74 RVA: 0x0000371E File Offset: 0x0000191E
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00003726 File Offset: 0x00001926
		public float occlusionMeshScale { get; private set; }

		// Token: 0x0600004C RID: 76 RVA: 0x0000372F File Offset: 0x0000192F
		public Matrix4x4 GetProjMatrix(int viewIndex = 0)
		{
			return this.m_Views[viewIndex].projMatrix;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003742 File Offset: 0x00001942
		public Matrix4x4 GetViewMatrix(int viewIndex = 0)
		{
			return this.m_Views[viewIndex].viewMatrix;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003755 File Offset: 0x00001955
		public Rect GetViewport(int viewIndex = 0)
		{
			return this.m_Views[viewIndex].viewport;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003768 File Offset: 0x00001968
		public Mesh GetOcclusionMesh(int viewIndex = 0)
		{
			return this.m_Views[viewIndex].occlusionMesh;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0000377B File Offset: 0x0000197B
		public int GetTextureArraySlice(int viewIndex = 0)
		{
			return this.m_Views[viewIndex].textureArraySlice;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003790 File Offset: 0x00001990
		public void StartSinglePass(CommandBuffer cmd)
		{
			if (!this.enabled || !this.singlePassEnabled)
			{
				return;
			}
			if (this.viewCount > TextureXR.slices)
			{
				throw new NotImplementedException(string.Format("Invalid XR setup for single-pass, trying to render too many views! Max supported: {0}", TextureXR.slices));
			}
			if (SystemInfo.supportsMultiview)
			{
				cmd.EnableShaderKeyword("STEREO_MULTIVIEW_ON");
				return;
			}
			cmd.EnableShaderKeyword("STEREO_INSTANCING_ON");
			cmd.SetInstanceMultiplier((uint)this.viewCount);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000037FF File Offset: 0x000019FF
		public void StopSinglePass(CommandBuffer cmd)
		{
			if (this.enabled && this.singlePassEnabled)
			{
				if (SystemInfo.supportsMultiview)
				{
					cmd.DisableShaderKeyword("STEREO_MULTIVIEW_ON");
					return;
				}
				cmd.DisableShaderKeyword("STEREO_INSTANCING_ON");
				cmd.SetInstanceMultiplier(1U);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00003836 File Offset: 0x00001A36
		public bool hasValidOcclusionMesh
		{
			get
			{
				return this.m_OcclusionMesh.hasValidOcclusionMesh;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003843 File Offset: 0x00001A43
		public void RenderOcclusionMesh(CommandBuffer cmd, bool renderIntoTexture = false)
		{
			if (this.occlusionMeshScale > 0f)
			{
				this.m_OcclusionMesh.RenderOcclusionMesh(cmd, this.occlusionMeshScale, renderIntoTexture);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003868 File Offset: 0x00001A68
		public Vector4 ApplyXRViewCenterOffset(Vector2 center)
		{
			Vector4 zero = Vector4.zero;
			float num = 0.5f - center.x;
			float num2 = 0.5f - center.y;
			zero.x = this.m_Views[0].eyeCenterUV.x - num;
			zero.y = this.m_Views[0].eyeCenterUV.y - num2;
			if (this.singlePassEnabled)
			{
				zero.z = this.m_Views[1].eyeCenterUV.x - num;
				zero.w = this.m_Views[1].eyeCenterUV.y - num2;
			}
			return zero;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0000391A File Offset: 0x00001B1A
		internal void AssignView(int viewId, XRView xrView)
		{
			if (viewId < 0 || viewId >= this.m_Views.Count)
			{
				throw new ArgumentOutOfRangeException("viewId");
			}
			this.m_Views[viewId] = xrView;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003946 File Offset: 0x00001B46
		internal void AssignCullingParams(int cullingPassId, ScriptableCullingParameters cullingParams)
		{
			cullingParams.cullingOptions &= ~CullingOptions.Stereo;
			this.cullingPassId = cullingPassId;
			this.cullingParams = cullingParams;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003966 File Offset: 0x00001B66
		internal void UpdateCombinedOcclusionMesh()
		{
			this.m_OcclusionMesh.UpdateCombinedMesh();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003974 File Offset: 0x00001B74
		public void InitBase(XRPassCreateInfo createInfo)
		{
			this.m_Views.Clear();
			this.copyDepth = createInfo.copyDepth;
			this.multipassId = createInfo.multipassId;
			this.AssignCullingParams(createInfo.cullingPassId, createInfo.cullingParameters);
			this.renderTarget = new RenderTargetIdentifier(createInfo.renderTarget, 0, CubemapFace.Unknown, -1);
			this.renderTargetDesc = createInfo.renderTargetDesc;
			this.m_OcclusionMesh.SetMaterial(createInfo.occlusionMeshMaterial);
			this.occlusionMeshScale = createInfo.occlusionMeshScale;
			this.foveatedRenderingInfo = createInfo.foveatedRenderingInfo;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000039FF File Offset: 0x00001BFF
		internal void AddView(XRView xrView)
		{
			if (this.m_Views.Count < TextureXR.slices)
			{
				this.m_Views.Add(xrView);
				return;
			}
			throw new NotImplementedException(string.Format("Invalid XR setup for single-pass, trying to add too many views! Max supported: {0}", TextureXR.slices));
		}

		// Token: 0x04000039 RID: 57
		private readonly List<XRView> m_Views;

		// Token: 0x0400003A RID: 58
		private readonly XROcclusionMesh m_OcclusionMesh;
	}
}
