using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.RenderGraphModule
{
	// Token: 0x02000020 RID: 32
	public struct ComputeBufferDesc
	{
		// Token: 0x06000151 RID: 337 RVA: 0x00007B47 File Offset: 0x00005D47
		public ComputeBufferDesc(int count, int stride)
		{
			this = default(ComputeBufferDesc);
			this.count = count;
			this.stride = stride;
			this.type = ComputeBufferType.Default;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007B65 File Offset: 0x00005D65
		public ComputeBufferDesc(int count, int stride, ComputeBufferType type)
		{
			this = default(ComputeBufferDesc);
			this.count = count;
			this.stride = stride;
			this.type = type;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007B84 File Offset: 0x00005D84
		public override int GetHashCode()
		{
			HashFNV1A32 hashFNV1A = HashFNV1A32.Create();
			hashFNV1A.Append(in this.count);
			hashFNV1A.Append(in this.stride);
			int num = (int)this.type;
			hashFNV1A.Append(in num);
			return hashFNV1A.value;
		}

		// Token: 0x040000C2 RID: 194
		public int count;

		// Token: 0x040000C3 RID: 195
		public int stride;

		// Token: 0x040000C4 RID: 196
		public ComputeBufferType type;

		// Token: 0x040000C5 RID: 197
		public string name;
	}
}
