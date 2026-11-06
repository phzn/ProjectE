using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements
{
	// Token: 0x0200037E RID: 894
	internal static class UIEventRegistration
	{
		// Token: 0x06001E3A RID: 7738 RVA: 0x00074CFC File Offset: 0x00072EFC
		static UIEventRegistration()
		{
			GUIUtility.takeCapture = (Action)Delegate.Combine(GUIUtility.takeCapture, new Action(delegate
			{
				UIEventRegistration.TakeCapture();
			}));
			GUIUtility.releaseCapture = (Action)Delegate.Combine(GUIUtility.releaseCapture, new Action(delegate
			{
				UIEventRegistration.ReleaseCapture();
			}));
			GUIUtility.processEvent = (Func<int, IntPtr, bool>)Delegate.Combine(GUIUtility.processEvent, new Func<int, IntPtr, bool>((int i, IntPtr ptr) => UIEventRegistration.ProcessEvent(i, ptr)));
			GUIUtility.cleanupRoots = (Action)Delegate.Combine(GUIUtility.cleanupRoots, new Action(delegate
			{
				UIEventRegistration.CleanupRoots();
			}));
			GUIUtility.endContainerGUIFromException = (Func<Exception, bool>)Delegate.Combine(GUIUtility.endContainerGUIFromException, new Func<Exception, bool>((Exception exception) => UIEventRegistration.EndContainerGUIFromException(exception)));
			GUIUtility.guiChanged = (Action)Delegate.Combine(GUIUtility.guiChanged, new Action(delegate
			{
				UIEventRegistration.MakeCurrentIMGUIContainerDirty();
			}));
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x00074DEC File Offset: 0x00072FEC
		internal static void RegisterUIElementSystem(IUIElementsUtility utility)
		{
			UIEventRegistration.s_Utilities.Insert(0, utility);
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x00074DFC File Offset: 0x00072FFC
		private static void TakeCapture()
		{
			foreach (IUIElementsUtility iuielementsUtility in UIEventRegistration.s_Utilities)
			{
				bool flag = iuielementsUtility.TakeCapture();
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x00074E58 File Offset: 0x00073058
		private static void ReleaseCapture()
		{
			foreach (IUIElementsUtility iuielementsUtility in UIEventRegistration.s_Utilities)
			{
				bool flag = iuielementsUtility.ReleaseCapture();
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x00074EB4 File Offset: 0x000730B4
		private static bool EndContainerGUIFromException(Exception exception)
		{
			foreach (IUIElementsUtility iuielementsUtility in UIEventRegistration.s_Utilities)
			{
				bool flag = iuielementsUtility.EndContainerGUIFromException(exception);
				if (flag)
				{
					return true;
				}
			}
			return GUIUtility.ShouldRethrowException(exception);
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x00074F20 File Offset: 0x00073120
		private static bool ProcessEvent(int instanceID, IntPtr nativeEventPtr)
		{
			bool flag = false;
			foreach (IUIElementsUtility iuielementsUtility in UIEventRegistration.s_Utilities)
			{
				bool flag2 = iuielementsUtility.ProcessEvent(instanceID, nativeEventPtr, ref flag);
				if (flag2)
				{
					return flag;
				}
			}
			return false;
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x00074F90 File Offset: 0x00073190
		private static void CleanupRoots()
		{
			foreach (IUIElementsUtility iuielementsUtility in UIEventRegistration.s_Utilities)
			{
				bool flag = iuielementsUtility.CleanupRoots();
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x00074FEC File Offset: 0x000731EC
		internal static void MakeCurrentIMGUIContainerDirty()
		{
			foreach (IUIElementsUtility iuielementsUtility in UIEventRegistration.s_Utilities)
			{
				bool flag = iuielementsUtility.MakeCurrentIMGUIContainerDirty();
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x00075048 File Offset: 0x00073248
		internal static void UpdateSchedulers()
		{
			foreach (IUIElementsUtility iuielementsUtility in UIEventRegistration.s_Utilities)
			{
				iuielementsUtility.UpdateSchedulers();
			}
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000750A0 File Offset: 0x000732A0
		internal static void RequestRepaintForPanels(Action<ScriptableObject> repaintCallback)
		{
			foreach (IUIElementsUtility iuielementsUtility in UIEventRegistration.s_Utilities)
			{
				iuielementsUtility.RequestRepaintForPanels(repaintCallback);
			}
		}

		// Token: 0x04000C92 RID: 3218
		private static List<IUIElementsUtility> s_Utilities = new List<IUIElementsUtility>();
	}
}
