using System;
using System.Collections.Generic;
using Unity.Properties.Internal;

namespace Unity.Properties
{
	// Token: 0x02000050 RID: 80
	public static class PropertyBag
	{
		// Token: 0x06000162 RID: 354 RVA: 0x00005B7C File Offset: 0x00003D7C
		public static void AcceptWithSpecializedVisitor<TContainer>(IPropertyBag<TContainer> properties, IPropertyBagVisitor visitor, ref TContainer container)
		{
			bool flag = properties == null;
			if (flag)
			{
				throw new ArgumentNullException("properties");
			}
			IDictionaryPropertyBagAccept<TContainer> dictionaryPropertyBagAccept = properties as IDictionaryPropertyBagAccept<TContainer>;
			if (dictionaryPropertyBagAccept != null)
			{
				IDictionaryPropertyBagVisitor dictionaryPropertyBagVisitor = visitor as IDictionaryPropertyBagVisitor;
				if (dictionaryPropertyBagVisitor != null)
				{
					dictionaryPropertyBagAccept.Accept(dictionaryPropertyBagVisitor, ref container);
					return;
				}
			}
			IListPropertyBagAccept<TContainer> listPropertyBagAccept = properties as IListPropertyBagAccept<TContainer>;
			if (listPropertyBagAccept != null)
			{
				IListPropertyBagVisitor listPropertyBagVisitor = visitor as IListPropertyBagVisitor;
				if (listPropertyBagVisitor != null)
				{
					listPropertyBagAccept.Accept(listPropertyBagVisitor, ref container);
					return;
				}
			}
			ISetPropertyBagAccept<TContainer> setPropertyBagAccept = properties as ISetPropertyBagAccept<TContainer>;
			if (setPropertyBagAccept != null)
			{
				ISetPropertyBagVisitor setPropertyBagVisitor = visitor as ISetPropertyBagVisitor;
				if (setPropertyBagVisitor != null)
				{
					setPropertyBagAccept.Accept(setPropertyBagVisitor, ref container);
					return;
				}
			}
			ICollectionPropertyBagAccept<TContainer> collectionPropertyBagAccept = properties as ICollectionPropertyBagAccept<TContainer>;
			if (collectionPropertyBagAccept != null)
			{
				ICollectionPropertyBagVisitor collectionPropertyBagVisitor = visitor as ICollectionPropertyBagVisitor;
				if (collectionPropertyBagVisitor != null)
				{
					collectionPropertyBagAccept.Accept(collectionPropertyBagVisitor, ref container);
					return;
				}
			}
			properties.Accept(visitor, ref container);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00005C48 File Offset: 0x00003E48
		public static void Register<TContainer>(PropertyBag<TContainer> propertyBag)
		{
			PropertyBagStore.AddPropertyBag<TContainer>(propertyBag);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005C54 File Offset: 0x00003E54
		public static void RegisterArray<TElement>()
		{
			bool flag = PropertyBagStore.TypedStore<IPropertyBag<TElement[]>>.PropertyBag == null;
			if (flag)
			{
				PropertyBagStore.AddPropertyBag<TElement[]>(new ArrayPropertyBag<TElement>());
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005C7B File Offset: 0x00003E7B
		public static void RegisterArray<TContainer, TElement>()
		{
			PropertyBag.RegisterArray<TElement>();
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005C84 File Offset: 0x00003E84
		public static void RegisterList<TElement>()
		{
			bool flag = PropertyBagStore.TypedStore<IPropertyBag<TElement[]>>.PropertyBag == null;
			if (flag)
			{
				PropertyBagStore.AddPropertyBag<List<TElement>>(new ListPropertyBag<TElement>());
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005CAB File Offset: 0x00003EAB
		public static void RegisterList<TContainer, TElement>()
		{
			PropertyBag.RegisterList<TElement>();
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00005CB4 File Offset: 0x00003EB4
		public static void RegisterHashSet<TElement>()
		{
			bool flag = PropertyBagStore.TypedStore<IPropertyBag<HashSet<TElement>>>.PropertyBag == null;
			if (flag)
			{
				PropertyBagStore.AddPropertyBag<HashSet<TElement>>(new HashSetPropertyBag<TElement>());
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005CDB File Offset: 0x00003EDB
		public static void RegisterHashSet<TContainer, TElement>()
		{
			PropertyBag.RegisterHashSet<TElement>();
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00005CE4 File Offset: 0x00003EE4
		public static void RegisterDictionary<TKey, TValue>()
		{
			bool flag = PropertyBagStore.TypedStore<IPropertyBag<Dictionary<TKey, TValue>>>.PropertyBag == null;
			if (flag)
			{
				PropertyBagStore.AddPropertyBag<Dictionary<TKey, TValue>>(new DictionaryPropertyBag<TKey, TValue>());
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005D0B File Offset: 0x00003F0B
		public static void RegisterDictionary<TContainer, TKey, TValue>()
		{
			PropertyBag.RegisterDictionary<TKey, TValue>();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00005D14 File Offset: 0x00003F14
		public static void RegisterIList<TList, TElement>() where TList : IList<TElement>
		{
			bool flag = PropertyBagStore.TypedStore<IPropertyBag<TList>>.PropertyBag == null;
			if (flag)
			{
				PropertyBagStore.AddPropertyBag<TList>(new IndexedCollectionPropertyBag<TList, TElement>());
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00005D3B File Offset: 0x00003F3B
		public static void RegisterIList<TContainer, TList, TElement>() where TList : IList<TElement>
		{
			PropertyBag.RegisterIList<TList, TElement>();
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00005D44 File Offset: 0x00003F44
		public static void RegisterISet<TSet, TElement>() where TSet : ISet<TElement>
		{
			bool flag = PropertyBagStore.TypedStore<IPropertyBag<TSet>>.PropertyBag == null;
			if (flag)
			{
				PropertyBagStore.AddPropertyBag<TSet>(new SetPropertyBagBase<TSet, TElement>());
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00005D6B File Offset: 0x00003F6B
		public static void RegisterISet<TContainer, TSet, TElement>() where TSet : ISet<TElement>
		{
			PropertyBag.RegisterISet<TSet, TElement>();
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005D74 File Offset: 0x00003F74
		public static void RegisterIDictionary<TDictionary, TKey, TValue>() where TDictionary : IDictionary<TKey, TValue>
		{
			bool flag = PropertyBagStore.TypedStore<IPropertyBag<TDictionary>>.PropertyBag == null;
			if (flag)
			{
				PropertyBagStore.AddPropertyBag<TDictionary>(new KeyValueCollectionPropertyBag<TDictionary, TKey, TValue>());
				PropertyBagStore.AddPropertyBag<KeyValuePair<TKey, TValue>>(new KeyValuePairPropertyBag<TKey, TValue>());
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005DA6 File Offset: 0x00003FA6
		public static void RegisterIDictionary<TContainer, TDictionary, TKey, TValue>() where TDictionary : IDictionary<TKey, TValue>
		{
			PropertyBag.RegisterIDictionary<TDictionary, TKey, TValue>();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00005DB0 File Offset: 0x00003FB0
		public static TContainer CreateInstance<TContainer>()
		{
			IPropertyBag<TContainer> propertyBag = PropertyBagStore.GetPropertyBag<TContainer>();
			bool flag = propertyBag == null;
			if (flag)
			{
				throw new MissingPropertyBagException(typeof(TContainer));
			}
			return propertyBag.CreateInstance();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00005DE8 File Offset: 0x00003FE8
		public static IPropertyBag GetPropertyBag(Type type)
		{
			return PropertyBagStore.GetPropertyBag(type);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00005E00 File Offset: 0x00004000
		public static IPropertyBag<TContainer> GetPropertyBag<TContainer>()
		{
			return PropertyBagStore.GetPropertyBag<TContainer>();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00005E18 File Offset: 0x00004018
		public static bool TryGetPropertyBagForValue<TValue>(ref TValue value, out IPropertyBag propertyBag)
		{
			return PropertyBagStore.TryGetPropertyBagForValue<TValue>(ref value, out propertyBag);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00005E34 File Offset: 0x00004034
		public static bool Exists<TContainer>()
		{
			return PropertyBagStore.Exists<TContainer>();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00005E4C File Offset: 0x0000404C
		public static bool Exists(Type type)
		{
			return PropertyBagStore.Exists(type);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00005E64 File Offset: 0x00004064
		public static IEnumerable<Type> GetAllTypesWithAPropertyBag()
		{
			return PropertyBagStore.AllTypes;
		}
	}
}
