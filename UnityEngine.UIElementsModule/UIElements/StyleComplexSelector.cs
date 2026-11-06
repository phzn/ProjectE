using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEngine.UIElements
{
	// Token: 0x0200034C RID: 844
	[Serializable]
	internal class StyleComplexSelector : ISerializationCallbackReceiver
	{
		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x06001C60 RID: 7264 RVA: 0x0006E6C8 File Offset: 0x0006C8C8
		// (set) Token: 0x06001C61 RID: 7265 RVA: 0x0006E6E0 File Offset: 0x0006C8E0
		public int specificity
		{
			get
			{
				return this.m_Specificity;
			}
			internal set
			{
				this.m_Specificity = value;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x06001C62 RID: 7266 RVA: 0x0006E6EA File Offset: 0x0006C8EA
		// (set) Token: 0x06001C63 RID: 7267 RVA: 0x0006E6F2 File Offset: 0x0006C8F2
		public StyleRule rule { get; internal set; }

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x06001C64 RID: 7268 RVA: 0x0006E6FC File Offset: 0x0006C8FC
		public bool isSimple
		{
			get
			{
				return this.m_isSimple;
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x06001C65 RID: 7269 RVA: 0x0006E714 File Offset: 0x0006C914
		// (set) Token: 0x06001C66 RID: 7270 RVA: 0x0006E72C File Offset: 0x0006C92C
		public StyleSelector[] selectors
		{
			get
			{
				return this.m_Selectors;
			}
			internal set
			{
				this.m_Selectors = value;
				this.m_isSimple = this.m_Selectors.Length == 1;
			}
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x00003CD2 File Offset: 0x00001ED2
		public void OnBeforeSerialize()
		{
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x0006E747 File Offset: 0x0006C947
		public virtual void OnAfterDeserialize()
		{
			this.m_isSimple = this.m_Selectors.Length == 1;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x0006E75C File Offset: 0x0006C95C
		internal void CachePseudoStateMasks()
		{
			bool flag = StyleComplexSelector.s_PseudoStates == null;
			if (flag)
			{
				StyleComplexSelector.s_PseudoStates = new Dictionary<string, StyleComplexSelector.PseudoStateData>();
				StyleComplexSelector.s_PseudoStates["active"] = new StyleComplexSelector.PseudoStateData(PseudoStates.Active, false);
				StyleComplexSelector.s_PseudoStates["hover"] = new StyleComplexSelector.PseudoStateData(PseudoStates.Hover, false);
				StyleComplexSelector.s_PseudoStates["checked"] = new StyleComplexSelector.PseudoStateData(PseudoStates.Checked, false);
				StyleComplexSelector.s_PseudoStates["selected"] = new StyleComplexSelector.PseudoStateData(PseudoStates.Checked, false);
				StyleComplexSelector.s_PseudoStates["disabled"] = new StyleComplexSelector.PseudoStateData(PseudoStates.Disabled, false);
				StyleComplexSelector.s_PseudoStates["focus"] = new StyleComplexSelector.PseudoStateData(PseudoStates.Focus, false);
				StyleComplexSelector.s_PseudoStates["root"] = new StyleComplexSelector.PseudoStateData(PseudoStates.Root, false);
				StyleComplexSelector.s_PseudoStates["inactive"] = new StyleComplexSelector.PseudoStateData(PseudoStates.Active, true);
				StyleComplexSelector.s_PseudoStates["enabled"] = new StyleComplexSelector.PseudoStateData(PseudoStates.Disabled, true);
			}
			int i = 0;
			int num = this.selectors.Length;
			while (i < num)
			{
				StyleSelector styleSelector = this.selectors[i];
				StyleSelectorPart[] parts = styleSelector.parts;
				PseudoStates pseudoStates = (PseudoStates)0;
				PseudoStates pseudoStates2 = (PseudoStates)0;
				for (int j = 0; j < styleSelector.parts.Length; j++)
				{
					bool flag2 = styleSelector.parts[j].type == StyleSelectorType.PseudoClass;
					if (flag2)
					{
						StyleComplexSelector.PseudoStateData pseudoStateData;
						bool flag3 = StyleComplexSelector.s_PseudoStates.TryGetValue(parts[j].value, out pseudoStateData);
						if (flag3)
						{
							bool flag4 = !pseudoStateData.negate;
							if (flag4)
							{
								pseudoStates |= pseudoStateData.state;
							}
							else
							{
								pseudoStates2 |= pseudoStateData.state;
							}
						}
						else
						{
							Debug.LogWarningFormat("Unknown pseudo class \"{0}\"", new object[] { parts[j].value });
						}
					}
				}
				styleSelector.pseudoStateMask = (int)pseudoStates;
				styleSelector.negatedPseudoStateMask = (int)pseudoStates2;
				i++;
			}
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x0006E95C File Offset: 0x0006CB5C
		public override string ToString()
		{
			return string.Format("[{0}]", string.Join(", ", this.m_Selectors.Select<StyleSelector, string>((StyleSelector x) => x.ToString()).ToArray<string>()));
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x0006E9B4 File Offset: 0x0006CBB4
		private static int StyleSelectorPartCompare(StyleSelectorPart x, StyleSelectorPart y)
		{
			bool flag = y.type < x.type;
			int num;
			if (flag)
			{
				num = -1;
			}
			else
			{
				bool flag2 = y.type > x.type;
				if (flag2)
				{
					num = 1;
				}
				else
				{
					num = y.value.CompareTo(x.value);
				}
			}
			return num;
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x0006EA0C File Offset: 0x0006CC0C
		internal unsafe void CalculateHashes()
		{
			bool isSimple = this.isSimple;
			if (!isSimple)
			{
				for (int i = this.selectors.Length - 2; i > -1; i--)
				{
					StyleComplexSelector.m_HashList.AddRange(this.selectors[i].parts);
				}
				StyleComplexSelector.m_HashList.RemoveAll((StyleSelectorPart p) => p.type != StyleSelectorType.Class && p.type != StyleSelectorType.ID && p.type != StyleSelectorType.Type);
				StyleComplexSelector.m_HashList.Sort(new Comparison<StyleSelectorPart>(StyleComplexSelector.StyleSelectorPartCompare));
				bool flag = true;
				StyleSelectorType styleSelectorType = StyleSelectorType.Unknown;
				string text = "";
				int num = 0;
				int num2 = Math.Min(4, StyleComplexSelector.m_HashList.Count);
				for (int j = 0; j < num2; j++)
				{
					bool flag2 = flag;
					if (flag2)
					{
						flag = false;
					}
					else
					{
						while (num < StyleComplexSelector.m_HashList.Count && StyleComplexSelector.m_HashList[num].type == styleSelectorType && StyleComplexSelector.m_HashList[num].value == text)
						{
							num++;
						}
						bool flag3 = num == StyleComplexSelector.m_HashList.Count;
						if (flag3)
						{
							break;
						}
					}
					styleSelectorType = StyleComplexSelector.m_HashList[num].type;
					text = StyleComplexSelector.m_HashList[num].value;
					bool flag4 = styleSelectorType == StyleSelectorType.ID;
					Salt salt;
					if (flag4)
					{
						salt = Salt.IdSalt;
					}
					else
					{
						bool flag5 = styleSelectorType == StyleSelectorType.Class;
						if (flag5)
						{
							salt = Salt.ClassSalt;
						}
						else
						{
							salt = Salt.TagNameSalt;
						}
					}
					*((ref this.ancestorHashes.hashes.FixedElementField) + (IntPtr)j * 4) = text.GetHashCode() * (int)salt;
				}
				StyleComplexSelector.m_HashList.Clear();
			}
		}

		// Token: 0x04000BB3 RID: 2995
		[NonSerialized]
		public Hashes ancestorHashes;

		// Token: 0x04000BB4 RID: 2996
		[SerializeField]
		private int m_Specificity;

		// Token: 0x04000BB6 RID: 2998
		[NonSerialized]
		private bool m_isSimple;

		// Token: 0x04000BB7 RID: 2999
		[SerializeField]
		private StyleSelector[] m_Selectors;

		// Token: 0x04000BB8 RID: 3000
		[SerializeField]
		internal int ruleIndex;

		// Token: 0x04000BB9 RID: 3001
		[NonSerialized]
		internal StyleComplexSelector nextInTable;

		// Token: 0x04000BBA RID: 3002
		[NonSerialized]
		internal int orderInStyleSheet;

		// Token: 0x04000BBB RID: 3003
		private static Dictionary<string, StyleComplexSelector.PseudoStateData> s_PseudoStates;

		// Token: 0x04000BBC RID: 3004
		private static List<StyleSelectorPart> m_HashList = new List<StyleSelectorPart>();

		// Token: 0x0200034D RID: 845
		private struct PseudoStateData
		{
			// Token: 0x06001C6F RID: 7279 RVA: 0x0006EBE4 File Offset: 0x0006CDE4
			public PseudoStateData(PseudoStates state, bool negate)
			{
				this.state = state;
				this.negate = negate;
			}

			// Token: 0x04000BBD RID: 3005
			public readonly PseudoStates state;

			// Token: 0x04000BBE RID: 3006
			public readonly bool negate;
		}
	}
}
