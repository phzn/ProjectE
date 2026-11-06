using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SqlServer.Server;
using Unity;

namespace System.Data.SqlClient
{
	/// <summary>Represents a Transact-SQL statement or stored procedure to execute against a SQL Server database. This class cannot be inherited.</summary>
	// Token: 0x020001B1 RID: 433
	public sealed class SqlCommand : DbCommand, ICloneable, IDbCommand, IDisposable
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x00060CEB File Offset: 0x0005EEEB
		internal bool InPrepare
		{
			get
			{
				return this._inPrepare;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x0600153F RID: 5439 RVA: 0x00060CF3 File Offset: 0x0005EEF3
		private SqlCommand.CachedAsyncState cachedAsyncState
		{
			get
			{
				if (this._cachedAsyncState == null)
				{
					this._cachedAsyncState = new SqlCommand.CachedAsyncState();
				}
				return this._cachedAsyncState;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Data.SqlClient.SqlCommand" /> class.</summary>
		// Token: 0x06001540 RID: 5440 RVA: 0x00060D0E File Offset: 0x0005EF0E
		public SqlCommand()
		{
			this._commandTimeout = 30;
			this._updatedRowSource = UpdateRowSource.Both;
			this._prepareHandle = -1;
			this._preparedConnectionCloseCount = -1;
			this._preparedConnectionReconnectCount = -1;
			this._rowsAffected = -1;
			base..ctor();
			GC.SuppressFinalize(this);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Data.SqlClient.SqlCommand" /> class with the text of the query.</summary>
		/// <param name="cmdText">The text of the query.</param>
		// Token: 0x06001541 RID: 5441 RVA: 0x00060D47 File Offset: 0x0005EF47
		public SqlCommand(string cmdText)
			: this()
		{
			this.CommandText = cmdText;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Data.SqlClient.SqlCommand" /> class with the text of the query and a <see cref="T:System.Data.SqlClient.SqlConnection" />.</summary>
		/// <param name="cmdText">The text of the query.</param>
		/// <param name="connection">A <see cref="T:System.Data.SqlClient.SqlConnection" /> that represents the connection to an instance of SQL Server.</param>
		// Token: 0x06001542 RID: 5442 RVA: 0x00060D56 File Offset: 0x0005EF56
		public SqlCommand(string cmdText, SqlConnection connection)
			: this()
		{
			this.CommandText = cmdText;
			this.Connection = connection;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Data.SqlClient.SqlCommand" /> class with the text of the query, a <see cref="T:System.Data.SqlClient.SqlConnection" />, and the <see cref="T:System.Data.SqlClient.SqlTransaction" />.</summary>
		/// <param name="cmdText">The text of the query.</param>
		/// <param name="connection">A <see cref="T:System.Data.SqlClient.SqlConnection" /> that represents the connection to an instance of SQL Server.</param>
		/// <param name="transaction">The <see cref="T:System.Data.SqlClient.SqlTransaction" /> in which the <see cref="T:System.Data.SqlClient.SqlCommand" /> executes.</param>
		// Token: 0x06001543 RID: 5443 RVA: 0x00060D6C File Offset: 0x0005EF6C
		public SqlCommand(string cmdText, SqlConnection connection, SqlTransaction transaction)
			: this()
		{
			this.CommandText = cmdText;
			this.Connection = connection;
			this.Transaction = transaction;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00060D8C File Offset: 0x0005EF8C
		private SqlCommand(SqlCommand from)
			: this()
		{
			this.CommandText = from.CommandText;
			this.CommandTimeout = from.CommandTimeout;
			this.CommandType = from.CommandType;
			this.Connection = from.Connection;
			this.DesignTimeVisible = from.DesignTimeVisible;
			this.Transaction = from.Transaction;
			this.UpdatedRowSource = from.UpdatedRowSource;
			SqlParameterCollection parameters = this.Parameters;
			foreach (object obj in from.Parameters)
			{
				parameters.Add((obj is ICloneable) ? (obj as ICloneable).Clone() : obj);
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Data.SqlClient.SqlConnection" /> used by this instance of the <see cref="T:System.Data.SqlClient.SqlCommand" />.</summary>
		/// <returns>The connection to a data source. The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Data.SqlClient.SqlCommand.Connection" /> property was changed while the command was enlisted in a transaction.</exception>
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001545 RID: 5445 RVA: 0x00060E58 File Offset: 0x0005F058
		// (set) Token: 0x06001546 RID: 5446 RVA: 0x00060E60 File Offset: 0x0005F060
		public new SqlConnection Connection
		{
			get
			{
				return this._activeConnection;
			}
			set
			{
				if (this._activeConnection != value && this._activeConnection != null && this.cachedAsyncState.PendingAsyncOperation)
				{
					throw SQL.CannotModifyPropertyAsyncOperationInProgress("Connection");
				}
				if (this._transaction != null && this._transaction.Connection == null)
				{
					this._transaction = null;
				}
				if (this.IsPrepared && this._activeConnection != value && this._activeConnection != null)
				{
					try
					{
						this.Unprepare();
					}
					catch (Exception)
					{
					}
					finally
					{
						this._prepareHandle = -1;
						this._execType = SqlCommand.EXECTYPE.UNPREPARED;
					}
				}
				this._activeConnection = value;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001547 RID: 5447 RVA: 0x00060F08 File Offset: 0x0005F108
		// (set) Token: 0x06001548 RID: 5448 RVA: 0x00060F10 File Offset: 0x0005F110
		protected override DbConnection DbConnection
		{
			get
			{
				return this.Connection;
			}
			set
			{
				this.Connection = (SqlConnection)value;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001549 RID: 5449 RVA: 0x00060F1E File Offset: 0x0005F11E
		private SqlInternalConnectionTds InternalTdsConnection
		{
			get
			{
				return (SqlInternalConnectionTds)this._activeConnection.InnerConnection;
			}
		}

		/// <summary>Gets or sets a value that specifies the <see cref="T:System.Data.Sql.SqlNotificationRequest" /> object bound to this command.</summary>
		/// <returns>When set to null (default), no notification should be requested.</returns>
		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x00060F30 File Offset: 0x0005F130
		// (set) Token: 0x0600154B RID: 5451 RVA: 0x00060F38 File Offset: 0x0005F138
		public SqlNotificationRequest Notification
		{
			get
			{
				return this._notification;
			}
			set
			{
				this._sqlDep = null;
				this._notification = value;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x00060F48 File Offset: 0x0005F148
		internal SqlStatistics Statistics
		{
			get
			{
				if (this._activeConnection != null && (this._activeConnection.StatisticsEnabled || SqlCommand._diagnosticListener.IsEnabled("System.Data.SqlClient.WriteCommandAfter")))
				{
					return this._activeConnection.Statistics;
				}
				return null;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Data.SqlClient.SqlTransaction" /> within which the <see cref="T:System.Data.SqlClient.SqlCommand" /> executes.</summary>
		/// <returns>The <see cref="T:System.Data.SqlClient.SqlTransaction" />. The default value is <see langword="null" />.</returns>
		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x00060F7D File Offset: 0x0005F17D
		// (set) Token: 0x0600154E RID: 5454 RVA: 0x00060FA1 File Offset: 0x0005F1A1
		public new SqlTransaction Transaction
		{
			get
			{
				if (this._transaction != null && this._transaction.Connection == null)
				{
					this._transaction = null;
				}
				return this._transaction;
			}
			set
			{
				if (this._transaction != value && this._activeConnection != null && this.cachedAsyncState.PendingAsyncOperation)
				{
					throw SQL.CannotModifyPropertyAsyncOperationInProgress("Transaction");
				}
				this._transaction = value;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x0600154F RID: 5455 RVA: 0x00060FD3 File Offset: 0x0005F1D3
		// (set) Token: 0x06001550 RID: 5456 RVA: 0x00060FDB File Offset: 0x0005F1DB
		protected override DbTransaction DbTransaction
		{
			get
			{
				return this.Transaction;
			}
			set
			{
				this.Transaction = (SqlTransaction)value;
			}
		}

		/// <summary>Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source.</summary>
		/// <returns>The Transact-SQL statement or stored procedure to execute. The default is an empty string.</returns>
		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001551 RID: 5457 RVA: 0x00060FEC File Offset: 0x0005F1EC
		// (set) Token: 0x06001552 RID: 5458 RVA: 0x0006100A File Offset: 0x0005F20A
		public override string CommandText
		{
			get
			{
				string commandText = this._commandText;
				if (commandText == null)
				{
					return ADP.StrEmpty;
				}
				return commandText;
			}
			set
			{
				if (this._commandText != value)
				{
					this.PropertyChanging();
					this._commandText = value;
				}
			}
		}

		/// <summary>Gets or sets the wait time before terminating the attempt to execute a command and generating an error.</summary>
		/// <returns>The time in seconds to wait for the command to execute. The default is 30 seconds.</returns>
		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001553 RID: 5459 RVA: 0x00061027 File Offset: 0x0005F227
		// (set) Token: 0x06001554 RID: 5460 RVA: 0x0006102F File Offset: 0x0005F22F
		public override int CommandTimeout
		{
			get
			{
				return this._commandTimeout;
			}
			set
			{
				if (value < 0)
				{
					throw ADP.InvalidCommandTimeout(value, "CommandTimeout");
				}
				if (value != this._commandTimeout)
				{
					this.PropertyChanging();
					this._commandTimeout = value;
				}
			}
		}

		/// <summary>Resets the <see cref="P:System.Data.SqlClient.SqlCommand.CommandTimeout" /> property to its default value.</summary>
		// Token: 0x06001555 RID: 5461 RVA: 0x00061057 File Offset: 0x0005F257
		public void ResetCommandTimeout()
		{
			if (30 != this._commandTimeout)
			{
				this.PropertyChanging();
				this._commandTimeout = 30;
			}
		}

		/// <summary>Gets or sets a value indicating how the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText" /> property is to be interpreted.</summary>
		/// <returns>One of the <see cref="T:System.Data.CommandType" /> values. The default is <see langword="Text" />.</returns>
		/// <exception cref="T:System.ArgumentException">The value was not a valid <see cref="T:System.Data.CommandType" />.</exception>
		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001556 RID: 5462 RVA: 0x00061074 File Offset: 0x0005F274
		// (set) Token: 0x06001557 RID: 5463 RVA: 0x0006108E File Offset: 0x0005F28E
		public override CommandType CommandType
		{
			get
			{
				CommandType commandType = this._commandType;
				if (commandType == (CommandType)0)
				{
					return CommandType.Text;
				}
				return commandType;
			}
			set
			{
				if (this._commandType == value)
				{
					return;
				}
				if (value == CommandType.Text || value == CommandType.StoredProcedure)
				{
					this.PropertyChanging();
					this._commandType = value;
					return;
				}
				if (value != CommandType.TableDirect)
				{
					throw ADP.InvalidCommandType(value);
				}
				throw SQL.NotSupportedCommandType(value);
			}
		}

		/// <summary>Gets or sets a value indicating whether the command object should be visible in a Windows Form Designer control.</summary>
		/// <returns>A value indicating whether the command object should be visible in a control. The default is <see langword="true" />.</returns>
		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001558 RID: 5464 RVA: 0x000610C7 File Offset: 0x0005F2C7
		// (set) Token: 0x06001559 RID: 5465 RVA: 0x000610D2 File Offset: 0x0005F2D2
		public override bool DesignTimeVisible
		{
			get
			{
				return !this._designTimeInvisible;
			}
			set
			{
				this._designTimeInvisible = !value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Data.SqlClient.SqlParameterCollection" />.</summary>
		/// <returns>The parameters of the Transact-SQL statement or stored procedure. The default is an empty collection.</returns>
		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x000610DE File Offset: 0x0005F2DE
		public new SqlParameterCollection Parameters
		{
			get
			{
				if (this._parameters == null)
				{
					this._parameters = new SqlParameterCollection();
				}
				return this._parameters;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x0600155B RID: 5467 RVA: 0x000610F9 File Offset: 0x0005F2F9
		protected override DbParameterCollection DbParameterCollection
		{
			get
			{
				return this.Parameters;
			}
		}

		/// <summary>Gets or sets how command results are applied to the <see cref="T:System.Data.DataRow" /> when used by the Update method of the <see cref="T:System.Data.Common.DbDataAdapter" />.</summary>
		/// <returns>One of the <see cref="T:System.Data.UpdateRowSource" /> values.</returns>
		// Token: 0x170003AB RID: 939
		// (get) Token: 0x0600155C RID: 5468 RVA: 0x00061101 File Offset: 0x0005F301
		// (set) Token: 0x0600155D RID: 5469 RVA: 0x00061109 File Offset: 0x0005F309
		public override UpdateRowSource UpdatedRowSource
		{
			get
			{
				return this._updatedRowSource;
			}
			set
			{
				if (value <= UpdateRowSource.Both)
				{
					this._updatedRowSource = value;
					return;
				}
				throw ADP.InvalidUpdateRowSource(value);
			}
		}

		/// <summary>Occurs when the execution of a Transact-SQL statement completes.</summary>
		// Token: 0x14000024 RID: 36
		// (add) Token: 0x0600155E RID: 5470 RVA: 0x0006111D File Offset: 0x0005F31D
		// (remove) Token: 0x0600155F RID: 5471 RVA: 0x00061136 File Offset: 0x0005F336
		public event StatementCompletedEventHandler StatementCompleted
		{
			add
			{
				this._statementCompletedEventHandler = (StatementCompletedEventHandler)Delegate.Combine(this._statementCompletedEventHandler, value);
			}
			remove
			{
				this._statementCompletedEventHandler = (StatementCompletedEventHandler)Delegate.Remove(this._statementCompletedEventHandler, value);
			}
		}

		// Token: 0x06001560 RID: 5472 RVA: 0x00061150 File Offset: 0x0005F350
		internal void OnStatementCompleted(int recordCount)
		{
			if (0 <= recordCount)
			{
				StatementCompletedEventHandler statementCompletedEventHandler = this._statementCompletedEventHandler;
				if (statementCompletedEventHandler != null)
				{
					try
					{
						statementCompletedEventHandler(this, new StatementCompletedEventArgs(recordCount));
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableOrSecurityExceptionType(ex))
						{
							throw;
						}
					}
				}
			}
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x00061198 File Offset: 0x0005F398
		private void PropertyChanging()
		{
			this.IsDirty = true;
		}

		/// <summary>Creates a prepared version of the command on an instance of SQL Server.</summary>
		// Token: 0x06001562 RID: 5474 RVA: 0x000611A4 File Offset: 0x0005F3A4
		public override void Prepare()
		{
			this._pendingCancel = false;
			SqlStatistics sqlStatistics = null;
			sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
			if ((this.IsPrepared && !this.IsDirty) || this.CommandType == CommandType.StoredProcedure || (CommandType.Text == this.CommandType && this.GetParameterCount(this._parameters) == 0))
			{
				if (this.Statistics != null)
				{
					this.Statistics.SafeIncrement(ref this.Statistics._prepares);
				}
				this._hiddenPrepare = false;
			}
			else
			{
				this.ValidateCommand(false, "Prepare");
				bool flag = true;
				try
				{
					this.GetStateObject(null);
					if (this._parameters != null)
					{
						int count = this._parameters.Count;
						for (int i = 0; i < count; i++)
						{
							this._parameters[i].Prepare(this);
						}
					}
					this.InternalPrepare();
				}
				catch (Exception ex)
				{
					flag = ADP.IsCatchableExceptionType(ex);
					throw;
				}
				finally
				{
					if (flag)
					{
						this._hiddenPrepare = false;
						this.ReliablePutStateObject();
					}
				}
			}
			SqlStatistics.StopTimer(sqlStatistics);
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x000612AC File Offset: 0x0005F4AC
		private void InternalPrepare()
		{
			if (this.IsDirty)
			{
				this.Unprepare();
				this.IsDirty = false;
			}
			this._execType = SqlCommand.EXECTYPE.PREPAREPENDING;
			this._preparedConnectionCloseCount = this._activeConnection.CloseCount;
			this._preparedConnectionReconnectCount = this._activeConnection.ReconnectCount;
			if (this.Statistics != null)
			{
				this.Statistics.SafeIncrement(ref this.Statistics._prepares);
			}
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00061316 File Offset: 0x0005F516
		internal void Unprepare()
		{
			this._execType = SqlCommand.EXECTYPE.PREPAREPENDING;
			if (this._activeConnection.CloseCount != this._preparedConnectionCloseCount || this._activeConnection.ReconnectCount != this._preparedConnectionReconnectCount)
			{
				this._prepareHandle = -1;
			}
			this._cachedMetaData = null;
		}

		/// <summary>Tries to cancel the execution of a <see cref="T:System.Data.SqlClient.SqlCommand" />.</summary>
		// Token: 0x06001565 RID: 5477 RVA: 0x00061354 File Offset: 0x0005F554
		public override void Cancel()
		{
			SqlStatistics sqlStatistics = null;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				TaskCompletionSource<object> reconnectionCompletionSource = this._reconnectionCompletionSource;
				if (reconnectionCompletionSource == null || !reconnectionCompletionSource.TrySetCanceled())
				{
					if (this._activeConnection != null)
					{
						SqlInternalConnectionTds sqlInternalConnectionTds = this._activeConnection.InnerConnection as SqlInternalConnectionTds;
						if (sqlInternalConnectionTds != null)
						{
							SqlInternalConnectionTds sqlInternalConnectionTds2 = sqlInternalConnectionTds;
							lock (sqlInternalConnectionTds2)
							{
								if (sqlInternalConnectionTds == this._activeConnection.InnerConnection as SqlInternalConnectionTds)
								{
									if (sqlInternalConnectionTds.Parser != null)
									{
										if (!this._pendingCancel)
										{
											this._pendingCancel = true;
											TdsParserStateObject stateObj = this._stateObj;
											if (stateObj != null)
											{
												stateObj.Cancel(this);
											}
											else
											{
												SqlDataReader sqlDataReader = sqlInternalConnectionTds.FindLiveReader(this);
												if (sqlDataReader != null)
												{
													sqlDataReader.Cancel(this);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
		}

		/// <summary>Creates a new instance of a <see cref="T:System.Data.SqlClient.SqlParameter" /> object.</summary>
		/// <returns>A <see cref="T:System.Data.SqlClient.SqlParameter" /> object.</returns>
		// Token: 0x06001566 RID: 5478 RVA: 0x00060C2F File Offset: 0x0005EE2F
		public new SqlParameter CreateParameter()
		{
			return new SqlParameter();
		}

		// Token: 0x06001567 RID: 5479 RVA: 0x0006144C File Offset: 0x0005F64C
		protected override DbParameter CreateDbParameter()
		{
			return this.CreateParameter();
		}

		// Token: 0x06001568 RID: 5480 RVA: 0x00061454 File Offset: 0x0005F654
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._cachedMetaData = null;
			}
			base.Dispose(disposing);
		}

		/// <summary>Executes the query, and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.</summary>
		/// <returns>The first column of the first row in the result set, or a null reference (<see langword="Nothing" /> in Visual Basic) if the result set is empty. Returns a maximum of 2033 characters.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x06001569 RID: 5481 RVA: 0x00061468 File Offset: 0x0005F668
		public override object ExecuteScalar()
		{
			this._pendingCancel = false;
			Guid guid = SqlCommand._diagnosticListener.WriteCommandBefore(this, "ExecuteScalar");
			SqlStatistics sqlStatistics = null;
			Exception ex = null;
			object obj;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				SqlDataReader sqlDataReader = this.RunExecuteReader(CommandBehavior.Default, RunBehavior.ReturnImmediately, true, "ExecuteScalar");
				obj = this.CompleteExecuteScalar(sqlDataReader, false);
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				if (ex != null)
				{
					SqlCommand._diagnosticListener.WriteCommandError(guid, this, ex, "ExecuteScalar");
				}
				else
				{
					SqlCommand._diagnosticListener.WriteCommandAfter(guid, this, "ExecuteScalar");
				}
			}
			return obj;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x0006150C File Offset: 0x0005F70C
		private object CompleteExecuteScalar(SqlDataReader ds, bool returnSqlValue)
		{
			object obj = null;
			try
			{
				if (ds.Read() && ds.FieldCount > 0)
				{
					if (returnSqlValue)
					{
						obj = ds.GetSqlValue(0);
					}
					else
					{
						obj = ds.GetValue(0);
					}
				}
			}
			finally
			{
				ds.Close();
			}
			return obj;
		}

		/// <summary>Executes a Transact-SQL statement against the connection and returns the number of rows affected.</summary>
		/// <returns>The number of rows affected.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x0600156B RID: 5483 RVA: 0x0006155C File Offset: 0x0005F75C
		public override int ExecuteNonQuery()
		{
			this._pendingCancel = false;
			Guid guid = SqlCommand._diagnosticListener.WriteCommandBefore(this, "ExecuteNonQuery");
			SqlStatistics sqlStatistics = null;
			Exception ex = null;
			int rowsAffected;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this.InternalExecuteNonQuery(null, false, this.CommandTimeout, false, "ExecuteNonQuery");
				rowsAffected = this._rowsAffected;
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				if (ex != null)
				{
					SqlCommand._diagnosticListener.WriteCommandError(guid, this, ex, "ExecuteNonQuery");
				}
				else
				{
					SqlCommand._diagnosticListener.WriteCommandAfter(guid, this, "ExecuteNonQuery");
				}
			}
			return rowsAffected;
		}

		/// <summary>Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this <see cref="T:System.Data.SqlClient.SqlCommand" />.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that can be used to poll or wait for results, or both; this value is also needed when invoking <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteNonQuery(System.IAsyncResult)" />, which returns the number of affected rows.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">Any error that occurred while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The name/value pair "Asynchronous Processing=true" was not included within the connection string defining the connection for this <see cref="T:System.Data.SqlClient.SqlCommand" />.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x0600156C RID: 5484 RVA: 0x00061600 File Offset: 0x0005F800
		public IAsyncResult BeginExecuteNonQuery()
		{
			return this.BeginExecuteNonQuery(null, null);
		}

		/// <summary>Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this <see cref="T:System.Data.SqlClient.SqlCommand" />, given a callback procedure and state information.</summary>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that is invoked when the command's execution has completed. Pass <see langword="null" /> (<see langword="Nothing" /> in Microsoft Visual Basic) to indicate that no callback is required.</param>
		/// <param name="stateObject">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the <see cref="P:System.IAsyncResult.AsyncState" /> property.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that can be used to poll or wait for results, or both; this value is also needed when invoking <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteNonQuery(System.IAsyncResult)" />, which returns the number of affected rows.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">Any error that occurred while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The name/value pair "Asynchronous Processing=true" was not included within the connection string defining the connection for this <see cref="T:System.Data.SqlClient.SqlCommand" />.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x0600156D RID: 5485 RVA: 0x0006160C File Offset: 0x0005F80C
		public IAsyncResult BeginExecuteNonQuery(AsyncCallback callback, object stateObject)
		{
			this._pendingCancel = false;
			this.ValidateAsyncCommand();
			SqlStatistics sqlStatistics = null;
			IAsyncResult task2;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				TaskCompletionSource<object> completion = new TaskCompletionSource<object>(stateObject);
				try
				{
					Task task = this.InternalExecuteNonQuery(completion, false, this.CommandTimeout, true, "BeginExecuteNonQuery");
					this.cachedAsyncState.SetActiveConnectionAndResult(completion, "EndExecuteNonQuery", this._activeConnection);
					if (task != null)
					{
						AsyncHelper.ContinueTask(task, completion, delegate
						{
							this.BeginExecuteNonQueryInternalReadStage(completion);
						}, null, null, null, null, null);
					}
					else
					{
						this.BeginExecuteNonQueryInternalReadStage(completion);
					}
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableOrSecurityExceptionType(ex))
					{
						throw;
					}
					this.ReliablePutStateObject();
					throw;
				}
				if (callback != null)
				{
					completion.Task.ContinueWith(delegate(Task<object> t)
					{
						callback(t);
					}, TaskScheduler.Default);
				}
				task2 = completion.Task;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return task2;
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x00061744 File Offset: 0x0005F944
		private void BeginExecuteNonQueryInternalReadStage(TaskCompletionSource<object> completion)
		{
			try
			{
				this._stateObj.ReadSni(completion);
			}
			catch (Exception)
			{
				if (this._cachedAsyncState != null)
				{
					this._cachedAsyncState.ResetAsyncState();
				}
				this.ReliablePutStateObject();
				throw;
			}
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x0006178C File Offset: 0x0005F98C
		private void VerifyEndExecuteState(Task completionTask, string endMethod)
		{
			if (completionTask.IsCanceled)
			{
				if (this._stateObj == null)
				{
					throw SQL.CR_ReconnectionCancelled();
				}
				this._stateObj.Parser.State = TdsParserState.Broken;
				this._stateObj.Parser.Connection.BreakConnection();
				this._stateObj.Parser.ThrowExceptionAndWarning(this._stateObj, false, false);
			}
			else if (completionTask.IsFaulted)
			{
				throw completionTask.Exception.InnerException;
			}
			if (this.cachedAsyncState.EndMethodName == null)
			{
				throw ADP.MethodCalledTwice(endMethod);
			}
			if (endMethod != this.cachedAsyncState.EndMethodName)
			{
				throw ADP.MismatchedAsyncResult(this.cachedAsyncState.EndMethodName, endMethod);
			}
			if (this._activeConnection.State != ConnectionState.Open || !this.cachedAsyncState.IsActiveConnectionValid(this._activeConnection))
			{
				throw ADP.ClosedConnectionError();
			}
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x00061863 File Offset: 0x0005FA63
		private void WaitForAsyncResults(IAsyncResult asyncResult)
		{
			Task task = (Task)asyncResult;
			if (!asyncResult.IsCompleted)
			{
				asyncResult.AsyncWaitHandle.WaitOne();
			}
			this._stateObj._networkPacketTaskSource = null;
			this._activeConnection.GetOpenTdsConnection().DecrementAsyncCount();
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0006189C File Offset: 0x0005FA9C
		private void ThrowIfReconnectionHasBeenCanceled()
		{
			if (this._stateObj == null)
			{
				TaskCompletionSource<object> reconnectionCompletionSource = this._reconnectionCompletionSource;
				if (reconnectionCompletionSource != null && reconnectionCompletionSource.Task.IsCanceled)
				{
					throw SQL.CR_ReconnectionCancelled();
				}
			}
		}

		/// <summary>Finishes asynchronous execution of a Transact-SQL statement.</summary>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> returned by the call to <see cref="M:System.Data.SqlClient.SqlCommand.BeginExecuteNonQuery" />.</param>
		/// <returns>The number of rows affected (the same behavior as <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteNonQuery" />).</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> parameter is null (<see langword="Nothing" /> in Microsoft Visual Basic)</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteNonQuery(System.IAsyncResult)" /> was called more than once for a single command execution, or the method was mismatched against its execution method (for example, the code called <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteNonQuery(System.IAsyncResult)" /> to complete execution of a call to <see cref="M:System.Data.SqlClient.SqlCommand.BeginExecuteXmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">The amount of time specified in <see cref="P:System.Data.SqlClient.SqlCommand.CommandTimeout" /> elapsed and the asynchronous operation specified with <see cref="Overload:System.Data.SqlClient.SqlCommand.BeginExecuteNonQuery" /> is not complete.  
		///  In some situations, <see cref="T:System.IAsyncResult" /> can be set to <see langword="IsCompleted" /> incorrectly. If this occurs and <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteNonQuery(System.IAsyncResult)" /> is called, EndExecuteNonQuery could raise a SqlException error if the amount of time specified in <see cref="P:System.Data.SqlClient.SqlCommand.CommandTimeout" /> elapsed and the asynchronous operation specified with <see cref="Overload:System.Data.SqlClient.SqlCommand.BeginExecuteNonQuery" /> is not complete. To correct this situation, you should either increase the value of CommandTimeout or reduce the work being done by the asynchronous operation.</exception>
		// Token: 0x06001572 RID: 5490 RVA: 0x000618D0 File Offset: 0x0005FAD0
		public int EndExecuteNonQuery(IAsyncResult asyncResult)
		{
			Exception exception = ((Task)asyncResult).Exception;
			if (exception != null)
			{
				if (this.cachedAsyncState != null)
				{
					this.cachedAsyncState.ResetAsyncState();
				}
				this.ReliablePutStateObject();
				throw exception.InnerException;
			}
			this.ThrowIfReconnectionHasBeenCanceled();
			TdsParserStateObject stateObj = this._stateObj;
			int num;
			lock (stateObj)
			{
				num = this.EndExecuteNonQueryInternal(asyncResult);
			}
			return num;
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00061948 File Offset: 0x0005FB48
		private int EndExecuteNonQueryInternal(IAsyncResult asyncResult)
		{
			SqlStatistics sqlStatistics = null;
			int rowsAffected;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				this.VerifyEndExecuteState((Task)asyncResult, "EndExecuteNonQuery");
				this.WaitForAsyncResults(asyncResult);
				bool flag = true;
				try
				{
					this.CheckThrowSNIException();
					if (CommandType.Text == this.CommandType && this.GetParameterCount(this._parameters) == 0)
					{
						try
						{
							bool flag2;
							if (!this._stateObj.Parser.TryRun(RunBehavior.UntilDone, this, null, null, this._stateObj, out flag2))
							{
								throw SQL.SynchronousCallMayNotPend();
							}
							goto IL_0087;
						}
						finally
						{
							this.cachedAsyncState.ResetAsyncState();
						}
					}
					SqlDataReader sqlDataReader = this.CompleteAsyncExecuteReader();
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
					}
					IL_0087:;
				}
				catch (Exception ex)
				{
					flag = ADP.IsCatchableExceptionType(ex);
					throw;
				}
				finally
				{
					if (flag)
					{
						this.PutStateObject();
					}
				}
				rowsAffected = this._rowsAffected;
			}
			catch (Exception ex2)
			{
				if (this.cachedAsyncState != null)
				{
					this.cachedAsyncState.ResetAsyncState();
				}
				if (ADP.IsCatchableExceptionType(ex2))
				{
					this.ReliablePutStateObject();
				}
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return rowsAffected;
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x00061A68 File Offset: 0x0005FC68
		private Task InternalExecuteNonQuery(TaskCompletionSource<object> completion, bool sendToPipe, int timeout, bool asyncWrite = false, [CallerMemberName] string methodName = "")
		{
			bool flag = completion != null;
			SqlStatistics statistics = this.Statistics;
			this._rowsAffected = -1;
			this.ValidateCommand(flag, methodName);
			this.CheckNotificationStateAndAutoEnlist();
			Task task = null;
			if (!this.BatchRPCMode && CommandType.Text == this.CommandType && this.GetParameterCount(this._parameters) == 0)
			{
				if (statistics != null)
				{
					if (!this.IsDirty && this.IsPrepared)
					{
						statistics.SafeIncrement(ref statistics._preparedExecs);
					}
					else
					{
						statistics.SafeIncrement(ref statistics._unpreparedExecs);
					}
				}
				task = this.RunExecuteNonQueryTds(methodName, flag, timeout, asyncWrite);
			}
			else
			{
				SqlDataReader reader = this.RunExecuteReader(CommandBehavior.Default, RunBehavior.UntilDone, false, completion, timeout, out task, asyncWrite, methodName);
				if (reader != null)
				{
					if (task != null)
					{
						task = AsyncHelper.CreateContinuationTask(task, delegate
						{
							reader.Close();
						}, null, null);
					}
					else
					{
						reader.Close();
					}
				}
			}
			return task;
		}

		/// <summary>Sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText" /> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection" /> and builds an <see cref="T:System.Xml.XmlReader" /> object.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x06001575 RID: 5493 RVA: 0x00061B44 File Offset: 0x0005FD44
		public XmlReader ExecuteXmlReader()
		{
			this._pendingCancel = false;
			Guid guid = SqlCommand._diagnosticListener.WriteCommandBefore(this, "ExecuteXmlReader");
			SqlStatistics sqlStatistics = null;
			Exception ex = null;
			XmlReader xmlReader;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				SqlDataReader sqlDataReader = this.RunExecuteReader(CommandBehavior.SequentialAccess, RunBehavior.ReturnImmediately, true, "ExecuteXmlReader");
				xmlReader = this.CompleteXmlReader(sqlDataReader, false);
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				if (ex != null)
				{
					SqlCommand._diagnosticListener.WriteCommandError(guid, this, ex, "ExecuteXmlReader");
				}
				else
				{
					SqlCommand._diagnosticListener.WriteCommandAfter(guid, this, "ExecuteXmlReader");
				}
			}
			return xmlReader;
		}

		/// <summary>Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this <see cref="T:System.Data.SqlClient.SqlCommand" /> and returns results as an <see cref="T:System.Xml.XmlReader" /> object.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that can be used to poll or wait for results, or both; this value is also needed when invoking <see langword="EndExecuteXmlReader" />, which returns a single XML value.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">Any error that occurred while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The name/value pair "Asynchronous Processing=true" was not included within the connection string defining the connection for this <see cref="T:System.Data.SqlClient.SqlCommand" />.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x06001576 RID: 5494 RVA: 0x00061BE8 File Offset: 0x0005FDE8
		public IAsyncResult BeginExecuteXmlReader()
		{
			return this.BeginExecuteXmlReader(null, null);
		}

		/// <summary>Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this <see cref="T:System.Data.SqlClient.SqlCommand" /> and returns results as an <see cref="T:System.Xml.XmlReader" /> object, using a callback procedure.</summary>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that is invoked when the command's execution has completed. Pass <see langword="null" /> (<see langword="Nothing" /> in Microsoft Visual Basic) to indicate that no callback is required.</param>
		/// <param name="stateObject">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the <see cref="P:System.IAsyncResult.AsyncState" /> property.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that can be used to poll, wait for results, or both; this value is also needed when the <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteXmlReader(System.IAsyncResult)" /> is called, which returns the results of the command as XML.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">Any error that occurred while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The name/value pair "Asynchronous Processing=true" was not included within the connection string defining the connection for this <see cref="T:System.Data.SqlClient.SqlCommand" />.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x06001577 RID: 5495 RVA: 0x00061BF4 File Offset: 0x0005FDF4
		public IAsyncResult BeginExecuteXmlReader(AsyncCallback callback, object stateObject)
		{
			this._pendingCancel = false;
			this.ValidateAsyncCommand();
			SqlStatistics sqlStatistics = null;
			IAsyncResult task2;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				TaskCompletionSource<object> completion = new TaskCompletionSource<object>(stateObject);
				Task task;
				try
				{
					this.RunExecuteReader(CommandBehavior.SequentialAccess, RunBehavior.ReturnImmediately, true, completion, this.CommandTimeout, out task, true, "BeginExecuteXmlReader");
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableOrSecurityExceptionType(ex))
					{
						throw;
					}
					this.ReliablePutStateObject();
					throw;
				}
				this.cachedAsyncState.SetActiveConnectionAndResult(completion, "EndExecuteXmlReader", this._activeConnection);
				if (task != null)
				{
					AsyncHelper.ContinueTask(task, completion, delegate
					{
						this.BeginExecuteXmlReaderInternalReadStage(completion);
					}, null, null, null, null, null);
				}
				else
				{
					this.BeginExecuteXmlReaderInternalReadStage(completion);
				}
				if (callback != null)
				{
					completion.Task.ContinueWith(delegate(Task<object> t)
					{
						callback(t);
					}, TaskScheduler.Default);
				}
				task2 = completion.Task;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return task2;
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x00061D30 File Offset: 0x0005FF30
		private void BeginExecuteXmlReaderInternalReadStage(TaskCompletionSource<object> completion)
		{
			try
			{
				this._stateObj.ReadSni(completion);
			}
			catch (Exception ex)
			{
				if (this._cachedAsyncState != null)
				{
					this._cachedAsyncState.ResetAsyncState();
				}
				this.ReliablePutStateObject();
				completion.TrySetException(ex);
			}
		}

		/// <summary>Finishes asynchronous execution of a Transact-SQL statement, returning the requested data as XML.</summary>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> returned by the call to <see cref="M:System.Data.SqlClient.SqlCommand.BeginExecuteXmlReader" />.</param>
		/// <returns>An <see cref="T:System.Xml.XmlReader" /> object that can be used to fetch the resulting XML data.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> parameter is null (<see langword="Nothing" /> in Microsoft Visual Basic)</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteXmlReader(System.IAsyncResult)" /> was called more than once for a single command execution, or the method was mismatched against its execution method (for example, the code called <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteXmlReader(System.IAsyncResult)" /> to complete execution of a call to <see cref="M:System.Data.SqlClient.SqlCommand.BeginExecuteNonQuery" />.</exception>
		// Token: 0x06001579 RID: 5497 RVA: 0x00061D80 File Offset: 0x0005FF80
		public XmlReader EndExecuteXmlReader(IAsyncResult asyncResult)
		{
			Exception exception = ((Task)asyncResult).Exception;
			if (exception != null)
			{
				if (this.cachedAsyncState != null)
				{
					this.cachedAsyncState.ResetAsyncState();
				}
				this.ReliablePutStateObject();
				throw exception.InnerException;
			}
			this.ThrowIfReconnectionHasBeenCanceled();
			TdsParserStateObject stateObj = this._stateObj;
			XmlReader xmlReader;
			lock (stateObj)
			{
				xmlReader = this.EndExecuteXmlReaderInternal(asyncResult);
			}
			return xmlReader;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x00061DF8 File Offset: 0x0005FFF8
		private XmlReader EndExecuteXmlReaderInternal(IAsyncResult asyncResult)
		{
			XmlReader xmlReader;
			try
			{
				xmlReader = this.CompleteXmlReader(this.InternalEndExecuteReader(asyncResult, "EndExecuteXmlReader"), true);
			}
			catch (Exception ex)
			{
				if (this.cachedAsyncState != null)
				{
					this.cachedAsyncState.ResetAsyncState();
				}
				if (ADP.IsCatchableExceptionType(ex))
				{
					this.ReliablePutStateObject();
				}
				throw;
			}
			return xmlReader;
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x00061E50 File Offset: 0x00060050
		private XmlReader CompleteXmlReader(SqlDataReader ds, bool async = false)
		{
			XmlReader xmlReader = null;
			SmiExtendedMetaData[] internalSmiMetaData = ds.GetInternalSmiMetaData();
			if (internalSmiMetaData != null && internalSmiMetaData.Length == 1 && (internalSmiMetaData[0].SqlDbType == SqlDbType.NText || internalSmiMetaData[0].SqlDbType == SqlDbType.NVarChar || internalSmiMetaData[0].SqlDbType == SqlDbType.Xml))
			{
				try
				{
					xmlReader = new SqlStream(ds, true, internalSmiMetaData[0].SqlDbType != SqlDbType.Xml).ToXmlReader(async);
				}
				catch (Exception ex)
				{
					if (ADP.IsCatchableExceptionType(ex))
					{
						ds.Close();
					}
					throw;
				}
			}
			if (xmlReader == null)
			{
				ds.Close();
				throw SQL.NonXmlResult();
			}
			return xmlReader;
		}

		// Token: 0x0600157C RID: 5500 RVA: 0x00061EEC File Offset: 0x000600EC
		protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
		{
			return this.ExecuteReader(behavior);
		}

		/// <summary>Sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText" /> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection" /> and builds a <see cref="T:System.Data.SqlClient.SqlDataReader" />.</summary>
		/// <returns>A <see cref="T:System.Data.SqlClient.SqlDataReader" /> object.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">An exception occurred while executing the command against a locked row. This exception is not generated when you are using Microsoft .NET Framework version 1.0.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The current state of the connection is closed. <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteReader" /> requires an open <see cref="T:System.Data.SqlClient.SqlConnection" />.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x0600157D RID: 5501 RVA: 0x00061EF8 File Offset: 0x000600F8
		public new SqlDataReader ExecuteReader()
		{
			SqlStatistics sqlStatistics = null;
			SqlDataReader sqlDataReader;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				sqlDataReader = this.ExecuteReader(CommandBehavior.Default);
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return sqlDataReader;
		}

		/// <summary>Sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText" /> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection" />, and builds a <see cref="T:System.Data.SqlClient.SqlDataReader" /> using one of the <see cref="T:System.Data.CommandBehavior" /> values.</summary>
		/// <param name="behavior">One of the <see cref="T:System.Data.CommandBehavior" /> values.</param>
		/// <returns>A <see cref="T:System.Data.SqlClient.SqlDataReader" /> object.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x0600157E RID: 5502 RVA: 0x00061F38 File Offset: 0x00060138
		public new SqlDataReader ExecuteReader(CommandBehavior behavior)
		{
			this._pendingCancel = false;
			Guid guid = SqlCommand._diagnosticListener.WriteCommandBefore(this, "ExecuteReader");
			SqlStatistics sqlStatistics = null;
			Exception ex = null;
			SqlDataReader sqlDataReader;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				sqlDataReader = this.RunExecuteReader(behavior, RunBehavior.ReturnImmediately, true, "ExecuteReader");
			}
			catch (Exception ex)
			{
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
				if (ex != null)
				{
					SqlCommand._diagnosticListener.WriteCommandError(guid, this, ex, "ExecuteReader");
				}
				else
				{
					SqlCommand._diagnosticListener.WriteCommandAfter(guid, this, "ExecuteReader");
				}
			}
			return sqlDataReader;
		}

		/// <summary>Finishes asynchronous execution of a Transact-SQL statement, returning the requested <see cref="T:System.Data.SqlClient.SqlDataReader" />.</summary>
		/// <param name="asyncResult">The <see cref="T:System.IAsyncResult" /> returned by the call to <see cref="M:System.Data.SqlClient.SqlCommand.BeginExecuteReader" />.</param>
		/// <returns>A <see cref="T:System.Data.SqlClient.SqlDataReader" /> object that can be used to retrieve the requested rows.</returns>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="asyncResult" /> parameter is null (<see langword="Nothing" /> in Microsoft Visual Basic)</exception>
		/// <exception cref="T:System.InvalidOperationException">
		///   <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteReader(System.IAsyncResult)" /> was called more than once for a single command execution, or the method was mismatched against its execution method (for example, the code called <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteReader(System.IAsyncResult)" /> to complete execution of a call to <see cref="M:System.Data.SqlClient.SqlCommand.BeginExecuteXmlReader" />.</exception>
		// Token: 0x0600157F RID: 5503 RVA: 0x00061FD0 File Offset: 0x000601D0
		public SqlDataReader EndExecuteReader(IAsyncResult asyncResult)
		{
			Exception exception = ((Task)asyncResult).Exception;
			if (exception != null)
			{
				if (this.cachedAsyncState != null)
				{
					this.cachedAsyncState.ResetAsyncState();
				}
				this.ReliablePutStateObject();
				throw exception.InnerException;
			}
			this.ThrowIfReconnectionHasBeenCanceled();
			TdsParserStateObject stateObj = this._stateObj;
			SqlDataReader sqlDataReader;
			lock (stateObj)
			{
				sqlDataReader = this.EndExecuteReaderInternal(asyncResult);
			}
			return sqlDataReader;
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x00062048 File Offset: 0x00060248
		private SqlDataReader EndExecuteReaderInternal(IAsyncResult asyncResult)
		{
			SqlStatistics sqlStatistics = null;
			SqlDataReader sqlDataReader;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				sqlDataReader = this.InternalEndExecuteReader(asyncResult, "EndExecuteReader");
			}
			catch (Exception ex)
			{
				if (this.cachedAsyncState != null)
				{
					this.cachedAsyncState.ResetAsyncState();
				}
				if (ADP.IsCatchableExceptionType(ex))
				{
					this.ReliablePutStateObject();
				}
				throw;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return sqlDataReader;
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x000620B8 File Offset: 0x000602B8
		internal IAsyncResult BeginExecuteReader(CommandBehavior behavior, AsyncCallback callback, object stateObject)
		{
			this._pendingCancel = false;
			SqlStatistics sqlStatistics = null;
			IAsyncResult task2;
			try
			{
				sqlStatistics = SqlStatistics.StartTimer(this.Statistics);
				TaskCompletionSource<object> completion = new TaskCompletionSource<object>(stateObject);
				this.ValidateAsyncCommand();
				Task task = null;
				try
				{
					this.RunExecuteReader(behavior, RunBehavior.ReturnImmediately, true, completion, this.CommandTimeout, out task, true, "BeginExecuteReader");
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableOrSecurityExceptionType(ex))
					{
						throw;
					}
					this.ReliablePutStateObject();
					throw;
				}
				this.cachedAsyncState.SetActiveConnectionAndResult(completion, "EndExecuteReader", this._activeConnection);
				if (task != null)
				{
					AsyncHelper.ContinueTask(task, completion, delegate
					{
						this.BeginExecuteReaderInternalReadStage(completion);
					}, null, null, null, null, null);
				}
				else
				{
					this.BeginExecuteReaderInternalReadStage(completion);
				}
				if (callback != null)
				{
					completion.Task.ContinueWith(delegate(Task<object> t)
					{
						callback(t);
					}, TaskScheduler.Default);
				}
				task2 = completion.Task;
			}
			finally
			{
				SqlStatistics.StopTimer(sqlStatistics);
			}
			return task2;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x000621F4 File Offset: 0x000603F4
		private void BeginExecuteReaderInternalReadStage(TaskCompletionSource<object> completion)
		{
			try
			{
				this._stateObj.ReadSni(completion);
			}
			catch (Exception ex)
			{
				if (this._cachedAsyncState != null)
				{
					this._cachedAsyncState.ResetAsyncState();
				}
				this.ReliablePutStateObject();
				completion.TrySetException(ex);
			}
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x00062244 File Offset: 0x00060444
		private SqlDataReader InternalEndExecuteReader(IAsyncResult asyncResult, string endMethod)
		{
			this.VerifyEndExecuteState((Task)asyncResult, endMethod);
			this.WaitForAsyncResults(asyncResult);
			this.CheckThrowSNIException();
			return this.CompleteAsyncExecuteReader();
		}

		/// <summary>An asynchronous version of <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteNonQuery" />, which executes a Transact-SQL statement against the connection and returns the number of rows affected. The cancellation token can be used to request that the operation be abandoned before the command timeout elapses.  Exceptions will be reported via the returned Task object.</summary>
		/// <param name="cancellationToken">The cancellation instruction.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">Calling <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteNonQueryAsync(System.Threading.CancellationToken)" /> more than once for the same instance before task completion.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.  
		///  <see langword="Context Connection=true" /> is specified in the connection string.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">SQL Server returned an error while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x06001584 RID: 5508 RVA: 0x00062268 File Offset: 0x00060468
		public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
		{
			Guid operationId = SqlCommand._diagnosticListener.WriteCommandBefore(this, "ExecuteNonQueryAsync");
			TaskCompletionSource<int> source = new TaskCompletionSource<int>();
			CancellationTokenRegistration registration = default(CancellationTokenRegistration);
			if (cancellationToken.CanBeCanceled)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					source.SetCanceled();
					return source.Task;
				}
				registration = cancellationToken.Register(delegate(object s)
				{
					((SqlCommand)s).CancelIgnoreFailure();
				}, this);
			}
			Task<int> task = source.Task;
			try
			{
				this.RegisterForConnectionCloseNotification<int>(ref task);
				Task<int>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginExecuteNonQuery), new Func<IAsyncResult, int>(this.EndExecuteNonQuery), null).ContinueWith(delegate(Task<int> t)
				{
					registration.Dispose();
					if (t.IsFaulted)
					{
						Exception innerException = t.Exception.InnerException;
						SqlCommand._diagnosticListener.WriteCommandError(operationId, this, innerException, "ExecuteNonQueryAsync");
						source.SetException(innerException);
						return;
					}
					if (t.IsCanceled)
					{
						source.SetCanceled();
					}
					else
					{
						source.SetResult(t.Result);
					}
					SqlCommand._diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteNonQueryAsync");
				}, TaskScheduler.Default);
			}
			catch (Exception ex)
			{
				SqlCommand._diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteNonQueryAsync");
				source.SetException(ex);
			}
			return task;
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0006238C File Offset: 0x0006058C
		protected override Task<DbDataReader> ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
		{
			return this.ExecuteReaderAsync(behavior, cancellationToken).ContinueWith<DbDataReader>(delegate(Task<SqlDataReader> result)
			{
				if (result.IsFaulted)
				{
					throw result.Exception.InnerException;
				}
				return result.Result;
			}, CancellationToken.None, TaskContinuationOptions.NotOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
		}

		/// <summary>An asynchronous version of <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteReader" />, which sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText" /> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection" /> and builds a <see cref="T:System.Data.SqlClient.SqlDataReader" />. Exceptions will be reported via the returned Task object.</summary>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.ArgumentException">An invalid <see cref="T:System.Data.CommandBehavior" /> value.</exception>
		/// <exception cref="T:System.InvalidOperationException">Calling <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteReaderAsync" /> more than once for the same instance before task completion.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.  
		///  <see langword="Context Connection=true" /> is specified in the connection string.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">SQL Server returned an error while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x06001586 RID: 5510 RVA: 0x000623C9 File Offset: 0x000605C9
		public new Task<SqlDataReader> ExecuteReaderAsync()
		{
			return this.ExecuteReaderAsync(CommandBehavior.Default, CancellationToken.None);
		}

		/// <summary>An asynchronous version of <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteReader(System.Data.CommandBehavior)" />, which sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText" /> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection" />, and builds a <see cref="T:System.Data.SqlClient.SqlDataReader" />. Exceptions will be reported via the returned Task object.</summary>
		/// <param name="behavior">Options for statement execution and data retrieval.  When is set to <see langword="Default" />, <see cref="M:System.Data.SqlClient.SqlDataReader.ReadAsync(System.Threading.CancellationToken)" /> reads the entire row before returning a complete Task.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.ArgumentException">An invalid <see cref="T:System.Data.CommandBehavior" /> value.</exception>
		/// <exception cref="T:System.InvalidOperationException">Calling <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior)" /> more than once for the same instance before task completion.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.  
		///  <see langword="Context Connection=true" /> is specified in the connection string.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">SQL Server returned an error while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x06001587 RID: 5511 RVA: 0x000623D7 File Offset: 0x000605D7
		public new Task<SqlDataReader> ExecuteReaderAsync(CommandBehavior behavior)
		{
			return this.ExecuteReaderAsync(behavior, CancellationToken.None);
		}

		/// <summary>An asynchronous version of <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteReader" />, which sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText" /> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection" /> and builds a <see cref="T:System.Data.SqlClient.SqlDataReader" />.  
		///  The cancellation token can be used to request that the operation be abandoned before the command timeout elapses.  Exceptions will be reported via the returned Task object.</summary>
		/// <param name="cancellationToken">The cancellation instruction.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.ArgumentException">An invalid <see cref="T:System.Data.CommandBehavior" /> value.</exception>
		/// <exception cref="T:System.InvalidOperationException">Calling <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior,System.Threading.CancellationToken)" /> more than once for the same instance before task completion.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.  
		///  <see langword="Context Connection=true" /> is specified in the connection string.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">SQL Server returned an error while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x06001588 RID: 5512 RVA: 0x000623E5 File Offset: 0x000605E5
		public new Task<SqlDataReader> ExecuteReaderAsync(CancellationToken cancellationToken)
		{
			return this.ExecuteReaderAsync(CommandBehavior.Default, cancellationToken);
		}

		/// <summary>An asynchronous version of <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteReader(System.Data.CommandBehavior)" />, which sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText" /> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection" />, and builds a <see cref="T:System.Data.SqlClient.SqlDataReader" />  
		///  The cancellation token can be used to request that the operation be abandoned before the command timeout elapses.  Exceptions will be reported via the returned Task object.</summary>
		/// <param name="behavior">Options for statement execution and data retrieval.  When is set to <see langword="Default" />, <see cref="M:System.Data.SqlClient.SqlDataReader.ReadAsync(System.Threading.CancellationToken)" /> reads the entire row before returning a complete Task.</param>
		/// <param name="cancellationToken">The cancellation instruction.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.ArgumentException">An invalid <see cref="T:System.Data.CommandBehavior" /> value.</exception>
		/// <exception cref="T:System.InvalidOperationException">Calling <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteReaderAsync(System.Data.CommandBehavior,System.Threading.CancellationToken)" /> more than once for the same instance before task completion.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.  
		///  <see langword="Context Connection=true" /> is specified in the connection string.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">SQL Server returned an error while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x06001589 RID: 5513 RVA: 0x000623F0 File Offset: 0x000605F0
		public new Task<SqlDataReader> ExecuteReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
		{
			Guid operationId = default(Guid);
			if (!this._parentOperationStarted)
			{
				operationId = SqlCommand._diagnosticListener.WriteCommandBefore(this, "ExecuteReaderAsync");
			}
			TaskCompletionSource<SqlDataReader> source = new TaskCompletionSource<SqlDataReader>();
			CancellationTokenRegistration registration = default(CancellationTokenRegistration);
			if (cancellationToken.CanBeCanceled)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					source.SetCanceled();
					return source.Task;
				}
				registration = cancellationToken.Register(delegate(object s)
				{
					((SqlCommand)s).CancelIgnoreFailure();
				}, this);
			}
			Task<SqlDataReader> task = source.Task;
			try
			{
				this.RegisterForConnectionCloseNotification<SqlDataReader>(ref task);
				Task<SqlDataReader>.Factory.FromAsync<CommandBehavior>(new Func<CommandBehavior, AsyncCallback, object, IAsyncResult>(this.BeginExecuteReader), new Func<IAsyncResult, SqlDataReader>(this.EndExecuteReader), behavior, null).ContinueWith(delegate(Task<SqlDataReader> t)
				{
					registration.Dispose();
					if (t.IsFaulted)
					{
						Exception innerException = t.Exception.InnerException;
						if (!this._parentOperationStarted)
						{
							SqlCommand._diagnosticListener.WriteCommandError(operationId, this, innerException, "ExecuteReaderAsync");
						}
						source.SetException(innerException);
						return;
					}
					if (t.IsCanceled)
					{
						source.SetCanceled();
					}
					else
					{
						source.SetResult(t.Result);
					}
					if (!this._parentOperationStarted)
					{
						SqlCommand._diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteReaderAsync");
					}
				}, TaskScheduler.Default);
			}
			catch (Exception ex)
			{
				if (!this._parentOperationStarted)
				{
					SqlCommand._diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteReaderAsync");
				}
				source.SetException(ex);
			}
			return task;
		}

		/// <summary>An asynchronous version of <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteScalar" />, which executes the query asynchronously and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.  
		///  The cancellation token can be used to request that the operation be abandoned before the command timeout elapses. Exceptions will be reported via the returned Task object.</summary>
		/// <param name="cancellationToken">The cancellation instruction.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">Calling <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteScalarAsync(System.Threading.CancellationToken)" /> more than once for the same instance before task completion.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.  
		///  <see langword="Context Connection=true" /> is specified in the connection string.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">SQL Server returned an error while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x0600158A RID: 5514 RVA: 0x00062534 File Offset: 0x00060734
		public override Task<object> ExecuteScalarAsync(CancellationToken cancellationToken)
		{
			this._parentOperationStarted = true;
			Guid operationId = SqlCommand._diagnosticListener.WriteCommandBefore(this, "ExecuteScalarAsync");
			return this.ExecuteReaderAsync(cancellationToken).ContinueWith<Task<object>>(delegate(Task<SqlDataReader> executeTask)
			{
				TaskCompletionSource<object> source = new TaskCompletionSource<object>();
				if (executeTask.IsCanceled)
				{
					source.SetCanceled();
				}
				else if (executeTask.IsFaulted)
				{
					SqlCommand._diagnosticListener.WriteCommandError(operationId, this, executeTask.Exception.InnerException, "ExecuteScalarAsync");
					source.SetException(executeTask.Exception.InnerException);
				}
				else
				{
					SqlDataReader reader = executeTask.Result;
					reader.ReadAsync(cancellationToken).ContinueWith(delegate(Task<bool> readTask)
					{
						try
						{
							if (readTask.IsCanceled)
							{
								reader.Dispose();
								source.SetCanceled();
							}
							else if (readTask.IsFaulted)
							{
								reader.Dispose();
								SqlCommand._diagnosticListener.WriteCommandError(operationId, this, readTask.Exception.InnerException, "ExecuteScalarAsync");
								source.SetException(readTask.Exception.InnerException);
							}
							else
							{
								Exception ex = null;
								object obj = null;
								try
								{
									if (readTask.Result && reader.FieldCount > 0)
									{
										try
										{
											obj = reader.GetValue(0);
										}
										catch (Exception ex)
										{
										}
									}
								}
								finally
								{
									reader.Dispose();
								}
								if (ex != null)
								{
									SqlCommand._diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteScalarAsync");
									source.SetException(ex);
								}
								else
								{
									SqlCommand._diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteScalarAsync");
									source.SetResult(obj);
								}
							}
						}
						catch (Exception ex2)
						{
							source.SetException(ex2);
						}
					}, TaskScheduler.Default);
				}
				this._parentOperationStarted = false;
				return source.Task;
			}, TaskScheduler.Default).Unwrap<object>();
		}

		/// <summary>An asynchronous version of <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteXmlReader" />, which sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText" /> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection" /> and builds an <see cref="T:System.Xml.XmlReader" /> object.  
		///  Exceptions will be reported via the returned Task object.</summary>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">Calling <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteScalarAsync(System.Threading.CancellationToken)" /> more than once for the same instance before task completion.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.  
		///  <see langword="Context Connection=true" /> is specified in the connection string.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">SQL Server returned an error while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x0600158B RID: 5515 RVA: 0x00062599 File Offset: 0x00060799
		public Task<XmlReader> ExecuteXmlReaderAsync()
		{
			return this.ExecuteXmlReaderAsync(CancellationToken.None);
		}

		/// <summary>An asynchronous version of <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteXmlReader" />, which sends the <see cref="P:System.Data.SqlClient.SqlCommand.CommandText" /> to the <see cref="P:System.Data.SqlClient.SqlCommand.Connection" /> and builds an <see cref="T:System.Xml.XmlReader" /> object.  
		///  The cancellation token can be used to request that the operation be abandoned before the command timeout elapses.  Exceptions will be reported via the returned Task object.</summary>
		/// <param name="cancellationToken">The cancellation instruction.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.InvalidOperationException">Calling <see cref="M:System.Data.SqlClient.SqlCommand.ExecuteScalarAsync(System.Threading.CancellationToken)" /> more than once for the same instance before task completion.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.  
		///  <see langword="Context Connection=true" /> is specified in the connection string.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">SQL Server returned an error while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x0600158C RID: 5516 RVA: 0x000625A8 File Offset: 0x000607A8
		public Task<XmlReader> ExecuteXmlReaderAsync(CancellationToken cancellationToken)
		{
			Guid operationId = SqlCommand._diagnosticListener.WriteCommandBefore(this, "ExecuteXmlReaderAsync");
			TaskCompletionSource<XmlReader> source = new TaskCompletionSource<XmlReader>();
			CancellationTokenRegistration registration = default(CancellationTokenRegistration);
			if (cancellationToken.CanBeCanceled)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					source.SetCanceled();
					return source.Task;
				}
				registration = cancellationToken.Register(delegate(object s)
				{
					((SqlCommand)s).CancelIgnoreFailure();
				}, this);
			}
			Task<XmlReader> task = source.Task;
			try
			{
				this.RegisterForConnectionCloseNotification<XmlReader>(ref task);
				Task<XmlReader>.Factory.FromAsync(new Func<AsyncCallback, object, IAsyncResult>(this.BeginExecuteXmlReader), new Func<IAsyncResult, XmlReader>(this.EndExecuteXmlReader), null).ContinueWith(delegate(Task<XmlReader> t)
				{
					registration.Dispose();
					if (t.IsFaulted)
					{
						Exception innerException = t.Exception.InnerException;
						SqlCommand._diagnosticListener.WriteCommandError(operationId, this, innerException, "ExecuteXmlReaderAsync");
						source.SetException(innerException);
						return;
					}
					if (t.IsCanceled)
					{
						source.SetCanceled();
					}
					else
					{
						source.SetResult(t.Result);
					}
					SqlCommand._diagnosticListener.WriteCommandAfter(operationId, this, "ExecuteXmlReaderAsync");
				}, TaskScheduler.Default);
			}
			catch (Exception ex)
			{
				SqlCommand._diagnosticListener.WriteCommandError(operationId, this, ex, "ExecuteXmlReaderAsync");
				source.SetException(ex);
			}
			return task;
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x000626CC File Offset: 0x000608CC
		private static string UnquoteProcedurePart(string part)
		{
			if (part != null && 2 <= part.Length && '[' == part[0] && ']' == part[part.Length - 1])
			{
				part = part.Substring(1, part.Length - 2);
				part = part.Replace("]]", "]");
			}
			return part;
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x00062728 File Offset: 0x00060928
		private static string UnquoteProcedureName(string name, out object groupNumber)
		{
			groupNumber = null;
			string text = name;
			if (text != null)
			{
				if (char.IsDigit(text[text.Length - 1]))
				{
					int num = text.LastIndexOf(';');
					if (num != -1)
					{
						string text2 = text.Substring(num + 1);
						int num2 = 0;
						if (int.TryParse(text2, out num2))
						{
							groupNumber = num2;
							text = text.Substring(0, num);
						}
					}
				}
				text = SqlCommand.UnquoteProcedurePart(text);
			}
			return text;
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x0006278C File Offset: 0x0006098C
		internal void DeriveParameters()
		{
			CommandType commandType = this.CommandType;
			if (commandType == CommandType.Text)
			{
				throw ADP.DeriveParametersNotSupported(this);
			}
			if (commandType != CommandType.StoredProcedure)
			{
				if (commandType != CommandType.TableDirect)
				{
					throw ADP.InvalidCommandType(this.CommandType);
				}
				throw ADP.DeriveParametersNotSupported(this);
			}
			else
			{
				this.ValidateCommand(false, "DeriveParameters");
				string[] array = MultipartIdentifier.ParseMultipartIdentifier(this.CommandText, "[\"", "]\"", "SqlCommand.DeriveParameters failed because the SqlCommand.CommandText property value is an invalid multipart name", false);
				if (array[3] == null || string.IsNullOrEmpty(array[3]))
				{
					throw ADP.NoStoredProcedureExists(this.CommandText);
				}
				SqlCommand sqlCommand = null;
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(array[0]))
				{
					SqlCommandSet.BuildStoredProcedureName(stringBuilder, array[0]);
					stringBuilder.Append(".");
				}
				if (string.IsNullOrEmpty(array[1]))
				{
					array[1] = this.Connection.Database;
				}
				SqlCommandSet.BuildStoredProcedureName(stringBuilder, array[1]);
				stringBuilder.Append(".");
				string[] array2;
				bool flag;
				if (this.Connection.IsKatmaiOrNewer)
				{
					stringBuilder.Append("[sys].[").Append("sp_procedure_params_100_managed").Append("]");
					array2 = SqlCommand.KatmaiProcParamsNames;
					flag = true;
				}
				else
				{
					stringBuilder.Append("[sys].[").Append("sp_procedure_params_managed").Append("]");
					array2 = SqlCommand.PreKatmaiProcParamsNames;
					flag = false;
				}
				sqlCommand = new SqlCommand(stringBuilder.ToString(), this.Connection, this.Transaction)
				{
					CommandType = CommandType.StoredProcedure
				};
				sqlCommand.Parameters.Add(new SqlParameter("@procedure_name", SqlDbType.NVarChar, 255));
				object obj;
				sqlCommand.Parameters[0].Value = SqlCommand.UnquoteProcedureName(array[3], out obj);
				if (obj != null)
				{
					sqlCommand.Parameters.Add(new SqlParameter("@group_number", SqlDbType.Int)).Value = obj;
				}
				if (!string.IsNullOrEmpty(array[2]))
				{
					sqlCommand.Parameters.Add(new SqlParameter("@procedure_schema", SqlDbType.NVarChar, 255)).Value = SqlCommand.UnquoteProcedurePart(array[2]);
				}
				SqlDataReader sqlDataReader = null;
				List<SqlParameter> list = new List<SqlParameter>();
				bool flag2 = true;
				try
				{
					sqlDataReader = sqlCommand.ExecuteReader();
					while (sqlDataReader.Read())
					{
						SqlParameter sqlParameter = new SqlParameter
						{
							ParameterName = (string)sqlDataReader[array2[0]]
						};
						if (flag)
						{
							sqlParameter.SqlDbType = (SqlDbType)((short)sqlDataReader[array2[3]]);
							SqlDbType sqlDbType = sqlParameter.SqlDbType;
							if (sqlDbType <= SqlDbType.NText)
							{
								if (sqlDbType != SqlDbType.Image)
								{
									if (sqlDbType != SqlDbType.NText)
									{
										goto IL_02BA;
									}
									sqlParameter.SqlDbType = SqlDbType.NVarChar;
									goto IL_02BA;
								}
							}
							else
							{
								if (sqlDbType == SqlDbType.Text)
								{
									sqlParameter.SqlDbType = SqlDbType.VarChar;
									goto IL_02BA;
								}
								if (sqlDbType != SqlDbType.Timestamp)
								{
									goto IL_02BA;
								}
							}
							sqlParameter.SqlDbType = SqlDbType.VarBinary;
						}
						else
						{
							sqlParameter.SqlDbType = MetaType.GetSqlDbTypeFromOleDbType((short)sqlDataReader[array2[2]], ADP.IsNull(sqlDataReader[array2[9]]) ? ADP.StrEmpty : ((string)sqlDataReader[array2[9]]));
						}
						IL_02BA:
						object obj2 = sqlDataReader[array2[4]];
						if (obj2 is int)
						{
							int num = (int)obj2;
							if (num == 0 && (sqlParameter.SqlDbType == SqlDbType.NVarChar || sqlParameter.SqlDbType == SqlDbType.VarBinary || sqlParameter.SqlDbType == SqlDbType.VarChar))
							{
								num = -1;
							}
							sqlParameter.Size = num;
						}
						sqlParameter.Direction = this.ParameterDirectionFromOleDbDirection((short)sqlDataReader[array2[1]]);
						if (sqlParameter.SqlDbType == SqlDbType.Decimal)
						{
							sqlParameter.ScaleInternal = (byte)((short)sqlDataReader[array2[6]] & 255);
							sqlParameter.PrecisionInternal = (byte)((short)sqlDataReader[array2[5]] & 255);
						}
						if (SqlDbType.Udt == sqlParameter.SqlDbType)
						{
							string text;
							if (flag)
							{
								text = (string)sqlDataReader[array2[9]];
							}
							else
							{
								text = (string)sqlDataReader[array2[13]];
							}
							SqlParameter sqlParameter2 = sqlParameter;
							string[] array3 = new string[5];
							int num2 = 0;
							object obj3 = sqlDataReader[array2[7]];
							array3[num2] = ((obj3 != null) ? obj3.ToString() : null);
							array3[1] = ".";
							int num3 = 2;
							object obj4 = sqlDataReader[array2[8]];
							array3[num3] = ((obj4 != null) ? obj4.ToString() : null);
							array3[3] = ".";
							array3[4] = text;
							sqlParameter2.UdtTypeName = string.Concat(array3);
						}
						if (SqlDbType.Structured == sqlParameter.SqlDbType)
						{
							SqlParameter sqlParameter3 = sqlParameter;
							string[] array4 = new string[5];
							int num4 = 0;
							object obj5 = sqlDataReader[array2[7]];
							array4[num4] = ((obj5 != null) ? obj5.ToString() : null);
							array4[1] = ".";
							int num5 = 2;
							object obj6 = sqlDataReader[array2[8]];
							array4[num5] = ((obj6 != null) ? obj6.ToString() : null);
							array4[3] = ".";
							int num6 = 4;
							object obj7 = sqlDataReader[array2[9]];
							array4[num6] = ((obj7 != null) ? obj7.ToString() : null);
							sqlParameter3.TypeName = string.Concat(array4);
						}
						if (SqlDbType.Xml == sqlParameter.SqlDbType)
						{
							object obj8 = sqlDataReader[array2[10]];
							sqlParameter.XmlSchemaCollectionDatabase = (ADP.IsNull(obj8) ? string.Empty : ((string)obj8));
							obj8 = sqlDataReader[array2[11]];
							sqlParameter.XmlSchemaCollectionOwningSchema = (ADP.IsNull(obj8) ? string.Empty : ((string)obj8));
							obj8 = sqlDataReader[array2[12]];
							sqlParameter.XmlSchemaCollectionName = (ADP.IsNull(obj8) ? string.Empty : ((string)obj8));
						}
						if (MetaType._IsVarTime(sqlParameter.SqlDbType))
						{
							object obj9 = sqlDataReader[array2[14]];
							if (obj9 is int)
							{
								sqlParameter.ScaleInternal = (byte)((int)obj9 & 255);
							}
						}
						list.Add(sqlParameter);
					}
				}
				catch (Exception ex)
				{
					flag2 = ADP.IsCatchableExceptionType(ex);
					throw;
				}
				finally
				{
					if (flag2)
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
						sqlCommand.Connection = null;
					}
				}
				if (list.Count == 0)
				{
					throw ADP.NoStoredProcedureExists(this.CommandText);
				}
				this.Parameters.Clear();
				foreach (SqlParameter sqlParameter4 in list)
				{
					this._parameters.Add(sqlParameter4);
				}
				return;
			}
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x00062DB4 File Offset: 0x00060FB4
		private ParameterDirection ParameterDirectionFromOleDbDirection(short oledbDirection)
		{
			switch (oledbDirection)
			{
			case 2:
				return ParameterDirection.InputOutput;
			case 3:
				return ParameterDirection.Output;
			case 4:
				return ParameterDirection.ReturnValue;
			default:
				return ParameterDirection.Input;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001591 RID: 5521 RVA: 0x00062DD3 File Offset: 0x00060FD3
		internal _SqlMetaDataSet MetaData
		{
			get
			{
				return this._cachedMetaData;
			}
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00062DDC File Offset: 0x00060FDC
		private void CheckNotificationStateAndAutoEnlist()
		{
			if (this.Notification != null && this._sqlDep != null)
			{
				if (this._sqlDep.Options == null)
				{
					SqlInternalConnectionTds sqlInternalConnectionTds = this._activeConnection.InnerConnection as SqlInternalConnectionTds;
					SqlDependency.IdentityUserNamePair identityUserNamePair;
					if (sqlInternalConnectionTds.Identity != null)
					{
						identityUserNamePair = new SqlDependency.IdentityUserNamePair(sqlInternalConnectionTds.Identity, null);
					}
					else
					{
						identityUserNamePair = new SqlDependency.IdentityUserNamePair(null, sqlInternalConnectionTds.ConnectionOptions.UserID);
					}
					this.Notification.Options = SqlDependency.GetDefaultComposedOptions(this._activeConnection.DataSource, this.InternalTdsConnection.ServerProvidedFailOverPartner, identityUserNamePair, this._activeConnection.Database);
				}
				this.Notification.UserData = this._sqlDep.ComputeHashAndAddToDispatcher(this);
				this._sqlDep.AddToServerList(this._activeConnection.DataSource);
			}
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x00062EA8 File Offset: 0x000610A8
		private Task RunExecuteNonQueryTds(string methodName, bool async, int timeout, bool asyncWrite)
		{
			bool flag = true;
			try
			{
				Task task = this._activeConnection.ValidateAndReconnect(null, timeout);
				if (task != null)
				{
					long reconnectionStart = ADP.TimerCurrent();
					if (async)
					{
						TaskCompletionSource<object> completion = new TaskCompletionSource<object>();
						this._activeConnection.RegisterWaitingForReconnect(completion.Task);
						this._reconnectionCompletionSource = completion;
						CancellationTokenSource timeoutCTS = new CancellationTokenSource();
						AsyncHelper.SetTimeoutException(completion, timeout, new Func<Exception>(SQL.CR_ReconnectTimeout), timeoutCTS.Token);
						Action <>9__2;
						AsyncHelper.ContinueTask(task, completion, delegate
						{
							if (completion.Task.IsCompleted)
							{
								return;
							}
							Interlocked.CompareExchange<TaskCompletionSource<object>>(ref this._reconnectionCompletionSource, null, completion);
							timeoutCTS.Cancel();
							Task task2 = this.RunExecuteNonQueryTds(methodName, async, TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart), asyncWrite);
							if (task2 == null)
							{
								completion.SetResult(null);
								return;
							}
							Task task3 = task2;
							TaskCompletionSource<object> completion2 = completion;
							Action action;
							if ((action = <>9__2) == null)
							{
								action = (<>9__2 = delegate
								{
									completion.SetResult(null);
								});
							}
							AsyncHelper.ContinueTask(task3, completion2, action, null, null, null, null, null);
						}, null, null, null, null, this._activeConnection);
						return completion.Task;
					}
					AsyncHelper.WaitForCompletion(task, timeout, delegate
					{
						throw SQL.CR_ReconnectTimeout();
					}, true);
					timeout = TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart);
				}
				if (asyncWrite)
				{
					this._activeConnection.AddWeakReference(this, 2);
				}
				this.GetStateObject(null);
				this._stateObj.Parser.TdsExecuteSQLBatch(this.CommandText, timeout, this.Notification, this._stateObj, true, false);
				this.NotifyDependency();
				bool flag2;
				if (async)
				{
					this._activeConnection.GetOpenTdsConnection(methodName).IncrementAsyncCount();
				}
				else if (!this._stateObj.Parser.TryRun(RunBehavior.UntilDone, this, null, null, this._stateObj, out flag2))
				{
					throw SQL.SynchronousCallMayNotPend();
				}
			}
			catch (Exception ex)
			{
				flag = ADP.IsCatchableExceptionType(ex);
				throw;
			}
			finally
			{
				if (flag && !async)
				{
					this.PutStateObject();
				}
			}
			return null;
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x000630CC File Offset: 0x000612CC
		internal SqlDataReader RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, [CallerMemberName] string method = "")
		{
			Task task;
			return this.RunExecuteReader(cmdBehavior, runBehavior, returnStream, null, this.CommandTimeout, out task, false, method);
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x000630F0 File Offset: 0x000612F0
		internal SqlDataReader RunExecuteReader(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, TaskCompletionSource<object> completion, int timeout, out Task task, bool asyncWrite = false, [CallerMemberName] string method = "")
		{
			bool flag = completion != null;
			task = null;
			this._rowsAffected = -1;
			if ((CommandBehavior.SingleRow & cmdBehavior) != CommandBehavior.Default)
			{
				cmdBehavior |= CommandBehavior.SingleResult;
			}
			this.ValidateCommand(flag, method);
			this.CheckNotificationStateAndAutoEnlist();
			SqlStatistics statistics = this.Statistics;
			if (statistics != null)
			{
				if ((!this.IsDirty && this.IsPrepared && !this._hiddenPrepare) || (this.IsPrepared && this._execType == SqlCommand.EXECTYPE.PREPAREPENDING))
				{
					statistics.SafeIncrement(ref statistics._preparedExecs);
				}
				else
				{
					statistics.SafeIncrement(ref statistics._unpreparedExecs);
				}
			}
			return this.RunExecuteReaderTds(cmdBehavior, runBehavior, returnStream, flag, timeout, out task, asyncWrite && flag, null);
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x0006318C File Offset: 0x0006138C
		private SqlDataReader RunExecuteReaderTds(CommandBehavior cmdBehavior, RunBehavior runBehavior, bool returnStream, bool async, int timeout, out Task task, bool asyncWrite, SqlDataReader ds = null)
		{
			if ((ds == null) & returnStream)
			{
				ds = new SqlDataReader(this, cmdBehavior);
			}
			Task task2 = this._activeConnection.ValidateAndReconnect(null, timeout);
			if (task2 != null)
			{
				long reconnectionStart = ADP.TimerCurrent();
				if (async)
				{
					TaskCompletionSource<object> completion = new TaskCompletionSource<object>();
					this._activeConnection.RegisterWaitingForReconnect(completion.Task);
					this._reconnectionCompletionSource = completion;
					CancellationTokenSource timeoutCTS = new CancellationTokenSource();
					AsyncHelper.SetTimeoutException(completion, timeout, new Func<Exception>(SQL.CR_ReconnectTimeout), timeoutCTS.Token);
					Action <>9__2;
					AsyncHelper.ContinueTask(task2, completion, delegate
					{
						if (completion.Task.IsCompleted)
						{
							return;
						}
						Interlocked.CompareExchange<TaskCompletionSource<object>>(ref this._reconnectionCompletionSource, null, completion);
						timeoutCTS.Cancel();
						Task task4;
						this.RunExecuteReaderTds(cmdBehavior, runBehavior, returnStream, async, TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart), out task4, asyncWrite, ds);
						if (task4 == null)
						{
							completion.SetResult(null);
							return;
						}
						Task task5 = task4;
						TaskCompletionSource<object> completion2 = completion;
						Action action;
						if ((action = <>9__2) == null)
						{
							action = (<>9__2 = delegate
							{
								completion.SetResult(null);
							});
						}
						AsyncHelper.ContinueTask(task5, completion2, action, null, null, null, null, null);
					}, null, null, null, null, this._activeConnection);
					task = completion.Task;
					return ds;
				}
				AsyncHelper.WaitForCompletion(task2, timeout, delegate
				{
					throw SQL.CR_ReconnectTimeout();
				}, true);
				timeout = TdsParserStaticMethods.GetRemainingTimeout(timeout, reconnectionStart);
			}
			bool flag = (cmdBehavior & CommandBehavior.SchemaOnly) > CommandBehavior.Default;
			_SqlRPC sqlRPC = null;
			task = null;
			string optionSettings = null;
			bool flag2 = true;
			bool flag3 = false;
			if (async)
			{
				this._activeConnection.GetOpenTdsConnection().IncrementAsyncCount();
				flag3 = true;
			}
			try
			{
				if (asyncWrite)
				{
					this._activeConnection.AddWeakReference(this, 2);
				}
				this.GetStateObject(null);
				Task task3;
				if (this.BatchRPCMode)
				{
					task3 = this._stateObj.Parser.TdsExecuteRPC(this._SqlRPCBatchArray, timeout, flag, this.Notification, this._stateObj, CommandType.StoredProcedure == this.CommandType, !asyncWrite, null, 0, 0);
				}
				else if (CommandType.Text == this.CommandType && this.GetParameterCount(this._parameters) == 0)
				{
					string text = this.GetCommandText(cmdBehavior) + this.GetResetOptionsString(cmdBehavior);
					task3 = this._stateObj.Parser.TdsExecuteSQLBatch(text, timeout, this.Notification, this._stateObj, !asyncWrite, false);
				}
				else if (CommandType.Text == this.CommandType)
				{
					if (this.IsDirty)
					{
						if (this._execType == SqlCommand.EXECTYPE.PREPARED)
						{
							this._hiddenPrepare = true;
						}
						this.Unprepare();
						this.IsDirty = false;
					}
					if (this._execType == SqlCommand.EXECTYPE.PREPARED)
					{
						sqlRPC = this.BuildExecute(flag);
					}
					else if (this._execType == SqlCommand.EXECTYPE.PREPAREPENDING)
					{
						sqlRPC = this.BuildPrepExec(cmdBehavior);
						this._execType = SqlCommand.EXECTYPE.PREPARED;
						this._preparedConnectionCloseCount = this._activeConnection.CloseCount;
						this._preparedConnectionReconnectCount = this._activeConnection.ReconnectCount;
						this._inPrepare = true;
					}
					else
					{
						this.BuildExecuteSql(cmdBehavior, null, this._parameters, ref sqlRPC);
					}
					sqlRPC.options = 2;
					task3 = this._stateObj.Parser.TdsExecuteRPC(this._rpcArrayOf1, timeout, flag, this.Notification, this._stateObj, CommandType.StoredProcedure == this.CommandType, !asyncWrite, null, 0, 0);
				}
				else
				{
					this.BuildRPC(flag, this._parameters, ref sqlRPC);
					optionSettings = this.GetSetOptionsString(cmdBehavior);
					if (optionSettings != null)
					{
						this._stateObj.Parser.TdsExecuteSQLBatch(optionSettings, timeout, this.Notification, this._stateObj, true, false);
						bool flag4;
						if (!this._stateObj.Parser.TryRun(RunBehavior.UntilDone, this, null, null, this._stateObj, out flag4))
						{
							throw SQL.SynchronousCallMayNotPend();
						}
						optionSettings = this.GetResetOptionsString(cmdBehavior);
					}
					task3 = this._stateObj.Parser.TdsExecuteRPC(this._rpcArrayOf1, timeout, flag, this.Notification, this._stateObj, CommandType.StoredProcedure == this.CommandType, !asyncWrite, null, 0, 0);
				}
				if (async)
				{
					flag3 = false;
					if (task3 != null)
					{
						task = AsyncHelper.CreateContinuationTask(task3, delegate
						{
							this._activeConnection.GetOpenTdsConnection();
							this.cachedAsyncState.SetAsyncReaderState(ds, runBehavior, optionSettings);
						}, null, delegate(Exception exc)
						{
							this._activeConnection.GetOpenTdsConnection().DecrementAsyncCount();
						});
					}
					else
					{
						this.cachedAsyncState.SetAsyncReaderState(ds, runBehavior, optionSettings);
					}
				}
				else
				{
					this.FinishExecuteReader(ds, runBehavior, optionSettings);
				}
			}
			catch (Exception ex)
			{
				flag2 = ADP.IsCatchableExceptionType(ex);
				if (flag3)
				{
					SqlInternalConnectionTds sqlInternalConnectionTds = this._activeConnection.InnerConnection as SqlInternalConnectionTds;
					if (sqlInternalConnectionTds != null)
					{
						sqlInternalConnectionTds.DecrementAsyncCount();
					}
				}
				throw;
			}
			finally
			{
				if (flag2 && !async)
				{
					this.PutStateObject();
				}
			}
			return ds;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x000636F4 File Offset: 0x000618F4
		private SqlDataReader CompleteAsyncExecuteReader()
		{
			SqlDataReader cachedAsyncReader = this.cachedAsyncState.CachedAsyncReader;
			bool flag = true;
			try
			{
				this.FinishExecuteReader(cachedAsyncReader, this.cachedAsyncState.CachedRunBehavior, this.cachedAsyncState.CachedSetOptions);
			}
			catch (Exception ex)
			{
				flag = ADP.IsCatchableExceptionType(ex);
				throw;
			}
			finally
			{
				if (flag)
				{
					this.cachedAsyncState.ResetAsyncState();
					this.PutStateObject();
				}
			}
			return cachedAsyncReader;
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x00063768 File Offset: 0x00061968
		private void FinishExecuteReader(SqlDataReader ds, RunBehavior runBehavior, string resetOptionsString)
		{
			this.NotifyDependency();
			if (runBehavior == RunBehavior.UntilDone)
			{
				try
				{
					bool flag;
					if (!this._stateObj.Parser.TryRun(RunBehavior.UntilDone, this, ds, null, this._stateObj, out flag))
					{
						throw SQL.SynchronousCallMayNotPend();
					}
				}
				catch (Exception ex)
				{
					if (ADP.IsCatchableExceptionType(ex))
					{
						if (this._inPrepare)
						{
							this._inPrepare = false;
							this.IsDirty = true;
							this._execType = SqlCommand.EXECTYPE.PREPAREPENDING;
						}
						if (ds != null)
						{
							try
							{
								ds.Close();
							}
							catch (Exception)
							{
							}
						}
					}
					throw;
				}
			}
			if (ds != null)
			{
				ds.Bind(this._stateObj);
				this._stateObj = null;
				ds.ResetOptionsString = resetOptionsString;
				this._activeConnection.AddWeakReference(ds, 1);
				try
				{
					this._cachedMetaData = ds.MetaData;
					ds.IsInitialized = true;
				}
				catch (Exception ex2)
				{
					if (ADP.IsCatchableExceptionType(ex2))
					{
						if (this._inPrepare)
						{
							this._inPrepare = false;
							this.IsDirty = true;
							this._execType = SqlCommand.EXECTYPE.PREPAREPENDING;
						}
						try
						{
							ds.Close();
						}
						catch (Exception)
						{
						}
					}
					throw;
				}
			}
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00063880 File Offset: 0x00061A80
		private void RegisterForConnectionCloseNotification<T>(ref Task<T> outerTask)
		{
			SqlConnection activeConnection = this._activeConnection;
			if (activeConnection == null)
			{
				throw ADP.ClosedConnectionError();
			}
			activeConnection.RegisterForConnectionCloseNotification<T>(ref outerTask, this, 2);
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0006389C File Offset: 0x00061A9C
		private void ValidateCommand(bool async, [CallerMemberName] string method = "")
		{
			if (this._activeConnection == null)
			{
				throw ADP.ConnectionRequired(method);
			}
			SqlInternalConnectionTds sqlInternalConnectionTds = this._activeConnection.InnerConnection as SqlInternalConnectionTds;
			if (sqlInternalConnectionTds != null)
			{
				TdsParser parser = sqlInternalConnectionTds.Parser;
				if (parser == null || parser.State == TdsParserState.Closed)
				{
					throw ADP.OpenConnectionRequired(method, ConnectionState.Closed);
				}
				if (parser.State != TdsParserState.OpenLoggedIn)
				{
					throw ADP.OpenConnectionRequired(method, ConnectionState.Broken);
				}
			}
			else
			{
				if (this._activeConnection.State == ConnectionState.Closed)
				{
					throw ADP.OpenConnectionRequired(method, ConnectionState.Closed);
				}
				if (this._activeConnection.State == ConnectionState.Broken)
				{
					throw ADP.OpenConnectionRequired(method, ConnectionState.Broken);
				}
			}
			this.ValidateAsyncCommand();
			this._activeConnection.ValidateConnectionForExecute(method, this);
			if (this._transaction != null && this._transaction.Connection == null)
			{
				this._transaction = null;
			}
			if (this._activeConnection.HasLocalTransactionFromAPI && this._transaction == null)
			{
				throw ADP.TransactionRequired(method);
			}
			if (this._transaction != null && this._activeConnection != this._transaction.Connection)
			{
				throw ADP.TransactionConnectionMismatch();
			}
			if (string.IsNullOrEmpty(this.CommandText))
			{
				throw ADP.CommandTextRequired(method);
			}
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x000639A5 File Offset: 0x00061BA5
		private void ValidateAsyncCommand()
		{
			if (this.cachedAsyncState.PendingAsyncOperation)
			{
				if (this.cachedAsyncState.IsActiveConnectionValid(this._activeConnection))
				{
					throw SQL.PendingBeginXXXExists();
				}
				this._stateObj = null;
				this.cachedAsyncState.ResetAsyncState();
			}
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x000639E0 File Offset: 0x00061BE0
		private void GetStateObject(TdsParser parser = null)
		{
			if (this._pendingCancel)
			{
				this._pendingCancel = false;
				throw SQL.OperationCancelled();
			}
			if (parser == null)
			{
				parser = this._activeConnection.Parser;
				if (parser == null || parser.State == TdsParserState.Broken || parser.State == TdsParserState.Closed)
				{
					throw ADP.ClosedConnectionError();
				}
			}
			TdsParserStateObject session = parser.GetSession(this);
			session.StartSession(this);
			this._stateObj = session;
			if (this._pendingCancel)
			{
				this._pendingCancel = false;
				throw SQL.OperationCancelled();
			}
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x00063A5F File Offset: 0x00061C5F
		private void ReliablePutStateObject()
		{
			this.PutStateObject();
		}

		// Token: 0x0600159E RID: 5534 RVA: 0x00063A68 File Offset: 0x00061C68
		private void PutStateObject()
		{
			TdsParserStateObject stateObj = this._stateObj;
			this._stateObj = null;
			if (stateObj != null)
			{
				stateObj.CloseSession();
			}
		}

		// Token: 0x0600159F RID: 5535 RVA: 0x00063A8C File Offset: 0x00061C8C
		internal void OnDoneProc()
		{
			if (this.BatchRPCMode)
			{
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].cumulativeRecordsAffected = this._rowsAffected;
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].recordsAffected = new int?((0 < this._currentlyExecutingBatch && 0 <= this._rowsAffected) ? (this._rowsAffected - Math.Max(this._SqlRPCBatchArray[this._currentlyExecutingBatch - 1].cumulativeRecordsAffected, 0)) : this._rowsAffected);
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].errorsIndexStart = ((0 < this._currentlyExecutingBatch) ? this._SqlRPCBatchArray[this._currentlyExecutingBatch - 1].errorsIndexEnd : 0);
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].errorsIndexEnd = this._stateObj.ErrorCount;
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].errors = this._stateObj._errors;
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].warningsIndexStart = ((0 < this._currentlyExecutingBatch) ? this._SqlRPCBatchArray[this._currentlyExecutingBatch - 1].warningsIndexEnd : 0);
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].warningsIndexEnd = this._stateObj.WarningCount;
				this._SqlRPCBatchArray[this._currentlyExecutingBatch].warnings = this._stateObj._warnings;
				this._currentlyExecutingBatch++;
			}
		}

		// Token: 0x060015A0 RID: 5536 RVA: 0x00063BF4 File Offset: 0x00061DF4
		internal void OnReturnStatus(int status)
		{
			if (this._inPrepare)
			{
				return;
			}
			SqlParameterCollection sqlParameterCollection = this._parameters;
			if (this.BatchRPCMode)
			{
				if (this._parameterCollectionList.Count > this._currentlyExecutingBatch)
				{
					sqlParameterCollection = this._parameterCollectionList[this._currentlyExecutingBatch];
				}
				else
				{
					sqlParameterCollection = null;
				}
			}
			int parameterCount = this.GetParameterCount(sqlParameterCollection);
			int i = 0;
			while (i < parameterCount)
			{
				SqlParameter sqlParameter = sqlParameterCollection[i];
				if (sqlParameter.Direction == ParameterDirection.ReturnValue)
				{
					object value = sqlParameter.Value;
					if (value != null && value.GetType() == typeof(SqlInt32))
					{
						sqlParameter.Value = new SqlInt32(status);
						return;
					}
					sqlParameter.Value = status;
					return;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x060015A1 RID: 5537 RVA: 0x00063CAC File Offset: 0x00061EAC
		internal void OnReturnValue(SqlReturnValue rec, TdsParserStateObject stateObj)
		{
			if (this._inPrepare)
			{
				if (!rec.value.IsNull)
				{
					this._prepareHandle = rec.value.Int32;
				}
				this._inPrepare = false;
				return;
			}
			SqlParameterCollection currentParameterCollection = this.GetCurrentParameterCollection();
			int parameterCount = this.GetParameterCount(currentParameterCollection);
			SqlParameter parameterForOutputValueExtraction = this.GetParameterForOutputValueExtraction(currentParameterCollection, rec.parameter, parameterCount);
			if (parameterForOutputValueExtraction != null)
			{
				object value = parameterForOutputValueExtraction.Value;
				if (SqlDbType.Udt == parameterForOutputValueExtraction.SqlDbType)
				{
					try
					{
						this.Connection.CheckGetExtendedUDTInfo(rec, true);
						object obj;
						if (rec.value.IsNull)
						{
							obj = DBNull.Value;
						}
						else
						{
							obj = rec.value.ByteArray;
						}
						parameterForOutputValueExtraction.Value = this.Connection.GetUdtValue(obj, rec, false);
					}
					catch (FileNotFoundException ex)
					{
						parameterForOutputValueExtraction.SetUdtLoadError(ex);
					}
					catch (FileLoadException ex2)
					{
						parameterForOutputValueExtraction.SetUdtLoadError(ex2);
					}
					return;
				}
				parameterForOutputValueExtraction.SetSqlBuffer(rec.value);
				MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(rec.type, false);
				if (rec.type == SqlDbType.Decimal)
				{
					parameterForOutputValueExtraction.ScaleInternal = rec.scale;
					parameterForOutputValueExtraction.PrecisionInternal = rec.precision;
				}
				else if (metaTypeFromSqlDbType.IsVarTime)
				{
					parameterForOutputValueExtraction.ScaleInternal = rec.scale;
				}
				else if (rec.type == SqlDbType.Xml)
				{
					SqlCachedBuffer sqlCachedBuffer = parameterForOutputValueExtraction.Value as SqlCachedBuffer;
					if (sqlCachedBuffer != null)
					{
						parameterForOutputValueExtraction.Value = sqlCachedBuffer.ToString();
					}
				}
				if (rec.collation != null)
				{
					parameterForOutputValueExtraction.Collation = rec.collation;
				}
			}
		}

		// Token: 0x060015A2 RID: 5538 RVA: 0x00063E2C File Offset: 0x0006202C
		private SqlParameterCollection GetCurrentParameterCollection()
		{
			if (!this.BatchRPCMode)
			{
				return this._parameters;
			}
			if (this._parameterCollectionList.Count > this._currentlyExecutingBatch)
			{
				return this._parameterCollectionList[this._currentlyExecutingBatch];
			}
			return null;
		}

		// Token: 0x060015A3 RID: 5539 RVA: 0x00063E64 File Offset: 0x00062064
		private SqlParameter GetParameterForOutputValueExtraction(SqlParameterCollection parameters, string paramName, int paramCount)
		{
			SqlParameter sqlParameter = null;
			bool flag = false;
			if (paramName == null)
			{
				for (int i = 0; i < paramCount; i++)
				{
					sqlParameter = parameters[i];
					if (sqlParameter.Direction == ParameterDirection.ReturnValue)
					{
						flag = true;
						break;
					}
				}
			}
			else
			{
				for (int j = 0; j < paramCount; j++)
				{
					sqlParameter = parameters[j];
					if (sqlParameter.Direction != ParameterDirection.Input && sqlParameter.Direction != ParameterDirection.ReturnValue && paramName == sqlParameter.ParameterNameFixed)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return sqlParameter;
			}
			return null;
		}

		// Token: 0x060015A4 RID: 5540 RVA: 0x00063EDC File Offset: 0x000620DC
		private void GetRPCObject(int paramCount, ref _SqlRPC rpc)
		{
			if (rpc == null)
			{
				if (this._rpcArrayOf1 == null)
				{
					this._rpcArrayOf1 = new _SqlRPC[1];
					this._rpcArrayOf1[0] = new _SqlRPC();
				}
				rpc = this._rpcArrayOf1[0];
			}
			rpc.ProcID = 0;
			rpc.rpcName = null;
			rpc.options = 0;
			if (rpc.parameters == null || rpc.parameters.Length < paramCount)
			{
				rpc.parameters = new SqlParameter[paramCount];
			}
			else if (rpc.parameters.Length > paramCount)
			{
				rpc.parameters[paramCount] = null;
			}
			if (rpc.paramoptions == null || rpc.paramoptions.Length < paramCount)
			{
				rpc.paramoptions = new byte[paramCount];
				return;
			}
			for (int i = 0; i < paramCount; i++)
			{
				rpc.paramoptions[i] = 0;
			}
		}

		// Token: 0x060015A5 RID: 5541 RVA: 0x00063FA4 File Offset: 0x000621A4
		private void SetUpRPCParameters(_SqlRPC rpc, int startCount, bool inSchema, SqlParameterCollection parameters)
		{
			int parameterCount = this.GetParameterCount(parameters);
			int num = startCount;
			TdsParser parser = this._activeConnection.Parser;
			for (int i = 0; i < parameterCount; i++)
			{
				SqlParameter sqlParameter = parameters[i];
				sqlParameter.Validate(i, CommandType.StoredProcedure == this.CommandType);
				if (!sqlParameter.ValidateTypeLengths().IsPlp && sqlParameter.Direction != ParameterDirection.Output)
				{
					sqlParameter.FixStreamDataForNonPLP();
				}
				if (SqlCommand.ShouldSendParameter(sqlParameter))
				{
					rpc.parameters[num] = sqlParameter;
					if (sqlParameter.Direction == ParameterDirection.InputOutput || sqlParameter.Direction == ParameterDirection.Output)
					{
						rpc.paramoptions[num] = 1;
					}
					if (sqlParameter.Direction != ParameterDirection.Output && sqlParameter.Value == null && (!inSchema || SqlDbType.Structured == sqlParameter.SqlDbType))
					{
						byte[] paramoptions = rpc.paramoptions;
						int num2 = num;
						paramoptions[num2] |= 2;
					}
					num++;
				}
			}
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x00064070 File Offset: 0x00062270
		private _SqlRPC BuildPrepExec(CommandBehavior behavior)
		{
			int num = 3;
			int num2 = this.CountSendableParameters(this._parameters);
			_SqlRPC sqlRPC = null;
			this.GetRPCObject(num2 + num, ref sqlRPC);
			sqlRPC.ProcID = 13;
			sqlRPC.rpcName = "sp_prepexec";
			SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
			sqlParameter.Direction = ParameterDirection.InputOutput;
			sqlParameter.Value = this._prepareHandle;
			sqlRPC.parameters[0] = sqlParameter;
			sqlRPC.paramoptions[0] = 1;
			string text = this.BuildParamList(this._stateObj.Parser, this._parameters);
			sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
			sqlParameter.Value = text;
			sqlRPC.parameters[1] = sqlParameter;
			string commandText = this.GetCommandText(behavior);
			sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, commandText.Length);
			sqlParameter.Value = commandText;
			sqlRPC.parameters[2] = sqlParameter;
			this.SetUpRPCParameters(sqlRPC, num, false, this._parameters);
			return sqlRPC;
		}

		// Token: 0x060015A7 RID: 5543 RVA: 0x0006417C File Offset: 0x0006237C
		private static bool ShouldSendParameter(SqlParameter p)
		{
			ParameterDirection direction = p.Direction;
			return direction - ParameterDirection.Input <= 2;
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x000641A0 File Offset: 0x000623A0
		private int CountSendableParameters(SqlParameterCollection parameters)
		{
			int num = 0;
			if (parameters != null)
			{
				int count = parameters.Count;
				for (int i = 0; i < count; i++)
				{
					if (SqlCommand.ShouldSendParameter(parameters[i]))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x000641D8 File Offset: 0x000623D8
		private int GetParameterCount(SqlParameterCollection parameters)
		{
			if (parameters == null)
			{
				return 0;
			}
			return parameters.Count;
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x000641E8 File Offset: 0x000623E8
		private void BuildRPC(bool inSchema, SqlParameterCollection parameters, ref _SqlRPC rpc)
		{
			int num = this.CountSendableParameters(parameters);
			this.GetRPCObject(num, ref rpc);
			rpc.rpcName = this.CommandText;
			this.SetUpRPCParameters(rpc, 0, inSchema, parameters);
		}

		// Token: 0x060015AB RID: 5547 RVA: 0x00064220 File Offset: 0x00062420
		private _SqlRPC BuildExecute(bool inSchema)
		{
			int num = 1;
			int num2 = this.CountSendableParameters(this._parameters);
			_SqlRPC sqlRPC = null;
			this.GetRPCObject(num2 + num, ref sqlRPC);
			sqlRPC.ProcID = 12;
			sqlRPC.rpcName = "sp_execute";
			SqlParameter sqlParameter = new SqlParameter(null, SqlDbType.Int);
			sqlParameter.Value = this._prepareHandle;
			sqlRPC.parameters[0] = sqlParameter;
			this.SetUpRPCParameters(sqlRPC, num, inSchema, this._parameters);
			return sqlRPC;
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x00064290 File Offset: 0x00062490
		private void BuildExecuteSql(CommandBehavior behavior, string commandText, SqlParameterCollection parameters, ref _SqlRPC rpc)
		{
			int num = this.CountSendableParameters(parameters);
			int num2;
			if (num > 0)
			{
				num2 = 2;
			}
			else
			{
				num2 = 1;
			}
			this.GetRPCObject(num + num2, ref rpc);
			rpc.ProcID = 10;
			rpc.rpcName = "sp_executesql";
			if (commandText == null)
			{
				commandText = this.GetCommandText(behavior);
			}
			SqlParameter sqlParameter = new SqlParameter(null, (commandText.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, commandText.Length);
			sqlParameter.Value = commandText;
			rpc.parameters[0] = sqlParameter;
			if (num > 0)
			{
				string text = this.BuildParamList(this._stateObj.Parser, this.BatchRPCMode ? parameters : this._parameters);
				sqlParameter = new SqlParameter(null, (text.Length << 1 <= 8000) ? SqlDbType.NVarChar : SqlDbType.NText, text.Length);
				sqlParameter.Value = text;
				rpc.parameters[1] = sqlParameter;
				bool flag = (behavior & CommandBehavior.SchemaOnly) > CommandBehavior.Default;
				this.SetUpRPCParameters(rpc, num2, flag, parameters);
			}
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00064384 File Offset: 0x00062584
		internal string BuildParamList(TdsParser parser, SqlParameterCollection parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			int count = parameters.Count;
			for (int i = 0; i < count; i++)
			{
				SqlParameter sqlParameter = parameters[i];
				sqlParameter.Validate(i, CommandType.StoredProcedure == this.CommandType);
				if (SqlCommand.ShouldSendParameter(sqlParameter))
				{
					if (flag)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(sqlParameter.ParameterNameFixed);
					MetaType metaType = sqlParameter.InternalMetaType;
					stringBuilder.Append(" ");
					if (metaType.SqlDbType == SqlDbType.Udt)
					{
						string udtTypeName = sqlParameter.UdtTypeName;
						if (string.IsNullOrEmpty(udtTypeName))
						{
							throw SQL.MustSetUdtTypeNameForUdtParams();
						}
						stringBuilder.Append(this.ParseAndQuoteIdentifier(udtTypeName, true));
					}
					else if (metaType.SqlDbType == SqlDbType.Structured)
					{
						string typeName = sqlParameter.TypeName;
						if (string.IsNullOrEmpty(typeName))
						{
							throw SQL.MustSetTypeNameForParam(metaType.TypeName, sqlParameter.ParameterNameFixed);
						}
						stringBuilder.Append(this.ParseAndQuoteIdentifier(typeName, false));
						stringBuilder.Append(" READONLY");
					}
					else
					{
						metaType = sqlParameter.ValidateTypeLengths();
						if (!metaType.IsPlp && sqlParameter.Direction != ParameterDirection.Output)
						{
							sqlParameter.FixStreamDataForNonPLP();
						}
						stringBuilder.Append(metaType.TypeName);
					}
					flag = true;
					if (metaType.SqlDbType == SqlDbType.Decimal)
					{
						byte b = sqlParameter.GetActualPrecision();
						byte actualScale = sqlParameter.GetActualScale();
						stringBuilder.Append('(');
						if (b == 0)
						{
							b = 29;
						}
						stringBuilder.Append(b);
						stringBuilder.Append(',');
						stringBuilder.Append(actualScale);
						stringBuilder.Append(')');
					}
					else if (metaType.IsVarTime)
					{
						byte actualScale2 = sqlParameter.GetActualScale();
						stringBuilder.Append('(');
						stringBuilder.Append(actualScale2);
						stringBuilder.Append(')');
					}
					else if (!metaType.IsFixed && !metaType.IsLong && metaType.SqlDbType != SqlDbType.Timestamp && metaType.SqlDbType != SqlDbType.Udt && SqlDbType.Structured != metaType.SqlDbType)
					{
						int num = sqlParameter.Size;
						stringBuilder.Append('(');
						if (metaType.IsAnsiType)
						{
							object coercedValue = sqlParameter.GetCoercedValue();
							string text = null;
							if (coercedValue != null && DBNull.Value != coercedValue)
							{
								text = coercedValue as string;
								if (text == null)
								{
									SqlString sqlString = ((coercedValue is SqlString) ? ((SqlString)coercedValue) : SqlString.Null);
									if (!sqlString.IsNull)
									{
										text = sqlString.Value;
									}
								}
							}
							if (text != null)
							{
								int encodingCharLength = parser.GetEncodingCharLength(text, sqlParameter.GetActualSize(), sqlParameter.Offset, null);
								if (encodingCharLength > num)
								{
									num = encodingCharLength;
								}
							}
						}
						if (num == 0)
						{
							num = (metaType.IsSizeInCharacters ? 4000 : 8000);
						}
						stringBuilder.Append(num);
						stringBuilder.Append(')');
					}
					else if (metaType.IsPlp && metaType.SqlDbType != SqlDbType.Xml && metaType.SqlDbType != SqlDbType.Udt)
					{
						stringBuilder.Append("(max) ");
					}
					if (sqlParameter.Direction != ParameterDirection.Input)
					{
						stringBuilder.Append(" output");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00064698 File Offset: 0x00062898
		private string ParseAndQuoteIdentifier(string identifier, bool isUdtTypeName)
		{
			string[] array = SqlParameter.ParseTypeName(identifier, isUdtTypeName);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				if (0 < stringBuilder.Length)
				{
					stringBuilder.Append('.');
				}
				if (array[i] != null && array[i].Length != 0)
				{
					stringBuilder.Append(ADP.BuildQuotedString("[", "]", array[i]));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00064704 File Offset: 0x00062904
		private string GetSetOptionsString(CommandBehavior behavior)
		{
			string text = null;
			if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly) || CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
			{
				text = " SET FMTONLY OFF;";
				if (CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
				{
					text += " SET NO_BROWSETABLE ON;";
				}
				if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly))
				{
					text += " SET FMTONLY ON;";
				}
			}
			return text;
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0006474C File Offset: 0x0006294C
		private string GetResetOptionsString(CommandBehavior behavior)
		{
			string text = null;
			if (CommandBehavior.SchemaOnly == (behavior & CommandBehavior.SchemaOnly))
			{
				text += " SET FMTONLY OFF;";
			}
			if (CommandBehavior.KeyInfo == (behavior & CommandBehavior.KeyInfo))
			{
				text += " SET NO_BROWSETABLE OFF;";
			}
			return text;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x00064780 File Offset: 0x00062980
		private string GetCommandText(CommandBehavior behavior)
		{
			return this.GetSetOptionsString(behavior) + this.CommandText;
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00064794 File Offset: 0x00062994
		internal void CheckThrowSNIException()
		{
			TdsParserStateObject stateObj = this._stateObj;
			if (stateObj != null)
			{
				stateObj.CheckThrowSNIException();
			}
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x000647B4 File Offset: 0x000629B4
		internal void OnConnectionClosed()
		{
			TdsParserStateObject stateObj = this._stateObj;
			if (stateObj != null)
			{
				stateObj.OnConnectionClosed();
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060015B4 RID: 5556 RVA: 0x000647D1 File Offset: 0x000629D1
		internal TdsParserStateObject StateObject
		{
			get
			{
				return this._stateObj;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060015B5 RID: 5557 RVA: 0x000647D9 File Offset: 0x000629D9
		private bool IsPrepared
		{
			get
			{
				return this._execType > SqlCommand.EXECTYPE.UNPREPARED;
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x000647E4 File Offset: 0x000629E4
		private bool IsUserPrepared
		{
			get
			{
				return this.IsPrepared && !this._hiddenPrepare && !this.IsDirty;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060015B7 RID: 5559 RVA: 0x00064804 File Offset: 0x00062A04
		// (set) Token: 0x060015B8 RID: 5560 RVA: 0x00064867 File Offset: 0x00062A67
		internal bool IsDirty
		{
			get
			{
				SqlConnection activeConnection = this._activeConnection;
				return this.IsPrepared && (this._dirty || (this._parameters != null && this._parameters.IsDirty) || (activeConnection != null && (activeConnection.CloseCount != this._preparedConnectionCloseCount || activeConnection.ReconnectCount != this._preparedConnectionReconnectCount)));
			}
			set
			{
				this._dirty = value && this.IsPrepared;
				if (this._parameters != null)
				{
					this._parameters.IsDirty = this._dirty;
				}
				this._cachedMetaData = null;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060015B9 RID: 5561 RVA: 0x0006489B File Offset: 0x00062A9B
		// (set) Token: 0x060015BA RID: 5562 RVA: 0x000648A3 File Offset: 0x00062AA3
		internal int InternalRecordsAffected
		{
			get
			{
				return this._rowsAffected;
			}
			set
			{
				if (-1 == this._rowsAffected)
				{
					this._rowsAffected = value;
					return;
				}
				if (0 < value)
				{
					this._rowsAffected += value;
				}
			}
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x000648C8 File Offset: 0x00062AC8
		internal void ClearBatchCommand()
		{
			List<_SqlRPC> rpclist = this._RPCList;
			if (rpclist != null)
			{
				rpclist.Clear();
			}
			if (this._parameterCollectionList != null)
			{
				this._parameterCollectionList.Clear();
			}
			this._SqlRPCBatchArray = null;
			this._currentlyExecutingBatch = 0;
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x00064906 File Offset: 0x00062B06
		// (set) Token: 0x060015BD RID: 5565 RVA: 0x0006490E File Offset: 0x00062B0E
		internal bool BatchRPCMode
		{
			get
			{
				return this._batchRPCMode;
			}
			set
			{
				this._batchRPCMode = value;
				if (!this._batchRPCMode)
				{
					this.ClearBatchCommand();
					return;
				}
				if (this._RPCList == null)
				{
					this._RPCList = new List<_SqlRPC>();
				}
				if (this._parameterCollectionList == null)
				{
					this._parameterCollectionList = new List<SqlParameterCollection>();
				}
			}
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0006494C File Offset: 0x00062B4C
		internal void AddBatchCommand(string commandText, SqlParameterCollection parameters, CommandType cmdType)
		{
			_SqlRPC sqlRPC = new _SqlRPC();
			this.CommandText = commandText;
			this.CommandType = cmdType;
			this.GetStateObject(null);
			if (cmdType == CommandType.StoredProcedure)
			{
				this.BuildRPC(false, parameters, ref sqlRPC);
			}
			else
			{
				this.BuildExecuteSql(CommandBehavior.Default, commandText, parameters, ref sqlRPC);
			}
			this._RPCList.Add(sqlRPC);
			this._parameterCollectionList.Add(parameters);
			this.ReliablePutStateObject();
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x000649AD File Offset: 0x00062BAD
		internal int ExecuteBatchRPCCommand()
		{
			this._SqlRPCBatchArray = this._RPCList.ToArray();
			this._currentlyExecutingBatch = 0;
			return this.ExecuteNonQuery();
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x000649CD File Offset: 0x00062BCD
		internal int? GetRecordsAffected(int commandIndex)
		{
			return this._SqlRPCBatchArray[commandIndex].recordsAffected;
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x000649DC File Offset: 0x00062BDC
		internal SqlException GetErrors(int commandIndex)
		{
			SqlException ex = null;
			int num = this._SqlRPCBatchArray[commandIndex].errorsIndexEnd - this._SqlRPCBatchArray[commandIndex].errorsIndexStart;
			if (0 < num)
			{
				SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
				for (int i = this._SqlRPCBatchArray[commandIndex].errorsIndexStart; i < this._SqlRPCBatchArray[commandIndex].errorsIndexEnd; i++)
				{
					sqlErrorCollection.Add(this._SqlRPCBatchArray[commandIndex].errors[i]);
				}
				for (int j = this._SqlRPCBatchArray[commandIndex].warningsIndexStart; j < this._SqlRPCBatchArray[commandIndex].warningsIndexEnd; j++)
				{
					sqlErrorCollection.Add(this._SqlRPCBatchArray[commandIndex].warnings[j]);
				}
				ex = SqlException.CreateException(sqlErrorCollection, this.Connection.ServerVersion, this.Connection.ClientConnectionId, null);
			}
			return ex;
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x00064AB4 File Offset: 0x00062CB4
		internal new void CancelIgnoreFailure()
		{
			try
			{
				this.Cancel();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x00064ADC File Offset: 0x00062CDC
		private void NotifyDependency()
		{
			if (this._sqlDep != null)
			{
				this._sqlDep.StartTimer(this.Notification);
			}
		}

		/// <summary>Creates a new <see cref="T:System.Data.SqlClient.SqlCommand" /> object that is a copy of the current instance.</summary>
		/// <returns>A new <see cref="T:System.Data.SqlClient.SqlCommand" /> object that is a copy of this instance.</returns>
		// Token: 0x060015C4 RID: 5572 RVA: 0x00064AF7 File Offset: 0x00062CF7
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		/// <summary>Creates a new <see cref="T:System.Data.SqlClient.SqlCommand" /> object that is a copy of the current instance.</summary>
		/// <returns>A new <see cref="T:System.Data.SqlClient.SqlCommand" /> object that is a copy of this instance.</returns>
		// Token: 0x060015C5 RID: 5573 RVA: 0x00064AFF File Offset: 0x00062CFF
		public SqlCommand Clone()
		{
			return new SqlCommand(this);
		}

		/// <summary>Gets or sets a value indicating whether the application should automatically receive query notifications from a common <see cref="T:System.Data.SqlClient.SqlDependency" /> object.</summary>
		/// <returns>
		///   <see langword="true" /> if the application should automatically receive query notifications; otherwise <see langword="false" />. The default value is <see langword="true" />.</returns>
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x060015C6 RID: 5574 RVA: 0x00064B07 File Offset: 0x00062D07
		// (set) Token: 0x060015C7 RID: 5575 RVA: 0x00010C60 File Offset: 0x0000EE60
		[MonoTODO]
		public bool NotificationAutoEnlist
		{
			get
			{
				return this.Notification != null;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this <see cref="T:System.Data.SqlClient.SqlCommand" />, and retrieves one or more result sets from the server.</summary>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that can be used to poll or wait for results, or both; this value is also needed when invoking <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteReader(System.IAsyncResult)" />, which returns a <see cref="T:System.Data.SqlClient.SqlDataReader" /> instance that can be used to retrieve the returned rows.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">Any error that occurred while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The name/value pair "Asynchronous Processing=true" was not included within the connection string defining the connection for this <see cref="T:System.Data.SqlClient.SqlCommand" />.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x060015C8 RID: 5576 RVA: 0x00064B12 File Offset: 0x00062D12
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteReader()
		{
			return this.BeginExecuteReader(CommandBehavior.Default, null, null);
		}

		/// <summary>Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this <see cref="T:System.Data.SqlClient.SqlCommand" /> and retrieves one or more result sets from the server, given a callback procedure and state information.</summary>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that is invoked when the command's execution has completed. Pass <see langword="null" /> (<see langword="Nothing" /> in Microsoft Visual Basic) to indicate that no callback is required.</param>
		/// <param name="stateObject">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the <see cref="P:System.IAsyncResult.AsyncState" /> property.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that can be used to poll, wait for results, or both; this value is also needed when invoking <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteReader(System.IAsyncResult)" />, which returns a <see cref="T:System.Data.SqlClient.SqlDataReader" /> instance which can be used to retrieve the returned rows.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">Any error that occurred while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The name/value pair "Asynchronous Processing=true" was not included within the connection string defining the connection for this <see cref="T:System.Data.SqlClient.SqlCommand" />.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x060015C9 RID: 5577 RVA: 0x00064B1D File Offset: 0x00062D1D
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject)
		{
			return this.BeginExecuteReader(CommandBehavior.Default, callback, stateObject);
		}

		/// <summary>Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this <see cref="T:System.Data.SqlClient.SqlCommand" />, using one of the <see langword="CommandBehavior" /> values, and retrieving one or more result sets from the server, given a callback procedure and state information.</summary>
		/// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that is invoked when the command's execution has completed. Pass <see langword="null" /> (<see langword="Nothing" /> in Microsoft Visual Basic) to indicate that no callback is required.</param>
		/// <param name="stateObject">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the <see cref="P:System.IAsyncResult.AsyncState" /> property.</param>
		/// <param name="behavior">One of the <see cref="T:System.Data.CommandBehavior" /> values, indicating options for statement execution and data retrieval.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that can be used to poll or wait for results, or both; this value is also needed when invoking <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteReader(System.IAsyncResult)" />, which returns a <see cref="T:System.Data.SqlClient.SqlDataReader" /> instance which can be used to retrieve the returned rows.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">Any error that occurred while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The name/value pair "Asynchronous Processing=true" was not included within the connection string defining the connection for this <see cref="T:System.Data.SqlClient.SqlCommand" />.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x060015CA RID: 5578 RVA: 0x00064B28 File Offset: 0x00062D28
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteReader(AsyncCallback callback, object stateObject, CommandBehavior behavior)
		{
			return this.BeginExecuteReader(behavior, callback, stateObject);
		}

		/// <summary>Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this <see cref="T:System.Data.SqlClient.SqlCommand" /> using one of the <see cref="T:System.Data.CommandBehavior" /> values.</summary>
		/// <param name="behavior">One of the <see cref="T:System.Data.CommandBehavior" /> values, indicating options for statement execution and data retrieval.</param>
		/// <returns>An <see cref="T:System.IAsyncResult" /> that can be used to poll, wait for results, or both; this value is also needed when invoking <see cref="M:System.Data.SqlClient.SqlCommand.EndExecuteReader(System.IAsyncResult)" />, which returns a <see cref="T:System.Data.SqlClient.SqlDataReader" /> instance that can be used to retrieve the returned rows.</returns>
		/// <exception cref="T:System.InvalidCastException">A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Binary or VarBinary was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.Stream" />. For more information about streaming, see SqlClient Streaming Support.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Char, NChar, NVarChar, VarChar, or  Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.IO.TextReader" />.  
		///  A <see cref="P:System.Data.SqlClient.SqlParameter.SqlDbType" /> other than Xml was used when <see cref="P:System.Data.SqlClient.SqlParameter.Value" /> was set to <see cref="T:System.Xml.XmlReader" />.</exception>
		/// <exception cref="T:System.Data.SqlClient.SqlException">Any error that occurred while executing the command text.  
		///  A timeout occurred during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.InvalidOperationException">The name/value pair "Asynchronous Processing=true" was not included within the connection string defining the connection for this <see cref="T:System.Data.SqlClient.SqlCommand" />.  
		///  The <see cref="T:System.Data.SqlClient.SqlConnection" /> closed or dropped during a streaming operation. For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.IO.IOException">An error occurred in a <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		/// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.Stream" />, <see cref="T:System.Xml.XmlReader" /> or <see cref="T:System.IO.TextReader" /> object was closed during a streaming operation.  For more information about streaming, see SqlClient Streaming Support.</exception>
		// Token: 0x060015CB RID: 5579 RVA: 0x00064B33 File Offset: 0x00062D33
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public IAsyncResult BeginExecuteReader(CommandBehavior behavior)
		{
			return this.BeginExecuteReader(behavior, null, null);
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x00064B40 File Offset: 0x00062D40
		// Note: this type is marked as 'beforefieldinit'.
		static SqlCommand()
		{
			string[] array = new string[15];
			array[0] = "PARAMETER_NAME";
			array[1] = "PARAMETER_TYPE";
			array[2] = "DATA_TYPE";
			array[4] = "CHARACTER_MAXIMUM_LENGTH";
			array[5] = "NUMERIC_PRECISION";
			array[6] = "NUMERIC_SCALE";
			array[7] = "UDT_CATALOG";
			array[8] = "UDT_SCHEMA";
			array[9] = "TYPE_NAME";
			array[10] = "XML_CATALOGNAME";
			array[11] = "XML_SCHEMANAME";
			array[12] = "XML_SCHEMACOLLECTIONNAME";
			array[13] = "UDT_NAME";
			SqlCommand.PreKatmaiProcParamsNames = array;
			SqlCommand.KatmaiProcParamsNames = new string[]
			{
				"PARAMETER_NAME", "PARAMETER_TYPE", null, "MANAGED_DATA_TYPE", "CHARACTER_MAXIMUM_LENGTH", "NUMERIC_PRECISION", "NUMERIC_SCALE", "TYPE_CATALOG_NAME", "TYPE_SCHEMA_NAME", "TYPE_NAME",
				"XML_CATALOGNAME", "XML_SCHEMANAME", "XML_SCHEMACOLLECTIONNAME", null, "SS_DATETIME_PRECISION"
			};
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Data.SqlClient.SqlCommand" /> class with specified command text, connection, transaction, and encryption setting.</summary>
		/// <param name="cmdText">The text of the query.</param>
		/// <param name="connection">A <see cref="T:System.Data.SqlClient.SqlConnection" /> that represents the connection to an instance of SQL Server.</param>
		/// <param name="transaction">The <see cref="T:System.Data.SqlClient.SqlTransaction" /> in which the <see cref="T:System.Data.SqlClient.SqlCommand" /> executes.</param>
		/// <param name="columnEncryptionSetting">The encryption setting. For more information, see Always Encrypted.</param>
		// Token: 0x060015CD RID: 5581 RVA: 0x000108A6 File Offset: 0x0000EAA6
		public SqlCommand(string cmdText, SqlConnection connection, SqlTransaction transaction, SqlCommandColumnEncryptionSetting columnEncryptionSetting)
		{
			global::Unity.ThrowStub.ThrowNotSupportedException();
		}

		/// <summary>Gets or sets the column encryption setting for this command.</summary>
		/// <returns>The column encryption setting for this command.</returns>
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x060015CE RID: 5582 RVA: 0x00064C50 File Offset: 0x00062E50
		public SqlCommandColumnEncryptionSetting ColumnEncryptionSetting
		{
			get
			{
				global::Unity.ThrowStub.ThrowNotSupportedException();
				return SqlCommandColumnEncryptionSetting.UseConnectionSetting;
			}
		}

		// Token: 0x04000D90 RID: 3472
		private string _commandText;

		// Token: 0x04000D91 RID: 3473
		private CommandType _commandType;

		// Token: 0x04000D92 RID: 3474
		private int _commandTimeout;

		// Token: 0x04000D93 RID: 3475
		private UpdateRowSource _updatedRowSource;

		// Token: 0x04000D94 RID: 3476
		private bool _designTimeInvisible;

		// Token: 0x04000D95 RID: 3477
		internal SqlDependency _sqlDep;

		// Token: 0x04000D96 RID: 3478
		private static readonly DiagnosticListener _diagnosticListener = new DiagnosticListener("SqlClientDiagnosticListener");

		// Token: 0x04000D97 RID: 3479
		private bool _parentOperationStarted;

		// Token: 0x04000D98 RID: 3480
		private bool _inPrepare;

		// Token: 0x04000D99 RID: 3481
		private int _prepareHandle;

		// Token: 0x04000D9A RID: 3482
		private bool _hiddenPrepare;

		// Token: 0x04000D9B RID: 3483
		private int _preparedConnectionCloseCount;

		// Token: 0x04000D9C RID: 3484
		private int _preparedConnectionReconnectCount;

		// Token: 0x04000D9D RID: 3485
		private SqlParameterCollection _parameters;

		// Token: 0x04000D9E RID: 3486
		private SqlConnection _activeConnection;

		// Token: 0x04000D9F RID: 3487
		private bool _dirty;

		// Token: 0x04000DA0 RID: 3488
		private SqlCommand.EXECTYPE _execType;

		// Token: 0x04000DA1 RID: 3489
		private _SqlRPC[] _rpcArrayOf1;

		// Token: 0x04000DA2 RID: 3490
		private _SqlMetaDataSet _cachedMetaData;

		// Token: 0x04000DA3 RID: 3491
		private TaskCompletionSource<object> _reconnectionCompletionSource;

		// Token: 0x04000DA4 RID: 3492
		private SqlCommand.CachedAsyncState _cachedAsyncState;

		// Token: 0x04000DA5 RID: 3493
		internal int _rowsAffected;

		// Token: 0x04000DA6 RID: 3494
		private SqlNotificationRequest _notification;

		// Token: 0x04000DA7 RID: 3495
		private SqlTransaction _transaction;

		// Token: 0x04000DA8 RID: 3496
		private StatementCompletedEventHandler _statementCompletedEventHandler;

		// Token: 0x04000DA9 RID: 3497
		private TdsParserStateObject _stateObj;

		// Token: 0x04000DAA RID: 3498
		private volatile bool _pendingCancel;

		// Token: 0x04000DAB RID: 3499
		private bool _batchRPCMode;

		// Token: 0x04000DAC RID: 3500
		private List<_SqlRPC> _RPCList;

		// Token: 0x04000DAD RID: 3501
		private _SqlRPC[] _SqlRPCBatchArray;

		// Token: 0x04000DAE RID: 3502
		private List<SqlParameterCollection> _parameterCollectionList;

		// Token: 0x04000DAF RID: 3503
		private int _currentlyExecutingBatch;

		// Token: 0x04000DB0 RID: 3504
		internal static readonly string[] PreKatmaiProcParamsNames;

		// Token: 0x04000DB1 RID: 3505
		internal static readonly string[] KatmaiProcParamsNames;

		// Token: 0x020001B2 RID: 434
		private enum EXECTYPE
		{
			// Token: 0x04000DB3 RID: 3507
			UNPREPARED,
			// Token: 0x04000DB4 RID: 3508
			PREPAREPENDING,
			// Token: 0x04000DB5 RID: 3509
			PREPARED
		}

		// Token: 0x020001B3 RID: 435
		private class CachedAsyncState
		{
			// Token: 0x060015CF RID: 5583 RVA: 0x00064C6B File Offset: 0x00062E6B
			internal CachedAsyncState()
			{
			}

			// Token: 0x170003B5 RID: 949
			// (get) Token: 0x060015D0 RID: 5584 RVA: 0x00064C81 File Offset: 0x00062E81
			internal SqlDataReader CachedAsyncReader
			{
				get
				{
					return this._cachedAsyncReader;
				}
			}

			// Token: 0x170003B6 RID: 950
			// (get) Token: 0x060015D1 RID: 5585 RVA: 0x00064C89 File Offset: 0x00062E89
			internal RunBehavior CachedRunBehavior
			{
				get
				{
					return this._cachedRunBehavior;
				}
			}

			// Token: 0x170003B7 RID: 951
			// (get) Token: 0x060015D2 RID: 5586 RVA: 0x00064C91 File Offset: 0x00062E91
			internal string CachedSetOptions
			{
				get
				{
					return this._cachedSetOptions;
				}
			}

			// Token: 0x170003B8 RID: 952
			// (get) Token: 0x060015D3 RID: 5587 RVA: 0x00064C99 File Offset: 0x00062E99
			internal bool PendingAsyncOperation
			{
				get
				{
					return this._cachedAsyncResult != null;
				}
			}

			// Token: 0x170003B9 RID: 953
			// (get) Token: 0x060015D4 RID: 5588 RVA: 0x00064CA4 File Offset: 0x00062EA4
			internal string EndMethodName
			{
				get
				{
					return this._cachedEndMethod;
				}
			}

			// Token: 0x060015D5 RID: 5589 RVA: 0x00064CAC File Offset: 0x00062EAC
			internal bool IsActiveConnectionValid(SqlConnection activeConnection)
			{
				return this._cachedAsyncConnection == activeConnection && this._cachedAsyncCloseCount == activeConnection.CloseCount;
			}

			// Token: 0x060015D6 RID: 5590 RVA: 0x00064CC8 File Offset: 0x00062EC8
			internal void ResetAsyncState()
			{
				this._cachedAsyncCloseCount = -1;
				this._cachedAsyncResult = null;
				if (this._cachedAsyncConnection != null)
				{
					this._cachedAsyncConnection.AsyncCommandInProgress = false;
					this._cachedAsyncConnection = null;
				}
				this._cachedAsyncReader = null;
				this._cachedRunBehavior = RunBehavior.ReturnImmediately;
				this._cachedSetOptions = null;
				this._cachedEndMethod = null;
			}

			// Token: 0x060015D7 RID: 5591 RVA: 0x00064D1C File Offset: 0x00062F1C
			internal void SetActiveConnectionAndResult(TaskCompletionSource<object> completion, string endMethod, SqlConnection activeConnection)
			{
				TdsParser tdsParser = ((activeConnection != null) ? activeConnection.Parser : null);
				if (tdsParser == null || tdsParser.State == TdsParserState.Closed || tdsParser.State == TdsParserState.Broken)
				{
					throw ADP.ClosedConnectionError();
				}
				this._cachedAsyncCloseCount = activeConnection.CloseCount;
				this._cachedAsyncResult = completion;
				if (!tdsParser.MARSOn && activeConnection.AsyncCommandInProgress)
				{
					throw SQL.MARSUnspportedOnConnection();
				}
				this._cachedAsyncConnection = activeConnection;
				this._cachedAsyncConnection.AsyncCommandInProgress = true;
				this._cachedEndMethod = endMethod;
			}

			// Token: 0x060015D8 RID: 5592 RVA: 0x00064D93 File Offset: 0x00062F93
			internal void SetAsyncReaderState(SqlDataReader ds, RunBehavior runBehavior, string optionSettings)
			{
				this._cachedAsyncReader = ds;
				this._cachedRunBehavior = runBehavior;
				this._cachedSetOptions = optionSettings;
			}

			// Token: 0x04000DB6 RID: 3510
			private int _cachedAsyncCloseCount = -1;

			// Token: 0x04000DB7 RID: 3511
			private TaskCompletionSource<object> _cachedAsyncResult;

			// Token: 0x04000DB8 RID: 3512
			private SqlConnection _cachedAsyncConnection;

			// Token: 0x04000DB9 RID: 3513
			private SqlDataReader _cachedAsyncReader;

			// Token: 0x04000DBA RID: 3514
			private RunBehavior _cachedRunBehavior = RunBehavior.ReturnImmediately;

			// Token: 0x04000DBB RID: 3515
			private string _cachedSetOptions;

			// Token: 0x04000DBC RID: 3516
			private string _cachedEndMethod;
		}

		// Token: 0x020001B4 RID: 436
		private enum ProcParamsColIndex
		{
			// Token: 0x04000DBE RID: 3518
			ParameterName,
			// Token: 0x04000DBF RID: 3519
			ParameterType,
			// Token: 0x04000DC0 RID: 3520
			DataType,
			// Token: 0x04000DC1 RID: 3521
			ManagedDataType,
			// Token: 0x04000DC2 RID: 3522
			CharacterMaximumLength,
			// Token: 0x04000DC3 RID: 3523
			NumericPrecision,
			// Token: 0x04000DC4 RID: 3524
			NumericScale,
			// Token: 0x04000DC5 RID: 3525
			TypeCatalogName,
			// Token: 0x04000DC6 RID: 3526
			TypeSchemaName,
			// Token: 0x04000DC7 RID: 3527
			TypeName,
			// Token: 0x04000DC8 RID: 3528
			XmlSchemaCollectionCatalogName,
			// Token: 0x04000DC9 RID: 3529
			XmlSchemaCollectionSchemaName,
			// Token: 0x04000DCA RID: 3530
			XmlSchemaCollectionName,
			// Token: 0x04000DCB RID: 3531
			UdtTypeName,
			// Token: 0x04000DCC RID: 3532
			DateTimeScale
		}
	}
}
