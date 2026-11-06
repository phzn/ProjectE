using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security
{
	/// <summary>The exception that is thrown when a denied host resource is detected.</summary>
	// Token: 0x020003DE RID: 990
	[ComVisible(true)]
	[MonoTODO("Not supported in the runtime")]
	[Serializable]
	public class HostProtectionException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.HostProtectionException" /> class with default values.</summary>
		// Token: 0x060028A2 RID: 10402 RVA: 0x00092A55 File Offset: 0x00090C55
		public HostProtectionException()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.HostProtectionException" /> class with a specified error message.</summary>
		/// <param name="message">The message that describes the error.</param>
		// Token: 0x060028A3 RID: 10403 RVA: 0x0006E6B1 File Offset: 0x0006C8B1
		public HostProtectionException(string message)
			: base(message)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.HostProtectionException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="e">The exception that is the cause of the current exception. If the innerException parameter is not <see langword="null" />, the current exception is raised in a <see langword="catch" /> block that handles the inner exception.</param>
		// Token: 0x060028A4 RID: 10404 RVA: 0x0006E6BA File Offset: 0x0006C8BA
		public HostProtectionException(string message, Exception e)
			: base(message, e)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.HostProtectionException" /> class with a specified error message, the protected host resources, and the host resources that caused the exception to be thrown.</summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="protectedResources">A bitwise combination of the enumeration values that specify the host resources that are inaccessible to partially trusted code.</param>
		/// <param name="demandedResources">A bitwise combination of the enumeration values that specify the demanded host resources.</param>
		// Token: 0x060028A5 RID: 10405 RVA: 0x000932D0 File Offset: 0x000914D0
		public HostProtectionException(string message, HostProtectionResource protectedResources, HostProtectionResource demandedResources)
			: base(message)
		{
			this._protected = protectedResources;
			this._demanded = demandedResources;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.HostProtectionException" /> class using the provided serialization information and streaming context.</summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">Contextual information about the source or destination.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is <see langword="null" />.</exception>
		// Token: 0x060028A6 RID: 10406 RVA: 0x000932E7 File Offset: 0x000914E7
		protected HostProtectionException(SerializationInfo info, StreamingContext context)
		{
			this.GetObjectData(info, context);
		}

		/// <summary>Gets or sets the demanded host protection resources that caused the exception to be thrown.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Security.Permissions.HostProtectionResource" /> values identifying the protection resources causing the exception to be thrown. The default is <see cref="F:System.Security.Permissions.HostProtectionResource.None" />.</returns>
		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060028A7 RID: 10407 RVA: 0x000932F7 File Offset: 0x000914F7
		public HostProtectionResource DemandedResources
		{
			get
			{
				return this._demanded;
			}
		}

		/// <summary>Gets or sets the host protection resources that are inaccessible to partially trusted code.</summary>
		/// <returns>A bitwise combination of the <see cref="T:System.Security.Permissions.HostProtectionResource" /> values identifying the inaccessible host protection categories. The default is <see cref="F:System.Security.Permissions.HostProtectionResource.None" />.</returns>
		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x000932FF File Offset: 0x000914FF
		public HostProtectionResource ProtectedResources
		{
			get
			{
				return this._protected;
			}
		}

		/// <summary>Sets the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with information about the host protection exception.</summary>
		/// <param name="info">The serialized object data about the exception being thrown.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is <see langword="null" />.</exception>
		// Token: 0x060028A9 RID: 10409 RVA: 0x0005D90C File Offset: 0x0005BB0C
		[MonoTODO]
		[SecurityCritical]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
		}

		/// <summary>Returns a string representation of the current host protection exception.</summary>
		/// <returns>A string representation of the current <see cref="T:System.Security.HostProtectionException" />.</returns>
		// Token: 0x060028AA RID: 10410 RVA: 0x00093307 File Offset: 0x00091507
		[MonoTODO]
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x04001EB5 RID: 7861
		private HostProtectionResource _protected;

		// Token: 0x04001EB6 RID: 7862
		private HostProtectionResource _demanded;
	}
}
