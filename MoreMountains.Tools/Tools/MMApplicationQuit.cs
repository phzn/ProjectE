using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000090 RID: 144
	public class MMApplicationQuit : MonoBehaviour
	{
		// Token: 0x0600032A RID: 810 RVA: 0x000102AC File Offset: 0x0000E4AC
		public virtual void Quit()
		{
			Application.Quit();
		}

		// Token: 0x0400045B RID: 1115
		[Header("Debug")]
		[MMInspectorButton("Quit")]
		public bool QuitButton;
	}
}
