using System;

namespace UnityEngine.InputSystem.Layouts
{
	// Token: 0x02000110 RID: 272
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class InputControlLayoutAttribute : Attribute
	{
		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06000FA0 RID: 4000 RVA: 0x0004B9D3 File Offset: 0x00049BD3
		// (set) Token: 0x06000FA1 RID: 4001 RVA: 0x0004B9DB File Offset: 0x00049BDB
		public Type stateType { get; set; }

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0004B9E4 File Offset: 0x00049BE4
		// (set) Token: 0x06000FA3 RID: 4003 RVA: 0x0004B9EC File Offset: 0x00049BEC
		public string stateFormat { get; set; }

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x06000FA4 RID: 4004 RVA: 0x0004B9F5 File Offset: 0x00049BF5
		// (set) Token: 0x06000FA5 RID: 4005 RVA: 0x0004B9FD File Offset: 0x00049BFD
		public string[] commonUsages { get; set; }

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x0004BA06 File Offset: 0x00049C06
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x0004BA0E File Offset: 0x00049C0E
		public string variants { get; set; }

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06000FA8 RID: 4008 RVA: 0x0004BA17 File Offset: 0x00049C17
		// (set) Token: 0x06000FA9 RID: 4009 RVA: 0x0004BA1F File Offset: 0x00049C1F
		public bool isNoisy { get; set; }

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06000FAA RID: 4010 RVA: 0x0004BA28 File Offset: 0x00049C28
		// (set) Token: 0x06000FAB RID: 4011 RVA: 0x0004BA35 File Offset: 0x00049C35
		public bool canRunInBackground
		{
			get
			{
				return this.canRunInBackgroundInternal.Value;
			}
			set
			{
				this.canRunInBackgroundInternal = new bool?(value);
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x0004BA43 File Offset: 0x00049C43
		// (set) Token: 0x06000FAD RID: 4013 RVA: 0x0004BA50 File Offset: 0x00049C50
		public bool updateBeforeRender
		{
			get
			{
				return this.updateBeforeRenderInternal.Value;
			}
			set
			{
				this.updateBeforeRenderInternal = new bool?(value);
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06000FAE RID: 4014 RVA: 0x0004BA5E File Offset: 0x00049C5E
		// (set) Token: 0x06000FAF RID: 4015 RVA: 0x0004BA66 File Offset: 0x00049C66
		public bool isGenericTypeOfDevice { get; set; }

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x06000FB0 RID: 4016 RVA: 0x0004BA6F File Offset: 0x00049C6F
		// (set) Token: 0x06000FB1 RID: 4017 RVA: 0x0004BA77 File Offset: 0x00049C77
		public string displayName { get; set; }

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0004BA80 File Offset: 0x00049C80
		// (set) Token: 0x06000FB3 RID: 4019 RVA: 0x0004BA88 File Offset: 0x00049C88
		public string description { get; set; }

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0004BA91 File Offset: 0x00049C91
		// (set) Token: 0x06000FB5 RID: 4021 RVA: 0x0004BA99 File Offset: 0x00049C99
		public bool hideInUI { get; set; }

		// Token: 0x0400068A RID: 1674
		internal bool? canRunInBackgroundInternal;

		// Token: 0x0400068B RID: 1675
		internal bool? updateBeforeRenderInternal;
	}
}
