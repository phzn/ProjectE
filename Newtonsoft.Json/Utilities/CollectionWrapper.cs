using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x02000047 RID: 71
	[NullableContext(1)]
	[Nullable(0)]
	internal class CollectionWrapper<[Nullable(2)] T> : ICollection<T>, IEnumerable<T>, IEnumerable, IWrappedCollection, IList, ICollection
	{
		// Token: 0x06000458 RID: 1112 RVA: 0x0001108C File Offset: 0x0000F28C
		public CollectionWrapper(IList list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			ICollection<T> collection = list as ICollection<T>;
			if (collection != null)
			{
				this._genericCollection = collection;
				return;
			}
			this._list = list;
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x000110C3 File Offset: 0x0000F2C3
		public CollectionWrapper(ICollection<T> list)
		{
			ValidationUtils.ArgumentNotNull(list, "list");
			this._genericCollection = list;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000110DD File Offset: 0x0000F2DD
		public virtual void Add(T item)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Add(item);
				return;
			}
			this._list.Add(item);
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00011106 File Offset: 0x0000F306
		public virtual void Clear()
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.Clear();
				return;
			}
			this._list.Clear();
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00011127 File Offset: 0x0000F327
		public virtual bool Contains(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Contains(item);
			}
			return this._list.Contains(item);
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001114F File Offset: 0x0000F34F
		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			if (this._genericCollection != null)
			{
				this._genericCollection.CopyTo(array, arrayIndex);
				return;
			}
			this._list.CopyTo(array, arrayIndex);
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x00011174 File Offset: 0x0000F374
		public virtual int Count
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.Count;
				}
				return this._list.Count;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x00011195 File Offset: 0x0000F395
		public virtual bool IsReadOnly
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsReadOnly;
			}
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000111B6 File Offset: 0x0000F3B6
		public virtual bool Remove(T item)
		{
			if (this._genericCollection != null)
			{
				return this._genericCollection.Remove(item);
			}
			bool flag = this._list.Contains(item);
			if (flag)
			{
				this._list.Remove(item);
			}
			return flag;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000111F4 File Offset: 0x0000F3F4
		public virtual IEnumerator<T> GetEnumerator()
		{
			IEnumerable<T> genericCollection = this._genericCollection;
			return (genericCollection ?? this._list.Cast<T>()).GetEnumerator();
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00011220 File Offset: 0x0000F420
		IEnumerator IEnumerable.GetEnumerator()
		{
			IEnumerable genericCollection = this._genericCollection;
			return (genericCollection ?? this._list).GetEnumerator();
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00011244 File Offset: 0x0000F444
		[NullableContext(2)]
		int IList.Add(object value)
		{
			CollectionWrapper<T>.VerifyValueType(value);
			this.Add((T)((object)value));
			return this.Count - 1;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00011260 File Offset: 0x0000F460
		[NullableContext(2)]
		bool IList.Contains(object value)
		{
			return CollectionWrapper<T>.IsCompatibleObject(value) && this.Contains((T)((object)value));
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00011278 File Offset: 0x0000F478
		[NullableContext(2)]
		int IList.IndexOf(object value)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support IndexOf.");
			}
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				return this._list.IndexOf((T)((object)value));
			}
			return -1;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x000112AD File Offset: 0x0000F4AD
		void IList.RemoveAt(int index)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support RemoveAt.");
			}
			this._list.RemoveAt(index);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000112CE File Offset: 0x0000F4CE
		[NullableContext(2)]
		void IList.Insert(int index, object value)
		{
			if (this._genericCollection != null)
			{
				throw new InvalidOperationException("Wrapped ICollection<T> does not support Insert.");
			}
			CollectionWrapper<T>.VerifyValueType(value);
			this._list.Insert(index, (T)((object)value));
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00011300 File Offset: 0x0000F500
		bool IList.IsFixedSize
		{
			get
			{
				if (this._genericCollection != null)
				{
					return this._genericCollection.IsReadOnly;
				}
				return this._list.IsFixedSize;
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00011321 File Offset: 0x0000F521
		[NullableContext(2)]
		void IList.Remove(object value)
		{
			if (CollectionWrapper<T>.IsCompatibleObject(value))
			{
				this.Remove((T)((object)value));
			}
		}

		// Token: 0x170000A8 RID: 168
		[Nullable(2)]
		object IList.this[int index]
		{
			[NullableContext(2)]
			get
			{
				if (this._genericCollection != null)
				{
					throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
				}
				return this._list[index];
			}
			[NullableContext(2)]
			set
			{
				if (this._genericCollection != null)
				{
					throw new InvalidOperationException("Wrapped ICollection<T> does not support indexer.");
				}
				CollectionWrapper<T>.VerifyValueType(value);
				this._list[index] = (T)((object)value);
			}
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001138B File Offset: 0x0000F58B
		void ICollection.CopyTo(Array array, int arrayIndex)
		{
			this.CopyTo((T[])array, arrayIndex);
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0001139A File Offset: 0x0000F59A
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0001139D File Offset: 0x0000F59D
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000113BF File Offset: 0x0000F5BF
		[NullableContext(2)]
		private static void VerifyValueType(object value)
		{
			if (!CollectionWrapper<T>.IsCompatibleObject(value))
			{
				throw new ArgumentException("The value '{0}' is not of type '{1}' and cannot be used in this generic collection.".FormatWith(CultureInfo.InvariantCulture, value, typeof(T)), "value");
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000113EE File Offset: 0x0000F5EE
		[NullableContext(2)]
		private static bool IsCompatibleObject(object value)
		{
			return value is T || (value == null && (!typeof(T).IsValueType() || ReflectionUtils.IsNullableType(typeof(T))));
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x00011420 File Offset: 0x0000F620
		public object UnderlyingCollection
		{
			get
			{
				return this._genericCollection ?? this._list;
			}
		}

		// Token: 0x04000174 RID: 372
		[Nullable(2)]
		private readonly IList _list;

		// Token: 0x04000175 RID: 373
		[Nullable(new byte[] { 2, 1 })]
		private readonly ICollection<T> _genericCollection;

		// Token: 0x04000176 RID: 374
		[Nullable(2)]
		private object _syncRoot;
	}
}
