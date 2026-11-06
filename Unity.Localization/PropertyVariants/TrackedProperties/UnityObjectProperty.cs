using System;
using System.Collections.Generic;

namespace UnityEngine.Localization.PropertyVariants.TrackedProperties
{
	// Token: 0x02000100 RID: 256
	[Serializable]
	public class UnityObjectProperty : ITrackedPropertyValue<Object>, ITrackedProperty, ISerializationCallbackReceiver
	{
		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x00015E93 File Offset: 0x00014093
		// (set) Token: 0x0600067D RID: 1661 RVA: 0x00015E9B File Offset: 0x0001409B
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

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x00015EA4 File Offset: 0x000140A4
		// (set) Token: 0x0600067F RID: 1663 RVA: 0x00015EAC File Offset: 0x000140AC
		public Type PropertyType { get; set; }

		// Token: 0x06000680 RID: 1664 RVA: 0x00015EB5 File Offset: 0x000140B5
		public bool HasVariant(LocaleIdentifier localeIdentifier)
		{
			return this.m_VariantLookup.ContainsKey(localeIdentifier);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00015EC3 File Offset: 0x000140C3
		public void RemoveVariant(LocaleIdentifier localeIdentifier)
		{
			this.m_VariantLookup.Remove(localeIdentifier);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00015ED4 File Offset: 0x000140D4
		public bool GetValue(LocaleIdentifier localeIdentifier, out Object foundValue)
		{
			UnityObjectProperty.LocaleIdentifierValuePair localeIdentifierValuePair;
			if (this.m_VariantLookup.TryGetValue(localeIdentifier, out localeIdentifierValuePair))
			{
				foundValue = localeIdentifierValuePair.value.asset;
				return true;
			}
			foundValue = null;
			return false;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00015F04 File Offset: 0x00014104
		public bool GetValue(LocaleIdentifier localeIdentifier, LocaleIdentifier fallback, out Object foundValue)
		{
			UnityObjectProperty.LocaleIdentifierValuePair localeIdentifierValuePair;
			if (this.m_VariantLookup.TryGetValue(localeIdentifier, out localeIdentifierValuePair) || this.m_VariantLookup.TryGetValue(fallback, out localeIdentifierValuePair))
			{
				foundValue = localeIdentifierValuePair.value.asset;
				return true;
			}
			foundValue = null;
			return false;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00015F44 File Offset: 0x00014144
		public void SetValue(LocaleIdentifier localeIdentifier, Object newValue)
		{
			UnityObjectProperty.LocaleIdentifierValuePair localeIdentifierValuePair;
			if (!this.m_VariantLookup.TryGetValue(localeIdentifier, out localeIdentifierValuePair))
			{
				localeIdentifierValuePair = new UnityObjectProperty.LocaleIdentifierValuePair
				{
					localeIdentifier = localeIdentifier
				};
				this.m_VariantLookup[localeIdentifier] = localeIdentifierValuePair;
			}
			localeIdentifierValuePair.value.asset = newValue;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00015F88 File Offset: 0x00014188
		public void OnBeforeSerialize()
		{
			Type propertyType = this.PropertyType;
			this.m_TypeString = ((propertyType != null) ? propertyType.AssemblyQualifiedName : null);
			this.m_VariantData.Clear();
			foreach (UnityObjectProperty.LocaleIdentifierValuePair localeIdentifierValuePair in this.m_VariantLookup.Values)
			{
				this.m_VariantData.Add(localeIdentifierValuePair);
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00016008 File Offset: 0x00014208
		public void OnAfterDeserialize()
		{
			this.m_VariantLookup.Clear();
			foreach (UnityObjectProperty.LocaleIdentifierValuePair localeIdentifierValuePair in this.m_VariantData)
			{
				this.m_VariantLookup[localeIdentifierValuePair.localeIdentifier] = localeIdentifierValuePair;
			}
			if (!string.IsNullOrEmpty(this.m_TypeString))
			{
				this.PropertyType = Type.GetType(this.m_TypeString);
			}
		}

		// Token: 0x0400022E RID: 558
		[SerializeField]
		private string m_PropertyPath;

		// Token: 0x0400022F RID: 559
		[SerializeField]
		private string m_TypeString;

		// Token: 0x04000230 RID: 560
		[SerializeField]
		private List<UnityObjectProperty.LocaleIdentifierValuePair> m_VariantData = new List<UnityObjectProperty.LocaleIdentifierValuePair>();

		// Token: 0x04000231 RID: 561
		internal Dictionary<LocaleIdentifier, UnityObjectProperty.LocaleIdentifierValuePair> m_VariantLookup = new Dictionary<LocaleIdentifier, UnityObjectProperty.LocaleIdentifierValuePair>();

		// Token: 0x02000164 RID: 356
		[Serializable]
		internal class LocaleIdentifierValuePair
		{
			// Token: 0x04000322 RID: 802
			public LocaleIdentifier localeIdentifier;

			// Token: 0x04000323 RID: 803
			public LazyLoadReference<Object> value;
		}
	}
}
