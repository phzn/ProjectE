using System;
using UnityEngine;

namespace Bozo
{
	// Token: 0x02000817 RID: 2071
	public class BoZo_Links : MonoBehaviour
	{
		// Token: 0x06003740 RID: 14144 RVA: 0x0013F972 File Offset: 0x0013DB72
		private void Awake()
		{
			Object.Destroy(base.gameObject);
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x0013F97F File Offset: 0x0013DB7F
		public void Documentation()
		{
			Application.OpenURL("https://docs.google.com/document/d/1kf1KGT-OqV3Ecvnej0mpmgIOkc-_e7UJp-BkGuZfuN4/edit?usp=sharing");
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x0013F98B File Offset: 0x0013DB8B
		public void Discord()
		{
			Application.OpenURL("https://discord.gg/UCbRjUy7m7");
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x00004F31 File Offset: 0x00003131
		public void Twitter()
		{
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x00004F31 File Offset: 0x00003131
		public void Youtube()
		{
		}
	}
}
