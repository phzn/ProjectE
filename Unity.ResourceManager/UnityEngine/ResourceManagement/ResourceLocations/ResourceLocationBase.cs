using System;
using System.Collections.Generic;

namespace UnityEngine.ResourceManagement.ResourceLocations
{
	// Token: 0x02000048 RID: 72
	public class ResourceLocationBase : IResourceLocation
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00006F32 File Offset: 0x00005132
		public string InternalId
		{
			get
			{
				return this.m_Id;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00006F3A File Offset: 0x0000513A
		public string ProviderId
		{
			get
			{
				return this.m_ProviderId;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00006F42 File Offset: 0x00005142
		public IList<IResourceLocation> Dependencies
		{
			get
			{
				return this.m_Dependencies;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00006F4A File Offset: 0x0000514A
		public bool HasDependencies
		{
			get
			{
				return this.m_Dependencies != null && this.m_Dependencies.Count > 0;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00006F64 File Offset: 0x00005164
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00006F6C File Offset: 0x0000516C
		public object Data
		{
			get
			{
				return this.m_Data;
			}
			set
			{
				this.m_Data = value;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00006F75 File Offset: 0x00005175
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00006F7D File Offset: 0x0000517D
		public string PrimaryKey
		{
			get
			{
				return this.m_PrimaryKey;
			}
			set
			{
				this.m_PrimaryKey = value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00006F86 File Offset: 0x00005186
		public int DependencyHashCode
		{
			get
			{
				return this.m_DependencyHashCode;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00006F8E File Offset: 0x0000518E
		public Type ResourceType
		{
			get
			{
				return this.m_Type;
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00006F96 File Offset: 0x00005196
		public int Hash(Type t)
		{
			return (this.m_HashCode * 31 + t.GetHashCode()) * 31 + this.DependencyHashCode;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00006FB2 File Offset: 0x000051B2
		public override string ToString()
		{
			return this.m_Id;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00006FBC File Offset: 0x000051BC
		public ResourceLocationBase(string name, string id, string providerId, Type t, params IResourceLocation[] dependencies)
		{
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentNullException("id");
			}
			if (string.IsNullOrEmpty(providerId))
			{
				throw new ArgumentNullException("providerId");
			}
			this.m_PrimaryKey = name;
			this.m_HashCode = (name.GetHashCode() * 31 + id.GetHashCode()) * 31 + providerId.GetHashCode();
			this.m_Name = name;
			this.m_Id = id;
			this.m_ProviderId = providerId;
			this.m_Dependencies = new List<IResourceLocation>(dependencies);
			this.m_Type = ((t == null) ? typeof(object) : t);
			this.ComputeDependencyHash();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00007064 File Offset: 0x00005264
		public void ComputeDependencyHash()
		{
			this.m_DependencyHashCode = ((this.m_Dependencies.Count > 0) ? 17 : 0);
			foreach (IResourceLocation resourceLocation in this.m_Dependencies)
			{
				this.m_DependencyHashCode = this.m_DependencyHashCode * 31 + resourceLocation.Hash(typeof(object));
			}
		}

		// Token: 0x040000A4 RID: 164
		private string m_Name;

		// Token: 0x040000A5 RID: 165
		private string m_Id;

		// Token: 0x040000A6 RID: 166
		private string m_ProviderId;

		// Token: 0x040000A7 RID: 167
		private object m_Data;

		// Token: 0x040000A8 RID: 168
		private int m_DependencyHashCode;

		// Token: 0x040000A9 RID: 169
		private int m_HashCode;

		// Token: 0x040000AA RID: 170
		private Type m_Type;

		// Token: 0x040000AB RID: 171
		private List<IResourceLocation> m_Dependencies;

		// Token: 0x040000AC RID: 172
		private string m_PrimaryKey;
	}
}
