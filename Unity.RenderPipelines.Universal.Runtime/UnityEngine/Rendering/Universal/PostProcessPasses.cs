using System;
using UnityEngine.Rendering.Universal.Internal;

namespace UnityEngine.Rendering.Universal
{
	// Token: 0x020000BA RID: 186
	internal struct PostProcessPasses : IDisposable
	{
		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00024416 File Offset: 0x00022616
		public ColorGradingLutPass colorGradingLutPass
		{
			get
			{
				return this.m_ColorGradingLutPass;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060005F6 RID: 1526 RVA: 0x0002441E File Offset: 0x0002261E
		public PostProcessPass postProcessPass
		{
			get
			{
				return this.m_PostProcessPass;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00024426 File Offset: 0x00022626
		public PostProcessPass finalPostProcessPass
		{
			get
			{
				return this.m_FinalPostProcessPass;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060005F8 RID: 1528 RVA: 0x0002442E File Offset: 0x0002262E
		public RTHandle afterPostProcessColor
		{
			get
			{
				return this.m_AfterPostProcessColor;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060005F9 RID: 1529 RVA: 0x00024436 File Offset: 0x00022636
		public RTHandle colorGradingLut
		{
			get
			{
				return this.m_ColorGradingLut;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x0002443E File Offset: 0x0002263E
		public bool isCreated
		{
			get
			{
				return this.m_CurrentPostProcessData != null;
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x0002444C File Offset: 0x0002264C
		public PostProcessPasses(PostProcessData rendererPostProcessData, ref PostProcessParams postProcessParams)
		{
			this.m_ColorGradingLutPass = null;
			this.m_PostProcessPass = null;
			this.m_FinalPostProcessPass = null;
			this.m_CurrentPostProcessData = null;
			this.m_AfterPostProcessColor = null;
			this.m_ColorGradingLut = null;
			this.m_RendererPostProcessData = rendererPostProcessData;
			this.m_BlitMaterial = postProcessParams.blitMaterial;
			this.Recreate(rendererPostProcessData, ref postProcessParams);
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x000244A0 File Offset: 0x000226A0
		public void Recreate(PostProcessData data, ref PostProcessParams ppParams)
		{
			if (this.m_RendererPostProcessData)
			{
				data = this.m_RendererPostProcessData;
			}
			if (data == this.m_CurrentPostProcessData)
			{
				return;
			}
			if (this.m_CurrentPostProcessData != null)
			{
				ColorGradingLutPass colorGradingLutPass = this.m_ColorGradingLutPass;
				if (colorGradingLutPass != null)
				{
					colorGradingLutPass.Cleanup();
				}
				PostProcessPass postProcessPass = this.m_PostProcessPass;
				if (postProcessPass != null)
				{
					postProcessPass.Cleanup();
				}
				PostProcessPass finalPostProcessPass = this.m_FinalPostProcessPass;
				if (finalPostProcessPass != null)
				{
					finalPostProcessPass.Cleanup();
				}
				this.m_ColorGradingLutPass = null;
				this.m_PostProcessPass = null;
				this.m_FinalPostProcessPass = null;
				this.m_CurrentPostProcessData = null;
			}
			if (data != null)
			{
				this.m_ColorGradingLutPass = new ColorGradingLutPass(RenderPassEvent.BeforeRenderingPrePasses, data);
				this.m_PostProcessPass = new PostProcessPass(RenderPassEvent.BeforeRenderingPostProcessing, data, ref ppParams);
				this.m_FinalPostProcessPass = new PostProcessPass(RenderPassEvent.AfterRenderingPostProcessing, data, ref ppParams);
				this.m_CurrentPostProcessData = data;
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00024574 File Offset: 0x00022774
		public void Dispose()
		{
			ColorGradingLutPass colorGradingLutPass = this.m_ColorGradingLutPass;
			if (colorGradingLutPass != null)
			{
				colorGradingLutPass.Cleanup();
			}
			PostProcessPass postProcessPass = this.m_PostProcessPass;
			if (postProcessPass != null)
			{
				postProcessPass.Cleanup();
			}
			PostProcessPass finalPostProcessPass = this.m_FinalPostProcessPass;
			if (finalPostProcessPass != null)
			{
				finalPostProcessPass.Cleanup();
			}
			RTHandle afterPostProcessColor = this.m_AfterPostProcessColor;
			if (afterPostProcessColor != null)
			{
				afterPostProcessColor.Release();
			}
			RTHandle colorGradingLut = this.m_ColorGradingLut;
			if (colorGradingLut == null)
			{
				return;
			}
			colorGradingLut.Release();
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x000245D8 File Offset: 0x000227D8
		internal void ReleaseRenderTargets()
		{
			RTHandle afterPostProcessColor = this.m_AfterPostProcessColor;
			if (afterPostProcessColor != null)
			{
				afterPostProcessColor.Release();
			}
			PostProcessPass postProcessPass = this.m_PostProcessPass;
			if (postProcessPass != null)
			{
				postProcessPass.Dispose();
			}
			PostProcessPass finalPostProcessPass = this.m_FinalPostProcessPass;
			if (finalPostProcessPass != null)
			{
				finalPostProcessPass.Dispose();
			}
			RTHandle colorGradingLut = this.m_ColorGradingLut;
			if (colorGradingLut == null)
			{
				return;
			}
			colorGradingLut.Release();
		}

		// Token: 0x040004BB RID: 1211
		private ColorGradingLutPass m_ColorGradingLutPass;

		// Token: 0x040004BC RID: 1212
		private PostProcessPass m_PostProcessPass;

		// Token: 0x040004BD RID: 1213
		private PostProcessPass m_FinalPostProcessPass;

		// Token: 0x040004BE RID: 1214
		internal RTHandle m_AfterPostProcessColor;

		// Token: 0x040004BF RID: 1215
		internal RTHandle m_ColorGradingLut;

		// Token: 0x040004C0 RID: 1216
		private PostProcessData m_RendererPostProcessData;

		// Token: 0x040004C1 RID: 1217
		private PostProcessData m_CurrentPostProcessData;

		// Token: 0x040004C2 RID: 1218
		private Material m_BlitMaterial;
	}
}
