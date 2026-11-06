using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dinopunch.LivingMud
{
	// Token: 0x02000762 RID: 1890
	public class StampOverDistance : MonoBehaviour, ICheckInMud, IPlaceStamp
	{
		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x060030DE RID: 12510 RVA: 0x00113570 File Offset: 0x00111770
		public DynamicMudSurface Surface
		{
			get
			{
				return this.surface;
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x060030DF RID: 12511 RVA: 0x00113578 File Offset: 0x00111778
		public bool InMud
		{
			get
			{
				return this.inMud;
			}
		}

		// Token: 0x060030E0 RID: 12512 RVA: 0x00113580 File Offset: 0x00111780
		public void Start()
		{
			this.newPosition = (this.oldPosition = base.transform.position);
			this.newDirection = (this.oldDirection = base.transform.forward);
			this.newPosition = (this.oldPosition = base.transform.position);
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x001135DC File Offset: 0x001117DC
		public void FixedUpdate()
		{
			this.newPosition = base.transform.position;
			if (Mathf.Abs(Vector3.Distance(this.newPosition, this.oldPosition)) > this.stampDistance && this.inMud)
			{
				this.UpdateTransformData();
				this.PlaceStamp(this.stamp);
			}
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x00113634 File Offset: 0x00111834
		public void OnTriggerEnter(Collider other)
		{
			DynamicMudSurface componentInParent = other.GetComponentInParent<DynamicMudSurface>();
			if (componentInParent == null)
			{
				return;
			}
			this.AddSurfaceOverlap(componentInParent);
			DynamicMudSurface dynamicMudSurface = this.surface;
			this.surface = this.GetPreferredSurface();
			this.inMud = true;
			if (this.surface != null && this.surface != dynamicMudSurface)
			{
				this.PlaceStamp(this.stamp);
			}
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x00113698 File Offset: 0x00111898
		public void OnTriggerExit(Collider other)
		{
			DynamicMudSurface componentInParent = other.GetComponentInParent<DynamicMudSurface>();
			if (componentInParent == null)
			{
				return;
			}
			this.RemoveSurfaceOverlap(componentInParent);
			DynamicMudSurface dynamicMudSurface = this.surface;
			this.surface = this.GetPreferredSurface();
			this.inMud = this.surface != null;
			if (this.surface != null && this.surface != dynamicMudSurface)
			{
				this.PlaceStamp(this.stamp);
			}
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x00113704 File Offset: 0x00111904
		public void SetMudSurface(DynamicMudSurface surface)
		{
			this.surface = surface;
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x0011370D File Offset: 0x0011190D
		public float GetRotationDifference()
		{
			return this.newRot - this.oldRot;
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x0011371C File Offset: 0x0011191C
		public float GetRotationFromTravel()
		{
			float num = Mathf.Atan2(this.newDirection.x, this.newDirection.z) * 57.29578f;
			if (num < 0f)
			{
				num += 360f;
			}
			return num + this.angleOffset;
		}

		// Token: 0x060030E7 RID: 12519 RVA: 0x00113764 File Offset: 0x00111964
		public void UpdateTransformData()
		{
			this.oldDirection = this.newDirection;
			this.newDirection = this.newPosition - this.oldPosition;
			this.oldRot = this.newRot;
			if (this.useDirectionForRotation)
			{
				this.newRot = this.GetRotationFromTravel();
				this.stamp.transform.eulerAngles = new Vector3(0f, this.newRot, 0f);
			}
			else
			{
				this.newRot = base.transform.localEulerAngles.y;
				if (this.newRot > 180f)
				{
					this.newRot = 360f - this.newRot;
				}
			}
			this.oldPosition = base.transform.position;
		}

		// Token: 0x060030E8 RID: 12520 RVA: 0x00113821 File Offset: 0x00111A21
		public void PlaceStamp(MudStamp stamp = null)
		{
			if (!stamp || !this.surface)
			{
				return;
			}
			stamp.SetSurface(this.surface);
			if (this.bendWithRotation)
			{
				stamp.SetBendAmount(this.GetRotationDifference());
			}
			stamp.StampSurface();
		}

		// Token: 0x060030E9 RID: 12521 RVA: 0x00113860 File Offset: 0x00111A60
		private void AddSurfaceOverlap(DynamicMudSurface newSurface)
		{
			if (newSurface == null)
			{
				return;
			}
			int num;
			if (!this.overlappingSurfaces.TryGetValue(newSurface, out num))
			{
				num = 0;
			}
			this.overlappingSurfaces[newSurface] = num + 1;
		}

		// Token: 0x060030EA RID: 12522 RVA: 0x00113898 File Offset: 0x00111A98
		private void RemoveSurfaceOverlap(DynamicMudSurface exitedSurface)
		{
			if (exitedSurface == null)
			{
				return;
			}
			int num;
			if (!this.overlappingSurfaces.TryGetValue(exitedSurface, out num))
			{
				return;
			}
			num--;
			if (num <= 0)
			{
				this.overlappingSurfaces.Remove(exitedSurface);
				return;
			}
			this.overlappingSurfaces[exitedSurface] = num;
		}

		// Token: 0x060030EB RID: 12523 RVA: 0x001138E4 File Offset: 0x00111AE4
		private DynamicMudSurface GetPreferredSurface()
		{
			DynamicMudSurface dynamicMudSurface = null;
			int num = int.MinValue;
			foreach (KeyValuePair<DynamicMudSurface, int> keyValuePair in this.overlappingSurfaces)
			{
				if (!(keyValuePair.Key == null) && keyValuePair.Value > 0)
				{
					int transformDepth = StampOverDistance.GetTransformDepth(keyValuePair.Key.transform);
					if (transformDepth > num)
					{
						num = transformDepth;
						dynamicMudSurface = keyValuePair.Key;
					}
				}
			}
			return dynamicMudSurface;
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x00113978 File Offset: 0x00111B78
		private static int GetTransformDepth(Transform transform)
		{
			int num = 0;
			while (transform != null)
			{
				num++;
				transform = transform.parent;
			}
			return num;
		}

		// Token: 0x0400314A RID: 12618
		[SerializeField]
		private MudStamp stamp;

		// Token: 0x0400314B RID: 12619
		[SerializeField]
		private DynamicMudSurface surface;

		// Token: 0x0400314C RID: 12620
		[Tooltip("How far must the object travel to place a new stamp")]
		[SerializeField]
		private float stampDistance = 0.2f;

		// Token: 0x0400314D RID: 12621
		[Tooltip("Should the shader attempt to warp the texture to bend toward the current forward direction")]
		[SerializeField]
		private bool bendWithRotation = true;

		// Token: 0x0400314E RID: 12622
		[Tooltip("Offset to control the bend direction")]
		[SerializeField]
		private float angleOffset = -180f;

		// Token: 0x0400314F RID: 12623
		private bool useDirectionForRotation = true;

		// Token: 0x04003150 RID: 12624
		private bool inMud;

		// Token: 0x04003151 RID: 12625
		private Vector3 oldPosition;

		// Token: 0x04003152 RID: 12626
		private Vector3 newPosition;

		// Token: 0x04003153 RID: 12627
		private Vector3 oldDirection;

		// Token: 0x04003154 RID: 12628
		private Vector3 newDirection;

		// Token: 0x04003155 RID: 12629
		private float oldRot;

		// Token: 0x04003156 RID: 12630
		private float newRot;

		// Token: 0x04003157 RID: 12631
		private readonly Dictionary<DynamicMudSurface, int> overlappingSurfaces = new Dictionary<DynamicMudSurface, int>();
	}
}
