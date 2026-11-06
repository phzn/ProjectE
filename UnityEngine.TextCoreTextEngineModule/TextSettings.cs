using System;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.TextCore.LowLevel;

namespace UnityEngine.TextCore.Text
{
	// Token: 0x0200004B RID: 75
	[ExcludeFromPreset]
	[ExcludeFromObjectFactory]
	[Serializable]
	public class TextSettings : ScriptableObject
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000216 RID: 534 RVA: 0x00022DF7 File Offset: 0x00020FF7
		// (set) Token: 0x06000217 RID: 535 RVA: 0x00022DFF File Offset: 0x00020FFF
		public string version
		{
			get
			{
				return this.m_Version;
			}
			internal set
			{
				this.m_Version = value;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000218 RID: 536 RVA: 0x00022E08 File Offset: 0x00021008
		// (set) Token: 0x06000219 RID: 537 RVA: 0x00022E10 File Offset: 0x00021010
		public FontAsset defaultFontAsset
		{
			get
			{
				return this.m_DefaultFontAsset;
			}
			set
			{
				this.m_DefaultFontAsset = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00022E19 File Offset: 0x00021019
		// (set) Token: 0x0600021B RID: 539 RVA: 0x00022E21 File Offset: 0x00021021
		public string defaultFontAssetPath
		{
			get
			{
				return this.m_DefaultFontAssetPath;
			}
			set
			{
				this.m_DefaultFontAssetPath = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00022E2A File Offset: 0x0002102A
		// (set) Token: 0x0600021D RID: 541 RVA: 0x00022E32 File Offset: 0x00021032
		public List<FontAsset> fallbackFontAssets
		{
			get
			{
				return this.m_FallbackFontAssets;
			}
			set
			{
				this.m_FallbackFontAssets = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00022E3B File Offset: 0x0002103B
		// (set) Token: 0x0600021F RID: 543 RVA: 0x00022E43 File Offset: 0x00021043
		public bool matchMaterialPreset
		{
			get
			{
				return this.m_MatchMaterialPreset;
			}
			set
			{
				this.m_MatchMaterialPreset = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00022E4C File Offset: 0x0002104C
		// (set) Token: 0x06000221 RID: 545 RVA: 0x00022E54 File Offset: 0x00021054
		public int missingCharacterUnicode
		{
			get
			{
				return this.m_MissingCharacterUnicode;
			}
			set
			{
				this.m_MissingCharacterUnicode = value;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00022E5D File Offset: 0x0002105D
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00022E65 File Offset: 0x00021065
		public bool clearDynamicDataOnBuild
		{
			get
			{
				return this.m_ClearDynamicDataOnBuild;
			}
			set
			{
				this.m_ClearDynamicDataOnBuild = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00022E6E File Offset: 0x0002106E
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00022E76 File Offset: 0x00021076
		public SpriteAsset defaultSpriteAsset
		{
			get
			{
				return this.m_DefaultSpriteAsset;
			}
			set
			{
				this.m_DefaultSpriteAsset = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000226 RID: 550 RVA: 0x00022E7F File Offset: 0x0002107F
		// (set) Token: 0x06000227 RID: 551 RVA: 0x00022E87 File Offset: 0x00021087
		public string defaultSpriteAssetPath
		{
			get
			{
				return this.m_DefaultSpriteAssetPath;
			}
			set
			{
				this.m_DefaultSpriteAssetPath = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000228 RID: 552 RVA: 0x00022E90 File Offset: 0x00021090
		// (set) Token: 0x06000229 RID: 553 RVA: 0x00022E98 File Offset: 0x00021098
		public List<SpriteAsset> fallbackSpriteAssets
		{
			get
			{
				return this.m_FallbackSpriteAssets;
			}
			set
			{
				this.m_FallbackSpriteAssets = value;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600022A RID: 554 RVA: 0x00022EA1 File Offset: 0x000210A1
		// (set) Token: 0x0600022B RID: 555 RVA: 0x00022EA9 File Offset: 0x000210A9
		public uint missingSpriteCharacterUnicode
		{
			get
			{
				return this.m_MissingSpriteCharacterUnicode;
			}
			set
			{
				this.m_MissingSpriteCharacterUnicode = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600022C RID: 556 RVA: 0x00022EB2 File Offset: 0x000210B2
		// (set) Token: 0x0600022D RID: 557 RVA: 0x00022EBA File Offset: 0x000210BA
		public TextStyleSheet defaultStyleSheet
		{
			get
			{
				return this.m_DefaultStyleSheet;
			}
			set
			{
				this.m_DefaultStyleSheet = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600022E RID: 558 RVA: 0x00022EC3 File Offset: 0x000210C3
		// (set) Token: 0x0600022F RID: 559 RVA: 0x00022ECB File Offset: 0x000210CB
		public string styleSheetsResourcePath
		{
			get
			{
				return this.m_StyleSheetsResourcePath;
			}
			set
			{
				this.m_StyleSheetsResourcePath = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000230 RID: 560 RVA: 0x00022ED4 File Offset: 0x000210D4
		// (set) Token: 0x06000231 RID: 561 RVA: 0x00022EDC File Offset: 0x000210DC
		public string defaultColorGradientPresetsPath
		{
			get
			{
				return this.m_DefaultColorGradientPresetsPath;
			}
			set
			{
				this.m_DefaultColorGradientPresetsPath = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000232 RID: 562 RVA: 0x00022EE8 File Offset: 0x000210E8
		// (set) Token: 0x06000233 RID: 563 RVA: 0x00022F26 File Offset: 0x00021126
		public UnicodeLineBreakingRules lineBreakingRules
		{
			get
			{
				bool flag = this.m_UnicodeLineBreakingRules == null;
				if (flag)
				{
					this.m_UnicodeLineBreakingRules = new UnicodeLineBreakingRules();
					this.m_UnicodeLineBreakingRules.LoadLineBreakingRules();
				}
				return this.m_UnicodeLineBreakingRules;
			}
			set
			{
				this.m_UnicodeLineBreakingRules = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000234 RID: 564 RVA: 0x00022F30 File Offset: 0x00021130
		// (set) Token: 0x06000235 RID: 565 RVA: 0x00022F48 File Offset: 0x00021148
		public bool useModernHangulLineBreakingRules
		{
			get
			{
				return this.m_UseModernHangulLineBreakingRules;
			}
			set
			{
				this.m_UseModernHangulLineBreakingRules = value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00022F52 File Offset: 0x00021152
		// (set) Token: 0x06000237 RID: 567 RVA: 0x00022F5A File Offset: 0x0002115A
		public bool displayWarnings
		{
			get
			{
				return this.m_DisplayWarnings;
			}
			set
			{
				this.m_DisplayWarnings = value;
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00022F63 File Offset: 0x00021163
		private void OnEnable()
		{
			this.lineBreakingRules.LoadLineBreakingRules();
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00022F74 File Offset: 0x00021174
		protected void InitializeFontReferenceLookup()
		{
			bool flag = this.m_FontReferences == null;
			if (flag)
			{
				this.m_FontReferences = new List<TextSettings.FontReferenceMap>();
			}
			for (int i = 0; i < this.m_FontReferences.Count; i++)
			{
				TextSettings.FontReferenceMap fontReferenceMap = this.m_FontReferences[i];
				bool flag2 = fontReferenceMap.font == null || fontReferenceMap.fontAsset == null;
				if (flag2)
				{
					Debug.Log("Deleting invalid font reference.");
					this.m_FontReferences.RemoveAt(i);
					i--;
				}
				else
				{
					int instanceID = fontReferenceMap.font.GetInstanceID();
					bool flag3 = !this.m_FontLookup.ContainsKey(instanceID);
					if (flag3)
					{
						this.m_FontLookup.Add(instanceID, fontReferenceMap.fontAsset);
					}
				}
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00023044 File Offset: 0x00021244
		protected FontAsset GetCachedFontAssetInternal(Font font)
		{
			bool flag = this.m_FontLookup == null;
			if (flag)
			{
				this.m_FontLookup = new Dictionary<int, FontAsset>();
				this.InitializeFontReferenceLookup();
			}
			int instanceID = font.GetInstanceID();
			bool flag2 = this.m_FontLookup.ContainsKey(instanceID);
			FontAsset fontAsset;
			if (flag2)
			{
				fontAsset = this.m_FontLookup[instanceID];
			}
			else
			{
				bool flag3 = font.name == "System Normal";
				FontAsset fontAsset2;
				if (flag3)
				{
					fontAsset2 = FontAsset.CreateFontAsset("Lucida Grande", "Regular", 90);
				}
				else
				{
					fontAsset2 = FontAsset.CreateFontAsset(font, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024, AtlasPopulationMode.Dynamic, true);
				}
				bool flag4 = fontAsset2 != null;
				if (flag4)
				{
					fontAsset2.hideFlags = HideFlags.DontSave;
					fontAsset2.atlasTextures[0].hideFlags = HideFlags.DontSave;
					fontAsset2.material.hideFlags = HideFlags.DontSave;
					fontAsset2.isMultiAtlasTexturesEnabled = true;
					this.m_FontReferences.Add(new TextSettings.FontReferenceMap(font, fontAsset2));
					this.m_FontLookup.Add(instanceID, fontAsset2);
				}
				fontAsset = fontAsset2;
			}
			return fontAsset;
		}

		// Token: 0x040003A4 RID: 932
		[SerializeField]
		protected string m_Version;

		// Token: 0x040003A5 RID: 933
		[FormerlySerializedAs("m_defaultFontAsset")]
		[SerializeField]
		protected FontAsset m_DefaultFontAsset;

		// Token: 0x040003A6 RID: 934
		[FormerlySerializedAs("m_defaultFontAssetPath")]
		[SerializeField]
		protected string m_DefaultFontAssetPath = "Fonts & Materials/";

		// Token: 0x040003A7 RID: 935
		[FormerlySerializedAs("m_fallbackFontAssets")]
		[SerializeField]
		protected List<FontAsset> m_FallbackFontAssets;

		// Token: 0x040003A8 RID: 936
		[SerializeField]
		[FormerlySerializedAs("m_matchMaterialPreset")]
		protected bool m_MatchMaterialPreset;

		// Token: 0x040003A9 RID: 937
		[FormerlySerializedAs("m_missingGlyphCharacter")]
		[SerializeField]
		protected int m_MissingCharacterUnicode;

		// Token: 0x040003AA RID: 938
		[SerializeField]
		protected bool m_ClearDynamicDataOnBuild = true;

		// Token: 0x040003AB RID: 939
		[FormerlySerializedAs("m_defaultSpriteAsset")]
		[SerializeField]
		protected SpriteAsset m_DefaultSpriteAsset;

		// Token: 0x040003AC RID: 940
		[FormerlySerializedAs("m_defaultSpriteAssetPath")]
		[SerializeField]
		protected string m_DefaultSpriteAssetPath = "Sprite Assets/";

		// Token: 0x040003AD RID: 941
		[SerializeField]
		protected List<SpriteAsset> m_FallbackSpriteAssets;

		// Token: 0x040003AE RID: 942
		[SerializeField]
		protected uint m_MissingSpriteCharacterUnicode;

		// Token: 0x040003AF RID: 943
		[FormerlySerializedAs("m_defaultStyleSheet")]
		[SerializeField]
		protected TextStyleSheet m_DefaultStyleSheet;

		// Token: 0x040003B0 RID: 944
		[SerializeField]
		protected string m_StyleSheetsResourcePath = "Text Style Sheets/";

		// Token: 0x040003B1 RID: 945
		[FormerlySerializedAs("m_defaultColorGradientPresetsPath")]
		[SerializeField]
		protected string m_DefaultColorGradientPresetsPath = "Text Color Gradients/";

		// Token: 0x040003B2 RID: 946
		[SerializeField]
		protected UnicodeLineBreakingRules m_UnicodeLineBreakingRules;

		// Token: 0x040003B3 RID: 947
		[SerializeField]
		private bool m_UseModernHangulLineBreakingRules;

		// Token: 0x040003B4 RID: 948
		[FormerlySerializedAs("m_warningsDisabled")]
		[SerializeField]
		protected bool m_DisplayWarnings = false;

		// Token: 0x040003B5 RID: 949
		internal Dictionary<int, FontAsset> m_FontLookup;

		// Token: 0x040003B6 RID: 950
		private List<TextSettings.FontReferenceMap> m_FontReferences = new List<TextSettings.FontReferenceMap>();

		// Token: 0x0200004C RID: 76
		[Serializable]
		private struct FontReferenceMap
		{
			// Token: 0x0600023C RID: 572 RVA: 0x000231A9 File Offset: 0x000213A9
			public FontReferenceMap(Font font, FontAsset fontAsset)
			{
				this.font = font;
				this.fontAsset = fontAsset;
			}

			// Token: 0x040003B7 RID: 951
			public Font font;

			// Token: 0x040003B8 RID: 952
			public FontAsset fontAsset;
		}
	}
}
