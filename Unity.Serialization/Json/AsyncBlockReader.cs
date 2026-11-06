using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Unity.Serialization.Json
{
	// Token: 0x0200006D RID: 109
	internal class AsyncBlockReader : BlockStreamReader
	{
		// Token: 0x0600028B RID: 651 RVA: 0x0000B194 File Offset: 0x00009394
		public AsyncBlockReader(Stream input, int bufferSize, bool leaveInputOpen)
			: base(input, bufferSize, leaveInputOpen)
		{
			int num = bufferSize / 2;
			this.m_Buffers = new List<ManagedCharBuffer>
			{
				new ManagedCharBuffer(num),
				new ManagedCharBuffer(num)
			};
			this.m_NextBufferIndex = 0;
			this.m_ReadBlockTask = null;
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0000B1E0 File Offset: 0x000093E0
		public override BlockInfo GetNextBlock()
		{
			List<ManagedCharBuffer> buffers = this.m_Buffers;
			int nextBufferIndex = this.m_NextBufferIndex;
			this.m_NextBufferIndex = nextBufferIndex + 1;
			ManagedCharBuffer managedCharBuffer = buffers[nextBufferIndex];
			char[] managedArray = managedCharBuffer.GetManagedArray();
			if (this.m_NextBufferIndex >= this.m_Buffers.Count)
			{
				this.m_NextBufferIndex = 0;
			}
			int num;
			if (this.m_ReadBlockTask == null)
			{
				num = this.Reader.ReadBlock(managedArray, 0, managedArray.Length);
			}
			else
			{
				if (!this.m_ReadBlockTask.IsCompleted)
				{
					this.m_ReadBlockTask.Wait();
				}
				num = this.m_ReadBlockTask.Result;
			}
			if (num == managedArray.Length)
			{
				char[] managedArray2 = this.m_Buffers[this.m_NextBufferIndex].GetManagedArray();
				this.m_ReadBlockTask = this.Reader.ReadBlockAsync(managedArray2, 0, managedArray2.Length);
			}
			else
			{
				Task<int> readBlockTask = this.m_ReadBlockTask;
				if (readBlockTask != null)
				{
					readBlockTask.Dispose();
				}
				this.m_ReadBlockTask = null;
			}
			return new BlockInfo
			{
				Block = new UnsafeBuffer<char>(managedCharBuffer.GetUnsafePtr(), num),
				IsFinal = (num != managedArray.Length)
			};
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000B2ED File Offset: 0x000094ED
		public override void Reset()
		{
			base.Reset();
			this.m_NextBufferIndex = 0;
			Task<int> readBlockTask = this.m_ReadBlockTask;
			if (readBlockTask != null)
			{
				readBlockTask.Dispose();
			}
			this.m_ReadBlockTask = null;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000B314 File Offset: 0x00009514
		public override void Dispose()
		{
			base.Dispose();
			Task<int> readBlockTask = this.m_ReadBlockTask;
			if (readBlockTask != null)
			{
				readBlockTask.Dispose();
			}
			this.m_ReadBlockTask = null;
			foreach (ManagedCharBuffer managedCharBuffer in this.m_Buffers)
			{
				managedCharBuffer.Dispose();
			}
		}

		// Token: 0x0400014D RID: 333
		private readonly List<ManagedCharBuffer> m_Buffers;

		// Token: 0x0400014E RID: 334
		private int m_NextBufferIndex;

		// Token: 0x0400014F RID: 335
		private Task<int> m_ReadBlockTask;
	}
}
