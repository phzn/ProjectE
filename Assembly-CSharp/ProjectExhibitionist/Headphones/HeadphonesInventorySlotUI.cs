using System;
using Features.Inventory.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectExhibitionist.Headphones
{
	// Token: 0x020009D4 RID: 2516
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	public sealed class HeadphonesInventorySlotUI : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
	{
		// Token: 0x060046C5 RID: 18117 RVA: 0x001ABBF5 File Offset: 0x001A9DF5
		public void SetControllerSelected(bool selected)
		{
			this._controllerSelected = selected;
			this.ApplyOccupiedState(this._occupied);
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x001ABC0A File Offset: 0x001A9E0A
		public void OnPointerClick(PointerEventData eventData)
		{
			InventoryDialogController instance = InventoryDialogController.Instance;
			if (instance == null)
			{
				return;
			}
			instance.SelectHeadphones(this);
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x060046C7 RID: 18119 RVA: 0x001ABC1C File Offset: 0x001A9E1C
		public bool IsOccupied
		{
			get
			{
				return this._occupied;
			}
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x001ABC24 File Offset: 0x001A9E24
		private void Awake()
		{
			this.EnsureRuntimeVisual();
			this.ResolveInventoryPanel();
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x001ABC32 File Offset: 0x001A9E32
		private void OnEnable()
		{
			HeadphonesEquipment.WearStateChanged += this.HandleWearStateChanged;
			this.RefreshState();
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x001ABC4B File Offset: 0x001A9E4B
		private void Start()
		{
			this.RefreshState();
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x001ABC53 File Offset: 0x001A9E53
		private void OnDisable()
		{
			HeadphonesEquipment.WearStateChanged -= this.HandleWearStateChanged;
			this.CancelDrag();
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x001ABC6C File Offset: 0x001A9E6C
		public void Configure(Sprite sprite, RectTransform panel = null)
		{
			this.headphonesSprite = sprite;
			if (panel != null)
			{
				this.inventoryPanel = panel;
			}
			this.EnsureRuntimeVisual();
			this.RefreshState();
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x001ABC91 File Offset: 0x001A9E91
		public void RefreshState()
		{
			this.EnsureRuntimeVisual();
			this.ApplyOccupiedState(HeadphonesEquipment.IsWorn);
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x001ABCA4 File Offset: 0x001A9EA4
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.RefreshState();
			if (eventData == null || !this._occupied)
			{
				return;
			}
			this._dragging = true;
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = this.dragSourceAlpha;
			}
			this.CreateGhost(eventData);
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x001ABCF0 File Offset: 0x001A9EF0
		public void OnDrag(PointerEventData eventData)
		{
			if (!this._dragging || eventData == null)
			{
				return;
			}
			this.MoveGhost(eventData);
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x001ABD08 File Offset: 0x001A9F08
		public void OnEndDrag(PointerEventData eventData)
		{
			if (!this._dragging)
			{
				return;
			}
			this._dragging = false;
			this.DestroyGhost();
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = 1f;
			}
			if (eventData == null)
			{
				return;
			}
			this.ResolveInventoryPanel();
			if (this.inventoryPanel == null || RectTransformUtility.RectangleContainsScreenPoint(this.inventoryPanel, eventData.position, eventData.pressEventCamera))
			{
				return;
			}
			HeadphonesEquipment headphonesEquipment = HeadphonesEquipment.Current;
			if (headphonesEquipment != null && headphonesEquipment.HasHeadphones)
			{
				headphonesEquipment.TryDropTowardsScreenPoint(eventData.position);
			}
			this.RefreshState();
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x001ABDA5 File Offset: 0x001A9FA5
		private void HandleWearStateChanged(bool worn)
		{
			this.EnsureRuntimeVisual();
			this.ApplyOccupiedState(worn);
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x001ABDB4 File Offset: 0x001A9FB4
		private void ApplyOccupiedState(bool occupied)
		{
			this._occupied = occupied;
			if (this._background != null)
			{
				this._background.color = (occupied ? this.occupiedBackgroundColor : this.emptyBackgroundColor);
			}
			if (this._frame != null)
			{
				Color color = (this._controllerSelected ? new Color(1f, 0.78f, 0.18f, 1f) : (occupied ? this.occupiedFrameColor : this.emptyFrameColor));
				for (int i = 0; i < this._frame.Length; i++)
				{
					if (this._frame[i] != null)
					{
						this._frame[i].color = color;
					}
				}
			}
			if (this._icon != null)
			{
				this._icon.sprite = this.headphonesSprite;
				this._icon.preserveAspect = true;
				this._icon.raycastTarget = false;
				this._icon.enabled = occupied && this.headphonesSprite != null;
				this._icon.color = this.occupiedIconColor;
			}
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x001ABEC8 File Offset: 0x001AA0C8
		private void EnsureRuntimeVisual()
		{
			if (this._rect == null)
			{
				this._rect = (RectTransform)base.transform;
			}
			this._rect.sizeDelta = new Vector2(this.slotSize, this.slotSize);
			if (this._background == null)
			{
				this._background = base.GetComponent<Image>();
				if (this._background == null)
				{
					this._background = base.gameObject.AddComponent<Image>();
				}
				this._background.sprite = null;
				this._background.type = Image.Type.Simple;
				this._background.raycastTarget = true;
			}
			if (this._canvasGroup == null)
			{
				this._canvasGroup = base.GetComponent<CanvasGroup>();
				if (this._canvasGroup == null)
				{
					this._canvasGroup = base.gameObject.AddComponent<CanvasGroup>();
				}
				this._canvasGroup.interactable = true;
				this._canvasGroup.blocksRaycasts = true;
			}
			if (this._layoutElement == null)
			{
				this._layoutElement = base.GetComponent<LayoutElement>();
				if (this._layoutElement == null)
				{
					this._layoutElement = base.gameObject.AddComponent<LayoutElement>();
				}
			}
			this._layoutElement.ignoreLayout = false;
			this._layoutElement.minWidth = (this._layoutElement.preferredWidth = this.slotSize);
			this._layoutElement.minHeight = (this._layoutElement.preferredHeight = this.slotSize);
			if (this._frame == null || this._frame.Length != 4)
			{
				this._frame = new Image[]
				{
					this.EnsureBorder("FrameTop"),
					this.EnsureBorder("FrameRight"),
					this.EnsureBorder("FrameBottom"),
					this.EnsureBorder("FrameLeft")
				};
			}
			this.LayoutFrame();
			if (this._icon == null)
			{
				Transform transform = base.transform.Find("HeadphonesIcon");
				if (transform != null)
				{
					GameObject gameObject = transform.gameObject;
					this._icon = gameObject.GetComponent<Image>();
					if (this._icon == null)
					{
						this._icon = gameObject.AddComponent<Image>();
					}
				}
				else
				{
					GameObject gameObject = new GameObject("HeadphonesIcon", new Type[]
					{
						typeof(RectTransform),
						typeof(Image)
					});
					gameObject.layer = base.gameObject.layer;
					gameObject.transform.SetParent(base.transform, false);
					this._icon = gameObject.GetComponent<Image>();
				}
				this._icon.raycastTarget = false;
				this._icon.preserveAspect = true;
			}
			RectTransform rectTransform = this._icon.rectTransform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = Vector2.one * this.iconPadding;
			rectTransform.offsetMax = -Vector2.one * this.iconPadding;
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x001AC1BC File Offset: 0x001AA3BC
		private Image EnsureBorder(string childName)
		{
			Transform transform = base.transform.Find(childName);
			Image image;
			if (transform != null)
			{
				GameObject gameObject = transform.gameObject;
				image = gameObject.GetComponent<Image>();
				if (image == null)
				{
					image = gameObject.AddComponent<Image>();
				}
			}
			else
			{
				GameObject gameObject = new GameObject(childName, new Type[]
				{
					typeof(RectTransform),
					typeof(Image)
				});
				gameObject.layer = base.gameObject.layer;
				gameObject.transform.SetParent(base.transform, false);
				image = gameObject.GetComponent<Image>();
			}
			image.sprite = null;
			image.type = Image.Type.Simple;
			image.raycastTarget = false;
			return image;
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x001AC268 File Offset: 0x001AA468
		private void LayoutFrame()
		{
			if (this._frame == null || this._frame.Length != 4)
			{
				return;
			}
			HeadphonesInventorySlotUI.SetRect(this._frame[0].rectTransform, new Vector2(0f, 1f), Vector2.one, new Vector2(0f, -this.frameThickness), Vector2.zero);
			HeadphonesInventorySlotUI.SetRect(this._frame[1].rectTransform, new Vector2(1f, 0f), Vector2.one, new Vector2(-this.frameThickness, 0f), Vector2.zero);
			HeadphonesInventorySlotUI.SetRect(this._frame[2].rectTransform, Vector2.zero, new Vector2(1f, 0f), Vector2.zero, new Vector2(0f, this.frameThickness));
			HeadphonesInventorySlotUI.SetRect(this._frame[3].rectTransform, Vector2.zero, new Vector2(0f, 1f), Vector2.zero, new Vector2(this.frameThickness, 0f));
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x001AC377 File Offset: 0x001AA577
		private static void SetRect(RectTransform rect, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
		{
			if (rect == null)
			{
				return;
			}
			rect.anchorMin = anchorMin;
			rect.anchorMax = anchorMax;
			rect.offsetMin = offsetMin;
			rect.offsetMax = offsetMax;
			rect.localScale = Vector3.one;
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x001AC3AC File Offset: 0x001AA5AC
		private void ResolveInventoryPanel()
		{
			if (this.inventoryPanel != null)
			{
				return;
			}
			Transform transform = base.transform.parent;
			while (transform != null)
			{
				if (transform.name == "Panel")
				{
					RectTransform rectTransform = transform as RectTransform;
					if (rectTransform != null)
					{
						this.inventoryPanel = rectTransform;
						return;
					}
				}
				transform = transform.parent;
			}
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x001AC40C File Offset: 0x001AA60C
		private void CreateGhost(PointerEventData eventData)
		{
			this.DestroyGhost();
			this.ResolveInventoryPanel();
			RectTransform rectTransform;
			if (!(this.inventoryPanel != null))
			{
				Canvas componentInParent = base.GetComponentInParent<Canvas>();
				rectTransform = ((componentInParent != null) ? componentInParent.transform : null) as RectTransform;
			}
			else
			{
				rectTransform = this.inventoryPanel.parent as RectTransform;
			}
			this._ghostParent = rectTransform;
			if (this._ghostParent == null)
			{
				return;
			}
			GameObject gameObject = new GameObject("HeadphonesDragGhost", new Type[] { typeof(RectTransform) });
			gameObject.layer = base.gameObject.layer;
			this._ghost = (RectTransform)gameObject.transform;
			this._ghost.SetParent(this._ghostParent, false);
			RectTransform ghost = this._ghost;
			RectTransform ghost2 = this._ghost;
			Vector2 vector = new Vector2(0.5f, 0.5f);
			ghost2.anchorMax = vector;
			ghost.anchorMin = vector;
			this._ghost.pivot = new Vector2(0.5f, 0.5f);
			this._ghost.sizeDelta = new Vector2(this.slotSize, this.slotSize);
			Image image = gameObject.AddComponent<Image>();
			image.sprite = this.headphonesSprite;
			image.preserveAspect = true;
			image.raycastTarget = false;
			image.color = this.occupiedIconColor;
			image.enabled = this.headphonesSprite != null;
			Shadow shadow = gameObject.AddComponent<Shadow>();
			shadow.effectColor = new Color(0f, 0f, 0f, 0.6f);
			shadow.effectDistance = new Vector2(3f, -3f);
			this._ghost.SetAsLastSibling();
			this.MoveGhost(eventData);
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x001AC5AC File Offset: 0x001AA7AC
		private void MoveGhost(PointerEventData eventData)
		{
			if (this._ghost == null || this._ghostParent == null || eventData == null)
			{
				return;
			}
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this._ghostParent, eventData.position, eventData.pressEventCamera, out vector);
			this._ghost.anchoredPosition = vector;
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x001AC5FF File Offset: 0x001AA7FF
		private void CancelDrag()
		{
			this._dragging = false;
			this.DestroyGhost();
			if (this._canvasGroup != null)
			{
				this._canvasGroup.alpha = 1f;
			}
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x001AC62C File Offset: 0x001AA82C
		private void DestroyGhost()
		{
			if (this._ghost != null)
			{
				Object.Destroy(this._ghost.gameObject);
			}
			this._ghost = null;
			this._ghostParent = null;
		}

		// Token: 0x04004651 RID: 18001
		[Header("Content")]
		[SerializeField]
		private Sprite headphonesSprite;

		// Token: 0x04004652 RID: 18002
		[Header("Drop boundary")]
		[Tooltip("The purple InventoryDialog/Panel. If omitted, the closest ancestor named Panel is used.")]
		[SerializeField]
		private RectTransform inventoryPanel;

		// Token: 0x04004653 RID: 18003
		[Header("Runtime visual")]
		[SerializeField]
		[Min(40f)]
		private float slotSize = 72f;

		// Token: 0x04004654 RID: 18004
		[SerializeField]
		[Min(0f)]
		private float iconPadding = 6f;

		// Token: 0x04004655 RID: 18005
		[SerializeField]
		[Min(1f)]
		private float frameThickness = 2f;

		// Token: 0x04004656 RID: 18006
		[SerializeField]
		private Color emptyBackgroundColor = new Color(0.58f, 0.24f, 0.48f, 0.18f);

		// Token: 0x04004657 RID: 18007
		[SerializeField]
		private Color occupiedBackgroundColor = new Color(0.72f, 0.3f, 0.53f, 0.48f);

		// Token: 0x04004658 RID: 18008
		[SerializeField]
		private Color emptyFrameColor = new Color(1f, 1f, 1f, 0.22f);

		// Token: 0x04004659 RID: 18009
		[SerializeField]
		private Color occupiedFrameColor = new Color(1f, 1f, 1f, 0.92f);

		// Token: 0x0400465A RID: 18010
		[SerializeField]
		private Color emptyIconColor = new Color(1f, 1f, 1f, 0.18f);

		// Token: 0x0400465B RID: 18011
		[SerializeField]
		private Color occupiedIconColor = Color.white;

		// Token: 0x0400465C RID: 18012
		[SerializeField]
		[Range(0.1f, 1f)]
		private float dragSourceAlpha = 0.35f;

		// Token: 0x0400465D RID: 18013
		private RectTransform _rect;

		// Token: 0x0400465E RID: 18014
		private Image _background;

		// Token: 0x0400465F RID: 18015
		private Image _icon;

		// Token: 0x04004660 RID: 18016
		private Image[] _frame;

		// Token: 0x04004661 RID: 18017
		private CanvasGroup _canvasGroup;

		// Token: 0x04004662 RID: 18018
		private LayoutElement _layoutElement;

		// Token: 0x04004663 RID: 18019
		private RectTransform _ghost;

		// Token: 0x04004664 RID: 18020
		private RectTransform _ghostParent;

		// Token: 0x04004665 RID: 18021
		private bool _occupied;

		// Token: 0x04004666 RID: 18022
		private bool _dragging;

		// Token: 0x04004667 RID: 18023
		private bool _controllerSelected;
	}
}
