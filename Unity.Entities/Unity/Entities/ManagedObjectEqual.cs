using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Properties;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x02000250 RID: 592
	internal class ManagedObjectEqual : IPropertyBagVisitor, IListPropertyBagVisitor, ISetPropertyBagVisitor, IDictionaryPropertyBagVisitor, IPropertyVisitor
	{
		// Token: 0x06000F40 RID: 3904 RVA: 0x00044CB8 File Offset: 0x00042EB8
		public bool CompareEqual(object lhs, object rhs)
		{
			if (lhs == null)
			{
				return rhs == null;
			}
			if (rhs == null)
			{
				return false;
			}
			Type type = lhs.GetType();
			if (typeof(Object).IsAssignableFrom(type))
			{
				return lhs.Equals(rhs);
			}
			IPropertyBag propertyBag = PropertyBag.GetPropertyBag(type);
			if (propertyBag == null)
			{
				throw new MissingPropertyBagException(type);
			}
			Dictionary<object, object> references = this.m_References;
			if (references != null)
			{
				references.Clear();
			}
			this.m_LhsObject = lhs;
			this.m_RhsObject = rhs;
			this.m_Equals = true;
			this.m_Stack = rhs;
			propertyBag.Accept(this, ref lhs);
			return this.m_Equals;
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x00044D40 File Offset: 0x00042F40
		void IPropertyBagVisitor.Visit<TContainer>(IPropertyBag<TContainer> properties, ref TContainer srcContainer)
		{
			foreach (IProperty<TContainer> property in properties.GetProperties(ref srcContainer))
			{
				if (!this.m_Equals)
				{
					break;
				}
				property.Accept(this, ref srcContainer);
			}
		}

		// Token: 0x06000F42 RID: 3906 RVA: 0x00044DA4 File Offset: 0x00042FA4
		void IListPropertyBagVisitor.Visit<TList, TElement>(IListPropertyBag<TList, TElement> properties, ref TList srcContainer)
		{
			TList tlist = (TList)((object)this.m_Stack);
			if (srcContainer.Count != tlist.Count)
			{
				this.m_Equals = false;
				return;
			}
			for (int i = 0; i < srcContainer.Count; i++)
			{
				if (!this.CompareEquality<TElement>(srcContainer[i], tlist[i]))
				{
					this.m_Equals = false;
					return;
				}
			}
		}

		// Token: 0x06000F43 RID: 3907 RVA: 0x00044E24 File Offset: 0x00043024
		void ISetPropertyBagVisitor.Visit<TSet, TElement>(ISetPropertyBag<TSet, TElement> properties, ref TSet srcContainer)
		{
			TSet tset = (TSet)((object)this.m_Stack);
			if (srcContainer.Count != tset.Count)
			{
				this.m_Equals = false;
				return;
			}
			using (IEnumerator<TElement> enumerator = srcContainer.GetEnumerator())
			{
				using (IEnumerator<TElement> enumerator2 = tset.GetEnumerator())
				{
					for (;;)
					{
						bool flag = enumerator.MoveNext();
						bool flag2 = enumerator2.MoveNext();
						if (flag != flag2)
						{
							break;
						}
						if (!flag)
						{
							goto Block_6;
						}
						if (!this.CompareEquality<TElement>(enumerator.Current, enumerator2.Current))
						{
							goto Block_7;
						}
					}
					this.m_Equals = false;
					Block_6:
					return;
					Block_7:
					this.m_Equals = false;
				}
			}
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x00044EEC File Offset: 0x000430EC
		void IDictionaryPropertyBagVisitor.Visit<TDictionary, TKey, TValue>(IDictionaryPropertyBag<TDictionary, TKey, TValue> properties, ref TDictionary srcContainer)
		{
			TDictionary tdictionary = (TDictionary)((object)this.m_Stack);
			if (srcContainer.Count != tdictionary.Count)
			{
				this.m_Equals = false;
				return;
			}
			using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator = srcContainer.GetEnumerator())
			{
				using (IEnumerator<KeyValuePair<TKey, TValue>> enumerator2 = tdictionary.GetEnumerator())
				{
					for (;;)
					{
						bool flag = enumerator.MoveNext();
						bool flag2 = enumerator2.MoveNext();
						if (flag != flag2)
						{
							break;
						}
						if (!flag)
						{
							goto Block_6;
						}
						KeyValuePair<TKey, TValue> keyValuePair = enumerator.Current;
						TKey key = keyValuePair.Key;
						keyValuePair = enumerator2.Current;
						bool flag3 = this.CompareEquality<TKey>(key, keyValuePair.Key);
						keyValuePair = enumerator.Current;
						TKey key2 = keyValuePair.Key;
						keyValuePair = enumerator2.Current;
						bool flag4 = this.CompareEquality<TKey>(key2, keyValuePair.Key);
						if (!flag3 || !flag4)
						{
							goto Block_7;
						}
					}
					this.m_Equals = false;
					Block_6:
					return;
					Block_7:
					this.m_Equals = false;
				}
			}
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x00044FF0 File Offset: 0x000431F0
		void IPropertyVisitor.Visit<TContainer, TValue>(Property<TContainer, TValue> property, ref TContainer srcContainer)
		{
			TContainer tcontainer = (TContainer)((object)this.m_Stack);
			TValue value = property.GetValue(ref srcContainer);
			TValue value2 = property.GetValue(ref tcontainer);
			if (!this.CompareEquality<TValue>(value2, value))
			{
				this.m_Equals = false;
			}
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0004502C File Offset: 0x0004322C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool CompareEquality<TValue>(TValue lhs, TValue rhs)
		{
			if (!TypeTraits<TValue>.IsContainer)
			{
				return EqualityComparer<TValue>.Default.Equals(lhs, rhs);
			}
			if (TypeTraits<TValue>.CanBeNull)
			{
				if (lhs == null)
				{
					return rhs == null;
				}
				if (rhs == null)
				{
					return false;
				}
			}
			if (!TypeTraits<TValue>.IsValueType)
			{
				if (lhs == rhs)
				{
					return true;
				}
				Type type = lhs.GetType();
				if (typeof(Object).IsAssignableFrom(type))
				{
					return EqualityComparer<TValue>.Default.Equals(lhs, rhs);
				}
				if (!TypeTraits.IsContainer(type))
				{
					return EqualityComparer<TValue>.Default.Equals(lhs, rhs);
				}
				if (!type.IsValueType && !(type == typeof(string)))
				{
					if (this.m_LhsObject == lhs)
					{
						return this.m_RhsObject.Equals(rhs);
					}
					object obj;
					if (this.m_References != null && this.m_References.TryGetValue(lhs, out obj))
					{
						return obj.Equals(rhs);
					}
					if (this.m_References == null)
					{
						this.m_References = new Dictionary<object, object>();
					}
					this.m_References.Add(lhs, rhs);
				}
			}
			object stack = this.m_Stack;
			this.m_Stack = rhs;
			VisitReturnCode visitReturnCode;
			PropertyContainer.TryAccept<TValue>(this, ref lhs, out visitReturnCode, default(VisitParameters));
			this.m_Stack = stack;
			return this.m_Equals;
		}

		// Token: 0x0400061B RID: 1563
		private Dictionary<object, object> m_References;

		// Token: 0x0400061C RID: 1564
		private object m_LhsObject;

		// Token: 0x0400061D RID: 1565
		private object m_RhsObject;

		// Token: 0x0400061E RID: 1566
		private object m_Stack;

		// Token: 0x0400061F RID: 1567
		private bool m_Equals;
	}
}
