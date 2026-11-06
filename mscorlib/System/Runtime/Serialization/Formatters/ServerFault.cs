using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;

namespace System.Runtime.Serialization.Formatters
{
	/// <summary>Contains information for a server fault. This class cannot be inherited.</summary>
	// Token: 0x02000681 RID: 1665
	[SoapType(Embedded = true)]
	[ComVisible(true)]
	[Serializable]
	public sealed class ServerFault
	{
		// Token: 0x06003DEE RID: 15854 RVA: 0x000D5C69 File Offset: 0x000D3E69
		internal ServerFault(Exception exception)
		{
			this.exception = exception;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.Formatters.ServerFault" /> class.</summary>
		/// <param name="exceptionType">The type of the exception that occurred on the server.</param>
		/// <param name="message">The message that accompanied the exception.</param>
		/// <param name="stackTrace">The stack trace of the thread that threw the exception on the server.</param>
		// Token: 0x06003DEF RID: 15855 RVA: 0x000D5C78 File Offset: 0x000D3E78
		public ServerFault(string exceptionType, string message, string stackTrace)
		{
			this.exceptionType = exceptionType;
			this.message = message;
			this.stackTrace = stackTrace;
		}

		/// <summary>Gets or sets the type of exception that was thrown by the server.</summary>
		/// <returns>The type of exception that was thrown by the server.</returns>
		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x06003DF0 RID: 15856 RVA: 0x000D5C95 File Offset: 0x000D3E95
		// (set) Token: 0x06003DF1 RID: 15857 RVA: 0x000D5C9D File Offset: 0x000D3E9D
		public string ExceptionType
		{
			get
			{
				return this.exceptionType;
			}
			set
			{
				this.exceptionType = value;
			}
		}

		/// <summary>Gets or sets the exception message that accompanied the exception thrown on the server.</summary>
		/// <returns>The exception message that accompanied the exception thrown on the server.</returns>
		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x06003DF2 RID: 15858 RVA: 0x000D5CA6 File Offset: 0x000D3EA6
		// (set) Token: 0x06003DF3 RID: 15859 RVA: 0x000D5CAE File Offset: 0x000D3EAE
		public string ExceptionMessage
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		/// <summary>Gets or sets the stack trace of the thread that threw the exception on the server.</summary>
		/// <returns>The stack trace of the thread that threw the exception on the server.</returns>
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x06003DF4 RID: 15860 RVA: 0x000D5CB7 File Offset: 0x000D3EB7
		// (set) Token: 0x06003DF5 RID: 15861 RVA: 0x000D5CBF File Offset: 0x000D3EBF
		public string StackTrace
		{
			get
			{
				return this.stackTrace;
			}
			set
			{
				this.stackTrace = value;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x06003DF6 RID: 15862 RVA: 0x000D5CC8 File Offset: 0x000D3EC8
		internal Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		// Token: 0x040027B3 RID: 10163
		private string exceptionType;

		// Token: 0x040027B4 RID: 10164
		private string message;

		// Token: 0x040027B5 RID: 10165
		private string stackTrace;

		// Token: 0x040027B6 RID: 10166
		private Exception exception;
	}
}
