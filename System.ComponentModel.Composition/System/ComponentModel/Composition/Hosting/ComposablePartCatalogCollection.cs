using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Threading;
using Microsoft.Internal;
using Microsoft.Internal.Collections;

namespace System.ComponentModel.Composition.Hosting
{
	// Token: 0x020000BF RID: 191
	internal class ComposablePartCatalogCollection : ICollection<ComposablePartCatalog>, IEnumerable<ComposablePartCatalog>, IEnumerable, INotifyComposablePartCatalogChanged, IDisposable
	{
		// Token: 0x060004E0 RID: 1248 RVA: 0x0000E3D0 File Offset: 0x0000C5D0
		public ComposablePartCatalogCollection(IEnumerable<ComposablePartCatalog> catalogs, Action<ComposablePartCatalogChangeEventArgs> onChanged, Action<ComposablePartCatalogChangeEventArgs> onChanging)
		{
			catalogs = catalogs ?? Enumerable.Empty<ComposablePartCatalog>();
			this._catalogs = new List<ComposablePartCatalog>(catalogs);
			this._onChanged = onChanged;
			this._onChanging = onChanging;
			this.SubscribeToCatalogNotifications(catalogs);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000E428 File Offset: 0x0000C628
		public void Add(ComposablePartCatalog item)
		{
			Requires.NotNull<ComposablePartCatalog>(item, "item");
			this.ThrowIfDisposed();
			Lazy<IEnumerable<ComposablePartDefinition>> lazy = new Lazy<IEnumerable<ComposablePartDefinition>>(() => item.ToArray<ComposablePartDefinition>(), LazyThreadSafetyMode.PublicationOnly);
			using (AtomicComposition atomicComposition = new AtomicComposition())
			{
				this.RaiseChangingEvent(lazy, null, atomicComposition);
				using (new WriteLock(this._lock))
				{
					if (this._isCopyNeeded)
					{
						this._catalogs = new List<ComposablePartCatalog>(this._catalogs);
						this._isCopyNeeded = false;
					}
					this._hasChanged = true;
					this._catalogs.Add(item);
				}
				this.SubscribeToCatalogNotifications(item);
				atomicComposition.Complete();
			}
			this.RaiseChangedEvent(lazy, null);
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060004E2 RID: 1250 RVA: 0x0000E514 File Offset: 0x0000C714
		// (remove) Token: 0x060004E3 RID: 1251 RVA: 0x0000E54C File Offset: 0x0000C74C
		public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060004E4 RID: 1252 RVA: 0x0000E584 File Offset: 0x0000C784
		// (remove) Token: 0x060004E5 RID: 1253 RVA: 0x0000E5BC File Offset: 0x0000C7BC
		public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing;

		// Token: 0x060004E6 RID: 1254 RVA: 0x0000E5F4 File Offset: 0x0000C7F4
		public void Clear()
		{
			this.ThrowIfDisposed();
			ComposablePartCatalog[] catalogs = null;
			using (new ReadLock(this._lock))
			{
				if (this._catalogs.Count == 0)
				{
					return;
				}
				catalogs = this._catalogs.ToArray();
			}
			Lazy<IEnumerable<ComposablePartDefinition>> lazy = new Lazy<IEnumerable<ComposablePartDefinition>>(() => catalogs.SelectMany<ComposablePartCatalog, ComposablePartDefinition>((ComposablePartCatalog catalog) => catalog).ToArray<ComposablePartDefinition>(), LazyThreadSafetyMode.PublicationOnly);
			using (AtomicComposition atomicComposition = new AtomicComposition())
			{
				this.RaiseChangingEvent(null, lazy, atomicComposition);
				this.UnsubscribeFromCatalogNotifications(catalogs);
				using (new WriteLock(this._lock))
				{
					this._catalogs = new List<ComposablePartCatalog>();
					this._isCopyNeeded = false;
					this._hasChanged = true;
				}
				atomicComposition.Complete();
			}
			this.RaiseChangedEvent(null, lazy);
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0000E6FC File Offset: 0x0000C8FC
		public bool Contains(ComposablePartCatalog item)
		{
			Requires.NotNull<ComposablePartCatalog>(item, "item");
			this.ThrowIfDisposed();
			bool flag;
			using (new ReadLock(this._lock))
			{
				flag = this._catalogs.Contains(item);
			}
			return flag;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0000E758 File Offset: 0x0000C958
		public void CopyTo(ComposablePartCatalog[] array, int arrayIndex)
		{
			this.ThrowIfDisposed();
			using (new ReadLock(this._lock))
			{
				this._catalogs.CopyTo(array, arrayIndex);
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x0000E7A8 File Offset: 0x0000C9A8
		public int Count
		{
			get
			{
				this.ThrowIfDisposed();
				int count;
				using (new ReadLock(this._lock))
				{
					count = this._catalogs.Count;
				}
				return count;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x0000E7F8 File Offset: 0x0000C9F8
		public bool IsReadOnly
		{
			get
			{
				this.ThrowIfDisposed();
				return false;
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0000E804 File Offset: 0x0000CA04
		public bool Remove(ComposablePartCatalog item)
		{
			Requires.NotNull<ComposablePartCatalog>(item, "item");
			this.ThrowIfDisposed();
			using (new ReadLock(this._lock))
			{
				if (!this._catalogs.Contains(item))
				{
					return false;
				}
			}
			bool flag = false;
			Lazy<IEnumerable<ComposablePartDefinition>> lazy = new Lazy<IEnumerable<ComposablePartDefinition>>(() => item.ToArray<ComposablePartDefinition>(), LazyThreadSafetyMode.PublicationOnly);
			using (AtomicComposition atomicComposition = new AtomicComposition())
			{
				this.RaiseChangingEvent(null, lazy, atomicComposition);
				using (new WriteLock(this._lock))
				{
					if (this._isCopyNeeded)
					{
						this._catalogs = new List<ComposablePartCatalog>(this._catalogs);
						this._isCopyNeeded = false;
					}
					flag = this._catalogs.Remove(item);
					if (flag)
					{
						this._hasChanged = true;
					}
				}
				this.UnsubscribeFromCatalogNotifications(item);
				atomicComposition.Complete();
			}
			this.RaiseChangedEvent(null, lazy);
			return flag;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0000E944 File Offset: 0x0000CB44
		internal bool HasChanged
		{
			get
			{
				this.ThrowIfDisposed();
				bool hasChanged;
				using (new ReadLock(this._lock))
				{
					hasChanged = this._hasChanged;
				}
				return hasChanged;
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0000E98C File Offset: 0x0000CB8C
		public IEnumerator<ComposablePartCatalog> GetEnumerator()
		{
			this.ThrowIfDisposed();
			IEnumerator<ComposablePartCatalog> enumerator2;
			using (new WriteLock(this._lock))
			{
				IEnumerator<ComposablePartCatalog> enumerator = this._catalogs.GetEnumerator();
				this._isCopyNeeded = true;
				enumerator2 = enumerator;
			}
			return enumerator2;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0000E9E8 File Offset: 0x0000CBE8
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0000E9F0 File Offset: 0x0000CBF0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0000EA00 File Offset: 0x0000CC00
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this._isDisposed)
			{
				bool flag = false;
				IEnumerable<ComposablePartCatalog> enumerable = null;
				try
				{
					using (new WriteLock(this._lock))
					{
						if (!this._isDisposed)
						{
							flag = true;
							enumerable = this._catalogs;
							this._catalogs = null;
							this._isDisposed = true;
						}
					}
				}
				finally
				{
					if (enumerable != null)
					{
						this.UnsubscribeFromCatalogNotifications(enumerable);
						enumerable.ForEach<ComposablePartCatalog>(delegate(ComposablePartCatalog catalog)
						{
							catalog.Dispose();
						});
					}
					if (flag)
					{
						this._lock.Dispose();
					}
				}
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0000EAC0 File Offset: 0x0000CCC0
		private void RaiseChangedEvent(Lazy<IEnumerable<ComposablePartDefinition>> addedDefinitions, Lazy<IEnumerable<ComposablePartDefinition>> removedDefinitions)
		{
			if (this._onChanged == null || this.Changed == null)
			{
				return;
			}
			IEnumerable<ComposablePartDefinition> enumerable = ((addedDefinitions == null) ? Enumerable.Empty<ComposablePartDefinition>() : addedDefinitions.Value);
			IEnumerable<ComposablePartDefinition> enumerable2 = ((removedDefinitions == null) ? Enumerable.Empty<ComposablePartDefinition>() : removedDefinitions.Value);
			this._onChanged(new ComposablePartCatalogChangeEventArgs(enumerable, enumerable2, null));
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0000EB14 File Offset: 0x0000CD14
		public void OnChanged(object sender, ComposablePartCatalogChangeEventArgs e)
		{
			EventHandler<ComposablePartCatalogChangeEventArgs> changed = this.Changed;
			if (changed != null)
			{
				changed(sender, e);
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0000EB34 File Offset: 0x0000CD34
		private void RaiseChangingEvent(Lazy<IEnumerable<ComposablePartDefinition>> addedDefinitions, Lazy<IEnumerable<ComposablePartDefinition>> removedDefinitions, AtomicComposition atomicComposition)
		{
			if (this._onChanging == null || this.Changing == null)
			{
				return;
			}
			IEnumerable<ComposablePartDefinition> enumerable = ((addedDefinitions == null) ? Enumerable.Empty<ComposablePartDefinition>() : addedDefinitions.Value);
			IEnumerable<ComposablePartDefinition> enumerable2 = ((removedDefinitions == null) ? Enumerable.Empty<ComposablePartDefinition>() : removedDefinitions.Value);
			this._onChanging(new ComposablePartCatalogChangeEventArgs(enumerable, enumerable2, atomicComposition));
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0000EB88 File Offset: 0x0000CD88
		public void OnChanging(object sender, ComposablePartCatalogChangeEventArgs e)
		{
			EventHandler<ComposablePartCatalogChangeEventArgs> changing = this.Changing;
			if (changing != null)
			{
				changing(sender, e);
			}
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0000EBA7 File Offset: 0x0000CDA7
		private void OnContainedCatalogChanged(object sender, ComposablePartCatalogChangeEventArgs e)
		{
			if (this._onChanged == null || this.Changed == null)
			{
				return;
			}
			this._onChanged(e);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0000EBC6 File Offset: 0x0000CDC6
		private void OnContainedCatalogChanging(object sender, ComposablePartCatalogChangeEventArgs e)
		{
			if (this._onChanging == null || this.Changing == null)
			{
				return;
			}
			this._onChanging(e);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0000EBE8 File Offset: 0x0000CDE8
		private void SubscribeToCatalogNotifications(ComposablePartCatalog catalog)
		{
			INotifyComposablePartCatalogChanged notifyComposablePartCatalogChanged = catalog as INotifyComposablePartCatalogChanged;
			if (notifyComposablePartCatalogChanged != null)
			{
				notifyComposablePartCatalogChanged.Changed += this.OnContainedCatalogChanged;
				notifyComposablePartCatalogChanged.Changing += this.OnContainedCatalogChanging;
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0000EC24 File Offset: 0x0000CE24
		private void SubscribeToCatalogNotifications(IEnumerable<ComposablePartCatalog> catalogs)
		{
			foreach (ComposablePartCatalog composablePartCatalog in catalogs)
			{
				this.SubscribeToCatalogNotifications(composablePartCatalog);
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0000EC6C File Offset: 0x0000CE6C
		private void UnsubscribeFromCatalogNotifications(ComposablePartCatalog catalog)
		{
			INotifyComposablePartCatalogChanged notifyComposablePartCatalogChanged = catalog as INotifyComposablePartCatalogChanged;
			if (notifyComposablePartCatalogChanged != null)
			{
				notifyComposablePartCatalogChanged.Changed -= this.OnContainedCatalogChanged;
				notifyComposablePartCatalogChanged.Changing -= this.OnContainedCatalogChanging;
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0000ECA8 File Offset: 0x0000CEA8
		private void UnsubscribeFromCatalogNotifications(IEnumerable<ComposablePartCatalog> catalogs)
		{
			foreach (ComposablePartCatalog composablePartCatalog in catalogs)
			{
				this.UnsubscribeFromCatalogNotifications(composablePartCatalog);
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0000ECF0 File Offset: 0x0000CEF0
		private void ThrowIfDisposed()
		{
			if (this._isDisposed)
			{
				throw ExceptionBuilder.CreateObjectDisposed(this);
			}
		}

		// Token: 0x04000202 RID: 514
		private readonly Microsoft.Internal.Lock _lock = new Microsoft.Internal.Lock();

		// Token: 0x04000203 RID: 515
		private Action<ComposablePartCatalogChangeEventArgs> _onChanged;

		// Token: 0x04000204 RID: 516
		private Action<ComposablePartCatalogChangeEventArgs> _onChanging;

		// Token: 0x04000205 RID: 517
		private List<ComposablePartCatalog> _catalogs = new List<ComposablePartCatalog>();

		// Token: 0x04000206 RID: 518
		private volatile bool _isCopyNeeded;

		// Token: 0x04000207 RID: 519
		private volatile bool _isDisposed;

		// Token: 0x04000208 RID: 520
		private bool _hasChanged;
	}
}
