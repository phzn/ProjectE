using System;
using VehicleSpawnPlugin;

namespace Features.Vehicles.Traffic
{
	// Token: 0x02000BAA RID: 2986
	public static class TrafficDiag
	{
		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x060055A5 RID: 21925 RVA: 0x0021419C File Offset: 0x0021239C
		// (set) Token: 0x060055A6 RID: 21926 RVA: 0x002141A3 File Offset: 0x002123A3
		public static bool ClimbDiag
		{
			get
			{
				return VspDiag.Climb;
			}
			set
			{
				VspDiag.Climb = value;
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x060055A7 RID: 21927 RVA: 0x002141AB File Offset: 0x002123AB
		// (set) Token: 0x060055A8 RID: 21928 RVA: 0x002141B2 File Offset: 0x002123B2
		public static bool CarryDiag
		{
			get
			{
				return VspDiag.Carry;
			}
			set
			{
				VspDiag.Carry = value;
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x060055A9 RID: 21929 RVA: 0x002141BA File Offset: 0x002123BA
		// (set) Token: 0x060055AA RID: 21930 RVA: 0x002141C1 File Offset: 0x002123C1
		public static bool SenseDiag
		{
			get
			{
				return VspDiag.Sense;
			}
			set
			{
				VspDiag.Sense = value;
			}
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x060055AB RID: 21931 RVA: 0x002141C9 File Offset: 0x002123C9
		// (set) Token: 0x060055AC RID: 21932 RVA: 0x002141D0 File Offset: 0x002123D0
		public static bool CurveDiag
		{
			get
			{
				return VspDiag.Curve;
			}
			set
			{
				VspDiag.Curve = value;
			}
		}
	}
}
