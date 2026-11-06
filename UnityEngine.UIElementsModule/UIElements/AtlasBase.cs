using System;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x02000022 RID: 34
	internal abstract class AtlasBase
	{
		// Token: 0x0600015C RID: 348 RVA: 0x00003CAC File Offset: 0x00001EAC
		public virtual bool TryGetAtlas(VisualElement ctx, Texture2D src, out TextureId atlas, out RectInt atlasRect)
		{
			atlas = TextureId.invalid;
			atlasRect = default(RectInt);
			return false;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public virtual void ReturnAtlas(VisualElement ctx, Texture2D src, TextureId atlas)
		{
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public virtual void Reset()
		{
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00003CD2 File Offset: 0x00001ED2
		protected virtual void OnAssignedToPanel(IPanel panel)
		{
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00003CD2 File Offset: 0x00001ED2
		protected virtual void OnRemovedFromPanel(IPanel panel)
		{
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00003CD2 File Offset: 0x00001ED2
		protected virtual void OnUpdateDynamicTextures(IPanel panel)
		{
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00003CD5 File Offset: 0x00001ED5
		internal void InvokeAssignedToPanel(IPanel panel)
		{
			this.OnAssignedToPanel(panel);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00003CE0 File Offset: 0x00001EE0
		internal void InvokeRemovedFromPanel(IPanel panel)
		{
			this.OnRemovedFromPanel(panel);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00003CEB File Offset: 0x00001EEB
		internal void InvokeUpdateDynamicTextures(IPanel panel)
		{
			this.OnUpdateDynamicTextures(panel);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00003CF8 File Offset: 0x00001EF8
		protected static void RepaintTexturedElements(IPanel panel)
		{
			Panel panel2 = panel as Panel;
			UIRRepaintUpdater uirrepaintUpdater = ((panel2 != null) ? panel2.GetUpdater(VisualTreeUpdatePhase.Repaint) : null) as UIRRepaintUpdater;
			if (uirrepaintUpdater != null)
			{
				RenderChain renderChain = uirrepaintUpdater.renderChain;
				if (renderChain != null)
				{
					renderChain.RepaintTexturedElements();
				}
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00003D38 File Offset: 0x00001F38
		protected TextureId AllocateDynamicTexture()
		{
			return this.textureRegistry.AllocAndAcquireDynamic();
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00003D55 File Offset: 0x00001F55
		protected void FreeDynamicTexture(TextureId id)
		{
			this.textureRegistry.Release(id);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00003D65 File Offset: 0x00001F65
		protected void SetDynamicTexture(TextureId id, Texture texture)
		{
			this.textureRegistry.UpdateDynamic(id, texture);
		}

		// Token: 0x0400005D RID: 93
		internal TextureRegistry textureRegistry = TextureRegistry.instance;
	}
}
