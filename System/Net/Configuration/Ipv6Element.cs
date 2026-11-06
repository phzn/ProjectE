using System;
using System.Configuration;

namespace System.Net.Configuration
{
	/// <summary>Determines whether Internet Protocol version 6 is enabled on the local computer. This class cannot be inherited.</summary>
	// Token: 0x0200076B RID: 1899
	public sealed class Ipv6Element : ConfigurationElement
	{
		// Token: 0x06003BE6 RID: 15334 RVA: 0x000CD36C File Offset: 0x000CB56C
		static Ipv6Element()
		{
			Ipv6Element.properties.Add(Ipv6Element.enabledProp);
		}

		/// <summary>Gets or sets a Boolean value that indicates whether Internet Protocol version 6 is enabled on the local computer.</summary>
		/// <returns>
		///   <see langword="true" /> if IPv6 is enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06003BE8 RID: 15336 RVA: 0x000CD3A6 File Offset: 0x000CB5A6
		// (set) Token: 0x06003BE9 RID: 15337 RVA: 0x000CD3B8 File Offset: 0x000CB5B8
		[ConfigurationProperty("enabled", DefaultValue = "False")]
		public bool Enabled
		{
			get
			{
				return (bool)base[Ipv6Element.enabledProp];
			}
			set
			{
				base[Ipv6Element.enabledProp] = value;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06003BEA RID: 15338 RVA: 0x000CD3CB File Offset: 0x000CB5CB
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return Ipv6Element.properties;
			}
		}

		// Token: 0x04002399 RID: 9113
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x0400239A RID: 9114
		private static ConfigurationProperty enabledProp = new ConfigurationProperty("enabled", typeof(bool), false);
	}
}
