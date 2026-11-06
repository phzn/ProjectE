using System;
using System.Data.ProviderBase;

namespace System.Data.SqlClient
{
	// Token: 0x0200021D RID: 541
	internal sealed class SqlReferenceCollection : DbReferenceCollection
	{
		// Token: 0x06001A68 RID: 6760 RVA: 0x0007A25A File Offset: 0x0007845A
		public override void Add(object value, int tag)
		{
			base.AddItem(value, tag);
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0007A264 File Offset: 0x00078464
		internal void Deactivate()
		{
			base.Notify(0);
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x0007A270 File Offset: 0x00078470
		internal SqlDataReader FindLiveReader(SqlCommand command)
		{
			if (command == null)
			{
				return base.FindItem<SqlDataReader>(1, (SqlDataReader dataReader) => !dataReader.IsClosed);
			}
			return base.FindItem<SqlDataReader>(1, (SqlDataReader dataReader) => !dataReader.IsClosed && command == dataReader.Command);
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0007A2CC File Offset: 0x000784CC
		internal SqlCommand FindLiveCommand(TdsParserStateObject stateObj)
		{
			return base.FindItem<SqlCommand>(2, (SqlCommand command) => command.StateObject == stateObj);
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x0007A2FC File Offset: 0x000784FC
		protected override void NotifyItem(int message, int tag, object value)
		{
			if (tag == 1)
			{
				SqlDataReader sqlDataReader = (SqlDataReader)value;
				if (!sqlDataReader.IsClosed)
				{
					sqlDataReader.CloseReaderFromConnection();
					return;
				}
			}
			else
			{
				if (tag == 2)
				{
					((SqlCommand)value).OnConnectionClosed();
					return;
				}
				if (tag == 3)
				{
					((SqlBulkCopy)value).OnConnectionClosed();
				}
			}
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0007A342 File Offset: 0x00078542
		public override void Remove(object value)
		{
			base.RemoveItem(value);
		}

		// Token: 0x040010F2 RID: 4338
		internal const int DataReaderTag = 1;

		// Token: 0x040010F3 RID: 4339
		internal const int CommandTag = 2;

		// Token: 0x040010F4 RID: 4340
		internal const int BulkCopyTag = 3;
	}
}
