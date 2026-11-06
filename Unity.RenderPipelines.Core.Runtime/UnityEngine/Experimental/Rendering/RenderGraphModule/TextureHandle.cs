using System;
using System.Diagnostics;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x0200002C RID: 44
	[DebuggerDisplay("Texture ({handle.index})")]
	public struct TextureHandle
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00009315 File Offset: 0x00007515
		public static TextureHandle nullHandle
		{
			get
			{
				return TextureHandle.s_NullHandle;
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000931C File Offset: 0x0000751C
		internal TextureHandle(int handle, bool shared = false)
		{
			this.handle = new ResourceHandle(handle, RenderGraphResourceType.Texture, shared);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000932C File Offset: 0x0000752C
		public static implicit operator RenderTargetIdentifier(TextureHandle texture)
		{
			if (!texture.IsValid())
			{
				return default(RenderTargetIdentifier);
			}
			return RenderGraphResourceRegistry.current.GetTexture(in texture);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000935D File Offset: 0x0000755D
		public static implicit operator Texture(TextureHandle texture)
		{
			return texture.IsValid() ? RenderGraphResourceRegistry.current.GetTexture(in texture) : null;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000937C File Offset: 0x0000757C
		public static implicit operator RenderTexture(TextureHandle texture)
		{
			return texture.IsValid() ? RenderGraphResourceRegistry.current.GetTexture(in texture) : null;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000939B File Offset: 0x0000759B
		public static implicit operator RTHandle(TextureHandle texture)
		{
			if (!texture.IsValid())
			{
				return null;
			}
			return RenderGraphResourceRegistry.current.GetTexture(in texture);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000093B4 File Offset: 0x000075B4
		public bool IsValid()
		{
			return this.handle.IsValid();
		}

		// Token: 0x040000F0 RID: 240
		private static TextureHandle s_NullHandle;

		// Token: 0x040000F1 RID: 241
		internal ResourceHandle handle;
	}
}
