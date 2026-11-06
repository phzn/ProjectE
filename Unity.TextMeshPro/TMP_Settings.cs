using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000050 RID: 80
	[ExcludeFromPreset]
	[Serializable]
	public class TMP_Settings : ScriptableObject
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000366 RID: 870 RVA: 0x00024B7A File Offset: 0x00022D7A
		public static string version
		{
			get
			{
				return "1.4.0";
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00024B81 File Offset: 0x00022D81
		public static bool enableWordWrapping
		{
			get
			{
				return TMP_Settings.instance.m_enableWordWrapping;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00024B8D File Offset: 0x00022D8D
		public static bool enableKerning
		{
			get
			{
				return TMP_Settings.instance.m_enableKerning;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00024B99 File Offset: 0x00022D99
		public static bool enableExtraPadding
		{
			get
			{
				return TMP_Settings.instance.m_enableExtraPadding;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00024BA5 File Offset: 0x00022DA5
		public static bool enableTintAllSprites
		{
			get
			{
				return TMP_Settings.instance.m_enableTintAllSprites;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00024BB1 File Offset: 0x00022DB1
		public static bool enableParseEscapeCharacters
		{
			get
			{
				return TMP_Settings.instance.m_enableParseEscapeCharacters;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00024BBD File Offset: 0x00022DBD
		public static bool enableRaycastTarget
		{
			get
			{
				return TMP_Settings.instance.m_EnableRaycastTarget;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00024BC9 File Offset: 0x00022DC9
		public static bool getFontFeaturesAtRuntime
		{
			get
			{
				return TMP_Settings.instance.m_GetFontFeaturesAtRuntime;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600036E RID: 878 RVA: 0x00024BD5 File Offset: 0x00022DD5
		// (set) Token: 0x0600036F RID: 879 RVA: 0x00024BE1 File Offset: 0x00022DE1
		public static int missingGlyphCharacter
		{
			get
			{
				return TMP_Settings.instance.m_missingGlyphCharacter;
			}
			set
			{
				TMP_Settings.instance.m_missingGlyphCharacter = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00024BEE File Offset: 0x00022DEE
		public static bool warningsDisabled
		{
			get
			{
				return TMP_Settings.instance.m_warningsDisabled;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00024BFA File Offset: 0x00022DFA
		public static TMP_FontAsset defaultFontAsset
		{
			get
			{
				return TMP_Settings.instance.m_defaultFontAsset;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00024C06 File Offset: 0x00022E06
		public static string defaultFontAssetPath
		{
			get
			{
				return TMP_Settings.instance.m_defaultFontAssetPath;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00024C12 File Offset: 0x00022E12
		public static float defaultFontSize
		{
			get
			{
				return TMP_Settings.instance.m_defaultFontSize;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000374 RID: 884 RVA: 0x00024C1E File Offset: 0x00022E1E
		public static float defaultTextAutoSizingMinRatio
		{
			get
			{
				return TMP_Settings.instance.m_defaultAutoSizeMinRatio;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00024C2A File Offset: 0x00022E2A
		public static float defaultTextAutoSizingMaxRatio
		{
			get
			{
				return TMP_Settings.instance.m_defaultAutoSizeMaxRatio;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000376 RID: 886 RVA: 0x00024C36 File Offset: 0x00022E36
		public static Vector2 defaultTextMeshProTextContainerSize
		{
			get
			{
				return TMP_Settings.instance.m_defaultTextMeshProTextContainerSize;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00024C42 File Offset: 0x00022E42
		public static Vector2 defaultTextMeshProUITextContainerSize
		{
			get
			{
				return TMP_Settings.instance.m_defaultTextMeshProUITextContainerSize;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000378 RID: 888 RVA: 0x00024C4E File Offset: 0x00022E4E
		public static bool autoSizeTextContainer
		{
			get
			{
				return TMP_Settings.instance.m_autoSizeTextContainer;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000379 RID: 889 RVA: 0x00024C5A File Offset: 0x00022E5A
		// (set) Token: 0x0600037A RID: 890 RVA: 0x00024C66 File Offset: 0x00022E66
		public static bool isTextObjectScaleStatic
		{
			get
			{
				return TMP_Settings.instance.m_IsTextObjectScaleStatic;
			}
			set
			{
				TMP_Settings.instance.m_IsTextObjectScaleStatic = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600037B RID: 891 RVA: 0x00024C73 File Offset: 0x00022E73
		public static List<TMP_FontAsset> fallbackFontAssets
		{
			get
			{
				return TMP_Settings.instance.m_fallbackFontAssets;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00024C7F File Offset: 0x00022E7F
		public static bool matchMaterialPreset
		{
			get
			{
				return TMP_Settings.instance.m_matchMaterialPreset;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600037D RID: 893 RVA: 0x00024C8B File Offset: 0x00022E8B
		public static TMP_SpriteAsset defaultSpriteAsset
		{
			get
			{
				return TMP_Settings.instance.m_defaultSpriteAsset;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00024C97 File Offset: 0x00022E97
		public static string defaultSpriteAssetPath
		{
			get
			{
				return TMP_Settings.instance.m_defaultSpriteAssetPath;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600037F RID: 895 RVA: 0x00024CA3 File Offset: 0x00022EA3
		// (set) Token: 0x06000380 RID: 896 RVA: 0x00024CAF File Offset: 0x00022EAF
		public static bool enableEmojiSupport
		{
			get
			{
				return TMP_Settings.instance.m_enableEmojiSupport;
			}
			set
			{
				TMP_Settings.instance.m_enableEmojiSupport = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000381 RID: 897 RVA: 0x00024CBC File Offset: 0x00022EBC
		// (set) Token: 0x06000382 RID: 898 RVA: 0x00024CC8 File Offset: 0x00022EC8
		public static uint missingCharacterSpriteUnicode
		{
			get
			{
				return TMP_Settings.instance.m_MissingCharacterSpriteUnicode;
			}
			set
			{
				TMP_Settings.instance.m_MissingCharacterSpriteUnicode = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000383 RID: 899 RVA: 0x00024CD5 File Offset: 0x00022ED5
		public static string defaultColorGradientPresetsPath
		{
			get
			{
				return TMP_Settings.instance.m_defaultColorGradientPresetsPath;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00024CE1 File Offset: 0x00022EE1
		public static TMP_StyleSheet defaultStyleSheet
		{
			get
			{
				return TMP_Settings.instance.m_defaultStyleSheet;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000385 RID: 901 RVA: 0x00024CED File Offset: 0x00022EED
		public static string styleSheetsResourcePath
		{
			get
			{
				return TMP_Settings.instance.m_StyleSheetsResourcePath;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000386 RID: 902 RVA: 0x00024CF9 File Offset: 0x00022EF9
		public static TextAsset leadingCharacters
		{
			get
			{
				return TMP_Settings.instance.m_leadingCharacters;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000387 RID: 903 RVA: 0x00024D05 File Offset: 0x00022F05
		public static TextAsset followingCharacters
		{
			get
			{
				return TMP_Settings.instance.m_followingCharacters;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00024D11 File Offset: 0x00022F11
		public static TMP_Settings.LineBreakingTable linebreakingRules
		{
			get
			{
				if (TMP_Settings.instance.m_linebreakingRules == null)
				{
					TMP_Settings.LoadLinebreakingRules();
				}
				return TMP_Settings.instance.m_linebreakingRules;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000389 RID: 905 RVA: 0x00024D2E File Offset: 0x00022F2E
		// (set) Token: 0x0600038A RID: 906 RVA: 0x00024D3A File Offset: 0x00022F3A
		public static bool useModernHangulLineBreakingRules
		{
			get
			{
				return TMP_Settings.instance.m_UseModernHangulLineBreakingRules;
			}
			set
			{
				TMP_Settings.instance.m_UseModernHangulLineBreakingRules = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600038B RID: 907 RVA: 0x00024D47 File Offset: 0x00022F47
		public static TMP_Settings instance
		{
			get
			{
				if (TMP_Settings.s_Instance == null)
				{
					TMP_Settings.s_Instance = Resources.Load<TMP_Settings>("TMP Settings");
				}
				return TMP_Settings.s_Instance;
			}
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00024D6C File Offset: 0x00022F6C
		public static TMP_Settings LoadDefaultSettings()
		{
			if (TMP_Settings.s_Instance == null)
			{
				TMP_Settings tmp_Settings = Resources.Load<TMP_Settings>("TMP Settings");
				if (tmp_Settings != null)
				{
					TMP_Settings.s_Instance = tmp_Settings;
				}
			}
			return TMP_Settings.s_Instance;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00024DA5 File Offset: 0x00022FA5
		public static TMP_Settings GetSettings()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00024DBB File Offset: 0x00022FBB
		public static TMP_FontAsset GetFontAsset()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultFontAsset;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00024DD6 File Offset: 0x00022FD6
		public static TMP_SpriteAsset GetSpriteAsset()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultSpriteAsset;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00024DF1 File Offset: 0x00022FF1
		public static TMP_StyleSheet GetStyleSheet()
		{
			if (TMP_Settings.instance == null)
			{
				return null;
			}
			return TMP_Settings.instance.m_defaultStyleSheet;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00024E0C File Offset: 0x0002300C
		public static void LoadLinebreakingRules()
		{
			if (TMP_Settings.instance == null)
			{
				return;
			}
			if (TMP_Settings.s_Instance.m_linebreakingRules == null)
			{
				TMP_Settings.s_Instance.m_linebreakingRules = new TMP_Settings.LineBreakingTable();
			}
			TMP_Settings.s_Instance.m_linebreakingRules.leadingCharacters = TMP_Settings.GetCharacters(TMP_Settings.s_Instance.m_leadingCharacters);
			TMP_Settings.s_Instance.m_linebreakingRules.followingCharacters = TMP_Settings.GetCharacters(TMP_Settings.s_Instance.m_followingCharacters);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00024E80 File Offset: 0x00023080
		private static Dictionary<int, char> GetCharacters(TextAsset file)
		{
			Dictionary<int, char> dictionary = new Dictionary<int, char>();
			foreach (char c in file.text)
			{
				if (!dictionary.ContainsKey((int)c))
				{
					dictionary.Add((int)c, c);
				}
			}
			return dictionary;
		}

		// Token: 0x0400032F RID: 815
		private static TMP_Settings s_Instance;

		// Token: 0x04000330 RID: 816
		[SerializeField]
		private bool m_enableWordWrapping;

		// Token: 0x04000331 RID: 817
		[SerializeField]
		private bool m_enableKerning;

		// Token: 0x04000332 RID: 818
		[SerializeField]
		private bool m_enableExtraPadding;

		// Token: 0x04000333 RID: 819
		[SerializeField]
		private bool m_enableTintAllSprites;

		// Token: 0x04000334 RID: 820
		[SerializeField]
		private bool m_enableParseEscapeCharacters;

		// Token: 0x04000335 RID: 821
		[SerializeField]
		private bool m_EnableRaycastTarget = true;

		// Token: 0x04000336 RID: 822
		[SerializeField]
		private bool m_GetFontFeaturesAtRuntime = true;

		// Token: 0x04000337 RID: 823
		[SerializeField]
		private int m_missingGlyphCharacter;

		// Token: 0x04000338 RID: 824
		[SerializeField]
		private bool m_warningsDisabled;

		// Token: 0x04000339 RID: 825
		[SerializeField]
		private TMP_FontAsset m_defaultFontAsset;

		// Token: 0x0400033A RID: 826
		[SerializeField]
		private string m_defaultFontAssetPath;

		// Token: 0x0400033B RID: 827
		[SerializeField]
		private float m_defaultFontSize;

		// Token: 0x0400033C RID: 828
		[SerializeField]
		private float m_defaultAutoSizeMinRatio;

		// Token: 0x0400033D RID: 829
		[SerializeField]
		private float m_defaultAutoSizeMaxRatio;

		// Token: 0x0400033E RID: 830
		[SerializeField]
		private Vector2 m_defaultTextMeshProTextContainerSize;

		// Token: 0x0400033F RID: 831
		[SerializeField]
		private Vector2 m_defaultTextMeshProUITextContainerSize;

		// Token: 0x04000340 RID: 832
		[SerializeField]
		private bool m_autoSizeTextContainer;

		// Token: 0x04000341 RID: 833
		[SerializeField]
		private bool m_IsTextObjectScaleStatic;

		// Token: 0x04000342 RID: 834
		[SerializeField]
		private List<TMP_FontAsset> m_fallbackFontAssets;

		// Token: 0x04000343 RID: 835
		[SerializeField]
		private bool m_matchMaterialPreset;

		// Token: 0x04000344 RID: 836
		[SerializeField]
		private TMP_SpriteAsset m_defaultSpriteAsset;

		// Token: 0x04000345 RID: 837
		[SerializeField]
		private string m_defaultSpriteAssetPath;

		// Token: 0x04000346 RID: 838
		[SerializeField]
		private bool m_enableEmojiSupport;

		// Token: 0x04000347 RID: 839
		[SerializeField]
		private uint m_MissingCharacterSpriteUnicode;

		// Token: 0x04000348 RID: 840
		[SerializeField]
		private string m_defaultColorGradientPresetsPath;

		// Token: 0x04000349 RID: 841
		[SerializeField]
		private TMP_StyleSheet m_defaultStyleSheet;

		// Token: 0x0400034A RID: 842
		[SerializeField]
		private string m_StyleSheetsResourcePath;

		// Token: 0x0400034B RID: 843
		[SerializeField]
		private TextAsset m_leadingCharacters;

		// Token: 0x0400034C RID: 844
		[SerializeField]
		private TextAsset m_followingCharacters;

		// Token: 0x0400034D RID: 845
		[SerializeField]
		private TMP_Settings.LineBreakingTable m_linebreakingRules;

		// Token: 0x0400034E RID: 846
		[SerializeField]
		private bool m_UseModernHangulLineBreakingRules;

		// Token: 0x020000A0 RID: 160
		public class LineBreakingTable
		{
			// Token: 0x040005EA RID: 1514
			public Dictionary<int, char> leadingCharacters;

			// Token: 0x040005EB RID: 1515
			public Dictionary<int, char> followingCharacters;
		}
	}
}
