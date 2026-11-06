using System;
using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x02000696 RID: 1686
	internal sealed class BinaryAssembly : IStreamable
	{
		// Token: 0x06003E1C RID: 15900 RVA: 0x0000259F File Offset: 0x0000079F
		internal BinaryAssembly()
		{
		}

		// Token: 0x06003E1D RID: 15901 RVA: 0x000D6352 File Offset: 0x000D4552
		internal void Set(int assemId, string assemblyString)
		{
			this.assemId = assemId;
			this.assemblyString = assemblyString;
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x000D6362 File Offset: 0x000D4562
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(12);
			sout.WriteInt32(this.assemId);
			sout.WriteString(this.assemblyString);
		}

		// Token: 0x06003E1F RID: 15903 RVA: 0x000D6384 File Offset: 0x000D4584
		[SecurityCritical]
		public void Read(__BinaryParser input)
		{
			this.assemId = input.ReadInt32();
			this.assemblyString = input.ReadString();
		}

		// Token: 0x06003E20 RID: 15904 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Dump()
		{
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x000D6189 File Offset: 0x000D4389
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x0400283D RID: 10301
		internal int assemId;

		// Token: 0x0400283E RID: 10302
		internal string assemblyString;
	}
}
