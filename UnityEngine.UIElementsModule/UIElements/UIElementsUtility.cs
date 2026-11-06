using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x02000380 RID: 896
	internal class UIElementsUtility : IUIElementsUtility
	{
		// Token: 0x06001E4C RID: 7756 RVA: 0x00075145 File Offset: 0x00073345
		private UIElementsUtility()
		{
			UIEventRegistration.RegisterUIElementSystem(this);
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x00075158 File Offset: 0x00073358
		internal static IMGUIContainer GetCurrentIMGUIContainer()
		{
			bool flag = UIElementsUtility.s_ContainerStack.Count > 0;
			IMGUIContainer imguicontainer;
			if (flag)
			{
				imguicontainer = UIElementsUtility.s_ContainerStack.Peek();
			}
			else
			{
				imguicontainer = null;
			}
			return imguicontainer;
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x0007518C File Offset: 0x0007338C
		bool IUIElementsUtility.MakeCurrentIMGUIContainerDirty()
		{
			bool flag = UIElementsUtility.s_ContainerStack.Count > 0;
			bool flag2;
			if (flag)
			{
				UIElementsUtility.s_ContainerStack.Peek().MarkDirtyLayout();
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000751C8 File Offset: 0x000733C8
		bool IUIElementsUtility.TakeCapture()
		{
			bool flag = UIElementsUtility.s_ContainerStack.Count > 0;
			bool flag2;
			if (flag)
			{
				IMGUIContainer imguicontainer = UIElementsUtility.s_ContainerStack.Peek();
				imguicontainer.CaptureMouse();
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x00075204 File Offset: 0x00073404
		bool IUIElementsUtility.ReleaseCapture()
		{
			return false;
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x00075218 File Offset: 0x00073418
		bool IUIElementsUtility.ProcessEvent(int instanceID, IntPtr nativeEventPtr, ref bool eventHandled)
		{
			Panel panel;
			bool flag = nativeEventPtr != IntPtr.Zero && UIElementsUtility.s_UIElementsCache.TryGetValue(instanceID, out panel);
			bool flag3;
			if (flag)
			{
				bool flag2 = panel.contextType == ContextType.Editor;
				if (flag2)
				{
					UIElementsUtility.s_EventInstance.CopyFromPtr(nativeEventPtr);
					eventHandled = UIElementsUtility.DoDispatch(panel);
				}
				flag3 = true;
			}
			else
			{
				flag3 = false;
			}
			return flag3;
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x00075274 File Offset: 0x00073474
		bool IUIElementsUtility.CleanupRoots()
		{
			UIElementsUtility.s_EventInstance = null;
			UIElementsUtility.s_UIElementsCache = null;
			UIElementsUtility.s_ContainerStack = null;
			return false;
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x0007529C File Offset: 0x0007349C
		bool IUIElementsUtility.EndContainerGUIFromException(Exception exception)
		{
			bool flag = UIElementsUtility.s_ContainerStack.Count > 0;
			if (flag)
			{
				GUIUtility.EndContainer();
				UIElementsUtility.s_ContainerStack.Pop();
			}
			return false;
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x000752D4 File Offset: 0x000734D4
		void IUIElementsUtility.UpdateSchedulers()
		{
			UIElementsUtility.s_PanelsIterationList.Clear();
			UIElementsUtility.GetAllPanels(UIElementsUtility.s_PanelsIterationList, ContextType.Editor);
			foreach (Panel panel in UIElementsUtility.s_PanelsIterationList)
			{
				panel.timerEventScheduler.UpdateScheduledEvents();
				panel.ValidateFocus();
				panel.UpdateAnimations();
				panel.UpdateBindings();
			}
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x0007535C File Offset: 0x0007355C
		void IUIElementsUtility.RequestRepaintForPanels(Action<ScriptableObject> repaintCallback)
		{
			Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
			while (panelsIterator.MoveNext())
			{
				KeyValuePair<int, Panel> keyValuePair = panelsIterator.Current;
				Panel value = keyValuePair.Value;
				bool flag = value.contextType != ContextType.Editor;
				if (!flag)
				{
					bool isDirty = value.isDirty;
					if (isDirty)
					{
						repaintCallback(value.ownerObject);
					}
				}
			}
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000753BE File Offset: 0x000735BE
		public static void RegisterCachedPanel(int instanceID, Panel panel)
		{
			UIElementsUtility.s_UIElementsCache.Add(instanceID, panel);
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000753CE File Offset: 0x000735CE
		public static void RemoveCachedPanel(int instanceID)
		{
			UIElementsUtility.s_UIElementsCache.Remove(instanceID);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000753E0 File Offset: 0x000735E0
		public static bool TryGetPanel(int instanceID, out Panel panel)
		{
			return UIElementsUtility.s_UIElementsCache.TryGetValue(instanceID, out panel);
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x00075400 File Offset: 0x00073600
		internal static void BeginContainerGUI(GUILayoutUtility.LayoutCache cache, Event evt, IMGUIContainer container)
		{
			bool useOwnerObjectGUIState = container.useOwnerObjectGUIState;
			if (useOwnerObjectGUIState)
			{
				GUIUtility.BeginContainerFromOwner(container.elementPanel.ownerObject);
			}
			else
			{
				GUIUtility.BeginContainer(container.guiState);
			}
			UIElementsUtility.s_ContainerStack.Push(container);
			GUIUtility.s_SkinMode = (int)container.contextType;
			GUIUtility.s_OriginalID = container.elementPanel.ownerObject.GetInstanceID();
			bool flag = Event.current == null;
			if (flag)
			{
				Event.current = evt;
			}
			else
			{
				Event.current.CopyFrom(evt);
			}
			GUI.enabled = container.enabledInHierarchy;
			GUILayoutUtility.BeginContainer(cache);
			GUIUtility.ResetGlobalState();
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x000754A8 File Offset: 0x000736A8
		internal static void EndContainerGUI(Event evt, Rect layoutSize)
		{
			bool flag = Event.current.type == EventType.Layout && UIElementsUtility.s_ContainerStack.Count > 0;
			if (flag)
			{
				GUILayoutUtility.LayoutFromContainer(layoutSize.width, layoutSize.height);
			}
			GUILayoutUtility.SelectIDList(GUIUtility.s_OriginalID, false);
			GUIContent.ClearStaticCache();
			bool flag2 = UIElementsUtility.s_ContainerStack.Count > 0;
			if (flag2)
			{
			}
			evt.CopyFrom(Event.current);
			bool flag3 = UIElementsUtility.s_ContainerStack.Count > 0;
			if (flag3)
			{
				GUIUtility.EndContainer();
				UIElementsUtility.s_ContainerStack.Pop();
			}
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x00075544 File Offset: 0x00073744
		internal static EventBase CreateEvent(Event systemEvent)
		{
			return UIElementsUtility.CreateEvent(systemEvent, systemEvent.rawType);
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x00075564 File Offset: 0x00073764
		internal static EventBase CreateEvent(Event systemEvent, EventType eventType)
		{
			switch (eventType)
			{
			case EventType.MouseDown:
				goto IL_0097;
			case EventType.MouseUp:
				goto IL_00C2;
			case EventType.MouseMove:
				break;
			case EventType.MouseDrag:
				return PointerEventBase<PointerMoveEvent>.GetPooled(systemEvent);
			case EventType.KeyDown:
				return KeyboardEventBase<KeyDownEvent>.GetPooled(systemEvent);
			case EventType.KeyUp:
				return KeyboardEventBase<KeyUpEvent>.GetPooled(systemEvent);
			case EventType.ScrollWheel:
				return WheelEvent.GetPooled(systemEvent);
			case EventType.Repaint:
			case EventType.Layout:
			case EventType.DragUpdated:
			case EventType.DragPerform:
			case EventType.Ignore:
			case EventType.Used:
			case EventType.DragExited:
			case (EventType)17:
			case (EventType)18:
			case (EventType)19:
				goto IL_0134;
			case EventType.ValidateCommand:
				return CommandEventBase<ValidateCommandEvent>.GetPooled(systemEvent);
			case EventType.ExecuteCommand:
				return CommandEventBase<ExecuteCommandEvent>.GetPooled(systemEvent);
			case EventType.ContextClick:
				return MouseEventBase<ContextClickEvent>.GetPooled(systemEvent);
			case EventType.MouseEnterWindow:
				return MouseEventBase<MouseEnterWindowEvent>.GetPooled(systemEvent);
			case EventType.MouseLeaveWindow:
				return MouseLeaveWindowEvent.GetPooled(systemEvent);
			default:
				switch (eventType)
				{
				case EventType.TouchDown:
					goto IL_0097;
				case EventType.TouchUp:
					goto IL_00C2;
				case EventType.TouchMove:
					break;
				default:
					goto IL_0134;
				}
				break;
			}
			return PointerEventBase<PointerMoveEvent>.GetPooled(systemEvent);
			IL_0097:
			bool flag = PointerDeviceState.HasAdditionalPressedButtons(PointerId.mousePointerId, systemEvent.button);
			if (flag)
			{
				return PointerEventBase<PointerMoveEvent>.GetPooled(systemEvent);
			}
			return PointerEventBase<PointerDownEvent>.GetPooled(systemEvent);
			IL_00C2:
			bool flag2 = PointerDeviceState.HasAdditionalPressedButtons(PointerId.mousePointerId, systemEvent.button);
			if (flag2)
			{
				return PointerEventBase<PointerMoveEvent>.GetPooled(systemEvent);
			}
			return PointerEventBase<PointerUpEvent>.GetPooled(systemEvent);
			IL_0134:
			return IMGUIEvent.GetPooled(systemEvent);
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x000756B0 File Offset: 0x000738B0
		private static bool DoDispatch(BaseVisualElementPanel panel)
		{
			Debug.Assert(panel.contextType == ContextType.Editor);
			bool flag = false;
			bool flag2 = UIElementsUtility.s_EventInstance.type == EventType.Repaint;
			if (flag2)
			{
				Camera current = Camera.current;
				RenderTexture active = RenderTexture.active;
				Camera.SetupCurrent(null);
				RenderTexture.active = null;
				using (UIElementsUtility.s_RepaintProfilerMarker.Auto())
				{
					panel.Repaint(UIElementsUtility.s_EventInstance);
				}
				flag = panel.IMGUIContainersCount > 0;
				Camera.SetupCurrent(current);
				RenderTexture.active = active;
			}
			else
			{
				panel.ValidateLayout();
				using (EventBase eventBase = UIElementsUtility.CreateEvent(UIElementsUtility.s_EventInstance))
				{
					bool flag3 = UIElementsUtility.s_EventInstance.type == EventType.Used || UIElementsUtility.s_EventInstance.type == EventType.Layout || UIElementsUtility.s_EventInstance.type == EventType.ExecuteCommand || UIElementsUtility.s_EventInstance.type == EventType.ValidateCommand;
					using (UIElementsUtility.s_EventProfilerMarker.Auto())
					{
						panel.SendEvent(eventBase, flag3 ? DispatchMode.Immediate : DispatchMode.Default);
					}
					bool isPropagationStopped = eventBase.isPropagationStopped;
					if (isPropagationStopped)
					{
						panel.visualTree.IncrementVersion(VersionChangeType.Repaint);
						flag = true;
					}
				}
			}
			return flag;
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x0007582C File Offset: 0x00073A2C
		internal static void GetAllPanels(List<Panel> panels, ContextType contextType)
		{
			Dictionary<int, Panel>.Enumerator panelsIterator = UIElementsUtility.GetPanelsIterator();
			while (panelsIterator.MoveNext())
			{
				KeyValuePair<int, Panel> keyValuePair = panelsIterator.Current;
				bool flag = keyValuePair.Value.contextType == contextType;
				if (flag)
				{
					keyValuePair = panelsIterator.Current;
					panels.Add(keyValuePair.Value);
				}
			}
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x00075884 File Offset: 0x00073A84
		internal static Dictionary<int, Panel>.Enumerator GetPanelsIterator()
		{
			return UIElementsUtility.s_UIElementsCache.GetEnumerator();
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000758A0 File Offset: 0x00073AA0
		internal static Panel FindOrCreateEditorPanel(ScriptableObject ownerObject)
		{
			Panel panel;
			bool flag = !UIElementsUtility.s_UIElementsCache.TryGetValue(ownerObject.GetInstanceID(), out panel);
			if (flag)
			{
				panel = Panel.CreateEditorPanel(ownerObject);
				UIElementsUtility.RegisterCachedPanel(ownerObject.GetInstanceID(), panel);
			}
			else
			{
				Debug.Assert(ContextType.Editor == panel.contextType, "Panel is not an editor panel.");
			}
			return panel;
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000758FC File Offset: 0x00073AFC
		internal static float PixelsPerUnitScaleForElement(VisualElement ve, Sprite sprite)
		{
			bool flag = ve == null || ve.elementPanel == null || sprite == null;
			float num;
			if (flag)
			{
				num = 1f;
			}
			else
			{
				float referenceSpritePixelsPerUnit = ve.elementPanel.referenceSpritePixelsPerUnit;
				float num2 = sprite.pixelsPerUnit;
				num2 = Mathf.Max(0.01f, num2);
				num = referenceSpritePixelsPerUnit / num2;
			}
			return num;
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x00075954 File Offset: 0x00073B54
		internal static string ParseMenuName(string menuName)
		{
			bool flag = string.IsNullOrEmpty(menuName);
			string text;
			if (flag)
			{
				text = string.Empty;
			}
			else
			{
				string text2 = menuName.TrimEnd();
				int num = text2.LastIndexOf(' ');
				bool flag2 = num > -1;
				if (flag2)
				{
					int num2 = Array.IndexOf<char>(UIElementsUtility.s_Modifiers, text2[num + 1]);
					bool flag3 = text2.Length > num + 1 && num2 > -1;
					if (flag3)
					{
						text2 = text2.Substring(0, num).TrimEnd();
					}
				}
				text = text2;
			}
			return text;
		}

		// Token: 0x04000C94 RID: 3220
		private static Stack<IMGUIContainer> s_ContainerStack = new Stack<IMGUIContainer>();

		// Token: 0x04000C95 RID: 3221
		private static Dictionary<int, Panel> s_UIElementsCache = new Dictionary<int, Panel>();

		// Token: 0x04000C96 RID: 3222
		private static Event s_EventInstance = new Event();

		// Token: 0x04000C97 RID: 3223
		internal static Color editorPlayModeTintColor = Color.white;

		// Token: 0x04000C98 RID: 3224
		internal static float singleLineHeight = 18f;

		// Token: 0x04000C99 RID: 3225
		public const string hiddenClassName = "unity-hidden";

		// Token: 0x04000C9A RID: 3226
		private static UIElementsUtility s_Instance = new UIElementsUtility();

		// Token: 0x04000C9B RID: 3227
		internal static List<Panel> s_PanelsIterationList = new List<Panel>();

		// Token: 0x04000C9C RID: 3228
		internal static readonly string s_RepaintProfilerMarkerName = "UIElementsUtility.DoDispatch(Repaint Event)";

		// Token: 0x04000C9D RID: 3229
		internal static readonly string s_EventProfilerMarkerName = "UIElementsUtility.DoDispatch(Non Repaint Event)";

		// Token: 0x04000C9E RID: 3230
		private static readonly ProfilerMarker s_RepaintProfilerMarker = new ProfilerMarker(UIElementsUtility.s_RepaintProfilerMarkerName);

		// Token: 0x04000C9F RID: 3231
		private static readonly ProfilerMarker s_EventProfilerMarker = new ProfilerMarker(UIElementsUtility.s_EventProfilerMarkerName);

		// Token: 0x04000CA0 RID: 3232
		internal static char[] s_Modifiers = new char[] { '&', '%', '^', '#', '_' };
	}
}
