using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Localization
{
	// Token: 0x020010CF RID: 4303
	[DisallowMultipleComponent]
	public sealed class LocalizedLegacyGameText : MonoBehaviour
	{
		// Token: 0x06008275 RID: 33397 RVA: 0x00354A14 File Offset: 0x00352C14
		public static Font SourceFont(Text target)
		{
			if (target == null)
			{
				return null;
			}
			LocalizedLegacyGameText component = target.GetComponent<LocalizedLegacyGameText>();
			if (!(component != null) || !component._initialized)
			{
				return target.font;
			}
			return component._originalFont;
		}

		// Token: 0x06008276 RID: 33398 RVA: 0x00354A54 File Offset: 0x00352C54
		public static LocalizedLegacyGameText Bind(Text target, string key, string english)
		{
			if (target == null)
			{
				return null;
			}
			LocalizedLegacyGameText localizedLegacyGameText = target.GetComponent<LocalizedLegacyGameText>() ?? target.gameObject.AddComponent<LocalizedLegacyGameText>();
			localizedLegacyGameText._target = target;
			if (!localizedLegacyGameText._initialized)
			{
				localizedLegacyGameText._originalFont = target.font;
				localizedLegacyGameText._originalBestFit = target.resizeTextForBestFit;
				localizedLegacyGameText._originalSize = target.fontSize;
				localizedLegacyGameText._originalMin = target.resizeTextMinSize;
				localizedLegacyGameText._originalMax = target.resizeTextMaxSize;
				localizedLegacyGameText._initialized = true;
			}
			localizedLegacyGameText._key = key;
			localizedLegacyGameText._english = english;
			localizedLegacyGameText._fontOnly = false;
			localizedLegacyGameText._resolver = null;
			localizedLegacyGameText.Refresh();
			return localizedLegacyGameText;
		}

		// Token: 0x06008277 RID: 33399 RVA: 0x00354AF8 File Offset: 0x00352CF8
		public static LocalizedLegacyGameText BindSource(Text target, string scope, string english)
		{
			string text;
			GameLocalization.TryGetSourceKey(scope, english, out text);
			return LocalizedLegacyGameText.Bind(target, text, english);
		}

		// Token: 0x06008278 RID: 33400 RVA: 0x00354B17 File Offset: 0x00352D17
		public static LocalizedLegacyGameText BindComputed(Text target, Func<string> resolver)
		{
			if (target == null)
			{
				return null;
			}
			LocalizedLegacyGameText localizedLegacyGameText = LocalizedLegacyGameText.Bind(target, null, target.text);
			localizedLegacyGameText._resolver = resolver;
			localizedLegacyGameText.Refresh();
			return localizedLegacyGameText;
		}

		// Token: 0x06008279 RID: 33401 RVA: 0x00354B3E File Offset: 0x00352D3E
		public static LocalizedLegacyGameText BindFontOnly(Text target)
		{
			if (target == null)
			{
				return null;
			}
			LocalizedLegacyGameText localizedLegacyGameText = LocalizedLegacyGameText.Bind(target, null, target.text);
			localizedLegacyGameText._fontOnly = true;
			localizedLegacyGameText.Refresh();
			return localizedLegacyGameText;
		}

		// Token: 0x0600827A RID: 33402 RVA: 0x00354B68 File Offset: 0x00352D68
		public static LocalizedLegacyGameText BindFormat(Text target, string scope, string english, params object[] arguments)
		{
			return LocalizedLegacyGameText.BindComputed(target, () => GameLocalization.FormatSource(scope, english, arguments));
		}

		// Token: 0x0600827B RID: 33403 RVA: 0x00354BA2 File Offset: 0x00352DA2
		public void FitTranslationToRectangle()
		{
			this._fit = true;
			this.Refresh();
		}

		// Token: 0x0600827C RID: 33404 RVA: 0x00354BB1 File Offset: 0x00352DB1
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.Refresh;
			this.Refresh();
		}

		// Token: 0x0600827D RID: 33405 RVA: 0x00354BCA File Offset: 0x00352DCA
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.Refresh;
		}

		// Token: 0x0600827E RID: 33406 RVA: 0x00354BE0 File Offset: 0x00352DE0
		private void Refresh()
		{
			if (this._target == null)
			{
				return;
			}
			TMP_FontAsset font = GameLocalization.GetFont(null);
			this._target.font = ((font != null && font.sourceFontFile != null) ? font.sourceFontFile : this._originalFont);
			if (this._fit && GameLocalization.CurrentCode != "en" && !this._originalBestFit)
			{
				this._target.resizeTextForBestFit = true;
				this._target.resizeTextMinSize = Mathf.Max(1, Mathf.FloorToInt((float)this._originalSize * 0.68f));
				this._target.resizeTextMaxSize = this._originalSize;
				this._layoutAdjusted = true;
			}
			else if (this._layoutAdjusted)
			{
				this._target.resizeTextForBestFit = this._originalBestFit;
				this._target.resizeTextMinSize = this._originalMin;
				this._target.resizeTextMaxSize = this._originalMax;
				this._target.fontSize = this._originalSize;
				this._layoutAdjusted = false;
			}
			if (!this._fontOnly)
			{
				this._target.text = ((this._resolver != null) ? this._resolver() : GameLocalization.Get(this._key, this._english));
			}
		}

		// Token: 0x040085A0 RID: 34208
		private Text _target;

		// Token: 0x040085A1 RID: 34209
		private Font _originalFont;

		// Token: 0x040085A2 RID: 34210
		private string _key;

		// Token: 0x040085A3 RID: 34211
		private string _english;

		// Token: 0x040085A4 RID: 34212
		private bool _initialized;

		// Token: 0x040085A5 RID: 34213
		private bool _fontOnly;

		// Token: 0x040085A6 RID: 34214
		private bool _fit;

		// Token: 0x040085A7 RID: 34215
		private bool _layoutAdjusted;

		// Token: 0x040085A8 RID: 34216
		private bool _originalBestFit;

		// Token: 0x040085A9 RID: 34217
		private int _originalSize;

		// Token: 0x040085AA RID: 34218
		private int _originalMin;

		// Token: 0x040085AB RID: 34219
		private int _originalMax;

		// Token: 0x040085AC RID: 34220
		private Func<string> _resolver;
	}
}
