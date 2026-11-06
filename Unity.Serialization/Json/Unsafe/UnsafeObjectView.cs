using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json.Unsafe
{
	// Token: 0x02000091 RID: 145
	internal readonly struct UnsafeObjectView : ISerializedView, IEnumerable<UnsafeMemberView>, IEnumerable
	{
		// Token: 0x0600038B RID: 907 RVA: 0x0000F2CF File Offset: 0x0000D4CF
		internal unsafe UnsafeObjectView(UnsafePackedBinaryStream* stream, int tokenIndex)
		{
			this.m_Stream = stream;
			this.m_TokenIndex = tokenIndex;
		}

		// Token: 0x1700006B RID: 107
		public UnsafeValueView this[string name]
		{
			get
			{
				UnsafeValueView unsafeValueView;
				if (this.TryGetValue(name, out unsafeValueView))
				{
					return unsafeValueView;
				}
				throw new KeyNotFoundException();
			}
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0000F300 File Offset: 0x0000D500
		public bool TryGetMember(string name, out UnsafeMemberView member)
		{
			foreach (UnsafeMemberView unsafeMemberView in this)
			{
				if (unsafeMemberView.Key().AsStringView().Equals(name))
				{
					member = unsafeMemberView;
					return true;
				}
			}
			member = default(UnsafeMemberView);
			return false;
		}

		// Token: 0x0600038E RID: 910 RVA: 0x0000F378 File Offset: 0x0000D578
		public unsafe bool TryGetValue(string name, out UnsafeValueView value)
		{
			UnsafeView* ptr;
			int i;
			checked
			{
				ptr = stackalloc UnsafeView[unchecked((UIntPtr)1) * (UIntPtr)sizeof(UnsafeView)];
				ptr->Stream = this.m_Stream;
				i = this.m_Stream->GetFirstChildIndex(this.m_TokenIndex);
			}
			int num = this.m_TokenIndex + this.m_Stream->GetToken(this.m_TokenIndex).Length;
			while (i < num)
			{
				ptr->TokenIndex = i;
				if (((UnsafeStringView*)ptr)->Equals(name))
				{
					ptr->TokenIndex = this.m_Stream->GetFirstChildIndex(i);
					value = *(UnsafeValueView*)ptr;
					return true;
				}
				i += this.m_Stream->GetToken(i).Length;
			}
			value = default(UnsafeValueView);
			return false;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x0000F420 File Offset: 0x0000D620
		public unsafe int Count()
		{
			int num = 0;
			int length = this.m_Stream->GetToken(this.m_TokenIndex).Length;
			if (length == 1)
			{
				return 0;
			}
			int i = this.m_Stream->GetFirstChildIndex(this.m_TokenIndex);
			int num2 = this.m_TokenIndex + length;
			while (i < num2)
			{
				i += this.m_Stream->GetToken(i).Length;
				num++;
			}
			return num;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0000F486 File Offset: 0x0000D686
		public UnsafeObjectView.Enumerator GetEnumerator()
		{
			return new UnsafeObjectView.Enumerator(this.m_Stream, this.m_TokenIndex);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0000F499 File Offset: 0x0000D699
		IEnumerator<UnsafeMemberView> IEnumerable<UnsafeMemberView>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0000F499 File Offset: 0x0000D699
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000393 RID: 915 RVA: 0x0000F4A6 File Offset: 0x0000D6A6
		public UnsafeValueView AsValue()
		{
			return new UnsafeValueView(this.m_Stream, this.m_TokenIndex);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0000F4B9 File Offset: 0x0000D6B9
		public unsafe SerializedObjectView AsSafe()
		{
			return new SerializedObjectView(this.m_Stream, this.m_Stream->GetHandle(this.m_TokenIndex));
		}

		// Token: 0x040001CC RID: 460
		[NativeDisableUnsafePtrRestriction]
		private unsafe readonly UnsafePackedBinaryStream* m_Stream;

		// Token: 0x040001CD RID: 461
		private readonly int m_TokenIndex;

		// Token: 0x02000092 RID: 146
		public struct Enumerator : IEnumerator<UnsafeMemberView>, IEnumerator, IDisposable
		{
			// Token: 0x06000395 RID: 917 RVA: 0x0000F4D7 File Offset: 0x0000D6D7
			internal unsafe Enumerator(UnsafePackedBinaryStream* stream, int index)
			{
				this.m_Stream = stream;
				this.m_Start = index;
				this.m_End = index + this.m_Stream->GetToken(index).Length;
				this.m_Position = index;
			}

			// Token: 0x06000396 RID: 918 RVA: 0x0000F508 File Offset: 0x0000D708
			public unsafe bool MoveNext()
			{
				if (this.m_Position >= this.m_End)
				{
					throw new InvalidOperationException();
				}
				int length = this.m_Stream->GetToken(this.m_Position).Length;
				if (this.m_Position == this.m_Start)
				{
					if (length == 1)
					{
						return false;
					}
					this.m_Position = this.m_Stream->GetFirstChildIndex(this.m_Position);
					return true;
				}
				else
				{
					if (this.m_Position + length < this.m_End)
					{
						this.m_Position += length;
						return true;
					}
					return false;
				}
			}

			// Token: 0x1700006C RID: 108
			// (get) Token: 0x06000397 RID: 919 RVA: 0x0000F58E File Offset: 0x0000D78E
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x1700006D RID: 109
			// (get) Token: 0x06000398 RID: 920 RVA: 0x0000F59B File Offset: 0x0000D79B
			public UnsafeMemberView Current
			{
				get
				{
					return new UnsafeMemberView(this.m_Stream, this.m_Position);
				}
			}

			// Token: 0x06000399 RID: 921 RVA: 0x0000F5AE File Offset: 0x0000D7AE
			public void Reset()
			{
				this.m_Position = this.m_Start;
			}

			// Token: 0x0600039A RID: 922 RVA: 0x0000578E File Offset: 0x0000398E
			public void Dispose()
			{
			}

			// Token: 0x040001CE RID: 462
			private unsafe readonly UnsafePackedBinaryStream* m_Stream;

			// Token: 0x040001CF RID: 463
			private readonly int m_Start;

			// Token: 0x040001D0 RID: 464
			private readonly int m_End;

			// Token: 0x040001D1 RID: 465
			private int m_Position;
		}
	}
}
