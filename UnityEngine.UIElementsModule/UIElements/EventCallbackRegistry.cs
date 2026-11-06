using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020001B1 RID: 433
	internal class EventCallbackRegistry
	{
		// Token: 0x06000D34 RID: 3380 RVA: 0x00033984 File Offset: 0x00031B84
		private static EventCallbackList GetCallbackList(EventCallbackList initializer = null)
		{
			return EventCallbackRegistry.s_ListPool.Get(initializer);
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x000339A1 File Offset: 0x00031BA1
		private static void ReleaseCallbackList(EventCallbackList toRelease)
		{
			EventCallbackRegistry.s_ListPool.Release(toRelease);
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x000339B0 File Offset: 0x00031BB0
		public EventCallbackRegistry()
		{
			this.m_IsInvoking = 0;
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x000339C4 File Offset: 0x00031BC4
		private EventCallbackList GetCallbackListForWriting()
		{
			bool flag = this.m_IsInvoking > 0;
			EventCallbackList eventCallbackList;
			if (flag)
			{
				bool flag2 = this.m_TemporaryCallbacks == null;
				if (flag2)
				{
					bool flag3 = this.m_Callbacks != null;
					if (flag3)
					{
						this.m_TemporaryCallbacks = EventCallbackRegistry.GetCallbackList(this.m_Callbacks);
					}
					else
					{
						this.m_TemporaryCallbacks = EventCallbackRegistry.GetCallbackList(null);
					}
				}
				eventCallbackList = this.m_TemporaryCallbacks;
			}
			else
			{
				bool flag4 = this.m_Callbacks == null;
				if (flag4)
				{
					this.m_Callbacks = EventCallbackRegistry.GetCallbackList(null);
				}
				eventCallbackList = this.m_Callbacks;
			}
			return eventCallbackList;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x00033A50 File Offset: 0x00031C50
		private EventCallbackList GetCallbackListForReading()
		{
			bool flag = this.m_TemporaryCallbacks != null;
			EventCallbackList eventCallbackList;
			if (flag)
			{
				eventCallbackList = this.m_TemporaryCallbacks;
			}
			else
			{
				eventCallbackList = this.m_Callbacks;
			}
			return eventCallbackList;
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00033A80 File Offset: 0x00031C80
		private bool ShouldRegisterCallback(long eventTypeId, Delegate callback, CallbackPhase phase)
		{
			bool flag = callback == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				EventCallbackList callbackListForReading = this.GetCallbackListForReading();
				bool flag3 = callbackListForReading != null;
				flag2 = !flag3 || !callbackListForReading.Contains(eventTypeId, callback, phase);
			}
			return flag2;
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00033AC0 File Offset: 0x00031CC0
		private bool UnregisterCallback(long eventTypeId, Delegate callback, TrickleDown useTrickleDown)
		{
			bool flag = callback == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				EventCallbackList callbackListForWriting = this.GetCallbackListForWriting();
				CallbackPhase callbackPhase = ((useTrickleDown == TrickleDown.TrickleDown) ? CallbackPhase.TrickleDownAndTarget : CallbackPhase.TargetAndBubbleUp);
				flag2 = callbackListForWriting.Remove(eventTypeId, callback, callbackPhase);
			}
			return flag2;
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00033AF8 File Offset: 0x00031CF8
		public void RegisterCallback<TEventType>(EventCallback<TEventType> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown, InvokePolicy invokePolicy = InvokePolicy.Default) where TEventType : EventBase<TEventType>, new()
		{
			bool flag = callback == null;
			if (flag)
			{
				throw new ArgumentException("callback parameter is null");
			}
			long num = EventBase<TEventType>.TypeId();
			CallbackPhase callbackPhase = ((useTrickleDown == TrickleDown.TrickleDown) ? CallbackPhase.TrickleDownAndTarget : CallbackPhase.TargetAndBubbleUp);
			EventCallbackList eventCallbackList = this.GetCallbackListForReading();
			bool flag2 = eventCallbackList == null || !eventCallbackList.Contains(num, callback, callbackPhase);
			if (flag2)
			{
				eventCallbackList = this.GetCallbackListForWriting();
				eventCallbackList.Add(new EventCallbackFunctor<TEventType>(callback, callbackPhase, invokePolicy));
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00033B60 File Offset: 0x00031D60
		public void RegisterCallback<TEventType, TCallbackArgs>(EventCallback<TEventType, TCallbackArgs> callback, TCallbackArgs userArgs, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown, InvokePolicy invokePolicy = InvokePolicy.Default) where TEventType : EventBase<TEventType>, new()
		{
			bool flag = callback == null;
			if (flag)
			{
				throw new ArgumentException("callback parameter is null");
			}
			long num = EventBase<TEventType>.TypeId();
			CallbackPhase callbackPhase = ((useTrickleDown == TrickleDown.TrickleDown) ? CallbackPhase.TrickleDownAndTarget : CallbackPhase.TargetAndBubbleUp);
			EventCallbackList eventCallbackList = this.GetCallbackListForReading();
			bool flag2 = eventCallbackList != null;
			if (flag2)
			{
				EventCallbackFunctor<TEventType, TCallbackArgs> eventCallbackFunctor = eventCallbackList.Find(num, callback, callbackPhase) as EventCallbackFunctor<TEventType, TCallbackArgs>;
				bool flag3 = eventCallbackFunctor != null;
				if (flag3)
				{
					eventCallbackFunctor.userArgs = userArgs;
					return;
				}
			}
			eventCallbackList = this.GetCallbackListForWriting();
			eventCallbackList.Add(new EventCallbackFunctor<TEventType, TCallbackArgs>(callback, userArgs, callbackPhase, invokePolicy));
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00033BE4 File Offset: 0x00031DE4
		public bool UnregisterCallback<TEventType>(EventCallback<TEventType> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
		{
			long num = EventBase<TEventType>.TypeId();
			return this.UnregisterCallback(num, callback, useTrickleDown);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00033C08 File Offset: 0x00031E08
		public bool UnregisterCallback<TEventType, TCallbackArgs>(EventCallback<TEventType, TCallbackArgs> callback, TrickleDown useTrickleDown = TrickleDown.NoTrickleDown) where TEventType : EventBase<TEventType>, new()
		{
			long num = EventBase<TEventType>.TypeId();
			return this.UnregisterCallback(num, callback, useTrickleDown);
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00033C2C File Offset: 0x00031E2C
		internal bool TryGetUserArgs<TEventType, TCallbackArgs>(EventCallback<TEventType, TCallbackArgs> callback, TrickleDown useTrickleDown, out TCallbackArgs userArgs) where TEventType : EventBase<TEventType>, new()
		{
			userArgs = default(TCallbackArgs);
			bool flag = callback == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				EventCallbackList callbackListForReading = this.GetCallbackListForReading();
				long num = EventBase<TEventType>.TypeId();
				CallbackPhase callbackPhase = ((useTrickleDown == TrickleDown.TrickleDown) ? CallbackPhase.TrickleDownAndTarget : CallbackPhase.TargetAndBubbleUp);
				EventCallbackFunctor<TEventType, TCallbackArgs> eventCallbackFunctor = callbackListForReading.Find(num, callback, callbackPhase) as EventCallbackFunctor<TEventType, TCallbackArgs>;
				bool flag3 = eventCallbackFunctor == null;
				if (flag3)
				{
					flag2 = false;
				}
				else
				{
					userArgs = eventCallbackFunctor.userArgs;
					flag2 = true;
				}
			}
			return flag2;
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00033C98 File Offset: 0x00031E98
		public void InvokeCallbacks(EventBase evt, PropagationPhase propagationPhase)
		{
			bool flag = this.m_Callbacks == null;
			if (!flag)
			{
				this.m_IsInvoking++;
				bool flag2;
				if (evt.skipDisabledElements)
				{
					VisualElement visualElement = evt.currentTarget as VisualElement;
					if (visualElement != null)
					{
						flag2 = !visualElement.enabledInHierarchy;
						goto IL_0045;
					}
				}
				flag2 = false;
				IL_0045:
				bool flag3 = flag2;
				for (int i = 0; i < this.m_Callbacks.Count; i++)
				{
					bool isImmediatePropagationStopped = evt.isImmediatePropagationStopped;
					if (isImmediatePropagationStopped)
					{
						break;
					}
					bool flag4 = flag3 && this.m_Callbacks[i].invokePolicy != InvokePolicy.IncludeDisabled;
					if (!flag4)
					{
						this.m_Callbacks[i].Invoke(evt, propagationPhase);
					}
				}
				this.m_IsInvoking--;
				bool flag5 = this.m_IsInvoking == 0;
				if (flag5)
				{
					bool flag6 = this.m_TemporaryCallbacks != null;
					if (flag6)
					{
						EventCallbackRegistry.ReleaseCallbackList(this.m_Callbacks);
						this.m_Callbacks = EventCallbackRegistry.GetCallbackList(this.m_TemporaryCallbacks);
						EventCallbackRegistry.ReleaseCallbackList(this.m_TemporaryCallbacks);
						this.m_TemporaryCallbacks = null;
					}
				}
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00033DB4 File Offset: 0x00031FB4
		public bool HasTrickleDownHandlers()
		{
			return this.m_Callbacks != null && this.m_Callbacks.trickleDownCallbackCount > 0;
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00033DE0 File Offset: 0x00031FE0
		public bool HasBubbleHandlers()
		{
			return this.m_Callbacks != null && this.m_Callbacks.bubbleUpCallbackCount > 0;
		}

		// Token: 0x04000641 RID: 1601
		private static readonly EventCallbackListPool s_ListPool = new EventCallbackListPool();

		// Token: 0x04000642 RID: 1602
		private EventCallbackList m_Callbacks;

		// Token: 0x04000643 RID: 1603
		private EventCallbackList m_TemporaryCallbacks;

		// Token: 0x04000644 RID: 1604
		private int m_IsInvoking;
	}
}
