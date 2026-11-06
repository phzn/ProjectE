using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x0200041E RID: 1054
	internal class VisualTreeViewDataUpdater : BaseVisualTreeUpdater
	{
		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x06002192 RID: 8594 RVA: 0x0007F193 File Offset: 0x0007D393
		public override ProfilerMarker profilerMarker
		{
			get
			{
				return VisualTreeViewDataUpdater.s_ProfilerMarker;
			}
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x0007F19C File Offset: 0x0007D39C
		public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
			bool flag = (versionChangeType & VersionChangeType.ViewData) != VersionChangeType.ViewData;
			if (!flag)
			{
				this.m_Version += 1U;
				this.m_UpdateList.Add(ve);
				this.PropagateToParents(ve);
			}
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x0007F1DC File Offset: 0x0007D3DC
		public override void Update()
		{
			bool flag = this.m_Version == this.m_LastVersion;
			if (!flag)
			{
				int num = 0;
				while (this.m_LastVersion != this.m_Version)
				{
					this.m_LastVersion = this.m_Version;
					this.ValidateViewDataOnSubTree(base.visualTree, true);
					num++;
					bool flag2 = num > 5;
					if (flag2)
					{
						string text = "UIElements: Too many children recursively added that rely on persistent view data: ";
						VisualElement visualTree = base.visualTree;
						Debug.LogError(text + ((visualTree != null) ? visualTree.ToString() : null));
						break;
					}
				}
				this.m_UpdateList.Clear();
				this.m_ParentList.Clear();
			}
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x0007F27C File Offset: 0x0007D47C
		private void ValidateViewDataOnSubTree(VisualElement ve, bool enablePersistence)
		{
			enablePersistence = ve.IsViewDataPersitenceSupportedOnChildren(enablePersistence);
			bool flag = this.m_UpdateList.Contains(ve);
			if (flag)
			{
				this.m_UpdateList.Remove(ve);
				ve.OnViewDataReady(enablePersistence);
			}
			bool flag2 = this.m_ParentList.Contains(ve);
			if (flag2)
			{
				this.m_ParentList.Remove(ve);
				int childCount = ve.hierarchy.childCount;
				for (int i = 0; i < childCount; i++)
				{
					this.ValidateViewDataOnSubTree(ve.hierarchy[i], enablePersistence);
				}
			}
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x0007F318 File Offset: 0x0007D518
		private void PropagateToParents(VisualElement ve)
		{
			for (VisualElement visualElement = ve.hierarchy.parent; visualElement != null; visualElement = visualElement.hierarchy.parent)
			{
				bool flag = !this.m_ParentList.Add(visualElement);
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x04000E47 RID: 3655
		private HashSet<VisualElement> m_UpdateList = new HashSet<VisualElement>();

		// Token: 0x04000E48 RID: 3656
		private HashSet<VisualElement> m_ParentList = new HashSet<VisualElement>();

		// Token: 0x04000E49 RID: 3657
		private const int kMaxValidatePersistentDataCount = 5;

		// Token: 0x04000E4A RID: 3658
		private uint m_Version = 0U;

		// Token: 0x04000E4B RID: 3659
		private uint m_LastVersion = 0U;

		// Token: 0x04000E4C RID: 3660
		private static readonly string s_Description = "Update ViewData";

		// Token: 0x04000E4D RID: 3661
		private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(VisualTreeViewDataUpdater.s_Description);
	}
}
