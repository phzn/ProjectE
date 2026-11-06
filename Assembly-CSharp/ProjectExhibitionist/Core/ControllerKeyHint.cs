using System;
using Features.Localization;
using Invector.vCharacterController;
using ProjectExhibitionist.Interactions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectExhibitionist.Core
{
	// Token: 0x02000A4F RID: 2639
	[DisallowMultipleComponent]
	public sealed class ControllerKeyHint : MonoBehaviour
	{
		// Token: 0x06004B1D RID: 19229 RVA: 0x001D3E92 File Offset: 0x001D2092
		public static void Bind(TMP_Text text, GenericInput input)
		{
			if (text == null || input == null)
			{
				return;
			}
			ControllerKeyHint.Bind(text, input.keyboard, null, false);
			ControllerKeyHint component = text.GetComponent<ControllerKeyHint>();
			component._input = input;
			component._binding.Refresh();
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x001D3EC8 File Offset: 0x001D20C8
		public static void BindMatching(GameObject root, GenericInput input)
		{
			if (root == null || input == null)
			{
				return;
			}
			foreach (TMP_Text tmp_Text in root.GetComponentsInChildren<TMP_Text>(true))
			{
				ControllerKeyHint component = tmp_Text.GetComponent<ControllerKeyHint>();
				string text = ((component != null) ? component._source : tmp_Text.text);
				if (tmp_Text.transform.parent != null && tmp_Text.transform.parent.name == "Keyboard Input" && string.Equals(text, input.keyboard, StringComparison.OrdinalIgnoreCase))
				{
					ControllerKeyHint.Bind(tmp_Text, input);
				}
			}
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x001D3F64 File Offset: 0x001D2164
		public static void Bind(TMP_Text text, string source, InteractionConfigSO config = null, bool translateUnmapped = false)
		{
			if (text == null)
			{
				return;
			}
			ControllerKeyHint controllerKeyHint = text.GetComponent<ControllerKeyHint>();
			if (controllerKeyHint == null)
			{
				controllerKeyHint = text.gameObject.AddComponent<ControllerKeyHint>();
			}
			controllerKeyHint._text = text;
			if (!controllerKeyHint._capturedStyle)
			{
				controllerKeyHint._keyboardRichText = text.richText;
				controllerKeyHint._capturedStyle = true;
			}
			controllerKeyHint._input = null;
			controllerKeyHint._source = source;
			controllerKeyHint._config = config;
			controllerKeyHint._translateUnmapped = translateUnmapped;
			controllerKeyHint._binding = LocalizedGameText.BindComputed(text, new Func<string>(controllerKeyHint.Resolve));
			controllerKeyHint._lastController = ControllerInput.IsActive;
			controllerKeyHint._lastVersion = ControllerContextBindings.LabelVersion;
			controllerKeyHint._glyphVersion = ControllerGlyphs.Version;
		}

		// Token: 0x06004B20 RID: 19232 RVA: 0x001D4010 File Offset: 0x001D2210
		private string Resolve()
		{
			string text = ((this._input != null) ? (ControllerInput.IsActive ? this._input.buttonName : this._input.keyboard) : ControllerContextBindings.KeyLabel(this._config, this._source, this._translateUnmapped));
			if (ControllerInput.IsActive)
			{
				ControllerButton controllerButton;
				if (this._input != null && ControllerBindings.TryLegacyButton(text, out controllerButton))
				{
					text = ControllerBindings.Label(controllerButton);
				}
				if (text != null && text.StartsWith("Hold ", StringComparison.Ordinal))
				{
					text = InteractionLocalization.KeyName(text);
				}
				if (this._text != null)
				{
					this._text.richText = true;
				}
				return ControllerGlyphs.Format(text);
			}
			if (this._text != null)
			{
				this._text.richText = this._keyboardRichText;
			}
			return InteractionLocalization.KeyName(text);
		}

		// Token: 0x06004B21 RID: 19233 RVA: 0x001D40DC File Offset: 0x001D22DC
		private void LateUpdate()
		{
			bool isActive = ControllerInput.IsActive;
			int labelVersion = ControllerContextBindings.LabelVersion;
			int version = ControllerGlyphs.Version;
			if (this._lastController == isActive && this._lastVersion == labelVersion && this._glyphVersion == version)
			{
				return;
			}
			this._lastController = isActive;
			this._lastVersion = labelVersion;
			this._glyphVersion = version;
			if (this._text != null && this._binding != null)
			{
				this._binding.Refresh();
				RectTransform rectTransform = base.transform.parent as RectTransform;
				if (rectTransform != null)
				{
					LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
				}
			}
		}

		// Token: 0x04004D06 RID: 19718
		private TMP_Text _text;

		// Token: 0x04004D07 RID: 19719
		private LocalizedGameText _binding;

		// Token: 0x04004D08 RID: 19720
		private string _source;

		// Token: 0x04004D09 RID: 19721
		private InteractionConfigSO _config;

		// Token: 0x04004D0A RID: 19722
		private bool _translateUnmapped;

		// Token: 0x04004D0B RID: 19723
		private bool _lastController;

		// Token: 0x04004D0C RID: 19724
		private int _lastVersion;

		// Token: 0x04004D0D RID: 19725
		private int _glyphVersion;

		// Token: 0x04004D0E RID: 19726
		private bool _capturedStyle;

		// Token: 0x04004D0F RID: 19727
		private bool _keyboardRichText;

		// Token: 0x04004D10 RID: 19728
		private GenericInput _input;
	}
}
