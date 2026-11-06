using System;
using System.Configuration;

namespace System.Net.Configuration
{
	/// <summary>Represents the maximum length for response headers. This class cannot be inherited.</summary>
	// Token: 0x0200076A RID: 1898
	public sealed class HttpWebRequestElement : ConfigurationElement
	{
		// Token: 0x06003BDA RID: 15322 RVA: 0x000CD200 File Offset: 0x000CB400
		static HttpWebRequestElement()
		{
			HttpWebRequestElement.properties.Add(HttpWebRequestElement.maximumErrorResponseLengthProp);
			HttpWebRequestElement.properties.Add(HttpWebRequestElement.maximumResponseHeadersLengthProp);
			HttpWebRequestElement.properties.Add(HttpWebRequestElement.maximumUnauthorizedUploadLengthProp);
			HttpWebRequestElement.properties.Add(HttpWebRequestElement.useUnsafeHeaderParsingProp);
		}

		/// <summary>Gets or sets the maximum allowed length of an error response.</summary>
		/// <returns>A 32-bit signed integer containing the maximum length in kilobytes (1024 bytes) of the error response. The default value is 64.</returns>
		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06003BDC RID: 15324 RVA: 0x000CD2D1 File Offset: 0x000CB4D1
		// (set) Token: 0x06003BDD RID: 15325 RVA: 0x000CD2E3 File Offset: 0x000CB4E3
		[ConfigurationProperty("maximumErrorResponseLength", DefaultValue = "64")]
		public int MaximumErrorResponseLength
		{
			get
			{
				return (int)base[HttpWebRequestElement.maximumErrorResponseLengthProp];
			}
			set
			{
				base[HttpWebRequestElement.maximumErrorResponseLengthProp] = value;
			}
		}

		/// <summary>Gets or sets the maximum allowed length of the response headers.</summary>
		/// <returns>A 32-bit signed integer containing the maximum length in kilobytes (1024 bytes) of the response headers. The default value is 64.</returns>
		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06003BDE RID: 15326 RVA: 0x000CD2F6 File Offset: 0x000CB4F6
		// (set) Token: 0x06003BDF RID: 15327 RVA: 0x000CD308 File Offset: 0x000CB508
		[ConfigurationProperty("maximumResponseHeadersLength", DefaultValue = "64")]
		public int MaximumResponseHeadersLength
		{
			get
			{
				return (int)base[HttpWebRequestElement.maximumResponseHeadersLengthProp];
			}
			set
			{
				base[HttpWebRequestElement.maximumResponseHeadersLengthProp] = value;
			}
		}

		/// <summary>Gets or sets the maximum length of an upload in response to an unauthorized error code.</summary>
		/// <returns>A 32-bit signed integer containing the maximum length (in multiple of 1,024 byte units) of an upload in response to an unauthorized error code. A value of -1 indicates that no size limit will be imposed on the upload. Setting the <see cref="P:System.Net.Configuration.HttpWebRequestElement.MaximumUnauthorizedUploadLength" /> property to any other value will only send the request body if it is smaller than the number of bytes specified. So a value of 1 would indicate to only send the request body if it is smaller than 1,024 bytes.  
		///  The default value for this property is -1.</returns>
		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x000CD31B File Offset: 0x000CB51B
		// (set) Token: 0x06003BE1 RID: 15329 RVA: 0x000CD32D File Offset: 0x000CB52D
		[ConfigurationProperty("maximumUnauthorizedUploadLength", DefaultValue = "-1")]
		public int MaximumUnauthorizedUploadLength
		{
			get
			{
				return (int)base[HttpWebRequestElement.maximumUnauthorizedUploadLengthProp];
			}
			set
			{
				base[HttpWebRequestElement.maximumUnauthorizedUploadLengthProp] = value;
			}
		}

		/// <summary>Setting this property ignores validation errors that occur during HTTP parsing.</summary>
		/// <returns>Boolean that indicates whether this property has been set.</returns>
		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06003BE2 RID: 15330 RVA: 0x000CD340 File Offset: 0x000CB540
		// (set) Token: 0x06003BE3 RID: 15331 RVA: 0x000CD352 File Offset: 0x000CB552
		[ConfigurationProperty("useUnsafeHeaderParsing", DefaultValue = "False")]
		public bool UseUnsafeHeaderParsing
		{
			get
			{
				return (bool)base[HttpWebRequestElement.useUnsafeHeaderParsingProp];
			}
			set
			{
				base[HttpWebRequestElement.useUnsafeHeaderParsingProp] = value;
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06003BE4 RID: 15332 RVA: 0x000CD365 File Offset: 0x000CB565
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpWebRequestElement.properties;
			}
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x00066D7A File Offset: 0x00064F7A
		[MonoTODO]
		protected override void PostDeserialize()
		{
			base.PostDeserialize();
		}

		// Token: 0x04002394 RID: 9108
		private static ConfigurationProperty maximumErrorResponseLengthProp = new ConfigurationProperty("maximumErrorResponseLength", typeof(int), 64);

		// Token: 0x04002395 RID: 9109
		private static ConfigurationProperty maximumResponseHeadersLengthProp = new ConfigurationProperty("maximumResponseHeadersLength", typeof(int), 64);

		// Token: 0x04002396 RID: 9110
		private static ConfigurationProperty maximumUnauthorizedUploadLengthProp = new ConfigurationProperty("maximumUnauthorizedUploadLength", typeof(int), -1);

		// Token: 0x04002397 RID: 9111
		private static ConfigurationProperty useUnsafeHeaderParsingProp = new ConfigurationProperty("useUnsafeHeaderParsing", typeof(bool), false);

		// Token: 0x04002398 RID: 9112
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();
	}
}
