using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200002F RID: 47
	public struct TextureDesc
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x000093C3 File Offset: 0x000075C3
		private void InitDefaultValues(bool dynamicResolution, bool xrReady)
		{
			this.useDynamicScale = dynamicResolution;
			this.vrUsage = VRTextureUsage.None;
			if (xrReady)
			{
				this.slices = TextureXR.slices;
				this.dimension = TextureXR.dimension;
				return;
			}
			this.slices = 1;
			this.dimension = TextureDimension.Tex2D;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000093FB File Offset: 0x000075FB
		public TextureDesc(int width, int height, bool dynamicResolution = false, bool xrReady = false)
		{
			this = default(TextureDesc);
			this.sizeMode = TextureSizeMode.Explicit;
			this.width = width;
			this.height = height;
			this.msaaSamples = MSAASamples.None;
			this.InitDefaultValues(dynamicResolution, xrReady);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009429 File Offset: 0x00007629
		public TextureDesc(Vector2 scale, bool dynamicResolution = false, bool xrReady = false)
		{
			this = default(TextureDesc);
			this.sizeMode = TextureSizeMode.Scale;
			this.scale = scale;
			this.msaaSamples = MSAASamples.None;
			this.dimension = TextureDimension.Tex2D;
			this.InitDefaultValues(dynamicResolution, xrReady);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00009456 File Offset: 0x00007656
		public TextureDesc(ScaleFunc func, bool dynamicResolution = false, bool xrReady = false)
		{
			this = default(TextureDesc);
			this.sizeMode = TextureSizeMode.Functor;
			this.func = func;
			this.msaaSamples = MSAASamples.None;
			this.dimension = TextureDimension.Tex2D;
			this.InitDefaultValues(dynamicResolution, xrReady);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00009483 File Offset: 0x00007683
		public TextureDesc(TextureDesc input)
		{
			this = input;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000948C File Offset: 0x0000768C
		public override int GetHashCode()
		{
			HashFNV1A32 hashFNV1A = HashFNV1A32.Create();
			switch (this.sizeMode)
			{
			case TextureSizeMode.Explicit:
				hashFNV1A.Append(in this.width);
				hashFNV1A.Append(in this.height);
				break;
			case TextureSizeMode.Scale:
				hashFNV1A.Append(in this.scale);
				break;
			case TextureSizeMode.Functor:
				if (this.func != null)
				{
					hashFNV1A.Append(this.func);
				}
				break;
			}
			hashFNV1A.Append(in this.mipMapBias);
			hashFNV1A.Append(in this.slices);
			int num = (int)this.depthBufferBits;
			hashFNV1A.Append(in num);
			num = (int)this.colorFormat;
			hashFNV1A.Append(in num);
			num = (int)this.filterMode;
			hashFNV1A.Append(in num);
			num = (int)this.wrapMode;
			hashFNV1A.Append(in num);
			num = (int)this.dimension;
			hashFNV1A.Append(in num);
			num = (int)this.memoryless;
			hashFNV1A.Append(in num);
			num = (int)this.vrUsage;
			hashFNV1A.Append(in num);
			hashFNV1A.Append(in this.anisoLevel);
			hashFNV1A.Append(in this.enableRandomWrite);
			hashFNV1A.Append(in this.useMipMap);
			hashFNV1A.Append(in this.autoGenerateMips);
			hashFNV1A.Append(in this.isShadowMap);
			hashFNV1A.Append(in this.bindTextureMS);
			hashFNV1A.Append(in this.useDynamicScale);
			num = (int)this.msaaSamples;
			hashFNV1A.Append(in num);
			hashFNV1A.Append(in this.fastMemoryDesc.inFastMemory);
			return hashFNV1A.value;
		}

		// Token: 0x040000F9 RID: 249
		public TextureSizeMode sizeMode;

		// Token: 0x040000FA RID: 250
		public int width;

		// Token: 0x040000FB RID: 251
		public int height;

		// Token: 0x040000FC RID: 252
		public int slices;

		// Token: 0x040000FD RID: 253
		public Vector2 scale;

		// Token: 0x040000FE RID: 254
		public ScaleFunc func;

		// Token: 0x040000FF RID: 255
		public DepthBits depthBufferBits;

		// Token: 0x04000100 RID: 256
		public GraphicsFormat colorFormat;

		// Token: 0x04000101 RID: 257
		public FilterMode filterMode;

		// Token: 0x04000102 RID: 258
		public TextureWrapMode wrapMode;

		// Token: 0x04000103 RID: 259
		public TextureDimension dimension;

		// Token: 0x04000104 RID: 260
		public bool enableRandomWrite;

		// Token: 0x04000105 RID: 261
		public bool useMipMap;

		// Token: 0x04000106 RID: 262
		public bool autoGenerateMips;

		// Token: 0x04000107 RID: 263
		public bool isShadowMap;

		// Token: 0x04000108 RID: 264
		public int anisoLevel;

		// Token: 0x04000109 RID: 265
		public float mipMapBias;

		// Token: 0x0400010A RID: 266
		public MSAASamples msaaSamples;

		// Token: 0x0400010B RID: 267
		public bool bindTextureMS;

		// Token: 0x0400010C RID: 268
		public bool useDynamicScale;

		// Token: 0x0400010D RID: 269
		public RenderTextureMemoryless memoryless;

		// Token: 0x0400010E RID: 270
		public VRTextureUsage vrUsage;

		// Token: 0x0400010F RID: 271
		public string name;

		// Token: 0x04000110 RID: 272
		public FastMemoryDesc fastMemoryDesc;

		// Token: 0x04000111 RID: 273
		public bool fallBackToBlackTexture;

		// Token: 0x04000112 RID: 274
		public bool disableFallBackToImportedTexture;

		// Token: 0x04000113 RID: 275
		public bool clearBuffer;

		// Token: 0x04000114 RID: 276
		public Color clearColor;
	}
}
