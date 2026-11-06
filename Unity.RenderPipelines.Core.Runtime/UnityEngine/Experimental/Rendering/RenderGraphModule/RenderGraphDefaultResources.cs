using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000018 RID: 24
	public class RenderGraphDefaultResources
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x000071B3 File Offset: 0x000053B3
		// (set) Token: 0x060000FA RID: 250 RVA: 0x000071BB File Offset: 0x000053BB
		public TextureHandle blackTexture { get; private set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000071C4 File Offset: 0x000053C4
		// (set) Token: 0x060000FC RID: 252 RVA: 0x000071CC File Offset: 0x000053CC
		public TextureHandle whiteTexture { get; private set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000FD RID: 253 RVA: 0x000071D5 File Offset: 0x000053D5
		// (set) Token: 0x060000FE RID: 254 RVA: 0x000071DD File Offset: 0x000053DD
		public TextureHandle clearTextureXR { get; private set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000071E6 File Offset: 0x000053E6
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000071EE File Offset: 0x000053EE
		public TextureHandle magentaTextureXR { get; private set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000071F7 File Offset: 0x000053F7
		// (set) Token: 0x06000102 RID: 258 RVA: 0x000071FF File Offset: 0x000053FF
		public TextureHandle blackTextureXR { get; private set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000103 RID: 259 RVA: 0x00007208 File Offset: 0x00005408
		// (set) Token: 0x06000104 RID: 260 RVA: 0x00007210 File Offset: 0x00005410
		public TextureHandle blackTextureArrayXR { get; private set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00007219 File Offset: 0x00005419
		// (set) Token: 0x06000106 RID: 262 RVA: 0x00007221 File Offset: 0x00005421
		public TextureHandle blackUIntTextureXR { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000722A File Offset: 0x0000542A
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00007232 File Offset: 0x00005432
		public TextureHandle blackTexture3DXR { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000109 RID: 265 RVA: 0x0000723B File Offset: 0x0000543B
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00007243 File Offset: 0x00005443
		public TextureHandle whiteTextureXR { get; private set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000724C File Offset: 0x0000544C
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00007254 File Offset: 0x00005454
		public TextureHandle defaultShadowTexture { get; private set; }

		// Token: 0x0600010D RID: 269 RVA: 0x00007260 File Offset: 0x00005460
		internal RenderGraphDefaultResources()
		{
			this.m_BlackTexture2D = RTHandles.Alloc(Texture2D.blackTexture);
			this.m_WhiteTexture2D = RTHandles.Alloc(Texture2D.whiteTexture);
			this.m_ShadowTexture2D = RTHandles.Alloc(1, 1, 1, DepthBits.Depth32, GraphicsFormat.R8G8B8A8_SRGB, FilterMode.Point, TextureWrapMode.Repeat, TextureDimension.Tex2D, false, false, true, true, 1, 0f, MSAASamples.None, false, false, RenderTextureMemoryless.None, VRTextureUsage.None, "");
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000072BB File Offset: 0x000054BB
		internal void Cleanup()
		{
			this.m_BlackTexture2D.Release();
			this.m_WhiteTexture2D.Release();
			this.m_ShadowTexture2D.Release();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000072E0 File Offset: 0x000054E0
		internal void InitializeForRendering(RenderGraph renderGraph)
		{
			this.blackTexture = renderGraph.ImportTexture(this.m_BlackTexture2D);
			this.whiteTexture = renderGraph.ImportTexture(this.m_WhiteTexture2D);
			this.defaultShadowTexture = renderGraph.ImportTexture(this.m_ShadowTexture2D);
			this.clearTextureXR = renderGraph.ImportTexture(TextureXR.GetClearTexture());
			this.magentaTextureXR = renderGraph.ImportTexture(TextureXR.GetMagentaTexture());
			this.blackTextureXR = renderGraph.ImportTexture(TextureXR.GetBlackTexture());
			this.blackTextureArrayXR = renderGraph.ImportTexture(TextureXR.GetBlackTextureArray());
			this.blackUIntTextureXR = renderGraph.ImportTexture(TextureXR.GetBlackUIntTexture());
			this.blackTexture3DXR = renderGraph.ImportTexture(TextureXR.GetBlackTexture3D());
			this.whiteTextureXR = renderGraph.ImportTexture(TextureXR.GetWhiteTexture());
		}

		// Token: 0x04000093 RID: 147
		private bool m_IsValid;

		// Token: 0x04000094 RID: 148
		private RTHandle m_BlackTexture2D;

		// Token: 0x04000095 RID: 149
		private RTHandle m_WhiteTexture2D;

		// Token: 0x04000096 RID: 150
		private RTHandle m_ShadowTexture2D;
	}
}
