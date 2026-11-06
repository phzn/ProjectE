using System;
using UnityEngine;

namespace Features.CielOS.Apps.Story
{
	// Token: 0x02001435 RID: 5173
	public static class PersonalDiaryPhotoLayout
	{
		// Token: 0x06009FCE RID: 40910 RVA: 0x0041F060 File Offset: 0x0041D260
		public static Vector2 RotatedHalfExtents(Vector2 size, float rotation)
		{
			float num = Mathf.Abs(PersonalDiaryPhotoLayout.FiniteOr(size.x, 0f));
			float num2 = Mathf.Abs(PersonalDiaryPhotoLayout.FiniteOr(size.y, 0f));
			float num3 = PersonalDiaryPhotoLayout.FiniteOr(rotation, 0f) % 360f * 0.017453292f;
			float num4 = Mathf.Abs(Mathf.Cos(num3));
			float num5 = Mathf.Abs(Mathf.Sin(num3));
			return new Vector2((num * num4 + num2 * num5) * 0.5f, (num * num5 + num2 * num4) * 0.5f);
		}

		// Token: 0x06009FCF RID: 40911 RVA: 0x0041F0E8 File Offset: 0x0041D2E8
		public static Rect Bounds(Vector2 center, Vector2 size, float rotation)
		{
			Vector2 vector = PersonalDiaryPhotoLayout.RotatedHalfExtents(size, rotation);
			center = new Vector2(PersonalDiaryPhotoLayout.FiniteOr(center.x, vector.x), PersonalDiaryPhotoLayout.FiniteOr(center.y, vector.y));
			return new Rect(center - vector, vector * 2f);
		}

		// Token: 0x06009FD0 RID: 40912 RVA: 0x0041F140 File Offset: 0x0041D340
		public static Vector2 ClampPosition(Vector2 center, Vector2 size, float rotation, float documentWidth, float top = 0f, float sidePadding = 0f, float maxBottom = 100000f)
		{
			documentWidth = Mathf.Max(1f, PersonalDiaryPhotoLayout.FiniteOr(documentWidth, 1560f));
			top = Mathf.Max(0f, PersonalDiaryPhotoLayout.FiniteOr(top, 0f));
			maxBottom = Mathf.Max(top, PersonalDiaryPhotoLayout.FiniteOr(maxBottom, 100000f));
			sidePadding = Mathf.Clamp(PersonalDiaryPhotoLayout.FiniteOr(sidePadding, 0f), 0f, documentWidth * 0.5f);
			Vector2 vector = PersonalDiaryPhotoLayout.RotatedHalfExtents(size, rotation);
			float num = sidePadding + vector.x;
			float num2 = documentWidth - sidePadding - vector.x;
			float num3 = top + vector.y;
			float num4 = maxBottom - vector.y;
			return new Vector2((num <= num2) ? Mathf.Clamp(PersonalDiaryPhotoLayout.FiniteOr(center.x, documentWidth * 0.5f), num, num2) : (documentWidth * 0.5f), (num3 <= num4) ? Mathf.Clamp(PersonalDiaryPhotoLayout.FiniteOr(center.y, num3), num3, num4) : ((top + maxBottom) * 0.5f));
		}

		// Token: 0x06009FD1 RID: 40913 RVA: 0x0041F234 File Offset: 0x0041D434
		public static float RequiredPageHeight(Vector2 center, Vector2 size, float rotation, float bottomPadding = 96f)
		{
			return Mathf.Max(0f, PersonalDiaryPhotoLayout.Bounds(center, size, rotation).yMax) + Mathf.Max(0f, PersonalDiaryPhotoLayout.FiniteOr(bottomPadding, 0f));
		}

		// Token: 0x06009FD2 RID: 40914 RVA: 0x001B3A41 File Offset: 0x001B1C41
		private static float FiniteOr(float value, float fallback)
		{
			if (!float.IsNaN(value) && !float.IsInfinity(value))
			{
				return value;
			}
			return fallback;
		}

		// Token: 0x0400A3AD RID: 41901
		public const float DocumentWidth = 1560f;

		// Token: 0x0400A3AE RID: 41902
		public const float MaximumPageHeight = 100000f;
	}
}
