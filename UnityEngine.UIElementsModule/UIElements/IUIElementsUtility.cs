using System;

namespace UnityEngine.UIElements
{
	// Token: 0x0200037D RID: 893
	internal interface IUIElementsUtility
	{
		// Token: 0x06001E32 RID: 7730
		bool TakeCapture();

		// Token: 0x06001E33 RID: 7731
		bool ReleaseCapture();

		// Token: 0x06001E34 RID: 7732
		bool ProcessEvent(int instanceID, IntPtr nativeEventPtr, ref bool eventHandled);

		// Token: 0x06001E35 RID: 7733
		bool CleanupRoots();

		// Token: 0x06001E36 RID: 7734
		bool EndContainerGUIFromException(Exception exception);

		// Token: 0x06001E37 RID: 7735
		bool MakeCurrentIMGUIContainerDirty();

		// Token: 0x06001E38 RID: 7736
		void UpdateSchedulers();

		// Token: 0x06001E39 RID: 7737
		void RequestRepaintForPanels(Action<ScriptableObject> repaintCallback);
	}
}
