using System;
using UnityEngine;

namespace Features.EggRotor
{
	// Token: 0x02001223 RID: 4643
	[Serializable]
	public sealed class EggRotorRigSettings
	{
		// Token: 0x170016F5 RID: 5877
		// (get) Token: 0x06008FC0 RID: 36800 RVA: 0x003ACBDC File Offset: 0x003AADDC
		public Pose RemotePose
		{
			get
			{
				return new Pose(this.remotePosition, Quaternion.Euler(this.remoteEuler));
			}
		}

		// Token: 0x170016F6 RID: 5878
		// (get) Token: 0x06008FC1 RID: 36801 RVA: 0x003ACBF4 File Offset: 0x003AADF4
		public Pose OutsidePose
		{
			get
			{
				return new Pose(this.outsidePosition, Quaternion.Euler(this.outsideEuler));
			}
		}

		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x06008FC2 RID: 36802 RVA: 0x003ACC0C File Offset: 0x003AAE0C
		public Pose EntryPose
		{
			get
			{
				return new Pose(this.entryPosition, Quaternion.Euler(this.entryEuler));
			}
		}

		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x06008FC3 RID: 36803 RVA: 0x003ACC24 File Offset: 0x003AAE24
		public Pose InsidePose
		{
			get
			{
				return new Pose(this.insidePosition, Quaternion.Euler(this.insideEuler));
			}
		}

		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x06008FC4 RID: 36804 RVA: 0x003ACC3C File Offset: 0x003AAE3C
		public Pose GlansPose
		{
			get
			{
				return new Pose(this.glansPosition, Quaternion.Euler(this.glansEuler));
			}
		}

		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x06008FC5 RID: 36805 RVA: 0x003ACC54 File Offset: 0x003AAE54
		public Quaternion SwingEndRotation
		{
			get
			{
				return Quaternion.Euler(this.swingEndEuler);
			}
		}

		// Token: 0x06008FC6 RID: 36806 RVA: 0x003ACC64 File Offset: 0x003AAE64
		public bool MigrateLegacyInsidePose()
		{
			if (Vector3.Distance(this.insidePosition, EggRotorRigSettings.LegacyHalfCanalInsidePosition) < 1E-05f && Quaternion.Angle(Quaternion.Euler(this.insideEuler), Quaternion.Euler(EggRotorRigSettings.CanalEuler)) < 0.01f)
			{
				this.insidePosition = EggRotorRigSettings.DefaultInsidePosition;
				return true;
			}
			if ((!(this.insidePosition == EggRotorRigSettings.LegacyFullInsidePosition) && !(this.insidePosition == EggRotorRigSettings.LegacyHalfInsidePosition)) || !(this.insideEuler == Vector3.zero))
			{
				return false;
			}
			this.insidePosition = EggRotorRigSettings.DefaultInsidePosition;
			this.insideEuler = EggRotorRigSettings.CanalEuler;
			if (this.entryPosition == EggRotorRigSettings.LegacyEntryPosition && this.entryEuler == Vector3.zero)
			{
				this.entryPosition = EggRotorRigSettings.DefaultEntryPosition;
				this.entryEuler = EggRotorRigSettings.CanalEuler;
			}
			return true;
		}

		// Token: 0x04009370 RID: 37744
		[Header("Remote (thigh_stretch.r local space, on top of the prefab root's pose)")]
		[Tooltip("Band ring centre under the thigh bone (measured by the builder).")]
		public Vector3 remotePosition = Vector3.zero;

		// Token: 0x04009371 RID: 37745
		[Tooltip("Band frame rotation under the thigh bone: +Y up the thigh, +Z out through the box (measured by the builder).")]
		public Vector3 remoteEuler = Vector3.zero;

		// Token: 0x04009372 RID: 37746
		[Tooltip("Multiplies the prefab root's localScale under the thigh bone.")]
		public float remoteScale = 1f;

		// Token: 0x04009373 RID: 37747
		public static readonly Vector3 CanalOpening = new Vector3(0f, -0.0242f, -0.0337f);

		// Token: 0x04009374 RID: 37748
		public static readonly Vector3 CanalAxis = new Vector3(0f, 0.962f, -0.272f);

		// Token: 0x04009375 RID: 37749
		public const float EggLength = 0.0259f;

		// Token: 0x04009376 RID: 37750
		public const float InsideTailOutFraction = 0.1f;

		// Token: 0x04009377 RID: 37751
		public static readonly Vector3 CanalEuler = new Vector3(74.2f, 0f, 0f);

		// Token: 0x04009378 RID: 37752
		public static readonly Vector3 DefaultEntryPosition = EggRotorRigSettings.CanalOpening - EggRotorRigSettings.CanalAxis * 0.0259f;

		// Token: 0x04009379 RID: 37753
		public static readonly Vector3 DefaultInsidePosition = EggRotorRigSettings.CanalOpening - EggRotorRigSettings.CanalAxis * 0.0025900002f;

		// Token: 0x0400937A RID: 37754
		public static readonly Vector3 LegacyHalfCanalInsidePosition = EggRotorRigSettings.CanalOpening - EggRotorRigSettings.CanalAxis * 0.01295f;

		// Token: 0x0400937B RID: 37755
		public static readonly Vector3 LegacyHalfInsidePosition = new Vector3(0f, -0.0259f, 0.016f);

		// Token: 0x0400937C RID: 37756
		public static readonly Vector3 LegacyEntryPosition = new Vector3(0f, -0.027f, 0.024f);

		// Token: 0x0400937D RID: 37757
		public static readonly Vector3 LegacyFullInsidePosition = new Vector3(0f, -0.024f, -0.01f);

		// Token: 0x0400937E RID: 37758
		[Header("Egg root = cord socket frame (PussyAnchor axes, Ciel body units)")]
		[Tooltip("Egg world scale = body scale * this * the prefab root's scale.")]
		public float eggScale = 1f;

		// Token: 0x0400937F RID: 37759
		[Tooltip("Outside: taped on the vulva, +Y (taped side) away from the skin.")]
		public Vector3 outsidePosition = new Vector3(0f, -0.008f, 0.002f);

		// Token: 0x04009380 RID: 37760
		public Vector3 outsideEuler = new Vector3(90f, 0f, 0f);

		// Token: 0x04009381 RID: 37761
		[Tooltip("At the opening (tip at the entry): where the insertion slide starts and the removal ends.")]
		public Vector3 entryPosition = EggRotorRigSettings.DefaultEntryPosition;

		// Token: 0x04009382 RID: 37762
		public Vector3 entryEuler = EggRotorRigSettings.CanalEuler;

		// Token: 0x04009383 RID: 37763
		[Tooltip("Inside: half inserted, the tail and the cable outside the opening. The slide runs from the entry pose to this one, and the egg stays here (visible) while it is in.")]
		public Vector3 insidePosition = EggRotorRigSettings.DefaultInsidePosition;

		// Token: 0x04009384 RID: 37764
		public Vector3 insideEuler = EggRotorRigSettings.CanalEuler;

		// Token: 0x04009385 RID: 37765
		[Tooltip("Egg socket rotation relative to the Swing free end. The cord's FreeEndFollower already points +Z back up the cable, like the socket, so identity fits.")]
		public Vector3 swingEndEuler = Vector3.zero;

		// Token: 0x04009386 RID: 37766
		[Header("Glans (Futanari Mode): egg root in the glans frame, Ciel body units")]
		[Tooltip("Distance behind the tip of the penis (body units, along the shaft at rest) of the frame's origin: the egg's middle. The corona is widest about 0.022 behind the tip; the shaft starts about 0.026.")]
		public float glansStation = 0.039f;

		// Token: 0x04009387 RID: 37767
		[Tooltip("The shaft axis runs through the stations this far in front of and behind glansStation.")]
		public float glansStationSpan = 0.011f;

		// Token: 0x04009388 RID: 37768
		[Tooltip("Taped under the glans along the shaft: the tail toward the base (the cable leaves there), the taped side (+Y of the egg) away from the skin.")]
		public Vector3 glansPosition = EggRotorRigSettings.DefaultGlansPosition;

		// Token: 0x04009389 RID: 37769
		public Vector3 glansEuler = new Vector3(90f, 0f, 0f);

		// Token: 0x0400938A RID: 37770
		[Tooltip("Lays the tape's side flaps on the round shaft (off = the flat vulva tape).")]
		public bool wrapTapeOnGlans = true;

		// Token: 0x0400938B RID: 37771
		public const float DefaultGlansStation = 0.039f;

		// Token: 0x0400938C RID: 37772
		public const float DefaultGlansStationSpan = 0.011f;

		// Token: 0x0400938D RID: 37773
		public static readonly Vector3 DefaultGlansPosition = new Vector3(0f, -0.01295f, 0.0065f);
	}
}
