using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x0200002F RID: 47
	internal class VisualTreeBindingsUpdater : BaseVisualTreeHierarchyTrackerUpdater
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00004D74 File Offset: 0x00002F74
		public override ProfilerMarker profilerMarker
		{
			get
			{
				return VisualTreeBindingsUpdater.s_ProfilerMarker;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x00004D7B File Offset: 0x00002F7B
		// (set) Token: 0x060001BA RID: 442 RVA: 0x00004D82 File Offset: 0x00002F82
		public static bool disableBindingsThrottling { get; set; } = false;

		// Token: 0x060001BB RID: 443 RVA: 0x00004D8C File Offset: 0x00002F8C
		private IBinding GetBindingObjectFromElement(VisualElement ve)
		{
			IBindable bindable = ve as IBindable;
			bool flag = bindable != null;
			if (flag)
			{
				bool flag2 = bindable.binding != null;
				if (flag2)
				{
					return bindable.binding;
				}
			}
			return VisualTreeBindingsUpdater.GetAdditionalBinding(ve);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00004DCB File Offset: 0x00002FCB
		private void StartTracking(VisualElement ve)
		{
			this.m_ElementsToAdd.Add(ve);
			this.m_ElementsToRemove.Remove(ve);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00004DE8 File Offset: 0x00002FE8
		private void StopTracking(VisualElement ve)
		{
			this.m_ElementsToRemove.Add(ve);
			this.m_ElementsToAdd.Remove(ve);
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00004E05 File Offset: 0x00003005
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00004E0D File Offset: 0x0000300D
		public Dictionary<object, object> temporaryObjectCache { get; private set; } = new Dictionary<object, object>();

		// Token: 0x060001C0 RID: 448 RVA: 0x00004E18 File Offset: 0x00003018
		public static void SetAdditionalBinding(VisualElement ve, IBinding b)
		{
			IBinding additionalBinding = VisualTreeBindingsUpdater.GetAdditionalBinding(ve);
			if (additionalBinding != null)
			{
				additionalBinding.Release();
			}
			ve.SetProperty(VisualTreeBindingsUpdater.s_AdditionalBindingObjectVEPropertyName, b);
			ve.IncrementVersion(VersionChangeType.Bindings);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00004E4E File Offset: 0x0000304E
		public static void ClearAdditionalBinding(VisualElement ve)
		{
			VisualTreeBindingsUpdater.SetAdditionalBinding(ve, null);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00004E5C File Offset: 0x0000305C
		public static IBinding GetAdditionalBinding(VisualElement ve)
		{
			return ve.GetProperty(VisualTreeBindingsUpdater.s_AdditionalBindingObjectVEPropertyName) as IBinding;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00004E80 File Offset: 0x00003080
		public static void AddBindingRequest(VisualElement ve, IBindingRequest req)
		{
			List<IBindingRequest> list = ve.GetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName) as List<IBindingRequest>;
			bool flag = list == null;
			if (flag)
			{
				list = ObjectListPool<IBindingRequest>.Get();
				ve.SetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName, list);
			}
			list.Add(req);
			ve.IncrementVersion(VersionChangeType.Bindings);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00004ECC File Offset: 0x000030CC
		public static void RemoveBindingRequest(VisualElement ve, IBindingRequest req)
		{
			List<IBindingRequest> list = ve.GetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName) as List<IBindingRequest>;
			bool flag = list != null;
			if (flag)
			{
				req.Release();
				list.Remove(req);
				bool flag2 = list.Count == 0;
				if (flag2)
				{
					ObjectListPool<IBindingRequest>.Release(list);
					ve.SetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName, null);
				}
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00004F28 File Offset: 0x00003128
		public static void ClearBindingRequests(VisualElement ve)
		{
			List<IBindingRequest> list = ve.GetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName) as List<IBindingRequest>;
			bool flag = list != null;
			if (flag)
			{
				foreach (IBindingRequest bindingRequest in list)
				{
					bindingRequest.Release();
				}
				ObjectListPool<IBindingRequest>.Release(list);
				ve.SetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName, null);
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00004FAC File Offset: 0x000031AC
		private void StartTrackingRecursive(VisualElement ve)
		{
			IBinding bindingObjectFromElement = this.GetBindingObjectFromElement(ve);
			bool flag = bindingObjectFromElement != null;
			if (flag)
			{
				this.StartTracking(ve);
			}
			object property = ve.GetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName);
			bool flag2 = property != null;
			if (flag2)
			{
				this.m_ElementsToBind.Add(ve);
			}
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				VisualElement visualElement = ve.hierarchy[i];
				this.StartTrackingRecursive(visualElement);
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00005040 File Offset: 0x00003240
		private void StopTrackingRecursive(VisualElement ve)
		{
			this.StopTracking(ve);
			object property = ve.GetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName);
			bool flag = property != null;
			if (flag)
			{
				this.m_ElementsToBind.Remove(ve);
			}
			int childCount = ve.hierarchy.childCount;
			for (int i = 0; i < childCount; i++)
			{
				VisualElement visualElement = ve.hierarchy[i];
				this.StopTrackingRecursive(visualElement);
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000050BC File Offset: 0x000032BC
		public override void OnVersionChanged(VisualElement ve, VersionChangeType versionChangeType)
		{
			base.OnVersionChanged(ve, versionChangeType);
			bool flag = (versionChangeType & VersionChangeType.Bindings) == VersionChangeType.Bindings;
			if (flag)
			{
				bool flag2 = this.GetBindingObjectFromElement(ve) != null;
				if (flag2)
				{
					this.StartTracking(ve);
				}
				else
				{
					this.StopTracking(ve);
				}
				object property = ve.GetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName);
				bool flag3 = property != null;
				if (flag3)
				{
					this.m_ElementsToBind.Add(ve);
				}
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00005128 File Offset: 0x00003328
		protected override void OnHierarchyChange(VisualElement ve, HierarchyChangeType type)
		{
			if (type != HierarchyChangeType.Add)
			{
				if (type == HierarchyChangeType.Remove)
				{
					this.StopTrackingRecursive(ve);
				}
			}
			else
			{
				this.StartTrackingRecursive(ve);
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000515C File Offset: 0x0000335C
		private static long CurrentTime()
		{
			return Panel.TimeSinceStartupMs();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00005174 File Offset: 0x00003374
		public static bool ShouldThrottle(long startTime)
		{
			return !VisualTreeBindingsUpdater.disableBindingsThrottling && VisualTreeBindingsUpdater.CurrentTime() - startTime < 100L;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000519C File Offset: 0x0000339C
		public void PerformTrackingOperations()
		{
			foreach (VisualElement visualElement in this.m_ElementsToAdd)
			{
				IBinding bindingObjectFromElement = this.GetBindingObjectFromElement(visualElement);
				bool flag = bindingObjectFromElement != null;
				if (flag)
				{
					this.m_ElementsWithBindings.Add(visualElement);
				}
			}
			this.m_ElementsToAdd.Clear();
			foreach (VisualElement visualElement2 in this.m_ElementsToRemove)
			{
				this.m_ElementsWithBindings.Remove(visualElement2);
			}
			this.m_ElementsToRemove.Clear();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00005274 File Offset: 0x00003474
		public override void Update()
		{
			base.Update();
			bool flag = this.m_ElementsToBind.Count > 0;
			if (flag)
			{
				using (VisualTreeBindingsUpdater.s_ProfilerBindingRequestsMarker.Auto())
				{
					long num = VisualTreeBindingsUpdater.CurrentTime();
					while (this.m_ElementsToBind.Count > 0 && VisualTreeBindingsUpdater.CurrentTime() - num < 100L)
					{
						VisualElement visualElement = this.m_ElementsToBind.FirstOrDefault<VisualElement>();
						bool flag2 = visualElement != null;
						if (!flag2)
						{
							break;
						}
						this.m_ElementsToBind.Remove(visualElement);
						List<IBindingRequest> list = visualElement.GetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName) as List<IBindingRequest>;
						bool flag3 = list != null;
						if (flag3)
						{
							visualElement.SetProperty(VisualTreeBindingsUpdater.s_BindingRequestObjectVEPropertyName, null);
							foreach (IBindingRequest bindingRequest in list)
							{
								bindingRequest.Bind(visualElement);
							}
							ObjectListPool<IBindingRequest>.Release(list);
						}
					}
				}
			}
			this.PerformTrackingOperations();
			bool flag4 = this.m_ElementsWithBindings.Count > 0;
			if (flag4)
			{
				long num2 = VisualTreeBindingsUpdater.CurrentTime();
				bool flag5 = VisualTreeBindingsUpdater.disableBindingsThrottling || this.m_LastUpdateTime + 100L < num2;
				if (flag5)
				{
					this.UpdateBindings();
					this.m_LastUpdateTime = num2;
				}
			}
			bool flag6 = this.m_ElementsToBind.Count == 0;
			if (flag6)
			{
				this.temporaryObjectCache.Clear();
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00005428 File Offset: 0x00003628
		private void UpdateBindings()
		{
			foreach (VisualElement visualElement in this.m_ElementsWithBindings)
			{
				IBinding bindingObjectFromElement = this.GetBindingObjectFromElement(visualElement);
				bool flag = bindingObjectFromElement == null || visualElement.elementPanel != base.panel;
				if (flag)
				{
					if (bindingObjectFromElement != null)
					{
						bindingObjectFromElement.Release();
					}
					this.StopTracking(visualElement);
				}
				else
				{
					this.updatedBindings.Add(bindingObjectFromElement);
				}
			}
			foreach (IBinding binding in this.updatedBindings)
			{
				binding.PreUpdate();
			}
			foreach (IBinding binding2 in this.updatedBindings)
			{
				binding2.Update();
			}
			this.updatedBindings.Clear();
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00005564 File Offset: 0x00003764
		internal void PollElementsWithBindings(Action<VisualElement, IBinding> callback)
		{
			this.PerformTrackingOperations();
			bool flag = this.m_ElementsWithBindings.Count > 0;
			if (flag)
			{
				foreach (VisualElement visualElement in this.m_ElementsWithBindings)
				{
					IBinding bindingObjectFromElement = this.GetBindingObjectFromElement(visualElement);
					bool flag2 = bindingObjectFromElement == null || visualElement.elementPanel != base.panel;
					if (flag2)
					{
						if (bindingObjectFromElement != null)
						{
							bindingObjectFromElement.Release();
						}
						this.StopTracking(visualElement);
					}
					else
					{
						callback(visualElement, bindingObjectFromElement);
					}
				}
			}
		}

		// Token: 0x0400007E RID: 126
		private static readonly PropertyName s_BindingRequestObjectVEPropertyName = "__unity-binding-request-object";

		// Token: 0x0400007F RID: 127
		private static readonly PropertyName s_AdditionalBindingObjectVEPropertyName = "__unity-additional-binding-object";

		// Token: 0x04000080 RID: 128
		private static readonly string s_Description = "Update Bindings";

		// Token: 0x04000081 RID: 129
		private static readonly ProfilerMarker s_ProfilerMarker = new ProfilerMarker(VisualTreeBindingsUpdater.s_Description);

		// Token: 0x04000082 RID: 130
		private static readonly ProfilerMarker s_ProfilerBindingRequestsMarker = new ProfilerMarker("Bindings.Requests");

		// Token: 0x04000083 RID: 131
		private static ProfilerMarker s_MarkerUpdate = new ProfilerMarker("Bindings.Update");

		// Token: 0x04000084 RID: 132
		private static ProfilerMarker s_MarkerPoll = new ProfilerMarker("Bindings.PollElementsWithBindings");

		// Token: 0x04000086 RID: 134
		private readonly HashSet<VisualElement> m_ElementsWithBindings = new HashSet<VisualElement>();

		// Token: 0x04000087 RID: 135
		private readonly HashSet<VisualElement> m_ElementsToAdd = new HashSet<VisualElement>();

		// Token: 0x04000088 RID: 136
		private readonly HashSet<VisualElement> m_ElementsToRemove = new HashSet<VisualElement>();

		// Token: 0x04000089 RID: 137
		private const int k_MinUpdateDelayMs = 100;

		// Token: 0x0400008A RID: 138
		private const int k_MaxBindingTimeMs = 100;

		// Token: 0x0400008B RID: 139
		private long m_LastUpdateTime = 0L;

		// Token: 0x0400008C RID: 140
		private HashSet<VisualElement> m_ElementsToBind = new HashSet<VisualElement>();

		// Token: 0x0400008E RID: 142
		private List<IBinding> updatedBindings = new List<IBinding>();

		// Token: 0x02000030 RID: 48
		private class RequestObjectListPool : ObjectListPool<IBindingRequest>
		{
		}
	}
}
