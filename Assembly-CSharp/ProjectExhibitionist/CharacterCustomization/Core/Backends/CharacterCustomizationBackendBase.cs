using System;
using Bozo.AnimeCharacters;
using UnityEngine;

namespace ProjectExhibitionist.CharacterCustomization.Core.Backends
{
	// Token: 0x02000B1F RID: 2847
	public abstract class CharacterCustomizationBackendBase : MonoBehaviour, ICharacterCustomizationBackend
	{
		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x060050A1 RID: 20641
		public abstract string BackendId { get; }

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x060050A2 RID: 20642
		public abstract CustomizationFeature SupportedFeatures { get; }

		// Token: 0x17000D69 RID: 3433
		// (get) Token: 0x060050A3 RID: 20643 RVA: 0x001F352F File Offset: 0x001F172F
		// (set) Token: 0x060050A4 RID: 20644 RVA: 0x001F3537 File Offset: 0x001F1737
		public bool IsInitialized { get; protected set; }

		// Token: 0x17000D6A RID: 3434
		// (get) Token: 0x060050A5 RID: 20645 RVA: 0x001F3540 File Offset: 0x001F1740
		// (set) Token: 0x060050A6 RID: 20646 RVA: 0x001F3548 File Offset: 0x001F1748
		private protected CustomizationSessionContext SessionContext { protected get; private set; }

		// Token: 0x060050A7 RID: 20647
		public abstract bool CanInitialize(CustomizationSessionContext context);

		// Token: 0x060050A8 RID: 20648 RVA: 0x001F3551 File Offset: 0x001F1751
		public virtual bool Initialize(CustomizationSessionContext context)
		{
			if (context == null)
			{
				return false;
			}
			this.SessionContext = context;
			this.IsInitialized = true;
			return true;
		}

		// Token: 0x060050A9 RID: 20649 RVA: 0x001F3567 File Offset: 0x001F1767
		public virtual void Shutdown()
		{
			this.IsInitialized = false;
			this.SessionContext = null;
		}

		// Token: 0x060050AA RID: 20650
		public abstract bool TrySetBlendShape(string blendShapeName, float value);

		// Token: 0x060050AB RID: 20651
		public abstract bool TryGetBlendShape(string blendShapeName, out float value);

		// Token: 0x060050AC RID: 20652 RVA: 0x001F3577 File Offset: 0x001F1777
		public virtual CharacterStateData CaptureState()
		{
			return new CharacterStateData();
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x001F357E File Offset: 0x001F177E
		public virtual bool ApplyState(CharacterStateData stateData)
		{
			return stateData != null;
		}
	}
}
