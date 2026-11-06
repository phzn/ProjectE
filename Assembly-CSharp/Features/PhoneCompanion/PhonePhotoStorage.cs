using System;
using System.IO;
using ProjectExhibitionist.Core;

namespace Features.PhoneCompanion
{
	// Token: 0x02000E48 RID: 3656
	public static class PhonePhotoStorage
	{
		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x06006D46 RID: 27974 RVA: 0x002BD762 File Offset: 0x002BB962
		public static string MediaFolder
		{
			get
			{
				return PlayerMediaExport.GetSourceDirectory("Smartphone", null);
			}
		}

		// Token: 0x170011F2 RID: 4594
		// (get) Token: 0x06006D47 RID: 27975 RVA: 0x002BD76F File Offset: 0x002BB96F
		public static string ImagesFolder
		{
			get
			{
				return PlayerMediaExport.GetDirectory("Photos");
			}
		}

		// Token: 0x170011F3 RID: 4595
		// (get) Token: 0x06006D48 RID: 27976 RVA: 0x002BD77B File Offset: 0x002BB97B
		public static string VideosFolder
		{
			get
			{
				return PlayerMediaExport.GetDirectory("Videos");
			}
		}

		// Token: 0x06006D49 RID: 27977 RVA: 0x002BD787 File Offset: 0x002BB987
		public static bool IsPhotoBoothPhoto(string pathOrFileName)
		{
			return !string.IsNullOrWhiteSpace(pathOrFileName) && Path.GetFileNameWithoutExtension(pathOrFileName).StartsWith("PHOTOBOOTH_", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06006D4A RID: 27978 RVA: 0x002BD7A4 File Offset: 0x002BB9A4
		public static bool IsExcludedFromGallery(string pathOrFileName)
		{
			if (string.IsNullOrWhiteSpace(pathOrFileName))
			{
				return false;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(pathOrFileName);
			return PhonePhotoStorage.IsPhotoBoothPhoto(pathOrFileName) || fileNameWithoutExtension.StartsWith("NPC_PHOTO_", StringComparison.OrdinalIgnoreCase) || fileNameWithoutExtension.StartsWith("CCTV_", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x04006FFB RID: 28667
		public const string PhotoBoothPrefix = "PHOTOBOOTH_";
	}
}
