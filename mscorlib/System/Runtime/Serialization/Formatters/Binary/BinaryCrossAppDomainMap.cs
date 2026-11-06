using System;
using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x0200069D RID: 1693
	internal sealed class BinaryCrossAppDomainMap : IStreamable
	{
		// Token: 0x06003E47 RID: 15943 RVA: 0x0000259F File Offset: 0x0000079F
		internal BinaryCrossAppDomainMap()
		{
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x000D703D File Offset: 0x000D523D
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(18);
			sout.WriteInt32(this.crossAppDomainArrayIndex);
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x000D7053 File Offset: 0x000D5253
		[SecurityCritical]
		public void Read(__BinaryParser input)
		{
			this.crossAppDomainArrayIndex = input.ReadInt32();
		}

		// Token: 0x06003E4A RID: 15946 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Dump()
		{
		}

		// Token: 0x06003E4B RID: 15947 RVA: 0x000D6189 File Offset: 0x000D4389
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x04002860 RID: 10336
		internal int crossAppDomainArrayIndex;
	}
}
