using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Invector.vCharacterController.vActions
{
	// Token: 0x02000705 RID: 1797
	public sealed class VehicleDisableDismount : MonoBehaviour
	{
		// Token: 0x06002E30 RID: 11824 RVA: 0x00103B6A File Offset: 0x00101D6A
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void ResetSession()
		{
			VehicleDisableDismount._instance = null;
			VehicleDisableDismount._quitting = false;
			Application.quitting -= VehicleDisableDismount.OnQuitting;
			Application.quitting += VehicleDisableDismount.OnQuitting;
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x00103B9A File Offset: 0x00101D9A
		private static void OnQuitting()
		{
			VehicleDisableDismount._quitting = true;
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06002E32 RID: 11826 RVA: 0x00103BA2 File Offset: 0x00101DA2
		private static bool SessionIsRunning
		{
			get
			{
				return !VehicleDisableDismount._quitting && Application.isPlaying;
			}
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x00103BB8 File Offset: 0x00101DB8
		public static void Request(vVehicleController vehicle)
		{
			if (!VehicleDisableDismount.SessionIsRunning || vehicle == null || !vehicle.IsBeingRidden)
			{
				return;
			}
			if (VehicleDisableDismount._instance == null)
			{
				GameObject gameObject = new GameObject("Vehicle disable dismounts");
				Object.DontDestroyOnLoad(gameObject);
				VehicleDisableDismount._instance = gameObject.AddComponent<VehicleDisableDismount>();
			}
			if (VehicleDisableDismount._instance._pending.Add(vehicle))
			{
				VehicleDisableDismount._instance.StartCoroutine(VehicleDisableDismount._instance.CompleteAfterDisable(vehicle, vehicle.CurrentRider));
			}
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x00103C33 File Offset: 0x00101E33
		private IEnumerator CompleteAfterDisable(vVehicleController vehicle, vThirdPersonController rider)
		{
			yield return null;
			try
			{
				if (VehicleDisableDismount.SessionIsRunning && vehicle != null && rider != null && vehicle.gameObject.scene.isLoaded && rider.gameObject.scene.isLoaded && vehicle.IsBeingRidden && vehicle.CurrentRider == rider)
				{
					vehicle.ForceDismount();
				}
				yield break;
			}
			finally
			{
				this._pending.Remove(vehicle);
			}
			yield break;
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x00103C50 File Offset: 0x00101E50
		private void OnDestroy()
		{
			if (VehicleDisableDismount._instance == this)
			{
				VehicleDisableDismount._instance = null;
			}
		}

		// Token: 0x04002E7D RID: 11901
		private static VehicleDisableDismount _instance;

		// Token: 0x04002E7E RID: 11902
		private static bool _quitting;

		// Token: 0x04002E7F RID: 11903
		private readonly HashSet<vVehicleController> _pending = new HashSet<vVehicleController>();
	}
}
