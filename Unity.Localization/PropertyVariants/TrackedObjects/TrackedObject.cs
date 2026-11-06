using System;
using System.Collections.Generic;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.PropertyVariants.TrackedObjects
{
	// Token: 0x02000104 RID: 260
	[Serializable]
	public abstract class TrackedObject : ISerializationCallbackReceiver
	{
		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x00016A78 File Offset: 0x00014C78
		// (set) Token: 0x0600069A RID: 1690 RVA: 0x00016A80 File Offset: 0x00014C80
		public Object Target
		{
			get
			{
				return this.m_Target;
			}
			set
			{
				this.m_Target = value;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x0600069B RID: 1691 RVA: 0x00016A89 File Offset: 0x00014C89
		public IList<ITrackedProperty> TrackedProperties
		{
			get
			{
				return this.m_TrackedProperties.items;
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00016A96 File Offset: 0x00014C96
		public virtual bool CanTrackProperty(string propertyPath)
		{
			return true;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00016A9C File Offset: 0x00014C9C
		public T AddTrackedProperty<T>(string propertyPath) where T : ITrackedProperty, new()
		{
			T t = new T();
			t.PropertyPath = propertyPath;
			T t2 = t;
			this.AddTrackedProperty(t2);
			return t2;
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00016ACC File Offset: 0x00014CCC
		public virtual void AddTrackedProperty(ITrackedProperty trackedProperty)
		{
			if (trackedProperty == null)
			{
				throw new ArgumentNullException("trackedProperty");
			}
			if (string.IsNullOrEmpty(trackedProperty.PropertyPath))
			{
				throw new ArgumentException("Property path must not be null or empty.");
			}
			if (this.m_PropertiesLookup.ContainsKey(trackedProperty.PropertyPath))
			{
				throw new ArgumentException(trackedProperty.PropertyPath + " is already tracked.");
			}
			this.m_PropertiesLookup[trackedProperty.PropertyPath] = trackedProperty;
			this.TrackedProperties.Add(trackedProperty);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00016B46 File Offset: 0x00014D46
		public virtual bool RemoveTrackedProperty(ITrackedProperty trackedProperty)
		{
			this.m_PropertiesLookup.Remove(trackedProperty.PropertyPath);
			return this.TrackedProperties.Remove(trackedProperty);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00016B68 File Offset: 0x00014D68
		public T GetTrackedProperty<T>(string propertyPath, bool create = true) where T : ITrackedProperty, new()
		{
			ITrackedProperty trackedProperty = this.GetTrackedProperty(propertyPath);
			if (trackedProperty is T)
			{
				return (T)((object)trackedProperty);
			}
			if (!create)
			{
				return default(T);
			}
			return this.AddTrackedProperty<T>(propertyPath);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00016BA4 File Offset: 0x00014DA4
		public virtual ITrackedProperty GetTrackedProperty(string propertyPath)
		{
			ITrackedProperty trackedProperty;
			if (!this.m_PropertiesLookup.TryGetValue(propertyPath, out trackedProperty))
			{
				return null;
			}
			return trackedProperty;
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00016BC4 File Offset: 0x00014DC4
		public virtual ITrackedProperty CreateCustomTrackedProperty(string propertyPath)
		{
			return null;
		}

		// Token: 0x060006A3 RID: 1699
		public abstract AsyncOperationHandle ApplyLocale(Locale variantLocale, Locale defaultLocale);

		// Token: 0x060006A4 RID: 1700 RVA: 0x00016BC7 File Offset: 0x00014DC7
		protected virtual void PostApplyTrackedProperties()
		{
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00016BCC File Offset: 0x00014DCC
		public void OnAfterDeserialize()
		{
			this.m_PropertiesLookup.Clear();
			foreach (ITrackedProperty trackedProperty in this.m_TrackedProperties.items)
			{
				if (trackedProperty != null)
				{
					this.m_PropertiesLookup[trackedProperty.PropertyPath] = trackedProperty;
				}
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00016C40 File Offset: 0x00014E40
		public void OnBeforeSerialize()
		{
			this.m_TrackedProperties.items.Clear();
			foreach (ITrackedProperty trackedProperty in this.m_PropertiesLookup.Values)
			{
				this.m_TrackedProperties.items.Add(trackedProperty);
			}
		}

		// Token: 0x04000237 RID: 567
		[SerializeField]
		[HideInInspector]
		private Object m_Target;

		// Token: 0x04000238 RID: 568
		[SerializeField]
		private TrackedObject.TrackedPropertiesCollection m_TrackedProperties = new TrackedObject.TrackedPropertiesCollection();

		// Token: 0x04000239 RID: 569
		private readonly Dictionary<string, ITrackedProperty> m_PropertiesLookup = new Dictionary<string, ITrackedProperty>();

		// Token: 0x0200016A RID: 362
		[Serializable]
		internal class TrackedPropertiesCollection
		{
			// Token: 0x04000337 RID: 823
			[SerializeReference]
			public List<ITrackedProperty> items = new List<ITrackedProperty>();
		}
	}
}
