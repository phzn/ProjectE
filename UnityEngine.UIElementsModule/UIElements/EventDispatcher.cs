using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace UnityEngine.UIElements
{
	// Token: 0x02000184 RID: 388
	public sealed class EventDispatcher
	{
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000C4B RID: 3147 RVA: 0x000313CD File Offset: 0x0002F5CD
		internal PointerDispatchState pointerState { get; } = new PointerDispatchState();

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x000313D5 File Offset: 0x0002F5D5
		internal uint GateDepth
		{
			get
			{
				return this.m_GateDepth;
			}
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x000313E0 File Offset: 0x0002F5E0
		internal static EventDispatcher CreateDefault()
		{
			return new EventDispatcher(EventDispatcher.s_EditorStrategies);
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x000313FC File Offset: 0x0002F5FC
		internal static EventDispatcher CreateForRuntime(IList<IEventDispatchingStrategy> strategies)
		{
			return new EventDispatcher(strategies);
		}

		// Token: 0x06000C4F RID: 3151 RVA: 0x00031414 File Offset: 0x0002F614
		[Obsolete("Please use EventDispatcher.CreateDefault().")]
		internal EventDispatcher()
			: this(EventDispatcher.s_EditorStrategies)
		{
		}

		// Token: 0x06000C50 RID: 3152 RVA: 0x00031424 File Offset: 0x0002F624
		private EventDispatcher(IList<IEventDispatchingStrategy> strategies)
		{
			this.m_DispatchingStrategies = new List<IEventDispatchingStrategy>();
			this.m_DispatchingStrategies.AddRange(strategies);
			this.m_Queue = EventDispatcher.k_EventQueuePool.Get();
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000C51 RID: 3153 RVA: 0x00031498 File Offset: 0x0002F698
		private bool dispatchImmediately
		{
			get
			{
				return this.m_Immediate || this.m_GateCount == 0U;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x000314BE File Offset: 0x0002F6BE
		// (set) Token: 0x06000C53 RID: 3155 RVA: 0x000314C6 File Offset: 0x0002F6C6
		internal bool processingEvents { get; private set; }

		// Token: 0x06000C54 RID: 3156 RVA: 0x000314D0 File Offset: 0x0002F6D0
		internal void Dispatch(EventBase evt, [NotNull] IPanel panel, DispatchMode dispatchMode)
		{
			evt.MarkReceivedByDispatcher();
			bool flag = evt.eventTypeId == EventBase<IMGUIEvent>.TypeId();
			if (flag)
			{
				Event imguiEvent = evt.imguiEvent;
				bool flag2 = imguiEvent.rawType == EventType.Repaint;
				if (flag2)
				{
					return;
				}
			}
			bool flag3 = this.dispatchImmediately || dispatchMode == DispatchMode.Immediate;
			if (flag3)
			{
				this.ProcessEvent(evt, panel);
			}
			else
			{
				bool flag4 = this.HandleRecursiveState(evt);
				if (!flag4)
				{
					evt.Acquire();
					this.m_Queue.Enqueue(new EventDispatcher.EventRecord
					{
						m_Event = evt,
						m_Panel = panel
					});
				}
			}
		}

		// Token: 0x06000C55 RID: 3157 RVA: 0x00031570 File Offset: 0x0002F770
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private bool HandleRecursiveState(EventBase evt)
		{
			bool flag = this.m_GateDepth <= 400U;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this.m_DispatchStackFrame != 0;
				if (flag3)
				{
					StackTrace stackTrace = new StackTrace(1, true);
					StringBuilder stringBuilder = new StringBuilder();
					int num = stackTrace.FrameCount - this.m_DispatchStackFrame;
					stringBuilder.AppendLine(string.Format("Recursively dispatching event {0} from another event {1} (depth = {2})", evt, this.m_CurrentEvent, this.m_GateDepth));
					for (int i = 0; i < num; i++)
					{
						StackFrame frame = stackTrace.GetFrame(i);
						stringBuilder.Append(frame.GetMethod()).AppendFormat("({0}:{1}", frame.GetFileName(), frame.GetFileLineNumber()).AppendLine(")");
					}
					Debug.LogFormat(LogType.Error, LogOption.NoStacktrace, null, stringBuilder.ToString(), Array.Empty<object>());
				}
				else
				{
					Debug.LogFormat(LogType.Error, LogOption.NoStacktrace, null, string.Format("Recursively dispatching event {0} from another event {1} (depth = {2})", evt, this.m_CurrentEvent, this.m_GateDepth), Array.Empty<object>());
				}
				bool flag4 = this.m_GateDepth > 500U;
				if (flag4)
				{
					Debug.LogErrorFormat("Ignoring event {0}: too many events dispatched recurively", new object[] { evt });
					flag2 = true;
				}
				else
				{
					flag2 = false;
				}
			}
			return flag2;
		}

		// Token: 0x06000C56 RID: 3158 RVA: 0x000316BC File Offset: 0x0002F8BC
		internal void PushDispatcherContext()
		{
			this.ProcessEventQueue();
			this.m_DispatchContexts.Push(new EventDispatcher.DispatchContext
			{
				m_GateCount = this.m_GateCount,
				m_Queue = this.m_Queue
			});
			this.m_GateCount = 0U;
			this.m_Queue = EventDispatcher.k_EventQueuePool.Get();
		}

		// Token: 0x06000C57 RID: 3159 RVA: 0x00031718 File Offset: 0x0002F918
		internal void PopDispatcherContext()
		{
			Debug.Assert(this.m_GateCount == 0U, "All gates should have been opened before popping dispatch context.");
			Debug.Assert(this.m_Queue.Count == 0, "Queue should be empty when popping dispatch context.");
			EventDispatcher.k_EventQueuePool.Release(this.m_Queue);
			this.m_GateCount = this.m_DispatchContexts.Peek().m_GateCount;
			this.m_Queue = this.m_DispatchContexts.Peek().m_Queue;
			this.m_DispatchContexts.Pop();
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0003179C File Offset: 0x0002F99C
		internal void CloseGate()
		{
			this.m_GateCount += 1U;
			this.m_GateDepth += 1U;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x000317BC File Offset: 0x0002F9BC
		internal void OpenGate()
		{
			Debug.Assert(this.m_GateCount > 0U);
			bool flag = this.m_GateCount > 0U;
			if (flag)
			{
				this.m_GateCount -= 1U;
			}
			try
			{
				bool flag2 = this.m_GateCount == 0U;
				if (flag2)
				{
					this.ProcessEventQueue();
				}
			}
			finally
			{
				Debug.Assert(this.m_GateDepth > 0U, "m_GateDepth > 0");
				bool flag3 = this.m_GateDepth > 0U;
				if (flag3)
				{
					this.m_GateDepth -= 1U;
				}
			}
		}

		// Token: 0x06000C5A RID: 3162 RVA: 0x00031858 File Offset: 0x0002FA58
		private void ProcessEventQueue()
		{
			Queue<EventDispatcher.EventRecord> queue = this.m_Queue;
			this.m_Queue = EventDispatcher.k_EventQueuePool.Get();
			ExitGUIException ex = null;
			try
			{
				this.processingEvents = true;
				while (queue.Count > 0)
				{
					EventDispatcher.EventRecord eventRecord = queue.Dequeue();
					EventBase @event = eventRecord.m_Event;
					IPanel panel = eventRecord.m_Panel;
					try
					{
						this.ProcessEvent(@event, panel);
					}
					catch (ExitGUIException ex2)
					{
						Debug.Assert(ex == null);
						ex = ex2;
					}
					finally
					{
						@event.Dispose();
					}
				}
			}
			finally
			{
				this.processingEvents = false;
				EventDispatcher.k_EventQueuePool.Release(queue);
			}
			bool flag = ex != null;
			if (flag)
			{
				throw ex;
			}
		}

		// Token: 0x06000C5B RID: 3163 RVA: 0x00031930 File Offset: 0x0002FB30
		private void ProcessEvent(EventBase evt, [NotNull] IPanel panel)
		{
			Event imguiEvent = evt.imguiEvent;
			bool flag = imguiEvent != null && imguiEvent.rawType == EventType.Used;
			using (new EventDispatcherGate(this))
			{
				evt.PreDispatch(panel);
				try
				{
					this.m_CurrentEvent = evt;
					this.m_DispatchStackFrame = ((this.m_GateDepth > 490U) ? new StackTrace().FrameCount : 0);
					bool flag2 = !evt.stopDispatch && !evt.isPropagationStopped;
					if (flag2)
					{
						this.ApplyDispatchingStrategies(evt, panel, flag);
					}
					PropagationPaths propagationPaths = evt.path;
					VisualElement visualElement;
					bool flag3;
					if (propagationPaths == null && evt.bubblesOrTricklesDown)
					{
						visualElement = evt.leafTarget as VisualElement;
						flag3 = visualElement != null;
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						propagationPaths = PropagationPaths.Build(visualElement, evt);
						evt.path = propagationPaths;
						EventDebugger.LogPropagationPaths(evt, propagationPaths);
					}
					bool flag5 = propagationPaths != null;
					if (flag5)
					{
						foreach (VisualElement visualElement2 in propagationPaths.targetElements)
						{
							bool flag6 = visualElement2.panel == panel;
							if (flag6)
							{
								evt.target = visualElement2;
								EventDispatchUtilities.ExecuteDefaultAction(evt);
							}
						}
						evt.target = evt.leafTarget;
					}
					else
					{
						VisualElement visualElement3 = evt.target as VisualElement;
						bool flag7 = visualElement3 == null;
						if (flag7)
						{
							visualElement3 = (evt.target = panel.visualTree);
						}
						bool flag8 = visualElement3.panel == panel;
						if (flag8)
						{
							EventDispatchUtilities.ExecuteDefaultAction(evt);
						}
					}
				}
				finally
				{
					this.m_CurrentEvent = null;
				}
				evt.PostDispatch(panel);
				this.m_ClickDetector.ProcessEvent(evt);
				Debug.Assert(flag || evt.isPropagationStopped || imguiEvent == null || imguiEvent.rawType != EventType.Used, "Event is used but not stopped.");
			}
		}

		// Token: 0x06000C5C RID: 3164 RVA: 0x00031B60 File Offset: 0x0002FD60
		private void ApplyDispatchingStrategies(EventBase evt, IPanel panel, bool imguiEventIsInitiallyUsed)
		{
			foreach (IEventDispatchingStrategy eventDispatchingStrategy in this.m_DispatchingStrategies)
			{
				bool flag = eventDispatchingStrategy.CanDispatchEvent(evt);
				if (flag)
				{
					eventDispatchingStrategy.DispatchEvent(evt, panel);
					Debug.Assert(imguiEventIsInitiallyUsed || evt.isPropagationStopped || evt.imguiEvent == null || evt.imguiEvent.rawType != EventType.Used, "Unexpected condition: !evt.isPropagationStopped && evt.imguiEvent.rawType == EventType.Used.");
					bool flag2 = evt.stopDispatch || evt.isPropagationStopped;
					if (flag2)
					{
						break;
					}
				}
			}
		}

		// Token: 0x040005DD RID: 1501
		internal ClickDetector m_ClickDetector = new ClickDetector();

		// Token: 0x040005DE RID: 1502
		private List<IEventDispatchingStrategy> m_DispatchingStrategies;

		// Token: 0x040005DF RID: 1503
		private static readonly ObjectPool<Queue<EventDispatcher.EventRecord>> k_EventQueuePool = new ObjectPool<Queue<EventDispatcher.EventRecord>>(() => new Queue<EventDispatcher.EventRecord>(), 100);

		// Token: 0x040005E0 RID: 1504
		private Queue<EventDispatcher.EventRecord> m_Queue;

		// Token: 0x040005E2 RID: 1506
		private uint m_GateCount;

		// Token: 0x040005E3 RID: 1507
		private uint m_GateDepth = 0U;

		// Token: 0x040005E4 RID: 1508
		internal const int k_MaxGateDepth = 500;

		// Token: 0x040005E5 RID: 1509
		internal const int k_NumberOfEventsWithStackInfo = 10;

		// Token: 0x040005E6 RID: 1510
		internal const int k_NumberOfEventsWithEventInfo = 100;

		// Token: 0x040005E7 RID: 1511
		private int m_DispatchStackFrame = 0;

		// Token: 0x040005E8 RID: 1512
		private EventBase m_CurrentEvent;

		// Token: 0x040005E9 RID: 1513
		private Stack<EventDispatcher.DispatchContext> m_DispatchContexts = new Stack<EventDispatcher.DispatchContext>();

		// Token: 0x040005EA RID: 1514
		private static readonly IEventDispatchingStrategy[] s_EditorStrategies = new IEventDispatchingStrategy[]
		{
			new PointerCaptureDispatchingStrategy(),
			new MouseCaptureDispatchingStrategy(),
			new KeyboardEventDispatchingStrategy(),
			new PointerEventDispatchingStrategy(),
			new MouseEventDispatchingStrategy(),
			new NavigationEventDispatchingStrategy(),
			new CommandEventDispatchingStrategy(),
			new IMGUIEventDispatchingStrategy(),
			new DefaultDispatchingStrategy()
		};

		// Token: 0x040005EB RID: 1515
		private bool m_Immediate = false;

		// Token: 0x02000185 RID: 389
		private struct EventRecord
		{
			// Token: 0x040005ED RID: 1517
			public EventBase m_Event;

			// Token: 0x040005EE RID: 1518
			public IPanel m_Panel;
		}

		// Token: 0x02000186 RID: 390
		private struct DispatchContext
		{
			// Token: 0x040005EF RID: 1519
			public uint m_GateCount;

			// Token: 0x040005F0 RID: 1520
			public Queue<EventDispatcher.EventRecord> m_Queue;
		}
	}
}
