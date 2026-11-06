using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x02001064 RID: 4196
	public sealed class MainMenuLogoShimmer : MonoBehaviour
	{
		// Token: 0x06007F66 RID: 32614 RVA: 0x003410E0 File Offset: 0x0033F2E0
		private void Awake()
		{
			this._logoRect = base.transform as RectTransform;
			Image component = base.GetComponent<Image>();
			if (this._logoRect == null || component == null)
			{
				base.enabled = false;
				return;
			}
			Mask mask = base.GetComponent<Mask>();
			if (mask == null)
			{
				mask = base.gameObject.AddComponent<Mask>();
			}
			mask.showMaskGraphic = true;
			GameObject gameObject = new GameObject("Project Premium Shimmer", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasRenderer),
				typeof(Image)
			});
			this._shimmerRect = gameObject.GetComponent<RectTransform>();
			this._shimmerRect.SetParent(base.transform, false);
			this._shimmerRect.anchorMin = new Vector2(0f, 0.5f);
			this._shimmerRect.anchorMax = new Vector2(0f, 1f);
			this._shimmerRect.pivot = new Vector2(0.5f, 0.5f);
			this._shimmerRect.sizeDelta = new Vector2(126f, 0f);
			this._shimmerRect.localRotation = Quaternion.Euler(0f, 0f, -14f);
			this._shimmerImage = gameObject.GetComponent<Image>();
			this._shimmerImage.raycastTarget = false;
			this._shimmerImage.sprite = MainMenuLogoShimmer.BuildGradientSprite();
			this._shimmerImage.type = Image.Type.Simple;
			this._shimmerImage.color = new Color(1f, 0.98f, 0.91f, 0f);
			base.StartCoroutine(this.ShimmerLoop());
		}

		// Token: 0x06007F67 RID: 32615 RVA: 0x00341289 File Offset: 0x0033F489
		public void PlayNow()
		{
			if (base.isActiveAndEnabled && !this._sweepInProgress)
			{
				base.StartCoroutine(this.Sweep());
			}
		}

		// Token: 0x06007F68 RID: 32616 RVA: 0x003412A8 File Offset: 0x0033F4A8
		private IEnumerator ShimmerLoop()
		{
			yield return new WaitForSecondsRealtime(Random.Range(1.5f, 3.5f));
			while (base.isActiveAndEnabled)
			{
				yield return this.Sweep();
				yield return new WaitForSecondsRealtime(Random.Range(4.5f, 10f));
			}
			yield break;
		}

		// Token: 0x06007F69 RID: 32617 RVA: 0x003412B7 File Offset: 0x0033F4B7
		private IEnumerator Sweep()
		{
			this._sweepInProgress = true;
			float width = this._logoRect.rect.width;
			float startX = -width * 0.62f;
			float endX = width * 0.62f;
			float elapsed = 0f;
			while (elapsed < 1.15f)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / 1.15f);
				float num2 = num * num * (3f - 2f * num);
				this._shimmerRect.anchoredPosition = new Vector2(Mathf.Lerp(startX, endX, num2), 0f);
				Color color = this._shimmerImage.color;
				color.a = Mathf.Sin(num * 3.1415927f) * 0.86f;
				this._shimmerImage.color = color;
				yield return null;
			}
			Color color2 = this._shimmerImage.color;
			color2.a = 0f;
			this._shimmerImage.color = color2;
			this._sweepInProgress = false;
			yield break;
		}

		// Token: 0x06007F6A RID: 32618 RVA: 0x003412C8 File Offset: 0x0033F4C8
		private static Sprite BuildGradientSprite()
		{
			Texture2D texture2D = new Texture2D(64, 1, TextureFormat.RGBA32, false)
			{
				name = "Project Shimmer Gradient",
				wrapMode = TextureWrapMode.Clamp,
				filterMode = FilterMode.Bilinear
			};
			for (int i = 0; i < 64; i++)
			{
				float num = Mathf.Abs((float)i / 63f - 0.5f) * 2f;
				float num2 = Mathf.Pow(1f - Mathf.Clamp01(num), 2.4f);
				texture2D.SetPixel(i, 0, new Color(1f, 1f, 1f, num2));
			}
			texture2D.Apply(false, true);
			return Sprite.Create(texture2D, new Rect(0f, 0f, 64f, 1f), new Vector2(0.5f, 0.5f), 100f);
		}

		// Token: 0x040082DB RID: 33499
		private RectTransform _logoRect;

		// Token: 0x040082DC RID: 33500
		private RectTransform _shimmerRect;

		// Token: 0x040082DD RID: 33501
		private Image _shimmerImage;

		// Token: 0x040082DE RID: 33502
		private bool _sweepInProgress;
	}
}
