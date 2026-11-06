using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B6E RID: 2926
	internal sealed class WantedPosterCaptureVisibility
	{
		// Token: 0x06005329 RID: 21289 RVA: 0x002046F4 File Offset: 0x002028F4
		public void HideRegisteredPosters(int cameraCullingMask = -1)
		{
			IReadOnlyList<WantedPoster> registeredPosters = WantedPosterPhotoSystem.RegisteredPosters;
			for (int i = 0; i < registeredPosters.Count; i++)
			{
				WantedPoster wantedPoster = registeredPosters[i];
				if (wantedPoster != null && (cameraCullingMask & (1 << wantedPoster.gameObject.layer)) != 0)
				{
					this.Hide(wantedPoster.Renderer);
				}
			}
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x00204749 File Offset: 0x00202949
		public bool HideIfPoster(Renderer renderer)
		{
			if (renderer == null)
			{
				return false;
			}
			if (renderer.GetComponentInParent<WantedPoster>() == null && renderer.GetComponentInParent<WantedPosterBreakObject>() == null)
			{
				return false;
			}
			this.Hide(renderer);
			return true;
		}

		// Token: 0x0600532B RID: 21291 RVA: 0x0020477C File Offset: 0x0020297C
		private void Hide(Renderer renderer)
		{
			if (renderer == null || !renderer.enabled || !renderer.gameObject.activeInHierarchy || renderer.forceRenderingOff)
			{
				return;
			}
			this._hiddenRenderers.Add(renderer);
			renderer.forceRenderingOff = true;
		}

		// Token: 0x0600532C RID: 21292 RVA: 0x002047B8 File Offset: 0x002029B8
		public void Restore()
		{
			for (int i = this._hiddenRenderers.Count - 1; i >= 0; i--)
			{
				if (this._hiddenRenderers[i] != null)
				{
					this._hiddenRenderers[i].forceRenderingOff = false;
				}
			}
			this._hiddenRenderers.Clear();
		}

		// Token: 0x040053AF RID: 21423
		private readonly List<Renderer> _hiddenRenderers = new List<Renderer>();
	}
}
