using System;

namespace UnityEngine
{
	// Token: 0x02000021 RID: 33
	public struct ModifiableContactPair
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000234 RID: 564 RVA: 0x00004CAA File Offset: 0x00002EAA
		public int colliderInstanceID
		{
			get
			{
				return Physics.ResolveShapeToInstanceID(this.shape);
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00004CB7 File Offset: 0x00002EB7
		public int otherColliderInstanceID
		{
			get
			{
				return Physics.ResolveShapeToInstanceID(this.otherShape);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00004CC4 File Offset: 0x00002EC4
		public int bodyInstanceID
		{
			get
			{
				return Physics.ResolveActorToInstanceID(this.actor);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000237 RID: 567 RVA: 0x00004CD1 File Offset: 0x00002ED1
		public int otherBodyInstanceID
		{
			get
			{
				return Physics.ResolveActorToInstanceID(this.otherActor);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000238 RID: 568 RVA: 0x00004CDE File Offset: 0x00002EDE
		public Vector3 bodyVelocity
		{
			get
			{
				return Physics.GetActorLinearVelocity(this.actor);
			}
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00004CEB File Offset: 0x00002EEB
		public Vector3 bodyAngularVelocity
		{
			get
			{
				return Physics.GetActorAngularVelocity(this.actor);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00004CF8 File Offset: 0x00002EF8
		public Vector3 otherBodyVelocity
		{
			get
			{
				return Physics.GetActorLinearVelocity(this.otherActor);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00004D05 File Offset: 0x00002F05
		public Vector3 otherBodyAngularVelocity
		{
			get
			{
				return Physics.GetActorAngularVelocity(this.otherActor);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00004D12 File Offset: 0x00002F12
		public int contactCount
		{
			get
			{
				return this.numContacts;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00004D1C File Offset: 0x00002F1C
		// (set) Token: 0x0600023E RID: 574 RVA: 0x00004D3C File Offset: 0x00002F3C
		public unsafe ModifiableMassProperties massProperties
		{
			get
			{
				return this.GetContactPatch()->massProperties;
			}
			set
			{
				ModifiableContactPatch* contactPatch = this.GetContactPatch();
				contactPatch->massProperties = value;
				ModifiableContactPatch* ptr = contactPatch;
				ptr->internalFlags = ptr->internalFlags | 8;
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00004D64 File Offset: 0x00002F64
		public unsafe Vector3 GetPoint(int i)
		{
			return this.GetContact(i)->contact;
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00004D82 File Offset: 0x00002F82
		public unsafe void SetPoint(int i, Vector3 v)
		{
			this.GetContact(i)->contact = v;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00004D94 File Offset: 0x00002F94
		public unsafe Vector3 GetNormal(int i)
		{
			return this.GetContact(i)->normal;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00004DB2 File Offset: 0x00002FB2
		public unsafe void SetNormal(int i, Vector3 normal)
		{
			this.GetContact(i)->normal = normal;
			ModifiableContactPatch* contactPatch = this.GetContactPatch();
			contactPatch->internalFlags = contactPatch->internalFlags | 64;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00004DD4 File Offset: 0x00002FD4
		public unsafe float GetSeparation(int i)
		{
			return this.GetContact(i)->separation;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00004DF2 File Offset: 0x00002FF2
		public unsafe void SetSeparation(int i, float separation)
		{
			this.GetContact(i)->separation = separation;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00004E04 File Offset: 0x00003004
		public unsafe Vector3 GetTargetVelocity(int i)
		{
			return this.GetContact(i)->targetVelocity;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00004E22 File Offset: 0x00003022
		public unsafe void SetTargetVelocity(int i, Vector3 velocity)
		{
			this.GetContact(i)->targetVelocity = velocity;
			ModifiableContactPatch* contactPatch = this.GetContactPatch();
			contactPatch->internalFlags = contactPatch->internalFlags | 16;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00004E44 File Offset: 0x00003044
		public unsafe float GetBounciness(int i)
		{
			return this.GetContact(i)->restitution;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00004E62 File Offset: 0x00003062
		public unsafe void SetBounciness(int i, float bounciness)
		{
			this.GetContact(i)->restitution = bounciness;
			ModifiableContactPatch* contactPatch = this.GetContactPatch();
			contactPatch->internalFlags = contactPatch->internalFlags | 64;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00004E84 File Offset: 0x00003084
		public unsafe float GetStaticFriction(int i)
		{
			return this.GetContact(i)->staticFriction;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00004EA2 File Offset: 0x000030A2
		public unsafe void SetStaticFriction(int i, float staticFriction)
		{
			this.GetContact(i)->staticFriction = staticFriction;
			ModifiableContactPatch* contactPatch = this.GetContactPatch();
			contactPatch->internalFlags = contactPatch->internalFlags | 64;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00004EC4 File Offset: 0x000030C4
		public unsafe float GetDynamicFriction(int i)
		{
			return this.GetContact(i)->dynamicFriction;
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00004EE2 File Offset: 0x000030E2
		public unsafe void SetDynamicFriction(int i, float dynamicFriction)
		{
			this.GetContact(i)->dynamicFriction = dynamicFriction;
			ModifiableContactPatch* contactPatch = this.GetContactPatch();
			contactPatch->internalFlags = contactPatch->internalFlags | 64;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00004F04 File Offset: 0x00003104
		public unsafe float GetMaxImpulse(int i)
		{
			return this.GetContact(i)->maxImpulse;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00004F22 File Offset: 0x00003122
		public unsafe void SetMaxImpulse(int i, float value)
		{
			this.GetContact(i)->maxImpulse = value;
			ModifiableContactPatch* contactPatch = this.GetContactPatch();
			contactPatch->internalFlags = contactPatch->internalFlags | 32;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00004F44 File Offset: 0x00003144
		public void IgnoreContact(int i)
		{
			this.SetMaxImpulse(i, 0f);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00004F54 File Offset: 0x00003154
		public unsafe uint GetFaceIndex(int i)
		{
			bool flag = (this.GetContactPatch()->internalFlags & 1) > 0;
			uint num2;
			if (flag)
			{
				IntPtr intPtr = new IntPtr(this.contacts.ToInt64() + (long)(this.numContacts * sizeof(ModifiableContact)) + (long)((this.numContacts + i) * 4));
				uint num = *(uint*)(void*)intPtr;
				num2 = Physics.TranslateTriangleIndex(this.otherShape, num);
			}
			else
			{
				num2 = uint.MaxValue;
			}
			return num2;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00004FC0 File Offset: 0x000031C0
		private unsafe ModifiableContact* GetContact(int index)
		{
			IntPtr intPtr = new IntPtr(this.contacts.ToInt64() + (long)(index * sizeof(ModifiableContact)));
			return (ModifiableContact*)(void*)intPtr;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00004FF4 File Offset: 0x000031F4
		private unsafe ModifiableContactPatch* GetContactPatch()
		{
			IntPtr intPtr = new IntPtr(this.contacts.ToInt64() - (long)(this.numContacts * sizeof(ModifiableContactPatch)));
			return (ModifiableContactPatch*)(void*)intPtr;
		}

		// Token: 0x0400008E RID: 142
		private IntPtr actor;

		// Token: 0x0400008F RID: 143
		private IntPtr otherActor;

		// Token: 0x04000090 RID: 144
		private IntPtr shape;

		// Token: 0x04000091 RID: 145
		private IntPtr otherShape;

		// Token: 0x04000092 RID: 146
		public Quaternion rotation;

		// Token: 0x04000093 RID: 147
		public Vector3 position;

		// Token: 0x04000094 RID: 148
		public Quaternion otherRotation;

		// Token: 0x04000095 RID: 149
		public Vector3 otherPosition;

		// Token: 0x04000096 RID: 150
		private int numContacts;

		// Token: 0x04000097 RID: 151
		private IntPtr contacts;
	}
}
