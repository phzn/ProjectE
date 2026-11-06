using System;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine.Yoga;

namespace UnityEngine.UIElements
{
	// Token: 0x020002A9 RID: 681
	internal class UIRLayoutUpdater : BaseVisualTreeUpdater
	{
		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x0600139F RID: 5023 RVA: 0x00044AEA File Offset: 0x00042CEA
		public override ProfilerMarker profilerMarker
		{
			get
			{
				return UIRLayoutUpdater.s_ProfilerMarker;
			}
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00044AF4 File Offset: 0x00042CF4
		public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
			bool flag = (versionChangeType & (VersionChangeType.Hierarchy | VersionChangeType.Layout)) == (VersionChangeType)0;
			if (!flag)
			{
				YogaNode yogaNode = ve.yogaNode;
				bool flag2 = yogaNode != null && yogaNode.IsMeasureDefined;
				if (flag2)
				{
					yogaNode.MarkDirty();
				}
			}
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x00044B30 File Offset: 0x00042D30
		public override void Update()
		{
			int num = 0;
			while (base.visualTree.yogaNode.IsDirty)
			{
				this.changeEventsList.Clear();
				bool flag = num > 0;
				if (flag)
				{
					base.panel.ApplyStyles();
				}
				base.panel.duringLayoutPhase = true;
				base.visualTree.yogaNode.CalculateLayout(float.NaN, float.NaN);
				base.panel.duringLayoutPhase = false;
				this.UpdateSubTree(base.visualTree, true, this.changeEventsList);
				this.DispatchChangeEvents(this.changeEventsList, num);
				bool flag2 = num++ >= 10;
				if (flag2)
				{
					string text = "Layout update is struggling to process current layout (consider simplifying to avoid recursive layout): ";
					VisualElement visualTree = base.visualTree;
					Debug.LogError(text + ((visualTree != null) ? visualTree.ToString() : null));
					break;
				}
			}
			base.visualTree.focusController.ReevaluateFocus();
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x00044C1C File Offset: 0x00042E1C
		private void UpdateSubTree(VisualElement ve, bool isDisplayed, List<KeyValuePair<Rect, VisualElement>> changeEvents)
		{
			Rect rect = new Rect(ve.yogaNode.LayoutX, ve.yogaNode.LayoutY, ve.yogaNode.LayoutWidth, ve.yogaNode.LayoutHeight);
			Rect rect2 = new Rect(ve.yogaNode.LayoutPaddingLeft, ve.yogaNode.LayoutPaddingLeft, ve.yogaNode.LayoutPaddingRight, ve.yogaNode.LayoutPaddingBottom);
			Rect rect3 = new Rect(rect2.x, rect2.y, rect.width - (rect2.x + rect2.width), rect.height - (rect2.y + rect2.height));
			Rect lastLayout = ve.lastLayout;
			Rect lastPseudoPadding = ve.lastPseudoPadding;
			bool isHierarchyDisplayed = ve.isHierarchyDisplayed;
			VersionChangeType versionChangeType = (VersionChangeType)0;
			bool flag = lastLayout.size != rect.size;
			bool flag2 = lastPseudoPadding.size != rect3.size;
			bool flag3 = flag || flag2;
			if (flag3)
			{
				versionChangeType |= VersionChangeType.Size | VersionChangeType.Repaint;
			}
			bool flag4 = rect.position != lastLayout.position;
			bool flag5 = rect3.position != lastPseudoPadding.position;
			bool flag6 = flag4 || flag5;
			if (flag6)
			{
				versionChangeType |= VersionChangeType.Transform;
			}
			bool flag7 = (versionChangeType & VersionChangeType.Size) != (VersionChangeType)0 && (versionChangeType & VersionChangeType.Transform) == (VersionChangeType)0;
			if (flag7)
			{
				bool flag8 = !ve.hasDefaultRotationAndScale;
				if (flag8)
				{
					bool flag9 = !Mathf.Approximately(ve.resolvedStyle.transformOrigin.x, 0f) || !Mathf.Approximately(ve.resolvedStyle.transformOrigin.y, 0f);
					if (flag9)
					{
						versionChangeType |= VersionChangeType.Transform;
					}
				}
			}
			isDisplayed &= ve.resolvedStyle.display != DisplayStyle.None;
			ve.isHierarchyDisplayed = isDisplayed;
			bool flag10 = versionChangeType > (VersionChangeType)0;
			if (flag10)
			{
				ve.IncrementVersion(versionChangeType);
			}
			ve.lastLayout = rect;
			ve.lastPseudoPadding = rect3;
			bool hasNewLayout = ve.yogaNode.HasNewLayout;
			bool flag11 = hasNewLayout;
			if (flag11)
			{
				int childCount = ve.hierarchy.childCount;
				for (int i = 0; i < childCount; i++)
				{
					VisualElement visualElement = ve.hierarchy[i];
					bool hasNewLayout2 = visualElement.yogaNode.HasNewLayout;
					if (hasNewLayout2)
					{
						this.UpdateSubTree(visualElement, isDisplayed, changeEvents);
					}
				}
			}
			bool flag12 = (flag || flag4) && ve.HasEventCallbacksOrDefaultActions(EventBase<GeometryChangedEvent>.EventCategory);
			if (flag12)
			{
				changeEvents.Add(new KeyValuePair<Rect, VisualElement>(lastLayout, ve));
			}
			bool flag13 = hasNewLayout;
			if (flag13)
			{
				ve.yogaNode.MarkLayoutSeen();
			}
		}

		// Token: 0x060013A3 RID: 5027 RVA: 0x00044EDC File Offset: 0x000430DC
		private void DispatchChangeEvents(List<KeyValuePair<Rect, VisualElement>> changeEvents, int currentLayoutPass)
		{
			foreach (KeyValuePair<Rect, VisualElement> keyValuePair in changeEvents)
			{
				VisualElement value = keyValuePair.Value;
				using (GeometryChangedEvent pooled = GeometryChangedEvent.GetPooled(keyValuePair.Key, value.lastLayout))
				{
					pooled.layoutPass = currentLayoutPass;
					pooled.target = value;
					value.HandleEventAtTargetAndDefaultPhase(pooled);
				}
			}
		}

		// Token: 0x040008F4 RID: 2292
		private const int kMaxValidateLayoutCount = 10;

		// Token: 0x040008F5 RID: 2293
		private static readonly string s_Description = "Update Layout";

		// Token: 0x040008F6 RID: 2294
		private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(UIRLayoutUpdater.s_Description);

		// Token: 0x040008F7 RID: 2295
		private List<KeyValuePair<Rect, VisualElement>> changeEventsList = new List<KeyValuePair<Rect, VisualElement>>();
	}
}
