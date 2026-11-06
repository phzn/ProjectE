using System;

namespace UnityEngine.Rendering.Universal.Internal
{
	// Token: 0x02000127 RID: 295
	internal sealed class RenderTargetBufferSystem
	{
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x060009C5 RID: 2501 RVA: 0x00041D23 File Offset: 0x0003FF23
		private ref RenderTargetBufferSystem.SwapBuffer backBuffer
		{
			get
			{
				if (!RenderTargetBufferSystem.m_AisBackBuffer)
				{
					return ref this.m_B;
				}
				return ref this.m_A;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x060009C6 RID: 2502 RVA: 0x00041D39 File Offset: 0x0003FF39
		private ref RenderTargetBufferSystem.SwapBuffer frontBuffer
		{
			get
			{
				if (!RenderTargetBufferSystem.m_AisBackBuffer)
				{
					return ref this.m_A;
				}
				return ref this.m_B;
			}
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x00041D4F File Offset: 0x0003FF4F
		public RenderTargetBufferSystem(string name)
		{
			this.m_A.name = name + "A";
			this.m_B.name = name + "B";
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00041D8C File Offset: 0x0003FF8C
		public void Dispose()
		{
			RTHandle rtMSAA = this.m_A.rtMSAA;
			if (rtMSAA != null)
			{
				rtMSAA.Release();
			}
			RTHandle rtMSAA2 = this.m_B.rtMSAA;
			if (rtMSAA2 != null)
			{
				rtMSAA2.Release();
			}
			RTHandle rtResolve = this.m_A.rtResolve;
			if (rtResolve != null)
			{
				rtResolve.Release();
			}
			RTHandle rtResolve2 = this.m_B.rtResolve;
			if (rtResolve2 == null)
			{
				return;
			}
			rtResolve2.Release();
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x00041DF0 File Offset: 0x0003FFF0
		public RTHandle PeekBackBuffer()
		{
			if (!this.m_AllowMSAA || this.backBuffer.msaa <= 1)
			{
				return this.backBuffer.rtResolve;
			}
			return this.backBuffer.rtMSAA;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x00041E1F File Offset: 0x0004001F
		public RTHandle GetBackBuffer(CommandBuffer cmd)
		{
			this.ReAllocate(cmd);
			return this.PeekBackBuffer();
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00041E30 File Offset: 0x00040030
		public RTHandle GetFrontBuffer(CommandBuffer cmd)
		{
			if (!this.m_AllowMSAA && this.frontBuffer.msaa > 1)
			{
				this.frontBuffer.msaa = 1;
			}
			this.ReAllocate(cmd);
			if (!this.m_AllowMSAA || this.frontBuffer.msaa <= 1)
			{
				return this.frontBuffer.rtResolve;
			}
			return this.frontBuffer.rtMSAA;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00041E93 File Offset: 0x00040093
		public void Swap()
		{
			RenderTargetBufferSystem.m_AisBackBuffer = !RenderTargetBufferSystem.m_AisBackBuffer;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00041EA4 File Offset: 0x000400A4
		private void ReAllocate(CommandBuffer cmd)
		{
			RenderTextureDescriptor desc = RenderTargetBufferSystem.m_Desc;
			desc.msaaSamples = this.m_A.msaa;
			if (desc.msaaSamples > 1)
			{
				RenderingUtils.ReAllocateIfNeeded(ref this.m_A.rtMSAA, in desc, this.m_FilterMode, TextureWrapMode.Clamp, false, 1, 0f, this.m_A.name);
			}
			desc.msaaSamples = this.m_B.msaa;
			if (desc.msaaSamples > 1)
			{
				RenderingUtils.ReAllocateIfNeeded(ref this.m_B.rtMSAA, in desc, this.m_FilterMode, TextureWrapMode.Clamp, false, 1, 0f, this.m_B.name);
			}
			desc.msaaSamples = 1;
			RenderingUtils.ReAllocateIfNeeded(ref this.m_A.rtResolve, in desc, this.m_FilterMode, TextureWrapMode.Clamp, false, 1, 0f, this.m_A.name);
			RenderingUtils.ReAllocateIfNeeded(ref this.m_B.rtResolve, in desc, this.m_FilterMode, TextureWrapMode.Clamp, false, 1, 0f, this.m_B.name);
			cmd.SetGlobalTexture(this.m_A.name, this.m_A.rtResolve);
			cmd.SetGlobalTexture(this.m_B.name, this.m_B.rtResolve);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00041FE9 File Offset: 0x000401E9
		public void Clear()
		{
			RenderTargetBufferSystem.m_AisBackBuffer = true;
			this.m_AllowMSAA = this.m_A.msaa > 1 || this.m_B.msaa > 1;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00042018 File Offset: 0x00040218
		public void SetCameraSettings(RenderTextureDescriptor desc, FilterMode filterMode)
		{
			desc.depthBufferBits = 0;
			RenderTargetBufferSystem.m_Desc = desc;
			this.m_FilterMode = filterMode;
			this.m_A.msaa = RenderTargetBufferSystem.m_Desc.msaaSamples;
			this.m_B.msaa = RenderTargetBufferSystem.m_Desc.msaaSamples;
			if (RenderTargetBufferSystem.m_Desc.msaaSamples > 1)
			{
				this.EnableMSAA(true);
			}
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00042078 File Offset: 0x00040278
		public RTHandle GetBufferA()
		{
			if (!this.m_AllowMSAA || this.m_A.msaa <= 1)
			{
				return this.m_A.rtResolve;
			}
			return this.m_A.rtMSAA;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x000420A7 File Offset: 0x000402A7
		public void EnableMSAA(bool enable)
		{
			this.m_AllowMSAA = enable;
			if (enable)
			{
				this.m_A.msaa = RenderTargetBufferSystem.m_Desc.msaaSamples;
				this.m_B.msaa = RenderTargetBufferSystem.m_Desc.msaaSamples;
			}
		}

		// Token: 0x040008C8 RID: 2248
		private RenderTargetBufferSystem.SwapBuffer m_A;

		// Token: 0x040008C9 RID: 2249
		private RenderTargetBufferSystem.SwapBuffer m_B;

		// Token: 0x040008CA RID: 2250
		private static bool m_AisBackBuffer = true;

		// Token: 0x040008CB RID: 2251
		private static RenderTextureDescriptor m_Desc;

		// Token: 0x040008CC RID: 2252
		private FilterMode m_FilterMode;

		// Token: 0x040008CD RID: 2253
		private bool m_AllowMSAA = true;

		// Token: 0x020001F8 RID: 504
		private struct SwapBuffer
		{
			// Token: 0x04000CEE RID: 3310
			public RTHandle rtMSAA;

			// Token: 0x04000CEF RID: 3311
			public RTHandle rtResolve;

			// Token: 0x04000CF0 RID: 3312
			public string name;

			// Token: 0x04000CF1 RID: 3313
			public int msaa;
		}
	}
}
