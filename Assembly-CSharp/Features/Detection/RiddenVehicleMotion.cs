using System;
using Invector.vCharacterController;
using Invector.vCharacterController.vActions;
using UnityEngine;

namespace Features.Detection
{
	// Token: 0x020012DE RID: 4830
	public sealed class RiddenVehicleMotion
	{
		// Token: 0x060095BB RID: 38331 RVA: 0x003D799C File Offset: 0x003D5B9C
		public bool TryGetPlanarSpeed(vThirdPersonController rider, out float speed)
		{
			speed = 0f;
			if (rider == null)
			{
				return false;
			}
			if (!RiddenVehicleMotion.IsRiddenBy(this._vehicle, rider))
			{
				this._vehicle = this.FindVehicle(rider);
				this._body = ((this._vehicle != null) ? this._vehicle.GetComponent<Rigidbody>() : null);
			}
			if (this._vehicle == null || this._body == null)
			{
				return false;
			}
			Vector3 velocity = this._body.velocity;
			speed = new Vector2(velocity.x, velocity.z).magnitude;
			return true;
		}

		// Token: 0x060095BC RID: 38332 RVA: 0x003D7A40 File Offset: 0x003D5C40
		private Component FindVehicle(vThirdPersonController rider)
		{
			Transform transform = rider.transform.parent;
			while (transform != null)
			{
				vVehicleController component = transform.GetComponent<vVehicleController>();
				if (RiddenVehicleMotion.IsRiddenBy(component, rider))
				{
					return component;
				}
				vBikeController component2 = transform.GetComponent<vBikeController>();
				if (RiddenVehicleMotion.IsRiddenBy(component2, rider))
				{
					return component2;
				}
				SkateboardInteractable component3 = transform.GetComponent<SkateboardInteractable>();
				if (RiddenVehicleMotion.IsRiddenBy(component3, rider))
				{
					return component3;
				}
				SimpleRollerSkatesInteractable component4 = transform.GetComponent<SimpleRollerSkatesInteractable>();
				if (RiddenVehicleMotion.IsRiddenBy(component4, rider))
				{
					return component4;
				}
				transform = transform.parent;
			}
			for (int i = 0; i < rider.transform.childCount; i++)
			{
				vVehicleController component5 = rider.transform.GetChild(i).GetComponent<vVehicleController>();
				if (RiddenVehicleMotion.IsRiddenBy(component5, rider))
				{
					return component5;
				}
			}
			return null;
		}

		// Token: 0x060095BD RID: 38333 RVA: 0x003D7AF4 File Offset: 0x003D5CF4
		private static bool IsRiddenBy(Component vehicle, vThirdPersonController rider)
		{
			if (!(vehicle == null))
			{
				Behaviour behaviour = vehicle as Behaviour;
				if (behaviour != null && behaviour.isActiveAndEnabled)
				{
					vVehicleController vVehicleController = vehicle as vVehicleController;
					if (vVehicleController != null)
					{
						return vVehicleController.IsBeingRidden && vVehicleController.CurrentRider == rider;
					}
					vBikeController vBikeController = vehicle as vBikeController;
					if (vBikeController != null)
					{
						return vBikeController.IsBeingRidden && vBikeController.CurrentRider == rider;
					}
					SkateboardInteractable skateboardInteractable = vehicle as SkateboardInteractable;
					if (skateboardInteractable != null)
					{
						return skateboardInteractable.IsBeingWorn && skateboardInteractable.CurrentRider == rider;
					}
					SimpleRollerSkatesInteractable simpleRollerSkatesInteractable = vehicle as SimpleRollerSkatesInteractable;
					return simpleRollerSkatesInteractable != null && simpleRollerSkatesInteractable.IsBeingWorn && simpleRollerSkatesInteractable.CurrentRider == rider;
				}
			}
			return false;
		}

		// Token: 0x04009ADF RID: 39647
		private Component _vehicle;

		// Token: 0x04009AE0 RID: 39648
		private Rigidbody _body;
	}
}
