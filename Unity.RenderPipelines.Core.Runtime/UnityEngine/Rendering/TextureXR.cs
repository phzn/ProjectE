using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
	// Token: 0x020000C3 RID: 195
	public static class TextureXR
	{
		// Token: 0x170000E9 RID: 233
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x0001EFD6 File Offset: 0x0001D1D6
		public static int maxViews
		{
			set
			{
				TextureXR.m_MaxViews = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001EFDE File Offset: 0x0001D1DE
		public static int slices
		{
			get
			{
				return TextureXR.m_MaxViews;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0001EFE8 File Offset: 0x0001D1E8
		public static bool useTexArray
		{
			get
			{
				GraphicsDeviceType graphicsDeviceType = SystemInfo.graphicsDeviceType;
				if (graphicsDeviceType <= GraphicsDeviceType.Metal)
				{
					if (graphicsDeviceType != GraphicsDeviceType.Direct3D11 && graphicsDeviceType != GraphicsDeviceType.PlayStation4 && graphicsDeviceType != GraphicsDeviceType.Metal)
					{
						return false;
					}
				}
				else if (graphicsDeviceType != GraphicsDeviceType.Direct3D12 && graphicsDeviceType != GraphicsDeviceType.Vulkan && graphicsDeviceType - GraphicsDeviceType.PlayStation5 > 1)
				{
					return false;
				}
				return true;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001F024 File Offset: 0x0001D224
		public static TextureDimension dimension
		{
			get
			{
				if (!TextureXR.useTexArray)
				{
					return TextureDimension.Tex2D;
				}
				return TextureDimension.Tex2DArray;
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001F030 File Offset: 0x0001D230
		public static RTHandle GetBlackUIntTexture()
		{
			if (!TextureXR.useTexArray)
			{
				return TextureXR.m_BlackUIntTextureRTH;
			}
			return TextureXR.m_BlackUIntTexture2DArrayRTH;
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001F044 File Offset: 0x0001D244
		public static RTHandle GetClearTexture()
		{
			if (!TextureXR.useTexArray)
			{
				return TextureXR.m_ClearTextureRTH;
			}
			return TextureXR.m_ClearTexture2DArrayRTH;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0001F058 File Offset: 0x0001D258
		public static RTHandle GetMagentaTexture()
		{
			if (!TextureXR.useTexArray)
			{
				return TextureXR.m_MagentaTextureRTH;
			}
			return TextureXR.m_MagentaTexture2DArrayRTH;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001F06C File Offset: 0x0001D26C
		public static RTHandle GetBlackTexture()
		{
			if (!TextureXR.useTexArray)
			{
				return TextureXR.m_BlackTextureRTH;
			}
			return TextureXR.m_BlackTexture2DArrayRTH;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001F080 File Offset: 0x0001D280
		public static RTHandle GetBlackTextureArray()
		{
			return TextureXR.m_BlackTexture2DArrayRTH;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001F087 File Offset: 0x0001D287
		public static RTHandle GetBlackTexture3D()
		{
			return TextureXR.m_BlackTexture3DRTH;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001F08E File Offset: 0x0001D28E
		public static RTHandle GetWhiteTexture()
		{
			if (!TextureXR.useTexArray)
			{
				return TextureXR.m_WhiteTextureRTH;
			}
			return TextureXR.m_WhiteTexture2DArrayRTH;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001F0A4 File Offset: 0x0001D2A4
		public static void Initialize(CommandBuffer cmd, ComputeShader clearR32_UIntShader)
		{
			if (TextureXR.m_BlackUIntTexture2DArray == null)
			{
				RTHandles.Release(TextureXR.m_BlackUIntTexture2DArrayRTH);
				TextureXR.m_BlackUIntTexture2DArray = TextureXR.CreateBlackUIntTextureArray(cmd, clearR32_UIntShader);
				TextureXR.m_BlackUIntTexture2DArrayRTH = RTHandles.Alloc(TextureXR.m_BlackUIntTexture2DArray);
				RTHandles.Release(TextureXR.m_BlackUIntTextureRTH);
				TextureXR.m_BlackUIntTexture = TextureXR.CreateBlackUintTexture(cmd, clearR32_UIntShader);
				TextureXR.m_BlackUIntTextureRTH = RTHandles.Alloc(TextureXR.m_BlackUIntTexture);
				RTHandles.Release(TextureXR.m_ClearTextureRTH);
				TextureXR.m_ClearTexture = new Texture2D(1, 1, GraphicsFormat.R8G8B8A8_SRGB, TextureCreationFlags.None)
				{
					name = "Clear Texture"
				};
				TextureXR.m_ClearTexture.SetPixel(0, 0, Color.clear);
				TextureXR.m_ClearTexture.Apply();
				TextureXR.m_ClearTextureRTH = RTHandles.Alloc(TextureXR.m_ClearTexture);
				RTHandles.Release(TextureXR.m_ClearTexture2DArrayRTH);
				TextureXR.m_ClearTexture2DArray = TextureXR.CreateTexture2DArrayFromTexture2D(TextureXR.m_ClearTexture, "Clear Texture2DArray");
				TextureXR.m_ClearTexture2DArrayRTH = RTHandles.Alloc(TextureXR.m_ClearTexture2DArray);
				RTHandles.Release(TextureXR.m_MagentaTextureRTH);
				TextureXR.m_MagentaTexture = new Texture2D(1, 1, GraphicsFormat.R8G8B8A8_SRGB, TextureCreationFlags.None)
				{
					name = "Magenta Texture"
				};
				TextureXR.m_MagentaTexture.SetPixel(0, 0, Color.magenta);
				TextureXR.m_MagentaTexture.Apply();
				TextureXR.m_MagentaTextureRTH = RTHandles.Alloc(TextureXR.m_MagentaTexture);
				RTHandles.Release(TextureXR.m_MagentaTexture2DArrayRTH);
				TextureXR.m_MagentaTexture2DArray = TextureXR.CreateTexture2DArrayFromTexture2D(TextureXR.m_MagentaTexture, "Magenta Texture2DArray");
				TextureXR.m_MagentaTexture2DArrayRTH = RTHandles.Alloc(TextureXR.m_MagentaTexture2DArray);
				RTHandles.Release(TextureXR.m_BlackTextureRTH);
				TextureXR.m_BlackTexture = new Texture2D(1, 1, GraphicsFormat.R8G8B8A8_SRGB, TextureCreationFlags.None)
				{
					name = "Black Texture"
				};
				TextureXR.m_BlackTexture.SetPixel(0, 0, Color.black);
				TextureXR.m_BlackTexture.Apply();
				TextureXR.m_BlackTextureRTH = RTHandles.Alloc(TextureXR.m_BlackTexture);
				RTHandles.Release(TextureXR.m_BlackTexture2DArrayRTH);
				TextureXR.m_BlackTexture2DArray = TextureXR.CreateTexture2DArrayFromTexture2D(TextureXR.m_BlackTexture, "Black Texture2DArray");
				TextureXR.m_BlackTexture2DArrayRTH = RTHandles.Alloc(TextureXR.m_BlackTexture2DArray);
				RTHandles.Release(TextureXR.m_BlackTexture3DRTH);
				TextureXR.m_BlackTexture3D = TextureXR.CreateBlackTexture3D("Black Texture3D");
				TextureXR.m_BlackTexture3DRTH = RTHandles.Alloc(TextureXR.m_BlackTexture3D);
				RTHandles.Release(TextureXR.m_WhiteTextureRTH);
				TextureXR.m_WhiteTextureRTH = RTHandles.Alloc(Texture2D.whiteTexture);
				RTHandles.Release(TextureXR.m_WhiteTexture2DArrayRTH);
				TextureXR.m_WhiteTexture2DArray = TextureXR.CreateTexture2DArrayFromTexture2D(Texture2D.whiteTexture, "White Texture2DArray");
				TextureXR.m_WhiteTexture2DArrayRTH = RTHandles.Alloc(TextureXR.m_WhiteTexture2DArray);
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001F2E8 File Offset: 0x0001D4E8
		private static Texture2DArray CreateTexture2DArrayFromTexture2D(Texture2D source, string name)
		{
			Texture2DArray texture2DArray = new Texture2DArray(source.width, source.height, TextureXR.slices, source.format, false)
			{
				name = name
			};
			for (int i = 0; i < TextureXR.slices; i++)
			{
				Graphics.CopyTexture(source, 0, 0, texture2DArray, i, 0);
			}
			return texture2DArray;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001F338 File Offset: 0x0001D538
		private static Texture CreateBlackUIntTextureArray(CommandBuffer cmd, ComputeShader clearR32_UIntShader)
		{
			RenderTexture renderTexture = new RenderTexture(1, 1, 0, GraphicsFormat.R32_UInt)
			{
				dimension = TextureDimension.Tex2DArray,
				volumeDepth = TextureXR.slices,
				useMipMap = false,
				autoGenerateMips = false,
				enableRandomWrite = true,
				name = "Black UInt Texture Array"
			};
			renderTexture.Create();
			int num = clearR32_UIntShader.FindKernel("ClearUIntTextureArray");
			cmd.SetComputeTextureParam(clearR32_UIntShader, num, "_TargetArray", renderTexture);
			cmd.DispatchCompute(clearR32_UIntShader, num, 1, 1, TextureXR.slices);
			return renderTexture;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x0001F3B8 File Offset: 0x0001D5B8
		private static Texture CreateBlackUintTexture(CommandBuffer cmd, ComputeShader clearR32_UIntShader)
		{
			RenderTexture renderTexture = new RenderTexture(1, 1, 0, GraphicsFormat.R32_UInt)
			{
				dimension = TextureDimension.Tex2D,
				volumeDepth = 1,
				useMipMap = false,
				autoGenerateMips = false,
				enableRandomWrite = true,
				name = "Black UInt Texture"
			};
			renderTexture.Create();
			int num = clearR32_UIntShader.FindKernel("ClearUIntTexture");
			cmd.SetComputeTextureParam(clearR32_UIntShader, num, "_Target", renderTexture);
			cmd.DispatchCompute(clearR32_UIntShader, num, 1, 1, 1);
			return renderTexture;
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0001F430 File Offset: 0x0001D630
		private static Texture3D CreateBlackTexture3D(string name)
		{
			Texture3D texture3D = new Texture3D(1, 1, 1, GraphicsFormat.R8G8B8A8_SRGB, TextureCreationFlags.None);
			texture3D.name = name;
			texture3D.SetPixel(0, 0, 0, Color.black, 0);
			texture3D.Apply(false);
			return texture3D;
		}

		// Token: 0x04000441 RID: 1089
		private static int m_MaxViews = 1;

		// Token: 0x04000442 RID: 1090
		private static Texture m_BlackUIntTexture2DArray;

		// Token: 0x04000443 RID: 1091
		private static Texture m_BlackUIntTexture;

		// Token: 0x04000444 RID: 1092
		private static RTHandle m_BlackUIntTexture2DArrayRTH;

		// Token: 0x04000445 RID: 1093
		private static RTHandle m_BlackUIntTextureRTH;

		// Token: 0x04000446 RID: 1094
		private static Texture2DArray m_ClearTexture2DArray;

		// Token: 0x04000447 RID: 1095
		private static Texture2D m_ClearTexture;

		// Token: 0x04000448 RID: 1096
		private static RTHandle m_ClearTexture2DArrayRTH;

		// Token: 0x04000449 RID: 1097
		private static RTHandle m_ClearTextureRTH;

		// Token: 0x0400044A RID: 1098
		private static Texture2DArray m_MagentaTexture2DArray;

		// Token: 0x0400044B RID: 1099
		private static Texture2D m_MagentaTexture;

		// Token: 0x0400044C RID: 1100
		private static RTHandle m_MagentaTexture2DArrayRTH;

		// Token: 0x0400044D RID: 1101
		private static RTHandle m_MagentaTextureRTH;

		// Token: 0x0400044E RID: 1102
		private static Texture2D m_BlackTexture;

		// Token: 0x0400044F RID: 1103
		private static Texture3D m_BlackTexture3D;

		// Token: 0x04000450 RID: 1104
		private static Texture2DArray m_BlackTexture2DArray;

		// Token: 0x04000451 RID: 1105
		private static RTHandle m_BlackTexture2DArrayRTH;

		// Token: 0x04000452 RID: 1106
		private static RTHandle m_BlackTextureRTH;

		// Token: 0x04000453 RID: 1107
		private static RTHandle m_BlackTexture3DRTH;

		// Token: 0x04000454 RID: 1108
		private static Texture2DArray m_WhiteTexture2DArray;

		// Token: 0x04000455 RID: 1109
		private static RTHandle m_WhiteTexture2DArrayRTH;

		// Token: 0x04000456 RID: 1110
		private static RTHandle m_WhiteTextureRTH;
	}
}
