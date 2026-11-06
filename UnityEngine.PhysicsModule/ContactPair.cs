using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200003C RID: 60
	[UsedByNativeCode]
	public readonly struct ContactPair
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x000069F5 File Offset: 0x00004BF5
		public int ColliderInstanceID
		{
			get
			{
				return this.m_ColliderID;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x000069FD File Offset: 0x00004BFD
		public int OtherColliderInstanceID
		{
			get
			{
				return this.m_OtherColliderID;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x00006A05 File Offset: 0x00004C05
		public Collider Collider
		{
			get
			{
				return (this.m_ColliderID == 0) ? null : Physics.GetColliderByInstanceID(this.m_ColliderID);
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00006A1D File Offset: 0x00004C1D
		public Collider OtherCollider
		{
			get
			{
				return (this.m_OtherColliderID == 0) ? null : Physics.GetColliderByInstanceID(this.m_OtherColliderID);
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x00006A35 File Offset: 0x00004C35
		public int ContactCount
		{
			get
			{
				return (int)this.m_NbPoints;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00006A3D File Offset: 0x00004C3D
		public Vector3 ImpulseSum
		{
			get
			{
				return this.m_ImpulseSum;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00006A45 File Offset: 0x00004C45
		public bool IsCollisionEnter
		{
			get
			{
				return (this.m_Events & CollisionPairEventFlags.NotifyTouchFound) > (CollisionPairEventFlags)0;
			}
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00006A52 File Offset: 0x00004C52
		public bool IsCollisionExit
		{
			get
			{
				return (this.m_Events & CollisionPairEventFlags.NotifyTouchLost) > (CollisionPairEventFlags)0;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00006A60 File Offset: 0x00004C60
		public bool IsCollisionStay
		{
			get
			{
				return (this.m_Events & CollisionPairEventFlags.NotifyTouchPersists) > (CollisionPairEventFlags)0;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00006A6D File Offset: 0x00004C6D
		internal bool HasRemovedCollider
		{
			get
			{
				return (this.m_Flags & CollisionPairFlags.RemovedShape) != (CollisionPairFlags)0 || (this.m_Flags & CollisionPairFlags.RemovedOtherShape) > (CollisionPairFlags)0;
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00006A87 File Offset: 0x00004C87
		internal int ExtractContacts(List<ContactPoint> managedContainer, bool flipped)
		{
			return ContactPair.ExtractContacts_Injected(ref this, managedContainer, flipped);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00006A91 File Offset: 0x00004C91
		internal int ExtractContactsArray([Unmarshalled] ContactPoint[] managedContainer, bool flipped)
		{
			return ContactPair.ExtractContactsArray_Injected(ref this, managedContainer, flipped);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00006A9C File Offset: 0x00004C9C
		public unsafe void CopyToNativeArray(NativeArray<ContactPairPoint> buffer)
		{
			int num = Mathf.Min(buffer.Length, this.ContactCount);
			for (int i = 0; i < num; i++)
			{
				buffer[i] = *this.GetContactPoint(i);
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x00006AE4 File Offset: 0x00004CE4
		public readonly ref ContactPairPoint GetContactPoint(int index)
		{
			return this.GetContactPoint_Internal(index);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00006B00 File Offset: 0x00004D00
		public unsafe uint GetContactPointFaceIndex(int contactIndex)
		{
			uint internalFaceIndex = this.GetContactPoint_Internal(contactIndex)->m_InternalFaceIndex0;
			uint internalFaceIndex2 = this.GetContactPoint_Internal(contactIndex)->m_InternalFaceIndex1;
			bool flag = internalFaceIndex != uint.MaxValue;
			uint num;
			if (flag)
			{
				num = Physics.TranslateTriangleIndexFromID(this.m_ColliderID, internalFaceIndex);
			}
			else
			{
				bool flag2 = internalFaceIndex2 != uint.MaxValue;
				if (flag2)
				{
					num = Physics.TranslateTriangleIndexFromID(this.m_OtherColliderID, internalFaceIndex2);
				}
				else
				{
					num = uint.MaxValue;
				}
			}
			return num;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00006B64 File Offset: 0x00004D64
		internal unsafe ContactPairPoint* GetContactPoint_Internal(int index)
		{
			bool flag = (long)index >= (long)((ulong)this.m_NbPoints);
			if (flag)
			{
				throw new IndexOutOfRangeException("Invalid ContactPairPoint index. Index should be greater than 0 and less than ContactPair.ContactCount");
			}
			return this.m_StartPtr.ToInt64() / (long)sizeof(ContactPairPoint) + index * sizeof(ContactPairPoint);
		}

		// Token: 0x060004BD RID: 1213
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int ExtractContacts_Injected(ref ContactPair _unity_self, List<ContactPoint> managedContainer, bool flipped);

		// Token: 0x060004BE RID: 1214
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int ExtractContactsArray_Injected(ref ContactPair _unity_self, ContactPoint[] managedContainer, bool flipped);

		// Token: 0x040000D7 RID: 215
		private const uint c_InvalidFaceIndex = 4294967295U;

		// Token: 0x040000D8 RID: 216
		internal readonly int m_ColliderID;

		// Token: 0x040000D9 RID: 217
		internal readonly int m_OtherColliderID;

		// Token: 0x040000DA RID: 218
		internal readonly IntPtr m_StartPtr;

		// Token: 0x040000DB RID: 219
		internal readonly uint m_NbPoints;

		// Token: 0x040000DC RID: 220
		internal readonly CollisionPairFlags m_Flags;

		// Token: 0x040000DD RID: 221
		internal readonly CollisionPairEventFlags m_Events;

		// Token: 0x040000DE RID: 222
		internal readonly Vector3 m_ImpulseSum;
	}
}
