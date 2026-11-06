using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Properties;
using Unity.Serialization.Json.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000084 RID: 132
	public readonly struct SerializedArrayView : ISerializedView, IList<SerializedValueView>, ICollection<SerializedValueView>, IEnumerable<SerializedValueView>, IEnumerable
	{
		// Token: 0x060002F2 RID: 754 RVA: 0x0000DA59 File Offset: 0x0000BC59
		static SerializedArrayView()
		{
			PropertyBag.Register<SerializedArrayView>(new SerializedArrayViewPropertyBag());
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000DA65 File Offset: 0x0000BC65
		internal unsafe SerializedArrayView(UnsafePackedBinaryStream* stream, Handle handle)
		{
			this.m_Stream = stream;
			this.m_Handle = handle;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000DA75 File Offset: 0x0000BC75
		public SerializedArrayView.Enumerator GetEnumerator()
		{
			return new SerializedArrayView.Enumerator(this.m_Stream, this.m_Handle);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000DA88 File Offset: 0x0000BC88
		IEnumerator<SerializedValueView> IEnumerable<SerializedValueView>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000DA88 File Offset: 0x0000BC88
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x00003204 File Offset: 0x00001404
		bool ICollection<SerializedValueView>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000053 RID: 83
		SerializedValueView IList<SerializedValueView>.this[int index]
		{
			get
			{
				using (SerializedArrayView.Enumerator enumerator = this.GetEnumerator())
				{
					int num = -1;
					while (enumerator.MoveNext())
					{
						num++;
						if (num == index)
						{
							return enumerator.Current;
						}
					}
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				throw new NotSupportedException("SerializedArrayView is immutable");
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000DB00 File Offset: 0x0000BD00
		unsafe int ICollection<SerializedValueView>.Count
		{
			get
			{
				int tokenIndex = this.m_Stream->GetTokenIndex(this.m_Handle);
				BinaryToken token = this.m_Stream->GetToken(tokenIndex);
				if (token.Length <= 1)
				{
					return 0;
				}
				int num = 0;
				Handle handle = this.m_Stream->GetFirstChild(this.m_Handle);
				while (this.m_Stream->IsValid(handle))
				{
					num++;
					int tokenIndex2 = this.m_Stream->GetTokenIndex(handle);
					BinaryToken token2 = this.m_Stream->GetToken(tokenIndex2);
					if (tokenIndex2 + token2.Length >= tokenIndex + token.Length)
					{
						return num;
					}
					handle = this.m_Stream->GetHandle(tokenIndex2 + token2.Length);
				}
				return num;
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000DBAC File Offset: 0x0000BDAC
		bool ICollection<SerializedValueView>.Contains(SerializedValueView item)
		{
			if (item.m_Stream != this.m_Stream)
			{
				return false;
			}
			using (SerializedArrayView.Enumerator enumerator = this.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (item.m_Handle.Equals(enumerator.Current.m_Handle))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000DC20 File Offset: 0x0000BE20
		void ICollection<SerializedValueView>.CopyTo(SerializedValueView[] array, int arrayIndex)
		{
			foreach (SerializedValueView serializedValueView in this)
			{
				array[arrayIndex++] = serializedValueView;
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000DC74 File Offset: 0x0000BE74
		int IList<SerializedValueView>.IndexOf(SerializedValueView item)
		{
			int num = -1;
			if (item.m_Stream != this.m_Stream)
			{
				return num;
			}
			using (SerializedArrayView.Enumerator enumerator = this.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					num++;
					if (item.m_Handle.Equals(enumerator.Current.m_Handle))
					{
						return num;
					}
				}
			}
			return -1;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		void ICollection<SerializedValueView>.Clear()
		{
			throw new NotSupportedException("SerializedArrayView is immutable");
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		void IList<SerializedValueView>.Insert(int index, SerializedValueView item)
		{
			throw new NotSupportedException("SerializedArrayView is immutable");
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		void ICollection<SerializedValueView>.Add(SerializedValueView item)
		{
			throw new NotSupportedException("SerializedArrayView is immutable");
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		bool ICollection<SerializedValueView>.Remove(SerializedValueView item)
		{
			throw new NotSupportedException("SerializedArrayView is immutable");
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000DAF4 File Offset: 0x0000BCF4
		void IList<SerializedValueView>.RemoveAt(int index)
		{
			throw new NotSupportedException("SerializedArrayView is immutable");
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000DCEC File Offset: 0x0000BEEC
		internal unsafe UnsafeArrayView AsUnsafe()
		{
			return new UnsafeArrayView(this.m_Stream, this.m_Stream->GetTokenIndex(this.m_Handle));
		}

		// Token: 0x040001AF RID: 431
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001B0 RID: 432
		private readonly Handle m_Handle;

		// Token: 0x02000085 RID: 133
		public struct Enumerator : IEnumerator<SerializedValueView>, IEnumerator, IDisposable
		{
			// Token: 0x06000304 RID: 772 RVA: 0x0000DD0C File Offset: 0x0000BF0C
			internal unsafe Enumerator(UnsafePackedBinaryStream* stream, Handle start)
			{
				this.m_Stream = stream;
				this.m_Start = start;
				this.m_Current = new Handle
				{
					Index = -1,
					Version = -1
				};
			}

			// Token: 0x06000305 RID: 773 RVA: 0x0000DD48 File Offset: 0x0000BF48
			public unsafe bool MoveNext()
			{
				int tokenIndex = this.m_Stream->GetTokenIndex(this.m_Start);
				BinaryToken token = this.m_Stream->GetToken(tokenIndex);
				if (token.Length == 1)
				{
					return false;
				}
				if (this.m_Current.Index == -1)
				{
					this.m_Current = this.m_Stream->GetFirstChild(this.m_Start);
					return true;
				}
				if (!this.m_Stream->IsValid(this.m_Current))
				{
					return false;
				}
				int tokenIndex2 = this.m_Stream->GetTokenIndex(this.m_Current);
				BinaryToken token2 = this.m_Stream->GetToken(tokenIndex2);
				if (tokenIndex2 + token2.Length >= tokenIndex + token.Length)
				{
					return false;
				}
				this.m_Current = this.m_Stream->GetHandle(tokenIndex2 + token2.Length);
				return true;
			}

			// Token: 0x06000306 RID: 774 RVA: 0x0000DE08 File Offset: 0x0000C008
			public void Reset()
			{
				this.m_Current = new Handle
				{
					Index = -1,
					Version = -1
				};
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x06000307 RID: 775 RVA: 0x0000DE34 File Offset: 0x0000C034
			public SerializedValueView Current
			{
				get
				{
					if (this.m_Current.Index < 0)
					{
						throw new InvalidOperationException();
					}
					return new SerializedValueView(this.m_Stream, this.m_Current);
				}
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x06000308 RID: 776 RVA: 0x0000DE5B File Offset: 0x0000C05B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000309 RID: 777 RVA: 0x0000578E File Offset: 0x0000398E
			public void Dispose()
			{
			}

			// Token: 0x040001B1 RID: 433
			private unsafe readonly UnsafePackedBinaryStream* m_Stream;

			// Token: 0x040001B2 RID: 434
			private readonly Handle m_Start;

			// Token: 0x040001B3 RID: 435
			private Handle m_Current;
		}
	}
}
