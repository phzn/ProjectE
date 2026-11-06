using System;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace System.Reflection
{
	/// <summary>The exception that is thrown by the <see cref="M:System.Reflection.Module.GetTypes" /> method if any of the classes in a module cannot be loaded. This class cannot be inherited.</summary>
	// Token: 0x020008BE RID: 2238
	[Serializable]
	public sealed class ReflectionTypeLoadException : SystemException, ISerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.ReflectionTypeLoadException" /> class with the given classes and their associated exceptions.</summary>
		/// <param name="classes">An array of type <see langword="Type" /> containing the classes that were defined in the module and loaded. This array can contain null reference (<see langword="Nothing" /> in Visual Basic) values.</param>
		/// <param name="exceptions">An array of type <see langword="Exception" /> containing the exceptions that were thrown by the class loader. The null reference (<see langword="Nothing" /> in Visual Basic) values in the <paramref name="classes" /> array line up with the exceptions in this <paramref name="exceptions" /> array.</param>
		// Token: 0x06004A0C RID: 18956 RVA: 0x000EF555 File Offset: 0x000ED755
		public ReflectionTypeLoadException(Type[] classes, Exception[] exceptions)
			: base(null)
		{
			this.Types = classes;
			this.LoaderExceptions = exceptions;
			base.HResult = -2146232830;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Reflection.ReflectionTypeLoadException" /> class with the given classes, their associated exceptions, and exception descriptions.</summary>
		/// <param name="classes">An array of type <see langword="Type" /> containing the classes that were defined in the module and loaded. This array can contain null reference (<see langword="Nothing" /> in Visual Basic) values.</param>
		/// <param name="exceptions">An array of type <see langword="Exception" /> containing the exceptions that were thrown by the class loader. The null reference (<see langword="Nothing" /> in Visual Basic) values in the <paramref name="classes" /> array line up with the exceptions in this <paramref name="exceptions" /> array.</param>
		/// <param name="message">A <see langword="String" /> describing the reason the exception was thrown.</param>
		// Token: 0x06004A0D RID: 18957 RVA: 0x000EF577 File Offset: 0x000ED777
		public ReflectionTypeLoadException(Type[] classes, Exception[] exceptions, string message)
			: base(message)
		{
			this.Types = classes;
			this.LoaderExceptions = exceptions;
			base.HResult = -2146232830;
		}

		// Token: 0x06004A0E RID: 18958 RVA: 0x000EF599 File Offset: 0x000ED799
		private ReflectionTypeLoadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.LoaderExceptions = (Exception[])info.GetValue("Exceptions", typeof(Exception[]));
		}

		/// <summary>Provides an <see cref="T:System.Runtime.Serialization.ISerializable" /> implementation for serialized objects.</summary>
		/// <param name="info">The information and data needed to serialize or deserialize an object.</param>
		/// <param name="context">The context for the serialization.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <see langword="info" /> is <see langword="null" />.</exception>
		// Token: 0x06004A0F RID: 18959 RVA: 0x000EF5C3 File Offset: 0x000ED7C3
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Types", null, typeof(Type[]));
			info.AddValue("Exceptions", this.LoaderExceptions, typeof(Exception[]));
		}

		/// <summary>Gets the array of classes that were defined in the module and loaded.</summary>
		/// <returns>An array of type <see langword="Type" /> containing the classes that were defined in the module and loaded. This array can contain some <see langword="null" /> values.</returns>
		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x06004A10 RID: 18960 RVA: 0x000EF5FE File Offset: 0x000ED7FE
		public Type[] Types { get; }

		/// <summary>Gets the array of exceptions thrown by the class loader.</summary>
		/// <returns>An array of type <see langword="Exception" /> containing the exceptions thrown by the class loader. The null values in the <see cref="P:System.Reflection.ReflectionTypeLoadException.Types" /> array of this instance line up with the exceptions in this array.</returns>
		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x06004A11 RID: 18961 RVA: 0x000EF606 File Offset: 0x000ED806
		public Exception[] LoaderExceptions { get; }

		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06004A12 RID: 18962 RVA: 0x000EF60E File Offset: 0x000ED80E
		public override string Message
		{
			get
			{
				return this.CreateString(true);
			}
		}

		// Token: 0x06004A13 RID: 18963 RVA: 0x000EF617 File Offset: 0x000ED817
		public override string ToString()
		{
			return this.CreateString(false);
		}

		// Token: 0x06004A14 RID: 18964 RVA: 0x000EF620 File Offset: 0x000ED820
		private string CreateString(bool isMessage)
		{
			string text = (isMessage ? base.Message : base.ToString());
			Exception[] loaderExceptions = this.LoaderExceptions;
			if (loaderExceptions == null || loaderExceptions.Length == 0)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text);
			foreach (Exception ex in loaderExceptions)
			{
				if (ex != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(isMessage ? ex.Message : ex.ToString());
				}
			}
			return stringBuilder.ToString();
		}
	}
}
