using System;
using System.Collections.Generic;
using Features.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.MainMenu
{
	// Token: 0x02001040 RID: 4160
	public sealed class MainMenuArrowSelector : MonoBehaviour
	{
		// Token: 0x17001453 RID: 5203
		// (get) Token: 0x06007E55 RID: 32341 RVA: 0x0033ACEA File Offset: 0x00338EEA
		public int Index
		{
			get
			{
				return this._index;
			}
		}

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x06007E56 RID: 32342 RVA: 0x0033ACF4 File Offset: 0x00338EF4
		// (remove) Token: 0x06007E57 RID: 32343 RVA: 0x0033AD2C File Offset: 0x00338F2C
		public event Action<int> OnUserChangedIndex;

		// Token: 0x06007E58 RID: 32344 RVA: 0x0033AD61 File Offset: 0x00338F61
		public void Configure(TMP_Text label, Button previous, Button next)
		{
			this.Unwire();
			this.valueLabel = label;
			this.prevButton = previous;
			this.nextButton = next;
			this.Wire();
		}

		// Token: 0x06007E59 RID: 32345 RVA: 0x0033AD84 File Offset: 0x00338F84
		private void Awake()
		{
			this.Wire();
		}

		// Token: 0x06007E5A RID: 32346 RVA: 0x0033AD8C File Offset: 0x00338F8C
		private void OnDestroy()
		{
			this.Unwire();
		}

		// Token: 0x06007E5B RID: 32347 RVA: 0x0033AD94 File Offset: 0x00338F94
		private void Wire()
		{
			if (this._wired || this.prevButton == null || this.nextButton == null)
			{
				return;
			}
			this.prevButton.onClick.AddListener(new UnityAction(this.Prev));
			this.nextButton.onClick.AddListener(new UnityAction(this.Next));
			this._wired = true;
		}

		// Token: 0x06007E5C RID: 32348 RVA: 0x0033AE08 File Offset: 0x00339008
		private void Unwire()
		{
			if (!this._wired)
			{
				return;
			}
			if (this.prevButton != null)
			{
				this.prevButton.onClick.RemoveListener(new UnityAction(this.Prev));
			}
			if (this.nextButton != null)
			{
				this.nextButton.onClick.RemoveListener(new UnityAction(this.Next));
			}
			this._wired = false;
		}

		// Token: 0x06007E5D RID: 32349 RVA: 0x0033AE7C File Offset: 0x0033907C
		public void SetOptions(IEnumerable<string> options, string localizationScope = "settings")
		{
			this._localizationScope = localizationScope;
			this._optionFonts = null;
			this._options.Clear();
			if (options != null)
			{
				this._options.AddRange(options);
			}
			this._index = Mathf.Clamp(this._index, 0, Mathf.Max(0, this._options.Count - 1));
			this.RefreshLabel();
		}

		// Token: 0x06007E5E RID: 32350 RVA: 0x0033AEDC File Offset: 0x003390DC
		public void SetLanguageOptions(GameLocalizationConfig.Language[] languages)
		{
			if (languages == null)
			{
				this._optionFonts = null;
				this.RefreshLabel();
				return;
			}
			this._optionFonts = new TMP_FontAsset[languages.Length];
			for (int i = 0; i < languages.Length; i++)
			{
				this._optionFonts[i] = ((languages[i] != null) ? languages[i].font : null);
			}
			this.RefreshLabel();
		}

		// Token: 0x06007E5F RID: 32351 RVA: 0x0033AF34 File Offset: 0x00339134
		public void SetIndexWithoutNotify(int index)
		{
			this._index = Mathf.Clamp(index, 0, Mathf.Max(0, this._options.Count - 1));
			this.RefreshLabel();
		}

		// Token: 0x06007E60 RID: 32352 RVA: 0x0033AF5C File Offset: 0x0033915C
		public void Prev()
		{
			this.Step(-1);
		}

		// Token: 0x06007E61 RID: 32353 RVA: 0x0033AF65 File Offset: 0x00339165
		public void Next()
		{
			this.Step(1);
		}

		// Token: 0x06007E62 RID: 32354 RVA: 0x0033AF70 File Offset: 0x00339170
		private void Step(int delta)
		{
			if (this._options.Count == 0)
			{
				return;
			}
			this._index = (this._index + delta + this._options.Count) % this._options.Count;
			this.RefreshLabel();
			Action<int> onUserChangedIndex = this.OnUserChangedIndex;
			if (onUserChangedIndex == null)
			{
				return;
			}
			onUserChangedIndex(this._index);
		}

		// Token: 0x06007E63 RID: 32355 RVA: 0x0033AFD0 File Offset: 0x003391D0
		private void RefreshLabel()
		{
			if (this.valueLabel != null)
			{
				string text = ((this._options.Count > 0) ? this._options[this._index] : "-");
				TMP_FontAsset tmp_FontAsset = ((this._optionFonts != null && this._index < this._optionFonts.Length) ? this._optionFonts[this._index] : null);
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(this.valueLabel, this._localizationScope, text, null);
				if (localizedGameText != null)
				{
					localizedGameText.SetFontOverride(tmp_FontAsset);
				}
			}
		}

		// Token: 0x040081EB RID: 33259
		[SerializeField]
		private TMP_Text valueLabel;

		// Token: 0x040081EC RID: 33260
		[SerializeField]
		private Button prevButton;

		// Token: 0x040081ED RID: 33261
		[SerializeField]
		private Button nextButton;

		// Token: 0x040081EE RID: 33262
		private readonly List<string> _options = new List<string>();

		// Token: 0x040081EF RID: 33263
		private TMP_FontAsset[] _optionFonts;

		// Token: 0x040081F0 RID: 33264
		private int _index;

		// Token: 0x040081F1 RID: 33265
		private bool _wired;

		// Token: 0x040081F2 RID: 33266
		private string _localizationScope = "settings";
	}
}
