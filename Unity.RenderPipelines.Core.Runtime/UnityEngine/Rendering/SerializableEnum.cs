using System;

namespace UnityEngine.Rendering
{
	// Token: 0x02000056 RID: 86
	[Serializable]
	public class SerializableEnum
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000C8D0 File Offset: 0x0000AAD0
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x0000C90C File Offset: 0x0000AB0C
		public Enum value
		{
			get
			{
				object obj;
				if (string.IsNullOrEmpty(this.m_EnumTypeAsString) || !Enum.TryParse(Type.GetType(this.m_EnumTypeAsString), this.m_EnumValueAsString, out obj))
				{
					return null;
				}
				return (Enum)obj;
			}
			set
			{
				this.m_EnumValueAsString = value.ToString();
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000C91A File Offset: 0x0000AB1A
		public SerializableEnum(Type enumType)
		{
			this.m_EnumTypeAsString = enumType.AssemblyQualifiedName;
			this.m_EnumValueAsString = Enum.GetNames(enumType)[0];
		}

		// Token: 0x0400019F RID: 415
		[SerializeField]
		private string m_EnumValueAsString;

		// Token: 0x040001A0 RID: 416
		[SerializeField]
		private string m_EnumTypeAsString;
	}
}
