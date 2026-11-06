using System;
using TMPro;
using UnityEngine;

namespace Features.Localization
{
	// Token: 0x020010CD RID: 4301
	[DisallowMultipleComponent]
	public sealed class LocalizedGameText : MonoBehaviour
	{
		// Token: 0x06008261 RID: 33377 RVA: 0x0035428C File Offset: 0x0035248C
		public static TMP_FontAsset SourceFont(TMP_Text text)
		{
			if (text == null)
			{
				return null;
			}
			LocalizedGameText component = text.GetComponent<LocalizedGameText>();
			if (!(component != null) || !component._initialized)
			{
				return text.font;
			}
			return component._originalFont;
		}

		// Token: 0x06008262 RID: 33378 RVA: 0x003542CC File Offset: 0x003524CC
		public static float SourceFontSize(TMP_Text text)
		{
			if (text == null)
			{
				return 0f;
			}
			LocalizedGameText component = text.GetComponent<LocalizedGameText>();
			if (!(component != null) || !component._initialized)
			{
				return text.fontSize;
			}
			return component._originalSize;
		}

		// Token: 0x06008263 RID: 33379 RVA: 0x00354310 File Offset: 0x00352510
		public void SetSourceLayout(bool autoSize, bool wordWrapping, float minSize, float maxSize)
		{
			if (this.target == null || minSize <= 0f || maxSize < minSize)
			{
				return;
			}
			this.CaptureStyle();
			this._originalAutoSize = autoSize;
			this._originalWrap = wordWrapping;
			this._originalMin = minSize;
			this._originalMax = maxSize;
			this._originalSize = maxSize;
			this._layoutAdjusted = false;
			this._wrapAdjusted = false;
			this.target.enableAutoSizing = autoSize;
			this.target.enableWordWrapping = wordWrapping;
			this.target.fontSizeMin = minSize;
			this.target.fontSizeMax = maxSize;
			this.target.fontSize = maxSize;
			this.Refresh();
		}

		// Token: 0x06008264 RID: 33380 RVA: 0x003543B8 File Offset: 0x003525B8
		public static LocalizedGameText BindSource(TMP_Text target, string scope, string english, Func<string, string> transform = null)
		{
			if (target == null)
			{
				return null;
			}
			string text;
			GameLocalization.TryGetSourceKey(scope, english, out text);
			return LocalizedGameText.Bind(target, text, english, transform);
		}

		// Token: 0x06008265 RID: 33381 RVA: 0x003543E4 File Offset: 0x003525E4
		public static LocalizedGameText Bind(TMP_Text target, string key, string english, Func<string, string> transform = null)
		{
			if (target == null)
			{
				return null;
			}
			LocalizedGameText localizedGameText = target.GetComponent<LocalizedGameText>() ?? target.gameObject.AddComponent<LocalizedGameText>();
			localizedGameText.target = target;
			localizedGameText.CaptureStyle();
			localizedGameText.key = key;
			localizedGameText.english = english;
			localizedGameText._transform = transform;
			localizedGameText._resolver = null;
			localizedGameText._fontOnly = false;
			localizedGameText._fitTranslation = key != null && (key.StartsWith("settings.", StringComparison.Ordinal) || key.StartsWith("menu.settings.", StringComparison.Ordinal));
			localizedGameText.Refresh();
			return localizedGameText;
		}

		// Token: 0x06008266 RID: 33382 RVA: 0x00354470 File Offset: 0x00352670
		public static LocalizedGameText BindComputed(TMP_Text target, Func<string> resolver)
		{
			if (target == null)
			{
				return null;
			}
			LocalizedGameText localizedGameText = target.GetComponent<LocalizedGameText>() ?? target.gameObject.AddComponent<LocalizedGameText>();
			localizedGameText.target = target;
			localizedGameText.CaptureStyle();
			localizedGameText._resolver = resolver;
			localizedGameText._fontOnly = false;
			localizedGameText._fitTranslation = false;
			localizedGameText.key = null;
			localizedGameText._transform = null;
			localizedGameText.Refresh();
			return localizedGameText;
		}

		// Token: 0x06008267 RID: 33383 RVA: 0x003544D4 File Offset: 0x003526D4
		public static LocalizedGameText BindFormat(TMP_Text target, string scope, string english, params object[] arguments)
		{
			return LocalizedGameText.BindComputed(target, () => GameLocalization.FormatSource(scope, english, arguments));
		}

		// Token: 0x06008268 RID: 33384 RVA: 0x0035450E File Offset: 0x0035270E
		public static LocalizedGameText BindFontOnly(TMP_Text target)
		{
			if (target == null)
			{
				return null;
			}
			LocalizedGameText localizedGameText = LocalizedGameText.Bind(target, null, target.text, null);
			localizedGameText._fontOnly = true;
			localizedGameText.Refresh();
			return localizedGameText;
		}

		// Token: 0x06008269 RID: 33385 RVA: 0x00354536 File Offset: 0x00352736
		public void FitTranslationToRectangle(bool singleLine = false)
		{
			this._fitFixedRectangle = true;
			this._fitSingleLine = singleLine;
			this.Refresh();
		}

		// Token: 0x0600826A RID: 33386 RVA: 0x0035454C File Offset: 0x0035274C
		public void SetMaterialResolver(Func<TMP_FontAsset, Material> resolver)
		{
			this._materialResolver = resolver;
			this.Refresh();
		}

		// Token: 0x0600826B RID: 33387 RVA: 0x0035455B File Offset: 0x0035275B
		public void SetFontOverride(TMP_FontAsset font)
		{
			this._fontOverride = font;
			this.Refresh();
		}

		// Token: 0x0600826C RID: 33388 RVA: 0x0035456A File Offset: 0x0035276A
		public void SetSourceFont(TMP_FontAsset font)
		{
			if (font == null)
			{
				return;
			}
			this._originalFont = font;
			this._originalMaterial = font.material;
			this.Refresh();
		}

		// Token: 0x0600826D RID: 33389 RVA: 0x00354590 File Offset: 0x00352790
		public void SetSourceFontSize(float size)
		{
			if (this.target == null || size <= 0f)
			{
				return;
			}
			this.CaptureStyle();
			float num = ((this._originalSize > 0f) ? (this._originalMin / this._originalSize) : 0.68f);
			this._originalSize = size;
			this._originalMin = size * num;
			this._originalMax = size;
			this.target.fontSize = size;
			this.Refresh();
		}

		// Token: 0x0600826E RID: 33390 RVA: 0x00354605 File Offset: 0x00352805
		private void OnEnable()
		{
			GameLocalization.LocaleChanged += this.Refresh;
			if (this.target != null)
			{
				this.Refresh();
			}
		}

		// Token: 0x0600826F RID: 33391 RVA: 0x0035462C File Offset: 0x0035282C
		private void OnDisable()
		{
			GameLocalization.LocaleChanged -= this.Refresh;
		}

		// Token: 0x06008270 RID: 33392 RVA: 0x00354640 File Offset: 0x00352840
		private void CaptureStyle()
		{
			if (this._initialized || this.target == null)
			{
				return;
			}
			this._originalFont = ((this.target.font != null) ? this.target.font : TMP_Settings.defaultFontAsset);
			this._originalMaterial = this.target.fontSharedMaterial;
			this._originalAutoSize = this.target.enableAutoSizing;
			this._originalWrap = this.target.enableWordWrapping;
			this._originalSize = this.target.fontSize;
			this._originalMin = this.target.fontSizeMin;
			this._originalMax = this.target.fontSizeMax;
			this._initialized = true;
		}

		// Token: 0x06008271 RID: 33393 RVA: 0x003546FC File Offset: 0x003528FC
		public void Refresh()
		{
			if (this.target == null)
			{
				return;
			}
			this.CaptureStyle();
			string text = ((this._resolver != null) ? this._resolver() : GameLocalization.Get(this.key, this.english));
			bool flag = GameLocalization.CurrentCode != "en" && (this._fontOnly || this._resolver != null || !string.Equals(text, this.english, StringComparison.Ordinal));
			TMP_FontAsset tmp_FontAsset = ((this._fontOverride != null) ? this._fontOverride : (flag ? GameLocalization.GetFont(this._originalFont) : this._originalFont));
			if (tmp_FontAsset != null && this.target.font != tmp_FontAsset)
			{
				this.target.font = tmp_FontAsset;
				this.target.fontSharedMaterial = ((tmp_FontAsset == this._originalFont && this._originalMaterial != null) ? this._originalMaterial : tmp_FontAsset.material);
			}
			if (tmp_FontAsset != null && this._materialResolver != null)
			{
				Material material = this._materialResolver(tmp_FontAsset) ?? ((tmp_FontAsset == this._originalFont && this._originalMaterial != null) ? this._originalMaterial : tmp_FontAsset.material);
				if (this.target.fontSharedMaterial != material)
				{
					this.target.fontSharedMaterial = material;
				}
			}
			bool flag2 = flag && (this._fitTranslation || this._fitFixedRectangle);
			if (!this._originalAutoSize && flag2 && (this._layoutAdjusted || !this.target.enableAutoSizing))
			{
				this.target.enableAutoSizing = true;
				this.target.fontSizeMin = this._originalSize * (this._fitSingleLine ? 0.5f : 0.68f);
				this.target.fontSizeMax = this._originalSize;
				if (this._fitTranslation || this._fitSingleLine)
				{
					this.target.enableWordWrapping = !this._fitSingleLine && this.english != null && this.english.Contains("\n");
					this._wrapAdjusted = true;
				}
				this._layoutAdjusted = true;
			}
			else if (this._layoutAdjusted && !flag2)
			{
				this.target.enableAutoSizing = this._originalAutoSize;
				this.target.fontSizeMin = this._originalMin;
				this.target.fontSizeMax = this._originalMax;
				this.target.fontSize = this._originalSize;
				this._layoutAdjusted = false;
			}
			if (this._wrapAdjusted && (!flag2 || (!this._fitTranslation && !this._fitSingleLine)))
			{
				this.target.enableWordWrapping = this._originalWrap;
				this._wrapAdjusted = false;
			}
			if (!this._fontOnly)
			{
				this.target.text = ((this._transform == null) ? text : this._transform(text));
			}
		}

		// Token: 0x04008588 RID: 34184
		[SerializeField]
		private TMP_Text target;

		// Token: 0x04008589 RID: 34185
		[SerializeField]
		private string key;

		// Token: 0x0400858A RID: 34186
		[SerializeField]
		[TextArea]
		private string english;

		// Token: 0x0400858B RID: 34187
		[SerializeField]
		[HideInInspector]
		private TMP_FontAsset _originalFont;

		// Token: 0x0400858C RID: 34188
		[SerializeField]
		[HideInInspector]
		private Material _originalMaterial;

		// Token: 0x0400858D RID: 34189
		private Func<string, string> _transform;

		// Token: 0x0400858E RID: 34190
		private Func<string> _resolver;

		// Token: 0x0400858F RID: 34191
		private Func<TMP_FontAsset, Material> _materialResolver;

		// Token: 0x04008590 RID: 34192
		[SerializeField]
		[HideInInspector]
		private bool _initialized;

		// Token: 0x04008591 RID: 34193
		[SerializeField]
		[HideInInspector]
		private bool _fitTranslation;

		// Token: 0x04008592 RID: 34194
		[SerializeField]
		[HideInInspector]
		private bool _fitFixedRectangle;

		// Token: 0x04008593 RID: 34195
		[SerializeField]
		[HideInInspector]
		private bool _fitSingleLine;

		// Token: 0x04008594 RID: 34196
		[SerializeField]
		[HideInInspector]
		private bool _layoutAdjusted;

		// Token: 0x04008595 RID: 34197
		[SerializeField]
		[HideInInspector]
		private bool _wrapAdjusted;

		// Token: 0x04008596 RID: 34198
		[SerializeField]
		[HideInInspector]
		private bool _fontOnly;

		// Token: 0x04008597 RID: 34199
		[SerializeField]
		[HideInInspector]
		private TMP_FontAsset _fontOverride;

		// Token: 0x04008598 RID: 34200
		[SerializeField]
		[HideInInspector]
		private bool _originalAutoSize;

		// Token: 0x04008599 RID: 34201
		[SerializeField]
		[HideInInspector]
		private bool _originalWrap;

		// Token: 0x0400859A RID: 34202
		[SerializeField]
		[HideInInspector]
		private float _originalSize;

		// Token: 0x0400859B RID: 34203
		[SerializeField]
		[HideInInspector]
		private float _originalMin;

		// Token: 0x0400859C RID: 34204
		[SerializeField]
		[HideInInspector]
		private float _originalMax;
	}
}
