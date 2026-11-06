using System;
using System.Configuration;

namespace System.Net.Configuration
{
	/// <summary>Represents the maximum number of connections to a remote computer. This class cannot be inherited.</summary>
	// Token: 0x02000760 RID: 1888
	public sealed class ConnectionManagementElement : ConfigurationElement
	{
		// Token: 0x06003B91 RID: 15249 RVA: 0x000CC594 File Offset: 0x000CA794
		static ConnectionManagementElement()
		{
			ConnectionManagementElement.properties.Add(ConnectionManagementElement.addressProp);
			ConnectionManagementElement.properties.Add(ConnectionManagementElement.maxConnectionProp);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Configuration.ConnectionManagementElement" /> class.</summary>
		// Token: 0x06003B92 RID: 15250 RVA: 0x00031238 File Offset: 0x0002F438
		public ConnectionManagementElement()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.Configuration.ConnectionManagementElement" /> class with the specified address and connection limit information.</summary>
		/// <param name="address">A string that identifies the address of a remote computer.</param>
		/// <param name="maxConnection">An integer that identifies the maximum number of connections allowed to <paramref name="address" /> from the local computer.</param>
		// Token: 0x06003B93 RID: 15251 RVA: 0x000CC604 File Offset: 0x000CA804
		public ConnectionManagementElement(string address, int maxConnection)
		{
			this.Address = address;
			this.MaxConnection = maxConnection;
		}

		/// <summary>Gets or sets the address for remote computers.</summary>
		/// <returns>A string that contains a regular expression describing an IP address or DNS name.</returns>
		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06003B94 RID: 15252 RVA: 0x000CC61A File Offset: 0x000CA81A
		// (set) Token: 0x06003B95 RID: 15253 RVA: 0x000CC62C File Offset: 0x000CA82C
		[ConfigurationProperty("address", Options = ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey)]
		public string Address
		{
			get
			{
				return (string)base[ConnectionManagementElement.addressProp];
			}
			set
			{
				base[ConnectionManagementElement.addressProp] = value;
			}
		}

		/// <summary>Gets or sets the maximum number of connections that can be made to a remote computer.</summary>
		/// <returns>An integer that specifies the maximum number of connections.</returns>
		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06003B96 RID: 15254 RVA: 0x000CC63A File Offset: 0x000CA83A
		// (set) Token: 0x06003B97 RID: 15255 RVA: 0x000CC64C File Offset: 0x000CA84C
		[ConfigurationProperty("maxconnection", DefaultValue = "6", Options = ConfigurationPropertyOptions.IsRequired)]
		public int MaxConnection
		{
			get
			{
				return (int)base[ConnectionManagementElement.maxConnectionProp];
			}
			set
			{
				base[ConnectionManagementElement.maxConnectionProp] = value;
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06003B98 RID: 15256 RVA: 0x000CC65F File Offset: 0x000CA85F
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ConnectionManagementElement.properties;
			}
		}

		// Token: 0x04002380 RID: 9088
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002381 RID: 9089
		private static ConfigurationProperty addressProp = new ConfigurationProperty("address", typeof(string), null, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04002382 RID: 9090
		private static ConfigurationProperty maxConnectionProp = new ConfigurationProperty("maxconnection", typeof(int), 1, ConfigurationPropertyOptions.IsRequired);
	}
}
