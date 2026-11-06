using System;
using System.Collections.Generic;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace ProjectExhibitionist.Subway
{
	// Token: 0x0200092D RID: 2349
	[DisallowMultipleComponent]
	[DefaultExecutionOrder(32500)]
	public class SubwayPlatformBarrier : MonoBehaviour
	{
		// Token: 0x06003FAB RID: 16299 RVA: 0x0017A07C File Offset: 0x0017827C
		private void Start()
		{
			this.Apply(false, true);
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0017A088 File Offset: 0x00178288
		private void LateUpdate()
		{
			bool flag = this.doors != null && this.doors.DoorsPassable;
			bool flag2 = this.PlayerIsNear();
			if (!flag && !flag2)
			{
				this._wasNear = false;
				return;
			}
			bool flag3 = flag2 && !this._wasNear;
			this._wasNear = flag2;
			this.Apply(flag, flag3);
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x0017A0E8 File Offset: 0x001782E8
		private bool PlayerIsNear()
		{
			Transform player = PlayerRegistry.Player;
			return !(player == null) && (player.position - base.transform.position).sqrMagnitude <= this.enforceDistance * this.enforceDistance;
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0017A138 File Offset: 0x00178338
		private void Apply(bool open, bool silent = false)
		{
			bool flag = open != this._open;
			this._open = open;
			for (int i = 0; i < this.gates.Count; i++)
			{
				Collider collider = this.gates[i];
				if (!(collider == null))
				{
					bool flag2 = !open;
					if (collider.enabled != flag2)
					{
						if (!flag && !silent)
						{
							Debug.LogWarning(string.Concat(new string[]
							{
								"[SubwayBarrier] '",
								base.name,
								"': il cancello '",
								collider.name,
								"' è stato ",
								string.Format("rimesso a enabled={0} da qualcun altro mentre le porte ", collider.enabled),
								string.Format("erano {0} — lo riporto a {1}.", open ? "APERTE" : "chiuse", flag2)
							}), this);
						}
						collider.enabled = flag2;
					}
				}
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06003FAF RID: 16303 RVA: 0x0017A221 File Offset: 0x00178421
		public SubwayDoorSystem Doors
		{
			get
			{
				return this.doors;
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06003FB0 RID: 16304 RVA: 0x0017A229 File Offset: 0x00178429
		public IReadOnlyList<Collider> Gates
		{
			get
			{
				return this.gates;
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06003FB1 RID: 16305 RVA: 0x0017A234 File Offset: 0x00178434
		public int EnabledGateCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.gates.Count; i++)
				{
					if (this.gates[i] != null && this.gates[i].enabled)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x04003F8B RID: 16267
		[Tooltip("Il sistema porte del treno che serve QUESTO bordo di banchina.")]
		[SerializeField]
		private SubwayDoorSystem doors;

		// Token: 0x04003F8C RID: 16268
		[Tooltip("I tratti davanti alle porte: sono gli unici che si aprono. Li genera il tool, ma puoi aggiungerne o spostarli a mano.")]
		[SerializeField]
		private List<Collider> gates = new List<Collider>();

		// Token: 0x04003F8D RID: 16269
		[Tooltip("Oltre questa distanza dal player la barriera smette di imporre lo stato: nessuno può toccarla da lontano, e il culling dei collider dei chunk lontani spegne tutto per suo conto senza che ci sia niente da correggere.")]
		[SerializeField]
		private float enforceDistance = 70f;

		// Token: 0x04003F8E RID: 16270
		private bool _open;

		// Token: 0x04003F8F RID: 16271
		private bool _wasNear;
	}
}
