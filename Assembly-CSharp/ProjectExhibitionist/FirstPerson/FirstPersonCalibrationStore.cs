using System;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ProjectExhibitionist.FirstPerson
{
	// Token: 0x020009E9 RID: 2537
	public static class FirstPersonCalibrationStore
	{
		// Token: 0x17000C2A RID: 3114
		// (get) Token: 0x060047C9 RID: 18377 RVA: 0x001B43E2 File Offset: 0x001B25E2
		public static string DefaultDirectory
		{
			get
			{
				return Path.Combine(Application.isEditor ? Path.GetDirectoryName(Application.dataPath) : Application.persistentDataPath, "CameraCalibrations", "FirstPerson");
			}
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x001B440C File Offset: 0x001B260C
		public static string Save(FirstPersonCalibrationCapture capture, string directory = null)
		{
			if (capture == null)
			{
				throw new ArgumentNullException("capture");
			}
			if (capture.schemaVersion != 1)
			{
				throw new InvalidDataException("Unsupported first-person calibration schema.");
			}
			capture.Sanitize();
			if (string.IsNullOrWhiteSpace(capture.capturedUtc))
			{
				capture.capturedUtc = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture);
			}
			string fullPath = Path.GetFullPath(string.IsNullOrWhiteSpace(directory) ? FirstPersonCalibrationStore.DefaultDirectory : directory);
			Directory.CreateDirectory(fullPath);
			string text = "fp_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fff", CultureInfo.InvariantCulture) + "_" + Guid.NewGuid().ToString("N");
			if (capture.screenshotFile == "AUTO")
			{
				capture.screenshotFile = text + ".png";
			}
			string text2 = Path.Combine(fullPath, text + ".json");
			string text3 = Path.Combine(fullPath, text + ".tmp");
			bool flag = false;
			string text4;
			try
			{
				byte[] bytes = new UTF8Encoding(false).GetBytes(JsonUtility.ToJson(capture, true));
				using (FileStream fileStream = new FileStream(text3, FileMode.CreateNew, FileAccess.Write, FileShare.None))
				{
					flag = true;
					fileStream.Write(bytes, 0, bytes.Length);
					fileStream.Flush(true);
				}
				File.Move(text3, text2);
				text4 = text2;
			}
			finally
			{
				if (flag && File.Exists(text3))
				{
					try
					{
						File.Delete(text3);
					}
					catch (IOException)
					{
					}
					catch (UnauthorizedAccessException)
					{
					}
				}
			}
			return text4;
		}

		// Token: 0x060047CB RID: 18379 RVA: 0x001B45AC File Offset: 0x001B27AC
		public static FirstPersonCalibrationCapture Load(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				throw new ArgumentException("A capture path is required.", "path");
			}
			string text = File.ReadAllText(path, Encoding.UTF8);
			JObject jobject = JObject.Parse(text);
			JToken jtoken = jobject["schemaVersion"];
			if (jtoken == null || jtoken.Type != JTokenType.Integer || jobject["schemaVersion"].Value<int>() != 1)
			{
				throw new InvalidDataException("Unsupported or invalid first-person calibration capture.");
			}
			JToken jtoken2 = jobject["adjustment"];
			if (jtoken2 != null && jtoken2.Type == JTokenType.Object)
			{
				JToken jtoken3 = jobject["context"];
				if (jtoken3 != null && jtoken3.Type == JTokenType.Object)
				{
					FirstPersonCalibrationCapture firstPersonCalibrationCapture = JsonUtility.FromJson<FirstPersonCalibrationCapture>(text);
					firstPersonCalibrationCapture.Sanitize();
					return firstPersonCalibrationCapture;
				}
			}
			throw new InvalidDataException("First-person calibration capture is missing adjustment or context data.");
		}
	}
}
