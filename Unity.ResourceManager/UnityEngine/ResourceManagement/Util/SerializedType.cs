using System;
using System.IO;
using System.Reflection;
using UnityEngine.Serialization;

namespace UnityEngine.ResourceManagement.Util
{
	// Token: 0x02000026 RID: 38
	[Serializable]
	public struct SerializedType
	{
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00004E90 File Offset: 0x00003090
		public string AssemblyName
		{
			get
			{
				return this.m_AssemblyName;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00004E98 File Offset: 0x00003098
		public string ClassName
		{
			get
			{
				return this.m_ClassName;
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004EA0 File Offset: 0x000030A0
		public override string ToString()
		{
			if (!(this.Value == null))
			{
				return this.Value.Name;
			}
			return "<none>";
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00004EC4 File Offset: 0x000030C4
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00004F60 File Offset: 0x00003160
		public Type Value
		{
			get
			{
				Type type;
				try
				{
					if (string.IsNullOrEmpty(this.m_AssemblyName) || string.IsNullOrEmpty(this.m_ClassName))
					{
						type = null;
					}
					else
					{
						if (this.m_CachedType == null)
						{
							Assembly assembly = Assembly.Load(this.m_AssemblyName);
							if (assembly != null)
							{
								this.m_CachedType = assembly.GetType(this.m_ClassName);
							}
						}
						type = this.m_CachedType;
					}
				}
				catch (Exception ex)
				{
					if (ex.GetType() != typeof(FileNotFoundException))
					{
						Debug.LogException(ex);
					}
					type = null;
				}
				return type;
			}
			set
			{
				if (value != null)
				{
					this.m_AssemblyName = value.Assembly.FullName;
					this.m_ClassName = value.FullName;
					return;
				}
				this.m_AssemblyName = (this.m_ClassName = null);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00004FA4 File Offset: 0x000031A4
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00004FAC File Offset: 0x000031AC
		public bool ValueChanged { readonly get; set; }

		// Token: 0x0400005A RID: 90
		[FormerlySerializedAs("m_assemblyName")]
		[SerializeField]
		private string m_AssemblyName;

		// Token: 0x0400005B RID: 91
		[FormerlySerializedAs("m_className")]
		[SerializeField]
		private string m_ClassName;

		// Token: 0x0400005C RID: 92
		private Type m_CachedType;
	}
}
