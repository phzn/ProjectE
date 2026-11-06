using System;
using System.Diagnostics;
using System.Linq;
using Unity.Collections;

namespace Unity.Entities
{
	// Token: 0x02000210 RID: 528
	public class EntityQueryDesc : IEquatable<EntityQueryDesc>
	{
		// Token: 0x06000C61 RID: 3169 RVA: 0x00038A20 File Offset: 0x00036C20
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		private void AddComponentTypeIndicesToArray(ComponentType[] componentTypes, ref NativeArray<TypeIndex> allComponentTypeIds, ref int currentIndexInComponentTypeIdArray)
		{
			foreach (ComponentType componentType in componentTypes)
			{
				int num = currentIndexInComponentTypeIdArray;
				currentIndexInComponentTypeIdArray = num + 1;
				allComponentTypeIds[num] = componentType.TypeIndex;
				if (componentType.AccessModeType == ComponentType.AccessMode.Exclude)
				{
					throw new ArgumentException("EntityQueryDesc cannot contain Exclude Component types");
				}
			}
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x00038A6C File Offset: 0x00036C6C
		[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
		[Conditional("UNITY_DOTS_DEBUG")]
		public void Validate()
		{
			int num = this.None.Length + this.All.Length + this.Any.Length + this.Disabled.Length + this.Absent.Length + this.Present.Length;
			NativeArray<TypeIndex> nativeArray = new NativeArray<TypeIndex>(num, Allocator.Temp, NativeArrayOptions.ClearMemory);
			if (num > 1)
			{
				nativeArray.Sort<TypeIndex>();
				TypeIndex typeIndex = nativeArray[0];
				for (int i = 1; i < nativeArray.Length; i++)
				{
					TypeIndex typeIndex2 = nativeArray[i];
					if (typeIndex2 == typeIndex)
					{
						Type type = TypeManager.GetType(typeIndex2);
						throw new EntityQueryDescValidationException("The component type " + type.Name + " appears multiple times in an EntityQueryDesc. Duplicate component types are not allowed within an EntityQueryDesc.");
					}
					typeIndex = typeIndex2;
				}
			}
			nativeArray.Dispose();
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x00038B21 File Offset: 0x00036D21
		public override bool Equals(object obj)
		{
			return this.Equals(obj as EntityQueryDesc);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00038B30 File Offset: 0x00036D30
		public bool Equals(EntityQueryDesc other)
		{
			return this == other || (other != null && this.Options.Equals(other.Options) && EntityQueryDesc.ArraysEquivalent(this.All, other.All) && EntityQueryDesc.ArraysEquivalent(this.Any, other.Any) && EntityQueryDesc.ArraysEquivalent(this.None, other.None) && EntityQueryDesc.ArraysEquivalent(this.Disabled, other.Disabled) && EntityQueryDesc.ArraysEquivalent(this.Absent, other.Absent) && EntityQueryDesc.ArraysEquivalent(this.Present, other.Present));
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00038BE7 File Offset: 0x00036DE7
		public static bool operator ==(EntityQueryDesc lhs, EntityQueryDesc rhs)
		{
			if (lhs == null)
			{
				return rhs == null;
			}
			return lhs.Equals(rhs);
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x00038BF8 File Offset: 0x00036DF8
		public static bool operator !=(EntityQueryDesc lhs, EntityQueryDesc rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00038C04 File Offset: 0x00036E04
		public override int GetHashCode()
		{
			return (((((((((((((17 * 397) ^ this.Options.GetHashCode()) * 397) ^ (this.All ?? Array.Empty<ComponentType>()).GetHashCode()) * 397) ^ (this.Any ?? Array.Empty<ComponentType>()).GetHashCode()) * 397) ^ (this.None ?? Array.Empty<ComponentType>()).GetHashCode()) * 397) ^ (this.Disabled ?? Array.Empty<ComponentType>()).GetHashCode()) * 397) ^ (this.Absent ?? Array.Empty<ComponentType>()).GetHashCode()) * 397) ^ (this.Present ?? Array.Empty<ComponentType>()).GetHashCode();
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00038CD0 File Offset: 0x00036ED0
		private static bool ArraysEquivalent(ComponentType[] a1, ComponentType[] a2)
		{
			if (a1 == a2)
			{
				return true;
			}
			if (a1 == null || a2 == null)
			{
				return false;
			}
			if (a1.Length != a2.Length)
			{
				return false;
			}
			return a1.OrderBy<ComponentType, ComponentType>((ComponentType x) => x).SequenceEqual<ComponentType>(a2.OrderBy<ComponentType, ComponentType>((ComponentType x) => x));
		}

		// Token: 0x04000565 RID: 1381
		public ComponentType[] Any = Array.Empty<ComponentType>();

		// Token: 0x04000566 RID: 1382
		public ComponentType[] None = Array.Empty<ComponentType>();

		// Token: 0x04000567 RID: 1383
		public ComponentType[] All = Array.Empty<ComponentType>();

		// Token: 0x04000568 RID: 1384
		public ComponentType[] Disabled = Array.Empty<ComponentType>();

		// Token: 0x04000569 RID: 1385
		public ComponentType[] Absent = Array.Empty<ComponentType>();

		// Token: 0x0400056A RID: 1386
		public ComponentType[] Present = Array.Empty<ComponentType>();

		// Token: 0x0400056B RID: 1387
		public EntityQueryOptions Options;
	}
}
