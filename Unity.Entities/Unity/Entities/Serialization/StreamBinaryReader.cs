using System;
using System.IO;
using Unity.IO.LowLevel.Unsafe;

namespace Unity.Entities.Serialization
{
	// Token: 0x0200036B RID: 875
	internal class StreamBinaryReader : BinaryReader, IDisposable
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06001776 RID: 6006 RVA: 0x00058363 File Offset: 0x00056563
		internal string FilePath { get; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06001777 RID: 6007 RVA: 0x0005836B File Offset: 0x0005656B
		// (set) Token: 0x06001778 RID: 6008 RVA: 0x00058373 File Offset: 0x00056573
		public long Position { get; set; }

		// Token: 0x06001779 RID: 6009 RVA: 0x0005837C File Offset: 0x0005657C
		public StreamBinaryReader(string filePath, long bufferSize = 65536L)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				throw new ArgumentException("The filepath can neither be null nor empty", "filePath");
			}
			this.FilePath = filePath;
			this.Position = 0L;
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x00002287 File Offset: 0x00000487
		public void Dispose()
		{
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x000583AC File Offset: 0x000565AC
		public unsafe void ReadBytes(void* data, int bytes)
		{
			ReadCommand readCommand = new ReadCommand
			{
				Size = (long)bytes,
				Offset = this.Position,
				Buffer = data
			};
			ReadHandle readHandle = AsyncReadManager.Read(this.FilePath, &readCommand, 1U, "", 0UL, AssetLoadingSubsystem.Scripts);
			readHandle.JobHandle.Complete();
			if (readHandle.Status != ReadStatus.Complete)
			{
				throw new IOException("Failed to read from " + this.FilePath + "!");
			}
			this.Position += (long)bytes;
		}
	}
}
