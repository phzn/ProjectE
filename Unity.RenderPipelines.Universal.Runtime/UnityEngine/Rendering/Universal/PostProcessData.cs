using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public class PostProcessData : ScriptableObject
	{
		// Token: 0x040001AA RID: 426
		public PostProcessData.ShaderResources shaders;

		// Token: 0x040001AB RID: 427
		public PostProcessData.TextureResources textures;

		// Token: 0x02000165 RID: 357
		[ReloadGroup]
		[Serializable]
		public sealed class ShaderResources
		{
			// Token: 0x040009B2 RID: 2482
			[Reload("Shaders/PostProcessing/StopNaN.shader", ReloadAttribute.Package.Root)]
			public Shader stopNanPS;

			// Token: 0x040009B3 RID: 2483
			[Reload("Shaders/PostProcessing/SubpixelMorphologicalAntialiasing.shader", ReloadAttribute.Package.Root)]
			public Shader subpixelMorphologicalAntialiasingPS;

			// Token: 0x040009B4 RID: 2484
			[Reload("Shaders/PostProcessing/GaussianDepthOfField.shader", ReloadAttribute.Package.Root)]
			public Shader gaussianDepthOfFieldPS;

			// Token: 0x040009B5 RID: 2485
			[Reload("Shaders/PostProcessing/BokehDepthOfField.shader", ReloadAttribute.Package.Root)]
			public Shader bokehDepthOfFieldPS;

			// Token: 0x040009B6 RID: 2486
			[Reload("Shaders/PostProcessing/CameraMotionBlur.shader", ReloadAttribute.Package.Root)]
			public Shader cameraMotionBlurPS;

			// Token: 0x040009B7 RID: 2487
			[Reload("Shaders/PostProcessing/PaniniProjection.shader", ReloadAttribute.Package.Root)]
			public Shader paniniProjectionPS;

			// Token: 0x040009B8 RID: 2488
			[Reload("Shaders/PostProcessing/LutBuilderLdr.shader", ReloadAttribute.Package.Root)]
			public Shader lutBuilderLdrPS;

			// Token: 0x040009B9 RID: 2489
			[Reload("Shaders/PostProcessing/LutBuilderHdr.shader", ReloadAttribute.Package.Root)]
			public Shader lutBuilderHdrPS;

			// Token: 0x040009BA RID: 2490
			[Reload("Shaders/PostProcessing/Bloom.shader", ReloadAttribute.Package.Root)]
			public Shader bloomPS;

			// Token: 0x040009BB RID: 2491
			[Reload("Shaders/PostProcessing/TemporalAA.shader", ReloadAttribute.Package.Root)]
			public Shader temporalAntialiasingPS;

			// Token: 0x040009BC RID: 2492
			[Reload("Shaders/PostProcessing/LensFlareDataDriven.shader", ReloadAttribute.Package.Root)]
			public Shader LensFlareDataDrivenPS;

			// Token: 0x040009BD RID: 2493
			[Reload("Shaders/PostProcessing/ScalingSetup.shader", ReloadAttribute.Package.Root)]
			public Shader scalingSetupPS;

			// Token: 0x040009BE RID: 2494
			[Reload("Shaders/PostProcessing/EdgeAdaptiveSpatialUpsampling.shader", ReloadAttribute.Package.Root)]
			public Shader easuPS;

			// Token: 0x040009BF RID: 2495
			[Reload("Shaders/PostProcessing/UberPost.shader", ReloadAttribute.Package.Root)]
			public Shader uberPostPS;

			// Token: 0x040009C0 RID: 2496
			[Reload("Shaders/PostProcessing/FinalPost.shader", ReloadAttribute.Package.Root)]
			public Shader finalPostPassPS;
		}

		// Token: 0x02000166 RID: 358
		[ReloadGroup]
		[Serializable]
		public sealed class TextureResources
		{
			// Token: 0x040009C1 RID: 2497
			[Reload("Textures/BlueNoise16/L/LDR_LLL1_{0}.png", 0, 32, ReloadAttribute.Package.Root)]
			public Texture2D[] blueNoise16LTex;

			// Token: 0x040009C2 RID: 2498
			[Reload(new string[] { "Textures/FilmGrain/Thin01.png", "Textures/FilmGrain/Thin02.png", "Textures/FilmGrain/Medium01.png", "Textures/FilmGrain/Medium02.png", "Textures/FilmGrain/Medium03.png", "Textures/FilmGrain/Medium04.png", "Textures/FilmGrain/Medium05.png", "Textures/FilmGrain/Medium06.png", "Textures/FilmGrain/Large01.png", "Textures/FilmGrain/Large02.png" }, ReloadAttribute.Package.Root)]
			public Texture2D[] filmGrainTex;

			// Token: 0x040009C3 RID: 2499
			[Reload("Textures/SMAA/AreaTex.tga", ReloadAttribute.Package.Root)]
			public Texture2D smaaAreaTex;

			// Token: 0x040009C4 RID: 2500
			[Reload("Textures/SMAA/SearchTex.tga", ReloadAttribute.Package.Root)]
			public Texture2D smaaSearchTex;
		}
	}
}
