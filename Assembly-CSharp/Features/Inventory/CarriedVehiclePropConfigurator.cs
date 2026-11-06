using System;
using Invector.vCharacterController.vActions;
using RayznGames.VehicleSystem;
using UnityEngine;

namespace Features.Inventory
{
	// Token: 0x020010E3 RID: 4323
	public static class CarriedVehiclePropConfigurator
	{
		// Token: 0x0600831F RID: 33567 RVA: 0x003580E0 File Offset: 0x003562E0
		public static void ConfigureAsCarriedProp(GameObject instance)
		{
			if (instance == null)
			{
				return;
			}
			CarriedVehiclePropConfigurator.DisableAll<vVehicleController>(instance);
			CarriedVehiclePropConfigurator.DisableAll<vVehicleInteractable>(instance);
			CarriedVehiclePropConfigurator.DisableAll<vBikeController>(instance);
			CarriedVehiclePropConfigurator.DisableAll<vBikeInteractable>(instance);
			foreach (MonoBehaviour monoBehaviour in instance.GetComponentsInChildren<MonoBehaviour>(true))
			{
				if (monoBehaviour is IVehicle)
				{
					monoBehaviour.enabled = false;
				}
			}
			WheelCollider[] componentsInChildren2 = instance.GetComponentsInChildren<WheelCollider>(true);
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
			Collider[] componentsInChildren3 = instance.GetComponentsInChildren<Collider>(true);
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].enabled = false;
			}
			foreach (Rigidbody rigidbody in instance.GetComponentsInChildren<Rigidbody>(true))
			{
				if (!rigidbody.isKinematic)
				{
					rigidbody.velocity = Vector3.zero;
					rigidbody.angularVelocity = Vector3.zero;
				}
				rigidbody.isKinematic = true;
				rigidbody.useGravity = false;
				rigidbody.constraints = RigidbodyConstraints.FreezeAll;
			}
			foreach (AudioSource audioSource in instance.GetComponentsInChildren<AudioSource>(true))
			{
				if (audioSource.isPlaying)
				{
					audioSource.Stop();
				}
				audioSource.playOnAwake = false;
			}
		}

		// Token: 0x06008320 RID: 33568 RVA: 0x00358208 File Offset: 0x00356408
		private static void DisableAll<T>(GameObject root) where T : Behaviour
		{
			T[] componentsInChildren = root.GetComponentsInChildren<T>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}
	}
}
