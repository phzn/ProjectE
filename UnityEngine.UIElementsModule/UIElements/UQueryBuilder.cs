using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x02000392 RID: 914
	public struct UQueryBuilder<T> : IEquatable<UQueryBuilder<T>> where T : VisualElement
	{
		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06001EB5 RID: 7861 RVA: 0x000762C0 File Offset: 0x000744C0
		private List<StyleSelector> styleSelectors
		{
			get
			{
				List<StyleSelector> list;
				if ((list = this.m_StyleSelectors) == null)
				{
					list = (this.m_StyleSelectors = new List<StyleSelector>());
				}
				return list;
			}
		}

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06001EB6 RID: 7862 RVA: 0x000762EC File Offset: 0x000744EC
		private List<StyleSelectorPart> parts
		{
			get
			{
				List<StyleSelectorPart> list;
				if ((list = this.m_Parts) == null)
				{
					list = (this.m_Parts = new List<StyleSelectorPart>());
				}
				return list;
			}
		}

		// Token: 0x06001EB7 RID: 7863 RVA: 0x00076318 File Offset: 0x00074518
		public UQueryBuilder(VisualElement visualElement)
		{
			this = default(UQueryBuilder<T>);
			this.m_Element = visualElement;
			this.m_Parts = null;
			this.m_StyleSelectors = null;
			this.m_Relationship = StyleSelectorRelationship.None;
			this.m_Matchers = new List<RuleMatcher>();
			this.pseudoStatesMask = (this.negatedPseudoStatesMask = 0);
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x00076364 File Offset: 0x00074564
		public UQueryBuilder<T> Class(string classname)
		{
			this.AddClass(classname);
			return this;
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x00076384 File Offset: 0x00074584
		public UQueryBuilder<T> Name(string id)
		{
			this.AddName(id);
			return this;
		}

		// Token: 0x06001EBA RID: 7866 RVA: 0x000763A4 File Offset: 0x000745A4
		public UQueryBuilder<T2> Descendents<T2>(string name = null, params string[] classNames) where T2 : VisualElement
		{
			this.FinishCurrentSelector();
			this.AddType<T2>();
			this.AddName(name);
			this.AddClasses(classNames);
			return this.AddRelationship<T2>(StyleSelectorRelationship.Descendent);
		}

		// Token: 0x06001EBB RID: 7867 RVA: 0x000763DC File Offset: 0x000745DC
		public UQueryBuilder<T2> Descendents<T2>(string name = null, string classname = null) where T2 : VisualElement
		{
			this.FinishCurrentSelector();
			this.AddType<T2>();
			this.AddName(name);
			this.AddClass(classname);
			return this.AddRelationship<T2>(StyleSelectorRelationship.Descendent);
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x00076414 File Offset: 0x00074614
		public UQueryBuilder<T2> Children<T2>(string name = null, params string[] classes) where T2 : VisualElement
		{
			this.FinishCurrentSelector();
			this.AddType<T2>();
			this.AddName(name);
			this.AddClasses(classes);
			return this.AddRelationship<T2>(StyleSelectorRelationship.Child);
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x0007644C File Offset: 0x0007464C
		public UQueryBuilder<T2> Children<T2>(string name = null, string className = null) where T2 : VisualElement
		{
			this.FinishCurrentSelector();
			this.AddType<T2>();
			this.AddName(name);
			this.AddClass(className);
			return this.AddRelationship<T2>(StyleSelectorRelationship.Child);
		}

		// Token: 0x06001EBE RID: 7870 RVA: 0x00076484 File Offset: 0x00074684
		public UQueryBuilder<T2> OfType<T2>(string name = null, params string[] classes) where T2 : VisualElement
		{
			this.AddType<T2>();
			this.AddName(name);
			this.AddClasses(classes);
			return this.AddRelationship<T2>(StyleSelectorRelationship.None);
		}

		// Token: 0x06001EBF RID: 7871 RVA: 0x000764B4 File Offset: 0x000746B4
		public UQueryBuilder<T2> OfType<T2>(string name = null, string className = null) where T2 : VisualElement
		{
			this.AddType<T2>();
			this.AddName(name);
			this.AddClass(className);
			return this.AddRelationship<T2>(StyleSelectorRelationship.None);
		}

		// Token: 0x06001EC0 RID: 7872 RVA: 0x000764E4 File Offset: 0x000746E4
		internal UQueryBuilder<T> SingleBaseType()
		{
			this.parts.Add(StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance));
			return this;
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x00076514 File Offset: 0x00074714
		public UQueryBuilder<T> Where(Func<T, bool> selectorPredicate)
		{
			this.parts.Add(StyleSelectorPart.CreatePredicate(new UQuery.PredicateWrapper<T>(selectorPredicate)));
			return this;
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x00076544 File Offset: 0x00074744
		private void AddClass(string c)
		{
			bool flag = c != null;
			if (flag)
			{
				this.parts.Add(StyleSelectorPart.CreateClass(c));
			}
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0007656C File Offset: 0x0007476C
		private void AddClasses(params string[] classes)
		{
			bool flag = classes != null;
			if (flag)
			{
				for (int i = 0; i < classes.Length; i++)
				{
					this.AddClass(classes[i]);
				}
			}
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000765A0 File Offset: 0x000747A0
		private void AddName(string id)
		{
			bool flag = id != null;
			if (flag)
			{
				this.parts.Add(StyleSelectorPart.CreateId(id));
			}
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000765C8 File Offset: 0x000747C8
		private void AddType<T2>() where T2 : VisualElement
		{
			bool flag = typeof(T2) != typeof(VisualElement);
			if (flag)
			{
				this.parts.Add(StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T2>.s_Instance));
			}
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x0007660C File Offset: 0x0007480C
		private UQueryBuilder<T> AddPseudoState(PseudoStates s)
		{
			this.pseudoStatesMask |= (int)s;
			return this;
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x00076634 File Offset: 0x00074834
		private UQueryBuilder<T> AddNegativePseudoState(PseudoStates s)
		{
			this.negatedPseudoStatesMask |= (int)s;
			return this;
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x0007665C File Offset: 0x0007485C
		public UQueryBuilder<T> Active()
		{
			return this.AddPseudoState(PseudoStates.Active);
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x00076678 File Offset: 0x00074878
		public UQueryBuilder<T> NotActive()
		{
			return this.AddNegativePseudoState(PseudoStates.Active);
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x00076694 File Offset: 0x00074894
		public UQueryBuilder<T> Visible()
		{
			return this.Where((T e) => e.visible);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000766CC File Offset: 0x000748CC
		public UQueryBuilder<T> NotVisible()
		{
			return this.Where((T e) => !e.visible);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x00076704 File Offset: 0x00074904
		public UQueryBuilder<T> Hovered()
		{
			return this.AddPseudoState(PseudoStates.Hover);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x00076720 File Offset: 0x00074920
		public UQueryBuilder<T> NotHovered()
		{
			return this.AddNegativePseudoState(PseudoStates.Hover);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x0007673C File Offset: 0x0007493C
		public UQueryBuilder<T> Checked()
		{
			return this.AddPseudoState(PseudoStates.Checked);
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x00076758 File Offset: 0x00074958
		public UQueryBuilder<T> NotChecked()
		{
			return this.AddNegativePseudoState(PseudoStates.Checked);
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x00076774 File Offset: 0x00074974
		[Obsolete("Use Checked() instead")]
		public UQueryBuilder<T> Selected()
		{
			return this.AddPseudoState(PseudoStates.Checked);
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x00076790 File Offset: 0x00074990
		[Obsolete("Use NotChecked() instead")]
		public UQueryBuilder<T> NotSelected()
		{
			return this.AddNegativePseudoState(PseudoStates.Checked);
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000767AC File Offset: 0x000749AC
		public UQueryBuilder<T> Enabled()
		{
			return this.AddNegativePseudoState(PseudoStates.Disabled);
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000767C8 File Offset: 0x000749C8
		public UQueryBuilder<T> NotEnabled()
		{
			return this.AddPseudoState(PseudoStates.Disabled);
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000767E4 File Offset: 0x000749E4
		public UQueryBuilder<T> Focused()
		{
			return this.AddPseudoState(PseudoStates.Focus);
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x00076800 File Offset: 0x00074A00
		public UQueryBuilder<T> NotFocused()
		{
			return this.AddNegativePseudoState(PseudoStates.Focus);
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0007681C File Offset: 0x00074A1C
		private UQueryBuilder<T2> AddRelationship<T2>(StyleSelectorRelationship relationship) where T2 : VisualElement
		{
			return new UQueryBuilder<T2>(this.m_Element)
			{
				m_Matchers = this.m_Matchers,
				m_Parts = this.m_Parts,
				m_StyleSelectors = this.m_StyleSelectors,
				m_Relationship = ((relationship == StyleSelectorRelationship.None) ? this.m_Relationship : relationship),
				pseudoStatesMask = this.pseudoStatesMask,
				negatedPseudoStatesMask = this.negatedPseudoStatesMask
			};
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x00076890 File Offset: 0x00074A90
		private void AddPseudoStatesRuleIfNecessasy()
		{
			bool flag = this.pseudoStatesMask != 0 || this.negatedPseudoStatesMask != 0;
			if (flag)
			{
				this.parts.Add(new StyleSelectorPart
				{
					type = StyleSelectorType.PseudoClass
				});
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x000768D8 File Offset: 0x00074AD8
		private void FinishSelector()
		{
			this.FinishCurrentSelector();
			bool flag = this.styleSelectors.Count > 0;
			if (flag)
			{
				StyleComplexSelector styleComplexSelector = new StyleComplexSelector();
				styleComplexSelector.selectors = this.styleSelectors.ToArray();
				this.styleSelectors.Clear();
				this.m_Matchers.Add(new RuleMatcher
				{
					complexSelector = styleComplexSelector
				});
			}
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x00076944 File Offset: 0x00074B44
		private bool CurrentSelectorEmpty()
		{
			return this.parts.Count == 0 && this.m_Relationship == StyleSelectorRelationship.None && this.pseudoStatesMask == 0 && this.negatedPseudoStatesMask == 0;
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x00076980 File Offset: 0x00074B80
		private void FinishCurrentSelector()
		{
			bool flag = !this.CurrentSelectorEmpty();
			if (flag)
			{
				StyleSelector styleSelector = new StyleSelector();
				styleSelector.previousRelationship = this.m_Relationship;
				this.AddPseudoStatesRuleIfNecessasy();
				styleSelector.parts = this.m_Parts.ToArray();
				styleSelector.pseudoStateMask = this.pseudoStatesMask;
				styleSelector.negatedPseudoStateMask = this.negatedPseudoStatesMask;
				this.styleSelectors.Add(styleSelector);
				this.m_Parts.Clear();
				this.pseudoStatesMask = (this.negatedPseudoStatesMask = 0);
			}
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x00076A0C File Offset: 0x00074C0C
		public UQueryState<T> Build()
		{
			this.FinishSelector();
			bool flag = this.m_Matchers.Count == 0;
			if (flag)
			{
				this.parts.Add(new StyleSelectorPart
				{
					type = StyleSelectorType.Wildcard
				});
				this.FinishSelector();
			}
			return new UQueryState<T>(this.m_Element, this.m_Matchers);
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x00076A70 File Offset: 0x00074C70
		public static implicit operator T(UQueryBuilder<T> s)
		{
			return s.First();
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x00076A8C File Offset: 0x00074C8C
		public static bool operator ==(UQueryBuilder<T> builder1, UQueryBuilder<T> builder2)
		{
			return builder1.Equals(builder2);
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x00076AA8 File Offset: 0x00074CA8
		public static bool operator !=(UQueryBuilder<T> builder1, UQueryBuilder<T> builder2)
		{
			return !(builder1 == builder2);
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x00076AC4 File Offset: 0x00074CC4
		public T First()
		{
			return this.Build().First();
		}

		// Token: 0x06001EE0 RID: 7904 RVA: 0x00076AE4 File Offset: 0x00074CE4
		public T Last()
		{
			return this.Build().Last();
		}

		// Token: 0x06001EE1 RID: 7905 RVA: 0x00076B04 File Offset: 0x00074D04
		public List<T> ToList()
		{
			return this.Build().ToList();
		}

		// Token: 0x06001EE2 RID: 7906 RVA: 0x00076B24 File Offset: 0x00074D24
		public void ToList(List<T> results)
		{
			this.Build().ToList(results);
		}

		// Token: 0x06001EE3 RID: 7907 RVA: 0x00076B44 File Offset: 0x00074D44
		public T AtIndex(int index)
		{
			return this.Build().AtIndex(index);
		}

		// Token: 0x06001EE4 RID: 7908 RVA: 0x00076B68 File Offset: 0x00074D68
		public void ForEach<T2>(List<T2> result, Func<T, T2> funcCall)
		{
			this.Build().ForEach<T2>(result, funcCall);
		}

		// Token: 0x06001EE5 RID: 7909 RVA: 0x00076B88 File Offset: 0x00074D88
		public List<T2> ForEach<T2>(Func<T, T2> funcCall)
		{
			return this.Build().ForEach<T2>(funcCall);
		}

		// Token: 0x06001EE6 RID: 7910 RVA: 0x00076BAC File Offset: 0x00074DAC
		public void ForEach(Action<T> funcCall)
		{
			this.Build().ForEach(funcCall);
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x00076BCC File Offset: 0x00074DCC
		public bool Equals(UQueryBuilder<T> other)
		{
			return EqualityComparer<List<StyleSelector>>.Default.Equals(this.m_StyleSelectors, other.m_StyleSelectors) && EqualityComparer<List<StyleSelector>>.Default.Equals(this.styleSelectors, other.styleSelectors) && EqualityComparer<List<StyleSelectorPart>>.Default.Equals(this.m_Parts, other.m_Parts) && EqualityComparer<List<StyleSelectorPart>>.Default.Equals(this.parts, other.parts) && this.m_Element == other.m_Element && EqualityComparer<List<RuleMatcher>>.Default.Equals(this.m_Matchers, other.m_Matchers) && this.m_Relationship == other.m_Relationship && this.pseudoStatesMask == other.pseudoStatesMask && this.negatedPseudoStatesMask == other.negatedPseudoStatesMask;
		}

		// Token: 0x06001EE8 RID: 7912 RVA: 0x00076C9C File Offset: 0x00074E9C
		public override bool Equals(object obj)
		{
			bool flag = !(obj is UQueryBuilder<T>);
			return !flag && this.Equals((UQueryBuilder<T>)obj);
		}

		// Token: 0x06001EE9 RID: 7913 RVA: 0x00076CD0 File Offset: 0x00074ED0
		public override int GetHashCode()
		{
			int num = -949812380;
			num = num * -1521134295 + EqualityComparer<List<StyleSelector>>.Default.GetHashCode(this.m_StyleSelectors);
			num = num * -1521134295 + EqualityComparer<List<StyleSelector>>.Default.GetHashCode(this.styleSelectors);
			num = num * -1521134295 + EqualityComparer<List<StyleSelectorPart>>.Default.GetHashCode(this.m_Parts);
			num = num * -1521134295 + EqualityComparer<List<StyleSelectorPart>>.Default.GetHashCode(this.parts);
			num = num * -1521134295 + EqualityComparer<VisualElement>.Default.GetHashCode(this.m_Element);
			num = num * -1521134295 + EqualityComparer<List<RuleMatcher>>.Default.GetHashCode(this.m_Matchers);
			num = num * -1521134295 + this.m_Relationship.GetHashCode();
			num = num * -1521134295 + this.pseudoStatesMask.GetHashCode();
			return num * -1521134295 + this.negatedPseudoStatesMask.GetHashCode();
		}

		// Token: 0x04000CBC RID: 3260
		private List<StyleSelector> m_StyleSelectors;

		// Token: 0x04000CBD RID: 3261
		private List<StyleSelectorPart> m_Parts;

		// Token: 0x04000CBE RID: 3262
		private VisualElement m_Element;

		// Token: 0x04000CBF RID: 3263
		private List<RuleMatcher> m_Matchers;

		// Token: 0x04000CC0 RID: 3264
		private StyleSelectorRelationship m_Relationship;

		// Token: 0x04000CC1 RID: 3265
		private int pseudoStatesMask;

		// Token: 0x04000CC2 RID: 3266
		private int negatedPseudoStatesMask;
	}
}
