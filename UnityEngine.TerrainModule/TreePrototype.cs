using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x0200000C RID: 12
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class TreePrototype
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000024B0 File Offset: 0x000006B0
		// (set) Token: 0x06000094 RID: 148 RVA: 0x000024C8 File Offset: 0x000006C8
		public GameObject prefab
		{
			get
			{
				return this.m_Prefab;
			}
			set
			{
				this.m_Prefab = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000095 RID: 149 RVA: 0x000024D4 File Offset: 0x000006D4
		// (set) Token: 0x06000096 RID: 150 RVA: 0x000024EC File Offset: 0x000006EC
		public float bendFactor
		{
			get
			{
				return this.m_BendFactor;
			}
			set
			{
				this.m_BendFactor = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000024F8 File Offset: 0x000006F8
		// (set) Token: 0x06000098 RID: 152 RVA: 0x00002510 File Offset: 0x00000710
		public int navMeshLod
		{
			get
			{
				return this.m_NavMeshLod;
			}
			set
			{
				this.m_NavMeshLod = value;
			}
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000251A File Offset: 0x0000071A
		public TreePrototype()
		{
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00002524 File Offset: 0x00000724
		public TreePrototype(TreePrototype other)
		{
			this.prefab = other.prefab;
			this.bendFactor = other.bendFactor;
			this.navMeshLod = other.navMeshLod;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00002558 File Offset: 0x00000758
		public override bool Equals(object obj)
		{
			return this.Equals(obj as TreePrototype);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00002578 File Offset: 0x00000778
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00002590 File Offset: 0x00000790
		private bool Equals(TreePrototype other)
		{
			bool flag = other == null;
			bool flag2;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = other == this;
				if (flag3)
				{
					flag2 = true;
				}
				else
				{
					bool flag4 = base.GetType() != other.GetType();
					if (flag4)
					{
						flag2 = false;
					}
					else
					{
						bool flag5 = this.prefab == other.prefab && this.bendFactor == other.bendFactor && this.navMeshLod == other.navMeshLod;
						flag2 = flag5;
					}
				}
			}
			return flag2;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002609 File Offset: 0x00000809
		internal bool Validate(out string errorMessage)
		{
			return TreePrototype.ValidateTreePrototype(this, out errorMessage);
		}

		// Token: 0x0600009F RID: 159
		[FreeFunction("TerrainDataScriptingInterface::ValidateTreePrototype")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool ValidateTreePrototype([NotNull("ArgumentNullException")] TreePrototype prototype, out string errorMessage);

		// Token: 0x0400001B RID: 27
		internal GameObject m_Prefab;

		// Token: 0x0400001C RID: 28
		internal float m_BendFactor;

		// Token: 0x0400001D RID: 29
		internal int m_NavMeshLod;
	}
}
