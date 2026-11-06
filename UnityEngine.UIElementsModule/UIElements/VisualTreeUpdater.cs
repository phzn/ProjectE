using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200041A RID: 1050
	internal sealed class VisualTreeUpdater : IDisposable
	{
		// Token: 0x06002174 RID: 8564 RVA: 0x0007EE21 File Offset: 0x0007D021
		public VisualTreeUpdater(BaseVisualElementPanel panel)
		{
			this.m_Panel = panel;
			this.m_UpdaterArray = new VisualTreeUpdater.UpdaterArray();
			this.SetDefaultUpdaters();
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x0007EE44 File Offset: 0x0007D044
		public void Dispose()
		{
			for (int i = 0; i < 7; i++)
			{
				IVisualTreeUpdater visualTreeUpdater = this.m_UpdaterArray[i];
				visualTreeUpdater.Dispose();
			}
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x0007EE78 File Offset: 0x0007D078
		public void UpdateVisualTree()
		{
			for (int i = 0; i < 7; i++)
			{
				IVisualTreeUpdater visualTreeUpdater = this.m_UpdaterArray[i];
				using (visualTreeUpdater.profilerMarker.Auto())
				{
					visualTreeUpdater.Update();
				}
			}
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x0007EEE0 File Offset: 0x0007D0E0
		public void UpdateVisualTreePhase(VisualTreeUpdatePhase phase)
		{
			IVisualTreeUpdater visualTreeUpdater = this.m_UpdaterArray[phase];
			using (visualTreeUpdater.profilerMarker.Auto())
			{
				visualTreeUpdater.Update();
			}
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x0007EF34 File Offset: 0x0007D134
		public void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
			for (int i = 0; i < 7; i++)
			{
				IVisualTreeUpdater visualTreeUpdater = this.m_UpdaterArray[i];
				visualTreeUpdater.OnVersionChanged(ve, versionChangeType);
			}
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x0007EF6A File Offset: 0x0007D16A
		public void SetUpdater(IVisualTreeUpdater updater, VisualTreeUpdatePhase phase)
		{
			IVisualTreeUpdater visualTreeUpdater = this.m_UpdaterArray[phase];
			if (visualTreeUpdater != null)
			{
				visualTreeUpdater.Dispose();
			}
			updater.panel = this.m_Panel;
			this.m_UpdaterArray[phase] = updater;
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x0007EFA0 File Offset: 0x0007D1A0
		public void SetUpdater<T>(VisualTreeUpdatePhase phase) where T : IVisualTreeUpdater, new()
		{
			IVisualTreeUpdater visualTreeUpdater = this.m_UpdaterArray[phase];
			if (visualTreeUpdater != null)
			{
				visualTreeUpdater.Dispose();
			}
			T t = new T();
			t.panel = this.m_Panel;
			T t2 = t;
			this.m_UpdaterArray[phase] = t2;
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x0007EFF8 File Offset: 0x0007D1F8
		public IVisualTreeUpdater GetUpdater(VisualTreeUpdatePhase phase)
		{
			return this.m_UpdaterArray[phase];
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x0007F016 File Offset: 0x0007D216
		private void SetDefaultUpdaters()
		{
			this.SetUpdater<VisualTreeViewDataUpdater>(VisualTreeUpdatePhase.ViewData);
			this.SetUpdater<VisualTreeBindingsUpdater>(VisualTreeUpdatePhase.Bindings);
			this.SetUpdater<VisualElementAnimationSystem>(VisualTreeUpdatePhase.Animation);
			this.SetUpdater<VisualTreeStyleUpdater>(VisualTreeUpdatePhase.Styles);
			this.SetUpdater<UIRLayoutUpdater>(VisualTreeUpdatePhase.Layout);
			this.SetUpdater<VisualTreeHierarchyFlagsUpdater>(VisualTreeUpdatePhase.TransformClip);
			this.SetUpdater<UIRRepaintUpdater>(VisualTreeUpdatePhase.Repaint);
		}

		// Token: 0x04000E42 RID: 3650
		private BaseVisualElementPanel m_Panel;

		// Token: 0x04000E43 RID: 3651
		private VisualTreeUpdater.UpdaterArray m_UpdaterArray;

		// Token: 0x0200041B RID: 1051
		private class UpdaterArray
		{
			// Token: 0x0600217D RID: 8573 RVA: 0x0007F051 File Offset: 0x0007D251
			public UpdaterArray()
			{
				this.m_VisualTreeUpdaters = new IVisualTreeUpdater[7];
			}

			// Token: 0x170007BB RID: 1979
			public IVisualTreeUpdater this[VisualTreeUpdatePhase phase]
			{
				get
				{
					return this.m_VisualTreeUpdaters[(int)phase];
				}
				set
				{
					this.m_VisualTreeUpdaters[(int)phase] = value;
				}
			}

			// Token: 0x170007BC RID: 1980
			public IVisualTreeUpdater this[int index]
			{
				get
				{
					return this.m_VisualTreeUpdaters[index];
				}
				set
				{
					this.m_VisualTreeUpdaters[index] = value;
				}
			}

			// Token: 0x04000E44 RID: 3652
			private IVisualTreeUpdater[] m_VisualTreeUpdaters;
		}
	}
}
