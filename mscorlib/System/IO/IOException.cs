using System;
using System.Runtime.Serialization;

namespace System.IO
{
	/// <summary>The exception that is thrown when an I/O error occurs.</summary>
	// Token: 0x02000B0B RID: 2827
	[Serializable]
	public class IOException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.IO.IOException" /> class with its message string set to the empty string (""), its HRESULT set to COR_E_IO, and its inner exception set to a null reference.</summary>
		// Token: 0x060064DE RID: 25822 RVA: 0x00156BD6 File Offset: 0x00154DD6
		public IOException()
			: base("I/O error occurred.")
		{
			base.HResult = -2146232800;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.IOException" /> class with its message string set to <paramref name="message" />, its HRESULT set to COR_E_IO, and its inner exception set to <see langword="null" />.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
		// Token: 0x060064DF RID: 25823 RVA: 0x00156BEE File Offset: 0x00154DEE
		public IOException(string message)
			: base(message)
		{
			base.HResult = -2146232800;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.IOException" /> class with its message string set to <paramref name="message" /> and its HRESULT user-defined.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
		/// <param name="hresult">An integer identifying the error that has occurred.</param>
		// Token: 0x060064E0 RID: 25824 RVA: 0x00032814 File Offset: 0x00030A14
		public IOException(string message, int hresult)
			: base(message)
		{
			base.HResult = hresult;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.IOException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x060064E1 RID: 25825 RVA: 0x00156C02 File Offset: 0x00154E02
		public IOException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.HResult = -2146232800;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.IOException" /> class with the specified serialization and context information.</summary>
		/// <param name="info">The data for serializing or deserializing the object.</param>
		/// <param name="context">The source and destination for the object.</param>
		// Token: 0x060064E2 RID: 25826 RVA: 0x00020A69 File Offset: 0x0001EC69
		protected IOException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
