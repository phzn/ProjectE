using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	// Token: 0x0200001E RID: 30
	[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("Cinemachine/CinemachineTargetGroup")]
	[SaveDuringPlay]
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[HelpURL("https://docs.unity3d.com/Packages/com.unity.cinemachine@2.9/manual/CinemachineTargetGroup.html")]
	public class CinemachineTargetGroup : MonoBehaviour, ICinemachineTargetGroup
	{
		// Token: 0x06000145 RID: 325 RVA: 0x0000A4A8 File Offset: 0x000086A8
		private void OnValidate()
		{
			int num = ((this.m_Targets == null) ? 0 : this.m_Targets.Length);
			for (int i = 0; i < num; i++)
			{
				this.m_Targets[i].weight = Mathf.Max(0f, this.m_Targets[i].weight);
				this.m_Targets[i].radius = Mathf.Max(0f, this.m_Targets[i].radius);
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000A52D File Offset: 0x0000872D
		private void Reset()
		{
			this.m_PositionMode = CinemachineTargetGroup.PositionMode.GroupCenter;
			this.m_RotationMode = CinemachineTargetGroup.RotationMode.Manual;
			this.m_UpdateMethod = CinemachineTargetGroup.UpdateMethod.LateUpdate;
			this.m_Targets = Array.Empty<CinemachineTargetGroup.Target>();
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000147 RID: 327 RVA: 0x0000A54F File Offset: 0x0000874F
		public Transform Transform
		{
			get
			{
				return base.transform;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000148 RID: 328 RVA: 0x0000A557 File Offset: 0x00008757
		// (set) Token: 0x06000149 RID: 329 RVA: 0x0000A572 File Offset: 0x00008772
		public Bounds BoundingBox
		{
			get
			{
				if (this.m_LastUpdateFrame != Time.frameCount)
				{
					this.DoUpdate();
				}
				return this.m_BoundingBox;
			}
			private set
			{
				this.m_BoundingBox = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600014A RID: 330 RVA: 0x0000A57B File Offset: 0x0000877B
		// (set) Token: 0x0600014B RID: 331 RVA: 0x0000A596 File Offset: 0x00008796
		public BoundingSphere Sphere
		{
			get
			{
				if (this.m_LastUpdateFrame != Time.frameCount)
				{
					this.DoUpdate();
				}
				return this.m_BoundingSphere;
			}
			private set
			{
				this.m_BoundingSphere = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000A59F File Offset: 0x0000879F
		public bool IsEmpty
		{
			get
			{
				if (this.m_LastUpdateFrame != Time.frameCount)
				{
					this.DoUpdate();
				}
				return this.m_ValidMembers.Count == 0;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000A5C4 File Offset: 0x000087C4
		public void AddMember(Transform t, float weight, float radius)
		{
			int num = 0;
			if (this.m_Targets == null)
			{
				this.m_Targets = new CinemachineTargetGroup.Target[1];
			}
			else
			{
				num = this.m_Targets.Length;
				Array targets = this.m_Targets;
				this.m_Targets = new CinemachineTargetGroup.Target[num + 1];
				Array.Copy(targets, this.m_Targets, num);
			}
			this.m_Targets[num].target = t;
			this.m_Targets[num].weight = weight;
			this.m_Targets[num].radius = radius;
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000A648 File Offset: 0x00008848
		public void RemoveMember(Transform t)
		{
			int num = this.FindMember(t);
			if (num >= 0)
			{
				CinemachineTargetGroup.Target[] targets = this.m_Targets;
				this.m_Targets = new CinemachineTargetGroup.Target[this.m_Targets.Length - 1];
				if (num > 0)
				{
					Array.Copy(targets, this.m_Targets, num);
				}
				if (num < targets.Length - 1)
				{
					Array.Copy(targets, num + 1, this.m_Targets, num, targets.Length - num - 1);
				}
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000A6B0 File Offset: 0x000088B0
		public int FindMember(Transform t)
		{
			if (this.m_Targets != null)
			{
				for (int i = this.m_Targets.Length - 1; i >= 0; i--)
				{
					if (this.m_Targets[i].target == t)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A6F8 File Offset: 0x000088F8
		public BoundingSphere GetWeightedBoundsForMember(int index)
		{
			if (this.m_LastUpdateFrame != Time.frameCount)
			{
				this.DoUpdate();
			}
			if (!this.IndexIsValid(index) || !this.m_MemberValidity[index])
			{
				return this.Sphere;
			}
			return CinemachineTargetGroup.WeightedMemberBoundsForValidMember(ref this.m_Targets[index], this.m_AveragePos, this.m_MaxWeight);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000A754 File Offset: 0x00008954
		public Bounds GetViewSpaceBoundingBox(Matrix4x4 observer)
		{
			if (this.m_LastUpdateFrame != Time.frameCount)
			{
				this.DoUpdate();
			}
			Matrix4x4 matrix4x = observer;
			if (!Matrix4x4.Inverse3DAffine(observer, ref matrix4x))
			{
				matrix4x = observer.inverse;
			}
			Bounds bounds = new Bounds(matrix4x.MultiplyPoint3x4(this.m_AveragePos), Vector3.zero);
			if (this.CachedCountIsValid)
			{
				bool flag = false;
				Vector3 vector = 2f * Vector3.one;
				int count = this.m_ValidMembers.Count;
				for (int i = 0; i < count; i++)
				{
					BoundingSphere boundingSphere = CinemachineTargetGroup.WeightedMemberBoundsForValidMember(ref this.m_Targets[this.m_ValidMembers[i]], this.m_AveragePos, this.m_MaxWeight);
					boundingSphere.position = matrix4x.MultiplyPoint3x4(boundingSphere.position);
					if (flag)
					{
						bounds.Encapsulate(new Bounds(boundingSphere.position, boundingSphere.radius * vector));
					}
					else
					{
						bounds = new Bounds(boundingSphere.position, boundingSphere.radius * vector);
					}
					flag = true;
				}
			}
			return bounds;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000A865 File Offset: 0x00008A65
		private bool CachedCountIsValid
		{
			get
			{
				return this.m_MemberValidity.Count == ((this.m_Targets == null) ? 0 : this.m_Targets.Length);
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000A887 File Offset: 0x00008A87
		private bool IndexIsValid(int index)
		{
			return index >= 0 && this.m_Targets != null && index < this.m_Targets.Length && this.CachedCountIsValid;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000A8A8 File Offset: 0x00008AA8
		private static BoundingSphere WeightedMemberBoundsForValidMember(ref CinemachineTargetGroup.Target t, Vector3 avgPos, float maxWeight)
		{
			Vector3 vector = ((t.target == null) ? avgPos : TargetPositionCache.GetTargetPosition(t.target));
			float num = Mathf.Max(0f, t.weight);
			if (maxWeight > 0.0001f && num < maxWeight)
			{
				num /= maxWeight;
			}
			else
			{
				num = 1f;
			}
			return new BoundingSphere(Vector3.Lerp(avgPos, vector, num), t.radius * num);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000A910 File Offset: 0x00008B10
		public void DoUpdate()
		{
			this.m_LastUpdateFrame = Time.frameCount;
			this.UpdateMemberValidity();
			this.m_AveragePos = this.CalculateAveragePosition();
			this.BoundingBox = this.CalculateBoundingBox();
			this.m_BoundingSphere = this.CalculateBoundingSphere();
			CinemachineTargetGroup.PositionMode positionMode = this.m_PositionMode;
			if (positionMode != CinemachineTargetGroup.PositionMode.GroupCenter)
			{
				if (positionMode == CinemachineTargetGroup.PositionMode.GroupAverage)
				{
					base.transform.position = this.m_AveragePos;
				}
			}
			else
			{
				base.transform.position = this.Sphere.position;
			}
			CinemachineTargetGroup.RotationMode rotationMode = this.m_RotationMode;
			if (rotationMode != CinemachineTargetGroup.RotationMode.Manual && rotationMode == CinemachineTargetGroup.RotationMode.GroupAverage)
			{
				base.transform.rotation = this.CalculateAverageOrientation();
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000A9AC File Offset: 0x00008BAC
		private void UpdateMemberValidity()
		{
			int num = ((this.m_Targets == null) ? 0 : this.m_Targets.Length);
			this.m_ValidMembers.Clear();
			this.m_ValidMembers.Capacity = Mathf.Max(this.m_ValidMembers.Capacity, num);
			this.m_MemberValidity.Clear();
			this.m_MemberValidity.Capacity = Mathf.Max(this.m_MemberValidity.Capacity, num);
			this.m_WeightSum = (this.m_MaxWeight = 0f);
			for (int i = 0; i < num; i++)
			{
				this.m_MemberValidity.Add(this.m_Targets[i].target != null && this.m_Targets[i].weight > 0.0001f && this.m_Targets[i].target.gameObject.activeInHierarchy);
				if (this.m_MemberValidity[i])
				{
					this.m_ValidMembers.Add(i);
					this.m_MaxWeight = Mathf.Max(this.m_MaxWeight, this.m_Targets[i].weight);
					this.m_WeightSum += this.m_Targets[i].weight;
				}
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000AAF8 File Offset: 0x00008CF8
		private Vector3 CalculateAveragePosition()
		{
			if (this.m_WeightSum < 0.0001f)
			{
				return base.transform.position;
			}
			Vector3 vector = Vector3.zero;
			int count = this.m_ValidMembers.Count;
			for (int i = 0; i < count; i++)
			{
				int num = this.m_ValidMembers[i];
				float weight = this.m_Targets[num].weight;
				vector += TargetPositionCache.GetTargetPosition(this.m_Targets[num].target) * weight;
			}
			return vector / this.m_WeightSum;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000AB8C File Offset: 0x00008D8C
		private Bounds CalculateBoundingBox()
		{
			if (this.m_MaxWeight < 0.0001f)
			{
				return this.BoundingBox;
			}
			Bounds bounds = new Bounds(this.m_AveragePos, Vector3.zero);
			int count = this.m_ValidMembers.Count;
			for (int i = 0; i < count; i++)
			{
				BoundingSphere boundingSphere = CinemachineTargetGroup.WeightedMemberBoundsForValidMember(ref this.m_Targets[this.m_ValidMembers[i]], this.m_AveragePos, this.m_MaxWeight);
				bounds.Encapsulate(new Bounds(boundingSphere.position, boundingSphere.radius * 2f * Vector3.one));
			}
			return bounds;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000AC2C File Offset: 0x00008E2C
		private BoundingSphere CalculateBoundingSphere()
		{
			int count = this.m_ValidMembers.Count;
			if (count == 0 || this.m_MaxWeight < 0.0001f)
			{
				return this.m_BoundingSphere;
			}
			BoundingSphere boundingSphere = CinemachineTargetGroup.WeightedMemberBoundsForValidMember(ref this.m_Targets[this.m_ValidMembers[0]], this.m_AveragePos, this.m_MaxWeight);
			for (int i = 1; i < count; i++)
			{
				BoundingSphere boundingSphere2 = CinemachineTargetGroup.WeightedMemberBoundsForValidMember(ref this.m_Targets[this.m_ValidMembers[i]], this.m_AveragePos, this.m_MaxWeight);
				float num = (boundingSphere2.position - boundingSphere.position).magnitude + boundingSphere2.radius;
				if (num > boundingSphere.radius)
				{
					boundingSphere.radius = (boundingSphere.radius + num) * 0.5f;
					boundingSphere.position = (boundingSphere.radius * boundingSphere.position + (num - boundingSphere.radius) * boundingSphere2.position) / num;
				}
			}
			return boundingSphere;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000AD3C File Offset: 0x00008F3C
		private Quaternion CalculateAverageOrientation()
		{
			if (this.m_WeightSum > 0.001f)
			{
				Vector3 vector = Vector3.zero;
				Vector3 vector2 = Vector3.zero;
				int count = this.m_ValidMembers.Count;
				for (int i = 0; i < count; i++)
				{
					int num = this.m_ValidMembers[i];
					float num2 = this.m_Targets[num].weight / this.m_WeightSum;
					Quaternion targetRotation = TargetPositionCache.GetTargetRotation(this.m_Targets[num].target);
					vector += targetRotation * Vector3.forward * num2;
					vector2 += targetRotation * Vector3.up * num2;
				}
				if (vector.sqrMagnitude > 0.0001f && vector2.sqrMagnitude > 0.0001f)
				{
					return Quaternion.LookRotation(vector, vector2);
				}
			}
			return base.transform.rotation;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000AE22 File Offset: 0x00009022
		private void FixedUpdate()
		{
			if (this.m_UpdateMethod == CinemachineTargetGroup.UpdateMethod.FixedUpdate)
			{
				this.DoUpdate();
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000AE33 File Offset: 0x00009033
		private void Update()
		{
			if (!Application.isPlaying || this.m_UpdateMethod == CinemachineTargetGroup.UpdateMethod.Update)
			{
				this.DoUpdate();
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000AE4A File Offset: 0x0000904A
		private void LateUpdate()
		{
			if (this.m_UpdateMethod == CinemachineTargetGroup.UpdateMethod.LateUpdate)
			{
				this.DoUpdate();
			}
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000AE5C File Offset: 0x0000905C
		public void GetViewSpaceAngularBounds(Matrix4x4 observer, out Vector2 minAngles, out Vector2 maxAngles, out Vector2 zRange)
		{
			if (this.m_LastUpdateFrame != Time.frameCount)
			{
				this.DoUpdate();
			}
			Matrix4x4 matrix4x = observer;
			if (!Matrix4x4.Inverse3DAffine(observer, ref matrix4x))
			{
				matrix4x = observer.inverse;
			}
			float radius = this.m_BoundingSphere.radius;
			Bounds bounds = new Bounds
			{
				center = matrix4x.MultiplyPoint3x4(this.m_AveragePos),
				extents = new Vector3(radius, radius, radius)
			};
			zRange = new Vector2(bounds.center.z - radius, bounds.center.z + radius);
			if (this.CachedCountIsValid)
			{
				bool flag = false;
				int count = this.m_ValidMembers.Count;
				for (int i = 0; i < count; i++)
				{
					BoundingSphere boundingSphere = CinemachineTargetGroup.WeightedMemberBoundsForValidMember(ref this.m_Targets[this.m_ValidMembers[i]], this.m_AveragePos, this.m_MaxWeight);
					Vector3 vector = matrix4x.MultiplyPoint3x4(boundingSphere.position);
					if (vector.z >= 0.0001f)
					{
						float num = boundingSphere.radius / vector.z;
						Vector3 vector2 = new Vector3(num, num, 0f);
						Vector3 vector3 = vector / vector.z;
						if (!flag)
						{
							bounds.center = vector3;
							bounds.extents = vector2;
							zRange = new Vector2(vector.z, vector.z);
							flag = true;
						}
						else
						{
							bounds.Encapsulate(vector3 + vector2);
							bounds.Encapsulate(vector3 - vector2);
							zRange.x = Mathf.Min(zRange.x, vector.z);
							zRange.y = Mathf.Max(zRange.y, vector.z);
						}
					}
				}
			}
			Vector3 min = bounds.min;
			Vector3 max = bounds.max;
			minAngles = new Vector2(Vector3.SignedAngle(Vector3.forward, new Vector3(0f, min.y, 1f), Vector3.left), Vector3.SignedAngle(Vector3.forward, new Vector3(min.x, 0f, 1f), Vector3.up));
			maxAngles = new Vector2(Vector3.SignedAngle(Vector3.forward, new Vector3(0f, max.y, 1f), Vector3.left), Vector3.SignedAngle(Vector3.forward, new Vector3(max.x, 0f, 1f), Vector3.up));
		}

		// Token: 0x040000D9 RID: 217
		[Tooltip("How the group's position is calculated.  Select GroupCenter for the center of the bounding box, and GroupAverage for a weighted average of the positions of the members.")]
		public CinemachineTargetGroup.PositionMode m_PositionMode;

		// Token: 0x040000DA RID: 218
		[Tooltip("How the group's rotation is calculated.  Select Manual to use the value in the group's transform, and GroupAverage for a weighted average of the orientations of the members.")]
		public CinemachineTargetGroup.RotationMode m_RotationMode;

		// Token: 0x040000DB RID: 219
		[Tooltip("When to update the group's transform based on the position of the group members")]
		public CinemachineTargetGroup.UpdateMethod m_UpdateMethod = CinemachineTargetGroup.UpdateMethod.LateUpdate;

		// Token: 0x040000DC RID: 220
		[NoSaveDuringPlay]
		[Tooltip("The target objects, together with their weights and radii, that will contribute to the group's average position, orientation, and size.")]
		public CinemachineTargetGroup.Target[] m_Targets = Array.Empty<CinemachineTargetGroup.Target>();

		// Token: 0x040000DD RID: 221
		private float m_MaxWeight;

		// Token: 0x040000DE RID: 222
		private float m_WeightSum;

		// Token: 0x040000DF RID: 223
		private Vector3 m_AveragePos;

		// Token: 0x040000E0 RID: 224
		private Bounds m_BoundingBox;

		// Token: 0x040000E1 RID: 225
		private BoundingSphere m_BoundingSphere;

		// Token: 0x040000E2 RID: 226
		private int m_LastUpdateFrame = -1;

		// Token: 0x040000E3 RID: 227
		private List<int> m_ValidMembers = new List<int>();

		// Token: 0x040000E4 RID: 228
		private List<bool> m_MemberValidity = new List<bool>();

		// Token: 0x0200008C RID: 140
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		[Serializable]
		public struct Target
		{
			// Token: 0x04000308 RID: 776
			[Tooltip("The target objects.  This object's position and orientation will contribute to the group's average position and orientation, in accordance with its weight")]
			public Transform target;

			// Token: 0x04000309 RID: 777
			[Tooltip("How much weight to give the target when averaging.  Cannot be negative")]
			public float weight;

			// Token: 0x0400030A RID: 778
			[Tooltip("The radius of the target, used for calculating the bounding box.  Cannot be negative")]
			public float radius;
		}

		// Token: 0x0200008D RID: 141
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		public enum PositionMode
		{
			// Token: 0x0400030C RID: 780
			GroupCenter,
			// Token: 0x0400030D RID: 781
			GroupAverage
		}

		// Token: 0x0200008E RID: 142
		[DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
		public enum RotationMode
		{
			// Token: 0x0400030F RID: 783
			Manual,
			// Token: 0x04000310 RID: 784
			GroupAverage
		}

		// Token: 0x0200008F RID: 143
		public enum UpdateMethod
		{
			// Token: 0x04000312 RID: 786
			Update,
			// Token: 0x04000313 RID: 787
			FixedUpdate,
			// Token: 0x04000314 RID: 788
			LateUpdate
		}
	}
}
