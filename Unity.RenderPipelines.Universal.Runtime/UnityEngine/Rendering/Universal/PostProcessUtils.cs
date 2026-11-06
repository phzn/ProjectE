using System;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000BB RID: 187
	public static class PostProcessUtils
	{
		// Token: 0x060005FF RID: 1535 RVA: 0x00024628 File Offset: 0x00022828
		[Obsolete("This method is obsolete. Use ConfigureDithering override that takes camera pixel width and height instead.")]
		public static int ConfigureDithering(PostProcessData data, int index, Camera camera, Material material)
		{
			return PostProcessUtils.ConfigureDithering(data, index, camera.pixelWidth, camera.pixelHeight, material);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00024640 File Offset: 0x00022840
		public static int ConfigureDithering(PostProcessData data, int index, int cameraPixelWidth, int cameraPixelHeight, Material material)
		{
			Texture2D[] blueNoise16LTex = data.textures.blueNoise16LTex;
			if (blueNoise16LTex == null || blueNoise16LTex.Length == 0)
			{
				return 0;
			}
			if (++index >= blueNoise16LTex.Length)
			{
				index = 0;
			}
			Random.State state = Random.state;
			Random.InitState(Time.frameCount);
			float value = Random.value;
			float value2 = Random.value;
			Random.state = state;
			Texture2D texture2D = blueNoise16LTex[index];
			material.SetTexture(PostProcessUtils.ShaderConstants._BlueNoise_Texture, texture2D);
			material.SetVector(PostProcessUtils.ShaderConstants._Dithering_Params, new Vector4((float)cameraPixelWidth / (float)texture2D.width, (float)cameraPixelHeight / (float)texture2D.height, value, value2));
			return index;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000246C9 File Offset: 0x000228C9
		[Obsolete("This method is obsolete. Use ConfigureFilmGrain override that takes camera pixel width and height instead.")]
		public static void ConfigureFilmGrain(PostProcessData data, FilmGrain settings, Camera camera, Material material)
		{
			PostProcessUtils.ConfigureFilmGrain(data, settings, camera.pixelWidth, camera.pixelHeight, material);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000246E0 File Offset: 0x000228E0
		public static void ConfigureFilmGrain(PostProcessData data, FilmGrain settings, int cameraPixelWidth, int cameraPixelHeight, Material material)
		{
			Texture texture = settings.texture.value;
			if (settings.type.value != FilmGrainLookup.Custom)
			{
				texture = data.textures.filmGrainTex[(int)settings.type.value];
			}
			Random.State state = Random.state;
			Random.InitState(Time.frameCount);
			float value = Random.value;
			float value2 = Random.value;
			Random.state = state;
			Vector4 vector = ((texture == null) ? Vector4.zero : new Vector4((float)cameraPixelWidth / (float)texture.width, (float)cameraPixelHeight / (float)texture.height, value, value2));
			material.SetTexture(PostProcessUtils.ShaderConstants._Grain_Texture, texture);
			material.SetVector(PostProcessUtils.ShaderConstants._Grain_Params, new Vector2(settings.intensity.value * 4f, settings.response.value));
			material.SetVector(PostProcessUtils.ShaderConstants._Grain_TilingParams, vector);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000247B8 File Offset: 0x000229B8
		internal static void SetSourceSize(CommandBuffer cmd, RenderTextureDescriptor desc)
		{
			float num = (float)desc.width;
			float num2 = (float)desc.height;
			if (desc.useDynamicScale)
			{
				num *= ScalableBufferManager.widthScaleFactor;
				num2 *= ScalableBufferManager.heightScaleFactor;
			}
			cmd.SetGlobalVector(PostProcessUtils.ShaderConstants._SourceSize, new Vector4(num, num2, 1f / num, 1f / num2));
		}

		// Token: 0x020001AA RID: 426
		private static class ShaderConstants
		{
			// Token: 0x04000B1F RID: 2847
			public static readonly int _Grain_Texture = Shader.PropertyToID("_Grain_Texture");

			// Token: 0x04000B20 RID: 2848
			public static readonly int _Grain_Params = Shader.PropertyToID("_Grain_Params");

			// Token: 0x04000B21 RID: 2849
			public static readonly int _Grain_TilingParams = Shader.PropertyToID("_Grain_TilingParams");

			// Token: 0x04000B22 RID: 2850
			public static readonly int _BlueNoise_Texture = Shader.PropertyToID("_BlueNoise_Texture");

			// Token: 0x04000B23 RID: 2851
			public static readonly int _Dithering_Params = Shader.PropertyToID("_Dithering_Params");

			// Token: 0x04000B24 RID: 2852
			public static readonly int _SourceSize = Shader.PropertyToID("_SourceSize");
		}
	}
}
