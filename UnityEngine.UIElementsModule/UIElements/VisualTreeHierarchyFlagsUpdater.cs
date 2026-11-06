using System;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x02000410 RID: 1040
	internal class VisualTreeHierarchyFlagsUpdater : BaseVisualTreeUpdater
	{
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x0600213A RID: 8506 RVA: 0x0007DB49 File Offset: 0x0007BD49
		public override ProfilerMarker profilerMarker
		{
			get
			{
				return VisualTreeHierarchyFlagsUpdater.s_ProfilerMarker;
			}
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x0007DB50 File Offset: 0x0007BD50
		public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
			bool flag = (versionChangeType & (VersionChangeType.Hierarchy | VersionChangeType.Overflow | VersionChangeType.BorderWidth | VersionChangeType.Transform | VersionChangeType.Size | VersionChangeType.EventCallbackCategories | VersionChangeType.Picking)) == (VersionChangeType)0;
			if (!flag)
			{
				bool flag2 = (versionChangeType & VersionChangeType.Transform) > (VersionChangeType)0;
				bool flag3 = (versionChangeType & (VersionChangeType.Overflow | VersionChangeType.BorderWidth | VersionChangeType.Transform | VersionChangeType.Size)) > (VersionChangeType)0;
				bool flag4 = (versionChangeType & (VersionChangeType.Hierarchy | VersionChangeType.EventCallbackCategories)) > (VersionChangeType)0;
				VisualElementFlags visualElementFlags = (flag2 ? (VisualElementFlags.WorldTransformDirty | VisualElementFlags.WorldBoundingBoxDirty) : ((VisualElementFlags)0)) | (flag3 ? VisualElementFlags.WorldClipDirty : ((VisualElementFlags)0)) | (flag4 ? VisualElementFlags.EventCallbackParentCategoriesDirty : ((VisualElementFlags)0));
				VisualElementFlags visualElementFlags2 = visualElementFlags & ~ve.m_Flags;
				bool flag5 = visualElementFlags2 > (VisualElementFlags)0;
				if (flag5)
				{
					VisualTreeHierarchyFlagsUpdater.DirtyHierarchy(ve, visualElementFlags2);
				}
				VisualTreeHierarchyFlagsUpdater.DirtyBoundingBoxHierarchy(ve);
				this.m_Version += 1U;
			}
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x0007DBE4 File Offset: 0x0007BDE4
		private static void DirtyHierarchy(VisualElement ve, VisualElementFlags mustDirtyFlags)
		{
			ve.m_Flags |= mustDirtyFlags;
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				VisualElement visualElement = ve.hierarchy[i];
				VisualElementFlags visualElementFlags = mustDirtyFlags & ~visualElement.m_Flags;
				bool flag = visualElementFlags > (VisualElementFlags)0;
				if (flag)
				{
					VisualTreeHierarchyFlagsUpdater.DirtyHierarchy(visualElement, visualElementFlags);
				}
			}
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x0007DC54 File Offset: 0x0007BE54
		private static void DirtyBoundingBoxHierarchy(VisualElement ve)
		{
			ve.isBoundingBoxDirty = true;
			ve.isWorldBoundingBoxDirty = true;
			VisualElement visualElement = ve.hierarchy.parent;
			while (visualElement != null && !visualElement.isBoundingBoxDirty)
			{
				visualElement.isBoundingBoxDirty = true;
				visualElement.isWorldBoundingBoxDirty = true;
				visualElement = visualElement.hierarchy.parent;
			}
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x0007DCB8 File Offset: 0x0007BEB8
		public override void Update()
		{
			bool flag = this.m_Version == this.m_LastVersion;
			if (!flag)
			{
				this.m_LastVersion = this.m_Version;
				base.panel.UpdateElementUnderPointers();
				base.panel.visualTree.UpdateBoundingBox();
			}
		}

		// Token: 0x04000E0F RID: 3599
		private uint m_Version = 0U;

		// Token: 0x04000E10 RID: 3600
		private uint m_LastVersion = 0U;

		// Token: 0x04000E11 RID: 3601
		private static readonly string s_Description = "Update Hierarchy Flags";

		// Token: 0x04000E12 RID: 3602
		private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(VisualTreeHierarchyFlagsUpdater.s_Description);
	}
}
