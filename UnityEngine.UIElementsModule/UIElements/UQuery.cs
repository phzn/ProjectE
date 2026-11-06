using System;
using System.Collections.Generic;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
	// Token: 0x02000383 RID: 899
	public static class UQuery
	{
		// Token: 0x02000384 RID: 900
		internal interface IVisualPredicateWrapper
		{
			// Token: 0x06001E67 RID: 7783
			bool Predicate(object e);
		}

		// Token: 0x02000385 RID: 901
		internal class IsOfType<T> : UQuery.IVisualPredicateWrapper where T : VisualElement
		{
			// Token: 0x06001E68 RID: 7784 RVA: 0x00075AA0 File Offset: 0x00073CA0
			public bool Predicate(object e)
			{
				return e is T;
			}

			// Token: 0x04000CA5 RID: 3237
			public static UQuery.IsOfType<T> s_Instance = new UQuery.IsOfType<T>();
		}

		// Token: 0x02000386 RID: 902
		internal class PredicateWrapper<T> : UQuery.IVisualPredicateWrapper where T : VisualElement
		{
			// Token: 0x06001E6B RID: 7787 RVA: 0x00075AC7 File Offset: 0x00073CC7
			public PredicateWrapper(Func<T, bool> p)
			{
				this.predicate = p;
			}

			// Token: 0x06001E6C RID: 7788 RVA: 0x00075AD8 File Offset: 0x00073CD8
			public bool Predicate(object e)
			{
				T t = e as T;
				bool flag = t != null;
				return flag && this.predicate(t);
			}

			// Token: 0x04000CA6 RID: 3238
			private Func<T, bool> predicate;
		}

		// Token: 0x02000387 RID: 903
		internal abstract class UQueryMatcher : HierarchyTraversal
		{
			// Token: 0x06001E6E RID: 7790 RVA: 0x00075B1E File Offset: 0x00073D1E
			public override void Traverse(VisualElement element)
			{
				base.Traverse(element);
			}

			// Token: 0x06001E6F RID: 7791 RVA: 0x00075B2C File Offset: 0x00073D2C
			protected virtual bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				return false;
			}

			// Token: 0x06001E70 RID: 7792 RVA: 0x00003CD2 File Offset: 0x00001ED2
			private static void NoProcessResult(VisualElement e, MatchResultInfo i)
			{
			}

			// Token: 0x06001E71 RID: 7793 RVA: 0x00075B40 File Offset: 0x00073D40
			public override void TraverseRecursive(VisualElement element, int depth)
			{
				int count = this.m_Matchers.Count;
				int count2 = this.m_Matchers.Count;
				for (int j = 0; j < count2; j++)
				{
					RuleMatcher ruleMatcher = this.m_Matchers[j];
					bool flag = StyleSelectorHelper.MatchRightToLeft(element, ruleMatcher.complexSelector, delegate(VisualElement e, MatchResultInfo i)
					{
						UQuery.UQueryMatcher.NoProcessResult(e, i);
					});
					if (flag)
					{
						bool flag2 = this.OnRuleMatchedElement(ruleMatcher, element);
						if (flag2)
						{
							return;
						}
					}
				}
				base.Recurse(element, depth);
				bool flag3 = this.m_Matchers.Count > count;
				if (flag3)
				{
					this.m_Matchers.RemoveRange(count, this.m_Matchers.Count - count);
					return;
				}
			}

			// Token: 0x06001E72 RID: 7794 RVA: 0x00075C04 File Offset: 0x00073E04
			public virtual void Run(VisualElement root, List<RuleMatcher> matchers)
			{
				this.m_Matchers = matchers;
				this.Traverse(root);
			}

			// Token: 0x04000CA7 RID: 3239
			internal List<RuleMatcher> m_Matchers;
		}

		// Token: 0x02000389 RID: 905
		internal abstract class SingleQueryMatcher : UQuery.UQueryMatcher
		{
			// Token: 0x17000718 RID: 1816
			// (get) Token: 0x06001E76 RID: 7798 RVA: 0x00075C2C File Offset: 0x00073E2C
			// (set) Token: 0x06001E77 RID: 7799 RVA: 0x00075C34 File Offset: 0x00073E34
			public VisualElement match { get; set; }

			// Token: 0x06001E78 RID: 7800 RVA: 0x00075C3D File Offset: 0x00073E3D
			public override void Run(VisualElement root, List<RuleMatcher> matchers)
			{
				this.match = null;
				base.Run(root, matchers);
				this.m_Matchers = null;
			}

			// Token: 0x06001E79 RID: 7801 RVA: 0x00075C58 File Offset: 0x00073E58
			public bool IsInUse()
			{
				return this.m_Matchers != null;
			}

			// Token: 0x06001E7A RID: 7802
			public abstract UQuery.SingleQueryMatcher CreateNew();
		}

		// Token: 0x0200038A RID: 906
		internal class FirstQueryMatcher : UQuery.SingleQueryMatcher
		{
			// Token: 0x06001E7C RID: 7804 RVA: 0x00075C7C File Offset: 0x00073E7C
			protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				bool flag = base.match == null;
				if (flag)
				{
					base.match = element;
				}
				return true;
			}

			// Token: 0x06001E7D RID: 7805 RVA: 0x00075CA4 File Offset: 0x00073EA4
			public override UQuery.SingleQueryMatcher CreateNew()
			{
				return new UQuery.FirstQueryMatcher();
			}

			// Token: 0x04000CAB RID: 3243
			public static readonly UQuery.FirstQueryMatcher Instance = new UQuery.FirstQueryMatcher();
		}

		// Token: 0x0200038B RID: 907
		internal class LastQueryMatcher : UQuery.SingleQueryMatcher
		{
			// Token: 0x06001E80 RID: 7808 RVA: 0x00075CC0 File Offset: 0x00073EC0
			protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				base.match = element;
				return false;
			}

			// Token: 0x06001E81 RID: 7809 RVA: 0x00075CDB File Offset: 0x00073EDB
			public override UQuery.SingleQueryMatcher CreateNew()
			{
				return new UQuery.LastQueryMatcher();
			}

			// Token: 0x04000CAC RID: 3244
			public static readonly UQuery.LastQueryMatcher Instance = new UQuery.LastQueryMatcher();
		}

		// Token: 0x0200038C RID: 908
		internal class IndexQueryMatcher : UQuery.SingleQueryMatcher
		{
			// Token: 0x17000719 RID: 1817
			// (get) Token: 0x06001E84 RID: 7812 RVA: 0x00075CF0 File Offset: 0x00073EF0
			// (set) Token: 0x06001E85 RID: 7813 RVA: 0x00075D08 File Offset: 0x00073F08
			public int matchIndex
			{
				get
				{
					return this._matchIndex;
				}
				set
				{
					this.matchCount = -1;
					this._matchIndex = value;
				}
			}

			// Token: 0x06001E86 RID: 7814 RVA: 0x00075D19 File Offset: 0x00073F19
			public override void Run(VisualElement root, List<RuleMatcher> matchers)
			{
				this.matchCount = -1;
				base.Run(root, matchers);
			}

			// Token: 0x06001E87 RID: 7815 RVA: 0x00075D2C File Offset: 0x00073F2C
			protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				this.matchCount++;
				bool flag = this.matchCount == this._matchIndex;
				if (flag)
				{
					base.match = element;
				}
				return this.matchCount >= this._matchIndex;
			}

			// Token: 0x06001E88 RID: 7816 RVA: 0x00075D79 File Offset: 0x00073F79
			public override UQuery.SingleQueryMatcher CreateNew()
			{
				return new UQuery.IndexQueryMatcher();
			}

			// Token: 0x04000CAD RID: 3245
			public static readonly UQuery.IndexQueryMatcher Instance = new UQuery.IndexQueryMatcher();

			// Token: 0x04000CAE RID: 3246
			private int matchCount = -1;

			// Token: 0x04000CAF RID: 3247
			private int _matchIndex;
		}
	}
}
