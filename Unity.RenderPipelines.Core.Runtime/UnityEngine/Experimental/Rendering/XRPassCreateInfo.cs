using System;
using UnityEngine.Rendering;
using UnityEngine.XR;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x0200000A RID: 10
	public struct XRPassCreateInfo
	{
		// Token: 0x0400002F RID: 47
		internal RenderTargetIdentifier renderTarget;

		// Token: 0x04000030 RID: 48
		internal RenderTextureDescriptor renderTargetDesc;

		// Token: 0x04000031 RID: 49
		internal ScriptableCullingParameters cullingParameters;

		// Token: 0x04000032 RID: 50
		internal Material occlusionMeshMaterial;

		// Token: 0x04000033 RID: 51
		internal float occlusionMeshScale;

		// Token: 0x04000034 RID: 52
		internal IntPtr foveatedRenderingInfo;

		// Token: 0x04000035 RID: 53
		internal int multipassId;

		// Token: 0x04000036 RID: 54
		internal int cullingPassId;

		// Token: 0x04000037 RID: 55
		internal bool copyDepth;

		// Token: 0x04000038 RID: 56
		internal XRDisplaySubsystem.XRRenderPass xrSdkRenderPass;
	}
}
