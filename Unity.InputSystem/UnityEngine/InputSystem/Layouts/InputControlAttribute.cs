using System;

namespace UnityEngine.InputSystem.Layouts
{
	// Token: 0x0200010D RID: 269
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	public sealed class InputControlAttribute : PropertyAttribute
	{
		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x06000F3D RID: 3901 RVA: 0x0004A537 File Offset: 0x00048737
		// (set) Token: 0x06000F3E RID: 3902 RVA: 0x0004A53F File Offset: 0x0004873F
		public string layout { get; set; }

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x0004A548 File Offset: 0x00048748
		// (set) Token: 0x06000F40 RID: 3904 RVA: 0x0004A550 File Offset: 0x00048750
		public string variants { get; set; }

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x0004A559 File Offset: 0x00048759
		// (set) Token: 0x06000F42 RID: 3906 RVA: 0x0004A561 File Offset: 0x00048761
		public string name { get; set; }

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06000F43 RID: 3907 RVA: 0x0004A56A File Offset: 0x0004876A
		// (set) Token: 0x06000F44 RID: 3908 RVA: 0x0004A572 File Offset: 0x00048772
		public string format { get; set; }

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06000F45 RID: 3909 RVA: 0x0004A57B File Offset: 0x0004877B
		// (set) Token: 0x06000F46 RID: 3910 RVA: 0x0004A583 File Offset: 0x00048783
		public string usage { get; set; }

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06000F47 RID: 3911 RVA: 0x0004A58C File Offset: 0x0004878C
		// (set) Token: 0x06000F48 RID: 3912 RVA: 0x0004A594 File Offset: 0x00048794
		public string[] usages { get; set; }

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06000F49 RID: 3913 RVA: 0x0004A59D File Offset: 0x0004879D
		// (set) Token: 0x06000F4A RID: 3914 RVA: 0x0004A5A5 File Offset: 0x000487A5
		public string parameters { get; set; }

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x0004A5AE File Offset: 0x000487AE
		// (set) Token: 0x06000F4C RID: 3916 RVA: 0x0004A5B6 File Offset: 0x000487B6
		public string processors { get; set; }

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06000F4D RID: 3917 RVA: 0x0004A5BF File Offset: 0x000487BF
		// (set) Token: 0x06000F4E RID: 3918 RVA: 0x0004A5C7 File Offset: 0x000487C7
		public string alias { get; set; }

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06000F4F RID: 3919 RVA: 0x0004A5D0 File Offset: 0x000487D0
		// (set) Token: 0x06000F50 RID: 3920 RVA: 0x0004A5D8 File Offset: 0x000487D8
		public string[] aliases { get; set; }

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x0004A5E1 File Offset: 0x000487E1
		// (set) Token: 0x06000F52 RID: 3922 RVA: 0x0004A5E9 File Offset: 0x000487E9
		public string useStateFrom { get; set; }

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x0004A5F2 File Offset: 0x000487F2
		// (set) Token: 0x06000F54 RID: 3924 RVA: 0x0004A5FA File Offset: 0x000487FA
		public uint bit { get; set; } = uint.MaxValue;

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0004A603 File Offset: 0x00048803
		// (set) Token: 0x06000F56 RID: 3926 RVA: 0x0004A60B File Offset: 0x0004880B
		public uint offset { get; set; } = uint.MaxValue;

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06000F57 RID: 3927 RVA: 0x0004A614 File Offset: 0x00048814
		// (set) Token: 0x06000F58 RID: 3928 RVA: 0x0004A61C File Offset: 0x0004881C
		public uint sizeInBits { get; set; }

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06000F59 RID: 3929 RVA: 0x0004A625 File Offset: 0x00048825
		// (set) Token: 0x06000F5A RID: 3930 RVA: 0x0004A62D File Offset: 0x0004882D
		public int arraySize { get; set; }

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06000F5B RID: 3931 RVA: 0x0004A636 File Offset: 0x00048836
		// (set) Token: 0x06000F5C RID: 3932 RVA: 0x0004A63E File Offset: 0x0004883E
		public string displayName { get; set; }

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x0004A647 File Offset: 0x00048847
		// (set) Token: 0x06000F5E RID: 3934 RVA: 0x0004A64F File Offset: 0x0004884F
		public string shortDisplayName { get; set; }

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x0004A658 File Offset: 0x00048858
		// (set) Token: 0x06000F60 RID: 3936 RVA: 0x0004A660 File Offset: 0x00048860
		public bool noisy { get; set; }

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x06000F61 RID: 3937 RVA: 0x0004A669 File Offset: 0x00048869
		// (set) Token: 0x06000F62 RID: 3938 RVA: 0x0004A671 File Offset: 0x00048871
		public bool synthetic { get; set; }

		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06000F63 RID: 3939 RVA: 0x0004A67A File Offset: 0x0004887A
		// (set) Token: 0x06000F64 RID: 3940 RVA: 0x0004A682 File Offset: 0x00048882
		public bool dontReset { get; set; }

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06000F65 RID: 3941 RVA: 0x0004A68B File Offset: 0x0004888B
		// (set) Token: 0x06000F66 RID: 3942 RVA: 0x0004A693 File Offset: 0x00048893
		public object defaultState { get; set; }

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06000F67 RID: 3943 RVA: 0x0004A69C File Offset: 0x0004889C
		// (set) Token: 0x06000F68 RID: 3944 RVA: 0x0004A6A4 File Offset: 0x000488A4
		public object minValue { get; set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06000F69 RID: 3945 RVA: 0x0004A6AD File Offset: 0x000488AD
		// (set) Token: 0x06000F6A RID: 3946 RVA: 0x0004A6B5 File Offset: 0x000488B5
		public object maxValue { get; set; }
	}
}
