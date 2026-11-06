using System;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when an attempt is made to access an element of an array or collection with an index that is outside its bounds.</summary>
	// Token: 0x02000146 RID: 326
	[Serializable]
	public sealed class IndexOutOfRangeException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.IndexOutOfRangeException" /> class.</summary>
		// Token: 0x06000C16 RID: 3094 RVA: 0x00031F11 File Offset: 0x00030111
		public IndexOutOfRangeException()
			: base("Index was outside the bounds of the array.")
		{
			base.HResult = -2146233080;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IndexOutOfRangeException" /> class with a specified error message.</summary>
		/// <param name="message">The message that describes the error.</param>
		// Token: 0x06000C17 RID: 3095 RVA: 0x00031F29 File Offset: 0x00030129
		public IndexOutOfRangeException(string message)
			: base(message)
		{
			base.HResult = -2146233080;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IndexOutOfRangeException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not a null reference (<see langword="Nothing" /> in Visual Basic), the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x06000C18 RID: 3096 RVA: 0x00031F3D File Offset: 0x0003013D
		public IndexOutOfRangeException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146233080;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x00020A69 File Offset: 0x0001EC69
		internal IndexOutOfRangeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
