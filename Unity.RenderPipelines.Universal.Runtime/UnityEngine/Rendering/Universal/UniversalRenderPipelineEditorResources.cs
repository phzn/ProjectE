using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x02000054 RID: 84
	public class UniversalRenderPipelineEditorResources : ScriptableObject
	{
		// Token: 0x04000254 RID: 596
		public UniversalRenderPipelineEditorResources.ShaderResources shaders;

		// Token: 0x04000255 RID: 597
		public UniversalRenderPipelineEditorResources.MaterialResources materials;

		// Token: 0x02000168 RID: 360
		[ReloadGroup]
		[Serializable]
		public sealed class ShaderResources
		{
			// Token: 0x040009C7 RID: 2503
			[Reload("Shaders/AutodeskInteractive/AutodeskInteractive.shadergraph", ReloadAttribute.Package.Root)]
			public Shader autodeskInteractivePS;

			// Token: 0x040009C8 RID: 2504
			[Reload("Shaders/AutodeskInteractive/AutodeskInteractiveTransparent.shadergraph", ReloadAttribute.Package.Root)]
			public Shader autodeskInteractiveTransparentPS;

			// Token: 0x040009C9 RID: 2505
			[Reload("Shaders/AutodeskInteractive/AutodeskInteractiveMasked.shadergraph", ReloadAttribute.Package.Root)]
			public Shader autodeskInteractiveMaskedPS;

			// Token: 0x040009CA RID: 2506
			[Reload("Shaders/Nature/SpeedTree7.shader", ReloadAttribute.Package.Root)]
			public Shader defaultSpeedTree7PS;

			// Token: 0x040009CB RID: 2507
			[Reload("Shaders/Nature/SpeedTree8_PBRLit.shadergraph", ReloadAttribute.Package.Root)]
			public Shader defaultSpeedTree8PS;
		}

		// Token: 0x02000169 RID: 361
		[ReloadGroup]
		[Serializable]
		public sealed class MaterialResources
		{
			// Token: 0x040009CC RID: 2508
			[Reload("Runtime/Materials/Lit.mat", ReloadAttribute.Package.Root)]
			public Material lit;

			// Token: 0x040009CD RID: 2509
			[Reload("Runtime/Materials/ParticlesUnlit.mat", ReloadAttribute.Package.Root)]
			public Material particleLit;

			// Token: 0x040009CE RID: 2510
			[Reload("Runtime/Materials/TerrainLit.mat", ReloadAttribute.Package.Root)]
			public Material terrainLit;

			// Token: 0x040009CF RID: 2511
			[Reload("Runtime/Materials/Decal.mat", ReloadAttribute.Package.Root)]
			public Material decal;
		}
	}
}
