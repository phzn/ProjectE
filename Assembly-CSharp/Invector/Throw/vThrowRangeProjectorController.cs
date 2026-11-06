using System;
using UnityEngine;

namespace Invector.Throw
{
	// Token: 0x02000502 RID: 1282
	[RequireComponent(typeof(Projector))]
	[vClassHeader("Range Projector Controller", true, "icon_v2", false, "", openClose = false)]
	public class vThrowRangeProjectorController : vThrowVisualControlBase
	{
		// Token: 0x06001F37 RID: 7991 RVA: 0x000BA0A1 File Offset: 0x000B82A1
		protected override void OnInit(vThrowManagerBase tm)
		{
			this.projector = base.GetComponent<Projector>();
			if (tm)
			{
				tm.onSetActiveIndicator.AddListener(delegate(bool active)
				{
					this.projector.enabled = active;
				});
			}
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x000BA0D0 File Offset: 0x000B82D0
		public override void OnChangeVisual(vThrowVisualSettings settings)
		{
			if (settings != null && this.projector)
			{
				this.projector.enabled = settings.useIndicator;
				this.SetProjectorColor(settings.indicatorColor);
				this.SetProjectorSize(settings.indicatorRange);
				this.SetProjectorTexture(settings.indicatorTexture);
			}
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x000BA128 File Offset: 0x000B8328
		public void SetProjectorColor(Color color)
		{
			this.projector.material.SetColor(this.materialColorChannel, color);
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x000BA141 File Offset: 0x000B8341
		public void SetProjectorSize(float size)
		{
			if (this.tm.ObjectToThrow != null)
			{
				this.projector.orthographicSize = size * this.projectorSizeMultiplier * this.tm.ObjectToThrow.indicatorRangeMultiplier;
			}
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x000BA17A File Offset: 0x000B837A
		public void SetProjectorTexture(Texture2D texture)
		{
			this.projector.material.SetTexture(this.materialTextureChannel, texture);
		}

		// Token: 0x04001FC3 RID: 8131
		protected Projector projector;

		// Token: 0x04001FC4 RID: 8132
		public string materialTextureChannel = "_MainTex";

		// Token: 0x04001FC5 RID: 8133
		public string materialColorChannel = "_Color";

		// Token: 0x04001FC6 RID: 8134
		public float projectorSizeMultiplier = 1f;
	}
}
