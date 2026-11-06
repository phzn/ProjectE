using System;
using System.Collections.Generic;
using UnityEngine.Localization.SmartFormat;

namespace UnityEngine.Localization.PropertyVariants.TrackedProperties
{
	// Token: 0x020000F0 RID: 240
	[Serializable]
	public class TrackedProperty<TPrimitive> : ITrackedPropertyValue<TPrimitive>, ITrackedProperty, IStringProperty, ISerializationCallbackReceiver, ITrackedPropertyRemoveVariant
	{
		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000659 RID: 1625 RVA: 0x00015B51 File Offset: 0x00013D51
		// (set) Token: 0x0600065A RID: 1626 RVA: 0x00015B59 File Offset: 0x00013D59
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

		// Token: 0x0600065B RID: 1627 RVA: 0x00015B62 File Offset: 0x00013D62
		public bool HasVariant(LocaleIdentifier localeIdentifier)
		{
			return this.m_VariantLookup.ContainsKey(localeIdentifier);
		}

		// Token: 0x0600065C RID: 1628 RVA: 0x00015B70 File Offset: 0x00013D70
		public void RemoveVariant(LocaleIdentifier localeIdentifier)
		{
			this.m_VariantLookup.Remove(localeIdentifier);
		}

		// Token: 0x0600065D RID: 1629 RVA: 0x00015B80 File Offset: 0x00013D80
		public bool GetValue(LocaleIdentifier localeIdentifier, out TPrimitive foundValue)
		{
			TrackedProperty<TPrimitive>.LocaleIdentifierValuePair localeIdentifierValuePair;
			if (this.m_VariantLookup.TryGetValue(localeIdentifier, out localeIdentifierValuePair))
			{
				foundValue = localeIdentifierValuePair.value;
				return true;
			}
			foundValue = default(TPrimitive);
			return false;
		}

		// Token: 0x0600065E RID: 1630 RVA: 0x00015BB4 File Offset: 0x00013DB4
		public bool GetValue(LocaleIdentifier localeIdentifier, LocaleIdentifier fallback, out TPrimitive foundValue)
		{
			TrackedProperty<TPrimitive>.LocaleIdentifierValuePair localeIdentifierValuePair;
			if (this.m_VariantLookup.TryGetValue(localeIdentifier, out localeIdentifierValuePair) || this.m_VariantLookup.TryGetValue(fallback, out localeIdentifierValuePair))
			{
				foundValue = localeIdentifierValuePair.value;
				return true;
			}
			foundValue = default(TPrimitive);
			return false;
		}

		// Token: 0x0600065F RID: 1631 RVA: 0x00015BF8 File Offset: 0x00013DF8
		public void SetValue(LocaleIdentifier localeIdentifier, TPrimitive value)
		{
			TrackedProperty<TPrimitive>.LocaleIdentifierValuePair localeIdentifierValuePair;
			if (!this.m_VariantLookup.TryGetValue(localeIdentifier, out localeIdentifierValuePair))
			{
				localeIdentifierValuePair = new TrackedProperty<TPrimitive>.LocaleIdentifierValuePair
				{
					localeIdentifier = localeIdentifier
				};
				this.m_VariantLookup[localeIdentifier] = localeIdentifierValuePair;
			}
			localeIdentifierValuePair.value = value;
		}

		// Token: 0x06000660 RID: 1632 RVA: 0x00015C38 File Offset: 0x00013E38
		public string GetValueAsString(LocaleIdentifier localeIdentifier)
		{
			TPrimitive tprimitive;
			if (!this.GetValue(localeIdentifier, out tprimitive))
			{
				return null;
			}
			return this.ConvertToString(tprimitive);
		}

		// Token: 0x06000661 RID: 1633 RVA: 0x00015C5C File Offset: 0x00013E5C
		public string GetValueAsString(LocaleIdentifier localeIdentifier, LocaleIdentifier fallback)
		{
			TPrimitive tprimitive;
			if (!this.GetValue(localeIdentifier, fallback, out tprimitive))
			{
				return null;
			}
			return this.ConvertToString(tprimitive);
		}

		// Token: 0x06000662 RID: 1634 RVA: 0x00015C80 File Offset: 0x00013E80
		public void SetValueFromString(LocaleIdentifier localeIdentifier, string stringValue)
		{
			TPrimitive tprimitive = this.ConvertFromString(stringValue);
			this.SetValue(localeIdentifier, tprimitive);
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x00015C9D File Offset: 0x00013E9D
		protected virtual string ConvertToString(TPrimitive value)
		{
			return Convert.ToString(value);
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x00015CAA File Offset: 0x00013EAA
		protected virtual TPrimitive ConvertFromString(string value)
		{
			return (TPrimitive)((object)Convert.ChangeType(value, typeof(TPrimitive)));
		}

		// Token: 0x06000665 RID: 1637 RVA: 0x00015CC4 File Offset: 0x00013EC4
		public void OnBeforeSerialize()
		{
			this.m_VariantData.Clear();
			foreach (TrackedProperty<TPrimitive>.LocaleIdentifierValuePair localeIdentifierValuePair in this.m_VariantLookup.Values)
			{
				this.m_VariantData.Add(localeIdentifierValuePair);
			}
		}

		// Token: 0x06000666 RID: 1638 RVA: 0x00015D2C File Offset: 0x00013F2C
		public void OnAfterDeserialize()
		{
			this.m_VariantLookup.Clear();
			foreach (TrackedProperty<TPrimitive>.LocaleIdentifierValuePair localeIdentifierValuePair in this.m_VariantData)
			{
				this.m_VariantLookup[localeIdentifierValuePair.localeIdentifier] = localeIdentifierValuePair;
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00015D98 File Offset: 0x00013F98
		public override string ToString()
		{
			return Smart.Format("{GetType().Name}({PropertyPath}) - {1:list:{Key}({Value.value})|, |, }", this, this.m_VariantLookup);
		}

		// Token: 0x0400022B RID: 555
		[SerializeField]
		private string m_PropertyPath;

		// Token: 0x0400022C RID: 556
		[SerializeField]
		private List<TrackedProperty<TPrimitive>.LocaleIdentifierValuePair> m_VariantData = new List<TrackedProperty<TPrimitive>.LocaleIdentifierValuePair>();

		// Token: 0x0400022D RID: 557
		internal Dictionary<LocaleIdentifier, TrackedProperty<TPrimitive>.LocaleIdentifierValuePair> m_VariantLookup = new Dictionary<LocaleIdentifier, TrackedProperty<TPrimitive>.LocaleIdentifierValuePair>();

		// Token: 0x02000163 RID: 355
		[Serializable]
		internal class LocaleIdentifierValuePair
		{
			// Token: 0x04000320 RID: 800
			public LocaleIdentifier localeIdentifier;

			// Token: 0x04000321 RID: 801
			public TPrimitive value;
		}
	}
}
