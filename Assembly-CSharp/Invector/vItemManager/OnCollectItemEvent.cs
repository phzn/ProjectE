using System;
using UnityEngine.Events;

namespace Invector.vItemManager
{
	// Token: 0x020005C2 RID: 1474
	[Serializable]
	public class OnCollectItemEvent : UnityEvent<vItemManager.CollectedItemInfo>
	{
	}
}
