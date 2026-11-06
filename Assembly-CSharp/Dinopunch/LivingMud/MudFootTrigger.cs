using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000744 RID: 1860
	[RequireComponent(typeof(Collider))]
	public class MudFootTrigger : MonoBehaviour
	{
		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x001108BF File Offset: 0x0010EABF
		public bool IsInMud
		{
			get
			{
				return this.currentSurface != null;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x001108CD File Offset: 0x0010EACD
		public DynamicMudSurface CurrentSurface
		{
			get
			{
				return this.currentSurface;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x001108D8 File Offset: 0x0010EAD8
		public MudType ContactMudTypes
		{
			get
			{
				if (this.useRaycastFallback)
				{
					return this.raycastContactTypes;
				}
				MudType mudType = (MudType)0;
				foreach (KeyValuePair<DynamicMudSurface, int> keyValuePair in this.overlapping)
				{
					if (keyValuePair.Key != null && keyValuePair.Value > 0)
					{
						mudType |= keyValuePair.Key.Thickness;
					}
				}
				return mudType;
			}
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x00110960 File Offset: 0x0010EB60
		private void Awake()
		{
			Collider component = base.GetComponent<Collider>();
			if (!component.isTrigger)
			{
				component.isTrigger = true;
				Debug.LogWarning("[MudFootTrigger] Collider on '" + base.name + "' forced to isTrigger=true.", this);
			}
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x0011099E File Offset: 0x0010EB9E
		private bool IsLayerAllowed(GameObject go)
		{
			return (this.mudSurfaceMask.value & (1 << go.layer)) != 0;
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x001109BA File Offset: 0x0010EBBA
		private void Update()
		{
			if (this.useRaycastFallback)
			{
				this.UpdateRaycastSurface();
			}
			this.currentSurface = this.GetPreferredSurface();
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x001109D8 File Offset: 0x0010EBD8
		private void UpdateRaycastSurface()
		{
			this.raycastSurface = null;
			this.raycastContactTypes = (MudType)0;
			Ray ray = new Ray(base.transform.position + Vector3.up * 0.2f, Vector3.down);
			RaycastHit[] array = Physics.SphereCastAll(ray, this.raycastRadius, this.raycastDistance + 0.2f, this.raycastMask, QueryTriggerInteraction.Collide);
			if (array.Length != 0)
			{
				DynamicMudSurface dynamicMudSurface = null;
				foreach (RaycastHit raycastHit in array)
				{
					DynamicMudSurface componentInParent = raycastHit.collider.GetComponentInParent<DynamicMudSurface>();
					if (!(componentInParent == null) && this.IsLayerAllowed(componentInParent.gameObject))
					{
						bool flag = (componentInParent.Thickness & MudType.Watery) > (MudType)0;
						this.raycastContactTypes |= componentInParent.Thickness;
						if (this.debugPhysics)
						{
							Debug.Log(string.Format("[MudFootTrigger] Raycast hit '{0}' -> Surface '{1}' ({2})", raycastHit.collider.name, componentInParent.name, componentInParent.Thickness), raycastHit.collider.gameObject);
						}
						if (flag)
						{
							dynamicMudSurface = componentInParent;
						}
						else if (dynamicMudSurface == null)
						{
							dynamicMudSurface = componentInParent;
						}
					}
				}
				this.raycastSurface = dynamicMudSurface;
				return;
			}
			if (this.debugPhysics)
			{
				Debug.DrawRay(ray.origin, ray.direction * (this.raycastDistance + 0.2f), Color.red);
			}
		}

		// Token: 0x06003022 RID: 12322 RVA: 0x00110B48 File Offset: 0x0010ED48
		private void OnTriggerEnter(Collider other)
		{
			if (this.debugPhysics)
			{
				Debug.Log(string.Concat(new string[]
				{
					"[MudFootTrigger] OnTriggerEnter with '",
					other.name,
					"' (Layer: ",
					LayerMask.LayerToName(other.gameObject.layer),
					")"
				}), other.gameObject);
			}
			if (!this.IsLayerAllowed(other.gameObject))
			{
				if (this.debugPhysics)
				{
					Debug.Log(string.Format("[MudFootTrigger] Layer {0} not in mask {1}. Skipping.", 1 << other.gameObject.layer, this.mudSurfaceMask.value), other.gameObject);
				}
				return;
			}
			DynamicMudSurface componentInParent = other.GetComponentInParent<DynamicMudSurface>();
			if (componentInParent == null)
			{
				if (this.debugPhysics)
				{
					Debug.Log("[MudFootTrigger] No DynamicMudSurface found in parent of '" + other.name + "'. Skipping.", other.gameObject);
				}
				return;
			}
			if (!this.IsLayerAllowed(componentInParent.gameObject))
			{
				if (this.debugPhysics)
				{
					Debug.Log(string.Format("[MudFootTrigger] Surface GO layer {0} not allowed. Skipping.", componentInParent.gameObject.layer), componentInParent.gameObject);
				}
				return;
			}
			int num;
			if (!this.overlapping.TryGetValue(componentInParent, out num))
			{
				num = 0;
			}
			this.overlapping[componentInParent] = num + 1;
			this.currentSurface = this.GetPreferredSurface();
			if (this.debugPhysics)
			{
				Debug.Log(string.Format("[MudFootTrigger] Added surface '{0}' (Thickness: {1}). Current preferred: {2}", componentInParent.name, componentInParent.Thickness, (this.currentSurface != null) ? this.currentSurface.name : "null"), componentInParent.gameObject);
			}
		}

		// Token: 0x06003023 RID: 12323 RVA: 0x00110CE8 File Offset: 0x0010EEE8
		private void OnTriggerExit(Collider other)
		{
			DynamicMudSurface componentInParent = other.GetComponentInParent<DynamicMudSurface>();
			if (componentInParent == null)
			{
				return;
			}
			int num;
			if (!this.overlapping.TryGetValue(componentInParent, out num))
			{
				return;
			}
			num--;
			if (num <= 0)
			{
				this.overlapping.Remove(componentInParent);
			}
			else
			{
				this.overlapping[componentInParent] = num;
			}
			this.currentSurface = this.GetPreferredSurface();
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x00110D48 File Offset: 0x0010EF48
		private DynamicMudSurface GetPreferredSurface()
		{
			if (this.useRaycastFallback)
			{
				return this.raycastSurface;
			}
			DynamicMudSurface dynamicMudSurface = null;
			int num = int.MinValue;
			bool flag = false;
			if (this.raycastSurface != null)
			{
				dynamicMudSurface = this.raycastSurface;
				flag = (this.raycastSurface.Thickness & MudType.Watery) > (MudType)0;
				num = MudFootTrigger.GetTransformDepth(this.raycastSurface.transform);
			}
			foreach (KeyValuePair<DynamicMudSurface, int> keyValuePair in this.overlapping)
			{
				if (!(keyValuePair.Key == null) && keyValuePair.Value > 0 && !(keyValuePair.Key == dynamicMudSurface))
				{
					bool flag2 = (keyValuePair.Key.Thickness & MudType.Watery) > (MudType)0;
					if (dynamicMudSurface != null && flag2 != flag)
					{
						if (flag2)
						{
							dynamicMudSurface = keyValuePair.Key;
							flag = true;
							num = MudFootTrigger.GetTransformDepth(keyValuePair.Key.transform);
						}
					}
					else
					{
						int transformDepth = MudFootTrigger.GetTransformDepth(keyValuePair.Key.transform);
						if (transformDepth > num)
						{
							num = transformDepth;
							dynamicMudSurface = keyValuePair.Key;
							flag = flag2;
						}
					}
				}
			}
			return dynamicMudSurface;
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x00110E80 File Offset: 0x0010F080
		private static int GetTransformDepth(Transform t)
		{
			int num = 0;
			while (t != null)
			{
				num++;
				t = t.parent;
			}
			return num;
		}

		// Token: 0x040030B9 RID: 12473
		[Tooltip("Layers that count as valid mud surfaces. Colliders on excluded layers are ignored.")]
		[SerializeField]
		private LayerMask mudSurfaceMask = -1;

		// Token: 0x040030BA RID: 12474
		[Header("Debug")]
		[SerializeField]
		private bool debugPhysics;

		// Token: 0x040030BB RID: 12475
		[Header("Raycast Detection")]
		[Tooltip("If true, also performs a short raycast downwards to detect surfaces that OnTriggerEnter might miss.")]
		[SerializeField]
		private bool useRaycastFallback = true;

		// Token: 0x040030BC RID: 12476
		[SerializeField]
		private float raycastDistance = 0.3f;

		// Token: 0x040030BD RID: 12477
		[SerializeField]
		private float raycastRadius = 0.1f;

		// Token: 0x040030BE RID: 12478
		[SerializeField]
		private LayerMask raycastMask = -1;

		// Token: 0x040030BF RID: 12479
		private DynamicMudSurface currentSurface;

		// Token: 0x040030C0 RID: 12480
		private DynamicMudSurface raycastSurface;

		// Token: 0x040030C1 RID: 12481
		private MudType raycastContactTypes;

		// Token: 0x040030C2 RID: 12482
		private readonly Dictionary<DynamicMudSurface, int> overlapping = new Dictionary<DynamicMudSurface, int>();
	}
}
