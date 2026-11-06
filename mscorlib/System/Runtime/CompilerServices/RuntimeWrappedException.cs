using System;
using System.Runtime.Serialization;
using System.Security;
using Unity;

namespace System.Runtime.CompilerServices
{
	/// <summary>Wraps an exception that does not derive from the <see cref="T:System.Exception" /> class. This class cannot be inherited.</summary>
	// Token: 0x02000803 RID: 2051
	[Serializable]
	public sealed class RuntimeWrappedException : Exception
	{
		// Token: 0x0600461A RID: 17946 RVA: 0x000E58BA File Offset: 0x000E3ABA
		public RuntimeWrappedException(object thrownObject)
			: base("An object that does not derive from System.Exception has been wrapped in a RuntimeWrappedException.")
		{
			base.HResult = -2146233026;
			this._wrappedException = thrownObject;
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x000E58D9 File Offset: 0x000E3AD9
		private RuntimeWrappedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this._wrappedException = info.GetValue("WrappedException", typeof(object));
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with information about the exception.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />.</exception>
		// Token: 0x0600461C RID: 17948 RVA: 0x000E58FE File Offset: 0x000E3AFE
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("WrappedException", this._wrappedException, typeof(object));
		}

		/// <summary>Gets the object that was wrapped by the <see cref="T:System.Runtime.CompilerServices.RuntimeWrappedException" /> object.</summary>
		/// <returns>The object that was wrapped by the <see cref="T:System.Runtime.CompilerServices.RuntimeWrappedException" /> object.</returns>
		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x0600461D RID: 17949 RVA: 0x000E5923 File Offset: 0x000E3B23
		public object WrappedException
		{
			get
			{
				return this._wrappedException;
			}
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x000173AD File Offset: 0x000155AD
		internal RuntimeWrappedException()
		{
			ThrowStub.ThrowNotSupportedException();
		}

		// Token: 0x04002D40 RID: 11584
		private object _wrappedException;
	}
}
