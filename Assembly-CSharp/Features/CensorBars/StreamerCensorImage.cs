using System;
using Features.Inventory;
using Features.Inventory.Wearable;
using Features.MainMenu;
using ProjectExhibitionist.PhotoBooth;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace Features.CensorBars
{
	// Token: 0x020014BE RID: 5310
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Graphic))]
	public sealed class StreamerCensorImage : MonoBehaviour, IMaterialModifier
	{
		// Token: 0x0600A34B RID: 41803 RVA: 0x0043D796 File Offset: 0x0043B996
		public static bool IsSensitiveStyle(string partId)
		{
			return string.Equals(partId, "Outfit.PubicHair", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x0600A34C RID: 41804 RVA: 0x0043D7A4 File Offset: 0x0043B9A4
		public static bool IsSensitiveItem(InventoryItemSO item)
		{
			if (!(item != null) || item is PhotoBoothPhotoItemSO)
			{
				return false;
			}
			if (!item.sensitiveVisual)
			{
				WearableItemSO wearableItemSO = item as WearableItemSO;
				return wearableItemSO != null && StreamerCensorImage.IsSensitiveStyle(wearableItemSO.mcStylePartId);
			}
			return true;
		}

		// Token: 0x0600A34D RID: 41805 RVA: 0x0043D7E5 File Offset: 0x0043B9E5
		public static void BindItem(Graphic graphic, InventoryItemSO item)
		{
			StreamerCensorImage.Bind(graphic, StreamerCensorImage.IsSensitiveItem(item));
		}

		// Token: 0x0600A34E RID: 41806 RVA: 0x0043D7F4 File Offset: 0x0043B9F4
		public static void Bind(Graphic graphic, bool isSensitive = true)
		{
			if (graphic == null)
			{
				return;
			}
			StreamerCensorImage streamerCensorImage = graphic.GetComponent<StreamerCensorImage>();
			if (streamerCensorImage == null && isSensitive)
			{
				streamerCensorImage = graphic.gameObject.AddComponent<StreamerCensorImage>();
			}
			if (streamerCensorImage == null)
			{
				return;
			}
			streamerCensorImage.sensitive = isSensitive;
			graphic.SetMaterialDirty();
		}

		// Token: 0x0600A34F RID: 41807 RVA: 0x0043D840 File Offset: 0x0043BA40
		private void OnEnable()
		{
			this._graphic = base.GetComponent<Graphic>();
			GameSettingsService.SettingsChanged -= this.Refresh;
			GameSettingsService.SettingsChanged += this.Refresh;
			Canvas.preWillRenderCanvases -= this.UpdateParameters;
			Canvas.preWillRenderCanvases += this.UpdateParameters;
			this.Refresh();
		}

		// Token: 0x0600A350 RID: 41808 RVA: 0x0043D8A3 File Offset: 0x0043BAA3
		private void OnDisable()
		{
			GameSettingsService.SettingsChanged -= this.Refresh;
			Canvas.preWillRenderCanvases -= this.UpdateParameters;
			this.Refresh();
		}

		// Token: 0x0600A351 RID: 41809 RVA: 0x0043D8CD File Offset: 0x0043BACD
		private void OnDestroy()
		{
			if (this._material != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._material);
					return;
				}
				Object.DestroyImmediate(this._material);
			}
		}

		// Token: 0x0600A352 RID: 41810 RVA: 0x0043D8FB File Offset: 0x0043BAFB
		private void Refresh()
		{
			if (this._graphic != null)
			{
				this._graphic.SetMaterialDirty();
			}
		}

		// Token: 0x0600A353 RID: 41811 RVA: 0x0043D918 File Offset: 0x0043BB18
		public Material GetModifiedMaterial(Material baseMaterial)
		{
			if (!base.isActiveAndEnabled || !this.sensitive || !GameSettingsService.StreamerMode || baseMaterial == null)
			{
				return baseMaterial;
			}
			if (this._material == null)
			{
				Shader shader = Resources.Load<Shader>("StreamerUIPixelation");
				if (shader == null)
				{
					return baseMaterial;
				}
				this._material = new Material(shader)
				{
					hideFlags = HideFlags.HideAndDontSave
				};
			}
			this._material.CopyPropertiesFromMaterial(baseMaterial);
			this.UpdateParameters();
			return this._material;
		}

		// Token: 0x0600A354 RID: 41812 RVA: 0x0043D998 File Offset: 0x0043BB98
		private void UpdateParameters()
		{
			if (this._graphic == null || this._material == null || !this.sensitive || !GameSettingsService.StreamerMode)
			{
				return;
			}
			Vector4 outerUV = new Vector4(0f, 0f, 1f, 1f);
			Image image = this._graphic as Image;
			if (image != null)
			{
				Sprite sprite = ((image.overrideSprite != null) ? image.overrideSprite : image.sprite);
				if (sprite != null)
				{
					outerUV = DataUtility.GetOuterUV(sprite);
				}
			}
			else
			{
				RawImage rawImage = this._graphic as RawImage;
				if (rawImage != null)
				{
					Rect uvRect = rawImage.uvRect;
					outerUV = new Vector4(Mathf.Min(uvRect.x, uvRect.x + uvRect.width), Mathf.Min(uvRect.y, uvRect.y + uvRect.height), Mathf.Max(uvRect.x, uvRect.x + uvRect.width), Mathf.Max(uvRect.y, uvRect.y + uvRect.height));
				}
			}
			Canvas canvas = this._graphic.canvas;
			object obj = ((canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? canvas.worldCamera : null);
			this._graphic.rectTransform.GetWorldCorners(this._corners);
			object obj2 = obj;
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(obj2, this._corners[0]);
			Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(obj2, this._corners[1]);
			Vector2 vector3 = RectTransformUtility.WorldToScreenPoint(obj2, this._corners[3]);
			float num = Mathf.Max(1f, Vector2.Distance(vector, vector3));
			float num2 = Mathf.Max(1f, Vector2.Distance(vector, vector2));
			Image image2 = this._graphic as Image;
			if (image2 != null && image2.preserveAspect)
			{
				Sprite sprite2 = ((image2.overrideSprite != null) ? image2.overrideSprite : image2.sprite);
				if (sprite2 != null && sprite2.rect.width > 0f && sprite2.rect.height > 0f)
				{
					float num3 = Mathf.Min(num / sprite2.rect.width, num2 / sprite2.rect.height);
					num = Mathf.Max(1f, sprite2.rect.width * num3);
					num2 = Mathf.Max(1f, sprite2.rect.height * num3);
				}
			}
			float num4 = Mathf.InverseLerp(12f, 80f, (float)GameSettingsService.StreamerPixelSize);
			float num5 = Mathf.Max(1f, Mathf.Max(num, num2) / Mathf.Lerp(16f, 6f, num4));
			this._material.SetVector("_CensorUVRect", outerUV);
			this._material.SetVector("_CensorUVStep", new Vector4(Mathf.Abs(outerUV.z - outerUV.x) * num5 / num, Mathf.Abs(outerUV.w - outerUV.y) * num5 / num2, 0f, 0f));
		}

		// Token: 0x0400A717 RID: 42775
		[SerializeField]
		private bool sensitive = true;

		// Token: 0x0400A718 RID: 42776
		private Graphic _graphic;

		// Token: 0x0400A719 RID: 42777
		private Material _material;

		// Token: 0x0400A71A RID: 42778
		private readonly Vector3[] _corners = new Vector3[4];
	}
}
