using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000394 RID: 916
	public static class UQueryExtensions
	{
		// Token: 0x06001EEE RID: 7918 RVA: 0x00076DEC File Offset: 0x00074FEC
		public static T Q<T>(this VisualElement e, string name = null, params string[] classes) where T : VisualElement
		{
			return e.Query<T>(name, classes).Build().First();
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x00076E18 File Offset: 0x00075018
		public static VisualElement Q(this VisualElement e, string name = null, params string[] classes)
		{
			return e.Query<VisualElement>(name, classes).Build().First();
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x00076E44 File Offset: 0x00075044
		public static T Q<T>(this VisualElement e, string name = null, string className = null) where T : VisualElement
		{
			bool flag = e == null;
			if (flag)
			{
				throw new ArgumentNullException("e");
			}
			bool flag2 = typeof(T) == typeof(VisualElement);
			T t;
			if (flag2)
			{
				t = e.Q(name, className) as T;
			}
			else
			{
				bool flag3 = name == null;
				if (flag3)
				{
					bool flag4 = className == null;
					if (flag4)
					{
						UQueryState<VisualElement> uqueryState = UQueryExtensions.SingleElementTypeQuery.RebuildOn(e);
						uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance);
						t = uqueryState.First() as T;
					}
					else
					{
						UQueryState<VisualElement> uqueryState = UQueryExtensions.SingleElementTypeAndClassQuery.RebuildOn(e);
						uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance);
						uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[1] = StyleSelectorPart.CreateClass(className);
						t = uqueryState.First() as T;
					}
				}
				else
				{
					bool flag5 = className == null;
					if (flag5)
					{
						UQueryState<VisualElement> uqueryState = UQueryExtensions.SingleElementTypeAndNameQuery.RebuildOn(e);
						uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance);
						uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[1] = StyleSelectorPart.CreateId(name);
						t = uqueryState.First() as T;
					}
					else
					{
						UQueryState<VisualElement> uqueryState = UQueryExtensions.SingleElementTypeAndNameAndClassQuery.RebuildOn(e);
						uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreatePredicate(UQuery.IsOfType<T>.s_Instance);
						uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[1] = StyleSelectorPart.CreateId(name);
						uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[2] = StyleSelectorPart.CreateClass(className);
						t = uqueryState.First() as T;
					}
				}
			}
			return t;
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x000770A4 File Offset: 0x000752A4
		internal static T MandatoryQ<T>(this VisualElement e, string name, string className = null) where T : VisualElement
		{
			T t = e.Q<T>(name, className);
			bool flag = t == null;
			if (flag)
			{
				throw new UQueryExtensions.MissingVisualElementException("Element not found: " + name);
			}
			return t;
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x000770E0 File Offset: 0x000752E0
		public static VisualElement Q(this VisualElement e, string name = null, string className = null)
		{
			bool flag = e == null;
			if (flag)
			{
				throw new ArgumentNullException("e");
			}
			bool flag2 = name == null;
			VisualElement visualElement;
			if (flag2)
			{
				bool flag3 = className == null;
				if (flag3)
				{
					visualElement = UQueryExtensions.SingleElementEmptyQuery.RebuildOn(e).First();
				}
				else
				{
					UQueryState<VisualElement> uqueryState = UQueryExtensions.SingleElementClassQuery.RebuildOn(e);
					uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreateClass(className);
					visualElement = uqueryState.First();
				}
			}
			else
			{
				bool flag4 = className == null;
				if (flag4)
				{
					UQueryState<VisualElement> uqueryState = UQueryExtensions.SingleElementNameQuery.RebuildOn(e);
					uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreateId(name);
					visualElement = uqueryState.First();
				}
				else
				{
					UQueryState<VisualElement> uqueryState = UQueryExtensions.SingleElementNameAndClassQuery.RebuildOn(e);
					uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[0] = StyleSelectorPart.CreateId(name);
					uqueryState.m_Matchers[0].complexSelector.selectors[0].parts[1] = StyleSelectorPart.CreateClass(className);
					visualElement = uqueryState.First();
				}
			}
			return visualElement;
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x00077228 File Offset: 0x00075428
		internal static VisualElement MandatoryQ(this VisualElement e, string name, string className = null)
		{
			VisualElement visualElement = e.Q<VisualElement>(name, className);
			bool flag = visualElement == null;
			if (flag)
			{
				throw new UQueryExtensions.MissingVisualElementException("Element not found: " + name);
			}
			return visualElement;
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x00077260 File Offset: 0x00075460
		public static UQueryBuilder<VisualElement> Query(this VisualElement e, string name = null, params string[] classes)
		{
			return e.Query<VisualElement>(name, classes);
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0007727C File Offset: 0x0007547C
		public static UQueryBuilder<VisualElement> Query(this VisualElement e, string name = null, string className = null)
		{
			return e.Query<VisualElement>(name, className);
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x00077298 File Offset: 0x00075498
		public static UQueryBuilder<T> Query<T>(this VisualElement e, string name = null, params string[] classes) where T : VisualElement
		{
			bool flag = e == null;
			if (flag)
			{
				throw new ArgumentNullException("e");
			}
			return new UQueryBuilder<VisualElement>(e).OfType<T>(name, classes);
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x000772D0 File Offset: 0x000754D0
		public static UQueryBuilder<T> Query<T>(this VisualElement e, string name = null, string className = null) where T : VisualElement
		{
			bool flag = e == null;
			if (flag)
			{
				throw new ArgumentNullException("e");
			}
			return new UQueryBuilder<VisualElement>(e).OfType<T>(name, className);
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x00077308 File Offset: 0x00075508
		public static UQueryBuilder<VisualElement> Query(this VisualElement e)
		{
			bool flag = e == null;
			if (flag)
			{
				throw new ArgumentNullException("e");
			}
			return new UQueryBuilder<VisualElement>(e);
		}

		// Token: 0x04000CC6 RID: 3270
		private static UQueryState<VisualElement> SingleElementEmptyQuery = new UQueryBuilder<VisualElement>(null).Build();

		// Token: 0x04000CC7 RID: 3271
		private static UQueryState<VisualElement> SingleElementNameQuery = new UQueryBuilder<VisualElement>(null).Name(string.Empty).Build();

		// Token: 0x04000CC8 RID: 3272
		private static UQueryState<VisualElement> SingleElementClassQuery = new UQueryBuilder<VisualElement>(null).Class(string.Empty).Build();

		// Token: 0x04000CC9 RID: 3273
		private static UQueryState<VisualElement> SingleElementNameAndClassQuery = new UQueryBuilder<VisualElement>(null).Name(string.Empty).Class(string.Empty).Build();

		// Token: 0x04000CCA RID: 3274
		private static UQueryState<VisualElement> SingleElementTypeQuery = new UQueryBuilder<VisualElement>(null).SingleBaseType().Build();

		// Token: 0x04000CCB RID: 3275
		private static UQueryState<VisualElement> SingleElementTypeAndNameQuery = new UQueryBuilder<VisualElement>(null).SingleBaseType().Name(string.Empty).Build();

		// Token: 0x04000CCC RID: 3276
		private static UQueryState<VisualElement> SingleElementTypeAndClassQuery = new UQueryBuilder<VisualElement>(null).SingleBaseType().Class(string.Empty).Build();

		// Token: 0x04000CCD RID: 3277
		private static UQueryState<VisualElement> SingleElementTypeAndNameAndClassQuery = new UQueryBuilder<VisualElement>(null).SingleBaseType().Name(string.Empty).Class(string.Empty)
			.Build();

		// Token: 0x02000395 RID: 917
		private class MissingVisualElementException : Exception
		{
			// Token: 0x06001EFA RID: 7930 RVA: 0x00077461 File Offset: 0x00075661
			public MissingVisualElementException()
			{
			}

			// Token: 0x06001EFB RID: 7931 RVA: 0x0007746B File Offset: 0x0007566B
			public MissingVisualElementException(string message)
				: base(message)
			{
			}
		}
	}
}
