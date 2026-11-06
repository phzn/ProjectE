using System;
using UnityEngine;

namespace Michsky.MUIP
{
	// Token: 0x020001F5 RID: 501
	public class LaunchURL : MonoBehaviour
	{
		// Token: 0x06000E65 RID: 3685 RVA: 0x00061305 File Offset: 0x0005F505
		public void GoToURL(string URL)
		{
			Application.OpenURL(URL);
		}
	}
}
