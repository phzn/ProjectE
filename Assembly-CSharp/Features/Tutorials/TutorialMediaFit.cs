using System;
using UnityEngine;

namespace Features.Tutorials
{
	// Token: 0x02000BDE RID: 3038
	[ExecuteAlways]
	public sealed class TutorialMediaFit : MonoBehaviour
	{
		// Token: 0x0600576B RID: 22379 RVA: 0x00222259 File Offset: 0x00220459
		public void Configure(RectTransform artwork)
		{
			this._artwork = artwork;
			this.Fit();
		}

		// Token: 0x0600576C RID: 22380 RVA: 0x00222268 File Offset: 0x00220468
		private void OnRectTransformDimensionsChange()
		{
			this.Fit();
		}

		// Token: 0x0600576D RID: 22381 RVA: 0x00222270 File Offset: 0x00220470
		private void Fit()
		{
			if (this._artwork == null)
			{
				return;
			}
			RectTransform rectTransform = (RectTransform)base.transform;
			float num = Mathf.Min(rectTransform.rect.width / 1280f, rectTransform.rect.height / 720f);
			this._artwork.localScale = Vector3.one * Mathf.Max(0.001f, num);
		}

		// Token: 0x040057D8 RID: 22488
		private RectTransform _artwork;
	}
}
