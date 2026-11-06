using System;

namespace Unity.Entities.Serialization
{
	// Token: 0x020003A1 RID: 929
	[Serializable]
	public struct UntypedWeakReferenceId : IEquatable<UntypedWeakReferenceId>
	{
		// Token: 0x06001899 RID: 6297 RVA: 0x0005EA6A File Offset: 0x0005CC6A
		public UntypedWeakReferenceId(RuntimeGlobalObjectId globalObjId, WeakReferenceGenerationType genType)
		{
			this.GlobalId = globalObjId;
			this.GenerationType = genType;
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x0005EA7C File Offset: 0x0005CC7C
		public UntypedWeakReferenceId(Hash128 guid, long localFileIdentifier, int idType, WeakReferenceGenerationType genType)
		{
			this.GlobalId = new RuntimeGlobalObjectId
			{
				AssetGUID = guid,
				SceneObjectIdentifier0 = localFileIdentifier,
				IdentifierType = idType
			};
			this.GenerationType = genType;
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0005EAB8 File Offset: 0x0005CCB8
		public bool Equals(UntypedWeakReferenceId other)
		{
			return this.GenerationType == other.GenerationType && this.GlobalId.Equals(other.GlobalId);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0005EADB File Offset: 0x0005CCDB
		public override string ToString()
		{
			return this.GlobalId.ToString();
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0005EAF0 File Offset: 0x0005CCF0
		public override bool Equals(object obj)
		{
			if (obj is UntypedWeakReferenceId)
			{
				UntypedWeakReferenceId untypedWeakReferenceId = (UntypedWeakReferenceId)obj;
				return this.Equals(untypedWeakReferenceId);
			}
			return false;
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x0600189E RID: 6302 RVA: 0x0005EB15 File Offset: 0x0005CD15
		public bool IsValid
		{
			get
			{
				return this.GlobalId.IsValid;
			}
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x0005EB22 File Offset: 0x0005CD22
		public override int GetHashCode()
		{
			return this.GlobalId.GetHashCode();
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x0005EB35 File Offset: 0x0005CD35
		public static bool operator ==(UntypedWeakReferenceId left, UntypedWeakReferenceId right)
		{
			return left.Equals(right);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x0005EB3F File Offset: 0x0005CD3F
		public static bool operator !=(UntypedWeakReferenceId left, UntypedWeakReferenceId right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000979 RID: 2425
		public RuntimeGlobalObjectId GlobalId;

		// Token: 0x0400097A RID: 2426
		public WeakReferenceGenerationType GenerationType;
	}
}
