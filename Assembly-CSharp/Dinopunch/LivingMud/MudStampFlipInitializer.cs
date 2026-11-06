using System;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000747 RID: 1863
	[RequireComponent(typeof(MudStamp))]
	public class MudStampFlipInitializer : MonoBehaviour
	{
		// Token: 0x06003045 RID: 12357 RVA: 0x001118F8 File Offset: 0x0010FAF8
		private void Start()
		{
			MudStamp component = base.GetComponent<MudStamp>();
			component.SetFlipX(this.flipX);
			component.SetFlipY(this.flipY);
		}

		// Token: 0x040030DA RID: 12506
		[SerializeField]
		private bool flipX;

		// Token: 0x040030DB RID: 12507
		[SerializeField]
		private bool flipY;
	}
}
