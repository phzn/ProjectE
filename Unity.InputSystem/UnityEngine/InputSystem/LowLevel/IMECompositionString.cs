using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000E5 RID: 229
	[StructLayout(LayoutKind.Explicit, Size = 132)]
	public struct IMECompositionString : IEnumerable<char>, IEnumerable
	{
		// Token: 0x170003DE RID: 990
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x000451F4 File Offset: 0x000433F4
		public int Count
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x170003DF RID: 991
		public unsafe char this[int index]
		{
			get
			{
				if (index >= this.Count || index < 0)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				fixed (char* ptr = &this.buffer.FixedElementField)
				{
					return ptr[index];
				}
			}
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x00045238 File Offset: 0x00043438
		public unsafe IMECompositionString(string characters)
		{
			if (string.IsNullOrEmpty(characters))
			{
				this.size = 0;
				return;
			}
			this.size = characters.Length;
			for (int i = 0; i < this.size; i++)
			{
				*((ref this.buffer.FixedElementField) + (IntPtr)i * 2) = characters[i];
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0004528C File Offset: 0x0004348C
		public unsafe override string ToString()
		{
			fixed (char* ptr = &this.buffer.FixedElementField)
			{
				return new string(ptr, 0, this.size);
			}
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x000452B3 File Offset: 0x000434B3
		public IEnumerator<char> GetEnumerator()
		{
			return new IMECompositionString.Enumerator(this);
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x000452C5 File Offset: 0x000434C5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x040005AB RID: 1451
		[FieldOffset(0)]
		private int size;

		// Token: 0x040005AC RID: 1452
		[FixedBuffer(typeof(char), 64)]
		[FieldOffset(4)]
		private IMECompositionString.<buffer>e__FixedBuffer buffer;

		// Token: 0x02000214 RID: 532
		internal struct Enumerator : IEnumerator<char>, IEnumerator, IDisposable
		{
			// Token: 0x060014FA RID: 5370 RVA: 0x000621B3 File Offset: 0x000603B3
			public Enumerator(IMECompositionString compositionString)
			{
				this.m_CompositionString = compositionString;
				this.m_CurrentCharacter = '\0';
				this.m_CurrentIndex = -1;
			}

			// Token: 0x060014FB RID: 5371 RVA: 0x000621CC File Offset: 0x000603CC
			public unsafe bool MoveNext()
			{
				int count = this.m_CompositionString.Count;
				this.m_CurrentIndex++;
				if (this.m_CurrentIndex == count)
				{
					return false;
				}
				fixed (char* ptr = &this.m_CompositionString.buffer.FixedElementField)
				{
					char* ptr2 = ptr;
					this.m_CurrentCharacter = ptr2[this.m_CurrentIndex];
				}
				return true;
			}

			// Token: 0x060014FC RID: 5372 RVA: 0x00062228 File Offset: 0x00060428
			public void Reset()
			{
				this.m_CurrentIndex = -1;
			}

			// Token: 0x060014FD RID: 5373 RVA: 0x00062231 File Offset: 0x00060431
			public void Dispose()
			{
			}

			// Token: 0x170005A4 RID: 1444
			// (get) Token: 0x060014FE RID: 5374 RVA: 0x00062233 File Offset: 0x00060433
			public char Current
			{
				get
				{
					return this.m_CurrentCharacter;
				}
			}

			// Token: 0x170005A5 RID: 1445
			// (get) Token: 0x060014FF RID: 5375 RVA: 0x0006223B File Offset: 0x0006043B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x04000B97 RID: 2967
			private IMECompositionString m_CompositionString;

			// Token: 0x04000B98 RID: 2968
			private char m_CurrentCharacter;

			// Token: 0x04000B99 RID: 2969
			private int m_CurrentIndex;
		}

		// Token: 0x02000215 RID: 533
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 128)]
		public struct <buffer>e__FixedBuffer
		{
			// Token: 0x04000B9A RID: 2970
			public char FixedElementField;
		}
	}
}
