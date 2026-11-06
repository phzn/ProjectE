using System;
using System.Configuration;

namespace System.Net.Configuration
{
	/// <summary>Represents the type information for a custom <see cref="T:System.Net.IWebProxy" /> module. This class cannot be inherited.</summary>
	// Token: 0x0200076D RID: 1901
	public sealed class ModuleElement : ConfigurationElement
	{
		// Token: 0x06003BED RID: 15341 RVA: 0x000CD3E9 File Offset: 0x000CB5E9
		static ModuleElement()
		{
			ModuleElement.properties.Add(ModuleElement.typeProp);
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06003BEF RID: 15343 RVA: 0x000CD41E File Offset: 0x000CB61E
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ModuleElement.properties;
			}
		}

		/// <summary>Gets or sets the type and assembly information for the current instance.</summary>
		/// <returns>A string that identifies a type that implements the <see cref="T:System.Net.IWebProxy" /> interface or <see langword="null" /> if no value has been specified.</returns>
		// Token: 0x17000D96 RID: 3478
		// (get) Token: 0x06003BF0 RID: 15344 RVA: 0x000CD425 File Offset: 0x000CB625
		// (set) Token: 0x06003BF1 RID: 15345 RVA: 0x000CD437 File Offset: 0x000CB637
		[ConfigurationProperty("type")]
		public string Type
		{
			get
			{
				return (string)base[ModuleElement.typeProp];
			}
			set
			{
				base[ModuleElement.typeProp] = value;
			}
		}

		// Token: 0x0400239B RID: 9115
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x0400239C RID: 9116
		private static ConfigurationProperty typeProp = new ConfigurationProperty("type", typeof(string), null);
	}
}
