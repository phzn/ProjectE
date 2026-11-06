using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000018 RID: 24
	[RequiredByNativeCode(Optional = true, GenerateProxy = true)]
	[NativeClass("ContactFilter", "struct ContactFilter;")]
	[NativeHeader("Modules/Physics2D/Public/Collider2D.h")]
	[Serializable]
	public struct ContactFilter2D
	{
		// Token: 0x06000215 RID: 533 RVA: 0x00006630 File Offset: 0x00004830
		public ContactFilter2D NoFilter()
		{
			this.useTriggers = true;
			this.useLayerMask = false;
			this.layerMask = -1;
			this.useDepth = false;
			this.useOutsideDepth = false;
			this.minDepth = float.NegativeInfinity;
			this.maxDepth = float.PositiveInfinity;
			this.useNormalAngle = false;
			this.useOutsideNormalAngle = false;
			this.minNormalAngle = 0f;
			this.maxNormalAngle = 359.9999f;
			return this;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x000066AA File Offset: 0x000048AA
		private void CheckConsistency()
		{
			ContactFilter2D.CheckConsistency_Injected(ref this);
		}

		// Token: 0x06000217 RID: 535 RVA: 0x000066B2 File Offset: 0x000048B2
		public void ClearLayerMask()
		{
			this.useLayerMask = false;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x000066BC File Offset: 0x000048BC
		public void SetLayerMask(LayerMask layerMask)
		{
			this.layerMask = layerMask;
			this.useLayerMask = true;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x000066CD File Offset: 0x000048CD
		public void ClearDepth()
		{
			this.useDepth = false;
		}

		// Token: 0x0600021A RID: 538 RVA: 0x000066D7 File Offset: 0x000048D7
		public void SetDepth(float minDepth, float maxDepth)
		{
			this.minDepth = minDepth;
			this.maxDepth = maxDepth;
			this.useDepth = true;
			this.CheckConsistency();
		}

		// Token: 0x0600021B RID: 539 RVA: 0x000066F6 File Offset: 0x000048F6
		public void ClearNormalAngle()
		{
			this.useNormalAngle = false;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00006700 File Offset: 0x00004900
		public void SetNormalAngle(float minNormalAngle, float maxNormalAngle)
		{
			this.minNormalAngle = minNormalAngle;
			this.maxNormalAngle = maxNormalAngle;
			this.useNormalAngle = true;
			this.CheckConsistency();
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00006720 File Offset: 0x00004920
		public bool isFiltering
		{
			get
			{
				return !this.useTriggers || this.useLayerMask || this.useDepth || this.useNormalAngle;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00006754 File Offset: 0x00004954
		public bool IsFilteringTrigger([Writable] Collider2D collider)
		{
			return !this.useTriggers && collider.isTrigger;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00006778 File Offset: 0x00004978
		public bool IsFilteringLayerMask(GameObject obj)
		{
			return this.useLayerMask && (this.layerMask & (1 << obj.layer)) == 0;
		}

		// Token: 0x06000220 RID: 544 RVA: 0x000067B0 File Offset: 0x000049B0
		public bool IsFilteringDepth(GameObject obj)
		{
			bool flag = !this.useDepth;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = this.minDepth > this.maxDepth;
				if (flag3)
				{
					float num = this.minDepth;
					this.minDepth = this.maxDepth;
					this.maxDepth = num;
				}
				float z = obj.transform.position.z;
				bool flag4 = z < this.minDepth || z > this.maxDepth;
				bool flag5 = this.useOutsideDepth;
				if (flag5)
				{
					flag2 = !flag4;
				}
				else
				{
					flag2 = flag4;
				}
			}
			return flag2;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00006840 File Offset: 0x00004A40
		public bool IsFilteringNormalAngle(Vector2 normal)
		{
			return ContactFilter2D.IsFilteringNormalAngle_Injected(ref this, ref normal);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000684C File Offset: 0x00004A4C
		public bool IsFilteringNormalAngle(float angle)
		{
			return this.IsFilteringNormalAngleUsingAngle(angle);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00006865 File Offset: 0x00004A65
		private bool IsFilteringNormalAngleUsingAngle(float angle)
		{
			return ContactFilter2D.IsFilteringNormalAngleUsingAngle_Injected(ref this, angle);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x00006870 File Offset: 0x00004A70
		internal static ContactFilter2D CreateLegacyFilter(int layerMask, float minDepth, float maxDepth)
		{
			ContactFilter2D contactFilter2D = default(ContactFilter2D);
			contactFilter2D.useTriggers = Physics2D.queriesHitTriggers;
			contactFilter2D.SetLayerMask(layerMask);
			contactFilter2D.SetDepth(minDepth, maxDepth);
			return contactFilter2D;
		}

		// Token: 0x06000225 RID: 549
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void CheckConsistency_Injected(ref ContactFilter2D _unity_self);

		// Token: 0x06000226 RID: 550
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsFilteringNormalAngle_Injected(ref ContactFilter2D _unity_self, ref Vector2 normal);

		// Token: 0x06000227 RID: 551
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool IsFilteringNormalAngleUsingAngle_Injected(ref ContactFilter2D _unity_self, float angle);

		// Token: 0x0400005F RID: 95
		[NativeName("m_UseTriggers")]
		public bool useTriggers;

		// Token: 0x04000060 RID: 96
		[NativeName("m_UseLayerMask")]
		public bool useLayerMask;

		// Token: 0x04000061 RID: 97
		[NativeName("m_UseDepth")]
		public bool useDepth;

		// Token: 0x04000062 RID: 98
		[NativeName("m_UseOutsideDepth")]
		public bool useOutsideDepth;

		// Token: 0x04000063 RID: 99
		[NativeName("m_UseNormalAngle")]
		public bool useNormalAngle;

		// Token: 0x04000064 RID: 100
		[NativeName("m_UseOutsideNormalAngle")]
		public bool useOutsideNormalAngle;

		// Token: 0x04000065 RID: 101
		[NativeName("m_LayerMask")]
		public LayerMask layerMask;

		// Token: 0x04000066 RID: 102
		[NativeName("m_MinDepth")]
		public float minDepth;

		// Token: 0x04000067 RID: 103
		[NativeName("m_MaxDepth")]
		public float maxDepth;

		// Token: 0x04000068 RID: 104
		[NativeName("m_MinNormalAngle")]
		public float minNormalAngle;

		// Token: 0x04000069 RID: 105
		[NativeName("m_MaxNormalAngle")]
		public float maxNormalAngle;

		// Token: 0x0400006A RID: 106
		public const float NormalAngleUpperLimit = 359.9999f;
	}
}
