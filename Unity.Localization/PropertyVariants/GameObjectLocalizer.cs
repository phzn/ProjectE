using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization.PropertyVariants.TrackedObjects;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.Localization.Settings;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.PropertyVariants
{
	// Token: 0x020000E9 RID: 233
	[ExecuteAlways]
	[DisallowMultipleComponent]
	public class GameObjectLocalizer : MonoBehaviour
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x000155B8 File Offset: 0x000137B8
		// (set) Token: 0x06000634 RID: 1588 RVA: 0x000155C0 File Offset: 0x000137C0
		internal AsyncOperationHandle CurrentOperation { get; set; }

		// Token: 0x06000635 RID: 1589 RVA: 0x000155CC File Offset: 0x000137CC
		private void OnEnable()
		{
			LocalizationSettings.SelectedLocaleChanged += this.SelectedLocaleChanged;
			this.RegisterChanges();
			if (this.m_CurrentLocale != null)
			{
				Locale selectedLocale = LocalizationSettings.SelectedLocale;
				if (this.m_CurrentLocale != selectedLocale)
				{
					this.SelectedLocaleChanged(selectedLocale);
				}
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00015614 File Offset: 0x00013814
		private void OnDisable()
		{
			this.UnregisterChanges();
			AddressablesInterface.SafeRelease(this.CurrentOperation);
			this.CurrentOperation = default(AsyncOperationHandle);
			LocalizationSettings.SelectedLocaleChanged -= this.SelectedLocaleChanged;
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00015652 File Offset: 0x00013852
		private IEnumerator Start()
		{
			this.m_CurrentLocale = null;
			AsyncOperationHandle<Locale> localeOp = LocalizationSettings.SelectedLocaleAsync;
			if (!localeOp.IsDone)
			{
				yield return localeOp;
			}
			this.SelectedLocaleChanged(localeOp.Result);
			yield break;
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x00015661 File Offset: 0x00013861
		private void SelectedLocaleChanged(Locale locale)
		{
			this.m_CurrentLocale = locale;
			if (locale == null)
			{
				return;
			}
			this.ApplyLocaleVariant(locale);
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x0001567C File Offset: 0x0001387C
		public List<TrackedObject> TrackedObjects
		{
			get
			{
				return this.m_TrackedObjects;
			}
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x00015684 File Offset: 0x00013884
		public T GetTrackedObject<T>(Object target, bool create = true) where T : TrackedObject, new()
		{
			TrackedObject trackedObject = this.GetTrackedObject(target);
			if (trackedObject != null)
			{
				return (T)((object)trackedObject);
			}
			if (!create)
			{
				return default(T);
			}
			Component component = target as Component;
			if (component == null)
			{
				return default(T);
			}
			if (component.gameObject != base.gameObject)
			{
				throw new Exception("Tracked Objects must share the same GameObject as the GameObjectLocalizer. " + string.Format("The Component {0} is attached to the GameObject {1}", component, component.gameObject) + string.Format(" but the GameObjectLocalizer is attached to {0}.", base.gameObject));
			}
			T t = new T();
			t.Target = target;
			T t2 = t;
			this.TrackedObjects.Add(t2);
			return t2;
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x00015734 File Offset: 0x00013934
		public TrackedObject GetTrackedObject(Object target)
		{
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			foreach (TrackedObject trackedObject in this.TrackedObjects)
			{
				if (((trackedObject != null) ? trackedObject.Target : null) == target)
				{
					return trackedObject;
				}
			}
			return null;
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x000157B0 File Offset: 0x000139B0
		public AsyncOperationHandle ApplyLocaleVariant(Locale locale)
		{
			return this.ApplyLocaleVariant(locale, LocalizationSettings.ProjectLocale);
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x000157C0 File Offset: 0x000139C0
		public AsyncOperationHandle ApplyLocaleVariant(Locale locale, Locale fallback)
		{
			if (locale == null)
			{
				throw new ArgumentNullException("locale");
			}
			if (this.CurrentOperation.IsValid())
			{
				if (!this.CurrentOperation.IsDone)
				{
					Debug.LogWarning("Attempting to Apply Variant when the previous operation has not yet completed.", this);
				}
				AddressablesInterface.Release(this.CurrentOperation);
				this.CurrentOperation = default(AsyncOperationHandle);
			}
			List<AsyncOperationHandle> list = CollectionPool<List<AsyncOperationHandle>, AsyncOperationHandle>.Get();
			foreach (TrackedObject trackedObject in this.TrackedObjects)
			{
				if (trackedObject != null)
				{
					AsyncOperationHandle asyncOperationHandle = trackedObject.ApplyLocale(locale, fallback);
					if (!asyncOperationHandle.IsDone)
					{
						list.Add(asyncOperationHandle);
					}
				}
			}
			if (list.Count == 1)
			{
				AddressablesInterface.Acquire(list[0]);
				this.CurrentOperation = list[0];
				CollectionPool<List<AsyncOperationHandle>, AsyncOperationHandle>.Release(list);
				return this.CurrentOperation;
			}
			if (list.Count > 1)
			{
				this.CurrentOperation = AddressablesInterface.CreateGroupOperation(list);
				return this.CurrentOperation;
			}
			CollectionPool<List<AsyncOperationHandle>, AsyncOperationHandle>.Release(list);
			return default(AsyncOperationHandle);
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x000158E8 File Offset: 0x00013AE8
		private void RegisterChanges()
		{
			if (this.m_LocalizedStringChanged == null)
			{
				this.m_LocalizedStringChanged = delegate(string _)
				{
					this.RequestUpdate();
				};
			}
			try
			{
				this.m_IgnoreChange = true;
				foreach (TrackedObject trackedObject in this.m_TrackedObjects)
				{
					foreach (ITrackedProperty trackedProperty in trackedObject.TrackedProperties)
					{
						LocalizedStringProperty localizedStringProperty = trackedProperty as LocalizedStringProperty;
						if (localizedStringProperty != null)
						{
							localizedStringProperty.LocalizedString.StringChanged += this.m_LocalizedStringChanged;
						}
					}
				}
			}
			finally
			{
				this.m_IgnoreChange = false;
			}
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x000159B8 File Offset: 0x00013BB8
		private void UnregisterChanges()
		{
			if (this.m_LocalizedStringChanged == null)
			{
				return;
			}
			foreach (TrackedObject trackedObject in this.m_TrackedObjects)
			{
				foreach (ITrackedProperty trackedProperty in trackedObject.TrackedProperties)
				{
					LocalizedStringProperty localizedStringProperty = trackedProperty as LocalizedStringProperty;
					if (localizedStringProperty != null)
					{
						localizedStringProperty.LocalizedString.StringChanged -= this.m_LocalizedStringChanged;
					}
				}
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00015A5C File Offset: 0x00013C5C
		internal void RequestUpdate()
		{
			if (this.m_IgnoreChange || LocalizationSettings.Instance.IsChangingSelectedLocale || (this.CurrentOperation.IsValid() && !this.CurrentOperation.IsDone))
			{
				return;
			}
			if (this.m_CurrentLocale != null)
			{
				this.ApplyLocaleVariant(this.m_CurrentLocale);
			}
		}

		// Token: 0x04000222 RID: 546
		[SerializeReference]
		private List<TrackedObject> m_TrackedObjects = new List<TrackedObject>();

		// Token: 0x04000223 RID: 547
		private Locale m_CurrentLocale;

		// Token: 0x04000224 RID: 548
		private LocalizedString.ChangeHandler m_LocalizedStringChanged;

		// Token: 0x04000225 RID: 549
		private bool m_IgnoreChange;
	}
}
