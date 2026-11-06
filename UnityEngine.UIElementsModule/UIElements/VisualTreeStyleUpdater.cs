using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x02000415 RID: 1045
	internal class VisualTreeStyleUpdater : BaseVisualTreeUpdater
	{
		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06002150 RID: 8528 RVA: 0x0007E040 File Offset: 0x0007C240
		// (set) Token: 0x06002151 RID: 8529 RVA: 0x0007E048 File Offset: 0x0007C248
		public VisualTreeStyleUpdaterTraversal traversal
		{
			get
			{
				return this.m_StyleContextHierarchyTraversal;
			}
			set
			{
				this.m_StyleContextHierarchyTraversal = value;
				BaseVisualElementPanel panel = base.panel;
				if (panel != null)
				{
					panel.visualTree.IncrementVersion(VersionChangeType.Layout | VersionChangeType.StyleSheet | VersionChangeType.Styles | VersionChangeType.Transform);
				}
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06002152 RID: 8530 RVA: 0x0007E06E File Offset: 0x0007C26E
		public override ProfilerMarker profilerMarker
		{
			get
			{
				return VisualTreeStyleUpdater.s_ProfilerMarker;
			}
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x0007E078 File Offset: 0x0007C278
		public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
			bool flag = (versionChangeType & (VersionChangeType.StyleSheet | VersionChangeType.TransitionProperty)) == (VersionChangeType)0;
			if (!flag)
			{
				this.m_Version += 1U;
				bool flag2 = (versionChangeType & VersionChangeType.StyleSheet) > (VersionChangeType)0;
				if (flag2)
				{
					bool isApplyingStyles = this.m_IsApplyingStyles;
					if (isApplyingStyles)
					{
						this.m_ApplyStyleUpdateList.Add(ve);
					}
					else
					{
						this.m_StyleContextHierarchyTraversal.AddChangedElement(ve, versionChangeType);
					}
				}
				bool flag3 = (versionChangeType & VersionChangeType.TransitionProperty) > (VersionChangeType)0;
				if (flag3)
				{
					this.m_TransitionPropertyUpdateList.Add(ve);
				}
			}
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x0007E0FC File Offset: 0x0007C2FC
		public override void Update()
		{
			bool flag = this.m_Version == this.m_LastVersion;
			if (!flag)
			{
				this.m_LastVersion = this.m_Version;
				this.ApplyStyles();
				this.m_StyleContextHierarchyTraversal.Clear();
				foreach (VisualElement visualElement in this.m_ApplyStyleUpdateList)
				{
					this.m_StyleContextHierarchyTraversal.AddChangedElement(visualElement, VersionChangeType.StyleSheet);
				}
				this.m_ApplyStyleUpdateList.Clear();
				foreach (VisualElement visualElement2 in this.m_TransitionPropertyUpdateList)
				{
					bool flag2 = visualElement2.hasRunningAnimations || visualElement2.hasCompletedAnimations;
					if (flag2)
					{
						ComputedTransitionUtils.UpdateComputedTransitions(visualElement2.computedStyle);
						this.m_StyleContextHierarchyTraversal.CancelAnimationsWithNoTransitionProperty(visualElement2, visualElement2.computedStyle);
					}
				}
				this.m_TransitionPropertyUpdateList.Clear();
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x06002155 RID: 8533 RVA: 0x0007E228 File Offset: 0x0007C428
		// (set) Token: 0x06002156 RID: 8534 RVA: 0x0007E230 File Offset: 0x0007C430
		private protected bool disposed { protected get; private set; }

		// Token: 0x06002157 RID: 8535 RVA: 0x0007E23C File Offset: 0x0007C43C
		protected override void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			if (!disposed)
			{
				if (disposing)
				{
					this.m_StyleContextHierarchyTraversal.Clear();
				}
				this.disposed = true;
			}
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x0007E270 File Offset: 0x0007C470
		private void ApplyStyles()
		{
			Debug.Assert(base.visualTree.panel != null);
			this.m_IsApplyingStyles = true;
			this.m_StyleContextHierarchyTraversal.PrepareTraversal(base.panel.scaledPixelsPerPoint);
			this.m_StyleContextHierarchyTraversal.Traverse(base.visualTree);
			this.m_IsApplyingStyles = false;
		}

		// Token: 0x04000E21 RID: 3617
		private HashSet<VisualElement> m_ApplyStyleUpdateList = new HashSet<VisualElement>();

		// Token: 0x04000E22 RID: 3618
		private HashSet<VisualElement> m_TransitionPropertyUpdateList = new HashSet<VisualElement>();

		// Token: 0x04000E23 RID: 3619
		private bool m_IsApplyingStyles = false;

		// Token: 0x04000E24 RID: 3620
		private uint m_Version = 0U;

		// Token: 0x04000E25 RID: 3621
		private uint m_LastVersion = 0U;

		// Token: 0x04000E26 RID: 3622
		private VisualTreeStyleUpdaterTraversal m_StyleContextHierarchyTraversal = new VisualTreeStyleUpdaterTraversal();

		// Token: 0x04000E27 RID: 3623
		private static readonly string s_Description = "Update Style";

		// Token: 0x04000E28 RID: 3624
		private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(VisualTreeStyleUpdater.s_Description);
	}
}
