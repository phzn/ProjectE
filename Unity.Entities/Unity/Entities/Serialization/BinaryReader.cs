using System;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000369 RID: 873
	public interface BinaryReader : IDisposable
	{
		// Token: 0x0600176E RID: 5998
		unsafe void ReadBytes(void* data, int bytes);

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600176F RID: 5999
		// (set) Token: 0x06001770 RID: 6000
		long Position { get; set; }
	}
}
