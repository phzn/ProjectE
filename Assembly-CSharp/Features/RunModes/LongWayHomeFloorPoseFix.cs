using System;
using UnityEngine;
using UnityEngine.Animations;

namespace Features.RunModes
{
	// Token: 0x02000CB7 RID: 3255
	public struct LongWayHomeFloorPoseFix : IAnimationJob
	{
		// Token: 0x06005E6F RID: 24175 RVA: 0x00004F31 File Offset: 0x00003131
		public void ProcessRootMotion(AnimationStream stream)
		{
		}

		// Token: 0x06005E70 RID: 24176 RVA: 0x00255AF4 File Offset: 0x00253CF4
		public void ProcessAnimation(AnimationStream stream)
		{
			if (!stream.isHumanStream)
			{
				return;
			}
			AnimationHumanStream animationHumanStream = stream.AsHuman();
			if (this.BodyPitch != 0f)
			{
				animationHumanStream.bodyLocalRotation *= Quaternion.Euler(this.BodyPitch, 0f, 0f);
			}
			if (this.Count <= 0)
			{
				return;
			}
			this.Add(ref animationHumanStream, 0, this.Muscle0, this.Offset0);
			this.Add(ref animationHumanStream, 1, this.Muscle1, this.Offset1);
			this.Add(ref animationHumanStream, 2, this.Muscle2, this.Offset2);
			this.Add(ref animationHumanStream, 3, this.Muscle3, this.Offset3);
			this.Add(ref animationHumanStream, 4, this.Muscle4, this.Offset4);
			this.Add(ref animationHumanStream, 5, this.Muscle5, this.Offset5);
			this.Add(ref animationHumanStream, 6, this.Muscle6, this.Offset6);
			this.Add(ref animationHumanStream, 7, this.Muscle7, this.Offset7);
		}

		// Token: 0x06005E71 RID: 24177 RVA: 0x00255BFA File Offset: 0x00253DFA
		private void Add(ref AnimationHumanStream human, int index, MuscleHandle muscle, float offset)
		{
			if (index >= this.Count || offset == 0f)
			{
				return;
			}
			human.SetMuscle(muscle, human.GetMuscle(muscle) + offset);
		}

		// Token: 0x06005E72 RID: 24178 RVA: 0x00255C20 File Offset: 0x00253E20
		public void SetMuscle(int index, MuscleHandle muscle)
		{
			switch (index)
			{
			case 0:
				this.Muscle0 = muscle;
				return;
			case 1:
				this.Muscle1 = muscle;
				return;
			case 2:
				this.Muscle2 = muscle;
				return;
			case 3:
				this.Muscle3 = muscle;
				return;
			case 4:
				this.Muscle4 = muscle;
				return;
			case 5:
				this.Muscle5 = muscle;
				return;
			case 6:
				this.Muscle6 = muscle;
				return;
			case 7:
				this.Muscle7 = muscle;
				return;
			default:
				return;
			}
		}

		// Token: 0x06005E73 RID: 24179 RVA: 0x00255C94 File Offset: 0x00253E94
		public void SetOffset(int index, float offset)
		{
			switch (index)
			{
			case 0:
				this.Offset0 = offset;
				return;
			case 1:
				this.Offset1 = offset;
				return;
			case 2:
				this.Offset2 = offset;
				return;
			case 3:
				this.Offset3 = offset;
				return;
			case 4:
				this.Offset4 = offset;
				return;
			case 5:
				this.Offset5 = offset;
				return;
			case 6:
				this.Offset6 = offset;
				return;
			case 7:
				this.Offset7 = offset;
				return;
			default:
				return;
			}
		}

		// Token: 0x06005E74 RID: 24180 RVA: 0x00255D08 File Offset: 0x00253F08
		public static bool TryFindMuscle(string name, out MuscleHandle muscle)
		{
			muscle = default(MuscleHandle);
			if (string.IsNullOrEmpty(name))
			{
				return false;
			}
			if (LongWayHomeFloorPoseFix._handles == null)
			{
				LongWayHomeFloorPoseFix._handles = new MuscleHandle[MuscleHandle.muscleHandleCount];
				MuscleHandle.GetMuscleHandles(LongWayHomeFloorPoseFix._handles);
			}
			foreach (MuscleHandle muscleHandle in LongWayHomeFloorPoseFix._handles)
			{
				if (string.Equals(muscleHandle.name, name, StringComparison.OrdinalIgnoreCase))
				{
					muscle = muscleHandle;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005F7F RID: 24447
		public const int Capacity = 8;

		// Token: 0x04005F80 RID: 24448
		public MuscleHandle Muscle0;

		// Token: 0x04005F81 RID: 24449
		public MuscleHandle Muscle1;

		// Token: 0x04005F82 RID: 24450
		public MuscleHandle Muscle2;

		// Token: 0x04005F83 RID: 24451
		public MuscleHandle Muscle3;

		// Token: 0x04005F84 RID: 24452
		public MuscleHandle Muscle4;

		// Token: 0x04005F85 RID: 24453
		public MuscleHandle Muscle5;

		// Token: 0x04005F86 RID: 24454
		public MuscleHandle Muscle6;

		// Token: 0x04005F87 RID: 24455
		public MuscleHandle Muscle7;

		// Token: 0x04005F88 RID: 24456
		public float Offset0;

		// Token: 0x04005F89 RID: 24457
		public float Offset1;

		// Token: 0x04005F8A RID: 24458
		public float Offset2;

		// Token: 0x04005F8B RID: 24459
		public float Offset3;

		// Token: 0x04005F8C RID: 24460
		public float Offset4;

		// Token: 0x04005F8D RID: 24461
		public float Offset5;

		// Token: 0x04005F8E RID: 24462
		public float Offset6;

		// Token: 0x04005F8F RID: 24463
		public float Offset7;

		// Token: 0x04005F90 RID: 24464
		public int Count;

		// Token: 0x04005F91 RID: 24465
		public float BodyPitch;

		// Token: 0x04005F92 RID: 24466
		private static MuscleHandle[] _handles;
	}
}
