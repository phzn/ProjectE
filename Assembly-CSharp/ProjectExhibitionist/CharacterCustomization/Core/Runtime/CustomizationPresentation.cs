using System;
using System.Globalization;
using Features.Localization;
using ProjectExhibitionist.UI;
using TMPro;

namespace ProjectExhibitionist.CharacterCustomization.Core.Runtime
{
	// Token: 0x02000AF9 RID: 2809
	public static class CustomizationPresentation
	{
		// Token: 0x06004F9B RID: 20379 RVA: 0x001EDA3C File Offset: 0x001EBC3C
		public static void ShowError(string message, string title, TMP_Text localText = null)
		{
			string text = null;
			object obj = null;
			if (message != null && message.StartsWith("Could not complete the NPC save. ", StringComparison.Ordinal))
			{
				text = "Could not complete the NPC save. {0}";
				obj = message.Substring("Could not complete the NPC save. ".Length);
			}
			else if (message != null && message.StartsWith("The NPC look was written, but its preset details could not be updated. ", StringComparison.Ordinal))
			{
				text = "The NPC look was written, but its preset details could not be updated. {0}";
				obj = new CustomizationPresentation.AuthoredErrorArgument(message.Substring("The NPC look was written, but its preset details could not be updated. ".Length));
			}
			if (text == null)
			{
				ToastService.Show(message, title, null, -1f, "cielos_apps");
				LocalizedGameText localizedGameText = LocalizedGameText.BindSource(localText, "cielos_apps", message, null);
				if (localizedGameText == null)
				{
					return;
				}
				localizedGameText.FitTranslationToRectangle(false);
				return;
			}
			else
			{
				ToastService.ShowFormat("cielos_apps", text, new object[] { obj }, title, null, -1f);
				LocalizedGameText localizedGameText2 = LocalizedGameText.BindFormat(localText, "cielos_apps", text, new object[] { obj });
				if (localizedGameText2 == null)
				{
					return;
				}
				localizedGameText2.FitTranslationToRectangle(false);
				return;
			}
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x001EDB10 File Offset: 0x001EBD10
		public static string ColorLabel(string source)
		{
			if (GameLocalization.CurrentCode == "en" || string.IsNullOrEmpty(source))
			{
				return source;
			}
			string source2 = GameLocalization.GetSource("cielos_apps", source);
			if (source2 != source)
			{
				return source2;
			}
			int num = source.IndexOf(" · ", StringComparison.Ordinal);
			if (num >= 0)
			{
				string text = source.Substring(0, num);
				string text2 = source.Substring(num + 3);
				string text3 = CustomizationPresentation.ColorChannelScope(text, text2);
				return GameLocalization.FormatSource("cielos_apps", "{0} · {1}", new object[]
				{
					GameLocalization.GetSource("cielos_apps", text),
					(text3 == "cielos_apps") ? CustomizationPresentation.ColorLabel(text2) : GameLocalization.GetSource(text3, text2)
				});
			}
			foreach (string text4 in new string[] { "Variant ", "Style ", "Preset ", "Item ", "Color " })
			{
				int num2;
				if (source.StartsWith(text4, StringComparison.Ordinal) && int.TryParse(source.Substring(text4.Length), NumberStyles.None, CultureInfo.InvariantCulture, out num2))
				{
					return GameLocalization.FormatSource("cielos_apps", text4 + "{0}", new object[] { num2 });
				}
			}
			return source;
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x001EDC5A File Offset: 0x001EBE5A
		private static string ColorChannelScope(string outfitLabel, string channelName)
		{
			if (channelName == "Arms")
			{
				if (outfitLabel == "Round Glasses")
				{
					return "cielos_glasses";
				}
				if (outfitLabel == "Thick Hensley")
				{
					return "cielos_clothing_channels";
				}
			}
			return "cielos_apps";
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x001EDC94 File Offset: 0x001EBE94
		public static void BindColorLabel(TMP_Text text, string source, bool upper = false)
		{
			LocalizedGameText localizedGameText = LocalizedGameText.BindComputed(text, delegate
			{
				if (!upper)
				{
					return CustomizationPresentation.ColorLabel(source);
				}
				return CustomizationPresentation.ColorLabel(source).ToUpperInvariant();
			});
			if (localizedGameText == null)
			{
				return;
			}
			localizedGameText.FitTranslationToRectangle(false);
		}

		// Token: 0x02000AFA RID: 2810
		private sealed class AuthoredErrorArgument
		{
			// Token: 0x06004F9F RID: 20383 RVA: 0x001EDCD2 File Offset: 0x001EBED2
			public AuthoredErrorArgument(string source)
			{
				this._source = source;
			}

			// Token: 0x06004FA0 RID: 20384 RVA: 0x001EDCE1 File Offset: 0x001EBEE1
			public override string ToString()
			{
				return GameLocalization.GetSource("cielos_apps", this._source);
			}

			// Token: 0x04005096 RID: 20630
			private readonly string _source;
		}
	}
}
