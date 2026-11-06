using System;

namespace UnityEngine.UIElements
{
	// Token: 0x02000160 RID: 352
	internal static class DragAndDropUtility
	{
		// Token: 0x06000B85 RID: 2949 RVA: 0x0002DD40 File Offset: 0x0002BF40
		internal static IDragAndDrop GetDragAndDrop(IPanel panel)
		{
			bool flag = panel.contextType == ContextType.Player;
			IDragAndDrop dragAndDrop2;
			if (flag)
			{
				IDragAndDrop dragAndDrop;
				if ((dragAndDrop = DragAndDropUtility.s_DragAndDropPlayMode) == null)
				{
					dragAndDrop = (DragAndDropUtility.s_DragAndDropPlayMode = new DefaultDragAndDropClient());
				}
				dragAndDrop2 = dragAndDrop;
			}
			else
			{
				IDragAndDrop dragAndDrop3;
				if ((dragAndDrop3 = DragAndDropUtility.s_DragAndDropEditor) == null)
				{
					IDragAndDrop dragAndDrop5;
					if (DragAndDropUtility.s_MakeDragAndDropClientFunc == null)
					{
						IDragAndDrop dragAndDrop4 = new DefaultDragAndDropClient();
						dragAndDrop5 = dragAndDrop4;
					}
					else
					{
						dragAndDrop5 = DragAndDropUtility.s_MakeDragAndDropClientFunc();
					}
					dragAndDrop3 = (DragAndDropUtility.s_DragAndDropEditor = dragAndDrop5);
				}
				dragAndDrop2 = dragAndDrop3;
			}
			return dragAndDrop2;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x0002DDA0 File Offset: 0x0002BFA0
		internal static void RegisterMakeClientFunc(Func<IDragAndDrop> makeClient)
		{
			DragAndDropUtility.s_MakeDragAndDropClientFunc = makeClient;
			DragAndDropUtility.s_DragAndDropEditor = null;
		}

		// Token: 0x04000570 RID: 1392
		private static Func<IDragAndDrop> s_MakeDragAndDropClientFunc;

		// Token: 0x04000571 RID: 1393
		private static IDragAndDrop s_DragAndDropEditor;

		// Token: 0x04000572 RID: 1394
		private static IDragAndDrop s_DragAndDropPlayMode;
	}
}
