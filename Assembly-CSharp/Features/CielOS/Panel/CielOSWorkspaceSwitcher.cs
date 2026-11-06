using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.CielOS.Panel
{
	// Token: 0x020013E4 RID: 5092
	public class CielOSWorkspaceSwitcher : MonoBehaviour
	{
		// Token: 0x170018D8 RID: 6360
		// (get) Token: 0x06009D2A RID: 40234 RVA: 0x00408E81 File Offset: 0x00407081
		public int Active
		{
			get
			{
				return this._active;
			}
		}

		// Token: 0x06009D2B RID: 40235 RVA: 0x00408E8C File Offset: 0x0040708C
		public void Build(int count)
		{
			if (this._built)
			{
				return;
			}
			this._built = true;
			HorizontalLayoutGroup horizontalLayoutGroup = base.GetComponent<HorizontalLayoutGroup>();
			if (horizontalLayoutGroup == null)
			{
				horizontalLayoutGroup = base.gameObject.AddComponent<HorizontalLayoutGroup>();
			}
			horizontalLayoutGroup.spacing = 6f;
			horizontalLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
			horizontalLayoutGroup.childForceExpandWidth = false;
			horizontalLayoutGroup.childForceExpandHeight = false;
			for (int i = 0; i < count; i++)
			{
				int index = i;
				GameObject gameObject = new GameObject("WS" + (i + 1).ToString(), new Type[]
				{
					typeof(RectTransform),
					typeof(Image),
					typeof(Button),
					typeof(LayoutElement)
				});
				gameObject.transform.SetParent(base.transform, false);
				LayoutElement component = gameObject.GetComponent<LayoutElement>();
				component.minWidth = (component.preferredWidth = 28f);
				component.minHeight = (component.preferredHeight = 24f);
				Image component2 = gameObject.GetComponent<Image>();
				CielOSTheme.Round(component2);
				this._chipBgs.Add(component2);
				GameObject gameObject2 = new GameObject("Num", new Type[]
				{
					typeof(RectTransform),
					typeof(TextMeshProUGUI)
				});
				gameObject2.transform.SetParent(gameObject.transform, false);
				RectTransform rectTransform = (RectTransform)gameObject2.transform;
				rectTransform.anchorMin = Vector2.zero;
				rectTransform.anchorMax = Vector2.one;
				rectTransform.offsetMin = Vector2.zero;
				rectTransform.offsetMax = Vector2.zero;
				TextMeshProUGUI component3 = gameObject2.GetComponent<TextMeshProUGUI>();
				component3.text = (i + 1).ToString();
				component3.fontSize = 13f;
				component3.alignment = TextAlignmentOptions.Center;
				component3.raycastTarget = false;
				if (CielOSTheme.Active != null && CielOSTheme.Active.uiFont != null)
				{
					component3.font = CielOSTheme.Active.uiFont;
				}
				gameObject.GetComponent<Button>().onClick.AddListener(delegate
				{
					this.SetActive(index);
				});
			}
			this.SetActive(0);
		}

		// Token: 0x06009D2C RID: 40236 RVA: 0x004090C0 File Offset: 0x004072C0
		public void SetActive(int index)
		{
			this._active = index;
			CielOSTheme active = CielOSTheme.Active;
			Color color = ((active != null) ? active.accent : new Color(0.66f, 0.38f, 0.79f, 1f));
			Color color2 = ((active != null) ? active.controlBackground : new Color(0.18f, 0.14f, 0.23f, 1f));
			for (int i = 0; i < this._chipBgs.Count; i++)
			{
				if (this._chipBgs[i] != null)
				{
					this._chipBgs[i].color = ((i == index) ? color : color2);
				}
			}
		}

		// Token: 0x0400A1AC RID: 41388
		private readonly List<Image> _chipBgs = new List<Image>();

		// Token: 0x0400A1AD RID: 41389
		private int _active;

		// Token: 0x0400A1AE RID: 41390
		private bool _built;
	}
}
