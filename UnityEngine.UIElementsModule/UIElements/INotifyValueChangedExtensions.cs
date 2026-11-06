using System;

namespace UnityEngine.UIElements
{
	// Token: 0x020000CA RID: 202
	public static class INotifyValueChangedExtensions
	{
		// Token: 0x060006C7 RID: 1735 RVA: 0x00019D98 File Offset: 0x00017F98
		public static bool RegisterValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
		{
			CallbackEventHandler callbackEventHandler = control as CallbackEventHandler;
			bool flag = callbackEventHandler != null;
			bool flag2;
			if (flag)
			{
				callbackEventHandler.RegisterCallback<ChangeEvent<T>>(callback, TrickleDown.NoTrickleDown);
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00019DC8 File Offset: 0x00017FC8
		public static bool UnregisterValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
		{
			CallbackEventHandler callbackEventHandler = control as CallbackEventHandler;
			bool flag = callbackEventHandler != null;
			bool flag2;
			if (flag)
			{
				callbackEventHandler.UnregisterCallback<ChangeEvent<T>>(callback, TrickleDown.NoTrickleDown);
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}
	}
}
