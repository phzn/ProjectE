using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200038D RID: 909
	public struct UQueryState<T> : IEnumerable<T>, IEnumerable, IEquatable<UQueryState<T>> where T : VisualElement
	{
		// Token: 0x06001E8B RID: 7819 RVA: 0x00075D9C File Offset: 0x00073F9C
		internal UQueryState(VisualElement element, List<RuleMatcher> matchers)
		{
			this.m_Element = element;
			this.m_Matchers = matchers;
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x00075DB0 File Offset: 0x00073FB0
		public UQueryState<T> RebuildOn(VisualElement element)
		{
			return new UQueryState<T>(element, this.m_Matchers);
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x00075DD0 File Offset: 0x00073FD0
		private T Single(UQuery.SingleQueryMatcher matcher)
		{
			bool flag = matcher.IsInUse();
			if (flag)
			{
				matcher = matcher.CreateNew();
			}
			matcher.Run(this.m_Element, this.m_Matchers);
			T t = matcher.match as T;
			matcher.match = null;
			return t;
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x00075E23 File Offset: 0x00074023
		public T First()
		{
			return this.Single(UQuery.FirstQueryMatcher.Instance);
		}

		// Token: 0x06001E8F RID: 7823 RVA: 0x00075E30 File Offset: 0x00074030
		public T Last()
		{
			return this.Single(UQuery.LastQueryMatcher.Instance);
		}

		// Token: 0x06001E90 RID: 7824 RVA: 0x00075E3D File Offset: 0x0007403D
		public void ToList(List<T> results)
		{
			UQueryState<T>.s_List.matches = results;
			UQueryState<T>.s_List.Run(this.m_Element, this.m_Matchers);
			UQueryState<T>.s_List.Reset();
		}

		// Token: 0x06001E91 RID: 7825 RVA: 0x00075E70 File Offset: 0x00074070
		public List<T> ToList()
		{
			List<T> list = new List<T>();
			this.ToList(list);
			return list;
		}

		// Token: 0x06001E92 RID: 7826 RVA: 0x00075E94 File Offset: 0x00074094
		public T AtIndex(int index)
		{
			UQuery.IndexQueryMatcher instance = UQuery.IndexQueryMatcher.Instance;
			instance.matchIndex = index;
			return this.Single(instance);
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x00075EBC File Offset: 0x000740BC
		public void ForEach(Action<T> funcCall)
		{
			UQueryState<T>.ActionQueryMatcher actionQueryMatcher = UQueryState<T>.s_Action;
			bool flag = actionQueryMatcher.callBack != null;
			if (flag)
			{
				actionQueryMatcher = new UQueryState<T>.ActionQueryMatcher();
			}
			try
			{
				actionQueryMatcher.callBack = funcCall;
				actionQueryMatcher.Run(this.m_Element, this.m_Matchers);
			}
			finally
			{
				actionQueryMatcher.callBack = null;
			}
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x00075F20 File Offset: 0x00074120
		public void ForEach<T2>(List<T2> result, Func<T, T2> funcCall)
		{
			UQueryState<T>.DelegateQueryMatcher<T2> delegateQueryMatcher = UQueryState<T>.DelegateQueryMatcher<T2>.s_Instance;
			bool flag = delegateQueryMatcher.callBack != null;
			if (flag)
			{
				delegateQueryMatcher = new UQueryState<T>.DelegateQueryMatcher<T2>();
			}
			try
			{
				delegateQueryMatcher.callBack = funcCall;
				delegateQueryMatcher.result = result;
				delegateQueryMatcher.Run(this.m_Element, this.m_Matchers);
			}
			finally
			{
				delegateQueryMatcher.callBack = null;
				delegateQueryMatcher.result = null;
			}
		}

		// Token: 0x06001E95 RID: 7829 RVA: 0x00075F94 File Offset: 0x00074194
		public List<T2> ForEach<T2>(Func<T, T2> funcCall)
		{
			List<T2> list = new List<T2>();
			this.ForEach<T2>(list, funcCall);
			return list;
		}

		// Token: 0x06001E96 RID: 7830 RVA: 0x00075FB6 File Offset: 0x000741B6
		public UQueryState<T>.Enumerator GetEnumerator()
		{
			return new UQueryState<T>.Enumerator(this);
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x00075FC3 File Offset: 0x000741C3
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x00075FC3 File Offset: 0x000741C3
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x00075FD0 File Offset: 0x000741D0
		public bool Equals(UQueryState<T> other)
		{
			return this.m_Element == other.m_Element && EqualityComparer<List<RuleMatcher>>.Default.Equals(this.m_Matchers, other.m_Matchers);
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0007600C File Offset: 0x0007420C
		public override bool Equals(object obj)
		{
			bool flag = !(obj is UQueryState<T>);
			return !flag && this.Equals((UQueryState<T>)obj);
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x00076040 File Offset: 0x00074240
		public override int GetHashCode()
		{
			int num = 488160421;
			num = num * -1521134295 + EqualityComparer<VisualElement>.Default.GetHashCode(this.m_Element);
			return num * -1521134295 + EqualityComparer<List<RuleMatcher>>.Default.GetHashCode(this.m_Matchers);
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0007608C File Offset: 0x0007428C
		public static bool operator ==(UQueryState<T> state1, UQueryState<T> state2)
		{
			return state1.Equals(state2);
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x000760A8 File Offset: 0x000742A8
		public static bool operator !=(UQueryState<T> state1, UQueryState<T> state2)
		{
			return !(state1 == state2);
		}

		// Token: 0x04000CB0 RID: 3248
		private static UQueryState<T>.ActionQueryMatcher s_Action = new UQueryState<T>.ActionQueryMatcher();

		// Token: 0x04000CB1 RID: 3249
		private readonly VisualElement m_Element;

		// Token: 0x04000CB2 RID: 3250
		internal readonly List<RuleMatcher> m_Matchers;

		// Token: 0x04000CB3 RID: 3251
		private static readonly UQueryState<T>.ListQueryMatcher<T> s_List = new UQueryState<T>.ListQueryMatcher<T>();

		// Token: 0x04000CB4 RID: 3252
		private static readonly UQueryState<T>.ListQueryMatcher<VisualElement> s_EnumerationList = new UQueryState<T>.ListQueryMatcher<VisualElement>();

		// Token: 0x0200038E RID: 910
		private class ListQueryMatcher<TElement> : UQuery.UQueryMatcher where TElement : VisualElement
		{
			// Token: 0x1700071A RID: 1818
			// (get) Token: 0x06001E9F RID: 7839 RVA: 0x000760E4 File Offset: 0x000742E4
			// (set) Token: 0x06001EA0 RID: 7840 RVA: 0x000760EC File Offset: 0x000742EC
			public List<TElement> matches { get; set; }

			// Token: 0x06001EA1 RID: 7841 RVA: 0x000760F8 File Offset: 0x000742F8
			protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				this.matches.Add(element as TElement);
				return false;
			}

			// Token: 0x06001EA2 RID: 7842 RVA: 0x00076122 File Offset: 0x00074322
			public void Reset()
			{
				this.matches = null;
			}
		}

		// Token: 0x0200038F RID: 911
		private class ActionQueryMatcher : UQuery.UQueryMatcher
		{
			// Token: 0x1700071B RID: 1819
			// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x0007612D File Offset: 0x0007432D
			// (set) Token: 0x06001EA5 RID: 7845 RVA: 0x00076135 File Offset: 0x00074335
			internal Action<T> callBack { get; set; }

			// Token: 0x06001EA6 RID: 7846 RVA: 0x00076140 File Offset: 0x00074340
			protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				T t = element as T;
				bool flag = t != null;
				if (flag)
				{
					this.callBack(t);
				}
				return false;
			}
		}

		// Token: 0x02000390 RID: 912
		private class DelegateQueryMatcher<TReturnType> : UQuery.UQueryMatcher
		{
			// Token: 0x1700071C RID: 1820
			// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x0007617B File Offset: 0x0007437B
			// (set) Token: 0x06001EA9 RID: 7849 RVA: 0x00076183 File Offset: 0x00074383
			public Func<T, TReturnType> callBack { get; set; }

			// Token: 0x1700071D RID: 1821
			// (get) Token: 0x06001EAA RID: 7850 RVA: 0x0007618C File Offset: 0x0007438C
			// (set) Token: 0x06001EAB RID: 7851 RVA: 0x00076194 File Offset: 0x00074394
			public List<TReturnType> result { get; set; }

			// Token: 0x06001EAC RID: 7852 RVA: 0x000761A0 File Offset: 0x000743A0
			protected override bool OnRuleMatchedElement(RuleMatcher matcher, VisualElement element)
			{
				T t = element as T;
				bool flag = t != null;
				if (flag)
				{
					this.result.Add(this.callBack(t));
				}
				return false;
			}

			// Token: 0x04000CB9 RID: 3257
			public static UQueryState<T>.DelegateQueryMatcher<TReturnType> s_Instance = new UQueryState<T>.DelegateQueryMatcher<TReturnType>();
		}

		// Token: 0x02000391 RID: 913
		public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
		{
			// Token: 0x06001EAF RID: 7855 RVA: 0x000761F4 File Offset: 0x000743F4
			internal Enumerator(UQueryState<T> queryState)
			{
				this.iterationList = VisualElementListPool.Get(0);
				UQueryState<T>.s_EnumerationList.matches = this.iterationList;
				UQueryState<T>.s_EnumerationList.Run(queryState.m_Element, queryState.m_Matchers);
				UQueryState<T>.s_EnumerationList.Reset();
				this.currentIndex = -1;
			}

			// Token: 0x1700071E RID: 1822
			// (get) Token: 0x06001EB0 RID: 7856 RVA: 0x00076248 File Offset: 0x00074448
			public T Current
			{
				get
				{
					return (T)((object)this.iterationList[this.currentIndex]);
				}
			}

			// Token: 0x1700071F RID: 1823
			// (get) Token: 0x06001EB1 RID: 7857 RVA: 0x00076260 File Offset: 0x00074460
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06001EB2 RID: 7858 RVA: 0x00076270 File Offset: 0x00074470
			public bool MoveNext()
			{
				int num = this.currentIndex + 1;
				this.currentIndex = num;
				return num < this.iterationList.Count;
			}

			// Token: 0x06001EB3 RID: 7859 RVA: 0x000762A0 File Offset: 0x000744A0
			public void Reset()
			{
				this.currentIndex = -1;
			}

			// Token: 0x06001EB4 RID: 7860 RVA: 0x000762AA File Offset: 0x000744AA
			public void Dispose()
			{
				VisualElementListPool.Release(this.iterationList);
				this.iterationList = null;
			}

			// Token: 0x04000CBA RID: 3258
			private List<VisualElement> iterationList;

			// Token: 0x04000CBB RID: 3259
			private int currentIndex;
		}
	}
}
