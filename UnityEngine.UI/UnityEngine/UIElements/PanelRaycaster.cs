using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine.UIElements
{
	// Token: 0x0200004C RID: 76
	[AddComponentMenu("UI Toolkit/Panel Raycaster (UI Toolkit)")]
	public class PanelRaycaster : BaseRaycaster, IRuntimePanelComponent
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00017E5D File Offset: 0x0001605D
		// (set) Token: 0x06000524 RID: 1316 RVA: 0x00017E68 File Offset: 0x00016068
		public IPanel panel
		{
			get
			{
				return this.m_Panel;
			}
			set
			{
				BaseRuntimePanel baseRuntimePanel = (BaseRuntimePanel)value;
				if (this.m_Panel != baseRuntimePanel)
				{
					this.UnregisterCallbacks();
					this.m_Panel = baseRuntimePanel;
					this.RegisterCallbacks();
				}
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00017E98 File Offset: 0x00016098
		private void RegisterCallbacks()
		{
			if (this.m_Panel != null)
			{
				this.m_Panel.destroyed += this.OnPanelDestroyed;
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00017EB9 File Offset: 0x000160B9
		private void UnregisterCallbacks()
		{
			if (this.m_Panel != null)
			{
				this.m_Panel.destroyed -= this.OnPanelDestroyed;
			}
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00017EDA File Offset: 0x000160DA
		private void OnPanelDestroyed()
		{
			this.panel = null;
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00017EE3 File Offset: 0x000160E3
		private GameObject selectableGameObject
		{
			get
			{
				BaseRuntimePanel panel = this.m_Panel;
				if (panel == null)
				{
					return null;
				}
				return panel.selectableGameObject;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00017EF6 File Offset: 0x000160F6
		public override int sortOrderPriority
		{
			get
			{
				BaseRuntimePanel panel = this.m_Panel;
				return Mathf.FloorToInt((panel != null) ? panel.sortingPriority : 0f);
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00017F13 File Offset: 0x00016113
		public override int renderOrderPriority
		{
			get
			{
				int maxValue = int.MaxValue;
				int s_ResolvedSortingIndexMax = UIElementsRuntimeUtility.s_ResolvedSortingIndexMax;
				BaseRuntimePanel panel = this.m_Panel;
				return maxValue - (s_ResolvedSortingIndexMax - ((panel != null) ? panel.resolvedSortingIndex : 0));
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00017F34 File Offset: 0x00016134
		public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
		{
			if (this.m_Panel == null)
			{
				return;
			}
			int targetDisplay = this.m_Panel.targetDisplay;
			Vector3 relativeMousePositionForRaycast = MultipleDisplayUtilities.GetRelativeMousePositionForRaycast(eventData);
			if ((int)relativeMousePositionForRaycast.z != targetDisplay)
			{
				return;
			}
			Vector3 vector = relativeMousePositionForRaycast;
			Vector2 delta = eventData.delta;
			float num = (float)Screen.height;
			if (targetDisplay > 0 && targetDisplay < Display.displays.Length)
			{
				num = (float)Display.displays[targetDisplay].systemHeight;
			}
			vector.y = num - vector.y;
			delta.y = -delta.y;
			EventSystem eventSystem = UIElementsRuntimeUtility.activeEventSystem as EventSystem;
			if (eventSystem == null || eventSystem.currentInputModule == null)
			{
				return;
			}
			int num2 = eventSystem.currentInputModule.ConvertUIToolkitPointerId(eventData);
			IEventHandler capturingElement = this.m_Panel.GetCapturingElement(num2);
			VisualElement visualElement = capturingElement as VisualElement;
			if (visualElement != null && visualElement.panel != this.m_Panel)
			{
				return;
			}
			IPanel playerPanelWithSoftPointerCapture = PointerDeviceState.GetPlayerPanelWithSoftPointerCapture(num2);
			if (playerPanelWithSoftPointerCapture != null && playerPanelWithSoftPointerCapture != this.m_Panel)
			{
				return;
			}
			if (capturingElement == null && playerPanelWithSoftPointerCapture == null)
			{
				Vector2 vector2;
				Vector2 vector3;
				if (!this.m_Panel.ScreenToPanel(vector, delta, out vector2, out vector3, false))
				{
					return;
				}
				if (this.m_Panel.Pick(vector2) == null)
				{
					return;
				}
			}
			resultAppendList.Add(new RaycastResult
			{
				gameObject = this.selectableGameObject,
				module = this,
				screenPosition = relativeMousePositionForRaycast,
				displayIndex = this.m_Panel.targetDisplay
			});
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x000180A3 File Offset: 0x000162A3
		public override Camera eventCamera
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040001AB RID: 427
		private BaseRuntimePanel m_Panel;
	}
}
