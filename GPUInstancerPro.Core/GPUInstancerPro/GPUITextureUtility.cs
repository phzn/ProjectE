using System;
using UnityEngine;

namespace GPUInstancerPro
{
	// Token: 0x02000064 RID: 100
	public static class GPUITextureUtility
	{
		// Token: 0x060003BD RID: 957 RVA: 0x00014A18 File Offset: 0x00012C18
		public static void CopyTextureWithComputeShader(Texture source, Texture destination, int offsetX, int sourceMip = 0, int destinationMip = 0, int offsetY = 0)
		{
			int num = source.width;
			int num2 = source.height;
			for (int i = 0; i < sourceMip; i++)
			{
				num >>= 1;
				num2 >>= 1;
			}
			ComputeShader cs_TextureUtility = GPUIConstants.CS_TextureUtility;
			int num3 = 0;
			cs_TextureUtility.SetTexture(num3, GPUIConstants.PROP_source, source, sourceMip);
			cs_TextureUtility.SetTexture(num3, GPUIConstants.PROP_destination, destination, destinationMip);
			cs_TextureUtility.SetInt(GPUIConstants.PROP_offsetX, offsetX);
			cs_TextureUtility.SetInt(GPUIConstants.PROP_offsetY, offsetY);
			cs_TextureUtility.SetInt(GPUIConstants.PROP_sourceSizeX, num);
			cs_TextureUtility.SetInt(GPUIConstants.PROP_sourceSizeY, num2);
			cs_TextureUtility.DispatchXY(num3, num, num2);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00014AA4 File Offset: 0x00012CA4
		public static void CopyTextureSamplerWithComputeShader(Texture source, Texture destination)
		{
			int width = destination.width;
			int height = destination.height;
			ComputeShader cs_TextureUtility = GPUIConstants.CS_TextureUtility;
			int num = 1;
			cs_TextureUtility.SetTexture(num, GPUIConstants.PROP_source, source);
			cs_TextureUtility.SetTexture(num, GPUIConstants.PROP_destination, destination);
			cs_TextureUtility.SetInt(GPUIConstants.PROP_destinationSizeX, width);
			cs_TextureUtility.SetInt(GPUIConstants.PROP_destinationSizeY, height);
			cs_TextureUtility.DispatchXY(num, width, height);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00014B00 File Offset: 0x00012D00
		public static void SetTextureDataWithComputeShaderSingleChannel(GraphicsBuffer textureData, Texture destination, float multiplier = 1f, int offsetX = 0, int offsetY = 0, int destinationW = 0, int destinationH = 0)
		{
			if (destinationW == 0)
			{
				destinationW = destination.width;
			}
			if (destinationH == 0)
			{
				destinationH = destination.height;
			}
			ComputeShader cs_TextureUtility = GPUIConstants.CS_TextureUtility;
			int num = 2;
			cs_TextureUtility.SetBuffer(num, GPUIConstants.PROP_textureDataSingleChannel, textureData);
			cs_TextureUtility.SetTexture(num, GPUIConstants.PROP_destination, destination);
			cs_TextureUtility.SetInt(GPUIConstants.PROP_destinationSizeX, destinationW);
			cs_TextureUtility.SetInt(GPUIConstants.PROP_destinationSizeY, destinationH);
			cs_TextureUtility.SetFloat(GPUIConstants.PROP_multiplier, multiplier);
			cs_TextureUtility.SetInt(GPUIConstants.PROP_offsetX, offsetX);
			cs_TextureUtility.SetInt(GPUIConstants.PROP_offsetY, offsetY);
			cs_TextureUtility.DispatchXY(num, destinationW, destinationH);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00014B90 File Offset: 0x00012D90
		public static Texture2D RenderTextureToTexture2D(RenderTexture renderTexture, TextureFormat textureFormat, bool linear, FilterMode filterMode = FilterMode.Bilinear)
		{
			Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, textureFormat, false, linear);
			texture2D.name = renderTexture.name;
			texture2D.filterMode = filterMode;
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)renderTexture.width, (float)renderTexture.height), 0, 0);
			texture2D.Apply(false);
			RenderTexture.active = active;
			return texture2D;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00014C02 File Offset: 0x00012E02
		public static void DestroyRenderTexture(this RenderTexture rt)
		{
			if (rt == null)
			{
				return;
			}
			if (RenderTexture.active == rt)
			{
				RenderTexture.active = null;
			}
			rt.Release();
			rt.DestroyGeneric();
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00014C2D File Offset: 0x00012E2D
		public static void ClearRenderTexture(this RenderTexture rt)
		{
			rt.ClearRenderTexture(Color.clear);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00014C3A File Offset: 0x00012E3A
		public static void ClearRenderTexture(this RenderTexture rt, Color color)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = rt;
			GL.Clear(true, true, color);
			RenderTexture.active = active;
		}
	}
}
