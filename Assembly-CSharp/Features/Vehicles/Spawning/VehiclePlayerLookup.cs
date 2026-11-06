using System;
using Invector.vCharacterController;
using ProjectExhibitionist.Core.SceneManagement;
using UnityEngine;

namespace Features.Vehicles.Spawning
{
	// Token: 0x02000B89 RID: 2953
	public sealed class VehiclePlayerLookup
	{
		// Token: 0x0600544D RID: 21581 RVA: 0x0020B5E8 File Offset: 0x002097E8
		public Transform Resolve()
		{
			vThirdPersonController vThirdPersonController = VehiclePlayerLookup.ResolveCandidate(PlayerRegistry.Player, ref this._registeredRoot, ref this._registeredController);
			if (vThirdPersonController != null)
			{
				return vThirdPersonController.transform;
			}
			PlayerProximityProvider instance = PlayerProximityProvider.Instance;
			vThirdPersonController = VehiclePlayerLookup.ResolveCandidate((instance != null) ? instance.Player : null, ref this._proximityRoot, ref this._proximityController);
			if (vThirdPersonController != null)
			{
				return vThirdPersonController.transform;
			}
			if (!VehiclePlayerLookup.IsUsable(this._fallbackController))
			{
				this._fallbackController = Object.FindObjectOfType<vThirdPersonController>();
			}
			if (!VehiclePlayerLookup.IsUsable(this._fallbackController))
			{
				return null;
			}
			return this._fallbackController.transform;
		}

		// Token: 0x0600544E RID: 21582 RVA: 0x0020B688 File Offset: 0x00209888
		private static vThirdPersonController ResolveCandidate(Transform source, ref Transform cachedSource, ref vThirdPersonController cachedController)
		{
			if (source == null || !source.gameObject.activeInHierarchy)
			{
				cachedSource = null;
				cachedController = null;
				return null;
			}
			if (source == cachedSource && VehiclePlayerLookup.IsUsable(cachedController))
			{
				Transform transform = cachedController.transform;
				if (transform == source || transform.IsChildOf(source) || source.IsChildOf(transform))
				{
					return cachedController;
				}
			}
			cachedSource = source;
			if (!source.TryGetComponent<vThirdPersonController>(out cachedController))
			{
				cachedController = source.GetComponentInParent<vThirdPersonController>();
				if (!VehiclePlayerLookup.IsUsable(cachedController))
				{
					cachedController = source.GetComponentInChildren<vThirdPersonController>();
				}
			}
			if (!VehiclePlayerLookup.IsUsable(cachedController))
			{
				return null;
			}
			return cachedController;
		}

		// Token: 0x0600544F RID: 21583 RVA: 0x0020B71E File Offset: 0x0020991E
		private static bool IsUsable(vThirdPersonController controller)
		{
			return controller != null && controller.gameObject.activeInHierarchy;
		}

		// Token: 0x0400549F RID: 21663
		private Transform _registeredRoot;

		// Token: 0x040054A0 RID: 21664
		private vThirdPersonController _registeredController;

		// Token: 0x040054A1 RID: 21665
		private Transform _proximityRoot;

		// Token: 0x040054A2 RID: 21666
		private vThirdPersonController _proximityController;

		// Token: 0x040054A3 RID: 21667
		private vThirdPersonController _fallbackController;
	}
}
