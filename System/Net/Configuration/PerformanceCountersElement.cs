using System;
using System.Configuration;

namespace System.Net.Configuration
{
	/// <summary>Represents the performance counter element in the <see langword="System.Net" /> configuration file that determines whether networking performance counters are enabled. This class cannot be inherited.</summary>
	// Token: 0x02000771 RID: 1905
	public sealed class PerformanceCountersElement : ConfigurationElement
	{
		// Token: 0x06003C00 RID: 15360 RVA: 0x000CD859 File Offset: 0x000CBA59
		static PerformanceCountersElement()
		{
			PerformanceCountersElement.properties.Add(PerformanceCountersElement.enabledProp);
		}

		/// <summary>Gets or sets whether performance counters are enabled.</summary>
		/// <returns>
		///   <see langword="true" /> if performance counters are enabled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000D9E RID: 3486
		// (get) Token: 0x06003C01 RID: 15361 RVA: 0x000CD893 File Offset: 0x000CBA93
		// (set) Token: 0x06003C02 RID: 15362 RVA: 0x000CD8A5 File Offset: 0x000CBAA5
		[ConfigurationProperty("enabled", DefaultValue = "False")]
		public bool Enabled
		{
			get
			{
				return (bool)base[PerformanceCountersElement.enabledProp];
			}
			set
			{
				base[PerformanceCountersElement.enabledProp] = value;
			}
		}

		// Token: 0x17000D9F RID: 3487
		// (get) Token: 0x06003C03 RID: 15363 RVA: 0x000CD8B8 File Offset: 0x000CBAB8
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return PerformanceCountersElement.properties;
			}
		}

		// Token: 0x0400239D RID: 9117
		private static ConfigurationProperty enabledProp = new ConfigurationProperty("enabled", typeof(bool), false);

		// Token: 0x0400239E RID: 9118
		private static ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();
	}
}
