using System;
using ProjectExhibitionist.Minimap;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Apps.Maps
{
	// Token: 0x0200147B RID: 5243
	internal sealed class CielOSStaticCityMap : IDisposable
	{
		// Token: 0x0600A198 RID: 41368 RVA: 0x00430A02 File Offset: 0x0042EC02
		public CielOSStaticCityMap(RawImage image)
		{
			this._image = image;
		}

		// Token: 0x0600A199 RID: 41369 RVA: 0x00430A14 File Offset: 0x0042EC14
		public void SetVariant(int variant)
		{
			this._variant = Mathf.Clamp(variant, 0, 4);
			this.ReleaseTexture();
			string text = CielOSCityMapLayout.ResourcePath(this._variant);
			this._loadedTexture = Resources.Load<Texture2D>(text);
			if (this._image == null)
			{
				return;
			}
			this._image.texture = this._loadedTexture;
			this._image.uvRect = new Rect(0f, 0f, 1f, 1f);
			this._image.color = ((this._loadedTexture != null) ? Color.white : new Color(0.09f, 0.1f, 0.12f, 1f));
		}

		// Token: 0x0600A19A RID: 41370 RVA: 0x00430ACC File Offset: 0x0042ECCC
		public Vector2 GetPoiLocalPosition(MiniMapPoiKind kind)
		{
			CielOSCityMapLayout.Landmark landmark;
			if (this._image == null || !CielOSCityMapLayout.TryGetLandmark(this._variant, kind, out landmark))
			{
				return Vector2.zero;
			}
			Rect rect = this._image.rectTransform.rect;
			Vector2 normalizedCenter = landmark.NormalizedCenter;
			return new Vector2(Mathf.Lerp(rect.xMin, rect.xMax, normalizedCenter.x), Mathf.Lerp(rect.yMin, rect.yMax, normalizedCenter.y));
		}

		// Token: 0x0600A19B RID: 41371 RVA: 0x00004F31 File Offset: 0x00003131
		public void RefreshProjection()
		{
		}

		// Token: 0x0600A19C RID: 41372 RVA: 0x00430B4D File Offset: 0x0042ED4D
		public void Dispose()
		{
			this.ReleaseTexture();
		}

		// Token: 0x0600A19D RID: 41373 RVA: 0x00430B55 File Offset: 0x0042ED55
		private void ReleaseTexture()
		{
			if (this._image != null)
			{
				this._image.texture = null;
			}
			if (this._loadedTexture != null)
			{
				Resources.UnloadAsset(this._loadedTexture);
				this._loadedTexture = null;
			}
		}

		// Token: 0x0400A545 RID: 42309
		public const int VariantCount = 5;

		// Token: 0x0400A546 RID: 42310
		private readonly RawImage _image;

		// Token: 0x0400A547 RID: 42311
		private Texture2D _loadedTexture;

		// Token: 0x0400A548 RID: 42312
		private int _variant;
	}
}
