using System;
using UnityEngine;

namespace Features.MainMenu
{
	// Token: 0x02001049 RID: 4169
	public sealed class MainMenuCollectionBadge : MonoBehaviour
	{
		// Token: 0x06007E9F RID: 32415 RVA: 0x0033C763 File Offset: 0x0033A963
		private void OnEnable()
		{
			this.Refresh();
		}

		// Token: 0x06007EA0 RID: 32416 RVA: 0x0033C76B File Offset: 0x0033A96B
		private void Update()
		{
			if (Time.unscaledTime < this._nextRefresh)
			{
				return;
			}
			this._nextRefresh = Time.unscaledTime + 0.5f;
			this.Refresh();
		}

		// Token: 0x06007EA1 RID: 32417 RVA: 0x0033C792 File Offset: 0x0033A992
		public void Refresh()
		{
			if (this.badge != null)
			{
				this.badge.SetActive(this.collectionPanel != null && this.collectionPanel.HasNewDiscoveries());
			}
		}

		// Token: 0x04008237 RID: 33335
		[SerializeField]
		private MainMenuCollectionPanel collectionPanel;

		// Token: 0x04008238 RID: 33336
		[SerializeField]
		private GameObject badge;

		// Token: 0x04008239 RID: 33337
		private float _nextRefresh;
	}
}
