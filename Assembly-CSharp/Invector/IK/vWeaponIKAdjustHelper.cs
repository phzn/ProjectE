using System;

namespace Invector.IK
{
	// Token: 0x02000520 RID: 1312
	public static class vWeaponIKAdjustHelper
	{
		// Token: 0x06001FDC RID: 8156 RVA: 0x000BD5A8 File Offset: 0x000BB7A8
		public static IKAdjust Copy(this IKAdjust iKAdjust)
		{
			return new IKAdjust
			{
				name = iKAdjust.name,
				spineOffset = iKAdjust.spineOffset.Copy(),
				supportHandOffset = iKAdjust.supportHandOffset.Copy(),
				supportHintOffset = iKAdjust.supportHintOffset.Copy(),
				weaponHandOffset = iKAdjust.weaponHandOffset.Copy(),
				weaponHintOffset = iKAdjust.weaponHintOffset.Copy()
			};
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000BD61C File Offset: 0x000BB81C
		public static IKAdjust Copy(this IKAdjust iKAdjust, string name)
		{
			return new IKAdjust
			{
				name = name,
				spineOffset = iKAdjust.spineOffset.Copy(),
				supportHandOffset = iKAdjust.supportHandOffset.Copy(),
				supportHintOffset = iKAdjust.supportHintOffset.Copy(),
				weaponHandOffset = iKAdjust.weaponHandOffset.Copy(),
				weaponHintOffset = iKAdjust.weaponHintOffset.Copy()
			};
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000BD68A File Offset: 0x000BB88A
		public static IKOffsetSpine Copy(this IKOffsetSpine iKOffsetSpine)
		{
			return new IKOffsetSpine
			{
				head = iKOffsetSpine.head,
				spine = iKOffsetSpine.spine
			};
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000BD6A9 File Offset: 0x000BB8A9
		public static IKOffsetTransform Copy(this IKOffsetTransform iKOffsetTransform)
		{
			return new IKOffsetTransform
			{
				position = iKOffsetTransform.position,
				eulerAngles = iKOffsetTransform.eulerAngles
			};
		}
	}
}
