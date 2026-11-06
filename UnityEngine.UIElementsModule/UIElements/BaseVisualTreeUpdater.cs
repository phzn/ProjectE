using System;
using System.Diagnostics;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x0200041D RID: 1053
	internal abstract class BaseVisualTreeUpdater : IVisualTreeUpdater, IDisposable
	{
		// Token: 0x14000041 RID: 65
		// (add) Token: 0x06002187 RID: 8583 RVA: 0x0007F0AC File Offset: 0x0007D2AC
		// (remove) Token: 0x06002188 RID: 8584 RVA: 0x0007F0E4 File Offset: 0x0007D2E4
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<BaseVisualElementPanel> panelChanged;

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06002189 RID: 8585 RVA: 0x0007F11C File Offset: 0x0007D31C
		// (set) Token: 0x0600218A RID: 8586 RVA: 0x0007F134 File Offset: 0x0007D334
		public BaseVisualElementPanel panel
		{
			get
			{
				return this.m_Panel;
			}
			set
			{
				this.m_Panel = value;
				bool flag = this.panelChanged != null;
				if (flag)
				{
					this.panelChanged(value);
				}
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x0007F164 File Offset: 0x0007D364
		public VisualElement visualTree
		{
			get
			{
				return this.panel.visualTree;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x0600218C RID: 8588
		public abstract ProfilerMarker profilerMarker { get; }

		// Token: 0x0600218D RID: 8589 RVA: 0x0007F181 File Offset: 0x0007D381
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x00003CD2 File Offset: 0x00001ED2
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x0600218F RID: 8591
		public abstract void Update();

		// Token: 0x06002190 RID: 8592
		public abstract void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType);

		// Token: 0x04000E46 RID: 3654
		private BaseVisualElementPanel m_Panel;
	}
}
