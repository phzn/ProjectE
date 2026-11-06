using System;
using System.Collections;
using System.Collections.Generic;
using DA_Assets.DAG;
using ExhibitionistDemo.UI;
using Features.Inventory;
using Features.Localization;
using Features.WardrobeFeedback;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.WardrobeCombos
{
	// Token: 0x02000B5B RID: 2907
	public sealed class WardrobeComboHUD : MonoBehaviour
	{
		// Token: 0x0600527F RID: 21119 RVA: 0x001FF7A4 File Offset: 0x001FD9A4
		private void Awake()
		{
			this.BuildDefinitions();
			this.BuildRevealCanvas();
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x001FF7B4 File Offset: 0x001FD9B4
		private void Update()
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				this.ClearPresentation();
				return;
			}
			if (this._revealRoutine == null && this._revealQueue.Count > 0)
			{
				this._revealRoutine = base.StartCoroutine(this.RevealQueueRoutine());
			}
			if (this._badgeRoot != null && !this._badgeRoot.gameObject.activeSelf)
			{
				this._badgeRoot.gameObject.SetActive(true);
				this.RefreshBadgeVisibility();
			}
			if (this._badgeRoot != null || Time.unscaledTime < this._nextHudResolveTime)
			{
				return;
			}
			this._nextHudResolveTime = Time.unscaledTime + 0.5f;
			this.EnsureHudBadges();
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x001FF864 File Offset: 0x001FDA64
		private void OnDestroy()
		{
			if (this._badgeRoot != null)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._badgeRoot.gameObject);
				}
				else
				{
					Object.DestroyImmediate(this._badgeRoot.gameObject);
				}
			}
			for (int i = 0; i < this._views.Count; i++)
			{
				Sprite iconSprite = this._views[i].iconSprite;
				if (!(iconSprite == null))
				{
					Texture2D texture = iconSprite.texture;
					if (Application.isPlaying)
					{
						Object.Destroy(iconSprite);
						if (texture != null)
						{
							Object.Destroy(texture);
						}
					}
					else
					{
						Object.DestroyImmediate(iconSprite);
						if (texture != null)
						{
							Object.DestroyImmediate(texture);
						}
					}
				}
			}
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x001FF913 File Offset: 0x001FDB13
		public void SetActiveCombos(WardrobeCombo active)
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				this.ClearPresentation();
				return;
			}
			this._active = active;
			this.EnsureHudBadges();
			this.RefreshBadgeVisibility();
		}

		// Token: 0x06005283 RID: 21123 RVA: 0x001FF938 File Offset: 0x001FDB38
		public void QueueReveal(WardrobeCombo combo)
		{
			if (WardrobeGameplayContext.IsPresentationOnly(this))
			{
				this.ClearPresentation();
				return;
			}
			for (int i = 0; i < this._views.Count; i++)
			{
				if ((combo & this._views[i].combo) != WardrobeCombo.None)
				{
					this._revealQueue.Enqueue(this._views[i].combo);
				}
			}
		}

		// Token: 0x06005284 RID: 21124 RVA: 0x001FF99C File Offset: 0x001FDB9C
		private void ClearPresentation()
		{
			this._active = WardrobeCombo.None;
			this._revealQueue.Clear();
			if (this._revealRoutine != null)
			{
				base.StopCoroutine(this._revealRoutine);
				this._revealRoutine = null;
			}
			if (this._revealGroup != null)
			{
				this._revealGroup.alpha = 0f;
			}
			if (this._revealCard != null)
			{
				this._revealCard.gameObject.SetActive(false);
			}
			if (this._badgeRoot != null)
			{
				this._badgeRoot.gameObject.SetActive(false);
			}
		}

		// Token: 0x06005285 RID: 21125 RVA: 0x001FFA32 File Offset: 0x001FDC32
		private IEnumerator RevealQueueRoutine()
		{
			while (this._revealQueue.Count > 0)
			{
				WardrobeComboHUD.ComboView comboView = this.FindView(this._revealQueue.Dequeue());
				if (comboView != null)
				{
					LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this._revealTitle, "gameplay_residuals", comboView.title, null);
					if (localizedGameText != null)
					{
						localizedGameText.FitTranslationToRectangle(false);
					}
					this._revealTitle.color = WardrobeComboHUD.Ivory;
					LocalizedGameText localizedGameText2 = LocalizedGameText.BindSource(this._revealDescription, "gameplay_residuals", comboView.description, null);
					if (localizedGameText2 != null)
					{
						localizedGameText2.FitTranslationToRectangle(false);
					}
					this._revealAccent.color = WardrobeComboHUD.Accent;
					this._revealIcon.sprite = WardrobeComboHUD.GetIconSprite(comboView);
					this._revealCard.gameObject.SetActive(true);
					WardrobeFeedback.PlayComboReveal(WardrobeComboHUD.Accent);
					yield return this.AnimateReveal(0f, 1f, 0.24f, true);
					yield return new WaitForSecondsRealtime(1.9f);
					yield return this.AnimateReveal(1f, 0f, 0.42f, false);
					this._revealCard.gameObject.SetActive(false);
					yield return new WaitForSecondsRealtime(0.15f);
				}
			}
			this._revealRoutine = null;
			yield break;
		}

		// Token: 0x06005286 RID: 21126 RVA: 0x001FFA41 File Offset: 0x001FDC41
		private IEnumerator AnimateReveal(float from, float to, float duration, bool entering)
		{
			float elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.unscaledDeltaTime;
				float num = Mathf.Clamp01(elapsed / duration);
				float num2 = num * num * (3f - 2f * num);
				this._revealGroup.alpha = Mathf.Lerp(from, to, num2);
				float num3 = (entering ? Mathf.Lerp(0.975f, 1f, num2) : Mathf.Lerp(1f, 1.015f, num2));
				this._revealCard.localScale = new Vector3(num3, num3, 1f);
				yield return null;
			}
			this._revealGroup.alpha = to;
			yield break;
		}

		// Token: 0x06005287 RID: 21127 RVA: 0x001FFA70 File Offset: 0x001FDC70
		private void BuildDefinitions()
		{
			this._views.Add(new WardrobeComboHUD.ComboView
			{
				combo = WardrobeCombo.HiddenIdentity,
				title = "HIDDEN IDENTITY",
				description = "Civilian final detection blocked  •  Reporters ignore you  •  Police still react",
				glyph = WardrobeComboHUD.PerkGlyph.Incognito
			});
			this._views.Add(new WardrobeComboHUD.ComboView
			{
				combo = WardrobeCombo.Athletic,
				title = "ATHLETIC",
				description = "Infinite stamina while the gym outfit is complete",
				glyph = WardrobeComboHUD.PerkGlyph.AthleticShoe
			});
			this._views.Add(new WardrobeComboHUD.ComboView
			{
				combo = WardrobeCombo.Kinky,
				title = "KINKY",
				description = "Civilian final detection blocked  •  Reporters and police still react",
				glyph = WardrobeComboHUD.PerkGlyph.RopeLink
			});
			this._views.Add(new WardrobeComboHUD.ComboView
			{
				combo = WardrobeCombo.Lucky,
				title = "LUCKY",
				description = "Invulnerable  •  Reporters and police ignore you  •  Civilian detection remains active",
				glyph = WardrobeComboHUD.PerkGlyph.Clover
			});
			this._views.Add(new WardrobeComboHUD.ComboView
			{
				combo = WardrobeCombo.Ghost,
				title = "GHOST",
				description = "NPCs scream and flee on sight; no photos or encounters while worn",
				glyph = WardrobeComboHUD.PerkGlyph.Ghost
			});
			this._views.Add(new WardrobeComboHUD.ComboView
			{
				combo = WardrobeCombo.Blind,
				title = "BLIND",
				description = "See only nearby  •  NPCs beyond 3 m cannot catch you  •  Police still react",
				glyph = WardrobeComboHUD.PerkGlyph.Blindfold
			});
		}

		// Token: 0x06005288 RID: 21128 RVA: 0x001FFBB8 File Offset: 0x001FDDB8
		private bool EnsureHudBadges()
		{
			if (this._badgeRoot != null)
			{
				this._badgeRoot.gameObject.SetActive(true);
				return true;
			}
			DistanceHUD distanceHUD = Object.FindObjectOfType<DistanceHUD>(true);
			RectTransform rectTransform = ((distanceHUD != null) ? (distanceHUD.transform as RectTransform) : null);
			if (rectTransform == null)
			{
				return false;
			}
			GameObject gameObject = new GameObject("Wardrobe Perk Icons", new Type[]
			{
				typeof(RectTransform),
				typeof(HorizontalLayoutGroup),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(rectTransform, false);
			this._badgeRoot = gameObject.GetComponent<RectTransform>();
			this._badgeRoot.anchorMin = new Vector2(0f, 1f);
			this._badgeRoot.anchorMax = new Vector2(0f, 1f);
			this._badgeRoot.pivot = new Vector2(1f, 1f);
			this._badgeRoot.anchoredPosition = new Vector2(-12f, 0f);
			this._badgeRoot.sizeDelta = new Vector2((float)this._views.Count * 42f - 6f, 38f);
			gameObject.GetComponent<LayoutElement>().ignoreLayout = true;
			HorizontalLayoutGroup component = gameObject.GetComponent<HorizontalLayoutGroup>();
			component.childAlignment = TextAnchor.UpperRight;
			component.spacing = 6f;
			component.childControlWidth = false;
			component.childControlHeight = false;
			component.childForceExpandWidth = false;
			component.childForceExpandHeight = false;
			for (int i = 0; i < this._views.Count; i++)
			{
				this.BuildBadge(this._views[i]);
			}
			this.RefreshBadgeVisibility();
			return true;
		}

		// Token: 0x06005289 RID: 21129 RVA: 0x001FFD64 File Offset: 0x001FDF64
		private void BuildBadge(WardrobeComboHUD.ComboView view)
		{
			GameObject gameObject = new GameObject(view.title, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(LayoutElement),
				typeof(Shadow)
			});
			gameObject.transform.SetParent(this._badgeRoot, false);
			gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(36f, 36f);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.minWidth = (component.preferredWidth = 36f);
			component.minHeight = (component.preferredHeight = 36f);
			Image component2 = gameObject.GetComponent<Image>();
			component2.sprite = WardrobeComboHUD.GetIconSprite(view);
			component2.preserveAspect = true;
			component2.raycastTarget = false;
			Shadow component3 = gameObject.GetComponent<Shadow>();
			component3.effectColor = new Color(0f, 0f, 0f, 0.58f);
			component3.effectDistance = new Vector2(1f, -1f);
			view.badge = gameObject;
			gameObject.SetActive(false);
		}

		// Token: 0x0600528A RID: 21130 RVA: 0x001FFE78 File Offset: 0x001FE078
		private void RefreshBadgeVisibility()
		{
			if (this._badgeRoot == null)
			{
				return;
			}
			for (int i = 0; i < this._views.Count; i++)
			{
				WardrobeComboHUD.ComboView comboView = this._views[i];
				if (comboView.badge != null)
				{
					comboView.badge.SetActive((this._active & comboView.combo) > WardrobeCombo.None);
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this._badgeRoot);
		}

		// Token: 0x0600528B RID: 21131 RVA: 0x001FFEEC File Offset: 0x001FE0EC
		private void BuildRevealCanvas()
		{
			GameObject gameObject = new GameObject("Wardrobe Perk Reveal", new Type[]
			{
				typeof(RectTransform),
				typeof(Canvas),
				typeof(CanvasScaler)
			});
			gameObject.transform.SetParent(base.transform, false);
			Canvas component = gameObject.GetComponent<Canvas>();
			component.renderMode = RenderMode.ScreenSpaceOverlay;
			component.sortingOrder = 310;
			GameplayHudVisibility.RegisterCanvas(component);
			CanvasScaler component2 = gameObject.GetComponent<CanvasScaler>();
			component2.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			component2.referenceResolution = new Vector2(1920f, 1080f);
			component2.matchWidthOrHeight = 0.5f;
			GameObject gameObject2 = new GameObject("Perk Title", new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			this._revealCard = gameObject2.GetComponent<RectTransform>();
			this._revealCard.anchorMin = new Vector2(0.5f, 1f);
			this._revealCard.anchorMax = new Vector2(0.5f, 1f);
			this._revealCard.pivot = new Vector2(0.5f, 1f);
			this._revealCard.anchoredPosition = new Vector2(0f, -106f);
			this._revealCard.sizeDelta = new Vector2(820f, 126f);
			this._revealGroup = gameObject2.GetComponent<CanvasGroup>();
			this._revealGroup.alpha = 0f;
			this._revealGroup.interactable = false;
			this._revealGroup.blocksRaycasts = false;
			DAGradient dagradient = WardrobeComboHUD.CreateImage(this._revealCard, "Contrast Scrim", Vector2.zero, new Vector2(820f, 126f), Color.white).AddComponent<DAGradient>();
			Gradient gradient = new Gradient();
			Color color = new Color(0.027f, 0.024f, 0.031f, 1f);
			gradient.SetKeys(new GradientColorKey[]
			{
				new GradientColorKey(color, 0f),
				new GradientColorKey(color, 0.5f),
				new GradientColorKey(color, 1f)
			}, new GradientAlphaKey[]
			{
				new GradientAlphaKey(0f, 0f),
				new GradientAlphaKey(0.5f, 0.2f),
				new GradientAlphaKey(0.72f, 0.5f),
				new GradientAlphaKey(0.5f, 0.8f),
				new GradientAlphaKey(0f, 1f)
			});
			dagradient.Gradient = gradient;
			dagradient.BlendMode = DAColorBlendMode.Overlay;
			dagradient.Intensity = 1f;
			dagradient.Angle = 90f;
			GameObject gameObject3 = WardrobeComboHUD.CreateImage(this._revealCard, "Perk Icon", Vector2.zero, new Vector2(34f, 34f), Color.white);
			this._revealIcon = gameObject3.GetComponent<Image>();
			WardrobeComboHUD.PlaceFromTop(this._revealIcon.rectTransform, new Vector2(0f, -2f), new Vector2(34f, 34f));
			this._revealIcon.preserveAspect = true;
			this._revealTitle = WardrobeComboHUD.CreateText(this._revealCard, "Title", 34f, FontStyles.Bold);
			WardrobeComboHUD.PlaceFromTop(this._revealTitle.rectTransform, new Vector2(0f, -38f), new Vector2(780f, 42f));
			this._revealTitle.alignment = TextAlignmentOptions.Center;
			this._revealTitle.characterSpacing = 4f;
			this._revealDescription = WardrobeComboHUD.CreateText(this._revealCard, "Description", 15f, FontStyles.Normal);
			WardrobeComboHUD.PlaceFromTop(this._revealDescription.rectTransform, new Vector2(0f, -80f), new Vector2(780f, 24f));
			this._revealDescription.alignment = TextAlignmentOptions.Center;
			this._revealDescription.color = Color.white;
			this._revealDescription.enableWordWrapping = false;
			GameObject gameObject4 = WardrobeComboHUD.CreateImage(this._revealCard, "Accent", Vector2.zero, new Vector2(72f, 2f), Color.white);
			this._revealAccent = gameObject4.GetComponent<Image>();
			WardrobeComboHUD.PlaceFromTop(this._revealAccent.rectTransform, new Vector2(0f, -112f), new Vector2(72f, 2f));
			gameObject2.SetActive(false);
		}

		// Token: 0x0600528C RID: 21132 RVA: 0x00200378 File Offset: 0x001FE578
		private static TextMeshProUGUI CreateText(Transform parent, string name, float size, FontStyles style)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject.transform.SetParent(parent, false);
			TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
			component.fontSize = size;
			component.fontStyle = style;
			component.raycastTarget = false;
			component.overflowMode = TextOverflowModes.Ellipsis;
			return component;
		}

		// Token: 0x0600528D RID: 21133 RVA: 0x002003DC File Offset: 0x001FE5DC
		private static GameObject CreateImage(Transform parent, string name, Vector2 position, Vector2 size, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(Image)
			});
			gameObject.transform.SetParent(parent, false);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			Vector2 vector = new Vector2(0.5f, 0.5f);
			component.anchorMax = vector;
			component.anchorMin = vector;
			component.anchoredPosition = position;
			component.sizeDelta = size;
			Image component2 = gameObject.GetComponent<Image>();
			component2.color = color;
			component2.raycastTarget = false;
			return gameObject;
		}

		// Token: 0x0600528E RID: 21134 RVA: 0x00200464 File Offset: 0x001FE664
		private static void PlaceFromTop(RectTransform rect, Vector2 position, Vector2 size)
		{
			Vector2 vector = new Vector2(0.5f, 1f);
			rect.anchorMax = vector;
			rect.anchorMin = vector;
			rect.pivot = new Vector2(0.5f, 1f);
			rect.anchoredPosition = position;
			rect.sizeDelta = size;
		}

		// Token: 0x0600528F RID: 21135 RVA: 0x002004B4 File Offset: 0x001FE6B4
		private static Sprite CreatePerkIcon(WardrobeComboHUD.PerkGlyph glyph)
		{
			Color32[] array = new Color32[9216];
			Color32 color = new Color32(20, 19, 24, 232);
			Color32 color2 = new Color32(245, 241, 234, byte.MaxValue);
			Color32 color3 = new Color32(233, 92, 122, byte.MaxValue);
			Vector2 vector = new Vector2(47.5f, 47.5f);
			for (int i = 0; i < 96; i++)
			{
				for (int j = 0; j < 96; j++)
				{
					if (Vector2.Distance(new Vector2((float)j, (float)i), vector) <= 43f)
					{
						array[i * 96 + j] = color;
					}
				}
			}
			switch (glyph)
			{
			case WardrobeComboHUD.PerkGlyph.Incognito:
				WardrobeComboHUD.DrawIncognito(array, 96, color2, color3);
				break;
			case WardrobeComboHUD.PerkGlyph.AthleticShoe:
				WardrobeComboHUD.FillPolygon(array, 96, new Vector2[]
				{
					new Vector2(27f, 38f),
					new Vector2(38f, 61f),
					new Vector2(48f, 52f),
					new Vector2(57f, 45f),
					new Vector2(72f, 43f),
					new Vector2(76f, 36f),
					new Vector2(69f, 31f),
					new Vector2(37f, 31f)
				}, color2);
				WardrobeComboHUD.DrawLine(array, 96, new Vector2(34f, 28f), new Vector2(70f, 28f), 2.2f, color3);
				WardrobeComboHUD.DrawLine(array, 96, new Vector2(22f, 50f), new Vector2(34f, 50f), 2f, color2);
				WardrobeComboHUD.DrawLine(array, 96, new Vector2(19f, 43f), new Vector2(32f, 43f), 2f, color2);
				WardrobeComboHUD.DrawLine(array, 96, new Vector2(23f, 36f), new Vector2(32f, 36f), 2f, color2);
				break;
			case WardrobeComboHUD.PerkGlyph.RopeLink:
				WardrobeComboHUD.DrawRing(array, 96, new Vector2(39f, 48f), 17f, 5f, color2);
				WardrobeComboHUD.DrawRing(array, 96, new Vector2(57f, 48f), 17f, 5f, color2);
				WardrobeComboHUD.FillCircle(array, 96, new Vector2(48f, 48f), 4f, color3);
				break;
			case WardrobeComboHUD.PerkGlyph.Clover:
				WardrobeComboHUD.FillCircle(array, 96, new Vector2(39f, 57f), 11f, color2);
				WardrobeComboHUD.FillCircle(array, 96, new Vector2(57f, 57f), 11f, color2);
				WardrobeComboHUD.FillCircle(array, 96, new Vector2(39f, 40f), 11f, color2);
				WardrobeComboHUD.FillCircle(array, 96, new Vector2(57f, 40f), 11f, color2);
				WardrobeComboHUD.DrawLine(array, 96, new Vector2(49f, 43f), new Vector2(57f, 24f), 3.4f, color3);
				break;
			case WardrobeComboHUD.PerkGlyph.Ghost:
				WardrobeComboHUD.DrawGhost(array, 96, color2, color3, color);
				break;
			case WardrobeComboHUD.PerkGlyph.Blindfold:
				WardrobeComboHUD.FillPolygon(array, 96, new Vector2[]
				{
					new Vector2(23f, 60f),
					new Vector2(73f, 60f),
					new Vector2(68f, 39f),
					new Vector2(54f, 36f),
					new Vector2(48f, 40f),
					new Vector2(42f, 36f),
					new Vector2(28f, 39f)
				}, color2);
				WardrobeComboHUD.DrawLine(array, 96, new Vector2(25f, 59f), new Vector2(71f, 40f), 2.8f, color3);
				WardrobeComboHUD.DrawLine(array, 96, new Vector2(22f, 51f), new Vector2(15f, 37f), 2.8f, color3);
				WardrobeComboHUD.DrawLine(array, 96, new Vector2(74f, 51f), new Vector2(81f, 37f), 2.8f, color3);
				break;
			}
			Texture2D texture2D = new Texture2D(96, 96, TextureFormat.RGBA32, false)
			{
				name = "WardrobePerkIcon_" + glyph.ToString(),
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp
			};
			texture2D.SetPixels32(array);
			texture2D.Apply(false, true);
			Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, 96f, 96f), new Vector2(0.5f, 0.5f), 100f);
			sprite.name = texture2D.name;
			return sprite;
		}

		// Token: 0x06005290 RID: 21136 RVA: 0x002009E4 File Offset: 0x001FEBE4
		private static void DrawIncognito(Color32[] pixels, int size, Color32 white, Color32 accent)
		{
			WardrobeComboHUD.FillPolygon(pixels, size, new Vector2[]
			{
				new Vector2(31f, 56f),
				new Vector2(36f, 73f),
				new Vector2(60f, 73f),
				new Vector2(65f, 56f)
			}, white);
			WardrobeComboHUD.DrawLine(pixels, size, new Vector2(24f, 55f), new Vector2(72f, 55f), 3.8f, white);
			WardrobeComboHUD.DrawLine(pixels, size, new Vector2(33f, 59f), new Vector2(63f, 59f), 2.3f, accent);
			WardrobeComboHUD.DrawRing(pixels, size, new Vector2(38f, 39f), 9.5f, 3.2f, white);
			WardrobeComboHUD.DrawRing(pixels, size, new Vector2(58f, 39f), 9.5f, 3.2f, white);
			WardrobeComboHUD.DrawLine(pixels, size, new Vector2(47f, 40f), new Vector2(49f, 40f), 2.6f, white);
			WardrobeComboHUD.DrawLine(pixels, size, new Vector2(29f, 41f), new Vector2(24f, 43f), 2.4f, white);
			WardrobeComboHUD.DrawLine(pixels, size, new Vector2(67f, 41f), new Vector2(72f, 43f), 2.4f, white);
		}

		// Token: 0x06005291 RID: 21137 RVA: 0x00200B70 File Offset: 0x001FED70
		private static void DrawGhost(Color32[] pixels, int size, Color32 white, Color32 accent, Color32 dark)
		{
			WardrobeComboHUD.FillCircle(pixels, size, new Vector2(48f, 57f), 20f, white);
			WardrobeComboHUD.FillPolygon(pixels, size, new Vector2[]
			{
				new Vector2(28f, 58f),
				new Vector2(28f, 27f),
				new Vector2(37f, 34f),
				new Vector2(45f, 27f),
				new Vector2(53f, 34f),
				new Vector2(61f, 27f),
				new Vector2(68f, 34f),
				new Vector2(68f, 58f)
			}, white);
			WardrobeComboHUD.FillCircle(pixels, size, new Vector2(40f, 59f), 4.5f, dark);
			WardrobeComboHUD.FillCircle(pixels, size, new Vector2(56f, 59f), 4.5f, dark);
			WardrobeComboHUD.FillCircle(pixels, size, new Vector2(41f, 60f), 1.5f, accent);
			WardrobeComboHUD.FillCircle(pixels, size, new Vector2(57f, 60f), 1.5f, accent);
			WardrobeComboHUD.DrawRing(pixels, size, new Vector2(48f, 46f), 4f, 2f, accent);
		}

		// Token: 0x06005292 RID: 21138 RVA: 0x00200CEA File Offset: 0x001FEEEA
		private static Sprite GetIconSprite(WardrobeComboHUD.ComboView view)
		{
			if (view.iconSprite == null)
			{
				view.iconSprite = WardrobeComboHUD.CreatePerkIcon(view.glyph);
			}
			return view.iconSprite;
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x00200D14 File Offset: 0x001FEF14
		private static void DrawRing(Color32[] pixels, int size, Vector2 center, float radius, float thickness, Color32 color)
		{
			float num = radius - thickness;
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					float num2 = Vector2.Distance(new Vector2((float)j, (float)i), center);
					if (num2 >= num && num2 <= radius)
					{
						WardrobeComboHUD.SetPixel(pixels, size, j, i, color);
					}
				}
			}
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x00200D64 File Offset: 0x001FEF64
		private static void FillCircle(Color32[] pixels, int size, Vector2 center, float radius, Color32 color)
		{
			float num = radius * radius;
			for (int i = Mathf.Max(0, Mathf.FloorToInt(center.y - radius)); i <= Mathf.Min(size - 1, Mathf.CeilToInt(center.y + radius)); i++)
			{
				for (int j = Mathf.Max(0, Mathf.FloorToInt(center.x - radius)); j <= Mathf.Min(size - 1, Mathf.CeilToInt(center.x + radius)); j++)
				{
					if ((new Vector2((float)j, (float)i) - center).sqrMagnitude <= num)
					{
						WardrobeComboHUD.SetPixel(pixels, size, j, i, color);
					}
				}
			}
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x00200E00 File Offset: 0x001FF000
		private static void DrawLine(Color32[] pixels, int size, Vector2 from, Vector2 to, float thickness, Color32 color)
		{
			Vector2 vector = to - from;
			float sqrMagnitude = vector.sqrMagnitude;
			if (sqrMagnitude <= 0.0001f)
			{
				return;
			}
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					Vector2 vector2 = new Vector2((float)j, (float)i);
					float num = Mathf.Clamp01(Vector2.Dot(vector2 - from, vector) / sqrMagnitude);
					if (Vector2.Distance(vector2, from + vector * num) <= thickness)
					{
						WardrobeComboHUD.SetPixel(pixels, size, j, i, color);
					}
				}
			}
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x00200E80 File Offset: 0x001FF080
		private static void FillPolygon(Color32[] pixels, int size, Vector2[] polygon, Color32 color)
		{
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					bool flag = false;
					int k = 0;
					int num = polygon.Length - 1;
					while (k < polygon.Length)
					{
						Vector2 vector = polygon[k];
						Vector2 vector2 = polygon[num];
						if (vector.y > (float)i != vector2.y > (float)i)
						{
							float num2 = (vector2.x - vector.x) * ((float)i - vector.y) / (vector2.y - vector.y) + vector.x;
							if ((float)j < num2)
							{
								flag = !flag;
							}
						}
						num = k++;
					}
					if (flag)
					{
						WardrobeComboHUD.SetPixel(pixels, size, j, i, color);
					}
				}
			}
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x00200F42 File Offset: 0x001FF142
		private static void SetPixel(Color32[] pixels, int size, int x, int y, Color32 color)
		{
			if (x < 0 || x >= size || y < 0 || y >= size)
			{
				return;
			}
			pixels[y * size + x] = color;
		}

		// Token: 0x06005298 RID: 21144 RVA: 0x00200F64 File Offset: 0x001FF164
		private WardrobeComboHUD.ComboView FindView(WardrobeCombo combo)
		{
			for (int i = 0; i < this._views.Count; i++)
			{
				if (this._views[i].combo == combo)
				{
					return this._views[i];
				}
			}
			return null;
		}

		// Token: 0x04005312 RID: 21266
		private static readonly Color Accent = new Color(0.914f, 0.361f, 0.478f, 1f);

		// Token: 0x04005313 RID: 21267
		private static readonly Color Ivory = new Color(0.961f, 0.945f, 0.918f, 1f);

		// Token: 0x04005314 RID: 21268
		private readonly List<WardrobeComboHUD.ComboView> _views = new List<WardrobeComboHUD.ComboView>();

		// Token: 0x04005315 RID: 21269
		private readonly Queue<WardrobeCombo> _revealQueue = new Queue<WardrobeCombo>();

		// Token: 0x04005316 RID: 21270
		private WardrobeCombo _active;

		// Token: 0x04005317 RID: 21271
		private RectTransform _badgeRoot;

		// Token: 0x04005318 RID: 21272
		private float _nextHudResolveTime;

		// Token: 0x04005319 RID: 21273
		private CanvasGroup _revealGroup;

		// Token: 0x0400531A RID: 21274
		private RectTransform _revealCard;

		// Token: 0x0400531B RID: 21275
		private Image _revealAccent;

		// Token: 0x0400531C RID: 21276
		private Image _revealIcon;

		// Token: 0x0400531D RID: 21277
		private TextMeshProUGUI _revealTitle;

		// Token: 0x0400531E RID: 21278
		private TextMeshProUGUI _revealDescription;

		// Token: 0x0400531F RID: 21279
		private Coroutine _revealRoutine;

		// Token: 0x02000B5C RID: 2908
		private enum PerkGlyph
		{
			// Token: 0x04005321 RID: 21281
			Incognito,
			// Token: 0x04005322 RID: 21282
			AthleticShoe,
			// Token: 0x04005323 RID: 21283
			RopeLink,
			// Token: 0x04005324 RID: 21284
			Clover,
			// Token: 0x04005325 RID: 21285
			Ghost,
			// Token: 0x04005326 RID: 21286
			Blindfold
		}

		// Token: 0x02000B5D RID: 2909
		private sealed class ComboView
		{
			// Token: 0x04005327 RID: 21287
			public WardrobeCombo combo;

			// Token: 0x04005328 RID: 21288
			public string title;

			// Token: 0x04005329 RID: 21289
			public string description;

			// Token: 0x0400532A RID: 21290
			public WardrobeComboHUD.PerkGlyph glyph;

			// Token: 0x0400532B RID: 21291
			public GameObject badge;

			// Token: 0x0400532C RID: 21292
			public Sprite iconSprite;
		}
	}
}
