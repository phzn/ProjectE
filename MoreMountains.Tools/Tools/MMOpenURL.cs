using System;
using UnityEngine;

namespace MoreMountains.Tools
{
	// Token: 0x02000093 RID: 147
	[AddComponentMenu("More Mountains/Tools/Utilities/MMOpenURL")]
	public class MMOpenURL : MonoBehaviour
	{
		// Token: 0x06000332 RID: 818 RVA: 0x00010318 File Offset: 0x0000E518
		public virtual void OpenURL()
		{
			Application.OpenURL(this.DestinationURL);
		}

		// Token: 0x0400045F RID: 1119
		public string DestinationURL;
	}
}
