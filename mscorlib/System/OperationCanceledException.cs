using System;
using System.Runtime.Serialization;
using System.Threading;

namespace System
{
	/// <summary>The exception that is thrown in a thread upon cancellation of an operation that the thread was executing.</summary>
	// Token: 0x0200016A RID: 362
	[Serializable]
	public class OperationCanceledException : SystemException
	{
		/// <summary>Gets a token associated with the operation that was canceled.</summary>
		/// <returns>A token associated with the operation that was canceled, or a default token.</returns>
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x0003AD8F File Offset: 0x00038F8F
		// (set) Token: 0x06000E66 RID: 3686 RVA: 0x0003AD97 File Offset: 0x00038F97
		public CancellationToken CancellationToken
		{
			get
			{
				return this._cancellationToken;
			}
			private set
			{
				this._cancellationToken = value;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.OperationCanceledException" /> class with a system-supplied error message.</summary>
		// Token: 0x06000E67 RID: 3687 RVA: 0x0003ADA0 File Offset: 0x00038FA0
		public OperationCanceledException()
			: base("The operation was canceled.")
		{
			base.HResult = -2146233029;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.OperationCanceledException" /> class with a specified error message.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error.</param>
		// Token: 0x06000E68 RID: 3688 RVA: 0x0003ADB8 File Offset: 0x00038FB8
		public OperationCanceledException(string message)
			: base(message)
		{
			base.HResult = -2146233029;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.OperationCanceledException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x06000E69 RID: 3689 RVA: 0x0003ADCC File Offset: 0x00038FCC
		public OperationCanceledException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146233029;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.OperationCanceledException" /> class with a cancellation token.</summary>
		/// <param name="token">A cancellation token associated with the operation that was canceled.</param>
		// Token: 0x06000E6A RID: 3690 RVA: 0x0003ADE1 File Offset: 0x00038FE1
		public OperationCanceledException(CancellationToken token)
			: this()
		{
			this.CancellationToken = token;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.OperationCanceledException" /> class with a specified error message and a cancellation token.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="token">A cancellation token associated with the operation that was canceled.</param>
		// Token: 0x06000E6B RID: 3691 RVA: 0x0003ADF0 File Offset: 0x00038FF0
		public OperationCanceledException(string message, CancellationToken token)
			: this(message)
		{
			this.CancellationToken = token;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.OperationCanceledException" /> class with a specified error message, a reference to the inner exception that is the cause of this exception, and a cancellation token.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		/// <param name="token">A cancellation token associated with the operation that was canceled.</param>
		// Token: 0x06000E6C RID: 3692 RVA: 0x0003AE00 File Offset: 0x00039000
		public OperationCanceledException(string message, Exception innerException, CancellationToken token)
			: this(message, innerException)
		{
			this.CancellationToken = token;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.OperationCanceledException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		// Token: 0x06000E6D RID: 3693 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected OperationCanceledException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x040012A7 RID: 4775
		[NonSerialized]
		private CancellationToken _cancellationToken;
	}
}
