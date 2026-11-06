using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000074 RID: 116
	internal struct JsonTypeStack : IDisposable
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x0000BB02 File Offset: 0x00009D02
		public unsafe JsonTypeStack(int length, Allocator label)
		{
			this.m_Label = label;
			this.m_Stack = (JsonType*)UnsafeUtility.Malloc((long)(length * 4), UnsafeUtility.AlignOf<JsonType>(), label);
			this.m_Length = length;
			this.m_Position = -1;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000BB30 File Offset: 0x00009D30
		public unsafe void Push(JsonType type)
		{
			if (this.m_Position + 1 >= this.m_Length)
			{
				this.Resize(this.m_Length * 2);
			}
			ref int stack = ref *(int*)this.m_Stack;
			int num = this.m_Position + 1;
			this.m_Position = num;
			*((ref stack) + (IntPtr)num * 4) = (int)type;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000BB78 File Offset: 0x00009D78
		public void Pop()
		{
			this.m_Position--;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000BB88 File Offset: 0x00009D88
		public unsafe JsonType Peek()
		{
			if (this.m_Position >= 0)
			{
				return this.m_Stack[this.m_Position];
			}
			return JsonType.Undefined;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000BBA8 File Offset: 0x00009DA8
		public unsafe JsonType Peek(int offset)
		{
			int num = this.m_Position - offset;
			if (num >= 0)
			{
				return this.m_Stack[num];
			}
			return JsonType.Undefined;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000BBD0 File Offset: 0x00009DD0
		public void Clear()
		{
			this.m_Position = -1;
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000BBDC File Offset: 0x00009DDC
		private unsafe void Resize(int length)
		{
			void* ptr = UnsafeUtility.Malloc((long)(length * 4), UnsafeUtility.AlignOf<JsonType>(), this.m_Label);
			UnsafeUtility.MemCpy(ptr, (void*)this.m_Stack, (long)(this.m_Length * 4));
			UnsafeUtility.Free((void*)this.m_Stack, this.m_Label);
			this.m_Stack = (JsonType*)ptr;
			this.m_Length = length;
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000BC32 File Offset: 0x00009E32
		public unsafe void Dispose()
		{
			UnsafeUtility.Free((void*)this.m_Stack, this.m_Label);
			this.m_Stack = null;
		}

		// Token: 0x0400016D RID: 365
		private readonly Allocator m_Label;

		// Token: 0x0400016E RID: 366
		[NativeDisableUnsafePtrRestriction]
		private unsafe JsonType* m_Stack;

		// Token: 0x0400016F RID: 367
		private int m_Length;

		// Token: 0x04000170 RID: 368
		private int m_Position;
	}
}
