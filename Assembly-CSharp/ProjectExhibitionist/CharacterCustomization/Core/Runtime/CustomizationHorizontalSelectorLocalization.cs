using System;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AEF RID: 2799
	[DisallowMultipleComponent]
	public sealed class CustomizationHorizontalSelectorLocalization : MonoBehaviour
	{
		// Token: 0x06004F1D RID: 20253 RVA: 0x001EA920 File Offset: 0x001E8B20
		public static void Bind(HorizontalSelector selector)
		{
			if (selector == null)
			{
				return;
			}
			CustomizationHorizontalSelectorLocalization customizationHorizontalSelectorLocalization = selector.GetComponent<CustomizationHorizontalSelectorLocalization>() ?? selector.gameObject.AddComponent<CustomizationHorizontalSelectorLocalization>();
			if (customizationHorizontalSelectorLocalization._selector != null && customizationHorizontalSelectorLocalization._selector.onItemTextChanged != null)
			{
				customizationHorizontalSelectorLocalization._selector.onItemTextChanged.RemoveListener(new UnityAction<TMP_Text>(customizationHorizontalSelectorLocalization.OnTextChanged));
			}
			customizationHorizontalSelectorLocalization._selector = selector;
			UIManagerHSelector component = selector.GetComponent<UIManagerHSelector>();
			if (component != null)
			{
				component.overrideFonts = true;
			}
			if (selector.onItemTextChanged == null)
			{
				selector.onItemTextChanged = new HorizontalSelector.ItemTextChangedEvent();
			}
			selector.onItemTextChanged.AddListener(new UnityAction<TMP_Text>(customizationHorizontalSelectorLocalization.OnTextChanged));
			int num = Mathf.Clamp(selector.index, 0, selector.items.Count - 1);
			if (selector.items.Count > 0)
			{
				customizationHorizontalSelectorLocalization._currentSource = selector.items[num].itemTitle;
				CustomizationPresentation.BindColorLabel(selector.label, customizationHorizontalSelectorLocalization._currentSource, false);
				CustomizationPresentation.BindColorLabel(selector.labelHelper, customizationHorizontalSelectorLocalization._currentSource, false);
			}
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x001EAA30 File Offset: 0x001E8C30
		private void OnTextChanged(TMP_Text text)
		{
			string text2 = text.text;
			string text3 = ((this._selector.labelHelper != null && this._selector.labelHelper.text == text2) ? text2 : this._currentSource);
			this._currentSource = text2;
			CustomizationPresentation.BindColorLabel(text, text2, false);
			CustomizationPresentation.BindColorLabel(this._selector.labelHelper, text3, false);
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x001EAA9A File Offset: 0x001E8C9A
		private void OnDestroy()
		{
			if (this._selector != null && this._selector.onItemTextChanged != null)
			{
				this._selector.onItemTextChanged.RemoveListener(new UnityAction<TMP_Text>(this.OnTextChanged));
			}
		}

		// Token: 0x04005046 RID: 20550
		private HorizontalSelector _selector;

		// Token: 0x04005047 RID: 20551
		private string _currentSource;
	}
}
