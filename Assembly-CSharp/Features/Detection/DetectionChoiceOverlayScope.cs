using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Features.WardrobeCombos;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Detection
{
	// Token: 0x02001269 RID: 4713
	public sealed class DetectionChoiceOverlayScope : IDisposable
	{
		// Token: 0x060092AF RID: 37551 RVA: 0x003C2880 File Offset: 0x003C0A80
		public DetectionChoiceOverlayScope(Canvas canvas)
		{
			this._canvas = canvas;
			if (canvas == null)
			{
				return;
			}
			this._overrideSorting = canvas.overrideSorting;
			this._sortingOrder = canvas.sortingOrder;
			canvas.overrideSorting = true;
			canvas.sortingOrder = Mathf.Max(canvas.sortingOrder, 100);
			Shader shader = Resources.Load<Shader>("DetectionChoiceImage");
			Shader shader2 = Resources.Load<Shader>("DetectionChoiceText");
			foreach (Graphic graphic in canvas.GetComponentsInChildren<Graphic>(true))
			{
				if (DetectionChoiceOverlayScope.IsObscuringBackdrop(graphic, canvas))
				{
					Image image = (Image)graphic;
					this._backdrops.Add(new ValueTuple<Image, bool>(image, image.enabled));
					image.enabled = false;
				}
				else
				{
					TextMeshProUGUI textMeshProUGUI = graphic as TextMeshProUGUI;
					Material material = ((textMeshProUGUI != null) ? textMeshProUGUI.fontSharedMaterial : graphic.material);
					Shader shader3 = ((textMeshProUGUI != null) ? shader2 : shader);
					if (!(material == null) && !(shader3 == null) && (!(textMeshProUGUI == null) || !(material.shader.name != "UI/Default") || !(material.shader.name != "UI/AlwaysOnTop")))
					{
						Material material2 = new Material(material)
						{
							name = material.name + " (Encounter overlay)",
							hideFlags = HideFlags.HideAndDontSave,
							shader = shader3,
							renderQueue = 4000
						};
						if (textMeshProUGUI != null)
						{
							if (textMeshProUGUI.font != null)
							{
								material2.mainTexture = textMeshProUGUI.font.atlasTexture;
							}
							textMeshProUGUI.fontSharedMaterial = material2;
						}
						else
						{
							graphic.material = material2;
						}
						this._entries.Add(new DetectionChoiceOverlayScope.Entry
						{
							Graphic = graphic,
							Original = material,
							Overlay = material2
						});
						BlindfoldUiStencil blindfoldUiStencil;
						if (!graphic.TryGetComponent<BlindfoldUiStencil>(out blindfoldUiStencil))
						{
							this._ownedStencilMarkers.Add(graphic.gameObject.AddComponent<BlindfoldUiStencil>());
						}
					}
				}
			}
		}

		// Token: 0x060092B0 RID: 37552 RVA: 0x003C2AC0 File Offset: 0x003C0CC0
		private static bool IsObscuringBackdrop(Graphic graphic, Canvas canvas)
		{
			Image image = graphic as Image;
			if (image == null || image.transform.parent != canvas.transform || image.name != "Panel" || image.sprite != null)
			{
				return false;
			}
			Color color = image.color;
			if (color.r > 0.05f || color.g > 0.05f || color.b > 0.05f || color.a < 0.5f)
			{
				return false;
			}
			Rect rect = ((RectTransform)canvas.transform).rect;
			Rect rect2 = image.rectTransform.rect;
			return rect2.width >= rect.width && rect2.height >= rect.height;
		}

		// Token: 0x060092B1 RID: 37553 RVA: 0x003C2B90 File Offset: 0x003C0D90
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			foreach (BlindfoldUiStencil blindfoldUiStencil in this._ownedStencilMarkers)
			{
				if (!(blindfoldUiStencil == null))
				{
					blindfoldUiStencil.enabled = false;
					if (Application.isPlaying)
					{
						Object.Destroy(blindfoldUiStencil);
					}
					else
					{
						Object.DestroyImmediate(blindfoldUiStencil);
					}
				}
			}
			this._ownedStencilMarkers.Clear();
			foreach (DetectionChoiceOverlayScope.Entry entry in this._entries)
			{
				if (entry.Graphic != null)
				{
					TextMeshProUGUI textMeshProUGUI = entry.Graphic as TextMeshProUGUI;
					if (textMeshProUGUI != null)
					{
						if (textMeshProUGUI.fontSharedMaterial == entry.Overlay)
						{
							textMeshProUGUI.fontSharedMaterial = entry.Original;
						}
					}
					else if (entry.Graphic.material == entry.Overlay)
					{
						entry.Graphic.material = entry.Original;
					}
				}
				if (!(entry.Overlay == null))
				{
					if (Application.isPlaying)
					{
						Object.Destroy(entry.Overlay);
					}
					else
					{
						Object.DestroyImmediate(entry.Overlay);
					}
				}
			}
			this._entries.Clear();
			foreach (ValueTuple<Image, bool> valueTuple in this._backdrops)
			{
				if (valueTuple.Item1 != null)
				{
					valueTuple.Item1.enabled = valueTuple.Item2;
				}
			}
			this._backdrops.Clear();
			if (this._canvas != null)
			{
				this._canvas.overrideSorting = this._overrideSorting;
				this._canvas.sortingOrder = this._sortingOrder;
			}
		}

		// Token: 0x0400972D RID: 38701
		private readonly List<DetectionChoiceOverlayScope.Entry> _entries = new List<DetectionChoiceOverlayScope.Entry>();

		// Token: 0x0400972E RID: 38702
		private readonly List<BlindfoldUiStencil> _ownedStencilMarkers = new List<BlindfoldUiStencil>();

		// Token: 0x0400972F RID: 38703
		[TupleElementNames(new string[] { "Image", "Enabled" })]
		private readonly List<ValueTuple<Image, bool>> _backdrops = new List<ValueTuple<Image, bool>>();

		// Token: 0x04009730 RID: 38704
		private readonly Canvas _canvas;

		// Token: 0x04009731 RID: 38705
		private readonly bool _overrideSorting;

		// Token: 0x04009732 RID: 38706
		private readonly int _sortingOrder;

		// Token: 0x04009733 RID: 38707
		private bool _disposed;

		// Token: 0x0200126A RID: 4714
		private struct Entry
		{
			// Token: 0x04009734 RID: 38708
			public Graphic Graphic;

			// Token: 0x04009735 RID: 38709
			public Material Original;

			// Token: 0x04009736 RID: 38710
			public Material Overlay;
		}
	}
}
