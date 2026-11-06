using System;
using UnityEngine;

namespace Invector
{
	// Token: 0x020004C7 RID: 1223
	[vClassHeader("Check Effect Visibility", "Use it to identify if the object is within the camera frustrum and not hide by obstacles", openClose = false)]
	public class vCheckEffectVisibility : vMonoBehaviour
	{
		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06001D92 RID: 7570 RVA: 0x000B58DE File Offset: 0x000B3ADE
		private Vector3 visibilityPoint
		{
			get
			{
				return base.transform.TransformPoint(this.checkPoint);
			}
		}

		// Token: 0x06001D93 RID: 7571 RVA: 0x000B58F1 File Offset: 0x000B3AF1
		private void OnDrawGizmos()
		{
			if (this.debugMode)
			{
				Gizmos.DrawSphere(this.visibilityPoint, 0.1f);
			}
		}

		// Token: 0x06001D94 RID: 7572 RVA: 0x000B590B File Offset: 0x000B3B0B
		private void Start()
		{
			this.mainCamera = Camera.main;
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x000B5918 File Offset: 0x000B3B18
		public void CheckEffectIsVisible(vIEffect effect)
		{
			if (this.CheckIfEffectIsVisible(effect))
			{
				this.OnVisible.Invoke(effect);
				return;
			}
			this.OnNotVisible.Invoke(effect);
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x000B593C File Offset: 0x000B3B3C
		private bool IsObjectVisible(Camera camera, Vector3 position)
		{
			Vector3 vector = camera.WorldToViewportPoint(position);
			return vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f && vector.z >= 0f;
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000B5998 File Offset: 0x000B3B98
		private bool CheckIfEffectIsVisible(vIEffect effect)
		{
			if (!(this.mainCamera != null))
			{
				return false;
			}
			if (this.IsObjectVisible(this.mainCamera, effect.EffectPosition))
			{
				RaycastHit raycastHit;
				if (Physics.Linecast(effect.EffectPosition, this.visibilityPoint, out raycastHit, this.layerObstacle, QueryTriggerInteraction.Ignore))
				{
					if (raycastHit.collider.gameObject != base.gameObject && raycastHit.distance >= 1f)
					{
						if (this.debugMode)
						{
							Debug.DrawLine(effect.EffectPosition, raycastHit.point, Color.red, effect.EffectDuration);
						}
						if (this.debugMode)
						{
							Debug.Log("Object is not in view because of a obstacle: " + raycastHit.collider.name, raycastHit.collider.gameObject);
						}
						return false;
					}
				}
				else if (this.debugMode)
				{
					Debug.DrawLine(effect.EffectPosition, this.visibilityPoint, Color.green, effect.EffectDuration);
				}
				if (this.debugMode)
				{
					Debug.Log("Object is in view!");
				}
				return true;
			}
			if (this.debugMode)
			{
				Debug.Log("Object is in view!");
			}
			return true;
		}

		// Token: 0x04001EB8 RID: 7864
		public LayerMask layerObstacle;

		// Token: 0x04001EB9 RID: 7865
		[Tooltip("The point to check if effect is visible")]
		public Vector3 checkPoint = new Vector3(0f, 1.5f, 0f);

		// Token: 0x04001EBA RID: 7866
		public bool debugMode;

		// Token: 0x04001EBB RID: 7867
		public vEffectReceiver.vEffectEvent OnVisible;

		// Token: 0x04001EBC RID: 7868
		public vEffectReceiver.vEffectEvent OnNotVisible;

		// Token: 0x04001EBD RID: 7869
		private Camera mainCamera;
	}
}
