using System;

namespace MoreMountains.Feedbacks
{
	// Token: 0x02000269 RID: 617
	[Serializable]
	public abstract class MMSpringDefinition<T>
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06001181 RID: 4481
		// (set) Token: 0x06001182 RID: 4482
		public abstract T CurrentValue { get; set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06001183 RID: 4483
		// (set) Token: 0x06001184 RID: 4484
		public abstract T TargetValue { get; set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06001185 RID: 4485
		// (set) Token: 0x06001186 RID: 4486
		public abstract T Velocity { get; set; }

		// Token: 0x06001187 RID: 4487
		public abstract void UpdateSpringValue(float deltaTime);

		// Token: 0x06001188 RID: 4488
		public abstract void MoveToInstant(T newValue);

		// Token: 0x06001189 RID: 4489
		public abstract void Stop();

		// Token: 0x0600118A RID: 4490
		public abstract void SetInitialValue(T newInitialValue);

		// Token: 0x0600118B RID: 4491
		public abstract void RestoreInitialValue();

		// Token: 0x0600118C RID: 4492
		public abstract void SetCurrentValueAsInitialValue();

		// Token: 0x0600118D RID: 4493
		public abstract void MoveTo(T newValue);

		// Token: 0x0600118E RID: 4494
		public abstract void MoveToAdditive(T newValue);

		// Token: 0x0600118F RID: 4495
		public abstract void MoveToSubtractive(T newValue);

		// Token: 0x06001190 RID: 4496
		public abstract void MoveToRandom(T min, T max);

		// Token: 0x06001191 RID: 4497
		public abstract void Bump(T bumpAmount);

		// Token: 0x06001192 RID: 4498
		public abstract void BumpRandom(T min, T max);

		// Token: 0x06001193 RID: 4499
		public abstract void Finish();
	}
}
