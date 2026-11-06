using System;

namespace UnityEngine.Localization.PropertyVariants.TrackedProperties
{
	// Token: 0x020000EC RID: 236
	[Serializable]
	public class LocalizedAssetProperty : ITrackedProperty
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00015AD4 File Offset: 0x00013CD4
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x00015ADC File Offset: 0x00013CDC
		public LocalizedAssetBase LocalizedObject
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

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00015AE5 File Offset: 0x00013CE5
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x00015AED File Offset: 0x00013CED
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

		// Token: 0x0600064D RID: 1613 RVA: 0x00015AF6 File Offset: 0x00013CF6
		public bool HasVariant(LocaleIdentifier localeIdentifier)
		{
			bool isEmpty = this.LocalizedObject.IsEmpty;
			return false;
		}

		// Token: 0x04000227 RID: 551
		[SerializeReference]
		private LocalizedAssetBase m_Localized;

		// Token: 0x04000228 RID: 552
		[SerializeField]
		private string m_PropertyPath;
	}
}
