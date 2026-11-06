using System;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when a feature does not run on a particular platform.</summary>
	// Token: 0x0200016F RID: 367
	[Serializable]
	public class PlatformNotSupportedException : NotSupportedException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.PlatformNotSupportedException" /> class with default properties.</summary>
		// Token: 0x06000E89 RID: 3721 RVA: 0x0003B9DA File Offset: 0x00039BDA
		public PlatformNotSupportedException()
			: base("Operation is not supported on this platform.")
		{
			base.HResult = -2146233031;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.PlatformNotSupportedException" /> class with a specified error message.</summary>
		/// <param name="message">The text message that explains the reason for the exception.</param>
		// Token: 0x06000E8A RID: 3722 RVA: 0x0003B9F2 File Offset: 0x00039BF2
		public PlatformNotSupportedException(string message)
			: base(message)
		{
			base.HResult = -2146233031;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.PlatformNotSupportedException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x06000E8B RID: 3723 RVA: 0x0003BA06 File Offset: 0x00039C06
		public PlatformNotSupportedException(string message, Exception inner)
			: base(message, inner)
		{
			base.HResult = -2146233031;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.PlatformNotSupportedException" /> class with serialized data.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		// Token: 0x06000E8C RID: 3724 RVA: 0x0003BA1B File Offset: 0x00039C1B
		protected PlatformNotSupportedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
