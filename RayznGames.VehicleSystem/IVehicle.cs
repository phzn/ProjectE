using System;

namespace RayznGames.VehicleSystem
{
	// Token: 0x0200000D RID: 13
	public interface IVehicle
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000091 RID: 145
		// (set) Token: 0x06000092 RID: 146
		float horizontalInput { get; set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000093 RID: 147
		// (set) Token: 0x06000094 RID: 148
		float verticalInput { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000095 RID: 149
		// (set) Token: 0x06000096 RID: 150
		bool braking { get; set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000097 RID: 151
		bool isInControl { get; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000098 RID: 152
		float currentSpeed { get; }

		// Token: 0x06000099 RID: 153
		void InControl(bool state);

		// Token: 0x0600009A RID: 154
		void ConstrainRotation(bool state);

		// Token: 0x0600009B RID: 155
		bool OnGround();
	}
}
