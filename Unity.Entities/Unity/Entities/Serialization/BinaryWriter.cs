using System;

namespace Unity.Entities.Serialization
{
	// Token: 0x02000367 RID: 871
	public interface BinaryWriter : IDisposable
	{
		// Token: 0x06001764 RID: 5988
		unsafe void WriteBytes(void* data, int bytes);

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06001765 RID: 5989
		// (set) Token: 0x06001766 RID: 5990
		long Position { get; set; }
	}
}
