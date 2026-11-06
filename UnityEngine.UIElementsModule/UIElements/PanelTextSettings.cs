using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.TextCore.Text;

namespace UnityEngine.UIElements
{
	// Token: 0x02000369 RID: 873
	public class PanelTextSettings : TextSettings
	{
		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06001CF9 RID: 7417 RVA: 0x00070638 File Offset: 0x0006E838
		internal static PanelTextSettings defaultPanelTextSettings
		{
			get
			{
				bool flag = PanelTextSettings.s_DefaultPanelTextSettings == null;
				if (flag)
				{
					bool flag2 = PanelTextSettings.s_DefaultPanelTextSettings == null;
					if (flag2)
					{
						PanelTextSettings.s_DefaultPanelTextSettings = ScriptableObject.CreateInstance<PanelTextSettings>();
					}
				}
				return PanelTextSettings.s_DefaultPanelTextSettings;
			}
		}

		// Token: 0x06001CFA RID: 7418 RVA: 0x0007067C File Offset: 0x0006E87C
		internal static void UpdateLocalizationFontAsset()
		{
			string text = " - Linux";
			Dictionary<SystemLanguage, string> dictionary = new Dictionary<SystemLanguage, string>
			{
				{
					SystemLanguage.English,
					Path.Combine(UIElementsPackageUtility.EditorResourcesBasePath, "UIPackageResources/FontAssets/DynamicOSFontAssets/Localization/English" + text + ".asset")
				},
				{
					SystemLanguage.Japanese,
					Path.Combine(UIElementsPackageUtility.EditorResourcesBasePath, "UIPackageResources/FontAssets/DynamicOSFontAssets/Localization/Japanese" + text + ".asset")
				},
				{
					SystemLanguage.ChineseSimplified,
					Path.Combine(UIElementsPackageUtility.EditorResourcesBasePath, "UIPackageResources/FontAssets/DynamicOSFontAssets/Localization/ChineseSimplified" + text + ".asset")
				},
				{
					SystemLanguage.ChineseTraditional,
					Path.Combine(UIElementsPackageUtility.EditorResourcesBasePath, "UIPackageResources/FontAssets/DynamicOSFontAssets/Localization/ChineseTraditional" + text + ".asset")
				},
				{
					SystemLanguage.Korean,
					Path.Combine(UIElementsPackageUtility.EditorResourcesBasePath, "UIPackageResources/FontAssets/DynamicOSFontAssets/Localization/Korean" + text + ".asset")
				}
			};
			string text2 = Path.Combine(UIElementsPackageUtility.EditorResourcesBasePath, "UIPackageResources/FontAssets/DynamicOSFontAssets/GlobalFallback/GlobalFallback" + text + ".asset");
			FontAsset fontAsset = PanelTextSettings.EditorGUIUtilityLoad(dictionary[PanelTextSettings.GetCurrentLanguage()]) as FontAsset;
			FontAsset fontAsset2 = PanelTextSettings.EditorGUIUtilityLoad(text2) as FontAsset;
			PanelTextSettings.defaultPanelTextSettings.fallbackFontAssets[0] = fontAsset;
			PanelTextSettings.defaultPanelTextSettings.fallbackFontAssets[PanelTextSettings.defaultPanelTextSettings.fallbackFontAssets.Count - 1] = fontAsset2;
		}

		// Token: 0x06001CFB RID: 7419 RVA: 0x000707C8 File Offset: 0x0006E9C8
		internal FontAsset GetCachedFontAsset(Font font)
		{
			return base.GetCachedFontAssetInternal(font);
		}

		// Token: 0x04000C37 RID: 3127
		private static PanelTextSettings s_DefaultPanelTextSettings;

		// Token: 0x04000C38 RID: 3128
		internal static Func<string, Object> EditorGUIUtilityLoad;

		// Token: 0x04000C39 RID: 3129
		internal static Func<SystemLanguage> GetCurrentLanguage;

		// Token: 0x04000C3A RID: 3130
		internal static readonly string s_DefaultEditorPanelTextSettingPath = "UIPackageResources/Default Editor Text Settings.asset";
	}
}
