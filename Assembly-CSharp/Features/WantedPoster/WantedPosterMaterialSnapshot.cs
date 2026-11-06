using System;
using UnityEngine;

namespace Features.WantedPoster
{
	// Token: 0x02000B71 RID: 2929
	[DisallowMultipleComponent]
	public sealed class WantedPosterMaterialSnapshot : MonoBehaviour
	{
		// Token: 0x0600533F RID: 21311 RVA: 0x00204D48 File Offset: 0x00202F48
		public void Initialize(Material material, RenderTexture texture, Renderer[] renderers, Material[] authoredMaterials)
		{
			this._material = material;
			this._texture = texture;
			this._renderers = renderers;
			this._authoredMaterials = authoredMaterials;
		}

		// Token: 0x06005340 RID: 21312 RVA: 0x00204D68 File Offset: 0x00202F68
		public void RestoreAndDispose()
		{
			if (this._disposed)
			{
				return;
			}
			if (this._renderers != null && this._authoredMaterials != null)
			{
				int num = Mathf.Min(this._renderers.Length, this._authoredMaterials.Length);
				for (int i = 0; i < num; i++)
				{
					if (this._renderers[i] != null)
					{
						this._renderers[i].sharedMaterial = this._authoredMaterials[i];
					}
				}
			}
			this.ReleaseResources();
			Object.Destroy(this);
		}

		// Token: 0x06005341 RID: 21313 RVA: 0x00204DE1 File Offset: 0x00202FE1
		private void OnDestroy()
		{
			this.ReleaseResources();
		}

		// Token: 0x06005342 RID: 21314 RVA: 0x00204DEC File Offset: 0x00202FEC
		private void ReleaseResources()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			if (this._material != null)
			{
				Object.Destroy(this._material);
			}
			if (this._texture != null)
			{
				this._texture.Release();
				Object.Destroy(this._texture);
			}
			this._material = null;
			this._texture = null;
		}

		// Token: 0x040053B9 RID: 21433
		private Material _material;

		// Token: 0x040053BA RID: 21434
		private RenderTexture _texture;

		// Token: 0x040053BB RID: 21435
		private Renderer[] _renderers;

		// Token: 0x040053BC RID: 21436
		private Material[] _authoredMaterials;

		// Token: 0x040053BD RID: 21437
		private bool _disposed;
	}
}
