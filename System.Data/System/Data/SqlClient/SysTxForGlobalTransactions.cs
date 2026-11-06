using System;
using System.Reflection;
using System.Transactions;

namespace System.Data.SqlClient
{
	// Token: 0x0200023B RID: 571
	internal static class SysTxForGlobalTransactions
	{
		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x0007DB1A File Offset: 0x0007BD1A
		public static MethodInfo EnlistPromotableSinglePhase
		{
			get
			{
				return SysTxForGlobalTransactions._enlistPromotableSinglePhase.Value;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x0007DB26 File Offset: 0x0007BD26
		public static MethodInfo SetDistributedTransactionIdentifier
		{
			get
			{
				return SysTxForGlobalTransactions._setDistributedTransactionIdentifier.Value;
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x0007DB32 File Offset: 0x0007BD32
		public static MethodInfo GetPromotedToken
		{
			get
			{
				return SysTxForGlobalTransactions._getPromotedToken.Value;
			}
		}

		// Token: 0x0400115F RID: 4447
		private static readonly Lazy<MethodInfo> _enlistPromotableSinglePhase = new Lazy<MethodInfo>(() => typeof(Transaction).GetMethod("EnlistPromotableSinglePhase", new Type[]
		{
			typeof(IPromotableSinglePhaseNotification),
			typeof(Guid)
		}));

		// Token: 0x04001160 RID: 4448
		private static readonly Lazy<MethodInfo> _setDistributedTransactionIdentifier = new Lazy<MethodInfo>(() => typeof(Transaction).GetMethod("SetDistributedTransactionIdentifier", new Type[]
		{
			typeof(IPromotableSinglePhaseNotification),
			typeof(Guid)
		}));

		// Token: 0x04001161 RID: 4449
		private static readonly Lazy<MethodInfo> _getPromotedToken = new Lazy<MethodInfo>(() => typeof(Transaction).GetMethod("GetPromotedToken"));
	}
}
