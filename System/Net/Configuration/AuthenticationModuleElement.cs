using System;
using System.Configuration;

namespace System.Net.Configuration
{
	/// <summary>Represents the type information for an authentication module. This class cannot be inherited.</summary>
	// Token: 0x0200075B RID: 1883
	public sealed class AuthenticationModuleElement : ConfigurationElement
	{
		// Token: 0x06003B64 RID: 15204 RVA: 0x000CC3DE File Offset: 0x000CA5DE
		static AuthenticationModuleElement()
		{
			AuthenticationModuleElement.properties.Add(AuthenticationModuleElement.typeProp);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Configuration.AuthenticationModuleElement" /> class.</summary>
		// Token: 0x06003B65 RID: 15205 RVA: 0x00031238 File Offset: 0x0002F438
		public AuthenticationModuleElement()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Configuration.AuthenticationModuleElement" /> class with the specified type information.</summary>
		/// <param name="typeName">A string that identifies the type and the assembly that contains it.</param>
		// Token: 0x06003B66 RID: 15206 RVA: 0x000CC414 File Offset: 0x000CA614
		public AuthenticationModuleElement(string typeName)
		{
			this.Type = typeName;
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06003B67 RID: 15207 RVA: 0x000CC423 File Offset: 0x000CA623
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AuthenticationModuleElement.properties;
			}
		}

		/// <summary>Gets or sets the type and assembly information for the current instance.</summary>
		/// <returns>A string that identifies a type that implements an authentication module or <see langword="null" /> if no value has been specified.</returns>
		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06003B68 RID: 15208 RVA: 0x000CC42A File Offset: 0x000CA62A
		// (set) Token: 0x06003B69 RID: 15209 RVA: 0x000CC43C File Offset: 0x000CA63C
		[ConfigurationProperty("type", Options = ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey)]
		public string Type
		{
			get
			{
				return (string)base[AuthenticationModuleElement.typeProp];
			}
			set
			{
				base[AuthenticationModuleElement.typeProp] = value;
			}
		}

		// Token: 0x0400237A RID: 9082
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x0400237B RID: 9083
		private static ConfigurationProperty typeProp = new ConfigurationProperty("type", typeof(string), null, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);
	}
}
