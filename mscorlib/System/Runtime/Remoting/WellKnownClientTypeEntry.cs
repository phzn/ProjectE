using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting
{
	/// <summary>Holds values for an object type registered on the client as a server-activated type (single call or singleton).</summary>
	// Token: 0x02000578 RID: 1400
	[ComVisible(true)]
	public class WellKnownClientTypeEntry : TypeEntry
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.WellKnownClientTypeEntry" /> class with the given type and URL.</summary>
		/// <param name="type">The <see cref="T:System.Type" /> of the server-activated type.</param>
		/// <param name="objectUrl">The URL of the server-activated type.</param>
		// Token: 0x060036F8 RID: 14072 RVA: 0x000C6891 File Offset: 0x000C4A91
		public WellKnownClientTypeEntry(Type type, string objectUrl)
		{
			base.AssemblyName = type.Assembly.FullName;
			base.TypeName = type.FullName;
			this.obj_type = type;
			this.obj_url = objectUrl;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.WellKnownClientTypeEntry" /> class with the given type, assembly name, and URL.</summary>
		/// <param name="typeName">The type name of the server-activated type.</param>
		/// <param name="assemblyName">The assembly name of the server-activated type.</param>
		/// <param name="objectUrl">The URL of the server-activated type.</param>
		// Token: 0x060036F9 RID: 14073 RVA: 0x000C68C4 File Offset: 0x000C4AC4
		public WellKnownClientTypeEntry(string typeName, string assemblyName, string objectUrl)
		{
			this.obj_url = objectUrl;
			base.AssemblyName = assemblyName;
			base.TypeName = typeName;
			Assembly assembly = Assembly.Load(assemblyName);
			this.obj_type = assembly.GetType(typeName);
			if (this.obj_type == null)
			{
				throw new RemotingException("Type not found: " + typeName + ", " + assemblyName);
			}
		}

		/// <summary>Gets or sets the URL of the application to activate the type in.</summary>
		/// <returns>The URL of the application to activate the type in.</returns>
		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x060036FA RID: 14074 RVA: 0x000C6925 File Offset: 0x000C4B25
		// (set) Token: 0x060036FB RID: 14075 RVA: 0x000C692D File Offset: 0x000C4B2D
		public string ApplicationUrl
		{
			get
			{
				return this.app_url;
			}
			set
			{
				this.app_url = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Type" /> of the server-activated client type.</summary>
		/// <returns>Gets the <see cref="T:System.Type" /> of the server-activated client type.</returns>
		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x060036FC RID: 14076 RVA: 0x000C6936 File Offset: 0x000C4B36
		public Type ObjectType
		{
			get
			{
				return this.obj_type;
			}
		}

		/// <summary>Gets the URL of the server-activated client object.</summary>
		/// <returns>The URL of the server-activated client object.</returns>
		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x060036FD RID: 14077 RVA: 0x000C693E File Offset: 0x000C4B3E
		public string ObjectUrl
		{
			get
			{
				return this.obj_url;
			}
		}

		/// <summary>Returns the full type name, assembly name, and object URL of the server-activated client type as a <see cref="T:System.String" />.</summary>
		/// <returns>The full type name, assembly name, and object URL of the server-activated client type as a <see cref="T:System.String" />.</returns>
		// Token: 0x060036FE RID: 14078 RVA: 0x000C6946 File Offset: 0x000C4B46
		public override string ToString()
		{
			if (this.ApplicationUrl != null)
			{
				return base.TypeName + base.AssemblyName + this.ObjectUrl + this.ApplicationUrl;
			}
			return base.TypeName + base.AssemblyName + this.ObjectUrl;
		}

		// Token: 0x0400256D RID: 9581
		private Type obj_type;

		// Token: 0x0400256E RID: 9582
		private string obj_url;

		// Token: 0x0400256F RID: 9583
		private string app_url;
	}
}
