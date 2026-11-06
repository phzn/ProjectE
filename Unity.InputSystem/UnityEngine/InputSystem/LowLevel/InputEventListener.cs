using System;
using UnityEngine.InputSystem.Utilities;

namespace UnityEngine.InputSystem.LowLevel
{
	// Token: 0x020000E8 RID: 232
	public struct InputEventListener : IObservable<InputEventPtr>
	{
		// Token: 0x06000DD6 RID: 3542 RVA: 0x000459E0 File Offset: 0x00043BE0
		public static InputEventListener operator +(InputEventListener _, Action<InputEventPtr, InputDevice> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			InputManager s_Manager = InputSystem.s_Manager;
			lock (s_Manager)
			{
				InputSystem.s_Manager.onEvent += callback;
			}
			return default(InputEventListener);
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00045A3C File Offset: 0x00043C3C
		public static InputEventListener operator -(InputEventListener _, Action<InputEventPtr, InputDevice> callback)
		{
			if (callback == null)
			{
				throw new ArgumentNullException("callback");
			}
			InputManager s_Manager = InputSystem.s_Manager;
			lock (s_Manager)
			{
				InputSystem.s_Manager.onEvent -= callback;
			}
			return default(InputEventListener);
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00045A98 File Offset: 0x00043C98
		public IDisposable Subscribe(IObserver<InputEventPtr> observer)
		{
			if (InputEventListener.s_ObserverState == null)
			{
				InputEventListener.s_ObserverState = new InputEventListener.ObserverState();
			}
			if (InputEventListener.s_ObserverState.observers.length == 0)
			{
				InputSystem.s_Manager.onEvent += InputEventListener.s_ObserverState.onEventDelegate;
			}
			InputEventListener.s_ObserverState.observers.AppendWithCapacity(observer, 10);
			return new InputEventListener.DisposableObserver
			{
				observer = observer
			};
		}

		// Token: 0x040005B8 RID: 1464
		internal static InputEventListener.ObserverState s_ObserverState;

		// Token: 0x02000217 RID: 535
		internal class ObserverState
		{
			// Token: 0x06001506 RID: 5382 RVA: 0x00062316 File Offset: 0x00060516
			public ObserverState()
			{
				this.onEventDelegate = delegate(InputEventPtr eventPtr, InputDevice device)
				{
					for (int i = this.observers.length - 1; i >= 0; i--)
					{
						this.observers[i].OnNext(eventPtr);
					}
				};
			}

			// Token: 0x04000B9F RID: 2975
			public InlinedArray<IObserver<InputEventPtr>> observers;

			// Token: 0x04000BA0 RID: 2976
			public Action<InputEventPtr, InputDevice> onEventDelegate;
		}

		// Token: 0x02000218 RID: 536
		private class DisposableObserver : IDisposable
		{
			// Token: 0x06001508 RID: 5384 RVA: 0x00062368 File Offset: 0x00060568
			public void Dispose()
			{
				int num = InputEventListener.s_ObserverState.observers.IndexOfReference(this.observer);
				if (num >= 0)
				{
					InputEventListener.s_ObserverState.observers.RemoveAtWithCapacity(num);
				}
				if (InputEventListener.s_ObserverState.observers.length == 0)
				{
					InputSystem.s_Manager.onEvent -= InputEventListener.s_ObserverState.onEventDelegate;
				}
			}

			// Token: 0x04000BA1 RID: 2977
			public IObserver<InputEventPtr> observer;
		}
	}
}
