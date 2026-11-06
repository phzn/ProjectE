using System;
using UnityEngine;

namespace Invector.Throw
{
	// Token: 0x020004F0 RID: 1264
	[RequireComponent(typeof(LineRenderer))]
	[vClassHeader("Line Controller", true, "icon_v2", false, "", openClose = false)]
	public class vThrowLineController : vThrowVisualControlBase
	{
		// Token: 0x06001E77 RID: 7799 RVA: 0x000B7BF6 File Offset: 0x000B5DF6
		protected override void OnInit(vThrowManagerBase tm)
		{
			this.line = base.GetComponent<LineRenderer>();
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000B7C04 File Offset: 0x000B5E04
		public override void OnChangeVisual(vThrowVisualSettings settings)
		{
			if (settings != null && this.line)
			{
				this.line.enabled = settings.useLine;
				this.SetLineColor(settings.lineRendererColor);
				this.SetLineWidth(settings.lineRendererWidth);
				this.SetLineTexture(settings.lineTexture);
				this.SetLineAlignment(settings.lineAlignment);
				this.SetLineTextureMode(settings.lineTextureMode);
				this.SetLineTextureScale(settings.lineTile);
				this.SetLineTextureOffset(settings.lineOffset);
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000B7C8C File Offset: 0x000B5E8C
		public virtual void SetLineColor(Color color)
		{
			this.line.material.SetColor(this.materialColorChannel, color);
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000B7CA5 File Offset: 0x000B5EA5
		public virtual void SetLineAlignment(LineAlignment alignment)
		{
			this.line.alignment = alignment;
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000B7CB3 File Offset: 0x000B5EB3
		public virtual void SetLineWidth(float size)
		{
			this.line.widthMultiplier = size * this.lineWidthMultiplier;
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000B7CC8 File Offset: 0x000B5EC8
		public virtual void SetLineTexture(Texture2D texture)
		{
			this.line.material.SetTexture(this.materialTextureChannel, texture);
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000B7CE1 File Offset: 0x000B5EE1
		public virtual void SetLineTextureMode(LineTextureMode lineTextureMode)
		{
			this.line.textureMode = lineTextureMode;
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000B7CEF File Offset: 0x000B5EEF
		public virtual void SetLineTextureScale(Vector2 scale)
		{
			this.line.material.SetTextureScale(this.materialTextureChannel, scale);
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000B7D08 File Offset: 0x000B5F08
		public virtual void SetLineTextureOffset(Vector2 offset)
		{
			this.line.material.SetTextureOffset(this.materialTextureChannel, offset);
		}

		// Token: 0x04001F5F RID: 8031
		protected LineRenderer line;

		// Token: 0x04001F60 RID: 8032
		public string materialTextureChannel = "_MainTex";

		// Token: 0x04001F61 RID: 8033
		public string materialColorChannel = "_Color";

		// Token: 0x04001F62 RID: 8034
		public float lineWidthMultiplier = 1f;
	}
}
