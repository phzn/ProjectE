using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000022 RID: 34
	[RequireComponent(typeof(Transform))]
	[NativeHeader("Modules/Physics2D/Public/Collider2D.h")]
	[RequiredByNativeCode(Optional = true)]
	public class Collider2D : Behaviour
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000322 RID: 802
		// (set) Token: 0x06000323 RID: 803
		public extern float density
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000324 RID: 804
		// (set) Token: 0x06000325 RID: 805
		public extern bool isTrigger
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000326 RID: 806
		// (set) Token: 0x06000327 RID: 807
		public extern bool usedByEffector
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000328 RID: 808
		// (set) Token: 0x06000329 RID: 809
		public extern bool usedByComposite
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600032A RID: 810
		public extern CompositeCollider2D composite
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600032B RID: 811 RVA: 0x000077E4 File Offset: 0x000059E4
		// (set) Token: 0x0600032C RID: 812 RVA: 0x000077FA File Offset: 0x000059FA
		public Vector2 offset
		{
			get
			{
				Vector2 vector;
				this.get_offset_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_offset_Injected(ref value);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600032D RID: 813
		public extern Rigidbody2D attachedRigidbody
		{
			[NativeMethod("GetAttachedRigidbody_Binding")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600032E RID: 814
		public extern int shapeCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x0600032F RID: 815
		[NativeMethod("CreateMesh_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Mesh CreateMesh(bool useBodyPosition, bool useBodyRotation);

		// Token: 0x06000330 RID: 816
		[NativeMethod("GetShapeHash_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern uint GetShapeHash();

		// Token: 0x06000331 RID: 817 RVA: 0x00007804 File Offset: 0x00005A04
		public int GetShapes(PhysicsShapeGroup2D physicsShapeGroup)
		{
			return this.GetShapes_Internal(ref physicsShapeGroup.m_GroupState, 0, this.shapeCount);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000782C File Offset: 0x00005A2C
		public int GetShapes(PhysicsShapeGroup2D physicsShapeGroup, int shapeIndex, [DefaultValue("1")] int shapeCount = 1)
		{
			int shapeCount2 = this.shapeCount;
			bool flag = shapeIndex < 0 || shapeIndex >= shapeCount2 || shapeCount < 1 || shapeIndex + shapeCount > shapeCount2;
			if (flag)
			{
				throw new ArgumentOutOfRangeException(string.Format("Cannot get shape range from {0} to {1} as Collider2D only has {2} shape(s).", shapeIndex, shapeIndex + shapeCount - 1, shapeCount2));
			}
			return this.GetShapes_Internal(ref physicsShapeGroup.m_GroupState, shapeIndex, shapeCount);
		}

		// Token: 0x06000333 RID: 819
		[NativeMethod("GetShapes_Binding")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetShapes_Internal(ref PhysicsShapeGroup2D.GroupState physicsShapeGroupState, int shapeIndex, int shapeCount);

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000334 RID: 820 RVA: 0x00007894 File Offset: 0x00005A94
		public Bounds bounds
		{
			get
			{
				Bounds bounds;
				this.get_bounds_Injected(out bounds);
				return bounds;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000335 RID: 821
		public extern ColliderErrorState2D errorState
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000336 RID: 822
		internal extern bool compositeCapable
		{
			[NativeMethod("GetCompositeCapable_Binding")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000337 RID: 823
		// (set) Token: 0x06000338 RID: 824
		public extern PhysicsMaterial2D sharedMaterial
		{
			[NativeMethod("GetMaterial")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod("SetMaterial")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000339 RID: 825
		// (set) Token: 0x0600033A RID: 826
		public extern int layerOverridePriority
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600033B RID: 827 RVA: 0x000078AC File Offset: 0x00005AAC
		// (set) Token: 0x0600033C RID: 828 RVA: 0x000078C2 File Offset: 0x00005AC2
		public LayerMask excludeLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_excludeLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_excludeLayers_Injected(ref value);
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600033D RID: 829 RVA: 0x000078CC File Offset: 0x00005ACC
		// (set) Token: 0x0600033E RID: 830 RVA: 0x000078E2 File Offset: 0x00005AE2
		public LayerMask includeLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_includeLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_includeLayers_Injected(ref value);
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600033F RID: 831 RVA: 0x000078EC File Offset: 0x00005AEC
		// (set) Token: 0x06000340 RID: 832 RVA: 0x00007902 File Offset: 0x00005B02
		public LayerMask forceSendLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_forceSendLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_forceSendLayers_Injected(ref value);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000790C File Offset: 0x00005B0C
		// (set) Token: 0x06000342 RID: 834 RVA: 0x00007922 File Offset: 0x00005B22
		public LayerMask forceReceiveLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_forceReceiveLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_forceReceiveLayers_Injected(ref value);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000343 RID: 835 RVA: 0x0000792C File Offset: 0x00005B2C
		// (set) Token: 0x06000344 RID: 836 RVA: 0x00007942 File Offset: 0x00005B42
		public LayerMask contactCaptureLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_contactCaptureLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_contactCaptureLayers_Injected(ref value);
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000345 RID: 837 RVA: 0x0000794C File Offset: 0x00005B4C
		// (set) Token: 0x06000346 RID: 838 RVA: 0x00007962 File Offset: 0x00005B62
		public LayerMask callbackLayers
		{
			get
			{
				LayerMask layerMask;
				this.get_callbackLayers_Injected(out layerMask);
				return layerMask;
			}
			set
			{
				this.set_callbackLayers_Injected(ref value);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000347 RID: 839
		public extern float friction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000348 RID: 840
		public extern float bounciness
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000349 RID: 841
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsTouching([NotNull("ArgumentNullException")] [Writable] Collider2D collider);

		// Token: 0x0600034A RID: 842 RVA: 0x0000796C File Offset: 0x00005B6C
		public bool IsTouching([Writable] Collider2D collider, ContactFilter2D contactFilter)
		{
			return this.IsTouching_OtherColliderWithFilter(collider, contactFilter);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00007986 File Offset: 0x00005B86
		[NativeMethod("IsTouching")]
		private bool IsTouching_OtherColliderWithFilter([NotNull("ArgumentNullException")] [Writable] Collider2D collider, ContactFilter2D contactFilter)
		{
			return this.IsTouching_OtherColliderWithFilter_Injected(collider, ref contactFilter);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00007994 File Offset: 0x00005B94
		public bool IsTouching(ContactFilter2D contactFilter)
		{
			return this.IsTouching_AnyColliderWithFilter(contactFilter);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000079AD File Offset: 0x00005BAD
		[NativeMethod("IsTouching")]
		private bool IsTouching_AnyColliderWithFilter(ContactFilter2D contactFilter)
		{
			return this.IsTouching_AnyColliderWithFilter_Injected(ref contactFilter);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x000079B8 File Offset: 0x00005BB8
		[ExcludeFromDocs]
		public bool IsTouchingLayers()
		{
			return this.IsTouchingLayers(-1);
		}

		// Token: 0x0600034F RID: 847
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool IsTouchingLayers([DefaultValue("Physics2D.AllLayers")] int layerMask);

		// Token: 0x06000350 RID: 848 RVA: 0x000079D1 File Offset: 0x00005BD1
		public bool OverlapPoint(Vector2 point)
		{
			return this.OverlapPoint_Injected(ref point);
		}

		// Token: 0x06000351 RID: 849 RVA: 0x000079DC File Offset: 0x00005BDC
		public ColliderDistance2D Distance([Writable] Collider2D collider)
		{
			return Physics2D.Distance(this, collider);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x000079F8 File Offset: 0x00005BF8
		public int OverlapCollider(ContactFilter2D contactFilter, Collider2D[] results)
		{
			return PhysicsScene2D.OverlapCollider(this, contactFilter, results);
		}

		// Token: 0x06000353 RID: 851 RVA: 0x00007A14 File Offset: 0x00005C14
		public int OverlapCollider(ContactFilter2D contactFilter, List<Collider2D> results)
		{
			return PhysicsScene2D.OverlapCollider(this, contactFilter, results);
		}

		// Token: 0x06000354 RID: 852 RVA: 0x00007A30 File Offset: 0x00005C30
		public int GetContacts(ContactPoint2D[] contacts)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), contacts);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00007A58 File Offset: 0x00005C58
		public int GetContacts(List<ContactPoint2D> contacts)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), contacts);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00007A80 File Offset: 0x00005C80
		public int GetContacts(ContactFilter2D contactFilter, ContactPoint2D[] contacts)
		{
			return Physics2D.GetContacts(this, contactFilter, contacts);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00007A9C File Offset: 0x00005C9C
		public int GetContacts(ContactFilter2D contactFilter, List<ContactPoint2D> contacts)
		{
			return Physics2D.GetContacts(this, contactFilter, contacts);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00007AB8 File Offset: 0x00005CB8
		public int GetContacts(Collider2D[] colliders)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), colliders);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00007AE0 File Offset: 0x00005CE0
		public int GetContacts(List<Collider2D> colliders)
		{
			return Physics2D.GetContacts(this, default(ContactFilter2D).NoFilter(), colliders);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00007B08 File Offset: 0x00005D08
		public int GetContacts(ContactFilter2D contactFilter, Collider2D[] colliders)
		{
			return Physics2D.GetContacts(this, contactFilter, colliders);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00007B24 File Offset: 0x00005D24
		public int GetContacts(ContactFilter2D contactFilter, List<Collider2D> colliders)
		{
			return Physics2D.GetContacts(this, contactFilter, colliders);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00007B40 File Offset: 0x00005D40
		[ExcludeFromDocs]
		public int Cast(Vector2 direction, RaycastHit2D[] results)
		{
			ContactFilter2D contactFilter2D = default(ContactFilter2D);
			contactFilter2D.useTriggers = Physics2D.queriesHitTriggers;
			contactFilter2D.SetLayerMask(Physics2D.GetLayerCollisionMask(base.gameObject.layer));
			return this.CastArray_Internal(direction, float.PositiveInfinity, contactFilter2D, true, results);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00007B94 File Offset: 0x00005D94
		[ExcludeFromDocs]
		public int Cast(Vector2 direction, RaycastHit2D[] results, float distance)
		{
			ContactFilter2D contactFilter2D = default(ContactFilter2D);
			contactFilter2D.useTriggers = Physics2D.queriesHitTriggers;
			contactFilter2D.SetLayerMask(Physics2D.GetLayerCollisionMask(base.gameObject.layer));
			return this.CastArray_Internal(direction, distance, contactFilter2D, true, results);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00007BE4 File Offset: 0x00005DE4
		public int Cast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("true")] bool ignoreSiblingColliders)
		{
			ContactFilter2D contactFilter2D = default(ContactFilter2D);
			contactFilter2D.useTriggers = Physics2D.queriesHitTriggers;
			contactFilter2D.SetLayerMask(Physics2D.GetLayerCollisionMask(base.gameObject.layer));
			return this.CastArray_Internal(direction, distance, contactFilter2D, ignoreSiblingColliders, results);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00007C34 File Offset: 0x00005E34
		[ExcludeFromDocs]
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return this.CastArray_Internal(direction, float.PositiveInfinity, contactFilter, true, results);
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00007C58 File Offset: 0x00005E58
		[ExcludeFromDocs]
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, float distance)
		{
			return this.CastArray_Internal(direction, distance, contactFilter, true, results);
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00007C78 File Offset: 0x00005E78
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("true")] bool ignoreSiblingColliders)
		{
			return this.CastArray_Internal(direction, distance, contactFilter, ignoreSiblingColliders, results);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00007C97 File Offset: 0x00005E97
		[NativeMethod("CastArray_Binding")]
		private int CastArray_Internal(Vector2 direction, float distance, ContactFilter2D contactFilter, bool ignoreSiblingColliders, [NotNull("ArgumentNullException")] [Unmarshalled] RaycastHit2D[] results)
		{
			return this.CastArray_Internal_Injected(ref direction, distance, ref contactFilter, ignoreSiblingColliders, results);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00007CA8 File Offset: 0x00005EA8
		public int Cast(Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity, [DefaultValue("true")] bool ignoreSiblingColliders = true)
		{
			return this.CastList_Internal(direction, distance, contactFilter, ignoreSiblingColliders, results);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00007CC7 File Offset: 0x00005EC7
		[NativeMethod("CastList_Binding")]
		private int CastList_Internal(Vector2 direction, float distance, ContactFilter2D contactFilter, bool ignoreSiblingColliders, [NotNull("ArgumentNullException")] List<RaycastHit2D> results)
		{
			return this.CastList_Internal_Injected(ref direction, distance, ref contactFilter, ignoreSiblingColliders, results);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00007CD8 File Offset: 0x00005ED8
		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-1, float.NegativeInfinity, float.PositiveInfinity);
			return this.RaycastArray_Internal(direction, float.PositiveInfinity, contactFilter2D, results);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00007D0C File Offset: 0x00005F0C
		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(-1, float.NegativeInfinity, float.PositiveInfinity);
			return this.RaycastArray_Internal(direction, distance, contactFilter2D, results);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00007D3C File Offset: 0x00005F3C
		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance, int layerMask)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, float.NegativeInfinity, float.PositiveInfinity);
			return this.RaycastArray_Internal(direction, distance, contactFilter2D, results);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00007D6C File Offset: 0x00005F6C
		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, RaycastHit2D[] results, float distance, int layerMask, float minDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, float.PositiveInfinity);
			return this.RaycastArray_Internal(direction, distance, contactFilter2D, results);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00007D98 File Offset: 0x00005F98
		public int Raycast(Vector2 direction, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance, [DefaultValue("Physics2D.AllLayers")] int layerMask, [DefaultValue("-Mathf.Infinity")] float minDepth, [DefaultValue("Mathf.Infinity")] float maxDepth)
		{
			ContactFilter2D contactFilter2D = ContactFilter2D.CreateLegacyFilter(layerMask, minDepth, maxDepth);
			return this.RaycastArray_Internal(direction, distance, contactFilter2D, results);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00007DC0 File Offset: 0x00005FC0
		[ExcludeFromDocs]
		public int Raycast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results)
		{
			return this.RaycastArray_Internal(direction, float.PositiveInfinity, contactFilter, results);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00007DE0 File Offset: 0x00005FE0
		public int Raycast(Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, [DefaultValue("Mathf.Infinity")] float distance)
		{
			return this.RaycastArray_Internal(direction, distance, contactFilter, results);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00007DFD File Offset: 0x00005FFD
		[NativeMethod("RaycastArray_Binding")]
		private int RaycastArray_Internal(Vector2 direction, float distance, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] [Unmarshalled] RaycastHit2D[] results)
		{
			return this.RaycastArray_Internal_Injected(ref direction, distance, ref contactFilter, results);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00007E0C File Offset: 0x0000600C
		public int Raycast(Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, [DefaultValue("Mathf.Infinity")] float distance = float.PositiveInfinity)
		{
			return this.RaycastList_Internal(direction, distance, contactFilter, results);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00007E29 File Offset: 0x00006029
		[NativeMethod("RaycastList_Binding")]
		private int RaycastList_Internal(Vector2 direction, float distance, ContactFilter2D contactFilter, [NotNull("ArgumentNullException")] List<RaycastHit2D> results)
		{
			return this.RaycastList_Internal_Injected(ref direction, distance, ref contactFilter, results);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00007E38 File Offset: 0x00006038
		public Vector2 ClosestPoint(Vector2 position)
		{
			return Physics2D.ClosestPoint(position, this);
		}

		// Token: 0x06000371 RID: 881
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_offset_Injected(out Vector2 ret);

		// Token: 0x06000372 RID: 882
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_offset_Injected(ref Vector2 value);

		// Token: 0x06000373 RID: 883
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bounds_Injected(out Bounds ret);

		// Token: 0x06000374 RID: 884
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_excludeLayers_Injected(out LayerMask ret);

		// Token: 0x06000375 RID: 885
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_excludeLayers_Injected(ref LayerMask value);

		// Token: 0x06000376 RID: 886
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_includeLayers_Injected(out LayerMask ret);

		// Token: 0x06000377 RID: 887
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_includeLayers_Injected(ref LayerMask value);

		// Token: 0x06000378 RID: 888
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_forceSendLayers_Injected(out LayerMask ret);

		// Token: 0x06000379 RID: 889
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_forceSendLayers_Injected(ref LayerMask value);

		// Token: 0x0600037A RID: 890
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_forceReceiveLayers_Injected(out LayerMask ret);

		// Token: 0x0600037B RID: 891
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_forceReceiveLayers_Injected(ref LayerMask value);

		// Token: 0x0600037C RID: 892
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_contactCaptureLayers_Injected(out LayerMask ret);

		// Token: 0x0600037D RID: 893
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_contactCaptureLayers_Injected(ref LayerMask value);

		// Token: 0x0600037E RID: 894
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_callbackLayers_Injected(out LayerMask ret);

		// Token: 0x0600037F RID: 895
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_callbackLayers_Injected(ref LayerMask value);

		// Token: 0x06000380 RID: 896
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsTouching_OtherColliderWithFilter_Injected([Writable] Collider2D collider, ref ContactFilter2D contactFilter);

		// Token: 0x06000381 RID: 897
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool IsTouching_AnyColliderWithFilter_Injected(ref ContactFilter2D contactFilter);

		// Token: 0x06000382 RID: 898
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool OverlapPoint_Injected(ref Vector2 point);

		// Token: 0x06000383 RID: 899
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int CastArray_Internal_Injected(ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, bool ignoreSiblingColliders, RaycastHit2D[] results);

		// Token: 0x06000384 RID: 900
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int CastList_Internal_Injected(ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, bool ignoreSiblingColliders, List<RaycastHit2D> results);

		// Token: 0x06000385 RID: 901
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int RaycastArray_Internal_Injected(ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, RaycastHit2D[] results);

		// Token: 0x06000386 RID: 902
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int RaycastList_Internal_Injected(ref Vector2 direction, float distance, ref ContactFilter2D contactFilter, List<RaycastHit2D> results);
	}
}
