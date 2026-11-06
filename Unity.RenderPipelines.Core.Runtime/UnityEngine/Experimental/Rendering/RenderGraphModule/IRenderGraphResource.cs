using System;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200002A RID: 42
	internal class IRenderGraphResource
	{
		// Token: 0x060001BD RID: 445 RVA: 0x00009248 File Offset: 0x00007448
		public virtual void Reset(IRenderGraphResourcePool pool)
		{
			this.imported = false;
			this.shared = false;
			this.sharedExplicitRelease = false;
			this.cachedHash = -1;
			this.transientPassIndex = -1;
			this.sharedResourceLastFrameUsed = -1;
			this.requestFallBack = false;
			this.writeCount = 0U;
			this.m_Pool = pool;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009294 File Offset: 0x00007494
		public virtual string GetName()
		{
			return "";
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000929B File Offset: 0x0000749B
		public virtual bool IsCreated()
		{
			return false;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000929E File Offset: 0x0000749E
		public virtual void IncrementWriteCount()
		{
			this.writeCount += 1U;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000092AE File Offset: 0x000074AE
		public virtual bool NeedsFallBack()
		{
			return this.requestFallBack && this.writeCount == 0U;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000092C3 File Offset: 0x000074C3
		public virtual void CreatePooledGraphicsResource()
		{
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000092C5 File Offset: 0x000074C5
		public virtual void CreateGraphicsResource(string name = "")
		{
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000092C7 File Offset: 0x000074C7
		public virtual void ReleasePooledGraphicsResource(int frameIndex)
		{
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000092C9 File Offset: 0x000074C9
		public virtual void ReleaseGraphicsResource()
		{
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x000092CB File Offset: 0x000074CB
		public virtual void LogCreation(RenderGraphLogger logger)
		{
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x000092CD File Offset: 0x000074CD
		public virtual void LogRelease(RenderGraphLogger logger)
		{
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000092CF File Offset: 0x000074CF
		public virtual int GetSortIndex()
		{
			return 0;
		}

		// Token: 0x040000E5 RID: 229
		public bool imported;

		// Token: 0x040000E6 RID: 230
		public bool shared;

		// Token: 0x040000E7 RID: 231
		public bool sharedExplicitRelease;

		// Token: 0x040000E8 RID: 232
		public bool requestFallBack;

		// Token: 0x040000E9 RID: 233
		public uint writeCount;

		// Token: 0x040000EA RID: 234
		public int cachedHash;

		// Token: 0x040000EB RID: 235
		public int transientPassIndex;

		// Token: 0x040000EC RID: 236
		public int sharedResourceLastFrameUsed;

		// Token: 0x040000ED RID: 237
		protected IRenderGraphResourcePool m_Pool;
	}
}
