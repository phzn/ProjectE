using System;
using System.Collections.Generic;
using UnityEngine.Localization.PropertyVariants.TrackedProperties;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityEngine.Localization.PropertyVariants.TrackedObjects
{
	// Token: 0x02000107 RID: 263
	[DisplayName("Transform", null)]
	[CustomTrackedObject(typeof(Transform), false)]
	[Serializable]
	public class TrackedTransform : TrackedObject
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x00016F50 File Offset: 0x00015150
		protected virtual void AddPropertyHandlers(Dictionary<string, Action<float>> handlers)
		{
			handlers["m_LocalPosition.x"] = delegate(float val)
			{
				this.m_PositionToApply.x = val;
			};
			handlers["m_LocalPosition.y"] = delegate(float val)
			{
				this.m_PositionToApply.y = val;
			};
			handlers["m_LocalPosition.z"] = delegate(float val)
			{
				this.m_PositionToApply.z = val;
			};
			handlers["m_LocalRotation.x"] = delegate(float val)
			{
				this.m_RotationToApply.x = val;
			};
			handlers["m_LocalRotation.y"] = delegate(float val)
			{
				this.m_RotationToApply.y = val;
			};
			handlers["m_LocalRotation.z"] = delegate(float val)
			{
				this.m_RotationToApply.z = val;
			};
			handlers["m_LocalRotation.w"] = delegate(float val)
			{
				this.m_RotationToApply.w = val;
			};
			handlers["m_LocalScale.x"] = delegate(float val)
			{
				this.m_ScaleToApply.x = val;
			};
			handlers["m_LocalScale.y"] = delegate(float val)
			{
				this.m_ScaleToApply.y = val;
			};
			handlers["m_LocalScale.z"] = delegate(float val)
			{
				this.m_ScaleToApply.z = val;
			};
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x00017043 File Offset: 0x00015243
		public override bool CanTrackProperty(string propertyPath)
		{
			return !propertyPath.StartsWith("m_LocalEulerAnglesHint") && !(propertyPath == "m_RootOrder");
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00017064 File Offset: 0x00015264
		public override AsyncOperationHandle ApplyLocale(Locale variantLocale, Locale defaultLocale)
		{
			Transform transform = (Transform)base.Target;
			if (this.m_PropertyHandlers == null)
			{
				this.m_PropertyHandlers = new Dictionary<string, Action<float>>();
				this.AddPropertyHandlers(this.m_PropertyHandlers);
			}
			this.m_PositionToApply = transform.localPosition;
			this.m_RotationToApply = transform.localRotation;
			this.m_ScaleToApply = transform.localScale;
			LocaleIdentifier identifier = variantLocale.Identifier;
			LocaleIdentifier localeIdentifier = ((defaultLocale != null) ? defaultLocale.Identifier : default(LocaleIdentifier));
			foreach (ITrackedProperty trackedProperty in base.TrackedProperties)
			{
				float num;
				Action<float> action;
				if (((FloatTrackedProperty)trackedProperty).GetValue(identifier, localeIdentifier, out num) && this.m_PropertyHandlers.TryGetValue(trackedProperty.PropertyPath, out action))
				{
					action(num);
				}
			}
			transform.localScale = this.m_ScaleToApply;
			transform.localPosition = this.m_PositionToApply;
			transform.localRotation = this.m_RotationToApply;
			return default(AsyncOperationHandle);
		}

		// Token: 0x0400023F RID: 575
		private Vector3 m_PositionToApply;

		// Token: 0x04000240 RID: 576
		private Quaternion m_RotationToApply;

		// Token: 0x04000241 RID: 577
		private Vector3 m_ScaleToApply;

		// Token: 0x04000242 RID: 578
		private Dictionary<string, Action<float>> m_PropertyHandlers;
	}
}
