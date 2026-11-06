using System;
using System.Text;
using Unity.Collections;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200037D RID: 893
	internal struct StringTableReaderHandle : IDisposable
	{
		// Token: 0x060017C1 RID: 6081 RVA: 0x000592BF File Offset: 0x000574BF
		public unsafe StringTableReaderHandle(DotsSerializationReader.NodeHandle stringTableNode)
		{
			this._stringTableDataSize = (uint)stringTableNode.DataSize;
			this._stringTableData = (byte*)Memory.Unmanaged.Allocate(stringTableNode.DataSize, 4, Allocator.TempJob);
			stringTableNode.ReadData(this._stringTableData, 0L, -1L);
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x000592FA File Offset: 0x000574FA
		public void Dispose()
		{
			Memory.Unmanaged.Free<byte>(this._stringTableData, Allocator.TempJob);
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00059310 File Offset: 0x00057510
		public string GetString(int offset)
		{
			int stringLength = this.GetStringLength(offset);
			if (stringLength == -1)
			{
				return string.Format("Offset out of range {0}", offset);
			}
			return Encoding.UTF8.GetString(this._stringTableData + offset + 4, stringLength);
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x00059350 File Offset: 0x00057550
		public FixedString32Bytes GetString32(int offset)
		{
			int stringLength = this.GetStringLength(offset);
			if (stringLength == -1)
			{
				return new FixedString32Bytes(string.Format("Invalid Offset {0}", offset));
			}
			FixedString32Bytes fixedString32Bytes = default(FixedString32Bytes);
			(ref fixedString32Bytes).Append<FixedString32Bytes>(this._stringTableData + offset + 4, Math.Min(stringLength, 32));
			return fixedString32Bytes;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x000593A4 File Offset: 0x000575A4
		public FixedString64Bytes GetString64(int offset)
		{
			int stringLength = this.GetStringLength(offset);
			if (stringLength == -1)
			{
				return new FixedString64Bytes(string.Format("Invalid Offset {0}", offset));
			}
			FixedString64Bytes fixedString64Bytes = default(FixedString64Bytes);
			(ref fixedString64Bytes).Append<FixedString64Bytes>(this._stringTableData + offset + 4, Math.Min(stringLength, 64));
			return fixedString64Bytes;
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x000593F8 File Offset: 0x000575F8
		public FixedString128Bytes GetString128(int offset)
		{
			int stringLength = this.GetStringLength(offset);
			if (stringLength == -1)
			{
				return new FixedString128Bytes(string.Format("Invalid Offset {0}", offset));
			}
			FixedString128Bytes fixedString128Bytes = default(FixedString128Bytes);
			(ref fixedString128Bytes).Append<FixedString128Bytes>(this._stringTableData + offset + 4, Math.Min(stringLength, 128));
			return fixedString128Bytes;
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x00059450 File Offset: 0x00057650
		public FixedString512Bytes GetString512(int offset)
		{
			int stringLength = this.GetStringLength(offset);
			if (stringLength == -1)
			{
				return new FixedString512Bytes(string.Format("Invalid Offset {0}", offset));
			}
			FixedString512Bytes fixedString512Bytes = default(FixedString512Bytes);
			(ref fixedString512Bytes).Append<FixedString512Bytes>(this._stringTableData + offset + 4, Math.Min(stringLength, 512));
			return fixedString512Bytes;
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x000594A8 File Offset: 0x000576A8
		public FixedString4096Bytes GetString4096(int offset)
		{
			int stringLength = this.GetStringLength(offset);
			if (stringLength == -1)
			{
				return new FixedString4096Bytes(string.Format("Invalid Offset {0}", offset));
			}
			FixedString4096Bytes fixedString4096Bytes = default(FixedString4096Bytes);
			(ref fixedString4096Bytes).Append<FixedString4096Bytes>(this._stringTableData + offset + 4, Math.Min(stringLength, 4096));
			return fixedString4096Bytes;
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x000594FD File Offset: 0x000576FD
		public unsafe int GetStringLength(int offset)
		{
			if (offset < 0 || (long)offset >= (long)((ulong)this._stringTableDataSize))
			{
				return -1;
			}
			return *(int*)(this._stringTableData + offset);
		}

		// Token: 0x04000912 RID: 2322
		private unsafe readonly byte* _stringTableData;

		// Token: 0x04000913 RID: 2323
		private readonly uint _stringTableDataSize;
	}
}
