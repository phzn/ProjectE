using System;
using Unity.Collections;
using UnityEngine;

namespace Unity.Entities
{
	// Token: 0x0200028B RID: 651
	[Serializable]
	public struct UnityObjectRef<T> : IEquatable<UnityObjectRef<T>> where T : Object
	{
		// Token: 0x0600114F RID: 4431 RVA: 0x00049345 File Offset: 0x00047545
		public static implicit operator UnityObjectRef<T>(T instance)
		{
			return UnityObjectRef<T>.FromInstanceID((instance == null) ? 0 : instance.GetInstanceID());
		}

		// Token: 0x06001150 RID: 4432 RVA: 0x00049368 File Offset: 0x00047568
		internal static UnityObjectRef<T> FromInstanceID(int instanceId)
		{
			return new UnityObjectRef<T>
			{
				Id = new UntypedUnityObjectRef
				{
					instanceId = instanceId
				}
			};
		}

		// Token: 0x06001151 RID: 4433 RVA: 0x00049398 File Offset: 0x00047598
		public static implicit operator T(UnityObjectRef<T> unityObjectRef)
		{
			if (unityObjectRef.Id.instanceId == 0)
			{
				return default(T);
			}
			return (T)((object)Resources.InstanceIDToObject(unityObjectRef.Id.instanceId));
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x000493D1 File Offset: 0x000475D1
		// (set) Token: 0x06001153 RID: 4435 RVA: 0x000493DE File Offset: 0x000475DE
		public T Value
		{
			[ExcludeFromBurstCompatTesting("Returns managed object")]
			get
			{
				return this;
			}
			[ExcludeFromBurstCompatTesting("Sets managed object")]
			set
			{
				this = value;
			}
		}

		// Token: 0x06001154 RID: 4436 RVA: 0x000493EC File Offset: 0x000475EC
		public bool Equals(UnityObjectRef<T> other)
		{
			return this.Id.instanceId == other.Id.instanceId;
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00049408 File Offset: 0x00047608
		public override bool Equals(object obj)
		{
			if (obj is UnityObjectRef<T>)
			{
				UnityObjectRef<T> unityObjectRef = (UnityObjectRef<T>)obj;
				return this.Equals(unityObjectRef);
			}
			return false;
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x0004942D File Offset: 0x0004762D
		public static implicit operator bool(UnityObjectRef<T> obj)
		{
			return obj.IsValid();
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00049436 File Offset: 0x00047636
		public override int GetHashCode()
		{
			return this.Id.instanceId.GetHashCode();
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00049448 File Offset: 0x00047648
		public bool IsValid()
		{
			return Resources.InstanceIDIsValid(this.Id.instanceId);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0004945A File Offset: 0x0004765A
		public static bool operator ==(UnityObjectRef<T> left, UnityObjectRef<T> right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00049464 File Offset: 0x00047664
		public static bool operator !=(UnityObjectRef<T> left, UnityObjectRef<T> right)
		{
			return !left.Equals(right);
		}

		// Token: 0x0400069E RID: 1694
		[SerializeField]
		internal UntypedUnityObjectRef Id;
	}
}
