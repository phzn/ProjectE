using System;
using System.Collections.Generic;
using Features.Localization;
using ProjectExhibitionist.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.CielOS
{
	// Token: 0x0200137F RID: 4991
	[DefaultExecutionOrder(-400)]
	public class CielOSContextMenu : MonoBehaviour
	{
		// Token: 0x170018A6 RID: 6310
		// (get) Token: 0x06009A95 RID: 39573 RVA: 0x003F3A1B File Offset: 0x003F1C1B
		// (set) Token: 0x06009A96 RID: 39574 RVA: 0x003F3A22 File Offset: 0x003F1C22
		public static CielOSContextMenu Instance { get; private set; }

		// Token: 0x170018A7 RID: 6311
		// (get) Token: 0x06009A97 RID: 39575 RVA: 0x003F3A2A File Offset: 0x003F1C2A
		public bool IsOpen
		{
			get
			{
				return this.panel != null && this.panel.gameObject.activeSelf;
			}
		}

		// Token: 0x06009A98 RID: 39576 RVA: 0x003F3A4C File Offset: 0x003F1C4C
		private void Awake()
		{
			if (CielOSContextMenu.Instance != null && CielOSContextMenu.Instance != this)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			CielOSContextMenu.Instance = this;
			this._canvas = base.GetComponentInParent<Canvas>();
			if (this.blockerButton != null)
			{
				this.blockerButton.onClick.AddListener(new UnityAction(this.Hide));
			}
			if (this.panel != null && this._canvas != null)
			{
				this.panel.SetParent((RectTransform)this._canvas.transform, false);
			}
			this.Hide();
		}

		// Token: 0x06009A99 RID: 39577 RVA: 0x003F3AFC File Offset: 0x003F1CFC
		private void Update()
		{
			if (!this.IsOpen)
			{
				return;
			}
			if (EscapeInputCoordinator.TryConsume(this))
			{
				this.Hide();
				return;
			}
			if (Time.frameCount == this._openedFrame)
			{
				return;
			}
			if ((GameplayInput.GetMouseButtonDown(0) || GameplayInput.GetMouseButtonDown(1) || GameplayInput.GetMouseButtonDown(2)) && !this.PointerOverPanel())
			{
				this.Hide();
			}
		}

		// Token: 0x06009A9A RID: 39578 RVA: 0x003F3B58 File Offset: 0x003F1D58
		private bool PointerOverPanel()
		{
			return !(this.panel == null) && RectTransformUtility.RectangleContainsScreenPoint(this.panel, GameplayInput.mousePosition, (this._canvas != null && this._canvas.renderMode != RenderMode.ScreenSpaceOverlay) ? this._canvas.worldCamera : null);
		}

		// Token: 0x06009A9B RID: 39579 RVA: 0x003F3BB4 File Offset: 0x003F1DB4
		public void Show(Vector2 screenPos, List<CielOSContextMenu.Item> items)
		{
			if (this.panel == null || items == null || items.Count == 0)
			{
				return;
			}
			RectTransform rectTransform = ((this.itemParent != null) ? this.itemParent : this.panel);
			foreach (GameObject gameObject in this._rows)
			{
				if (gameObject != null)
				{
					gameObject.transform.SetParent(null, false);
					Object.Destroy(gameObject);
				}
			}
			this._rows.Clear();
			foreach (CielOSContextMenu.Item item in items)
			{
				this._rows.Add(this.BuildRow(rectTransform, item));
			}
			this._openedFrame = Time.frameCount;
			ControllerNavigationScope.Configure(this.panel.gameObject, new Action(this.Hide), null, 110, false);
			if (this.blocker != null)
			{
				this.blocker.SetActive(true);
			}
			this.panel.gameObject.SetActive(true);
			this.panel.SetAsLastSibling();
			if (this.blocker != null)
			{
				this.blocker.transform.SetAsLastSibling();
			}
			this.panel.SetAsLastSibling();
			this.PositionAt(screenPos);
		}

		// Token: 0x06009A9C RID: 39580 RVA: 0x003F3D3C File Offset: 0x003F1F3C
		public void Hide()
		{
			if (this.panel != null)
			{
				this.panel.gameObject.SetActive(false);
			}
			if (this.blocker != null)
			{
				this.blocker.SetActive(false);
			}
		}

		// Token: 0x06009A9D RID: 39581 RVA: 0x003F3D78 File Offset: 0x003F1F78
		private GameObject BuildRow(RectTransform parent, CielOSContextMenu.Item item)
		{
			GameObject gameObject = new GameObject(item.label, new Type[]
			{
				typeof(RectTransform),
				typeof(Image),
				typeof(Button),
				typeof(LayoutElement)
			});
			gameObject.transform.SetParent(parent, false);
			Image component = gameObject.GetComponent<Image>();
			component.color = (item.enabled ? this.rowColor : this.rowDisabledColor);
			CielOSTheme.Round(component);
			LayoutElement component2 = gameObject.GetComponent<LayoutElement>();
			component2.minHeight = 28f;
			component2.preferredHeight = 28f;
			GameObject gameObject2 = new GameObject("Label", new Type[]
			{
				typeof(RectTransform),
				typeof(TextMeshProUGUI)
			});
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform = (RectTransform)gameObject2.transform;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.offsetMin = new Vector2(12f, 0f);
			rectTransform.offsetMax = new Vector2(-12f, 0f);
			TextMeshProUGUI component3 = gameObject2.GetComponent<TextMeshProUGUI>();
			component3.text = item.label;
			component3.fontSize = 13f;
			component3.color = (item.enabled ? this.textColor : this.textDisabledColor);
			component3.alignment = TextAlignmentOptions.MidlineLeft;
			component3.raycastTarget = false;
			if (item.localizationScope != null)
			{
				LocalizedGameText.BindSource(component3, item.localizationScope, item.label, null).FitTranslationToRectangle(false);
			}
			Button component4 = gameObject.GetComponent<Button>();
			component4.interactable = item.enabled;
			if (item.enabled)
			{
				Action a = item.action;
				component4.onClick.AddListener(delegate
				{
					this.Hide();
					Action a2 = a;
					if (a2 == null)
					{
						return;
					}
					a2();
				});
			}
			return gameObject;
		}

		// Token: 0x06009A9E RID: 39582 RVA: 0x003F3F58 File Offset: 0x003F2158
		private void PositionAt(Vector2 screenPos)
		{
			RectTransform rectTransform = (RectTransform)this._canvas.transform;
			Vector2 vector;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, (this._canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : this._canvas.worldCamera, out vector);
			this.panel.SetParent(rectTransform, false);
			this.panel.pivot = new Vector2(0f, 1f);
			RectTransform rectTransform2 = this.panel;
			RectTransform rectTransform3 = this.panel;
			Vector2 vector2 = new Vector2(0.5f, 0.5f);
			rectTransform3.anchorMax = vector2;
			rectTransform2.anchorMin = vector2;
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.panel);
			Vector2 sizeDelta = this.panel.sizeDelta;
			Rect rect = rectTransform.rect;
			float num = rect.xMax - sizeDelta.x;
			float num2 = rect.yMin + sizeDelta.y;
			vector.x = Mathf.Min(vector.x, num);
			vector.y = Mathf.Max(vector.y, num2);
			this.panel.anchoredPosition = vector;
		}

		// Token: 0x04009F64 RID: 40804
		[Header("References")]
		[Tooltip("Root panel toggled on/off (the popup). Should have a vertical layout + content size fitter.")]
		[SerializeField]
		private RectTransform panel;

		// Token: 0x04009F65 RID: 40805
		[Tooltip("Parent where item rows are spawned (vertical layout). If null, uses panel.")]
		[SerializeField]
		private RectTransform itemParent;

		// Token: 0x04009F66 RID: 40806
		[Tooltip("Full-screen invisible blocker that closes the menu when clicked.")]
		[SerializeField]
		private GameObject blocker;

		// Token: 0x04009F67 RID: 40807
		[Tooltip("Button on the blocker; clicking it (outside the menu) closes the menu.")]
		[SerializeField]
		private Button blockerButton;

		// Token: 0x04009F68 RID: 40808
		[Header("Style")]
		[SerializeField]
		private Color rowColor = new Color(0.13f, 0.1f, 0.16f, 1f);

		// Token: 0x04009F69 RID: 40809
		[SerializeField]
		private Color rowDisabledColor = new Color(0.1f, 0.09f, 0.12f, 1f);

		// Token: 0x04009F6A RID: 40810
		[SerializeField]
		private Color textColor = new Color(0.9f, 0.92f, 0.96f, 1f);

		// Token: 0x04009F6B RID: 40811
		[SerializeField]
		private Color textDisabledColor = new Color(0.5f, 0.48f, 0.55f, 1f);

		// Token: 0x04009F6C RID: 40812
		private readonly List<GameObject> _rows = new List<GameObject>();

		// Token: 0x04009F6D RID: 40813
		private Canvas _canvas;

		// Token: 0x04009F6E RID: 40814
		private int _openedFrame = -1;

		// Token: 0x02001380 RID: 4992
		public struct Item
		{
			// Token: 0x06009AA0 RID: 39584 RVA: 0x003F4101 File Offset: 0x003F2301
			public Item(string label, Action action, bool enabled = true, bool separatorAfter = false, string localizationScope = null)
			{
				this.label = label;
				this.action = action;
				this.enabled = enabled;
				this.separatorAfter = separatorAfter;
				this.localizationScope = localizationScope;
			}

			// Token: 0x04009F6F RID: 40815
			public string label;

			// Token: 0x04009F70 RID: 40816
			public Action action;

			// Token: 0x04009F71 RID: 40817
			public bool enabled;

			// Token: 0x04009F72 RID: 40818
			public bool separatorAfter;

			// Token: 0x04009F73 RID: 40819
			public string localizationScope;
		}
	}
}
