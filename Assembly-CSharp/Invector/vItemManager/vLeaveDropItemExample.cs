using System;
using UnityEngine;

namespace Invector.vItemManager
{
	// Token: 0x020005D1 RID: 1489
	public class vLeaveDropItemExample : MonoBehaviour
	{
		// Token: 0x060022AE RID: 8878 RVA: 0x000C7323 File Offset: 0x000C5523
		private void OnGUI()
		{
			this.windowRect = GUILayout.Window(0, this.windowRect, new GUI.WindowFunction(this.vLeaveDropItensWindow), "Leave and Drop Items test by Invector:", Array.Empty<GUILayoutOption>());
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x000C7350 File Offset: 0x000C5550
		private void vLeaveDropItensWindow(int windowID)
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			this.itemManager = Object.FindObjectOfType<vItemManager>();
			if (this.itemManager)
			{
				this.scroll = GUILayout.BeginScrollView(this.scroll, new GUILayoutOption[]
				{
					GUILayout.Width(300f),
					GUILayout.MinHeight(300f)
				});
				for (int i = 0; i < this.itemManager.items.Count; i++)
				{
					GUILayout.BeginHorizontal("box", Array.Empty<GUILayoutOption>());
					GUILayout.Label(new GUIContent("Name:" + this.itemManager.items[i].name + "\nAmount :" + this.itemManager.items[i].amount.ToString()), new GUILayoutOption[]
					{
						GUILayout.Width(200f),
						GUILayout.Height(40f)
					});
					GUILayout.BeginVertical("box", Array.Empty<GUILayoutOption>());
					if (GUILayout.Button("Leave", Array.Empty<GUILayoutOption>()) || GUILayout.Button("Drop", Array.Empty<GUILayoutOption>()))
					{
						break;
					}
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
				}
				GUILayout.EndScrollView();
			}
			GUILayout.EndVertical();
			GUI.DragWindow();
		}

		// Token: 0x040022F6 RID: 8950
		private vItemManager itemManager;

		// Token: 0x040022F7 RID: 8951
		private Rect windowRect;

		// Token: 0x040022F8 RID: 8952
		private Vector2 scroll;
	}
}
