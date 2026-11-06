using System;
using System.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x0200069B RID: 1691
	internal sealed class BinaryObjectString : IStreamable
	{
		// Token: 0x06003E3C RID: 15932 RVA: 0x0000259F File Offset: 0x0000079F
		internal BinaryObjectString()
		{
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x000D6FB6 File Offset: 0x000D51B6
		internal void Set(int objectId, string value)
		{
			this.objectId = objectId;
			this.value = value;
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x000D6FC6 File Offset: 0x000D51C6
		public void Write(__BinaryWriter sout)
		{
			sout.WriteByte(6);
			sout.WriteInt32(this.objectId);
			sout.WriteString(this.value);
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x000D6FE7 File Offset: 0x000D51E7
		[SecurityCritical]
		public void Read(__BinaryParser input)
		{
			this.objectId = input.ReadInt32();
			this.value = input.ReadString();
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x00004BF9 File Offset: 0x00002DF9
		public void Dump()
		{
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x000D6189 File Offset: 0x000D4389
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x0400285C RID: 10332
		internal int objectId;

		// Token: 0x0400285D RID: 10333
		internal string value;
	}
}
