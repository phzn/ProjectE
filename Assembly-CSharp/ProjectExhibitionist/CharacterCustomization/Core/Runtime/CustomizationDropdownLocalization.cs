using System;
using Features.Localization;
using Michsky.MUIP;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AEC RID: 2796
	[DisallowMultipleComponent]
	public sealed class CustomizationDropdownLocalization : MonoBehaviour
	{
		// Token: 0x06004F09 RID: 20233 RVA: 0x001EA318 File Offset: 0x001E8518
		public static void Bind(CustomDropdown dropdown, bool firstOptionOnly = false, string scope = "cielos_apps")
		{
			if (dropdown == null)
			{
				return;
			}
			CustomizationDropdownLocalization customizationDropdownLocalization = dropdown.GetComponent<CustomizationDropdownLocalization>() ?? dropdown.gameObject.AddComponent<CustomizationDropdownLocalization>();
			if (customizationDropdownLocalization._custom != null)
			{
				customizationDropdownLocalization._custom.onLocalizedItemTextChanged.RemoveListener(new UnityAction<TMP_Text, int>(customizationDropdownLocalization.OnItemTextChanged));
			}
			customizationDropdownLocalization._custom = dropdown;
			customizationDropdownLocalization._firstOptionOnly = firstOptionOnly;
			customizationDropdownLocalization._scope = scope;
			UIManagerDropdown component = dropdown.GetComponent<UIManagerDropdown>();
			if (component != null)
			{
				component.overrideFonts = true;
			}
			if (dropdown.onLocalizedItemTextChanged == null)
			{
				dropdown.onLocalizedItemTextChanged = new CustomDropdown.LocalizedItemTextChangedEvent();
			}
			dropdown.onLocalizedItemTextChanged.AddListener(new UnityAction<TMP_Text, int>(customizationDropdownLocalization.OnItemTextChanged));
			customizationDropdownLocalization.RefreshCustom();
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x001EA3CC File Offset: 0x001E85CC
		public static void Bind(TMP_Dropdown dropdown, string[] sources)
		{
			if (dropdown == null)
			{
				return;
			}
			CustomizationDropdownLocalization customizationDropdownLocalization = dropdown.GetComponent<CustomizationDropdownLocalization>() ?? dropdown.gameObject.AddComponent<CustomizationDropdownLocalization>();
			customizationDropdownLocalization._tmp = dropdown;
			customizationDropdownLocalization._sources = (string[])sources.Clone();
			LocalizedGameText.BindFontOnly(dropdown.captionText);
			LocalizedGameText.BindFontOnly(dropdown.itemText);
			customizationDropdownLocalization.Refresh();
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x001EA42D File Offset: 0x001E862D
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.Refresh;
			this.Refresh();
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x001EA446 File Offset: 0x001E8646
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.Refresh;
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x001EA459 File Offset: 0x001E8659
		private void OnDestroy()
		{
			if (this._custom != null)
			{
				this._custom.onLocalizedItemTextChanged.RemoveListener(new UnityAction<TMP_Text, int>(this.OnItemTextChanged));
			}
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x001EA485 File Offset: 0x001E8685
		private void OnItemTextChanged(TMP_Text text, int index)
		{
			this.BindCustomText(text, index);
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x001EA490 File Offset: 0x001E8690
		private void BindCustomText(TMP_Text text, int index)
		{
			if (text == null || index < 0 || index >= this._custom.items.Count)
			{
				return;
			}
			UIManagerDropdownItem componentInParent = text.GetComponentInParent<UIManagerDropdownItem>(true);
			if (componentInParent != null)
			{
				componentInParent.overrideFonts = true;
			}
			if (this._firstOptionOnly && index != 0)
			{
				LocalizedGameText.BindFontOnly(text);
				return;
			}
			LocalizedGameText localizedGameText = LocalizedGameText.BindSource(text, this._scope, this._custom.items[index].itemName, null);
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x001EA518 File Offset: 0x001E8718
		private void RefreshCustom()
		{
			if (this._custom == null)
			{
				return;
			}
			this.BindCustomText(this._custom.selectedText, this._custom.selectedItemIndex);
			if (this._custom.itemParent == null)
			{
				return;
			}
			int num = 0;
			while (num < this._custom.itemParent.childCount && num < this._custom.items.Count)
			{
				this.BindCustomText(this._custom.itemParent.GetChild(num).GetComponentInChildren<TMP_Text>(true), num);
				num++;
			}
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x001EA5B0 File Offset: 0x001E87B0
		private void Refresh()
		{
			this.RefreshCustom();
			if (this._tmp == null || this._sources == null)
			{
				return;
			}
			bool isExpanded = this._tmp.IsExpanded;
			if (isExpanded)
			{
				this._tmp.Hide();
			}
			int num = 0;
			while (num < this._sources.Length && num < this._tmp.options.Count)
			{
				this._tmp.options[num].text = GameLocalization.GetSource("cielos_apps", this._sources[num]);
				num++;
			}
			this._tmp.RefreshShownValue();
			if (isExpanded)
			{
				this._tmp.Show();
			}
		}

		// Token: 0x04005038 RID: 20536
		private CustomDropdown _custom;

		// Token: 0x04005039 RID: 20537
		private TMP_Dropdown _tmp;

		// Token: 0x0400503A RID: 20538
		private string[] _sources;

		// Token: 0x0400503B RID: 20539
		private bool _firstOptionOnly;

		// Token: 0x0400503C RID: 20540
		private string _scope = "cielos_apps";
	}
}
