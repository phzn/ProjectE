using System;
using System.Runtime.Serialization;

namespace System.IO
{
	/// <summary>The exception that is thrown when part of a file or directory cannot be found.</summary>
	// Token: 0x02000B02 RID: 2818
	[Serializable]
	public class DirectoryNotFoundException : IOException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.IO.DirectoryNotFoundException" /> class with its message string set to a system-supplied message and its HRESULT set to COR_E_DIRECTORYNOTFOUND.</summary>
		// Token: 0x060064B8 RID: 25784 RVA: 0x001566DF File Offset: 0x001548DF
		public DirectoryNotFoundException()
			: base("Attempted to access a path that is not on the disk.")
		{
			base.HResult = -2147024893;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.DirectoryNotFoundException" /> class with its message string set to <paramref name="message" /> and its HRESULT set to COR_E_DIRECTORYNOTFOUND.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
		// Token: 0x060064B9 RID: 25785 RVA: 0x001566F7 File Offset: 0x001548F7
		public DirectoryNotFoundException(string message)
			: base(message)
		{
			base.HResult = -2147024893;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.DirectoryNotFoundException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x060064BA RID: 25786 RVA: 0x0015670B File Offset: 0x0015490B
		public DirectoryNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2147024893;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.DirectoryNotFoundException" /> class with the specified serialization and context information.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		// Token: 0x060064BB RID: 25787 RVA: 0x00156720 File Offset: 0x00154920
		protected DirectoryNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
