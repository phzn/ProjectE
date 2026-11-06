using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;

namespace Unity.Serialization.Json
{
	// Token: 0x02000087 RID: 135
	public readonly struct SerializedMemberViewCollection : IDisposable, IEnumerable<SerializedMemberView>, IEnumerable
	{
		// Token: 0x0600030E RID: 782 RVA: 0x0000DEC7 File Offset: 0x0000C0C7
		public SerializedMemberViewCollection(Allocator label)
		{
			this.m_Members = new NativeList<SerializedMemberView>(label);
		}

		// Token: 0x17000057 RID: 87
		public SerializedValueView this[string name]
		{
			get
			{
				return this.GetValue(name);
			}
		}

		// Token: 0x17000058 RID: 88
		public SerializedValueView this[FixedString32Bytes name]
		{
			get
			{
				return this.GetValue<FixedString32Bytes>(name);
			}
		}

		// Token: 0x17000059 RID: 89
		public SerializedValueView this[FixedString64Bytes name]
		{
			get
			{
				return this.GetValue<FixedString64Bytes>(name);
			}
		}

		// Token: 0x1700005A RID: 90
		public SerializedValueView this[FixedString128Bytes name]
		{
			get
			{
				return this.GetValue<FixedString128Bytes>(name);
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000DF00 File Offset: 0x0000C100
		public SerializedValueView GetValue(string name)
		{
			SerializedValueView serializedValueView;
			if (!this.TryGetValue(name, out serializedValueView))
			{
				throw new KeyNotFoundException(name);
			}
			return serializedValueView;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000DF20 File Offset: 0x0000C120
		public bool TryGetValue(string name, out SerializedValueView value)
		{
			foreach (SerializedMemberView serializedMemberView in this)
			{
				if (serializedMemberView.Name().Equals(name))
				{
					value = serializedMemberView.Value();
					return true;
				}
			}
			value = default(SerializedValueView);
			return false;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000DF94 File Offset: 0x0000C194
		public SerializedValueView GetValue<[IsUnmanaged] T>(T name) where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			SerializedValueView serializedValueView;
			if (!this.TryGetValue<T>(name, out serializedValueView))
			{
				throw new KeyNotFoundException(string.Format("The Key=[\"{0}\"] could not be found in the SerializedObjectView.", name));
			}
			return serializedValueView;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000DFC4 File Offset: 0x0000C1C4
		public bool TryGetValue<[IsUnmanaged] T>(T name, out SerializedValueView value) where T : struct, ValueType, INativeList<byte>, IUTF8Bytes
		{
			foreach (SerializedMemberView serializedMemberView in this)
			{
				if (serializedMemberView.Name().Equals<T>(name))
				{
					value = serializedMemberView.Value();
					return true;
				}
			}
			value = default(SerializedValueView);
			return false;
		}

		// Token: 0x06000317 RID: 791 RVA: 0x0000E038 File Offset: 0x0000C238
		public void Add(SerializedMemberView view)
		{
			this.m_Members.Add(in view);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000E055 File Offset: 0x0000C255
		public SerializedMemberViewCollection.Enumerator GetEnumerator()
		{
			return new SerializedMemberViewCollection.Enumerator(this);
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0000E062 File Offset: 0x0000C262
		IEnumerator<SerializedMemberView> IEnumerable<SerializedMemberView>.GetEnumerator()
		{
			return new SerializedMemberViewCollection.Enumerator(this);
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000E074 File Offset: 0x0000C274
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0000E084 File Offset: 0x0000C284
		public void Dispose()
		{
			this.m_Members.Dispose();
		}

		// Token: 0x040001B6 RID: 438
		private readonly NativeList<SerializedMemberView> m_Members;

		// Token: 0x02000088 RID: 136
		public struct Enumerator : IEnumerator<SerializedMemberView>, IEnumerator, IDisposable
		{
			// Token: 0x0600031C RID: 796 RVA: 0x0000E09F File Offset: 0x0000C29F
			internal Enumerator(SerializedMemberViewCollection collection)
			{
				this.m_Collection = collection;
				this.m_Index = -1;
			}

			// Token: 0x0600031D RID: 797 RVA: 0x0000E0AF File Offset: 0x0000C2AF
			public bool MoveNext()
			{
				this.m_Index++;
				return this.m_Index < this.m_Collection.m_Members.Length;
			}

			// Token: 0x0600031E RID: 798 RVA: 0x0000E0D7 File Offset: 0x0000C2D7
			public void Reset()
			{
				this.m_Index = -1;
			}

			// Token: 0x1700005B RID: 91
			// (get) Token: 0x0600031F RID: 799 RVA: 0x0000E0E0 File Offset: 0x0000C2E0
			public SerializedMemberView Current
			{
				get
				{
					if (this.m_Index < 0)
					{
						throw new InvalidOperationException();
					}
					return this.m_Collection.m_Members[this.m_Index];
				}
			}

			// Token: 0x1700005C RID: 92
			// (get) Token: 0x06000320 RID: 800 RVA: 0x0000E115 File Offset: 0x0000C315
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000321 RID: 801 RVA: 0x0000578E File Offset: 0x0000398E
			public void Dispose()
			{
			}

			// Token: 0x040001B7 RID: 439
			private readonly SerializedMemberViewCollection m_Collection;

			// Token: 0x040001B8 RID: 440
			private int m_Index;
		}
	}
}
