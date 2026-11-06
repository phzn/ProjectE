using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000055 RID: 85
	[Serializable]
	public class XRSystemData : ScriptableObject
	{
		// Token: 0x04000256 RID: 598
		public XRSystemData.ShaderResources shaders;

		// Token: 0x0200016A RID: 362
		[ReloadGroup]
		[Serializable]
		public sealed class ShaderResources
		{
			// Token: 0x040009D0 RID: 2512
			[Reload("Shaders/XR/XROcclusionMesh.shader", ReloadAttribute.Package.Root)]
			public Shader xrOcclusionMeshPS;

			// Token: 0x040009D1 RID: 2513
			[Reload("Shaders/XR/XRMirrorView.shader", ReloadAttribute.Package.Root)]
			public Shader xrMirrorViewPS;
		}
	}
}
