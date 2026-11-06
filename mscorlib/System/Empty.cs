using System;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
	// Token: 0x020001F4 RID: 500
	[Serializable]
	internal sealed class Empty : ISerializable
	{
		// Token: 0x06001571 RID: 5489 RVA: 0x0000259F File Offset: 0x0000079F
		private Empty()
		{
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x000258DF File Offset: 0x00023ADF
		public override string ToString()
		{
			return string.Empty;
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00054FF3 File Offset: 0x000531F3
		[SecurityCritical]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, 1, null, null);
		}

		// Token: 0x0400150D RID: 5389
		public static readonly Empty Value = new Empty();
	}
}
