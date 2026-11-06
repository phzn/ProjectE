using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
	// Token: 0x02000266 RID: 614
	public class IMGUIContainer : VisualElement, IDisposable
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001173 RID: 4467 RVA: 0x0003F1CC File Offset: 0x0003D3CC
		// (set) Token: 0x06001174 RID: 4468 RVA: 0x0003F1E4 File Offset: 0x0003D3E4
		public Action onGUIHandler
		{
			get
			{
				return this.m_OnGUIHandler;
			}
			set
			{
				bool flag = this.m_OnGUIHandler != value;
				if (flag)
				{
					this.m_OnGUIHandler = value;
					base.IncrementVersion(VersionChangeType.Layout);
					base.IncrementVersion(VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x0003F220 File Offset: 0x0003D420
		internal ObjectGUIState guiState
		{
			get
			{
				Debug.Assert(!this.useOwnerObjectGUIState);
				bool flag = this.m_ObjectGUIState == null;
				if (flag)
				{
					this.m_ObjectGUIState = new ObjectGUIState();
				}
				return this.m_ObjectGUIState;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001176 RID: 4470 RVA: 0x0003F261 File Offset: 0x0003D461
		// (set) Token: 0x06001177 RID: 4471 RVA: 0x0003F269 File Offset: 0x0003D469
		internal Rect lastWorldClip { get; set; }

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06001178 RID: 4472 RVA: 0x0003F274 File Offset: 0x0003D474
		// (set) Token: 0x06001179 RID: 4473 RVA: 0x0003F28C File Offset: 0x0003D48C
		public bool cullingEnabled
		{
			get
			{
				return this.m_CullingEnabled;
			}
			set
			{
				this.m_CullingEnabled = value;
				base.IncrementVersion(VersionChangeType.Repaint);
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x0600117A RID: 4474 RVA: 0x0003F2A4 File Offset: 0x0003D4A4
		private GUILayoutUtility.LayoutCache cache
		{
			get
			{
				bool flag = this.m_Cache == null;
				if (flag)
				{
					this.m_Cache = new GUILayoutUtility.LayoutCache(-1);
				}
				return this.m_Cache;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x0600117B RID: 4475 RVA: 0x0003F2D8 File Offset: 0x0003D4D8
		private float layoutMeasuredWidth
		{
			get
			{
				return Mathf.Ceil(this.cache.topLevel.maxWidth);
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x0600117C RID: 4476 RVA: 0x0003F300 File Offset: 0x0003D500
		private float layoutMeasuredHeight
		{
			get
			{
				return Mathf.Ceil(this.cache.topLevel.maxHeight);
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x0003F327 File Offset: 0x0003D527
		// (set) Token: 0x0600117E RID: 4478 RVA: 0x0003F32F File Offset: 0x0003D52F
		public ContextType contextType { get; set; }

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x0600117F RID: 4479 RVA: 0x0003F338 File Offset: 0x0003D538
		// (set) Token: 0x06001180 RID: 4480 RVA: 0x0003F340 File Offset: 0x0003D540
		internal bool focusOnlyIfHasFocusableControls { get; set; } = true;

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x0003F349 File Offset: 0x0003D549
		public override bool canGrabFocus
		{
			get
			{
				return this.focusOnlyIfHasFocusableControls ? (this.hasFocusableControls && base.canGrabFocus) : base.canGrabFocus;
			}
		}

		// Token: 0x06001182 RID: 4482 RVA: 0x0003F36C File Offset: 0x0003D56C
		static IMGUIContainer()
		{
			for (int i = 0; i <= Foldout.ussFoldoutMaxDepth; i++)
			{
				IMGUIContainer.ussFoldoutChildDepthClassNames.Add(IMGUIContainer.ussFoldoutChildDepthClassName + i.ToString());
			}
			IMGUIContainer.ussFoldoutChildDepthClassNames.Add(IMGUIContainer.ussFoldoutChildDepthClassName + "max");
		}

		// Token: 0x06001183 RID: 4483 RVA: 0x0003F456 File Offset: 0x0003D656
		public IMGUIContainer()
			: this(null)
		{
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0003F464 File Offset: 0x0003D664
		public IMGUIContainer(Action onGUIHandler)
		{
			this.isIMGUIContainer = true;
			base.eventCallbackCategories |= 90166;
			base.AddToClassList(IMGUIContainer.ussClassName);
			this.onGUIHandler = onGUIHandler;
			this.contextType = ContextType.Editor;
			base.focusable = true;
			base.requireMeasureFunction = true;
			base.generateVisualContent = (Action<MeshGenerationContext>)Delegate.Combine(base.generateVisualContent, new Action<MeshGenerationContext>(this.OnGenerateVisualContent));
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0003F542 File Offset: 0x0003D742
		private void OnGenerateVisualContent(MeshGenerationContext mgc)
		{
			this.lastWorldClip = base.elementPanel.repaintData.currentWorldClip;
			mgc.painter.DrawImmediate(new Action(this.DoIMGUIRepaint), this.cullingEnabled);
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0003F57C File Offset: 0x0003D77C
		private void SaveGlobals()
		{
			this.m_GUIGlobals.matrix = GUI.matrix;
			this.m_GUIGlobals.color = GUI.color;
			this.m_GUIGlobals.contentColor = GUI.contentColor;
			this.m_GUIGlobals.backgroundColor = GUI.backgroundColor;
			this.m_GUIGlobals.enabled = GUI.enabled;
			this.m_GUIGlobals.changed = GUI.changed;
			bool flag = Event.current != null;
			if (flag)
			{
				this.m_GUIGlobals.displayIndex = Event.current.displayIndex;
			}
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0003F610 File Offset: 0x0003D810
		private void RestoreGlobals()
		{
			GUI.matrix = this.m_GUIGlobals.matrix;
			GUI.color = this.m_GUIGlobals.color;
			GUI.contentColor = this.m_GUIGlobals.contentColor;
			GUI.backgroundColor = this.m_GUIGlobals.backgroundColor;
			GUI.enabled = this.m_GUIGlobals.enabled;
			GUI.changed = this.m_GUIGlobals.changed;
			bool flag = Event.current != null;
			if (flag)
			{
				Event.current.displayIndex = this.m_GUIGlobals.displayIndex;
			}
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0003F6A8 File Offset: 0x0003D8A8
		private void DoOnGUI(Event evt, Matrix4x4 parentTransform, Rect clippingRect, bool isComputingLayout, Rect layoutSize, Action onGUIHandler, bool canAffectFocus = true)
		{
			bool flag = onGUIHandler == null || base.panel == null;
			if (!flag)
			{
				int num = GUIClip.Internal_GetCount();
				this.SaveGlobals();
				float layoutMeasuredWidth = this.layoutMeasuredWidth;
				float layoutMeasuredHeight = this.layoutMeasuredHeight;
				UIElementsUtility.BeginContainerGUI(this.cache, evt, this);
				GUI.color = UIElementsUtility.editorPlayModeTintColor;
				bool flag2 = Event.current.type != EventType.Layout;
				if (flag2)
				{
					bool flag3 = this.lostFocus;
					if (flag3)
					{
						bool flag4 = this.focusController != null;
						if (flag4)
						{
							bool flag5 = GUIUtility.OwnsId(GUIUtility.keyboardControl);
							if (flag5)
							{
								GUIUtility.keyboardControl = 0;
								this.focusController.imguiKeyboardControl = 0;
							}
						}
						this.lostFocus = false;
					}
					bool flag6 = this.receivedFocus;
					if (flag6)
					{
						bool flag7 = this.hasFocusableControls;
						if (flag7)
						{
							bool flag8 = this.focusChangeDirection != FocusChangeDirection.unspecified && this.focusChangeDirection != FocusChangeDirection.none;
							if (flag8)
							{
								bool flag9;
								if (Event.current.type == EventType.KeyDown)
								{
									char character = Event.current.character;
									flag9 = character == '\t' || character == '\u0019';
								}
								else
								{
									flag9 = false;
								}
								bool flag10 = flag9;
								if (flag10)
								{
									Event.current.Use();
								}
								bool flag11 = this.focusChangeDirection == VisualElementFocusChangeDirection.left;
								if (flag11)
								{
									GUIUtility.SetKeyboardControlToLastControlId();
								}
								else
								{
									bool flag12 = this.focusChangeDirection == VisualElementFocusChangeDirection.right;
									if (flag12)
									{
										GUIUtility.SetKeyboardControlToFirstControlId();
									}
								}
							}
							else
							{
								bool flag13 = GUIUtility.keyboardControl == 0 && this.m_IsFocusDelegated;
								if (flag13)
								{
									GUIUtility.SetKeyboardControlToFirstControlId();
								}
							}
						}
						bool flag14 = this.focusController != null;
						if (flag14)
						{
							bool flag15 = this.focusController.imguiKeyboardControl != GUIUtility.keyboardControl && this.focusChangeDirection != FocusChangeDirection.unspecified;
							if (flag15)
							{
								this.newKeyboardFocusControlID = GUIUtility.keyboardControl;
							}
							this.focusController.imguiKeyboardControl = GUIUtility.keyboardControl;
						}
						this.receivedFocus = false;
						this.focusChangeDirection = FocusChangeDirection.unspecified;
					}
				}
				EventType type = Event.current.type;
				bool flag16 = false;
				try
				{
					using (new GUIClip.ParentClipScope(parentTransform, clippingRect))
					{
						using (IMGUIContainer.k_OnGUIMarker.Auto())
						{
							onGUIHandler();
						}
					}
				}
				catch (Exception ex)
				{
					bool flag17 = type == EventType.Layout;
					if (!flag17)
					{
						throw;
					}
					flag16 = GUIUtility.IsExitGUIException(ex);
					bool flag18 = !flag16;
					if (flag18)
					{
						Debug.LogException(ex);
					}
				}
				finally
				{
					bool flag19 = Event.current.type != EventType.Layout && canAffectFocus;
					if (flag19)
					{
						bool flag20 = Event.current.type == EventType.Used;
						int keyboardControl = GUIUtility.keyboardControl;
						int num2 = GUIUtility.CheckForTabEvent(Event.current);
						bool flag21 = this.focusController != null;
						if (flag21)
						{
							bool flag22 = num2 < 0 && !flag20;
							if (flag22)
							{
								Focusable leafFocusedElement = this.focusController.GetLeafFocusedElement();
								Focusable focusable = this.focusController.FocusNextInDirection((num2 == -1) ? VisualElementFocusChangeDirection.right : VisualElementFocusChangeDirection.left);
								bool flag23 = leafFocusedElement == this;
								if (flag23)
								{
									bool flag24 = focusable == this;
									if (flag24)
									{
										bool flag25 = num2 == -2;
										if (flag25)
										{
											GUIUtility.SetKeyboardControlToLastControlId();
										}
										else
										{
											bool flag26 = num2 == -1;
											if (flag26)
											{
												GUIUtility.SetKeyboardControlToFirstControlId();
											}
										}
										this.newKeyboardFocusControlID = GUIUtility.keyboardControl;
										this.focusController.imguiKeyboardControl = GUIUtility.keyboardControl;
									}
									else
									{
										GUIUtility.keyboardControl = 0;
										this.focusController.imguiKeyboardControl = 0;
									}
								}
							}
							else
							{
								bool flag27 = num2 > 0 && !flag20;
								if (flag27)
								{
									this.focusController.imguiKeyboardControl = GUIUtility.keyboardControl;
									this.newKeyboardFocusControlID = GUIUtility.keyboardControl;
								}
								else
								{
									bool flag28 = num2 == 0;
									if (flag28)
									{
										bool flag29 = type == EventType.MouseDown && !this.focusOnlyIfHasFocusableControls;
										if (flag29)
										{
											this.focusController.SyncIMGUIFocus(GUIUtility.keyboardControl, this, true);
										}
										else
										{
											bool flag30 = keyboardControl != GUIUtility.keyboardControl || type == EventType.MouseDown;
											if (flag30)
											{
												this.focusController.SyncIMGUIFocus(GUIUtility.keyboardControl, this, false);
											}
											else
											{
												bool flag31 = GUIUtility.keyboardControl != this.focusController.imguiKeyboardControl;
												if (flag31)
												{
													this.newKeyboardFocusControlID = GUIUtility.keyboardControl;
													bool flag32 = this.focusController.GetLeafFocusedElement() == this;
													if (flag32)
													{
														this.focusController.imguiKeyboardControl = GUIUtility.keyboardControl;
													}
													else
													{
														this.focusController.SyncIMGUIFocus(GUIUtility.keyboardControl, this, false);
													}
												}
											}
										}
									}
								}
							}
						}
						this.hasFocusableControls = GUIUtility.HasFocusableControls();
					}
				}
				UIElementsUtility.EndContainerGUI(evt, layoutSize);
				this.RestoreGlobals();
				bool flag33 = evt.type == EventType.Layout && (!Mathf.Approximately(layoutMeasuredWidth, this.layoutMeasuredWidth) || !Mathf.Approximately(layoutMeasuredHeight, this.layoutMeasuredHeight));
				if (flag33)
				{
					bool flag34 = isComputingLayout && clippingRect == Rect.zero;
					if (flag34)
					{
						base.schedule.Execute(delegate
						{
							base.IncrementVersion(VersionChangeType.Layout);
						});
					}
					else
					{
						base.IncrementVersion(VersionChangeType.Layout);
					}
				}
				bool flag35 = !flag16;
				if (flag35)
				{
					bool flag36 = evt.type != EventType.Ignore && evt.type != EventType.Used;
					if (flag36)
					{
						int num3 = GUIClip.Internal_GetCount();
						bool flag37 = num3 > num;
						if (flag37)
						{
							Debug.LogError("GUI Error: You are pushing more GUIClips than you are popping. Make sure they are balanced.");
						}
						else
						{
							bool flag38 = num3 < num;
							if (flag38)
							{
								Debug.LogError("GUI Error: You are popping more GUIClips than you are pushing. Make sure they are balanced.");
							}
						}
					}
				}
				while (GUIClip.Internal_GetCount() > num)
				{
					GUIClip.Internal_Pop();
				}
				bool flag39 = evt.type == EventType.Used;
				if (flag39)
				{
					base.IncrementVersion(VersionChangeType.Repaint);
				}
			}
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0003FCF4 File Offset: 0x0003DEF4
		public void MarkDirtyLayout()
		{
			this.m_RefreshCachedLayout = true;
			base.IncrementVersion(VersionChangeType.Layout);
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0003FD08 File Offset: 0x0003DF08
		internal void ProcessEvent(EventBase evt)
		{
			bool flag = (evt.imguiEvent != null && this.SendEventToIMGUI(evt, true, true)) || evt.eventTypeId == EventBase<NavigationMoveEvent>.TypeId() || evt.eventTypeId == EventBase<NavigationSubmitEvent>.TypeId() || evt.eventTypeId == EventBase<NavigationCancelEvent>.TypeId();
			if (flag)
			{
				evt.StopPropagation();
				evt.PreventDefault();
			}
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0003FD68 File Offset: 0x0003DF68
		private void DoIMGUIRepaint()
		{
			using (IMGUIContainer.k_ImmediateCallbackMarker.Auto())
			{
				Matrix4x4 currentOffset = base.elementPanel.repaintData.currentOffset;
				this.m_CachedClippingRect = VisualElement.ComputeAAAlignedBound(base.worldClip, currentOffset);
				this.m_CachedTransform = currentOffset * base.worldTransform;
				this.HandleIMGUIEvent(base.elementPanel.repaintData.repaintEvent, this.m_CachedTransform, this.m_CachedClippingRect, this.onGUIHandler, true);
			}
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x0003FE08 File Offset: 0x0003E008
		internal bool SendEventToIMGUI(EventBase evt, bool canAffectFocus = true, bool verifyBounds = true)
		{
			bool flag = evt is IPointerEvent;
			bool flag12;
			if (flag)
			{
				bool flag2 = evt.imguiEvent != null && evt.imguiEvent.isDirectManipulationDevice;
				if (flag2)
				{
					bool flag3 = false;
					EventType rawType = evt.imguiEvent.rawType;
					bool flag4 = evt is PointerDownEvent;
					if (flag4)
					{
						flag3 = true;
						evt.imguiEvent.type = EventType.TouchDown;
					}
					else
					{
						bool flag5 = evt is PointerUpEvent;
						if (flag5)
						{
							flag3 = true;
							evt.imguiEvent.type = EventType.TouchUp;
						}
						else
						{
							bool flag6 = evt is PointerMoveEvent && evt.imguiEvent.rawType == EventType.MouseDrag;
							if (flag6)
							{
								flag3 = true;
								evt.imguiEvent.type = EventType.TouchMove;
							}
							else
							{
								bool flag7 = evt is PointerLeaveEvent;
								if (flag7)
								{
									flag3 = true;
									evt.imguiEvent.type = EventType.TouchLeave;
								}
								else
								{
									bool flag8 = evt is PointerEnterEvent;
									if (flag8)
									{
										flag3 = true;
										evt.imguiEvent.type = EventType.TouchEnter;
									}
									else
									{
										bool flag9 = evt is PointerStationaryEvent;
										if (flag9)
										{
											flag3 = true;
											evt.imguiEvent.type = EventType.TouchStationary;
										}
									}
								}
							}
						}
					}
					bool flag10 = flag3;
					if (flag10)
					{
						bool flag11 = this.SendEventToIMGUIRaw(evt, canAffectFocus, verifyBounds);
						evt.imguiEvent.type = rawType;
						return flag11;
					}
				}
				flag12 = false;
			}
			else
			{
				flag12 = this.SendEventToIMGUIRaw(evt, canAffectFocus, verifyBounds);
			}
			return flag12;
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0003FF74 File Offset: 0x0003E174
		private bool SendEventToIMGUIRaw(EventBase evt, bool canAffectFocus, bool verifyBounds)
		{
			bool flag = verifyBounds && !this.VerifyBounds(evt);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3;
				using (new EventDebuggerLogIMGUICall(evt))
				{
					flag3 = this.HandleIMGUIEvent(evt.imguiEvent, canAffectFocus);
				}
				flag2 = flag3;
			}
			return flag2;
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0003FFD8 File Offset: 0x0003E1D8
		private bool VerifyBounds(EventBase evt)
		{
			return this.IsContainerCapturingTheMouse() || !this.IsLocalEvent(evt) || this.IsEventInsideLocalWindow(evt) || IMGUIContainer.IsDockAreaMouseUp(evt);
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x00040010 File Offset: 0x0003E210
		private bool IsContainerCapturingTheMouse()
		{
			IPanel panel = base.panel;
			IMGUIContainer imguicontainer;
			if (panel == null)
			{
				imguicontainer = null;
			}
			else
			{
				EventDispatcher dispatcher = panel.dispatcher;
				imguicontainer = ((dispatcher != null) ? dispatcher.pointerState.GetCapturingElement(PointerId.mousePointerId) : null);
			}
			return this == imguicontainer;
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x00040050 File Offset: 0x0003E250
		private bool IsLocalEvent(EventBase evt)
		{
			long eventTypeId = evt.eventTypeId;
			return eventTypeId == EventBase<MouseDownEvent>.TypeId() || eventTypeId == EventBase<MouseUpEvent>.TypeId() || eventTypeId == EventBase<MouseMoveEvent>.TypeId() || eventTypeId == EventBase<PointerDownEvent>.TypeId() || eventTypeId == EventBase<PointerUpEvent>.TypeId() || eventTypeId == EventBase<PointerMoveEvent>.TypeId();
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0004009C File Offset: 0x0003E29C
		private bool IsEventInsideLocalWindow(EventBase evt)
		{
			Rect currentClipRect = this.GetCurrentClipRect();
			IPointerEvent pointerEvent = evt as IPointerEvent;
			string text = ((pointerEvent != null) ? pointerEvent.pointerType : null);
			bool flag = text == PointerType.touch || text == PointerType.pen;
			return GUIUtility.HitTest(currentClipRect, evt.originalMousePosition, flag);
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x000400F4 File Offset: 0x0003E2F4
		private static bool IsDockAreaMouseUp(EventBase evt)
		{
			bool flag;
			if (evt.eventTypeId == EventBase<MouseUpEvent>.TypeId())
			{
				IMGUIContainer target = evt.target;
				VisualElement visualElement = evt.target as VisualElement;
				flag = target == ((visualElement != null) ? visualElement.elementPanel.rootIMGUIContainer : null);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0004013C File Offset: 0x0003E33C
		private bool HandleIMGUIEvent(Event e, bool canAffectFocus)
		{
			return this.HandleIMGUIEvent(e, this.onGUIHandler, canAffectFocus);
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0004015C File Offset: 0x0003E35C
		internal bool HandleIMGUIEvent(Event e, Action onGUIHandler, bool canAffectFocus)
		{
			IMGUIContainer.GetCurrentTransformAndClip(this, e, out this.m_CachedTransform, out this.m_CachedClippingRect);
			return this.HandleIMGUIEvent(e, this.m_CachedTransform, this.m_CachedClippingRect, onGUIHandler, canAffectFocus);
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x00040198 File Offset: 0x0003E398
		private bool HandleIMGUIEvent(Event e, Matrix4x4 worldTransform, Rect clippingRect, Action onGUIHandler, bool canAffectFocus)
		{
			bool flag = e == null || onGUIHandler == null || base.elementPanel == null || !base.elementPanel.IMGUIEventInterests.WantsEvent(e.rawType);
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				EventType rawType = e.rawType;
				bool flag3 = rawType != EventType.Layout;
				if (flag3)
				{
					bool flag4 = this.m_RefreshCachedLayout || base.elementPanel.IMGUIEventInterests.WantsLayoutPass(e.rawType);
					if (flag4)
					{
						e.type = EventType.Layout;
						this.DoOnGUI(e, worldTransform, clippingRect, false, base.layout, onGUIHandler, canAffectFocus);
						this.m_RefreshCachedLayout = false;
						e.type = rawType;
					}
					else
					{
						this.cache.ResetCursor();
					}
				}
				this.DoOnGUI(e, worldTransform, clippingRect, false, base.layout, onGUIHandler, canAffectFocus);
				bool flag5 = this.newKeyboardFocusControlID > 0;
				if (flag5)
				{
					this.newKeyboardFocusControlID = 0;
					Event @event = new Event
					{
						type = EventType.ExecuteCommand,
						commandName = "NewKeyboardFocus"
					};
					this.HandleIMGUIEvent(@event, true);
				}
				bool flag6 = e.rawType == EventType.Used;
				if (flag6)
				{
					flag2 = true;
				}
				else
				{
					bool flag7 = e.rawType == EventType.MouseUp && this.HasMouseCapture();
					if (flag7)
					{
						GUIUtility.hotControl = 0;
					}
					bool flag8 = base.elementPanel == null;
					if (flag8)
					{
						GUIUtility.ExitGUI();
					}
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00040304 File Offset: 0x0003E504
		[EventInterest(new Type[]
		{
			typeof(BlurEvent),
			typeof(FocusEvent),
			typeof(DetachFromPanelEvent),
			typeof(AttachToPanelEvent)
		})]
		protected override void ExecuteDefaultAction(EventBase evt)
		{
			bool flag = evt == null;
			if (!flag)
			{
				bool flag2 = evt.eventTypeId == EventBase<BlurEvent>.TypeId();
				if (flag2)
				{
					this.lostFocus = true;
					base.IncrementVersion(VersionChangeType.Repaint);
				}
				else
				{
					bool flag3 = evt.eventTypeId == EventBase<FocusEvent>.TypeId();
					if (flag3)
					{
						FocusEvent focusEvent = evt as FocusEvent;
						this.receivedFocus = true;
						this.focusChangeDirection = focusEvent.direction;
						this.m_IsFocusDelegated = focusEvent.IsFocusDelegated;
					}
					else
					{
						bool flag4 = evt.eventTypeId == EventBase<DetachFromPanelEvent>.TypeId();
						if (flag4)
						{
							bool flag5 = base.elementPanel != null;
							if (flag5)
							{
								BaseVisualElementPanel elementPanel = base.elementPanel;
								int num = elementPanel.IMGUIContainersCount;
								elementPanel.IMGUIContainersCount = num - 1;
							}
						}
						else
						{
							bool flag6 = evt.eventTypeId == EventBase<AttachToPanelEvent>.TypeId();
							if (flag6)
							{
								bool flag7 = base.elementPanel != null;
								if (flag7)
								{
									BaseVisualElementPanel elementPanel2 = base.elementPanel;
									int num = elementPanel2.IMGUIContainersCount;
									elementPanel2.IMGUIContainersCount = num + 1;
									this.SetFoldoutDepthClass();
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x00040410 File Offset: 0x0003E610
		private void SetFoldoutDepthClass()
		{
			for (int i = 0; i < IMGUIContainer.ussFoldoutChildDepthClassNames.Count; i++)
			{
				base.RemoveFromClassList(IMGUIContainer.ussFoldoutChildDepthClassNames[i]);
			}
			int num = this.GetFoldoutDepth();
			bool flag = num == 0;
			if (!flag)
			{
				num = Mathf.Min(num, IMGUIContainer.ussFoldoutChildDepthClassNames.Count - 1);
				base.AddToClassList(IMGUIContainer.ussFoldoutChildDepthClassNames[num]);
			}
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00040484 File Offset: 0x0003E684
		protected internal override Vector2 DoMeasure(float desiredWidth, VisualElement.MeasureMode widthMode, float desiredHeight, VisualElement.MeasureMode heightMode)
		{
			float num = float.NaN;
			float num2 = float.NaN;
			bool flag = false;
			bool flag2 = widthMode != VisualElement.MeasureMode.Exactly || heightMode != VisualElement.MeasureMode.Exactly;
			if (flag2)
			{
				bool flag3 = Event.current != null;
				if (flag3)
				{
					IMGUIContainer.s_CurrentEvent.CopyFrom(Event.current);
					flag = true;
				}
				IMGUIContainer.s_MeasureEvent.CopyFrom(IMGUIContainer.s_DefaultMeasureEvent);
				Rect layout = base.layout;
				if (widthMode == VisualElement.MeasureMode.Exactly)
				{
					layout.width = desiredWidth;
				}
				if (heightMode == VisualElement.MeasureMode.Exactly)
				{
					layout.height = desiredHeight;
				}
				this.DoOnGUI(IMGUIContainer.s_MeasureEvent, this.m_CachedTransform, this.m_CachedClippingRect, true, layout, this.onGUIHandler, true);
				num = this.layoutMeasuredWidth;
				num2 = this.layoutMeasuredHeight;
				bool flag4 = flag;
				if (flag4)
				{
					Event.current.CopyFrom(IMGUIContainer.s_CurrentEvent);
				}
			}
			if (widthMode != VisualElement.MeasureMode.Exactly)
			{
				if (widthMode == VisualElement.MeasureMode.AtMost)
				{
					num = Mathf.Min(num, desiredWidth);
				}
			}
			else
			{
				num = desiredWidth;
			}
			if (heightMode != VisualElement.MeasureMode.Exactly)
			{
				if (heightMode == VisualElement.MeasureMode.AtMost)
				{
					num2 = Mathf.Min(num2, desiredHeight);
				}
			}
			else
			{
				num2 = desiredHeight;
			}
			return new Vector2(num, num2);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x000405C0 File Offset: 0x0003E7C0
		private Rect GetCurrentClipRect()
		{
			Rect rect = this.lastWorldClip;
			bool flag = rect.width == 0f || rect.height == 0f;
			if (flag)
			{
				rect = base.worldBound;
			}
			return rect;
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00040608 File Offset: 0x0003E808
		private static void GetCurrentTransformAndClip(IMGUIContainer container, Event evt, out Matrix4x4 transform, out Rect clipRect)
		{
			clipRect = container.GetCurrentClipRect();
			transform = container.worldTransform;
			bool flag = evt != null && evt.rawType == EventType.Repaint && container.elementPanel != null;
			if (flag)
			{
				transform = container.elementPanel.repaintData.currentOffset * container.worldTransform;
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0004066D File Offset: 0x0003E86D
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00040680 File Offset: 0x0003E880
		protected virtual void Dispose(bool disposeManaged)
		{
			if (disposeManaged)
			{
				ObjectGUIState objectGUIState = this.m_ObjectGUIState;
				if (objectGUIState != null)
				{
					objectGUIState.Dispose();
				}
			}
		}

		// Token: 0x040007AA RID: 1962
		private Action m_OnGUIHandler;

		// Token: 0x040007AB RID: 1963
		private ObjectGUIState m_ObjectGUIState;

		// Token: 0x040007AC RID: 1964
		internal bool useOwnerObjectGUIState;

		// Token: 0x040007AE RID: 1966
		private bool m_CullingEnabled = false;

		// Token: 0x040007AF RID: 1967
		private bool m_IsFocusDelegated = false;

		// Token: 0x040007B0 RID: 1968
		private bool m_RefreshCachedLayout = true;

		// Token: 0x040007B1 RID: 1969
		private GUILayoutUtility.LayoutCache m_Cache = null;

		// Token: 0x040007B2 RID: 1970
		private Rect m_CachedClippingRect = Rect.zero;

		// Token: 0x040007B3 RID: 1971
		private Matrix4x4 m_CachedTransform = Matrix4x4.identity;

		// Token: 0x040007B5 RID: 1973
		private bool lostFocus = false;

		// Token: 0x040007B6 RID: 1974
		private bool receivedFocus = false;

		// Token: 0x040007B7 RID: 1975
		private FocusChangeDirection focusChangeDirection = FocusChangeDirection.unspecified;

		// Token: 0x040007B8 RID: 1976
		private bool hasFocusableControls = false;

		// Token: 0x040007B9 RID: 1977
		private int newKeyboardFocusControlID = 0;

		// Token: 0x040007BB RID: 1979
		public static readonly string ussClassName = "unity-imgui-container";

		// Token: 0x040007BC RID: 1980
		internal static readonly string ussFoldoutChildDepthClassName = Foldout.ussClassName + "__" + IMGUIContainer.ussClassName + "--depth-";

		// Token: 0x040007BD RID: 1981
		internal static readonly List<string> ussFoldoutChildDepthClassNames = new List<string>(Foldout.ussFoldoutMaxDepth + 1);

		// Token: 0x040007BE RID: 1982
		private IMGUIContainer.GUIGlobals m_GUIGlobals;

		// Token: 0x040007BF RID: 1983
		private static readonly ProfilerMarker k_OnGUIMarker = new ProfilerMarker("OnGUI");

		// Token: 0x040007C0 RID: 1984
		private static readonly ProfilerMarker k_ImmediateCallbackMarker = new ProfilerMarker("IMGUIContainer");

		// Token: 0x040007C1 RID: 1985
		private static Event s_DefaultMeasureEvent = new Event
		{
			type = EventType.Layout
		};

		// Token: 0x040007C2 RID: 1986
		private static Event s_MeasureEvent = new Event
		{
			type = EventType.Layout
		};

		// Token: 0x040007C3 RID: 1987
		private static Event s_CurrentEvent = new Event
		{
			type = EventType.Layout
		};

		// Token: 0x02000267 RID: 615
		public new class UxmlFactory : UxmlFactory<IMGUIContainer, IMGUIContainer.UxmlTraits>
		{
		}

		// Token: 0x02000268 RID: 616
		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			// Token: 0x0600119F RID: 4511 RVA: 0x000406BA File Offset: 0x0003E8BA
			public UxmlTraits()
			{
				base.focusIndex.defaultValue = 0;
				base.focusable.defaultValue = true;
			}

			// Token: 0x170003B8 RID: 952
			// (get) Token: 0x060011A0 RID: 4512 RVA: 0x000406E0 File Offset: 0x0003E8E0
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get
				{
					yield break;
				}
			}
		}

		// Token: 0x0200026A RID: 618
		private struct GUIGlobals
		{
			// Token: 0x040007C8 RID: 1992
			public Matrix4x4 matrix;

			// Token: 0x040007C9 RID: 1993
			public Color color;

			// Token: 0x040007CA RID: 1994
			public Color contentColor;

			// Token: 0x040007CB RID: 1995
			public Color backgroundColor;

			// Token: 0x040007CC RID: 1996
			public bool enabled;

			// Token: 0x040007CD RID: 1997
			public bool changed;

			// Token: 0x040007CE RID: 1998
			public int displayIndex;
		}
	}
}
