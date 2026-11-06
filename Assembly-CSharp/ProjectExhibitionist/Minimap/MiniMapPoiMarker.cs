using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectExhibitionist.Minimap
{
	// Token: 0x020009A5 RID: 2469
	[DisallowMultipleComponent]
	public sealed class MiniMapPoiMarker : MonoBehaviour
	{
		// Token: 0x14000076 RID: 118
		// (add) Token: 0x060044E4 RID: 17636 RVA: 0x0019C8CC File Offset: 0x0019AACC
		// (remove) Token: 0x060044E5 RID: 17637 RVA: 0x0019C900 File Offset: 0x0019AB00
		internal static event Action Changed;

		// Token: 0x17000BCA RID: 3018
		// (get) Token: 0x060044E6 RID: 17638 RVA: 0x0019C933 File Offset: 0x0019AB33
		public MiniMapPoiKind Kind
		{
			get
			{
				return this.kind;
			}
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x060044E7 RID: 17639 RVA: 0x0019C93C File Offset: 0x0019AB3C
		public bool IsOperationallyReady
		{
			get
			{
				IncrementalPooledZoneActivation incrementalPooledZoneActivation;
				return MiniMapPoiMarker.IsSourceOperational(base.transform, this.kind, out incrementalPooledZoneActivation);
			}
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x0019C95C File Offset: 0x0019AB5C
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetStaticState()
		{
			MiniMapPoiMarker.Active.Clear();
			MiniMapPoiMarker.Changed = null;
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x0019C96E File Offset: 0x0019AB6E
		private void OnEnable()
		{
			MiniMapPoiMarker.Active.Add(this);
			this.BindReadinessGate();
			Action changed = MiniMapPoiMarker.Changed;
			if (changed == null)
			{
				return;
			}
			changed();
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x0019C991 File Offset: 0x0019AB91
		private void OnDisable()
		{
			this.UnbindReadinessGate();
			if (MiniMapPoiMarker.Active.Remove(this))
			{
				Action changed = MiniMapPoiMarker.Changed;
				if (changed == null)
				{
					return;
				}
				changed();
			}
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x0019C991 File Offset: 0x0019AB91
		private void OnDestroy()
		{
			this.UnbindReadinessGate();
			if (MiniMapPoiMarker.Active.Remove(this))
			{
				Action changed = MiniMapPoiMarker.Changed;
				if (changed == null)
				{
					return;
				}
				changed();
			}
		}

		// Token: 0x060044EC RID: 17644 RVA: 0x0019C9B5 File Offset: 0x0019ABB5
		internal static bool IsSourceOperational(Transform source, MiniMapPoiKind poiKind, out IncrementalPooledZoneActivation readinessGate)
		{
			readinessGate = null;
			if ((poiKind != MiniMapPoiKind.Metro && poiKind != MiniMapPoiKind.Kombini) || source == null)
			{
				return true;
			}
			readinessGate = source.GetComponentInParent<IncrementalPooledZoneActivation>(true);
			return readinessGate == null || readinessGate.IsOperationalReady;
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x0019C9EC File Offset: 0x0019ABEC
		private void BindReadinessGate()
		{
			this.UnbindReadinessGate();
			if (this.kind != MiniMapPoiKind.Metro && this.kind != MiniMapPoiKind.Kombini)
			{
				return;
			}
			this._readinessGate = base.GetComponentInParent<IncrementalPooledZoneActivation>(true);
			if (this._readinessGate != null)
			{
				this._readinessGate.OperationalReady += this.HandleOperationalReady;
			}
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x0019CA45 File Offset: 0x0019AC45
		private void UnbindReadinessGate()
		{
			if (this._readinessGate != null)
			{
				this._readinessGate.OperationalReady -= this.HandleOperationalReady;
			}
			this._readinessGate = null;
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x0019CA73 File Offset: 0x0019AC73
		private void HandleOperationalReady(IncrementalPooledZoneActivation source)
		{
			if (source == this._readinessGate)
			{
				Action changed = MiniMapPoiMarker.Changed;
				if (changed == null)
				{
					return;
				}
				changed();
			}
		}

		// Token: 0x04004484 RID: 17540
		internal static readonly HashSet<MiniMapPoiMarker> Active = new HashSet<MiniMapPoiMarker>();

		// Token: 0x04004486 RID: 17542
		[SerializeField]
		private MiniMapPoiKind kind = MiniMapPoiKind.Home;

		// Token: 0x04004487 RID: 17543
		private IncrementalPooledZoneActivation _readinessGate;
	}
}
