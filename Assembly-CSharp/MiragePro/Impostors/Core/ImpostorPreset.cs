using System;
using UnityEngine;

namespace MiragePro.Impostors.Core
{
	// Token: 0x0200027A RID: 634
	public class ImpostorPreset : ScriptableObject
	{
		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600107F RID: 4223 RVA: 0x0006D99E File Offset: 0x0006BB9E
		public bool FibonacciSphere
		{
			get
			{
				return this.type == SphereType.PseudoFibonacci;
			}
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x0006D9AC File Offset: 0x0006BBAC
		public static ImpostorPreset Clone(ImpostorPreset source)
		{
			ImpostorPreset impostorPreset = ScriptableObject.CreateInstance<ImpostorPreset>();
			impostorPreset.resIndex = source.resIndex;
			impostorPreset.latitudeSamples = source.latitudeSamples;
			impostorPreset.latitudeOffset = source.latitudeOffset;
			impostorPreset.latitudeAngularStep = source.latitudeAngularStep;
			impostorPreset.longitudeSamples = source.longitudeSamples;
			impostorPreset.longitudeOffset = source.longitudeOffset;
			impostorPreset.longitudeAngularStep = source.longitudeAngularStep;
			impostorPreset.type = source.type;
			impostorPreset.lightingMethod = source.lightingMethod;
			return impostorPreset;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x0006DA2C File Offset: 0x0006BC2C
		public static bool operator ==(ImpostorPreset lhs, ImpostorPreset rhs)
		{
			if (lhs == null)
			{
				return rhs == null;
			}
			return rhs != null && (lhs.resIndex == rhs.resIndex && lhs.latitudeSamples == rhs.latitudeSamples && lhs.latitudeOffset == rhs.latitudeOffset && lhs.latitudeAngularStep == rhs.latitudeAngularStep && lhs.longitudeSamples == rhs.longitudeSamples && lhs.longitudeOffset == rhs.longitudeOffset && lhs.longitudeAngularStep == rhs.longitudeAngularStep && lhs.type == rhs.type) && lhs.lightingMethod == rhs.lightingMethod;
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x0006DAC6 File Offset: 0x0006BCC6
		public static bool operator !=(ImpostorPreset lhs, ImpostorPreset rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0006DAD2 File Offset: 0x0006BCD2
		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && this == (ImpostorPreset)obj;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x0006DAF8 File Offset: 0x0006BCF8
		public override int GetHashCode()
		{
			return this.resIndex.GetHashCode() ^ this.latitudeSamples.GetHashCode() ^ this.latitudeOffset.GetHashCode() ^ this.latitudeAngularStep.GetHashCode() ^ this.longitudeSamples.GetHashCode() ^ this.longitudeOffset.GetHashCode() ^ this.longitudeAngularStep.GetHashCode() ^ this.type.GetHashCode() ^ this.lightingMethod.GetHashCode();
		}

		// Token: 0x04001200 RID: 4608
		public int resIndex = 3;

		// Token: 0x04001201 RID: 4609
		public int latitudeSamples = 4;

		// Token: 0x04001202 RID: 4610
		public int latitudeOffset = 3;

		// Token: 0x04001203 RID: 4611
		public float latitudeAngularStep = 5f;

		// Token: 0x04001204 RID: 4612
		public int longitudeSamples = 36;

		// Token: 0x04001205 RID: 4613
		public float longitudeOffset;

		// Token: 0x04001206 RID: 4614
		public float longitudeAngularStep = 10f;

		// Token: 0x04001207 RID: 4615
		public SphereType type;

		// Token: 0x04001208 RID: 4616
		public LightingMethod lightingMethod;
	}
}
