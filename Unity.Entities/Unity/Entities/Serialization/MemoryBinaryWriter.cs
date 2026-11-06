using System;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200036D RID: 877
	public class MemoryBinaryWriter : BinaryWriter, IDisposable
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06001782 RID: 6018 RVA: 0x00058503 File Offset: 0x00056703
		public unsafe byte* Data
		{
			get
			{
				return this.content.GetUnsafePtr<byte>();
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x00058510 File Offset: 0x00056710
		public int Length
		{
			get
			{
				return this.content.Length;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x0005851D File Offset: 0x0005671D
		// (set) Token: 0x06001785 RID: 6021 RVA: 0x00058525 File Offset: 0x00056725
		public long Position { get; set; }

		// Token: 0x06001786 RID: 6022 RVA: 0x0005852E File Offset: 0x0005672E
		public void Dispose()
		{
			this.content.Dispose();
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x0005853B File Offset: 0x0005673B
		internal NativeArray<byte> GetContentAsNativeArray()
		{
			return this.content.AsArray();
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00058548 File Offset: 0x00056748
		public unsafe void WriteBytes(void* data, int bytes)
		{
			this.content.ResizeUninitialized((int)this.Position + bytes);
			UnsafeUtility.MemCpy((void*)(this.content.GetUnsafePtr<byte>() + (int)this.Position), data, (long)bytes);
			this.Position += (long)bytes;
		}

		// Token: 0x040008D3 RID: 2259
		private NativeList<byte> content = new NativeList<byte>(Allocator.Temp);
	}
}
