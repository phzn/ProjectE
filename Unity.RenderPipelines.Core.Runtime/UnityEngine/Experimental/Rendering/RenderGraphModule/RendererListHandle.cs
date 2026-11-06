using System;
using System.Diagnostics;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000026 RID: 38
	[DebuggerDisplay("RendererList ({handle})")]
	public struct RendererListHandle
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000090E4 File Offset: 0x000072E4
		// (set) Token: 0x060001AE RID: 430 RVA: 0x000090EC File Offset: 0x000072EC
		internal int handle { readonly get; private set; }

		// Token: 0x060001AF RID: 431 RVA: 0x000090F5 File Offset: 0x000072F5
		internal RendererListHandle(int handle)
		{
			this.handle = handle;
			this.m_IsValid = true;
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00009105 File Offset: 0x00007305
		public static implicit operator int(RendererListHandle handle)
		{
			return handle.handle;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000910E File Offset: 0x0000730E
		public static implicit operator RendererList(RendererListHandle rendererList)
		{
			if (!rendererList.IsValid())
			{
				return RendererList.nullRendererList;
			}
			return RenderGraphResourceRegistry.current.GetRendererList(in rendererList);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000912B File Offset: 0x0000732B
		public bool IsValid()
		{
			return this.m_IsValid;
		}

		// Token: 0x040000D7 RID: 215
		private bool m_IsValid;
	}
}
