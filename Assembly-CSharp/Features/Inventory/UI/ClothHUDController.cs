using System;
using System.Collections.Generic;
using Features.Localization;
using ProjectExhibitionist.Core;
using ProjectExhibitionist.Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Inventory.UI
{
	// Token: 0x02001151 RID: 4433
	public class ClothHUDController : MonoBehaviour
	{
		// Token: 0x060086C7 RID: 34503 RVA: 0x003713F0 File Offset: 0x0036F5F0
		private void Start()
		{
			this.TryBind();
		}

		// Token: 0x060086C8 RID: 34504 RVA: 0x003713F8 File Offset: 0x0036F5F8
		private void OnEnable()
		{
			if (!this._bound)
			{
				this.TryBind();
			}
		}

		// Token: 0x060086C9 RID: 34505 RVA: 0x00371408 File Offset: 0x0036F608
		private void OnDisable()
		{
			this.Unbind();
			if (this._cycleHintRow != null)
			{
				this._cycleHintRow.gameObject.SetActive(false);
			}
		}

		// Token: 0x060086CA RID: 34506 RVA: 0x00371430 File Offset: 0x0036F630
		private void OnDestroy()
		{
			if (this._cycleHintRow != null && this._cycleHintRow.parent != base.transform)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(this._cycleHintRow.gameObject);
					return;
				}
				Object.DestroyImmediate(this._cycleHintRow.gameObject);
			}
		}

		// Token: 0x060086CB RID: 34507 RVA: 0x0037148C File Offset: 0x0036F68C
		private void Update()
		{
			if (this._bound && this.inventoryManager == null)
			{
				this._bound = false;
			}
			if (!this._bound)
			{
				this.TryBind();
			}
			this.RefreshCycleHint();
			if (!this._badgeResolved)
			{
				foreach (TMP_Text tmp_Text in base.GetComponentsInChildren<TMP_Text>(true))
				{
					if (tmp_Text.name == "Input" && tmp_Text.transform.parent.name == "Keyboard Input")
					{
						this._contextBadge = new InventoryContextBadge(tmp_Text.transform.parent, "D-Pad Left", default(Vector2));
					}
				}
				this._badgeResolved = true;
			}
			InventoryContextBadge contextBadge = this._contextBadge;
			if (contextBadge == null)
			{
				return;
			}
			contextBadge.Refresh(ControllerInput.IsActive);
		}

		// Token: 0x060086CC RID: 34508 RVA: 0x0037155C File Offset: 0x0036F75C
		private void TryBind()
		{
			if (this._bound)
			{
				return;
			}
			if (this.inventoryManager == null)
			{
				this.inventoryManager = PlayerInventoryManager.Instance;
			}
			if (this.inventoryManager == null)
			{
				return;
			}
			if (this.forceFirstSibling)
			{
				base.transform.SetAsFirstSibling();
			}
			this.BuildSlots(this.inventoryManager.MaxClothSlots);
			this.StabilizeAuthoredStripSize();
			this._uiToClothIndex = new int[this._slots.Count];
			for (int i = 0; i < this._uiToClothIndex.Length; i++)
			{
				this._uiToClothIndex[i] = -1;
			}
			this.inventoryManager.OnClothAdded += this.HandleClothAdded;
			this.inventoryManager.OnClothRemoved += this.HandleClothRemoved;
			this.inventoryManager.OnActiveClothChanged += this.HandleActiveClothChanged;
			this.inventoryManager.OnClothDurabilityChanged += this.HandleClothDurabilityChanged;
			this._bound = true;
			this.RefreshAll();
			this.RefreshCycleHint();
		}

		// Token: 0x060086CD RID: 34509 RVA: 0x00371668 File Offset: 0x0036F868
		private void Unbind()
		{
			if (this._bound && this.inventoryManager != null)
			{
				this.inventoryManager.OnClothAdded -= this.HandleClothAdded;
				this.inventoryManager.OnClothRemoved -= this.HandleClothRemoved;
				this.inventoryManager.OnActiveClothChanged -= this.HandleActiveClothChanged;
				this.inventoryManager.OnClothDurabilityChanged -= this.HandleClothDurabilityChanged;
			}
			this._bound = false;
		}

		// Token: 0x060086CE RID: 34510 RVA: 0x003716F0 File Offset: 0x0036F8F0
		private void StabilizeAuthoredStripSize()
		{
			RectTransform rectTransform = base.transform as RectTransform;
			HorizontalLayoutGroup component = base.GetComponent<HorizontalLayoutGroup>();
			if (rectTransform == null || component == null || !component.enabled || this._slots.Count == 0)
			{
				return;
			}
			component.CalculateLayoutInputHorizontal();
			component.CalculateLayoutInputVertical();
			float preferredWidth = component.preferredWidth;
			float num = component.preferredHeight;
			if (preferredWidth <= 0f || num <= 0f)
			{
				return;
			}
			ContentSizeFitter component2 = base.GetComponent<ContentSizeFitter>();
			if (component2 != null)
			{
				component2.enabled = false;
			}
			LayoutElement layoutElement = base.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = base.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.minWidth = (layoutElement.preferredWidth = preferredWidth);
			layoutElement.minHeight = (layoutElement.preferredHeight = num);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, preferredWidth);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num);
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		// Token: 0x060086CF RID: 34511 RVA: 0x003717DC File Offset: 0x0036F9DC
		private void RefreshCycleHint()
		{
			bool flag = !ControllerInput.IsActive && this._bound && this.inventoryManager != null && this.inventoryManager.CanCycleClothFromInput;
			if (flag && this._cycleHintRow == null)
			{
				this.BuildCycleHint();
			}
			if (this._cycleHintRow == null)
			{
				return;
			}
			if (flag)
			{
				string text = InteractionCandidateRegistry.ActiveBinding(this.inventoryManager.ClothCycleInput);
				flag = !string.IsNullOrEmpty(text);
				if (this._cycleHintBinding != text)
				{
					this._cycleHintBinding = text;
					LocalizedGameText.BindFormat(this._cycleHintText, "inventory_ui", "<color=#F278A2>{0}</color>  Switch clothing", new object[] { text.ToUpperInvariant() }).FitTranslationToRectangle(false);
				}
			}
			if (this._cycleHintRow.gameObject.activeSelf != flag)
			{
				this._cycleHintRow.gameObject.SetActive(flag);
			}
		}

		// Token: 0x060086D0 RID: 34512 RVA: 0x003718BC File Offset: 0x0036FABC
		private void BuildCycleHint()
		{
			GameObject gameObject = new GameObject("ClothingCycleHint", new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			});
			this._cycleHintRow = (RectTransform)gameObject.transform;
			bool flag = base.transform.parent != null && base.transform.parent.GetComponent<VerticalLayoutGroup>() != null;
			this._cycleHintRow.SetParent(flag ? base.transform.parent : base.transform, false);
			this._cycleHintRow.sizeDelta = new Vector2(250f, 26f);
			LayoutElement component = gameObject.GetComponent<LayoutElement>();
			component.minWidth = (component.preferredWidth = 250f);
			component.minHeight = (component.preferredHeight = 26f);
			if (flag)
			{
				this._cycleHintRow.SetSiblingIndex(base.transform.GetSiblingIndex() + 1);
			}
			else
			{
				component.ignoreLayout = true;
				RectTransform cycleHintRow = this._cycleHintRow;
				RectTransform cycleHintRow2 = this._cycleHintRow;
				Vector2 vector = new Vector2(0f, 0f);
				cycleHintRow2.anchorMax = vector;
				cycleHintRow.anchorMin = vector;
				this._cycleHintRow.pivot = new Vector2(0f, 1f);
				this._cycleHintRow.anchoredPosition = new Vector2(0f, -6f);
			}
			GameObject gameObject2 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject2.transform.SetParent(this._cycleHintRow, false);
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			ClothHUDController.StretchFull(rectTransform);
			rectTransform.offsetMin = new Vector2(8f, 0f);
			rectTransform.offsetMax = new Vector2(-8f, 0f);
			this._cycleHintText = gameObject2.GetComponent<TextMeshProUGUI>();
			TMP_Text tmp_Text = ((base.transform.parent != null) ? base.transform.parent.GetComponentInChildren<TMP_Text>(true) : null);
			if (tmp_Text != null && tmp_Text != this._cycleHintText)
			{
				this._cycleHintText.font = LocalizedGameText.SourceFont(tmp_Text);
			}
			this._cycleHintText.fontSize = 18f;
			this._cycleHintText.enableAutoSizing = true;
			this._cycleHintText.fontSizeMin = 14f;
			this._cycleHintText.fontSizeMax = 18f;
			this._cycleHintText.enableWordWrapping = false;
			this._cycleHintText.alignment = TextAlignmentOptions.MidlineLeft;
			this._cycleHintText.color = Color.white;
			this._cycleHintText.raycastTarget = false;
		}

		// Token: 0x060086D1 RID: 34513 RVA: 0x00371B70 File Offset: 0x0036FD70
		private void BuildSlots(int count)
		{
			if (this._slots.Count > 0)
			{
				return;
			}
			if (this.serializedSlots != null && this.serializedSlots.Count > 0)
			{
				for (int i = 0; i < this.serializedSlots.Count; i++)
				{
					ClothSlotUI clothSlotUI = this.serializedSlots[i];
					if (!(clothSlotUI == null))
					{
						clothSlotUI.Init(i, this);
						this._slots.Add(clothSlotUI);
					}
				}
				if (this._slots.Count > 0)
				{
					return;
				}
			}
			this.EnsureRadialSprite();
			if (this.slotContainer == null)
			{
				this.slotContainer = this.CreateContainer();
			}
			for (int j = 0; j < count; j++)
			{
				this._slots.Add(this.CreateSlot(j, this.slotContainer));
			}
		}

		// Token: 0x060086D2 RID: 34514 RVA: 0x00371C38 File Offset: 0x0036FE38
		private RectTransform CreateContainer()
		{
			GameObject gameObject = new GameObject("ClothSlots", new Type[] { typeof(RectTransform) });
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(base.transform, false);
			HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.spacing = this.slotSpacing;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			horizontalLayoutGroup.childControlWidth = false;
			horizontalLayoutGroup.childControlHeight = false;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			ContentSizeFitter contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
			contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			if (this.selfAnchorTopLeft)
			{
				rectTransform.anchorMin = new Vector2(0f, 1f);
				rectTransform.anchorMax = new Vector2(0f, 1f);
				rectTransform.pivot = new Vector2(0f, 1f);
				rectTransform.anchoredPosition = new Vector2(24f, -24f);
			}
			else
			{
				LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
				layoutElement.preferredHeight = this.preferredHeight;
				layoutElement.minHeight = this.preferredHeight;
			}
			return rectTransform;
		}

		// Token: 0x060086D3 RID: 34515 RVA: 0x00371D40 File Offset: 0x0036FF40
		private ClothSlotUI CreateSlot(int index, RectTransform parent)
		{
			GameObject gameObject = new GameObject(string.Format("ClothSlot_{0}", index), new Type[]
			{
				typeof(RectTransform),
				typeof(CanvasGroup)
			});
			RectTransform rectTransform = (RectTransform)gameObject.transform;
			rectTransform.SetParent(parent, false);
			rectTransform.sizeDelta = new Vector2(this.slotSize, this.slotSize);
			CanvasGroup component = gameObject.GetComponent<CanvasGroup>();
			Image image = this.CreateImage("Bg", rectTransform, this.radialSprite, this.radialBgColor);
			ClothHUDController.ConfigureRadial(image);
			ClothHUDController.StretchFull(image.rectTransform);
			Image image2 = this.CreateImage("Radial", rectTransform, this.radialSprite, this.radialColor);
			ClothHUDController.ConfigureRadial(image2);
			ClothHUDController.StretchFull(image2.rectTransform);
			Image image3 = this.CreateImage("Icon", rectTransform, null, Color.white);
			RectTransform rectTransform2 = image3.rectTransform;
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = new Vector2(8f, 8f);
			rectTransform2.offsetMax = new Vector2(-8f, -8f);
			image3.preserveAspect = true;
			Image image4 = this.CreateImage("SelectionFrame", rectTransform, this.radialSprite, this.selectionColor);
			image4.type = Image.Type.Sliced;
			ClothHUDController.StretchFull(image4.rectTransform);
			image4.enabled = false;
			GameObject gameObject2 = new GameObject("Number", new Type[] { typeof(RectTransform) });
			RectTransform rectTransform3 = (RectTransform)gameObject2.transform;
			rectTransform3.SetParent(rectTransform, false);
			rectTransform3.anchorMin = new Vector2(1f, 0f);
			rectTransform3.anchorMax = new Vector2(1f, 0f);
			rectTransform3.pivot = new Vector2(1f, 0f);
			rectTransform3.anchoredPosition = new Vector2(2f, -2f);
			rectTransform3.sizeDelta = new Vector2(18f, 18f);
			TextMeshProUGUI textMeshProUGUI = gameObject2.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = (index + 1).ToString();
			textMeshProUGUI.fontSize = 14f;
			textMeshProUGUI.alignment = TextAlignmentOptions.Center;
			textMeshProUGUI.color = Color.white;
			ClothSlotUI clothSlotUI = gameObject.AddComponent<ClothSlotUI>();
			clothSlotUI.Bind(image3, image2, image4, textMeshProUGUI, component);
			clothSlotUI.Init(index, this);
			return clothSlotUI;
		}

		// Token: 0x060086D4 RID: 34516 RVA: 0x00371F94 File Offset: 0x00370194
		private Image CreateImage(string name, RectTransform parent, Sprite sprite, Color color)
		{
			GameObject gameObject = new GameObject(name, new Type[] { typeof(RectTransform) });
			gameObject.transform.SetParent(parent, false);
			Image image = gameObject.AddComponent<Image>();
			image.sprite = sprite;
			image.color = color;
			image.raycastTarget = false;
			return image;
		}

		// Token: 0x060086D5 RID: 34517 RVA: 0x00371FE2 File Offset: 0x003701E2
		private static void ConfigureRadial(Image img)
		{
			img.type = Image.Type.Filled;
			img.fillMethod = Image.FillMethod.Radial360;
			img.fillOrigin = 2;
			img.fillClockwise = true;
			img.fillAmount = 1f;
		}

		// Token: 0x060086D6 RID: 34518 RVA: 0x001A38AD File Offset: 0x001A1AAD
		private static void StretchFull(RectTransform rt)
		{
			rt.anchorMin = Vector2.zero;
			rt.anchorMax = Vector2.one;
			rt.offsetMin = Vector2.zero;
			rt.offsetMax = Vector2.zero;
		}

		// Token: 0x060086D7 RID: 34519 RVA: 0x0037200B File Offset: 0x0037020B
		private void EnsureRadialSprite()
		{
			this.radialSprite != null;
		}

		// Token: 0x060086D8 RID: 34520 RVA: 0x0037201A File Offset: 0x0037021A
		private void HandleClothAdded(int index, InventoryItemSO cloth)
		{
			this.RebuildCarousel();
		}

		// Token: 0x060086D9 RID: 34521 RVA: 0x0037201A File Offset: 0x0037021A
		private void HandleClothRemoved(int index)
		{
			this.RebuildCarousel();
		}

		// Token: 0x060086DA RID: 34522 RVA: 0x00372022 File Offset: 0x00370222
		private void HandleActiveClothChanged(int index)
		{
			this.RebuildCarousel();
			this.RefreshCycleHint();
		}

		// Token: 0x060086DB RID: 34523 RVA: 0x00372030 File Offset: 0x00370230
		private void HandleClothDurabilityChanged(int index, float normalized01)
		{
			for (int i = 0; i < this._slots.Count; i++)
			{
				if (this._uiToClothIndex[i] == index)
				{
					this._slots[i].SetRadial(normalized01);
					return;
				}
			}
		}

		// Token: 0x060086DC RID: 34524 RVA: 0x00372074 File Offset: 0x00370274
		private void RebuildCarousel()
		{
			if (this._slots.Count == 0)
			{
				return;
			}
			this._occupied.Clear();
			int maxClothSlots = this.inventoryManager.MaxClothSlots;
			for (int i = 0; i < maxClothSlots; i++)
			{
				if (this.inventoryManager.GetClothAt(i) != null)
				{
					this._occupied.Add(i);
				}
			}
			if (this._occupied.Count == 0)
			{
				for (int j = 0; j < this._slots.Count; j++)
				{
					this._slots[j].Clear();
					this._uiToClothIndex[j] = -1;
				}
				return;
			}
			int activeClothIndex = this.inventoryManager.ActiveClothIndex;
			bool flag = activeClothIndex >= 0 && this.inventoryManager.GetClothAt(activeClothIndex) != null;
			int num = (flag ? activeClothIndex : this.inventoryManager.LastFocusedClothIndex);
			int num2 = 0;
			if (num >= 0)
			{
				int num3 = this._occupied.IndexOf(num);
				if (num3 >= 0)
				{
					num2 = num3;
				}
			}
			for (int k = 0; k < this._slots.Count; k++)
			{
				if (k < this._occupied.Count)
				{
					int num4 = this._occupied[(num2 + k) % this._occupied.Count];
					this._uiToClothIndex[k] = num4;
					InventoryItemSO clothAt = this.inventoryManager.GetClothAt(num4);
					this._slots[k].SetCloth(clothAt, this.NormalizedDurability(num4));
					this._slots[k].SetSelected(k == 0 && flag);
				}
				else
				{
					this._slots[k].Clear();
					this._uiToClothIndex[k] = -1;
				}
			}
		}

		// Token: 0x060086DD RID: 34525 RVA: 0x0037222C File Offset: 0x0037042C
		public int ResolveClothIndexForUiSlot(ClothSlotUI uiSlot)
		{
			int num = this._slots.IndexOf(uiSlot);
			if (num < 0 || num >= this._uiToClothIndex.Length)
			{
				return -1;
			}
			return this._uiToClothIndex[num];
		}

		// Token: 0x060086DE RID: 34526 RVA: 0x0037201A File Offset: 0x0037021A
		private void RefreshAll()
		{
			this.RebuildCarousel();
		}

		// Token: 0x060086DF RID: 34527 RVA: 0x0037225F File Offset: 0x0037045F
		private float NormalizedDurability(int index)
		{
			if (this.inventoryManager.GetClothAt(index) == null)
			{
				return 1f;
			}
			return Mathf.Clamp01(this.inventoryManager.GetClothDurability(index) / this.inventoryManager.GetClothDurabilityBase(index));
		}

		// Token: 0x060086E0 RID: 34528 RVA: 0x00372299 File Offset: 0x00370499
		private bool IsValid(int index)
		{
			return index >= 0 && index < this._slots.Count;
		}

		// Token: 0x04008978 RID: 35192
		[Header("Binding")]
		[SerializeField]
		private PlayerInventoryManager inventoryManager;

		// Token: 0x04008979 RID: 35193
		[Header("Slots")]
		[Tooltip("The cloth mini-slots, authored as real children in the prefab (visible/stylable in editor). If left empty, the strip falls back to building them procedurally at runtime.")]
		[SerializeField]
		private List<ClothSlotUI> serializedSlots = new List<ClothSlotUI>();

		// Token: 0x0400897A RID: 35194
		[Header("Layout (procedural fallback only)")]
		[Tooltip("Parent the mini-slots are created under. If null, a child container is created on this object.")]
		[SerializeField]
		private RectTransform slotContainer;

		// Token: 0x0400897B RID: 35195
		[SerializeField]
		private float slotSize = 56f;

		// Token: 0x0400897C RID: 35196
		[SerializeField]
		private float slotSpacing = 8f;

		// Token: 0x0400897D RID: 35197
		[Tooltip("If true the container self-anchors to the top-left of the screen. Turn OFF when this HUD lives inside an existing layout group (e.g. above the RapidKeybinds column) so the parent layout positions it instead.")]
		[SerializeField]
		private bool selfAnchorTopLeft;

		// Token: 0x0400897E RID: 35198
		[Tooltip("Fixed height for this strip when driven by a parent layout group.")]
		[SerializeField]
		private float preferredHeight = 60f;

		// Token: 0x0400897F RID: 35199
		[Tooltip("When inside a vertical layout (e.g. the RapidKeybinds column), force this strip to the TOP so the cloth slots sit ABOVE the Arms/Emote rows, matching the mock.")]
		[SerializeField]
		private bool forceFirstSibling = true;

		// Token: 0x04008980 RID: 35200
		[Header("Style")]
		[Tooltip("Radial ring sprite (Filled/Radial360). Defaults to the shared TimedEffects ring if left null.")]
		[SerializeField]
		private Sprite radialSprite;

		// Token: 0x04008981 RID: 35201
		[SerializeField]
		private Color radialColor = Color.white;

		// Token: 0x04008982 RID: 35202
		[SerializeField]
		private Color radialBgColor = new Color(1f, 1f, 1f, 0.25f);

		// Token: 0x04008983 RID: 35203
		[SerializeField]
		private Color selectionColor = Color.white;

		// Token: 0x04008984 RID: 35204
		private const string DefaultRadialSpriteGuid = "0b9510d2790829140953392aae2b6679";

		// Token: 0x04008985 RID: 35205
		private readonly List<ClothSlotUI> _slots = new List<ClothSlotUI>();

		// Token: 0x04008986 RID: 35206
		private int[] _uiToClothIndex = new int[0];

		// Token: 0x04008987 RID: 35207
		private readonly List<int> _occupied = new List<int>();

		// Token: 0x04008988 RID: 35208
		private bool _bound;

		// Token: 0x04008989 RID: 35209
		private RectTransform _cycleHintRow;

		// Token: 0x0400898A RID: 35210
		private TMP_Text _cycleHintText;

		// Token: 0x0400898B RID: 35211
		private string _cycleHintBinding;

		// Token: 0x0400898C RID: 35212
		private InventoryContextBadge _contextBadge;

		// Token: 0x0400898D RID: 35213
		private bool _badgeResolved;
	}
}
