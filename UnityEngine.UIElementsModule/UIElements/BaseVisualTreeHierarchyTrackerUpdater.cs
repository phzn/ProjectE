using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000412 RID: 1042
	internal abstract class BaseVisualTreeHierarchyTrackerUpdater : BaseVisualTreeUpdater
	{
		// Token: 0x06002141 RID: 8513
		protected abstract void OnHierarchyChange(VisualElement ve, HierarchyChangeType type);

		// Token: 0x06002142 RID: 8514 RVA: 0x0007DD38 File Offset: 0x0007BF38
		public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
			bool flag = (versionChangeType & VersionChangeType.Hierarchy) == VersionChangeType.Hierarchy;
			if (flag)
			{
				switch (this.m_State)
				{
				case BaseVisualTreeHierarchyTrackerUpdater.State.Waiting:
					this.ProcessNewChange(ve);
					break;
				case BaseVisualTreeHierarchyTrackerUpdater.State.TrackingAddOrMove:
					this.ProcessAddOrMove(ve);
					break;
				case BaseVisualTreeHierarchyTrackerUpdater.State.TrackingRemove:
					this.ProcessRemove(ve);
					break;
				}
			}
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x0007DD90 File Offset: 0x0007BF90
		public override void Update()
		{
			Debug.Assert(this.m_State == BaseVisualTreeHierarchyTrackerUpdater.State.TrackingAddOrMove || this.m_State == BaseVisualTreeHierarchyTrackerUpdater.State.Waiting);
			bool flag = this.m_State == BaseVisualTreeHierarchyTrackerUpdater.State.TrackingAddOrMove;
			if (flag)
			{
				this.OnHierarchyChange(this.m_CurrentChangeElement, HierarchyChangeType.Move);
				this.m_State = BaseVisualTreeHierarchyTrackerUpdater.State.Waiting;
			}
			this.m_CurrentChangeElement = null;
			this.m_CurrentChangeParent = null;
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x0007DDEC File Offset: 0x0007BFEC
		private void ProcessNewChange(VisualElement ve)
		{
			this.m_CurrentChangeElement = ve;
			this.m_CurrentChangeParent = ve.parent;
			bool flag = this.m_CurrentChangeParent == null && ve.panel != null;
			if (flag)
			{
				this.OnHierarchyChange(this.m_CurrentChangeElement, HierarchyChangeType.Move);
				this.m_State = BaseVisualTreeHierarchyTrackerUpdater.State.Waiting;
			}
			else
			{
				this.m_State = ((this.m_CurrentChangeParent == null) ? BaseVisualTreeHierarchyTrackerUpdater.State.TrackingRemove : BaseVisualTreeHierarchyTrackerUpdater.State.TrackingAddOrMove);
			}
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x0007DE54 File Offset: 0x0007C054
		private void ProcessAddOrMove(VisualElement ve)
		{
			Debug.Assert(this.m_CurrentChangeParent != null);
			bool flag = this.m_CurrentChangeParent == ve;
			if (flag)
			{
				this.OnHierarchyChange(this.m_CurrentChangeElement, HierarchyChangeType.Add);
				this.m_State = BaseVisualTreeHierarchyTrackerUpdater.State.Waiting;
			}
			else
			{
				this.OnHierarchyChange(this.m_CurrentChangeElement, HierarchyChangeType.Move);
				this.ProcessNewChange(ve);
			}
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x0007DEB0 File Offset: 0x0007C0B0
		private void ProcessRemove(VisualElement ve)
		{
			this.OnHierarchyChange(this.m_CurrentChangeElement, HierarchyChangeType.Remove);
			bool flag = ve.panel != null;
			if (flag)
			{
				this.m_CurrentChangeParent = null;
				this.m_CurrentChangeElement = null;
				this.m_State = BaseVisualTreeHierarchyTrackerUpdater.State.Waiting;
			}
			else
			{
				this.m_CurrentChangeElement = ve;
			}
		}

		// Token: 0x04000E17 RID: 3607
		private BaseVisualTreeHierarchyTrackerUpdater.State m_State = BaseVisualTreeHierarchyTrackerUpdater.State.Waiting;

		// Token: 0x04000E18 RID: 3608
		private VisualElement m_CurrentChangeElement;

		// Token: 0x04000E19 RID: 3609
		private VisualElement m_CurrentChangeParent;

		// Token: 0x02000413 RID: 1043
		private enum State
		{
			// Token: 0x04000E1B RID: 3611
			Waiting,
			// Token: 0x04000E1C RID: 3612
			TrackingAddOrMove,
			// Token: 0x04000E1D RID: 3613
			TrackingRemove
		}
	}
}
