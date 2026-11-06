using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Features.Localization;
using Features.MasturbationSystem;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.TrashcanLoot
{
	// Token: 0x02000BF1 RID: 3057
	public class TrashcanLootProgressUI : MonoBehaviour
	{
		// Token: 0x060057FE RID: 22526 RVA: 0x00226228 File Offset: 0x00224428
		public static TrashcanLootProgressUI Create(Transform parent, Vector3 localPosition)
		{
			GameObject gameObject = new GameObject("LootProgressUI");
			gameObject.transform.SetParent(parent, false);
			gameObject.transform.localPosition = localPosition;
			TrashcanLootProgressUI trashcanLootProgressUI = gameObject.AddComponent<TrashcanLootProgressUI>();
			trashcanLootProgressUI.Build(TrashcanLootProgressUI.RuntimeSprite(TrashcanLootProgressUI.RoundSide.Both), TrashcanLootProgressUI.RuntimeSprite(TrashcanLootProgressUI.RoundSide.Left), TrashcanLootProgressUI.RuntimeSprite(TrashcanLootProgressUI.RoundSide.Right), null);
			trashcanLootProgressUI.gameObject.SetActive(false);
			return trashcanLootProgressUI;
		}

		// Token: 0x060057FF RID: 22527 RVA: 0x00226284 File Offset: 0x00224484
		public void Build(Sprite roundedFull, Sprite roundedLeft, Sprite roundedRight, Material liquidMaterial)
		{
			if (this.title != null)
			{
				return;
			}
			this.roundedFullSprite = roundedFull;
			this.roundedLeftSprite = roundedLeft;
			this.roundedRightSprite = roundedRight;
			Canvas canvas = base.gameObject.GetComponent<Canvas>();
			if (canvas == null)
			{
				canvas = base.gameObject.AddComponent<Canvas>();
			}
			canvas.renderMode = RenderMode.WorldSpace;
			canvas.overrideSorting = true;
			canvas.sortingOrder = 110;
			GameplayHudVisibility.RegisterCanvas(canvas);
			RectTransform rectTransform = (RectTransform)base.transform;
			rectTransform.sizeDelta = new Vector2(380f, 96f);
			rectTransform.localScale = Vector3.one * 0.0021f;
			this.segmentMaterial = liquidMaterial;
			GameObject gameObject = new GameObject("Title", new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(rectTransform, false);
			this.title = gameObject.AddComponent<TextMeshProUGUI>();
			this.title.text = this.runningText;
			this.title.fontSize = 30f;
			this.title.fontStyle = FontStyles.Bold;
			this.title.characterSpacing = 10f;
			this.title.alignment = TextAlignmentOptions.Center;
			this.title.color = Color.white;
			this.title.raycastTarget = false;
			RectTransform rectTransform2 = (RectTransform)gameObject.transform;
			rectTransform2.anchorMin = new Vector2(0f, 0.5f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.offsetMin = Vector2.zero;
			rectTransform2.offsetMax = Vector2.zero;
			this.RefreshTitle();
			GameObject gameObject2 = new GameObject("BarArea", new Type[] { typeof(RectTransform) });
			gameObject2.transform.SetParent(rectTransform, false);
			this.barArea = (RectTransform)gameObject2.transform;
			this.barArea.anchorMin = new Vector2(0.5f, 0f);
			this.barArea.anchorMax = new Vector2(0.5f, 0f);
			this.barArea.pivot = new Vector2(0.5f, 0.5f);
			this.barArea.sizeDelta = new Vector2(330f, 20f);
			this.barArea.anchoredPosition = new Vector2(0f, 26f);
			this.segmentTemplate = TrashcanLootProgressUI.BuildSegment(this.barArea, roundedFull, liquidMaterial);
			this.segmentTemplate.name = "SegmentTemplate";
			this.segmentTemplate.SetActive(false);
		}

		// Token: 0x06005800 RID: 22528 RVA: 0x00226514 File Offset: 0x00224714
		public static GameObject BuildSegment(Transform parent, Sprite rounded, Material liquidMaterial)
		{
			GameObject gameObject = new GameObject("Segment", new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			((RectTransform)gameObject.transform).sizeDelta = new Vector2(100f, 20f);
			Image image = gameObject.AddComponent<Image>();
			image.sprite = rounded;
			image.type = Image.Type.Sliced;
			image.color = new Color(1f, 1f, 1f, 0.13f);
			image.raycastTarget = false;
			GameObject gameObject2 = new GameObject("Fill", new Type[] { typeof(RectTransform) });
			gameObject2.transform.SetParent(gameObject.transform, false);
			Image image2 = gameObject2.AddComponent<Image>();
			image2.raycastTarget = false;
			RectTransform rectTransform = image2.rectTransform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(1f, 1f);
			rectTransform.offsetMax = new Vector2(-1f, -1f);
			if (liquidMaterial != null)
			{
				image2.material = liquidMaterial;
				image2.color = Color.white;
				OrgasmBarRoundedSetup orgasmBarRoundedSetup = gameObject2.AddComponent<OrgasmBarRoundedSetup>();
				TrashcanLootProgressUI.SerializedObjectProxy serializedObjectProxy = new TrashcanLootProgressUI.SerializedObjectProxy(orgasmBarRoundedSetup);
				serializedObjectProxy.TrySetVector4("radii", new Vector4(8f, 8f, 8f, 8f));
			}
			else
			{
				image2.sprite = rounded;
				image2.type = Image.Type.Filled;
				image2.fillMethod = Image.FillMethod.Horizontal;
				image2.fillOrigin = 0;
				image2.fillAmount = 0f;
				image2.color = TrashcanLootProgressUI.Pink;
			}
			return gameObject;
		}

		// Token: 0x06005801 RID: 22529 RVA: 0x002266AC File Offset: 0x002248AC
		public void ConfigureSegmentShape(GameObject seg, int index, int count)
		{
			bool flag = count <= 1;
			bool flag2 = index == 0;
			bool flag3 = index == count - 1;
			float num = this.cornerRadius;
			Sprite sprite = (flag ? this.roundedFullSprite : (flag2 ? this.roundedLeftSprite : (flag3 ? this.roundedRightSprite : null)));
			Vector4 vector = (flag ? new Vector4(num, num, num, num) : (flag2 ? new Vector4(num, 0f, 0f, num) : (flag3 ? new Vector4(0f, num, num, 0f) : Vector4.zero)));
			Image component = seg.GetComponent<Image>();
			if (component != null)
			{
				component.sprite = sprite;
				component.type = ((sprite != null) ? Image.Type.Sliced : Image.Type.Simple);
			}
			OrgasmBarRoundedSetup componentInChildren = seg.GetComponentInChildren<OrgasmBarRoundedSetup>(true);
			if (componentInChildren != null)
			{
				new TrashcanLootProgressUI.SerializedObjectProxy(componentInChildren).TrySetVector4("radii", vector);
			}
			Image[] componentsInChildren = seg.GetComponentsInChildren<Image>(true);
			int i = 0;
			while (i < componentsInChildren.Length)
			{
				Image image = componentsInChildren[i];
				if (!(image.gameObject == seg))
				{
					if (image.material == null || !image.material.HasProperty(TrashcanLootProgressUI.PropFillAmount))
					{
						image.sprite = ((sprite != null) ? sprite : TrashcanLootProgressUI.RuntimeSprite(TrashcanLootProgressUI.RoundSide.None));
						image.type = Image.Type.Filled;
						image.fillMethod = Image.FillMethod.Horizontal;
						image.fillOrigin = 0;
						return;
					}
					break;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x00226814 File Offset: 0x00224A14
		public void Show(int segmentCount)
		{
			GameplayHudVisibility.RegisterCanvas(base.GetComponent<Canvas>());
			base.StopAllCoroutines();
			this._dotsCo = null;
			this.EnsureGroup().alpha = 1f;
			this._progress = 0f;
			this.ClearSegments();
			int num = Mathf.Max(1, segmentCount);
			float num2 = (330f - 6f * (float)(num - 1)) / (float)num;
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = ((this.segmentTemplate != null) ? Object.Instantiate<GameObject>(this.segmentTemplate, this.barArea) : TrashcanLootProgressUI.BuildSegment(this.barArea, (this.roundedFullSprite != null) ? this.roundedFullSprite : TrashcanLootProgressUI.RuntimeSprite(TrashcanLootProgressUI.RoundSide.Both), this.segmentMaterial));
				gameObject.name = string.Format("Segment{0}", i);
				RectTransform rectTransform = (RectTransform)gameObject.transform;
				Vector2 vector = new Vector2(0.5f, 0.5f);
				rectTransform.anchorMax = vector;
				rectTransform.anchorMin = vector;
				rectTransform.pivot = new Vector2(0.5f, 0.5f);
				rectTransform.sizeDelta = new Vector2(num2, 20f);
				rectTransform.anchoredPosition = new Vector2(-165f + num2 * 0.5f + (float)i * (num2 + 6f), 0f);
				rectTransform.localScale = Vector3.one;
				Image image = null;
				foreach (Image image2 in gameObject.GetComponentsInChildren<Image>(true))
				{
					if (image2.gameObject != gameObject)
					{
						image = image2;
						break;
					}
				}
				Material material = null;
				Color color = Color.Lerp(this.shadeLow, this.shadeHigh, (num > 1) ? ((float)i / (float)num) : 0f);
				Color color2 = Color.Lerp(this.shadeLow, this.shadeHigh, (num > 1) ? ((float)(i + 1) / (float)num) : 1f);
				float num3 = 0f;
				float num4 = 0f;
				if (image != null && image.material != null && image.material.HasProperty(TrashcanLootProgressUI.PropFillAmount))
				{
					material = new Material(image.material)
					{
						name = image.material.name + " (Instance)"
					};
					material.SetFloat(TrashcanLootProgressUI.PropFillAmount, 0f);
					if (material.HasProperty(TrashcanLootProgressUI.PropColorLow))
					{
						material.SetColor(TrashcanLootProgressUI.PropColorLow, color);
					}
					if (material.HasProperty(TrashcanLootProgressUI.PropColorHigh))
					{
						material.SetColor(TrashcanLootProgressUI.PropColorHigh, color2);
					}
					if (material.HasProperty(TrashcanLootProgressUI.PropEdgeGlowIntensity))
					{
						num3 = material.GetFloat(TrashcanLootProgressUI.PropEdgeGlowIntensity);
						material.SetFloat(TrashcanLootProgressUI.PropEdgeGlowIntensity, 0f);
					}
					if (material.HasProperty(TrashcanLootProgressUI.PropScanlineIntensity))
					{
						num4 = material.GetFloat(TrashcanLootProgressUI.PropScanlineIntensity);
						material.SetFloat(TrashcanLootProgressUI.PropScanlineIntensity, 0f);
					}
					image.material = material;
				}
				else if (image != null)
				{
					image.fillAmount = 0f;
					image.color = Color.Lerp(color, color2, 0.5f);
				}
				this.ConfigureSegmentShape(gameObject, i, num);
				gameObject.SetActive(true);
				this._segments.Add(gameObject);
				this._segmentFills.Add(image);
				this._segmentMats.Add(material);
				this._segmentBaseGlow.Add(num3);
				this._segmentBaseScanline.Add(num4);
			}
			base.gameObject.SetActive(true);
			this._dotsCo = base.StartCoroutine(this.DotsRoutine());
		}

		// Token: 0x06005803 RID: 22531 RVA: 0x00226BAC File Offset: 0x00224DAC
		private void ClearSegments()
		{
			foreach (GameObject gameObject in this._segments)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
					Object.Destroy(gameObject);
				}
			}
			this._segments.Clear();
			this._segmentFills.Clear();
			foreach (Material material in this._segmentMats)
			{
				if (material != null)
				{
					Object.Destroy(material);
				}
			}
			this._segmentMats.Clear();
			this._segmentBaseGlow.Clear();
			this._segmentBaseScanline.Clear();
			if (this.barArea != null)
			{
				for (int i = this.barArea.childCount - 1; i >= 0; i--)
				{
					Transform child = this.barArea.GetChild(i);
					if (child.name.StartsWith("Preview"))
					{
						child.gameObject.SetActive(false);
						Object.Destroy(child.gameObject);
					}
				}
			}
		}

		// Token: 0x06005804 RID: 22532 RVA: 0x00226CF4 File Offset: 0x00224EF4
		public void SetProgress(float t01)
		{
			this._progress = Mathf.Clamp01(t01);
			int count = this._segments.Count;
			for (int i = 0; i < count; i++)
			{
				float num = Mathf.Clamp01(this._progress * (float)count - (float)i);
				Material material = this._segmentMats[i];
				if (material != null)
				{
					material.SetFloat(TrashcanLootProgressUI.PropFillAmount, num);
					bool flag = num > 0.001f;
					if (material.HasProperty(TrashcanLootProgressUI.PropEdgeGlowIntensity))
					{
						material.SetFloat(TrashcanLootProgressUI.PropEdgeGlowIntensity, flag ? this._segmentBaseGlow[i] : 0f);
					}
					if (material.HasProperty(TrashcanLootProgressUI.PropScanlineIntensity))
					{
						material.SetFloat(TrashcanLootProgressUI.PropScanlineIntensity, flag ? this._segmentBaseScanline[i] : 0f);
					}
				}
				else if (this._segmentFills[i] != null)
				{
					this._segmentFills[i].fillAmount = num;
				}
			}
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x00226DEF File Offset: 0x00224FEF
		public void SetRunningText(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				this.runningText = value;
			}
			this._titleSuffix = string.Empty;
			this.RefreshTitle();
		}

		// Token: 0x06005806 RID: 22534 RVA: 0x00226E14 File Offset: 0x00225014
		private void RefreshTitle()
		{
			if (this.title == null)
			{
				return;
			}
			if (!(this._titleBinding == null))
			{
				this._titleBinding.Refresh();
				return;
			}
			this._titleBinding = LocalizedGameText.BindComputed(this.title, () => GameLocalization.GetSource("world_ui", this.runningText) + this._titleSuffix);
			LocalizedGameText titleBinding = this._titleBinding;
			if (titleBinding == null)
			{
				return;
			}
			titleBinding.FitTranslationToRectangle(false);
		}

		// Token: 0x06005807 RID: 22535 RVA: 0x00226E78 File Offset: 0x00225078
		public void MarkReward(int index)
		{
			if (index < 0 || index >= this._segments.Count || this._segments[index] == null)
			{
				return;
			}
			if (this._segmentMats[index] != null)
			{
				this._segmentMats[index].SetFloat(TrashcanLootProgressUI.PropFillAmount, 1f);
			}
			else if (this._segmentFills[index] != null)
			{
				this._segmentFills[index].fillAmount = 1f;
			}
			base.StartCoroutine(this.PopSegment((RectTransform)this._segments[index].transform));
		}

		// Token: 0x06005808 RID: 22536 RVA: 0x00226F2A File Offset: 0x0022512A
		private IEnumerator PopSegment(RectTransform rt)
		{
			float t = 0f;
			while (t < 0.3f && rt != null)
			{
				t += Time.deltaTime;
				float num = Mathf.Clamp01(t / 0.3f);
				float num2 = 1f + 0.22f * Mathf.Sin(num * 3.1415927f);
				rt.localScale = new Vector3(num2, num2, 1f);
				yield return null;
			}
			if (rt != null)
			{
				rt.localScale = Vector3.one;
			}
			yield break;
		}

		// Token: 0x06005809 RID: 22537 RVA: 0x00226F39 File Offset: 0x00225139
		private IEnumerator DotsRoutine()
		{
			int i = 0;
			WaitForSeconds wait = new WaitForSeconds(Mathf.Max(0.05f, this.dotInterval));
			for (;;)
			{
				if (this.title != null)
				{
					string text = ((i < 3) ? ("<color=#00000000>" + new string('.', 3 - i) + "</color>") : "");
					this._titleSuffix = new string('.', i) + text;
					this.RefreshTitle();
				}
				i = (i + 1) % 4;
				yield return wait;
			}
			yield break;
		}

		// Token: 0x0600580A RID: 22538 RVA: 0x00226F48 File Offset: 0x00225148
		public void Complete()
		{
			this.SetProgress(1f);
			base.StartCoroutine(this.FadeOutRoutine(this.completeLinger, this.fadeOutDuration));
		}

		// Token: 0x0600580B RID: 22539 RVA: 0x00226F6E File Offset: 0x0022516E
		public void FadeOut(float duration = 0.2f)
		{
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			base.StartCoroutine(this.FadeOutRoutine(0f, duration));
		}

		// Token: 0x0600580C RID: 22540 RVA: 0x00226F91 File Offset: 0x00225191
		private IEnumerator FadeOutRoutine(float delay, float duration)
		{
			CanvasGroup group = this.EnsureGroup();
			if (delay > 0f)
			{
				yield return new WaitForSeconds(delay);
			}
			float t = 0f;
			while (t < duration)
			{
				t += Time.deltaTime;
				group.alpha = 1f - Mathf.Clamp01(t / Mathf.Max(0.01f, duration));
				yield return null;
			}
			group.alpha = 1f;
			base.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x00226FB0 File Offset: 0x002251B0
		private CanvasGroup EnsureGroup()
		{
			if (this._group == null)
			{
				this._group = base.GetComponent<CanvasGroup>();
				if (this._group == null)
				{
					this._group = base.gameObject.AddComponent<CanvasGroup>();
				}
			}
			return this._group;
		}

		// Token: 0x0600580E RID: 22542 RVA: 0x00226FFC File Offset: 0x002251FC
		public void Hide()
		{
			base.StopAllCoroutines();
			this._dotsCo = null;
			this.EnsureGroup().alpha = 1f;
			base.gameObject.SetActive(false);
		}

		// Token: 0x0600580F RID: 22543 RVA: 0x00227028 File Offset: 0x00225228
		private void LateUpdate()
		{
			if (this._cam == null || !this._cam.isActiveAndEnabled)
			{
				this._cam = Camera.main;
				if (this._cam == null && Camera.allCamerasCount > 0)
				{
					this._cam = Camera.allCameras[0];
				}
			}
			if (this._cam == null)
			{
				return;
			}
			base.transform.LookAt(this._cam.transform);
			base.transform.Rotate(0f, 180f, 0f);
		}

		// Token: 0x06005810 RID: 22544 RVA: 0x002270C0 File Offset: 0x002252C0
		public static Texture2D GenerateRoundedRectTexture(TrashcanLootProgressUI.RoundSide side, out int cornerRadius)
		{
			cornerRadius = 14;
			Texture2D texture2D = new Texture2D(48, 48, TextureFormat.RGBA32, false);
			for (int i = 0; i < 48; i++)
			{
				for (int j = 0; j < 48; j++)
				{
					float num = ((side == TrashcanLootProgressUI.RoundSide.Right || side == TrashcanLootProgressUI.RoundSide.None) ? 0f : 14f);
					float num2 = (float)((side == TrashcanLootProgressUI.RoundSide.Left || side == TrashcanLootProgressUI.RoundSide.None) ? 48 : 34);
					float num3 = Mathf.Clamp((float)j + 0.5f, num, num2);
					float num4 = Mathf.Clamp((float)i + 0.5f, 14f, 34f);
					float num5 = Vector2.Distance(new Vector2((float)j + 0.5f, (float)i + 0.5f), new Vector2(num3, num4));
					texture2D.SetPixel(j, i, new Color(1f, 1f, 1f, Mathf.Clamp01(14f - num5 + 0.5f)));
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06005811 RID: 22545 RVA: 0x002271B0 File Offset: 0x002253B0
		public static Vector4 SpriteBorder(TrashcanLootProgressUI.RoundSide side, int radius)
		{
			float num = (float)(radius + 2);
			if (side == TrashcanLootProgressUI.RoundSide.None)
			{
				return Vector4.zero;
			}
			if (side == TrashcanLootProgressUI.RoundSide.Left)
			{
				return new Vector4(num, num, 0f, num);
			}
			if (side != TrashcanLootProgressUI.RoundSide.Right)
			{
				return new Vector4(num, num, num, num);
			}
			return new Vector4(0f, num, num, num);
		}

		// Token: 0x06005812 RID: 22546 RVA: 0x002271FC File Offset: 0x002253FC
		private static Sprite RuntimeSprite(TrashcanLootProgressUI.RoundSide side)
		{
			if (TrashcanLootProgressUI._runtimeSprites[(int)side] != null)
			{
				return TrashcanLootProgressUI._runtimeSprites[(int)side];
			}
			int num;
			Texture2D texture2D = TrashcanLootProgressUI.GenerateRoundedRectTexture(side, out num);
			texture2D.hideFlags = HideFlags.HideAndDontSave;
			Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect, TrashcanLootProgressUI.SpriteBorder(side, num));
			sprite.hideFlags = HideFlags.HideAndDontSave;
			TrashcanLootProgressUI._runtimeSprites[(int)side] = sprite;
			return sprite;
		}

		// Token: 0x04005877 RID: 22647
		public static readonly Color Pink = new Color(0.929f, 0.357f, 0.482f, 1f);

		// Token: 0x04005878 RID: 22648
		public const float CanvasWidth = 380f;

		// Token: 0x04005879 RID: 22649
		public const float CanvasHeight = 96f;

		// Token: 0x0400587A RID: 22650
		public const float BarWidth = 330f;

		// Token: 0x0400587B RID: 22651
		public const float BarHeight = 20f;

		// Token: 0x0400587C RID: 22652
		public const float SegmentGap = 6f;

		// Token: 0x0400587D RID: 22653
		[Header("Runtime-driven references (keep wired when restyling)")]
		[SerializeField]
		private TextMeshProUGUI title;

		// Token: 0x0400587E RID: 22654
		[SerializeField]
		private RectTransform barArea;

		// Token: 0x0400587F RID: 22655
		[Tooltip("Disabled segment prototype (Track Image + child 'Fill' Image), cloned per reward.")]
		[SerializeField]
		private GameObject segmentTemplate;

		// Token: 0x04005880 RID: 22656
		[Tooltip("LiquidWave material (orgasm-bar shader) for the segment fills. A per-segment instance is created at runtime; empty = plain filled Images fallback.")]
		[SerializeField]
		private Material segmentMaterial;

		// Token: 0x04005881 RID: 22657
		[Header("Corner shape (rounded OUTER ends only, square internal steps)")]
		[SerializeField]
		private float cornerRadius = 8f;

		// Token: 0x04005882 RID: 22658
		[SerializeField]
		private Sprite roundedFullSprite;

		// Token: 0x04005883 RID: 22659
		[SerializeField]
		private Sprite roundedLeftSprite;

		// Token: 0x04005884 RID: 22660
		[SerializeField]
		private Sprite roundedRightSprite;

		// Token: 0x04005885 RID: 22661
		[Header("Label")]
		[SerializeField]
		private string runningText = "SEARCHING";

		// Token: 0x04005886 RID: 22662
		[Tooltip("Seconds between ellipsis steps of the loading animation.")]
		[SerializeField]
		private float dotInterval = 0.35f;

		// Token: 0x04005887 RID: 22663
		[Header("Segment shades (light to deep across the bar)")]
		[SerializeField]
		private Color shadeLow = new Color(1f, 0.62f, 0.72f, 1f);

		// Token: 0x04005888 RID: 22664
		[SerializeField]
		private Color shadeHigh = new Color(0.78f, 0.13f, 0.35f, 1f);

		// Token: 0x04005889 RID: 22665
		[Header("Fade out")]
		[Tooltip("Seconds the full bar stays visible after completion before fading.")]
		[SerializeField]
		private float completeLinger = 0.35f;

		// Token: 0x0400588A RID: 22666
		[SerializeField]
		private float fadeOutDuration = 0.4f;

		// Token: 0x0400588B RID: 22667
		private static readonly int PropFillAmount = Shader.PropertyToID("_FillAmount");

		// Token: 0x0400588C RID: 22668
		private static readonly int PropColorLow = Shader.PropertyToID("_ColorLow");

		// Token: 0x0400588D RID: 22669
		private static readonly int PropColorHigh = Shader.PropertyToID("_ColorHigh");

		// Token: 0x0400588E RID: 22670
		private static readonly int PropEdgeGlowIntensity = Shader.PropertyToID("_EdgeGlowIntensity");

		// Token: 0x0400588F RID: 22671
		private static readonly int PropScanlineIntensity = Shader.PropertyToID("_ScanlineIntensity");

		// Token: 0x04005890 RID: 22672
		private readonly List<GameObject> _segments = new List<GameObject>();

		// Token: 0x04005891 RID: 22673
		private readonly List<Image> _segmentFills = new List<Image>();

		// Token: 0x04005892 RID: 22674
		private readonly List<Material> _segmentMats = new List<Material>();

		// Token: 0x04005893 RID: 22675
		private readonly List<float> _segmentBaseGlow = new List<float>();

		// Token: 0x04005894 RID: 22676
		private readonly List<float> _segmentBaseScanline = new List<float>();

		// Token: 0x04005895 RID: 22677
		private CanvasGroup _group;

		// Token: 0x04005896 RID: 22678
		private Camera _cam;

		// Token: 0x04005897 RID: 22679
		private Coroutine _dotsCo;

		// Token: 0x04005898 RID: 22680
		private float _progress;

		// Token: 0x04005899 RID: 22681
		private LocalizedGameText _titleBinding;

		// Token: 0x0400589A RID: 22682
		private string _titleSuffix = string.Empty;

		// Token: 0x0400589B RID: 22683
		private static readonly Sprite[] _runtimeSprites = new Sprite[4];

		// Token: 0x02000BF2 RID: 3058
		private struct SerializedObjectProxy
		{
			// Token: 0x06005816 RID: 22550 RVA: 0x002273F2 File Offset: 0x002255F2
			public SerializedObjectProxy(Object target)
			{
				this._target = target;
			}

			// Token: 0x06005817 RID: 22551 RVA: 0x002273FC File Offset: 0x002255FC
			public void TrySetVector4(string field, Vector4 value)
			{
				FieldInfo field2 = this._target.GetType().GetField(field, BindingFlags.Instance | BindingFlags.NonPublic);
				if (field2 != null && field2.FieldType == typeof(Vector4))
				{
					field2.SetValue(this._target, value);
				}
			}

			// Token: 0x0400589C RID: 22684
			private readonly Object _target;
		}

		// Token: 0x02000BF3 RID: 3059
		public enum RoundSide
		{
			// Token: 0x0400589E RID: 22686
			Both,
			// Token: 0x0400589F RID: 22687
			Left,
			// Token: 0x040058A0 RID: 22688
			Right,
			// Token: 0x040058A1 RID: 22689
			None
		}
	}
}
