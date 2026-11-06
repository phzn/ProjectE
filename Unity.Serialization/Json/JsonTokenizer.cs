using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000042 RID: 66
	internal struct JsonTokenizer : IDisposable
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00006190 File Offset: 0x00004390
		public unsafe int ResultCode
		{
			get
			{
				return this.m_Data->ResultCode;
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x000061A0 File Offset: 0x000043A0
		public unsafe JsonTokenizer(Allocator label = Allocator.TempJob)
		{
			this.m_Label = label;
			this.m_Data = (UnsafeJsonTokenizer*)UnsafeUtility.Malloc((long)sizeof(UnsafeJsonTokenizer), UnsafeUtility.AlignOf<UnsafeJsonTokenizer>(), label);
			UnsafeUtility.MemClear((void*)this.m_Data, (long)sizeof(UnsafeJsonTokenizer));
			*this.m_Data = new UnsafeJsonTokenizer(label);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000061EF File Offset: 0x000043EF
		public unsafe void Dispose()
		{
			if (null == this.m_Data)
			{
				return;
			}
			this.m_Data->Dispose();
			UnsafeUtility.Free((void*)this.m_Data, this.m_Label);
			this.m_Data = null;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00006220 File Offset: 0x00004420
		public unsafe readonly void Reset()
		{
			this.m_Data->Reset();
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000622D File Offset: 0x0000442D
		public unsafe readonly int Write(JsonTokenStream stream, UnsafeBuffer<char> buffer, int start, int count, bool isFinalBlock = false)
		{
			if (start + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.m_Data->Write(stream.GetUnsafePtr(), (ushort*)buffer.Buffer, start, count, isFinalBlock);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000625E File Offset: 0x0000445E
		public unsafe void CheckAndThrowInvalidJsonExceptions()
		{
			if (this.m_Data->ResultCode == -1)
			{
				throw new InvalidJsonException("Input json was structurally invalid. Try with JsonValidationType=[Standard or Simple]")
				{
					Line = -1,
					Character = -1
				};
			}
		}

		// Token: 0x04000099 RID: 153
		internal const int ResultSuccess = 0;

		// Token: 0x0400009A RID: 154
		internal const int ResultInvalidInput = -1;

		// Token: 0x0400009B RID: 155
		internal const int ResultEndOfStream = -2;

		// Token: 0x0400009C RID: 156
		private readonly Allocator m_Label;

		// Token: 0x0400009D RID: 157
		[NativeDisableUnsafePtrRestriction]
		private unsafe UnsafeJsonTokenizer* m_Data;
	}
}
