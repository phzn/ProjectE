using System;
using UnityEngine;

namespace Features.NPCs
{
	// Token: 0x02000EBB RID: 3771
	public class NPCProp : MonoBehaviour
	{
		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06007104 RID: 28932 RVA: 0x002DE7D3 File Offset: 0x002DC9D3
		public string PropName
		{
			get
			{
				if (!string.IsNullOrEmpty(this.propName))
				{
					return this.propName;
				}
				return base.gameObject.name;
			}
		}

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06007105 RID: 28933 RVA: 0x002DE7F4 File Offset: 0x002DC9F4
		public bool StartDisabled
		{
			get
			{
				return this.startDisabled;
			}
		}

		// Token: 0x040073C1 RID: 29633
		[Tooltip("Nome opzionale della prop per debug/identificazione")]
		[SerializeField]
		private string propName;

		// Token: 0x040073C2 RID: 29634
		[Tooltip("Se true, la prop verrà disattivata di default allo spawn")]
		[SerializeField]
		private bool startDisabled = true;
	}
}
