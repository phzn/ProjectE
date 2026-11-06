using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Profiling;
using UnityEngine.UIElements.UIR;

namespace UnityEngine.UIElements
{
	// Token: 0x0200037A RID: 890
	internal static class UIElementsRuntimeUtility
	{
		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06001E05 RID: 7685 RVA: 0x00074330 File Offset: 0x00072530
		// (remove) Token: 0x06001E06 RID: 7686 RVA: 0x00074364 File Offset: 0x00072564
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static event Action s_onRepaintOverlayPanels;

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06001E07 RID: 7687 RVA: 0x00074398 File Offset: 0x00072598
		// (remove) Token: 0x06001E08 RID: 7688 RVA: 0x000743C4 File Offset: 0x000725C4
		internal static event Action onRepaintOverlayPanels
		{
			add
			{
				bool flag = UIElementsRuntimeUtility.s_onRepaintOverlayPanels == null;
				if (flag)
				{
					UIElementsRuntimeUtility.RegisterPlayerloopCallback();
				}
				UIElementsRuntimeUtility.s_onRepaintOverlayPanels += value;
			}
			remove
			{
				UIElementsRuntimeUtility.s_onRepaintOverlayPanels -= value;
				bool flag = UIElementsRuntimeUtility.s_onRepaintOverlayPanels == null;
				if (flag)
				{
					UIElementsRuntimeUtility.UnregisterPlayerloopCallback();
				}
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06001E09 RID: 7689 RVA: 0x000743F0 File Offset: 0x000725F0
		// (remove) Token: 0x06001E0A RID: 7690 RVA: 0x00074424 File Offset: 0x00072624
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<BaseRuntimePanel> onCreatePanel;

		// Token: 0x06001E0B RID: 7691 RVA: 0x00074458 File Offset: 0x00072658
		static UIElementsRuntimeUtility()
		{
			UIElementsRuntimeUtilityNative.RepaintOverlayPanelsCallback = delegate
			{
			};
			UIElementsRuntimeUtilityNative.RepaintOffscreenPanelsCallback = new Action(UIElementsRuntimeUtility.RepaintOffscreenPanels);
			Canvas.externBeginRenderOverlays = new Action<int>(UIElementsRuntimeUtility.BeginRenderOverlays);
			Canvas.externRenderOverlaysBefore = delegate(int displayIndex, int sortOrder)
			{
				UIElementsRuntimeUtility.RenderOverlaysBeforePriority(displayIndex, (float)sortOrder);
			};
			Canvas.externEndRenderOverlays = new Action<int>(UIElementsRuntimeUtility.EndRenderOverlays);
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0007450C File Offset: 0x0007270C
		public static EventBase CreateEvent(Event systemEvent)
		{
			return UIElementsUtility.CreateEvent(systemEvent, systemEvent.rawType);
		}

		// Token: 0x06001E0D RID: 7693 RVA: 0x0007452C File Offset: 0x0007272C
		public static BaseRuntimePanel FindOrCreateRuntimePanel(ScriptableObject ownerObject, UIElementsRuntimeUtility.CreateRuntimePanelDelegate createDelegate)
		{
			Panel panel;
			bool flag = UIElementsUtility.TryGetPanel(ownerObject.GetInstanceID(), out panel);
			if (flag)
			{
				BaseRuntimePanel baseRuntimePanel = panel as BaseRuntimePanel;
				bool flag2 = baseRuntimePanel != null;
				if (flag2)
				{
					return baseRuntimePanel;
				}
				UIElementsRuntimeUtility.RemoveCachedPanelInternal(ownerObject.GetInstanceID());
			}
			BaseRuntimePanel baseRuntimePanel2 = createDelegate(ownerObject);
			baseRuntimePanel2.IMGUIEventInterests = new EventInterests
			{
				wantsMouseMove = true,
				wantsMouseEnterLeaveWindow = true
			};
			UIElementsRuntimeUtility.RegisterCachedPanelInternal(ownerObject.GetInstanceID(), baseRuntimePanel2);
			Action<BaseRuntimePanel> action = UIElementsRuntimeUtility.onCreatePanel;
			if (action != null)
			{
				action(baseRuntimePanel2);
			}
			return baseRuntimePanel2;
		}

		// Token: 0x06001E0E RID: 7694 RVA: 0x000745C0 File Offset: 0x000727C0
		public static void DisposeRuntimePanel(ScriptableObject ownerObject)
		{
			Panel panel;
			bool flag = UIElementsUtility.TryGetPanel(ownerObject.GetInstanceID(), out panel);
			if (flag)
			{
				panel.Dispose();
				UIElementsRuntimeUtility.RemoveCachedPanelInternal(ownerObject.GetInstanceID());
			}
		}

		// Token: 0x06001E0F RID: 7695 RVA: 0x000745F4 File Offset: 0x000727F4
		private static void RegisterCachedPanelInternal(int instanceID, IPanel panel)
		{
			UIElementsUtility.RegisterCachedPanel(instanceID, panel as Panel);
			UIElementsRuntimeUtility.s_PanelOrderingDirty = true;
			bool flag = !UIElementsRuntimeUtility.s_RegisteredPlayerloopCallback;
			if (flag)
			{
				UIElementsRuntimeUtility.s_RegisteredPlayerloopCallback = true;
				UIElementsRuntimeUtility.RegisterPlayerloopCallback();
				Canvas.SetExternalCanvasEnabled(true);
			}
		}

		// Token: 0x06001E10 RID: 7696 RVA: 0x00074638 File Offset: 0x00072838
		private static void RemoveCachedPanelInternal(int instanceID)
		{
			UIElementsUtility.RemoveCachedPanel(instanceID);
			UIElementsRuntimeUtility.s_PanelOrderingDirty = true;
			UIElementsRuntimeUtility.s_SortedRuntimePanels.Clear();
			UIElementsUtility.GetAllPanels(UIElementsRuntimeUtility.s_SortedRuntimePanels, ContextType.Player);
			bool flag = UIElementsRuntimeUtility.s_SortedRuntimePanels.Count == 0;
			if (flag)
			{
				UIElementsRuntimeUtility.s_RegisteredPlayerloopCallback = false;
				UIElementsRuntimeUtility.UnregisterPlayerloopCallback();
				Canvas.SetExternalCanvasEnabled(false);
			}
		}

		// Token: 0x06001E11 RID: 7697 RVA: 0x00074690 File Offset: 0x00072890
		public static void RepaintOverlayPanels()
		{
			foreach (Panel panel in UIElementsRuntimeUtility.GetSortedPlayerPanels())
			{
				BaseRuntimePanel baseRuntimePanel = (BaseRuntimePanel)panel;
				bool flag = !baseRuntimePanel.drawToCameras;
				if (flag)
				{
					UIElementsRuntimeUtility.RepaintOverlayPanel(baseRuntimePanel);
				}
			}
			bool flag2 = UIElementsRuntimeUtility.s_onRepaintOverlayPanels != null;
			if (flag2)
			{
				UIElementsRuntimeUtility.s_onRepaintOverlayPanels();
			}
		}

		// Token: 0x06001E12 RID: 7698 RVA: 0x00074714 File Offset: 0x00072914
		public static void RepaintOffscreenPanels()
		{
			foreach (Panel panel in UIElementsRuntimeUtility.GetSortedPlayerPanels())
			{
				BaseRuntimePanel baseRuntimePanel = (BaseRuntimePanel)panel;
				bool flag = baseRuntimePanel.targetTexture != null;
				if (flag)
				{
					UIElementsRuntimeUtility.RepaintOverlayPanel(baseRuntimePanel);
				}
			}
		}

		// Token: 0x06001E13 RID: 7699 RVA: 0x00074784 File Offset: 0x00072984
		public static void RepaintOverlayPanel(BaseRuntimePanel panel)
		{
			Camera current = Camera.current;
			RenderTexture active = RenderTexture.active;
			using (UIElementsRuntimeUtility.s_RepaintProfilerMarker.Auto())
			{
				panel.Repaint(Event.current);
			}
			Camera.SetupCurrent(current);
			RenderTexture.active = active;
		}

		// Token: 0x06001E14 RID: 7700 RVA: 0x000747E8 File Offset: 0x000729E8
		internal static void BeginRenderOverlays(int displayIndex)
		{
			UIElementsRuntimeUtility.currentOverlayIndex = 0;
		}

		// Token: 0x06001E15 RID: 7701 RVA: 0x000747F4 File Offset: 0x000729F4
		internal static void RenderOverlaysBeforePriority(int displayIndex, float maxPriority)
		{
			bool flag = UIElementsRuntimeUtility.currentOverlayIndex < 0;
			if (!flag)
			{
				List<Panel> sortedPlayerPanels = UIElementsRuntimeUtility.GetSortedPlayerPanels();
				while (UIElementsRuntimeUtility.currentOverlayIndex < sortedPlayerPanels.Count)
				{
					BaseRuntimePanel baseRuntimePanel = sortedPlayerPanels[UIElementsRuntimeUtility.currentOverlayIndex] as BaseRuntimePanel;
					bool flag2 = baseRuntimePanel != null;
					if (flag2)
					{
						bool flag3 = baseRuntimePanel.sortingPriority >= maxPriority;
						if (flag3)
						{
							break;
						}
						bool flag4 = baseRuntimePanel.targetDisplay == displayIndex;
						if (flag4)
						{
							UIElementsRuntimeUtility.RepaintOverlayPanel(baseRuntimePanel);
						}
					}
					UIElementsRuntimeUtility.currentOverlayIndex++;
				}
			}
		}

		// Token: 0x06001E16 RID: 7702 RVA: 0x00074880 File Offset: 0x00072A80
		internal static void EndRenderOverlays(int displayIndex)
		{
			UIElementsRuntimeUtility.RenderOverlaysBeforePriority(displayIndex, float.MaxValue);
			UIElementsRuntimeUtility.currentOverlayIndex = -1;
		}

		// Token: 0x17000715 RID: 1813
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x00074895 File Offset: 0x00072A95
		// (set) Token: 0x06001E18 RID: 7704 RVA: 0x0007489C File Offset: 0x00072A9C
		internal static Object activeEventSystem { get; private set; }

		// Token: 0x17000716 RID: 1814
		// (get) Token: 0x06001E19 RID: 7705 RVA: 0x000748A4 File Offset: 0x00072AA4
		internal static bool useDefaultEventSystem
		{
			get
			{
				return UIElementsRuntimeUtility.activeEventSystem == null;
			}
		}

		// Token: 0x06001E1A RID: 7706 RVA: 0x000748B4 File Offset: 0x00072AB4
		public static void RegisterEventSystem(Object eventSystem)
		{
			bool flag = UIElementsRuntimeUtility.activeEventSystem != null && UIElementsRuntimeUtility.activeEventSystem != eventSystem && eventSystem.GetType().Name == "EventSystem";
			if (flag)
			{
				Debug.LogWarning("There can be only one active Event System.");
			}
			UIElementsRuntimeUtility.activeEventSystem = eventSystem;
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x0007490C File Offset: 0x00072B0C
		public static void UnregisterEventSystem(Object eventSystem)
		{
			bool flag = UIElementsRuntimeUtility.activeEventSystem == eventSystem;
			if (flag)
			{
				UIElementsRuntimeUtility.activeEventSystem = null;
			}
		}

		// Token: 0x17000717 RID: 1815
		// (get) Token: 0x06001E1C RID: 7708 RVA: 0x00074930 File Offset: 0x00072B30
		internal static DefaultEventSystem defaultEventSystem
		{
			get
			{
				DefaultEventSystem defaultEventSystem;
				if ((defaultEventSystem = UIElementsRuntimeUtility.s_DefaultEventSystem) == null)
				{
					defaultEventSystem = (UIElementsRuntimeUtility.s_DefaultEventSystem = new DefaultEventSystem());
				}
				return defaultEventSystem;
			}
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x00074948 File Offset: 0x00072B48
		public static void UpdateRuntimePanels()
		{
			UIElementsRuntimeUtility.RemoveUnusedPanels();
			UIRenderDevice.ProcessDeviceFreeQueue();
			foreach (Panel panel in UIElementsRuntimeUtility.GetSortedPlayerPanels())
			{
				BaseRuntimePanel baseRuntimePanel = (BaseRuntimePanel)panel;
				baseRuntimePanel.Update();
			}
			bool flag = Application.isPlaying && UIElementsRuntimeUtility.useDefaultEventSystem;
			if (flag)
			{
				UIElementsRuntimeUtility.defaultEventSystem.Update(DefaultEventSystem.UpdateMode.IgnoreIfAppNotFocused);
			}
		}

		// Token: 0x06001E1E RID: 7710 RVA: 0x000749D4 File Offset: 0x00072BD4
		internal static void MarkPotentiallyEmpty(PanelSettings settings)
		{
			bool flag = !UIElementsRuntimeUtility.s_PotentiallyEmptyPanelSettings.Contains(settings);
			if (flag)
			{
				UIElementsRuntimeUtility.s_PotentiallyEmptyPanelSettings.Add(settings);
			}
		}

		// Token: 0x06001E1F RID: 7711 RVA: 0x00074A00 File Offset: 0x00072C00
		internal static void RemoveUnusedPanels()
		{
			foreach (PanelSettings panelSettings in UIElementsRuntimeUtility.s_PotentiallyEmptyPanelSettings)
			{
				UIDocumentList attachedUIDocumentsList = panelSettings.m_AttachedUIDocumentsList;
				bool flag = attachedUIDocumentsList == null || attachedUIDocumentsList.m_AttachedUIDocuments.Count == 0;
				if (flag)
				{
					panelSettings.DisposePanel();
				}
			}
			UIElementsRuntimeUtility.s_PotentiallyEmptyPanelSettings.Clear();
		}

		// Token: 0x06001E20 RID: 7712 RVA: 0x00074A84 File Offset: 0x00072C84
		public static void RegisterPlayerloopCallback()
		{
			UIElementsRuntimeUtilityNative.RegisterPlayerloopCallback();
			UIElementsRuntimeUtilityNative.UpdateRuntimePanelsCallback = new Action(UIElementsRuntimeUtility.UpdateRuntimePanels);
		}

		// Token: 0x06001E21 RID: 7713 RVA: 0x00074A9E File Offset: 0x00072C9E
		public static void UnregisterPlayerloopCallback()
		{
			UIElementsRuntimeUtilityNative.UnregisterPlayerloopCallback();
			UIElementsRuntimeUtilityNative.UpdateRuntimePanelsCallback = null;
		}

		// Token: 0x06001E22 RID: 7714 RVA: 0x00074AAD File Offset: 0x00072CAD
		internal static void SetPanelOrderingDirty()
		{
			UIElementsRuntimeUtility.s_PanelOrderingDirty = true;
		}

		// Token: 0x06001E23 RID: 7715 RVA: 0x00074AB8 File Offset: 0x00072CB8
		internal static List<Panel> GetSortedPlayerPanels()
		{
			bool flag = UIElementsRuntimeUtility.s_PanelOrderingDirty;
			if (flag)
			{
				UIElementsRuntimeUtility.SortPanels();
			}
			return UIElementsRuntimeUtility.s_SortedRuntimePanels;
		}

		// Token: 0x06001E24 RID: 7716 RVA: 0x00074AE0 File Offset: 0x00072CE0
		private static void SortPanels()
		{
			UIElementsRuntimeUtility.s_SortedRuntimePanels.Clear();
			UIElementsUtility.GetAllPanels(UIElementsRuntimeUtility.s_SortedRuntimePanels, ContextType.Player);
			UIElementsRuntimeUtility.s_SortedRuntimePanels.Sort(delegate(Panel a, Panel b)
			{
				BaseRuntimePanel baseRuntimePanel2 = a as BaseRuntimePanel;
				BaseRuntimePanel baseRuntimePanel3 = b as BaseRuntimePanel;
				bool flag2 = baseRuntimePanel2 == null || baseRuntimePanel3 == null;
				int num;
				if (flag2)
				{
					num = 0;
				}
				else
				{
					float num2 = baseRuntimePanel2.sortingPriority - baseRuntimePanel3.sortingPriority;
					bool flag3 = Mathf.Approximately(0f, num2);
					if (flag3)
					{
						num = baseRuntimePanel2.m_RuntimePanelCreationIndex.CompareTo(baseRuntimePanel3.m_RuntimePanelCreationIndex);
					}
					else
					{
						num = ((num2 < 0f) ? (-1) : 1);
					}
				}
				return num;
			});
			for (int i = 0; i < UIElementsRuntimeUtility.s_SortedRuntimePanels.Count; i++)
			{
				BaseRuntimePanel baseRuntimePanel = UIElementsRuntimeUtility.s_SortedRuntimePanels[i] as BaseRuntimePanel;
				bool flag = baseRuntimePanel != null;
				if (flag)
				{
					baseRuntimePanel.resolvedSortingIndex = i;
				}
			}
			UIElementsRuntimeUtility.s_ResolvedSortingIndexMax = UIElementsRuntimeUtility.s_SortedRuntimePanels.Count - 1;
			UIElementsRuntimeUtility.s_PanelOrderingDirty = false;
		}

		// Token: 0x06001E25 RID: 7717 RVA: 0x00074B84 File Offset: 0x00072D84
		internal static Vector2 MultiDisplayBottomLeftToPanelPosition(Vector2 position, out int? targetDisplay)
		{
			Vector2 vector = UIElementsRuntimeUtility.MultiDisplayToLocalScreenPosition(position, out targetDisplay);
			return UIElementsRuntimeUtility.ScreenBottomLeftToPanelPosition(vector, targetDisplay.GetValueOrDefault());
		}

		// Token: 0x06001E26 RID: 7718 RVA: 0x00074BAC File Offset: 0x00072DAC
		internal static Vector2 MultiDisplayToLocalScreenPosition(Vector2 position, out int? targetDisplay)
		{
			Vector3 vector = Display.RelativeMouseAt(position);
			bool flag = vector != Vector3.zero;
			Vector2 vector2;
			if (flag)
			{
				targetDisplay = new int?((int)vector.z);
				vector2 = vector;
			}
			else
			{
				targetDisplay = null;
				vector2 = position;
			}
			return vector2;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x00074C00 File Offset: 0x00072E00
		internal static Vector2 ScreenBottomLeftToPanelPosition(Vector2 position, int targetDisplay)
		{
			int num = Screen.height;
			bool flag = targetDisplay > 0 && targetDisplay < Display.displays.Length;
			if (flag)
			{
				num = Display.displays[targetDisplay].systemHeight;
			}
			position.y = (float)num - position.y;
			return position;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x00074C4C File Offset: 0x00072E4C
		internal static Vector2 ScreenBottomLeftToPanelDelta(Vector2 delta)
		{
			delta.y = -delta.y;
			return delta;
		}

		// Token: 0x04000C86 RID: 3206
		private static bool s_RegisteredPlayerloopCallback = false;

		// Token: 0x04000C87 RID: 3207
		private static List<Panel> s_SortedRuntimePanels = new List<Panel>();

		// Token: 0x04000C88 RID: 3208
		private static bool s_PanelOrderingDirty = true;

		// Token: 0x04000C89 RID: 3209
		internal static int s_ResolvedSortingIndexMax = 0;

		// Token: 0x04000C8A RID: 3210
		internal static readonly string s_RepaintProfilerMarkerName = "UIElementsRuntimeUtility.DoDispatch(Repaint Event)";

		// Token: 0x04000C8B RID: 3211
		private static readonly ProfilerMarker s_RepaintProfilerMarker = new ProfilerMarker(UIElementsRuntimeUtility.s_RepaintProfilerMarkerName);

		// Token: 0x04000C8C RID: 3212
		private static int currentOverlayIndex = -1;

		// Token: 0x04000C8E RID: 3214
		private static DefaultEventSystem s_DefaultEventSystem;

		// Token: 0x04000C8F RID: 3215
		private static List<PanelSettings> s_PotentiallyEmptyPanelSettings = new List<PanelSettings>();

		// Token: 0x0200037B RID: 891
		// (Invoke) Token: 0x06001E2A RID: 7722
		public delegate BaseRuntimePanel CreateRuntimePanelDelegate(ScriptableObject ownerObject);
	}
}
