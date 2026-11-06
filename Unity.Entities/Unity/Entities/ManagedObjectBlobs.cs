using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Properties;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x0200024E RID: 590
	internal class ManagedObjectBlobs : IPropertyBagVisitor, IPropertyVisitor, ISetPropertyBagVisitor, IListPropertyBagVisitor, IDictionaryPropertyBagVisitor
	{
		// Token: 0x06000F2F RID: 3887 RVA: 0x000442CC File Offset: 0x000424CC
		public void GatherBlobAssetReferences(object obj, NativeList<BlobAssetPtr> blobAssets, NativeParallelHashMap<BlobAssetPtr, int> blobAssetMap)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			Type type = obj.GetType();
			IPropertyBag propertyBag = PropertyBag.GetPropertyBag(type);
			if (propertyBag == null)
			{
				throw new MissingPropertyBagException(type);
			}
			HashSet<object> references = this.m_References;
			if (references != null)
			{
				references.Clear();
			}
			this.m_BlobAssets = blobAssets;
			this.m_BlobAssetMap = blobAssetMap;
			propertyBag.Accept(this, ref obj);
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x00044328 File Offset: 0x00042528
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer container)
		{
			foreach (IProperty<TContainer> property in properties.GetProperties(ref container))
			{
				property.Accept(this, ref container);
			}
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00044380 File Offset: 0x00042580
		void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer container)
		{
			if (TypeTraits<TValue>.IsContainer)
			{
				this.VisitValue<TValue>(property.GetValue(ref container));
			}
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00044398 File Offset: 0x00042598
		void ISetPropertyBagVisitor.Visit<TSet, TElement>(ISetPropertyBag<TSet, TElement> properties, ref TSet container)
		{
			HashSet<TElement> hashSet = container as HashSet<TElement>;
			if (hashSet != null)
			{
				using (HashSet<TElement>.Enumerator enumerator = hashSet.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TElement telement = enumerator.Current;
						this.VisitValue<TElement>(telement);
					}
					return;
				}
			}
			foreach (TElement telement2 in container)
			{
				this.VisitValue<TElement>(telement2);
			}
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0004443C File Offset: 0x0004263C
		void IListPropertyBagVisitor.Visit<TList, TElement>(IListPropertyBag<TList, TElement> properties, ref TList container)
		{
			List<TElement> list = container as List<TElement>;
			if (list != null)
			{
				using (List<TElement>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TElement telement = enumerator.Current;
						this.VisitValue<TElement>(telement);
					}
					return;
				}
			}
			TElement[] array = container as TElement[];
			if (array != null)
			{
				foreach (TElement telement2 in array)
				{
					this.VisitValue<TElement>(telement2);
				}
				return;
			}
			foreach (TElement telement3 in container)
			{
				this.VisitValue<TElement>(telement3);
			}
		}

		// Token: 0x06000F34 RID: 3892 RVA: 0x00044524 File Offset: 0x00042724
		void IDictionaryPropertyBagVisitor.Visit<TDictionary, TKey, TValue>(IDictionaryPropertyBag<TDictionary, TKey, TValue> properties, ref TDictionary container)
		{
			Dictionary<TKey, TValue> dictionary = container as Dictionary<TKey, TValue>;
			if (dictionary != null)
			{
				using (Dictionary<TKey, TValue>.Enumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
						this.VisitValue<TKey>(keyValuePair.Key);
						this.VisitValue<TValue>(keyValuePair.Value);
					}
					return;
				}
			}
			foreach (KeyValuePair<TKey, TValue> keyValuePair2 in container)
			{
				this.VisitValue<TKey>(keyValuePair2.Key);
				this.VisitValue<TValue>(keyValuePair2.Value);
			}
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x000445EC File Offset: 0x000427EC
		private unsafe void VisitValue<TValue>(TValue value)
		{
			if (typeof(TValue) == typeof(BlobAssetReferenceData))
			{
				BlobAssetReferenceData blobAssetReferenceData = *UnsafeUtility.As<TValue, BlobAssetReferenceData>(ref value);
				if (null != blobAssetReferenceData.m_Ptr)
				{
					BlobAssetPtr blobAssetPtr = new BlobAssetPtr(blobAssetReferenceData.Header);
					int num;
					if (!this.m_BlobAssetMap.TryGetValue(blobAssetPtr, out num))
					{
						int length = this.m_BlobAssets.Length;
						this.m_BlobAssets.Add(in blobAssetPtr);
						this.m_BlobAssetMap.Add(blobAssetPtr, length);
					}
				}
				return;
			}
			if (!TypeTraits<TValue>.IsContainer)
			{
				return;
			}
			if (TypeTraits<TValue>.CanBeNull)
			{
				if (value == null)
				{
					return;
				}
				if (value is Object)
				{
					return;
				}
			}
			if (!TypeTraits<TValue>.IsValueType && typeof(string) != typeof(TValue))
			{
				if (this.m_References == null)
				{
					this.m_References = new HashSet<object>();
				}
				if (!this.m_References.Add(value))
				{
					return;
				}
			}
			VisitReturnCode visitReturnCode;
			PropertyContainer.TryAccept<TValue>(this, ref value, out visitReturnCode, default(VisitParameters));
		}

		// Token: 0x04000614 RID: 1556
		private HashSet<object> m_References;

		// Token: 0x04000615 RID: 1557
		private NativeList<BlobAssetPtr> m_BlobAssets;

		// Token: 0x04000616 RID: 1558
		private NativeParallelHashMap<BlobAssetPtr, int> m_BlobAssetMap;
	}
}
