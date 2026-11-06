using System;

namespace UnityEngine.Localization.Metadata
{
	// Token: 0x020000DC RID: 220
	[Metadata(AllowedTypes = MetadataType.Locale)]
	[Serializable]
	public class FallbackLocale : IMetadata
	{
		// Token: 0x060005F8 RID: 1528 RVA: 0x00014D55 File Offset: 0x00012F55
		public FallbackLocale()
		{
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00014D5D File Offset: 0x00012F5D
		public FallbackLocale(Locale fallback)
		{
			this.Locale = fallback;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060005FA RID: 1530 RVA: 0x00014D6C File Offset: 0x00012F6C
		// (set) Token: 0x060005FB RID: 1531 RVA: 0x00014D74 File Offset: 0x00012F74
		public Locale Locale
		{
			get
			{
				return this.m_Locale;
			}
			set
			{
				this.m_Locale = value;
				if (this.IsCyclic(value))
				{
					this.m_Locale = null;
				}
			}
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00014D90 File Offset: 0x00012F90
		internal bool IsCyclic(Locale locale)
		{
			if (locale == null)
			{
				return false;
			}
			MetadataCollection metadata = locale.Metadata;
			FallbackLocale fallbackLocale = ((metadata != null) ? metadata.GetMetadata<FallbackLocale>() : null);
			while (fallbackLocale != null && fallbackLocale.Locale != null)
			{
				if (fallbackLocale.Locale == locale)
				{
					Debug.LogWarning(string.Format("Cyclic fallback linking detected. Can not set fallback locale '{0}' as it would create an infinite loop.", locale));
					return true;
				}
				MetadataCollection metadata2 = fallbackLocale.Locale.Metadata;
				fallbackLocale = ((metadata2 != null) ? metadata2.GetMetadata<FallbackLocale>() : null);
			}
			return false;
		}

		// Token: 0x04000211 RID: 529
		[SerializeField]
		private Locale m_Locale;
	}
}
