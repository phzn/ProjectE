using System;
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x02000356 RID: 854
	[HelpURL("UIE-USS")]
	[Serializable]
	public class StyleSheet : ScriptableObject
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x0006EF3C File Offset: 0x0006D13C
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x0006EF54 File Offset: 0x0006D154
		public bool importedWithErrors
		{
			get
			{
				return this.m_ImportedWithErrors;
			}
			internal set
			{
				this.m_ImportedWithErrors = value;
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x0006EF60 File Offset: 0x0006D160
		// (set) Token: 0x06001C96 RID: 7318 RVA: 0x0006EF78 File Offset: 0x0006D178
		public bool importedWithWarnings
		{
			get
			{
				return this.m_ImportedWithWarnings;
			}
			internal set
			{
				this.m_ImportedWithWarnings = value;
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x0006EF84 File Offset: 0x0006D184
		// (set) Token: 0x06001C98 RID: 7320 RVA: 0x0006EF9C File Offset: 0x0006D19C
		internal StyleRule[] rules
		{
			get
			{
				return this.m_Rules;
			}
			set
			{
				this.m_Rules = value;
				this.SetupReferences();
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x06001C99 RID: 7321 RVA: 0x0006EFB0 File Offset: 0x0006D1B0
		// (set) Token: 0x06001C9A RID: 7322 RVA: 0x0006EFC8 File Offset: 0x0006D1C8
		internal StyleComplexSelector[] complexSelectors
		{
			get
			{
				return this.m_ComplexSelectors;
			}
			set
			{
				this.m_ComplexSelectors = value;
				this.SetupReferences();
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x0006EFDC File Offset: 0x0006D1DC
		internal List<StyleSheet> flattenedRecursiveImports
		{
			get
			{
				return this.m_FlattenedImportedStyleSheets;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x0006EFF4 File Offset: 0x0006D1F4
		// (set) Token: 0x06001C9D RID: 7325 RVA: 0x0006F00C File Offset: 0x0006D20C
		public int contentHash
		{
			get
			{
				return this.m_ContentHash;
			}
			set
			{
				this.m_ContentHash = value;
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x06001C9E RID: 7326 RVA: 0x0006F018 File Offset: 0x0006D218
		// (set) Token: 0x06001C9F RID: 7327 RVA: 0x0006F030 File Offset: 0x0006D230
		internal bool isDefaultStyleSheet
		{
			get
			{
				return this.m_IsDefaultStyleSheet;
			}
			set
			{
				this.m_IsDefaultStyleSheet = value;
				bool flag = this.flattenedRecursiveImports != null;
				if (flag)
				{
					foreach (StyleSheet styleSheet in this.flattenedRecursiveImports)
					{
						styleSheet.isDefaultStyleSheet = value;
					}
				}
			}
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0006F0A0 File Offset: 0x0006D2A0
		private bool TryCheckAccess<T>(T[] list, StyleValueType type, StyleValueHandle handle, out T value)
		{
			bool flag = false;
			value = default(T);
			bool flag2 = handle.valueType == type && handle.valueIndex >= 0 && handle.valueIndex < list.Length;
			if (flag2)
			{
				value = list[handle.valueIndex];
				flag = true;
			}
			else
			{
				Debug.LogErrorFormat(this, "Trying to read value of type {0} while reading a value of type {1}", new object[] { type, handle.valueType });
			}
			return flag;
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x0006F128 File Offset: 0x0006D328
		private T CheckAccess<T>(T[] list, StyleValueType type, StyleValueHandle handle)
		{
			T t = default(T);
			bool flag = handle.valueType != type;
			if (flag)
			{
				Debug.LogErrorFormat(this, "Trying to read value of type {0} while reading a value of type {1}", new object[] { type, handle.valueType });
			}
			else
			{
				bool flag2 = list == null || handle.valueIndex < 0 || handle.valueIndex >= list.Length;
				if (flag2)
				{
					Debug.LogError("Accessing invalid property", this);
				}
				else
				{
					t = list[handle.valueIndex];
				}
			}
			return t;
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x0006F1C2 File Offset: 0x0006D3C2
		internal virtual void OnEnable()
		{
			this.SetupReferences();
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0006F1CC File Offset: 0x0006D3CC
		internal void FlattenImportedStyleSheetsRecursive()
		{
			this.m_FlattenedImportedStyleSheets = new List<StyleSheet>();
			this.FlattenImportedStyleSheetsRecursive(this);
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x0006F1E4 File Offset: 0x0006D3E4
		private void FlattenImportedStyleSheetsRecursive(StyleSheet sheet)
		{
			bool flag = sheet.imports == null;
			if (!flag)
			{
				for (int i = 0; i < sheet.imports.Length; i++)
				{
					StyleSheet styleSheet = sheet.imports[i].styleSheet;
					bool flag2 = styleSheet == null;
					if (!flag2)
					{
						styleSheet.isDefaultStyleSheet = this.isDefaultStyleSheet;
						this.FlattenImportedStyleSheetsRecursive(styleSheet);
						this.m_FlattenedImportedStyleSheets.Add(styleSheet);
					}
				}
			}
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x0006F260 File Offset: 0x0006D460
		private void SetupReferences()
		{
			bool flag = this.complexSelectors == null || this.rules == null;
			if (!flag)
			{
				foreach (StyleRule styleRule in this.rules)
				{
					foreach (StyleProperty styleProperty in styleRule.properties)
					{
						bool flag2 = StyleSheet.CustomStartsWith(styleProperty.name, StyleSheet.kCustomPropertyMarker);
						if (flag2)
						{
							styleRule.customPropertiesCount++;
							styleProperty.isCustomProperty = true;
						}
						foreach (StyleValueHandle styleValueHandle in styleProperty.values)
						{
							bool flag3 = styleValueHandle.IsVarFunction();
							if (flag3)
							{
								styleProperty.requireVariableResolve = true;
								break;
							}
						}
					}
				}
				int l = 0;
				int num = this.complexSelectors.Length;
				while (l < num)
				{
					this.complexSelectors[l].CachePseudoStateMasks();
					l++;
				}
				this.orderedClassSelectors = new Dictionary<string, StyleComplexSelector>(StringComparer.Ordinal);
				this.orderedNameSelectors = new Dictionary<string, StyleComplexSelector>(StringComparer.Ordinal);
				this.orderedTypeSelectors = new Dictionary<string, StyleComplexSelector>(StringComparer.Ordinal);
				int m = 0;
				while (m < this.complexSelectors.Length)
				{
					StyleComplexSelector styleComplexSelector = this.complexSelectors[m];
					bool flag4 = styleComplexSelector.ruleIndex < this.rules.Length;
					if (flag4)
					{
						styleComplexSelector.rule = this.rules[styleComplexSelector.ruleIndex];
					}
					styleComplexSelector.CalculateHashes();
					styleComplexSelector.orderInStyleSheet = m;
					StyleSelector styleSelector = styleComplexSelector.selectors[styleComplexSelector.selectors.Length - 1];
					StyleSelectorPart styleSelectorPart = styleSelector.parts[0];
					string text = styleSelectorPart.value;
					Dictionary<string, StyleComplexSelector> dictionary = null;
					switch (styleSelectorPart.type)
					{
					case StyleSelectorType.Wildcard:
					case StyleSelectorType.Type:
						text = styleSelectorPart.value ?? "*";
						dictionary = this.orderedTypeSelectors;
						break;
					case StyleSelectorType.Class:
						dictionary = this.orderedClassSelectors;
						break;
					case StyleSelectorType.PseudoClass:
						text = "*";
						dictionary = this.orderedTypeSelectors;
						break;
					case StyleSelectorType.RecursivePseudoClass:
						goto IL_0233;
					case StyleSelectorType.ID:
						dictionary = this.orderedNameSelectors;
						break;
					default:
						goto IL_0233;
					}
					IL_0252:
					bool flag5 = dictionary != null;
					if (flag5)
					{
						StyleComplexSelector styleComplexSelector2;
						bool flag6 = dictionary.TryGetValue(text, out styleComplexSelector2);
						if (flag6)
						{
							styleComplexSelector.nextInTable = styleComplexSelector2;
						}
						dictionary[text] = styleComplexSelector;
					}
					m++;
					continue;
					IL_0233:
					Debug.LogError(string.Format("Invalid first part type {0}", styleSelectorPart.type), this);
					goto IL_0252;
				}
			}
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x0006F510 File Offset: 0x0006D710
		internal StyleValueKeyword ReadKeyword(StyleValueHandle handle)
		{
			return (StyleValueKeyword)handle.valueIndex;
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0006F528 File Offset: 0x0006D728
		internal float ReadFloat(StyleValueHandle handle)
		{
			bool flag = handle.valueType == StyleValueType.Dimension;
			float num;
			if (flag)
			{
				Dimension dimension = this.CheckAccess<Dimension>(this.dimensions, StyleValueType.Dimension, handle);
				num = dimension.value;
			}
			else
			{
				num = this.CheckAccess<float>(this.floats, StyleValueType.Float, handle);
			}
			return num;
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0006F570 File Offset: 0x0006D770
		internal bool TryReadFloat(StyleValueHandle handle, out float value)
		{
			bool flag = this.TryCheckAccess<float>(this.floats, StyleValueType.Float, handle, out value);
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				Dimension dimension;
				bool flag3 = this.TryCheckAccess<Dimension>(this.dimensions, StyleValueType.Float, handle, out dimension);
				value = dimension.value;
				flag2 = flag3;
			}
			return flag2;
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x0006F5B4 File Offset: 0x0006D7B4
		internal Dimension ReadDimension(StyleValueHandle handle)
		{
			bool flag = handle.valueType == StyleValueType.Float;
			Dimension dimension;
			if (flag)
			{
				float num = this.CheckAccess<float>(this.floats, StyleValueType.Float, handle);
				dimension = new Dimension(num, Dimension.Unit.Unitless);
			}
			else
			{
				dimension = this.CheckAccess<Dimension>(this.dimensions, StyleValueType.Dimension, handle);
			}
			return dimension;
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x0006F5FC File Offset: 0x0006D7FC
		internal bool TryReadDimension(StyleValueHandle handle, out Dimension value)
		{
			bool flag = this.TryCheckAccess<Dimension>(this.dimensions, StyleValueType.Dimension, handle, out value);
			bool flag2;
			if (flag)
			{
				flag2 = true;
			}
			else
			{
				float num = 0f;
				bool flag3 = this.TryCheckAccess<float>(this.floats, StyleValueType.Float, handle, out num);
				value = new Dimension(num, Dimension.Unit.Unitless);
				flag2 = flag3;
			}
			return flag2;
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x0006F64C File Offset: 0x0006D84C
		internal Color ReadColor(StyleValueHandle handle)
		{
			return this.CheckAccess<Color>(this.colors, StyleValueType.Color, handle);
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x0006F66C File Offset: 0x0006D86C
		internal bool TryReadColor(StyleValueHandle handle, out Color value)
		{
			return this.TryCheckAccess<Color>(this.colors, StyleValueType.Color, handle, out value);
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x0006F690 File Offset: 0x0006D890
		internal string ReadString(StyleValueHandle handle)
		{
			return this.CheckAccess<string>(this.strings, StyleValueType.String, handle);
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x0006F6B4 File Offset: 0x0006D8B4
		internal bool TryReadString(StyleValueHandle handle, out string value)
		{
			return this.TryCheckAccess<string>(this.strings, StyleValueType.String, handle, out value);
		}

		// Token: 0x06001CAF RID: 7343 RVA: 0x0006F6D8 File Offset: 0x0006D8D8
		internal string ReadEnum(StyleValueHandle handle)
		{
			return this.CheckAccess<string>(this.strings, StyleValueType.Enum, handle);
		}

		// Token: 0x06001CB0 RID: 7344 RVA: 0x0006F6F8 File Offset: 0x0006D8F8
		internal bool TryReadEnum(StyleValueHandle handle, out string value)
		{
			return this.TryCheckAccess<string>(this.strings, StyleValueType.Enum, handle, out value);
		}

		// Token: 0x06001CB1 RID: 7345 RVA: 0x0006F71C File Offset: 0x0006D91C
		internal string ReadVariable(StyleValueHandle handle)
		{
			return this.CheckAccess<string>(this.strings, StyleValueType.Variable, handle);
		}

		// Token: 0x06001CB2 RID: 7346 RVA: 0x0006F73C File Offset: 0x0006D93C
		internal bool TryReadVariable(StyleValueHandle handle, out string value)
		{
			return this.TryCheckAccess<string>(this.strings, StyleValueType.Variable, handle, out value);
		}

		// Token: 0x06001CB3 RID: 7347 RVA: 0x0006F760 File Offset: 0x0006D960
		internal string ReadResourcePath(StyleValueHandle handle)
		{
			return this.CheckAccess<string>(this.strings, StyleValueType.ResourcePath, handle);
		}

		// Token: 0x06001CB4 RID: 7348 RVA: 0x0006F780 File Offset: 0x0006D980
		internal bool TryReadResourcePath(StyleValueHandle handle, out string value)
		{
			return this.TryCheckAccess<string>(this.strings, StyleValueType.ResourcePath, handle, out value);
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x0006F7A4 File Offset: 0x0006D9A4
		internal Object ReadAssetReference(StyleValueHandle handle)
		{
			return this.CheckAccess<Object>(this.assets, StyleValueType.AssetReference, handle);
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x0006F7C4 File Offset: 0x0006D9C4
		internal string ReadMissingAssetReferenceUrl(StyleValueHandle handle)
		{
			return this.CheckAccess<string>(this.strings, StyleValueType.MissingAssetReference, handle);
		}

		// Token: 0x06001CB7 RID: 7351 RVA: 0x0006F7E8 File Offset: 0x0006D9E8
		internal bool TryReadAssetReference(StyleValueHandle handle, out Object value)
		{
			return this.TryCheckAccess<Object>(this.assets, StyleValueType.AssetReference, handle, out value);
		}

		// Token: 0x06001CB8 RID: 7352 RVA: 0x0006F80C File Offset: 0x0006DA0C
		internal StyleValueFunction ReadFunction(StyleValueHandle handle)
		{
			return (StyleValueFunction)handle.valueIndex;
		}

		// Token: 0x06001CB9 RID: 7353 RVA: 0x0006F824 File Offset: 0x0006DA24
		internal string ReadFunctionName(StyleValueHandle handle)
		{
			bool flag = handle.valueType != StyleValueType.Function;
			string text;
			if (flag)
			{
				Debug.LogErrorFormat(this, string.Format("Trying to read value of type {0} while reading a value of type {1}", StyleValueType.Function, handle.valueType), Array.Empty<object>());
				text = string.Empty;
			}
			else
			{
				StyleValueFunction valueIndex = (StyleValueFunction)handle.valueIndex;
				text = valueIndex.ToUssString();
			}
			return text;
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x0006F888 File Offset: 0x0006DA88
		internal ScalableImage ReadScalableImage(StyleValueHandle handle)
		{
			return this.CheckAccess<ScalableImage>(this.scalableImages, StyleValueType.ScalableImage, handle);
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x0006F8AC File Offset: 0x0006DAAC
		private static bool CustomStartsWith(string originalString, string pattern)
		{
			int length = originalString.Length;
			int length2 = pattern.Length;
			int num = 0;
			int num2 = 0;
			while (num < length && num2 < length2 && originalString[num] == pattern[num2])
			{
				num++;
				num2++;
			}
			return (num2 == length2 && length >= length2) || (num == length && length2 >= length);
		}

		// Token: 0x04000BE0 RID: 3040
		[SerializeField]
		private bool m_ImportedWithErrors;

		// Token: 0x04000BE1 RID: 3041
		[SerializeField]
		private bool m_ImportedWithWarnings;

		// Token: 0x04000BE2 RID: 3042
		[SerializeField]
		private StyleRule[] m_Rules;

		// Token: 0x04000BE3 RID: 3043
		[SerializeField]
		private StyleComplexSelector[] m_ComplexSelectors;

		// Token: 0x04000BE4 RID: 3044
		[SerializeField]
		internal float[] floats;

		// Token: 0x04000BE5 RID: 3045
		[SerializeField]
		internal Dimension[] dimensions;

		// Token: 0x04000BE6 RID: 3046
		[SerializeField]
		internal Color[] colors;

		// Token: 0x04000BE7 RID: 3047
		[SerializeField]
		internal string[] strings;

		// Token: 0x04000BE8 RID: 3048
		[SerializeField]
		internal Object[] assets;

		// Token: 0x04000BE9 RID: 3049
		[SerializeField]
		internal StyleSheet.ImportStruct[] imports;

		// Token: 0x04000BEA RID: 3050
		[SerializeField]
		private List<StyleSheet> m_FlattenedImportedStyleSheets;

		// Token: 0x04000BEB RID: 3051
		[SerializeField]
		private int m_ContentHash;

		// Token: 0x04000BEC RID: 3052
		[SerializeField]
		internal ScalableImage[] scalableImages;

		// Token: 0x04000BED RID: 3053
		[NonSerialized]
		internal Dictionary<string, StyleComplexSelector> orderedNameSelectors;

		// Token: 0x04000BEE RID: 3054
		[NonSerialized]
		internal Dictionary<string, StyleComplexSelector> orderedTypeSelectors;

		// Token: 0x04000BEF RID: 3055
		[NonSerialized]
		internal Dictionary<string, StyleComplexSelector> orderedClassSelectors;

		// Token: 0x04000BF0 RID: 3056
		[NonSerialized]
		private bool m_IsDefaultStyleSheet;

		// Token: 0x04000BF1 RID: 3057
		private static string kCustomPropertyMarker = "--";

		// Token: 0x02000357 RID: 855
		[Serializable]
		internal struct ImportStruct
		{
			// Token: 0x04000BF2 RID: 3058
			public StyleSheet styleSheet;

			// Token: 0x04000BF3 RID: 3059
			public string[] mediaQueries;
		}
	}
}
