using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Serialization.Json
{
	// Token: 0x02000049 RID: 73
	internal struct PackedBinaryWriter : IDisposable
	{
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00007648 File Offset: 0x00005848
		public unsafe int TokenNextIndex
		{
			get
			{
				return this.m_Data->JsonTokenNextIndex;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x00007655 File Offset: 0x00005855
		public unsafe int TokenParentIndex
		{
			get
			{
				return this.m_Data->JsonTokenParentIndex;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00007662 File Offset: 0x00005862
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0000766F File Offset: 0x0000586F
		public unsafe bool StripStringEscapeCharacters
		{
			get
			{
				return this.m_Data->StripStringEscapeCharacters;
			}
			set
			{
				this.m_Data->StripStringEscapeCharacters = value;
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000767D File Offset: 0x0000587D
		public unsafe PackedBinaryWriter(JsonTokenStream tokenStream, PackedBinaryStream binaryStream, Allocator label)
		{
			this.m_Label = label;
			this.m_Data = (UnsafePackedBinaryWriter*)UnsafeUtility.Malloc((long)sizeof(UnsafePackedBinaryWriter), UnsafeUtility.AlignOf<UnsafePackedBinaryWriter>(), label);
			*this.m_Data = new UnsafePackedBinaryWriter(tokenStream.GetUnsafePtr(), binaryStream.GetUnsafePtr());
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000076BC File Offset: 0x000058BC
		public unsafe void Dispose()
		{
			this.m_Data->Dispose();
			UnsafeUtility.Free((void*)this.m_Data, this.m_Label);
			this.m_Data = null;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000076E2 File Offset: 0x000058E2
		public unsafe void Seek(int index, int parent)
		{
			this.m_Data->Seek(index, parent);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000076F1 File Offset: 0x000058F1
		internal unsafe SerializedValueView GetView(int index)
		{
			return this.m_Data->GetView(index);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x000076FF File Offset: 0x000058FF
		public unsafe int Write(UnsafeBuffer<char> buffer, int count)
		{
			return this.m_Data->Write(buffer, count);
		}

		// Token: 0x040000C3 RID: 195
		private readonly Allocator m_Label;

		// Token: 0x040000C4 RID: 196
		[NativeDisableUnsafePtrRestriction]
		private unsafe UnsafePackedBinaryWriter* m_Data;
	}
}
