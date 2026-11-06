using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace UnityEngine.EventSystems
{
	// Token: 0x02000065 RID: 101
	[AddComponentMenu("Event/Event System")]
	[DisallowMultipleComponent]
	public class EventSystem : UIBehaviour
	{
		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x000187E7 File Offset: 0x000169E7
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x00018804 File Offset: 0x00016A04
		public static EventSystem current
		{
			get
			{
				if (EventSystem.m_EventSystems.Count <= 0)
				{
					return null;
				}
				return EventSystem.m_EventSystems[0];
			}
			set
			{
				int num = EventSystem.m_EventSystems.IndexOf(value);
				if (num > 0)
				{
					EventSystem.m_EventSystems.RemoveAt(num);
					EventSystem.m_EventSystems.Insert(0, value);
					return;
				}
				if (num < 0)
				{
					Debug.LogError("Failed setting EventSystem.current to unknown EventSystem " + ((value != null) ? value.ToString() : null));
				}
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00018859 File Offset: 0x00016A59
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x00018861 File Offset: 0x00016A61
		public bool sendNavigationEvents
		{
			get
			{
				return this.m_sendNavigationEvents;
			}
			set
			{
				this.m_sendNavigationEvents = value;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0001886A File Offset: 0x00016A6A
		// (set) Token: 0x0600059A RID: 1434 RVA: 0x00018872 File Offset: 0x00016A72
		public int pixelDragThreshold
		{
			get
			{
				return this.m_DragThreshold;
			}
			set
			{
				this.m_DragThreshold = value;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x0001887B File Offset: 0x00016A7B
		public BaseInputModule currentInputModule
		{
			get
			{
				return this.m_CurrentInputModule;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00018883 File Offset: 0x00016A83
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x0001888B File Offset: 0x00016A8B
		public GameObject firstSelectedGameObject
		{
			get
			{
				return this.m_FirstSelected;
			}
			set
			{
				this.m_FirstSelected = value;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x00018894 File Offset: 0x00016A94
		public GameObject currentSelectedGameObject
		{
			get
			{
				return this.m_CurrentSelected;
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001889C File Offset: 0x00016A9C
		[Obsolete("lastSelectedGameObject is no longer supported")]
		public GameObject lastSelectedGameObject
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x0001889F File Offset: 0x00016A9F
		public bool isFocused
		{
			get
			{
				return this.m_HasFocus;
			}
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x000188A7 File Offset: 0x00016AA7
		protected EventSystem()
		{
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x000188D0 File Offset: 0x00016AD0
		public void UpdateModules()
		{
			base.GetComponents<BaseInputModule>(this.m_SystemInputModules);
			for (int i = this.m_SystemInputModules.Count - 1; i >= 0; i--)
			{
				if (!this.m_SystemInputModules[i] || !this.m_SystemInputModules[i].IsActive())
				{
					this.m_SystemInputModules.RemoveAt(i);
				}
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00018933 File Offset: 0x00016B33
		public bool alreadySelecting
		{
			get
			{
				return this.m_SelectionGuard;
			}
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x0001893C File Offset: 0x00016B3C
		public void SetSelectedGameObject(GameObject selected, BaseEventData pointer)
		{
			if (this.m_SelectionGuard)
			{
				Debug.LogError("Attempting to select " + ((selected != null) ? selected.ToString() : null) + "while already selecting an object.");
				return;
			}
			this.m_SelectionGuard = true;
			if (selected == this.m_CurrentSelected)
			{
				this.m_SelectionGuard = false;
				return;
			}
			ExecuteEvents.Execute<IDeselectHandler>(this.m_CurrentSelected, pointer, ExecuteEvents.deselectHandler);
			this.m_CurrentSelected = selected;
			ExecuteEvents.Execute<ISelectHandler>(this.m_CurrentSelected, pointer, ExecuteEvents.selectHandler);
			this.m_SelectionGuard = false;
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x000189C2 File Offset: 0x00016BC2
		private BaseEventData baseEventDataCache
		{
			get
			{
				if (this.m_DummyData == null)
				{
					this.m_DummyData = new BaseEventData(this);
				}
				return this.m_DummyData;
			}
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x000189DE File Offset: 0x00016BDE
		public void SetSelectedGameObject(GameObject selected)
		{
			this.SetSelectedGameObject(selected, this.baseEventDataCache);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x000189F0 File Offset: 0x00016BF0
		private static int RaycastComparer(RaycastResult lhs, RaycastResult rhs)
		{
			if (lhs.module != rhs.module)
			{
				Camera eventCamera = lhs.module.eventCamera;
				Camera eventCamera2 = rhs.module.eventCamera;
				if (eventCamera != null && eventCamera2 != null && eventCamera.depth != eventCamera2.depth)
				{
					if (eventCamera.depth < eventCamera2.depth)
					{
						return 1;
					}
					if (eventCamera.depth == eventCamera2.depth)
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (lhs.module.sortOrderPriority != rhs.module.sortOrderPriority)
					{
						return rhs.module.sortOrderPriority.CompareTo(lhs.module.sortOrderPriority);
					}
					if (lhs.module.renderOrderPriority != rhs.module.renderOrderPriority)
					{
						return rhs.module.renderOrderPriority.CompareTo(lhs.module.renderOrderPriority);
					}
				}
			}
			if (lhs.sortingLayer != rhs.sortingLayer)
			{
				int layerValueFromID = SortingLayer.GetLayerValueFromID(rhs.sortingLayer);
				int layerValueFromID2 = SortingLayer.GetLayerValueFromID(lhs.sortingLayer);
				return layerValueFromID.CompareTo(layerValueFromID2);
			}
			if (lhs.sortingOrder != rhs.sortingOrder)
			{
				return rhs.sortingOrder.CompareTo(lhs.sortingOrder);
			}
			if (lhs.depth != rhs.depth && lhs.module.rootRaycaster == rhs.module.rootRaycaster)
			{
				return rhs.depth.CompareTo(lhs.depth);
			}
			if (lhs.distance != rhs.distance)
			{
				return lhs.distance.CompareTo(rhs.distance);
			}
			if (lhs.sortingGroupID != SortingGroup.invalidSortingGroupID && rhs.sortingGroupID != SortingGroup.invalidSortingGroupID)
			{
				if (lhs.sortingGroupID != rhs.sortingGroupID)
				{
					return lhs.sortingGroupID.CompareTo(rhs.sortingGroupID);
				}
				if (lhs.sortingGroupOrder != rhs.sortingGroupOrder)
				{
					return rhs.sortingGroupOrder.CompareTo(lhs.sortingGroupOrder);
				}
			}
			return lhs.index.CompareTo(rhs.index);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00018BFC File Offset: 0x00016DFC
		public void RaycastAll(PointerEventData eventData, List<RaycastResult> raycastResults)
		{
			raycastResults.Clear();
			List<BaseRaycaster> raycasters = RaycasterManager.GetRaycasters();
			int count = raycasters.Count;
			for (int i = 0; i < count; i++)
			{
				BaseRaycaster baseRaycaster = raycasters[i];
				if (!(baseRaycaster == null) && baseRaycaster.IsActive())
				{
					baseRaycaster.Raycast(eventData, raycastResults);
				}
			}
			raycastResults.Sort(EventSystem.s_RaycastComparer);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00018C54 File Offset: 0x00016E54
		public bool IsPointerOverGameObject()
		{
			return this.IsPointerOverGameObject(-1);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00018C5D File Offset: 0x00016E5D
		public bool IsPointerOverGameObject(int pointerId)
		{
			return this.m_CurrentInputModule != null && this.m_CurrentInputModule.IsPointerOverGameObject(pointerId);
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x00018C7B File Offset: 0x00016E7B
		private bool isUIToolkitActiveEventSystem
		{
			get
			{
				return EventSystem.s_UIToolkitOverride.activeEventSystem == this || EventSystem.s_UIToolkitOverride.activeEventSystem == null;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060005AC RID: 1452 RVA: 0x00018CA1 File Offset: 0x00016EA1
		private bool sendUIToolkitEvents
		{
			get
			{
				return EventSystem.s_UIToolkitOverride.sendEvents && this.isUIToolkitActiveEventSystem;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x00018CB7 File Offset: 0x00016EB7
		private bool createUIToolkitPanelGameObjectsOnStart
		{
			get
			{
				return EventSystem.s_UIToolkitOverride.createPanelGameObjectsOnStart && this.isUIToolkitActiveEventSystem;
			}
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00018CD0 File Offset: 0x00016ED0
		public static void SetUITookitEventSystemOverride(EventSystem activeEventSystem, bool sendEvents = true, bool createPanelGameObjectsOnStart = true)
		{
			UIElementsRuntimeUtility.UnregisterEventSystem(UIElementsRuntimeUtility.activeEventSystem);
			EventSystem.s_UIToolkitOverride = new EventSystem.UIToolkitOverrideConfig
			{
				activeEventSystem = activeEventSystem,
				sendEvents = sendEvents,
				createPanelGameObjectsOnStart = createPanelGameObjectsOnStart
			};
			if (sendEvents && ((activeEventSystem != null) ? activeEventSystem : EventSystem.current).isActiveAndEnabled)
			{
				UIElementsRuntimeUtility.RegisterEventSystem(activeEventSystem);
			}
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00018D30 File Offset: 0x00016F30
		private void StartTrackingUIToolkitPanels()
		{
			if (this.createUIToolkitPanelGameObjectsOnStart)
			{
				foreach (Panel panel in UIElementsRuntimeUtility.GetSortedPlayerPanels())
				{
					BaseRuntimePanel baseRuntimePanel = (BaseRuntimePanel)panel;
					this.CreateUIToolkitPanelGameObject(baseRuntimePanel);
				}
				UIElementsRuntimeUtility.onCreatePanel += this.CreateUIToolkitPanelGameObject;
				this.m_IsTrackingUIToolkitPanels = true;
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00018DA8 File Offset: 0x00016FA8
		private void StopTrackingUIToolkitPanels()
		{
			if (this.m_IsTrackingUIToolkitPanels)
			{
				UIElementsRuntimeUtility.onCreatePanel -= this.CreateUIToolkitPanelGameObject;
				this.m_IsTrackingUIToolkitPanels = false;
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00018DCC File Offset: 0x00016FCC
		private void CreateUIToolkitPanelGameObject(BaseRuntimePanel panel)
		{
			if (panel.selectableGameObject == null)
			{
				GameObject go = new GameObject(panel.name, new Type[]
				{
					typeof(PanelEventHandler),
					typeof(PanelRaycaster)
				});
				go.transform.SetParent(base.transform);
				panel.selectableGameObject = go;
				panel.destroyed += delegate
				{
					Object.DestroyImmediate(go);
				};
			}
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00018E52 File Offset: 0x00017052
		protected override void Start()
		{
			base.Start();
			this.m_Started = true;
			this.StartTrackingUIToolkitPanels();
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00018E67 File Offset: 0x00017067
		protected override void OnEnable()
		{
			base.OnEnable();
			EventSystem.m_EventSystems.Add(this);
			if (this.m_Started && !this.m_IsTrackingUIToolkitPanels)
			{
				this.StartTrackingUIToolkitPanels();
			}
			if (this.sendUIToolkitEvents)
			{
				UIElementsRuntimeUtility.RegisterEventSystem(this);
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00018E9E File Offset: 0x0001709E
		protected override void OnDisable()
		{
			this.StopTrackingUIToolkitPanels();
			UIElementsRuntimeUtility.UnregisterEventSystem(this);
			if (this.m_CurrentInputModule != null)
			{
				this.m_CurrentInputModule.DeactivateModule();
				this.m_CurrentInputModule = null;
			}
			EventSystem.m_EventSystems.Remove(this);
			base.OnDisable();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00018EE0 File Offset: 0x000170E0
		private void TickModules()
		{
			int count = this.m_SystemInputModules.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.m_SystemInputModules[i] != null)
				{
					this.m_SystemInputModules[i].UpdateModule();
				}
			}
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00018F2A File Offset: 0x0001712A
		protected virtual void OnApplicationFocus(bool hasFocus)
		{
			this.m_HasFocus = hasFocus;
			if (!this.m_HasFocus)
			{
				this.TickModules();
			}
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00018F44 File Offset: 0x00017144
		protected virtual void Update()
		{
			if (EventSystem.current != this)
			{
				return;
			}
			this.TickModules();
			bool flag = false;
			int count = this.m_SystemInputModules.Count;
			int i = 0;
			while (i < count)
			{
				BaseInputModule baseInputModule = this.m_SystemInputModules[i];
				if (baseInputModule.IsModuleSupported() && baseInputModule.ShouldActivateModule())
				{
					if (this.m_CurrentInputModule != baseInputModule)
					{
						this.ChangeEventModule(baseInputModule);
						flag = true;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (this.m_CurrentInputModule == null)
			{
				for (int j = 0; j < count; j++)
				{
					BaseInputModule baseInputModule2 = this.m_SystemInputModules[j];
					if (baseInputModule2.IsModuleSupported())
					{
						this.ChangeEventModule(baseInputModule2);
						flag = true;
						break;
					}
				}
			}
			if (!flag && this.m_CurrentInputModule != null)
			{
				this.m_CurrentInputModule.Process();
			}
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00019013 File Offset: 0x00017213
		private void ChangeEventModule(BaseInputModule module)
		{
			if (this.m_CurrentInputModule == module)
			{
				return;
			}
			if (this.m_CurrentInputModule != null)
			{
				this.m_CurrentInputModule.DeactivateModule();
			}
			if (module != null)
			{
				module.ActivateModule();
			}
			this.m_CurrentInputModule = module;
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00019054 File Offset: 0x00017254
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = "<b>Selected:</b>";
			GameObject currentSelectedGameObject = this.currentSelectedGameObject;
			stringBuilder.AppendLine(text + ((currentSelectedGameObject != null) ? currentSelectedGameObject.ToString() : null));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine((this.m_CurrentInputModule != null) ? this.m_CurrentInputModule.ToString() : "No module");
			return stringBuilder.ToString();
		}

		// Token: 0x040001D5 RID: 469
		private List<BaseInputModule> m_SystemInputModules = new List<BaseInputModule>();

		// Token: 0x040001D6 RID: 470
		private BaseInputModule m_CurrentInputModule;

		// Token: 0x040001D7 RID: 471
		private static List<EventSystem> m_EventSystems = new List<EventSystem>();

		// Token: 0x040001D8 RID: 472
		[SerializeField]
		[FormerlySerializedAs("m_Selected")]
		private GameObject m_FirstSelected;

		// Token: 0x040001D9 RID: 473
		[SerializeField]
		private bool m_sendNavigationEvents = true;

		// Token: 0x040001DA RID: 474
		[SerializeField]
		private int m_DragThreshold = 10;

		// Token: 0x040001DB RID: 475
		private GameObject m_CurrentSelected;

		// Token: 0x040001DC RID: 476
		private bool m_HasFocus = true;

		// Token: 0x040001DD RID: 477
		private bool m_SelectionGuard;

		// Token: 0x040001DE RID: 478
		private BaseEventData m_DummyData;

		// Token: 0x040001DF RID: 479
		private static readonly Comparison<RaycastResult> s_RaycastComparer = new Comparison<RaycastResult>(EventSystem.RaycastComparer);

		// Token: 0x040001E0 RID: 480
		private static EventSystem.UIToolkitOverrideConfig s_UIToolkitOverride = new EventSystem.UIToolkitOverrideConfig
		{
			activeEventSystem = null,
			sendEvents = true,
			createPanelGameObjectsOnStart = true
		};

		// Token: 0x040001E1 RID: 481
		private bool m_Started;

		// Token: 0x040001E2 RID: 482
		private bool m_IsTrackingUIToolkitPanels;

		// Token: 0x020000C3 RID: 195
		private struct UIToolkitOverrideConfig
		{
			// Token: 0x0400034F RID: 847
			public EventSystem activeEventSystem;

			// Token: 0x04000350 RID: 848
			public bool sendEvents;

			// Token: 0x04000351 RID: 849
			public bool createPanelGameObjectsOnStart;
		}
	}
}
