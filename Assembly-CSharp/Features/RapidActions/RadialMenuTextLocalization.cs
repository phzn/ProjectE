using System;
using System.Collections.Generic;
using Features.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Features.RapidActions
{
	// Token: 0x02000D8A RID: 3466
	[DisallowMultipleComponent]
	public sealed class RadialMenuTextLocalization : MonoBehaviour
	{
		// Token: 0x060065FA RID: 26106 RVA: 0x0028B6E4 File Offset: 0x002898E4
		public static RadialMenuTextLocalization Begin(UltimateRadialMenu wheel)
		{
			if (wheel == null)
			{
				return null;
			}
			RadialMenuTextLocalization radialMenuTextLocalization = wheel.GetComponent<RadialMenuTextLocalization>() ?? wheel.gameObject.AddComponent<RadialMenuTextLocalization>();
			radialMenuTextLocalization._wheel = wheel;
			radialMenuTextLocalization._sources.Clear();
			RadialMenuTextLocalization.BindFont(wheel.nameText);
			RadialMenuTextLocalization.BindFont(wheel.descriptionText);
			return radialMenuTextLocalization;
		}

		// Token: 0x060065FB RID: 26107 RVA: 0x0028B73C File Offset: 0x0028993C
		public void BindButton(int index, string scope, string englishName, string englishDescription = null)
		{
			this._sources[index] = new RadialMenuTextLocalization.Source
			{
				Scope = scope,
				Name = englishName,
				Description = (englishDescription ?? string.Empty)
			};
			this.RefreshButton(index, this._sources[index]);
		}

		// Token: 0x060065FC RID: 26108 RVA: 0x0028B78C File Offset: 0x0028998C
		private static void BindFont(Text text)
		{
			if (text != null)
			{
				LocalizedLegacyGameText.BindFontOnly(text).FitTranslationToRectangle();
			}
		}

		// Token: 0x060065FD RID: 26109 RVA: 0x0028B7A2 File Offset: 0x002899A2
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.Refresh;
			this.Refresh();
		}

		// Token: 0x060065FE RID: 26110 RVA: 0x0028B7BB File Offset: 0x002899BB
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.Refresh;
		}

		// Token: 0x060065FF RID: 26111 RVA: 0x0028B7D0 File Offset: 0x002899D0
		private void Refresh()
		{
			foreach (KeyValuePair<int, RadialMenuTextLocalization.Source> keyValuePair in this._sources)
			{
				this.RefreshButton(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x06006600 RID: 26112 RVA: 0x0028B830 File Offset: 0x00289A30
		private void RefreshButton(int index, RadialMenuTextLocalization.Source source)
		{
			if (this._wheel == null || index < 0 || index >= this._wheel.UltimateRadialButtonList.Count)
			{
				return;
			}
			UltimateRadialMenu.UltimateRadialButton ultimateRadialButton = this._wheel.UltimateRadialButtonList[index];
			if (ultimateRadialButton == null)
			{
				return;
			}
			ultimateRadialButton.UpdateName(GameLocalization.GetSource(source.Scope, source.Name));
			ultimateRadialButton.UpdateDescription(GameLocalization.GetSource(source.Scope, source.Description));
			if (this._wheel.useButtonText)
			{
				RadialMenuTextLocalization.BindFont(ultimateRadialButton.text);
			}
		}

		// Token: 0x04006871 RID: 26737
		private UltimateRadialMenu _wheel;

		// Token: 0x04006872 RID: 26738
		private readonly Dictionary<int, RadialMenuTextLocalization.Source> _sources = new Dictionary<int, RadialMenuTextLocalization.Source>();

		// Token: 0x02000D8B RID: 3467
		private sealed class Source
		{
			// Token: 0x04006873 RID: 26739
			public string Scope;

			// Token: 0x04006874 RID: 26740
			public string Name;

			// Token: 0x04006875 RID: 26741
			public string Description;
		}
	}
}
