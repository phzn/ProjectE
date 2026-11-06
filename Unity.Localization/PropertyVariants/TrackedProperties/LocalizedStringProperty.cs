using System;

namespace UnityEngine.Localization.PropertyVariants.TrackedProperties
{
	// Token: 0x020000ED RID: 237
	[Serializable]
	public class LocalizedStringProperty : ITrackedProperty
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00015B0D File Offset: 0x00013D0D
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x00015B15 File Offset: 0x00013D15
		public LocalizedString LocalizedString
		{
			get
			{
				return this.m_Localized;
			}
			set
			{
				this.m_Localized = value;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00015B1E File Offset: 0x00013D1E
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x00015B26 File Offset: 0x00013D26
		public string PropertyPath
		{
			get
			{
				return this.m_PropertyPath;
			}
			set
			{
				this.m_PropertyPath = value;
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00015B2F File Offset: 0x00013D2F
		public bool HasVariant(LocaleIdentifier localeIdentifier)
		{
			bool isEmpty = this.LocalizedString.IsEmpty;
			return false;
		}

		// Token: 0x04000229 RID: 553
		[SerializeField]
		private LocalizedString m_Localized = new LocalizedString();

		// Token: 0x0400022A RID: 554
		[SerializeField]
		private string m_PropertyPath;
	}
}
