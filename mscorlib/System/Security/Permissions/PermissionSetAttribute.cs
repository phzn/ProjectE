using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using Mono.Security.Cryptography;
using Mono.Xml;

namespace System.Security.Permissions
{
	/// <summary>Allows security actions for a <see cref="T:System.Security.PermissionSet" /> to be applied to code using declarative security. This class cannot be inherited.</summary>
	// Token: 0x0200044B RID: 1099
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class PermissionSetAttribute : CodeAccessSecurityAttribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.PermissionSetAttribute" /> class with the specified security action.</summary>
		/// <param name="action">One of the enumeration values that specifies a security action.</param>
		// Token: 0x06002C8D RID: 11405 RVA: 0x0009DD00 File Offset: 0x0009BF00
		public PermissionSetAttribute(SecurityAction action)
			: base(action)
		{
		}

		/// <summary>Gets or sets a file containing the XML representation of a custom permission set to be declared.</summary>
		/// <returns>The physical path to the file containing the XML representation of the permission set.</returns>
		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06002C8E RID: 11406 RVA: 0x0009FD44 File Offset: 0x0009DF44
		// (set) Token: 0x06002C8F RID: 11407 RVA: 0x0009FD4C File Offset: 0x0009DF4C
		public string File
		{
			get
			{
				return this.file;
			}
			set
			{
				this.file = value;
			}
		}

		/// <summary>Gets or sets the hexadecimal representation of the XML encoded permission set.</summary>
		/// <returns>The hexadecimal representation of the XML encoded permission set.</returns>
		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06002C90 RID: 11408 RVA: 0x0009FD55 File Offset: 0x0009DF55
		// (set) Token: 0x06002C91 RID: 11409 RVA: 0x0009FD5D File Offset: 0x0009DF5D
		public string Hex
		{
			get
			{
				return this.hex;
			}
			set
			{
				this.hex = value;
			}
		}

		/// <summary>Gets or sets the name of the permission set.</summary>
		/// <returns>The name of an immutable <see cref="T:System.Security.NamedPermissionSet" /> (one of several permission sets that are contained in the default policy and cannot be altered).</returns>
		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06002C92 RID: 11410 RVA: 0x0009FD66 File Offset: 0x0009DF66
		// (set) Token: 0x06002C93 RID: 11411 RVA: 0x0009FD6E File Offset: 0x0009DF6E
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the file specified by <see cref="P:System.Security.Permissions.PermissionSetAttribute.File" /> is Unicode or ASCII encoded.</summary>
		/// <returns>
		///   <see langword="true" /> if the file is Unicode encoded; otherwise, <see langword="false" />.</returns>
		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x06002C94 RID: 11412 RVA: 0x0009FD77 File Offset: 0x0009DF77
		// (set) Token: 0x06002C95 RID: 11413 RVA: 0x0009FD7F File Offset: 0x0009DF7F
		public bool UnicodeEncoded
		{
			get
			{
				return this.isUnicodeEncoded;
			}
			set
			{
				this.isUnicodeEncoded = value;
			}
		}

		/// <summary>Gets or sets the XML representation of a permission set.</summary>
		/// <returns>The XML representation of a permission set.</returns>
		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06002C96 RID: 11414 RVA: 0x0009FD88 File Offset: 0x0009DF88
		// (set) Token: 0x06002C97 RID: 11415 RVA: 0x0009FD90 File Offset: 0x0009DF90
		public string XML
		{
			get
			{
				return this.xml;
			}
			set
			{
				this.xml = value;
			}
		}

		/// <summary>This method is not used.</summary>
		/// <returns>A null reference (<see langword="nothing" /> in Visual Basic) in all cases.</returns>
		// Token: 0x06002C98 RID: 11416 RVA: 0x0000AF5E File Offset: 0x0000915E
		public override IPermission CreatePermission()
		{
			return null;
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x0009FD9C File Offset: 0x0009DF9C
		private PermissionSet CreateFromXml(string xml)
		{
			SecurityParser securityParser = new SecurityParser();
			try
			{
				securityParser.LoadXml(xml);
			}
			catch (SmallXmlParserException ex)
			{
				throw new XmlSyntaxException(ex.Line, ex.ToString());
			}
			SecurityElement securityElement = securityParser.ToXml();
			string text = securityElement.Attribute("class");
			if (text == null)
			{
				return null;
			}
			PermissionState permissionState = PermissionState.None;
			if (CodeAccessPermission.IsUnrestricted(securityElement))
			{
				permissionState = PermissionState.Unrestricted;
			}
			if (text.EndsWith("NamedPermissionSet"))
			{
				NamedPermissionSet namedPermissionSet = new NamedPermissionSet(securityElement.Attribute("Name"), permissionState);
				namedPermissionSet.FromXml(securityElement);
				return namedPermissionSet;
			}
			if (text.EndsWith("PermissionSet"))
			{
				PermissionSet permissionSet = new PermissionSet(permissionState);
				permissionSet.FromXml(securityElement);
				return permissionSet;
			}
			return null;
		}

		/// <summary>Creates and returns a new permission set based on this permission set attribute object.</summary>
		/// <returns>A new permission set.</returns>
		// Token: 0x06002C9A RID: 11418 RVA: 0x0009FE44 File Offset: 0x0009E044
		public PermissionSet CreatePermissionSet()
		{
			PermissionSet permissionSet = null;
			if (base.Unrestricted)
			{
				permissionSet = new PermissionSet(PermissionState.Unrestricted);
			}
			else
			{
				permissionSet = new PermissionSet(PermissionState.None);
				if (this.name != null)
				{
					return PolicyLevel.CreateAppDomainLevel().GetNamedPermissionSet(this.name);
				}
				if (this.file != null)
				{
					Encoding encoding = (this.isUnicodeEncoded ? Encoding.Unicode : Encoding.ASCII);
					using (StreamReader streamReader = new StreamReader(this.file, encoding))
					{
						return this.CreateFromXml(streamReader.ReadToEnd());
					}
				}
				if (this.xml != null)
				{
					permissionSet = this.CreateFromXml(this.xml);
				}
				else if (this.hex != null)
				{
					Encoding ascii = Encoding.ASCII;
					byte[] array = CryptoConvert.FromHex(this.hex);
					permissionSet = this.CreateFromXml(ascii.GetString(array, 0, array.Length));
				}
			}
			return permissionSet;
		}

		// Token: 0x04002067 RID: 8295
		private string file;

		// Token: 0x04002068 RID: 8296
		private string name;

		// Token: 0x04002069 RID: 8297
		private bool isUnicodeEncoded;

		// Token: 0x0400206A RID: 8298
		private string xml;

		// Token: 0x0400206B RID: 8299
		private string hex;
	}
}
