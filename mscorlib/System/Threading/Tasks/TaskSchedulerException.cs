using System;
using System.Runtime.Serialization;

namespace System.Threading.Tasks
{
	/// <summary>Represents an exception used to communicate an invalid operation by a <see cref="T:System.Threading.Tasks.TaskScheduler" />.</summary>
	// Token: 0x0200030B RID: 779
	[Serializable]
	public class TaskSchedulerException : Exception
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskSchedulerException" /> class with a system-supplied message that describes the error.</summary>
		// Token: 0x0600217D RID: 8573 RVA: 0x000786D9 File Offset: 0x000768D9
		public TaskSchedulerException()
			: base("An exception was thrown by a TaskScheduler.")
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskSchedulerException" /> class with a specified message that describes the error.</summary>
		/// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
		// Token: 0x0600217E RID: 8574 RVA: 0x000328A6 File Offset: 0x00030AA6
		public TaskSchedulerException(string message)
			: base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskSchedulerException" /> class using the default error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="innerException">The exception that is the cause of the current exception.</param>
		// Token: 0x0600217F RID: 8575 RVA: 0x000786E6 File Offset: 0x000768E6
		public TaskSchedulerException(Exception innerException)
			: base("An exception was thrown by a TaskScheduler.", innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskSchedulerException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x06002180 RID: 8576 RVA: 0x000328AF File Offset: 0x00030AAF
		public TaskSchedulerException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.Tasks.TaskSchedulerException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		// Token: 0x06002181 RID: 8577 RVA: 0x00020FAB File Offset: 0x0001F1AB
		protected TaskSchedulerException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
