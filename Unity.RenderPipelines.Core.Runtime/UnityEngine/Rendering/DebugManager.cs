using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering.UI;

namespace UnityEngine.Rendering
{
	// Token: 0x02000063 RID: 99
	public sealed class DebugManager
	{
		// Token: 0x06000323 RID: 803 RVA: 0x0000D974 File Offset: 0x0000BB74
		private void RegisterActions()
		{
			this.m_DebugActions = new DebugActionDesc[9];
			this.m_DebugActionStates = new DebugActionState[9];
			this.AddAction(DebugAction.EnableDebugMenu, new DebugActionDesc
			{
				buttonAction = this.debugActionMap.FindAction("Enable Debug", false),
				repeatMode = DebugActionRepeatMode.Never
			});
			this.AddAction(DebugAction.ResetAll, new DebugActionDesc
			{
				buttonAction = this.debugActionMap.FindAction("Debug Reset", false),
				repeatMode = DebugActionRepeatMode.Never
			});
			this.AddAction(DebugAction.NextDebugPanel, new DebugActionDesc
			{
				buttonAction = this.debugActionMap.FindAction("Debug Next", false),
				repeatMode = DebugActionRepeatMode.Never
			});
			this.AddAction(DebugAction.PreviousDebugPanel, new DebugActionDesc
			{
				buttonAction = this.debugActionMap.FindAction("Debug Previous", false),
				repeatMode = DebugActionRepeatMode.Never
			});
			DebugActionDesc debugActionDesc = new DebugActionDesc();
			debugActionDesc.buttonAction = this.debugActionMap.FindAction("Debug Validate", false);
			debugActionDesc.repeatMode = DebugActionRepeatMode.Never;
			this.AddAction(DebugAction.Action, debugActionDesc);
			this.AddAction(DebugAction.MakePersistent, new DebugActionDesc
			{
				buttonAction = this.debugActionMap.FindAction("Debug Persistent", false),
				repeatMode = DebugActionRepeatMode.Never
			});
			DebugActionDesc debugActionDesc2 = new DebugActionDesc();
			debugActionDesc2.buttonAction = this.debugActionMap.FindAction("Debug Multiplier", false);
			debugActionDesc2.repeatMode = DebugActionRepeatMode.Delay;
			debugActionDesc.repeatDelay = 0f;
			this.AddAction(DebugAction.Multiplier, debugActionDesc2);
			this.AddAction(DebugAction.MoveVertical, new DebugActionDesc
			{
				buttonAction = this.debugActionMap.FindAction("Debug Vertical", false),
				repeatMode = DebugActionRepeatMode.Delay,
				repeatDelay = 0.16f
			});
			this.AddAction(DebugAction.MoveHorizontal, new DebugActionDesc
			{
				buttonAction = this.debugActionMap.FindAction("Debug Horizontal", false),
				repeatMode = DebugActionRepeatMode.Delay,
				repeatDelay = 0.16f
			});
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000DB60 File Offset: 0x0000BD60
		internal void EnableInputActions()
		{
			foreach (InputAction inputAction in this.debugActionMap)
			{
				inputAction.Enable();
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000DBAC File Offset: 0x0000BDAC
		private void AddAction(DebugAction action, DebugActionDesc desc)
		{
			this.m_DebugActions[(int)action] = desc;
			this.m_DebugActionStates[(int)action] = new DebugActionState();
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000DBD4 File Offset: 0x0000BDD4
		private void SampleAction(int actionIndex)
		{
			DebugActionDesc debugActionDesc = this.m_DebugActions[actionIndex];
			DebugActionState debugActionState = this.m_DebugActionStates[actionIndex];
			if (!debugActionState.runningAction && debugActionDesc.buttonAction != null)
			{
				float num = debugActionDesc.buttonAction.ReadValue<float>();
				if (!Mathf.Approximately(num, 0f))
				{
					debugActionState.TriggerWithButton(debugActionDesc.buttonAction, num);
				}
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000DC2C File Offset: 0x0000BE2C
		private void UpdateAction(int actionIndex)
		{
			DebugActionDesc debugActionDesc = this.m_DebugActions[actionIndex];
			DebugActionState debugActionState = this.m_DebugActionStates[actionIndex];
			if (debugActionState.runningAction)
			{
				debugActionState.Update(debugActionDesc);
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000DC5C File Offset: 0x0000BE5C
		internal void UpdateActions()
		{
			for (int i = 0; i < this.m_DebugActions.Length; i++)
			{
				this.UpdateAction(i);
				this.SampleAction(i);
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000DC8A File Offset: 0x0000BE8A
		internal float GetAction(DebugAction action)
		{
			return this.m_DebugActionStates[(int)action].actionState;
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000DC9C File Offset: 0x0000BE9C
		internal bool GetActionToggleDebugMenuWithTouch()
		{
			if (!EnhancedTouchSupport.enabled)
			{
				return false;
			}
			ReadOnlyArray<Touch> activeTouches = Touch.activeTouches;
			int count = activeTouches.Count;
			TouchPhase? touchPhase = null;
			if (count == 3)
			{
				foreach (Touch touch in activeTouches)
				{
					if ((touchPhase == null || touch.phase == touchPhase.Value) && touch.tapCount == 2)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000DD34 File Offset: 0x0000BF34
		internal bool GetActionReleaseScrollTarget()
		{
			bool flag = Mouse.current != null && Mouse.current.scroll.ReadValue() != Vector2.zero;
			bool flag2 = Touchscreen.current != null;
			return flag || flag2;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000DD70 File Offset: 0x0000BF70
		private void RegisterInputs()
		{
			this.debugActionMap.AddAction("Enable Debug", InputActionType.Button, null, null, null, null, null).AddCompositeBinding("ButtonWithOneModifier", null, null).With("Modifier", "<Gamepad>/rightStickPress", null, null)
				.With("Button", "<Gamepad>/leftStickPress", null, null)
				.With("Modifier", "<Keyboard>/leftCtrl", null, null)
				.With("Button", "<Keyboard>/backspace", null, null);
			this.debugActionMap.AddAction("Debug Reset", InputActionType.Button, null, null, null, null, null).AddCompositeBinding("ButtonWithOneModifier", null, null).With("Modifier", "<Gamepad>/rightStickPress", null, null)
				.With("Button", "<Gamepad>/b", null, null)
				.With("Modifier", "<Keyboard>/leftAlt", null, null)
				.With("Button", "<Keyboard>/backspace", null, null);
			InputAction inputAction = this.debugActionMap.AddAction("Debug Next", InputActionType.Button, null, null, null, null, null);
			inputAction.AddBinding("<Keyboard>/pageDown", null, null, null);
			inputAction.AddBinding("<Gamepad>/rightShoulder", null, null, null);
			InputAction inputAction2 = this.debugActionMap.AddAction("Debug Previous", InputActionType.Button, null, null, null, null, null);
			inputAction2.AddBinding("<Keyboard>/pageUp", null, null, null);
			inputAction2.AddBinding("<Gamepad>/leftShoulder", null, null, null);
			InputAction inputAction3 = this.debugActionMap.AddAction("Debug Validate", InputActionType.Button, null, null, null, null, null);
			inputAction3.AddBinding("<Keyboard>/enter", null, null, null);
			inputAction3.AddBinding("<Gamepad>/a", null, null, null);
			InputAction inputAction4 = this.debugActionMap.AddAction("Debug Persistent", InputActionType.Button, null, null, null, null, null);
			inputAction4.AddBinding("<Keyboard>/rightShift", null, null, null);
			inputAction4.AddBinding("<Gamepad>/x", null, null, null);
			InputAction inputAction5 = this.debugActionMap.AddAction("Debug Multiplier", InputActionType.Value, null, null, null, null, null);
			inputAction5.AddBinding("<Keyboard>/leftShift", null, null, null);
			inputAction5.AddBinding("<Gamepad>/y", null, null, null);
			this.debugActionMap.AddAction("Debug Vertical", InputActionType.Value, null, null, null, null, null).AddCompositeBinding("1DAxis", null, null).With("Positive", "<Gamepad>/dpad/up", null, null)
				.With("Negative", "<Gamepad>/dpad/down", null, null)
				.With("Positive", "<Keyboard>/upArrow", null, null)
				.With("Negative", "<Keyboard>/downArrow", null, null);
			this.debugActionMap.AddAction("Debug Horizontal", InputActionType.Value, null, null, null, null, null).AddCompositeBinding("1DAxis", null, null).With("Positive", "<Gamepad>/dpad/right", null, null)
				.With("Negative", "<Gamepad>/dpad/left", null, null)
				.With("Positive", "<Keyboard>/rightArrow", null, null)
				.With("Negative", "<Keyboard>/leftArrow", null, null);
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600032D RID: 813 RVA: 0x0000E048 File Offset: 0x0000C248
		public static DebugManager instance
		{
			get
			{
				return DebugManager.s_Instance.Value;
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000E054 File Offset: 0x0000C254
		private void UpdateReadOnlyCollection()
		{
			this.m_Panels.Sort();
			this.m_ReadOnlyPanels = this.m_Panels.AsReadOnly();
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000E072 File Offset: 0x0000C272
		public ReadOnlyCollection<DebugUI.Panel> panels
		{
			get
			{
				if (this.m_ReadOnlyPanels == null)
				{
					this.UpdateReadOnlyCollection();
				}
				return this.m_ReadOnlyPanels;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000330 RID: 816 RVA: 0x0000E088 File Offset: 0x0000C288
		// (remove) Token: 0x06000331 RID: 817 RVA: 0x0000E0C0 File Offset: 0x0000C2C0
		public event Action<bool> onDisplayRuntimeUIChanged = delegate
		{
		};

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000332 RID: 818 RVA: 0x0000E0F8 File Offset: 0x0000C2F8
		// (remove) Token: 0x06000333 RID: 819 RVA: 0x0000E130 File Offset: 0x0000C330
		public event Action onSetDirty = delegate
		{
		};

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000334 RID: 820 RVA: 0x0000E168 File Offset: 0x0000C368
		// (remove) Token: 0x06000335 RID: 821 RVA: 0x0000E1A0 File Offset: 0x0000C3A0
		private event Action resetData;

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000E1D5 File Offset: 0x0000C3D5
		public bool isAnyDebugUIActive
		{
			get
			{
				return this.displayRuntimeUI || this.displayPersistentRuntimeUI;
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000E1E8 File Offset: 0x0000C3E8
		private DebugManager()
		{
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000E28B File Offset: 0x0000C48B
		public void RefreshEditor()
		{
			this.refreshEditorRequested = true;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000E294 File Offset: 0x0000C494
		public void Reset()
		{
			Action action = this.resetData;
			if (action != null)
			{
				action();
			}
			this.ReDrawOnScreenDebug();
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000E2AD File Offset: 0x0000C4AD
		public void ReDrawOnScreenDebug()
		{
			if (this.displayRuntimeUI)
			{
				DebugUIHandlerCanvas rootUICanvas = this.m_RootUICanvas;
				if (rootUICanvas == null)
				{
					return;
				}
				rootUICanvas.RequestHierarchyReset();
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000E2C7 File Offset: 0x0000C4C7
		public void RegisterData(IDebugData data)
		{
			this.resetData += data.GetReset();
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000E2D5 File Offset: 0x0000C4D5
		public void UnregisterData(IDebugData data)
		{
			this.resetData -= data.GetReset();
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000E2E4 File Offset: 0x0000C4E4
		public int GetState()
		{
			int num = 17;
			foreach (DebugUI.Panel panel in this.m_Panels)
			{
				num = num * 23 + panel.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000E340 File Offset: 0x0000C540
		internal void RegisterRootCanvas(DebugUIHandlerCanvas root)
		{
			this.m_Root = root.gameObject;
			this.m_RootUICanvas = root;
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000E355 File Offset: 0x0000C555
		internal void ChangeSelection(DebugUIHandlerWidget widget, bool fromNext)
		{
			this.m_RootUICanvas.ChangeSelection(widget, fromNext);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000E364 File Offset: 0x0000C564
		internal void SetScrollTarget(DebugUIHandlerWidget widget)
		{
			if (this.m_RootUICanvas != null)
			{
				this.m_RootUICanvas.SetScrollTarget(widget);
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000E380 File Offset: 0x0000C580
		private void EnsurePersistentCanvas()
		{
			if (this.m_RootUIPersistentCanvas == null)
			{
				DebugUIHandlerPersistentCanvas debugUIHandlerPersistentCanvas = Object.FindObjectOfType<DebugUIHandlerPersistentCanvas>();
				if (debugUIHandlerPersistentCanvas == null)
				{
					this.m_PersistentRoot = Object.Instantiate<Transform>(Resources.Load<Transform>("DebugUIPersistentCanvas")).gameObject;
					this.m_PersistentRoot.name = "[Debug Canvas - Persistent]";
					this.m_PersistentRoot.transform.localPosition = Vector3.zero;
				}
				else
				{
					this.m_PersistentRoot = debugUIHandlerPersistentCanvas.gameObject;
				}
				this.m_RootUIPersistentCanvas = this.m_PersistentRoot.GetComponent<DebugUIHandlerPersistentCanvas>();
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000E408 File Offset: 0x0000C608
		internal void TogglePersistent(DebugUI.Widget widget, int? forceTupleIndex = null)
		{
			if (widget == null)
			{
				return;
			}
			this.EnsurePersistentCanvas();
			DebugUI.Value value = widget as DebugUI.Value;
			if (value != null)
			{
				this.m_RootUIPersistentCanvas.Toggle(value, null);
				return;
			}
			DebugUI.ValueTuple valueTuple = widget as DebugUI.ValueTuple;
			if (valueTuple == null)
			{
				DebugUI.Container container = widget as DebugUI.Container;
				if (container != null)
				{
					int num = container.children.Max<DebugUI.Widget>(delegate(DebugUI.Widget w)
					{
						DebugUI.ValueTuple valueTuple2 = w as DebugUI.ValueTuple;
						if (valueTuple2 == null)
						{
							return -1;
						}
						return valueTuple2.pinnedElementIndex;
					});
					using (IEnumerator<DebugUI.Widget> enumerator = container.children.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DebugUI.Widget widget2 = enumerator.Current;
							if (widget2 is DebugUI.Value || widget2 is DebugUI.ValueTuple)
							{
								this.TogglePersistent(widget2, new int?(num));
							}
						}
						return;
					}
				}
				Debug.Log("Only readonly items can be made persistent.");
				return;
			}
			this.m_RootUIPersistentCanvas.Toggle(valueTuple, forceTupleIndex);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000E4F4 File Offset: 0x0000C6F4
		private void OnPanelDirty(DebugUI.Panel panel)
		{
			this.onSetDirty();
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000E504 File Offset: 0x0000C704
		public int PanelIndex([DisallowNull] string displayName)
		{
			if (displayName == null)
			{
				displayName = string.Empty;
			}
			for (int i = 0; i < this.m_Panels.Count; i++)
			{
				if (displayName.Equals(this.m_Panels[i].displayName, StringComparison.InvariantCultureIgnoreCase))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000E54E File Offset: 0x0000C74E
		public string PanelDiplayName([DisallowNull] int panelIndex)
		{
			if (panelIndex < 0 || panelIndex > this.m_Panels.Count - 1)
			{
				return string.Empty;
			}
			return this.m_Panels[panelIndex].displayName;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000E57B File Offset: 0x0000C77B
		public void RequestEditorWindowPanelIndex(int index)
		{
			this.m_RequestedPanelIndex = new int?(index);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000E589 File Offset: 0x0000C789
		internal int? GetRequestedEditorWindowPanelIndex()
		{
			int? requestedPanelIndex = this.m_RequestedPanelIndex;
			this.m_RequestedPanelIndex = null;
			return requestedPanelIndex;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000E5A0 File Offset: 0x0000C7A0
		public DebugUI.Panel GetPanel(string displayName, bool createIfNull = false, int groupIndex = 0, bool overrideIfExist = false)
		{
			int num = this.PanelIndex(displayName);
			DebugUI.Panel panel = ((num >= 0) ? this.m_Panels[num] : null);
			if (panel != null)
			{
				if (!overrideIfExist)
				{
					return panel;
				}
				panel.onSetDirty -= this.OnPanelDirty;
				this.RemovePanel(panel);
				panel = null;
			}
			if (createIfNull)
			{
				panel = new DebugUI.Panel
				{
					displayName = displayName,
					groupIndex = groupIndex
				};
				panel.onSetDirty += this.OnPanelDirty;
				this.m_Panels.Add(panel);
				this.UpdateReadOnlyCollection();
			}
			return panel;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000E62C File Offset: 0x0000C82C
		public int FindPanelIndex(string displayName)
		{
			return this.m_Panels.FindIndex((DebugUI.Panel p) => p.displayName == displayName);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000E660 File Offset: 0x0000C860
		public void RemovePanel(string displayName)
		{
			DebugUI.Panel panel = null;
			foreach (DebugUI.Panel panel2 in this.m_Panels)
			{
				if (panel2.displayName == displayName)
				{
					panel2.onSetDirty -= this.OnPanelDirty;
					panel = panel2;
					break;
				}
			}
			this.RemovePanel(panel);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000E6DC File Offset: 0x0000C8DC
		public void RemovePanel(DebugUI.Panel panel)
		{
			if (panel == null)
			{
				return;
			}
			this.m_Panels.Remove(panel);
			this.UpdateReadOnlyCollection();
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000E6F8 File Offset: 0x0000C8F8
		public DebugUI.Widget[] GetItems(DebugUI.Flags flags)
		{
			List<DebugUI.Widget> list;
			DebugUI.Widget[] array;
			using (ListPool<DebugUI.Widget>.Get(out list))
			{
				foreach (DebugUI.Panel panel in this.m_Panels)
				{
					DebugUI.Widget[] itemsFromContainer = this.GetItemsFromContainer(flags, panel);
					list.AddRange(itemsFromContainer);
				}
				array = list.ToArray();
			}
			return array;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000E784 File Offset: 0x0000C984
		internal DebugUI.Widget[] GetItemsFromContainer(DebugUI.Flags flags, DebugUI.IContainer container)
		{
			List<DebugUI.Widget> list;
			DebugUI.Widget[] array;
			using (ListPool<DebugUI.Widget>.Get(out list))
			{
				foreach (DebugUI.Widget widget in container.children)
				{
					if (widget.flags.HasFlag(flags))
					{
						list.Add(widget);
					}
					else
					{
						DebugUI.IContainer container2 = widget as DebugUI.IContainer;
						if (container2 != null)
						{
							list.AddRange(this.GetItemsFromContainer(flags, container2));
						}
					}
				}
				array = list.ToArray();
			}
			return array;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0000E834 File Offset: 0x0000CA34
		public DebugUI.Widget GetItem(string queryPath)
		{
			foreach (DebugUI.Panel panel in this.m_Panels)
			{
				DebugUI.Widget item = this.GetItem(queryPath, panel);
				if (item != null)
				{
					return item;
				}
			}
			return null;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0000E894 File Offset: 0x0000CA94
		private DebugUI.Widget GetItem(string queryPath, DebugUI.IContainer container)
		{
			foreach (DebugUI.Widget widget in container.children)
			{
				if (widget.queryPath == queryPath)
				{
					return widget;
				}
				DebugUI.IContainer container2 = widget as DebugUI.IContainer;
				if (container2 != null)
				{
					DebugUI.Widget item = this.GetItem(queryPath, container2);
					if (item != null)
					{
						return item;
					}
				}
			}
			return null;
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000350 RID: 848 RVA: 0x0000E90C File Offset: 0x0000CB0C
		// (remove) Token: 0x06000351 RID: 849 RVA: 0x0000E940 File Offset: 0x0000CB40
		public static event Action<DebugManager.UIMode, bool> windowStateChanged;

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000E973 File Offset: 0x0000CB73
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0000E980 File Offset: 0x0000CB80
		public bool displayEditorUI
		{
			get
			{
				return this.editorUIState.open;
			}
			set
			{
				this.editorUIState.open = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000354 RID: 852 RVA: 0x0000E98E File Offset: 0x0000CB8E
		// (set) Token: 0x06000355 RID: 853 RVA: 0x0000E996 File Offset: 0x0000CB96
		public bool enableRuntimeUI
		{
			get
			{
				return this.m_EnableRuntimeUI;
			}
			set
			{
				if (value != this.m_EnableRuntimeUI)
				{
					this.m_EnableRuntimeUI = value;
					DebugUpdater.SetEnabled(value);
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000E9AE File Offset: 0x0000CBAE
		// (set) Token: 0x06000357 RID: 855 RVA: 0x0000E9CC File Offset: 0x0000CBCC
		public bool displayRuntimeUI
		{
			get
			{
				return this.m_Root != null && this.m_Root.activeInHierarchy;
			}
			set
			{
				if (value)
				{
					this.m_Root = Object.Instantiate<Transform>(Resources.Load<Transform>("DebugUICanvas")).gameObject;
					this.m_Root.name = "[Debug Canvas]";
					this.m_Root.transform.localPosition = Vector3.zero;
					this.m_RootUICanvas = this.m_Root.GetComponent<DebugUIHandlerCanvas>();
					this.m_Root.SetActive(true);
				}
				else
				{
					CoreUtils.Destroy(this.m_Root);
					this.m_Root = null;
					this.m_RootUICanvas = null;
				}
				this.onDisplayRuntimeUIChanged(value);
				DebugUpdater.HandleInternalEventSystemComponents(value);
				this.runtimeUIState.open = this.m_Root != null && this.m_Root.activeInHierarchy;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0000EA8C File Offset: 0x0000CC8C
		// (set) Token: 0x06000359 RID: 857 RVA: 0x0000EAA9 File Offset: 0x0000CCA9
		public bool displayPersistentRuntimeUI
		{
			get
			{
				return this.m_RootUIPersistentCanvas != null && this.m_PersistentRoot.activeInHierarchy;
			}
			set
			{
				if (value)
				{
					this.EnsurePersistentCanvas();
					return;
				}
				CoreUtils.Destroy(this.m_PersistentRoot);
				this.m_PersistentRoot = null;
				this.m_RootUIPersistentCanvas = null;
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000EACE File Offset: 0x0000CCCE
		[Obsolete("Use DebugManager.instance.displayEditorUI.open property instead. #from(23.1)")]
		public void ToggleEditorUI(bool open)
		{
			this.editorUIState.open = open;
		}

		// Token: 0x040001C3 RID: 451
		private const string kEnableDebugBtn1 = "Enable Debug Button 1";

		// Token: 0x040001C4 RID: 452
		private const string kEnableDebugBtn2 = "Enable Debug Button 2";

		// Token: 0x040001C5 RID: 453
		private const string kDebugPreviousBtn = "Debug Previous";

		// Token: 0x040001C6 RID: 454
		private const string kDebugNextBtn = "Debug Next";

		// Token: 0x040001C7 RID: 455
		private const string kValidateBtn = "Debug Validate";

		// Token: 0x040001C8 RID: 456
		private const string kPersistentBtn = "Debug Persistent";

		// Token: 0x040001C9 RID: 457
		private const string kDPadVertical = "Debug Vertical";

		// Token: 0x040001CA RID: 458
		private const string kDPadHorizontal = "Debug Horizontal";

		// Token: 0x040001CB RID: 459
		private const string kMultiplierBtn = "Debug Multiplier";

		// Token: 0x040001CC RID: 460
		private const string kResetBtn = "Debug Reset";

		// Token: 0x040001CD RID: 461
		private const string kEnableDebug = "Enable Debug";

		// Token: 0x040001CE RID: 462
		private DebugActionDesc[] m_DebugActions;

		// Token: 0x040001CF RID: 463
		private DebugActionState[] m_DebugActionStates;

		// Token: 0x040001D0 RID: 464
		private InputActionMap debugActionMap = new InputActionMap("Debug Menu");

		// Token: 0x040001D1 RID: 465
		private static readonly Lazy<DebugManager> s_Instance = new Lazy<DebugManager>(() => new DebugManager());

		// Token: 0x040001D2 RID: 466
		private ReadOnlyCollection<DebugUI.Panel> m_ReadOnlyPanels;

		// Token: 0x040001D3 RID: 467
		private readonly List<DebugUI.Panel> m_Panels = new List<DebugUI.Panel>();

		// Token: 0x040001D7 RID: 471
		public bool refreshEditorRequested;

		// Token: 0x040001D8 RID: 472
		private int? m_RequestedPanelIndex;

		// Token: 0x040001D9 RID: 473
		private GameObject m_Root;

		// Token: 0x040001DA RID: 474
		private DebugUIHandlerCanvas m_RootUICanvas;

		// Token: 0x040001DB RID: 475
		private GameObject m_PersistentRoot;

		// Token: 0x040001DC RID: 476
		private DebugUIHandlerPersistentCanvas m_RootUIPersistentCanvas;

		// Token: 0x040001DE RID: 478
		private DebugManager.UIState editorUIState = new DebugManager.UIState
		{
			mode = DebugManager.UIMode.EditorMode
		};

		// Token: 0x040001DF RID: 479
		private bool m_EnableRuntimeUI = true;

		// Token: 0x040001E0 RID: 480
		private DebugManager.UIState runtimeUIState = new DebugManager.UIState
		{
			mode = DebugManager.UIMode.RuntimeMode
		};

		// Token: 0x02000168 RID: 360
		public enum UIMode
		{
			// Token: 0x0400060B RID: 1547
			EditorMode,
			// Token: 0x0400060C RID: 1548
			RuntimeMode
		}

		// Token: 0x02000169 RID: 361
		private class UIState
		{
			// Token: 0x1700014D RID: 333
			// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0002CF93 File Offset: 0x0002B193
			// (set) Token: 0x06000A02 RID: 2562 RVA: 0x0002CF9B File Offset: 0x0002B19B
			public bool open
			{
				get
				{
					return this.m_Open;
				}
				set
				{
					if (this.m_Open == value)
					{
						return;
					}
					this.m_Open = value;
					Action<DebugManager.UIMode, bool> windowStateChanged = DebugManager.windowStateChanged;
					if (windowStateChanged == null)
					{
						return;
					}
					windowStateChanged(this.mode, this.m_Open);
				}
			}

			// Token: 0x0400060D RID: 1549
			public DebugManager.UIMode mode;

			// Token: 0x0400060E RID: 1550
			[SerializeField]
			private bool m_Open;
		}
	}
}
