using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.NPC
{
	// Token: 0x02000EE4 RID: 3812
	public sealed class NPCResidentBudget
	{
		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x06007228 RID: 29224 RVA: 0x002E719D File Offset: 0x002E539D
		public int Count
		{
			get
			{
				this.PruneDestroyed();
				return this._residents.Count;
			}
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x06007229 RID: 29225 RVA: 0x002E71B0 File Offset: 0x002E53B0
		public static int CustomCount
		{
			get
			{
				NPCResidentBudget.PruneCustomDestroyed();
				return NPCResidentBudget.CustomResidents.Count;
			}
		}

		// Token: 0x0600722A RID: 29226 RVA: 0x002E71C1 File Offset: 0x002E53C1
		public bool Contains(string identity)
		{
			this.PruneDestroyed();
			return this._residents.ContainsKey(identity);
		}

		// Token: 0x0600722B RID: 29227 RVA: 0x002E71D8 File Offset: 0x002E53D8
		public bool CanReserve(string identity, bool isCustom)
		{
			this.PruneDestroyed();
			NPCResidentBudget.PruneCustomDestroyed();
			return !string.IsNullOrEmpty(identity) && !this._residents.ContainsKey(identity) && (!isCustom || (!NPCResidentBudget.CustomResidents.ContainsKey(identity) && NPCResidentBudget.CustomResidents.Count < 4));
		}

		// Token: 0x0600722C RID: 29228 RVA: 0x002E722C File Offset: 0x002E542C
		public bool TryReserve(string identity, bool isCustom, out NPCResidentBudget.Reservation reservation)
		{
			reservation = null;
			if (!this.CanReserve(identity, isCustom))
			{
				return false;
			}
			reservation = new NPCResidentBudget.Reservation
			{
				identity = identity,
				isCustom = isCustom,
				owner = this
			};
			this._residents.Add(identity, reservation);
			if (isCustom)
			{
				NPCResidentBudget.CustomResidents.Add(identity, reservation);
			}
			return true;
		}

		// Token: 0x0600722D RID: 29229 RVA: 0x002E7283 File Offset: 0x002E5483
		public void Attach(NPCResidentBudget.Reservation reservation, GameObject instance)
		{
			if (reservation == null || reservation.owner != this)
			{
				return;
			}
			reservation.instance = instance;
			reservation.attached = true;
		}

		// Token: 0x0600722E RID: 29230 RVA: 0x002E72A0 File Offset: 0x002E54A0
		public void CancelUnattached(NPCResidentBudget.Reservation reservation)
		{
			if (reservation != null && reservation.owner == this && !reservation.attached)
			{
				this.Release(reservation);
			}
		}

		// Token: 0x0600722F RID: 29231 RVA: 0x002E72C0 File Offset: 0x002E54C0
		private void Release(NPCResidentBudget.Reservation reservation)
		{
			NPCResidentBudget.Reservation reservation2;
			if (this._residents.TryGetValue(reservation.identity, out reservation2) && reservation2 == reservation)
			{
				this._residents.Remove(reservation.identity);
			}
			if (reservation.isCustom && NPCResidentBudget.CustomResidents.TryGetValue(reservation.identity, out reservation2) && reservation2 == reservation)
			{
				NPCResidentBudget.CustomResidents.Remove(reservation.identity);
			}
		}

		// Token: 0x06007230 RID: 29232 RVA: 0x002E732C File Offset: 0x002E552C
		private void PruneDestroyed()
		{
			this._destroyedResidents.Clear();
			foreach (NPCResidentBudget.Reservation reservation in this._residents.Values)
			{
				if (reservation.attached && reservation.instance == null)
				{
					this._destroyedResidents.Add(reservation);
				}
			}
			for (int i = 0; i < this._destroyedResidents.Count; i++)
			{
				this.Release(this._destroyedResidents[i]);
			}
			this._destroyedResidents.Clear();
		}

		// Token: 0x06007231 RID: 29233 RVA: 0x002E73E0 File Offset: 0x002E55E0
		private static void PruneCustomDestroyed()
		{
			NPCResidentBudget.DestroyedCustomResidents.Clear();
			foreach (NPCResidentBudget.Reservation reservation in NPCResidentBudget.CustomResidents.Values)
			{
				if (reservation.attached && reservation.instance == null)
				{
					NPCResidentBudget.DestroyedCustomResidents.Add(reservation);
				}
			}
			for (int i = 0; i < NPCResidentBudget.DestroyedCustomResidents.Count; i++)
			{
				NPCResidentBudget.DestroyedCustomResidents[i].owner.Release(NPCResidentBudget.DestroyedCustomResidents[i]);
			}
			NPCResidentBudget.DestroyedCustomResidents.Clear();
		}

		// Token: 0x06007232 RID: 29234 RVA: 0x002E749C File Offset: 0x002E569C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStatics()
		{
			NPCResidentBudget.CustomResidents.Clear();
			NPCResidentBudget.DestroyedCustomResidents.Clear();
		}

		// Token: 0x04007546 RID: 30022
		private readonly Dictionary<string, NPCResidentBudget.Reservation> _residents = new Dictionary<string, NPCResidentBudget.Reservation>(StringComparer.Ordinal);

		// Token: 0x04007547 RID: 30023
		private static readonly Dictionary<string, NPCResidentBudget.Reservation> CustomResidents = new Dictionary<string, NPCResidentBudget.Reservation>(StringComparer.Ordinal);

		// Token: 0x04007548 RID: 30024
		private readonly List<NPCResidentBudget.Reservation> _destroyedResidents = new List<NPCResidentBudget.Reservation>();

		// Token: 0x04007549 RID: 30025
		private static readonly List<NPCResidentBudget.Reservation> DestroyedCustomResidents = new List<NPCResidentBudget.Reservation>();

		// Token: 0x02000EE5 RID: 3813
		public sealed class Reservation
		{
			// Token: 0x0400754A RID: 30026
			internal string identity;

			// Token: 0x0400754B RID: 30027
			internal bool isCustom;

			// Token: 0x0400754C RID: 30028
			internal bool attached;

			// Token: 0x0400754D RID: 30029
			internal GameObject instance;

			// Token: 0x0400754E RID: 30030
			internal NPCResidentBudget owner;
		}
	}
}
