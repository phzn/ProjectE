using System;
using System.Configuration;

namespace System.Net.Configuration
{
	/// <summary>Represents the configuration section for Web request modules. This class cannot be inherited.</summary>
	// Token: 0x02000781 RID: 1921
	public sealed class WebRequestModulesSection : ConfigurationSection
	{
		// Token: 0x06003C8A RID: 15498 RVA: 0x000CE5DC File Offset: 0x000CC7DC
		static WebRequestModulesSection()
		{
			WebRequestModulesSection.properties.Add(WebRequestModulesSection.webRequestModulesProp);
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06003C8B RID: 15499 RVA: 0x000CE612 File Offset: 0x000CC812
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return WebRequestModulesSection.properties;
			}
		}

		/// <summary>Gets the collection of Web request modules in the section.</summary>
		/// <returns>A <see cref="T:System.Net.Configuration.WebRequestModuleElementCollection" /> containing the registered Web request modules.</returns>
		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06003C8C RID: 15500 RVA: 0x000CE619 File Offset: 0x000CC819
		[ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
		public WebRequestModuleElementCollection WebRequestModules
		{
			get
			{
				return (WebRequestModuleElementCollection)base[WebRequestModulesSection.webRequestModulesProp];
			}
		}

		// Token: 0x06003C8D RID: 15501 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO]
		protected override void PostDeserialize()
		{
		}

		// Token: 0x06003C8E RID: 15502 RVA: 0x00003917 File Offset: 0x00001B17
		[MonoTODO]
		protected override void InitializeDefault()
		{
		}

		// Token: 0x040023D0 RID: 9168
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x040023D1 RID: 9169
		private static ConfigurationProperty webRequestModulesProp = new ConfigurationProperty("", typeof(WebRequestModuleElementCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
	}
}
