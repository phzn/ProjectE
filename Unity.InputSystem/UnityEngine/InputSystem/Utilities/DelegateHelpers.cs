using System;
using Unity.Profiling;

namespace UnityEngine.InputSystem.Utilities
{
	// Token: 0x02000130 RID: 304
	internal static class DelegateHelpers
	{
		// Token: 0x06001104 RID: 4356 RVA: 0x000516B8 File Offset: 0x0004F8B8
		public static void InvokeCallbacksSafe(ref CallbackArray<Action> callbacks, ProfilerMarker marker, string callbackName, object context = null)
		{
			if (callbacks.length == 0)
			{
				return;
			}
			callbacks.LockForChanges();
			for (int i = 0; i < callbacks.length; i++)
			{
				try
				{
					callbacks[i]();
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					if (context != null)
					{
						Debug.LogError(string.Format("{0} while executing '{1}' callbacks of '{2}'", ex.GetType().Name, callbackName, context));
					}
					else
					{
						Debug.LogError(ex.GetType().Name + " while executing '" + callbackName + "' callbacks");
					}
				}
			}
			callbacks.UnlockForChanges();
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00051754 File Offset: 0x0004F954
		public static void InvokeCallbacksSafe<TValue>(ref CallbackArray<Action<TValue>> callbacks, TValue argument, string callbackName, object context = null)
		{
			if (callbacks.length == 0)
			{
				return;
			}
			callbacks.LockForChanges();
			for (int i = 0; i < callbacks.length; i++)
			{
				try
				{
					callbacks[i](argument);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					if (context != null)
					{
						Debug.LogError(string.Format("{0} while executing '{1}' callbacks of '{2}'", ex.GetType().Name, callbackName, context));
					}
					else
					{
						Debug.LogError(ex.GetType().Name + " while executing '" + callbackName + "' callbacks");
					}
				}
			}
			callbacks.UnlockForChanges();
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000517F4 File Offset: 0x0004F9F4
		public static void InvokeCallbacksSafe<TValue1, TValue2>(ref CallbackArray<Action<TValue1, TValue2>> callbacks, TValue1 argument1, TValue2 argument2, ProfilerMarker marker, string callbackName, object context = null)
		{
			if (callbacks.length == 0)
			{
				return;
			}
			callbacks.LockForChanges();
			for (int i = 0; i < callbacks.length; i++)
			{
				try
				{
					callbacks[i](argument1, argument2);
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					if (context != null)
					{
						Debug.LogError(string.Format("{0} while executing '{1}' callbacks of '{2}'", ex.GetType().Name, callbackName, context));
					}
					else
					{
						Debug.LogError(ex.GetType().Name + " while executing '" + callbackName + "' callbacks");
					}
				}
			}
			callbacks.UnlockForChanges();
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00051898 File Offset: 0x0004FA98
		public static bool InvokeCallbacksSafe_AnyCallbackReturnsTrue<TValue1, TValue2>(ref CallbackArray<Func<TValue1, TValue2, bool>> callbacks, TValue1 argument1, TValue2 argument2, string callbackName, object context = null)
		{
			if (callbacks.length == 0)
			{
				return true;
			}
			callbacks.LockForChanges();
			for (int i = 0; i < callbacks.length; i++)
			{
				try
				{
					if (callbacks[i](argument1, argument2))
					{
						callbacks.UnlockForChanges();
						return true;
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					if (context != null)
					{
						Debug.LogError(string.Format("{0} while executing '{1}' callbacks of '{2}'", ex.GetType().Name, callbackName, context));
					}
					else
					{
						Debug.LogError(ex.GetType().Name + " while executing '" + callbackName + "' callbacks");
					}
				}
			}
			callbacks.UnlockForChanges();
			return false;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00051948 File Offset: 0x0004FB48
		public static void InvokeCallbacksSafe_AndInvokeReturnedActions<TValue>(ref CallbackArray<Func<TValue, Action>> callbacks, TValue argument, string callbackName, object context = null)
		{
			if (callbacks.length == 0)
			{
				return;
			}
			callbacks.LockForChanges();
			for (int i = 0; i < callbacks.length; i++)
			{
				try
				{
					Action action = callbacks[i](argument);
					if (action != null)
					{
						action();
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					if (context != null)
					{
						Debug.LogError(string.Format("{0} while executing '{1}' callbacks of '{2}'", ex.GetType().Name, callbackName, context));
					}
					else
					{
						Debug.LogError(ex.GetType().Name + " while executing '" + callbackName + "' callbacks");
					}
				}
			}
			callbacks.UnlockForChanges();
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x000519F0 File Offset: 0x0004FBF0
		public static bool InvokeCallbacksSafe_AnyCallbackReturnsObject<TValue, TReturn>(ref CallbackArray<Func<TValue, TReturn>> callbacks, TValue argument, string callbackName, object context = null)
		{
			if (callbacks.length == 0)
			{
				return false;
			}
			callbacks.LockForChanges();
			for (int i = 0; i < callbacks.length; i++)
			{
				try
				{
					if (callbacks[i](argument) != null)
					{
						callbacks.UnlockForChanges();
						return true;
					}
				}
				catch (Exception ex)
				{
					Debug.LogException(ex);
					if (context != null)
					{
						Debug.LogError(string.Format("{0} while executing '{1}' callbacks of '{2}'", ex.GetType().Name, callbackName, context));
					}
					else
					{
						Debug.LogError(ex.GetType().Name + " while executing '" + callbackName + "' callbacks");
					}
				}
			}
			callbacks.UnlockForChanges();
			return false;
		}
	}
}
