using System;
using System.IO;
using ImageMagick;

namespace Prototype
{
	class Program
	{
		static void Main(string[] args)
		{

			if (args.Length <= 0)
			{
				Console.WriteLine("Set a filename and pathfile");
				return;
			}

			var path = args[0];
			var fname = @args[1];

			var fullPath = $@"{path}\{fname}";

			Console.WriteLine($"{fullPath}");

			Stream imageStream = GetImageStream(fullPath);
			
			if (imageStream is null)
			{
				Console.WriteLine($"Cannot read {fullPath} to create Stream");
				return;
			}

			var outputName = NewImageName(path, fname);

			var rotatedImage = RotateImge(imageStream, outputName);

			if (rotatedImage == default)
			{
				Console.WriteLine($"Cannot Create rotated file {outputName} to create Stream");
				return;
			}

			Console.WriteLine($"Created {outputName}");

		}

		private static Stream GetImageStream(string fname)
		{
			try
			{
				FileStream fsSource = new FileStream(
					fname,
					FileMode.Open,
					FileAccess.Read);

				return fsSource;
			}
			catch
			{
				return null;
			}

		}

		private static string NewImageName(string path, string fname)
		{
			if (fname.Length <= 4)
			{
				return fname;
			}

			return $@"{path}\{fname.Replace(".jpg", "-rotated.jpg")}";

		}

		private static string RotateImge(Stream image, string fname)
		{

			string writtenFile = default;
			using (var img = new MagickImage(image))
			{
				img.AutoOrient();   // Fix orientation
				img.Strip();        // remove all EXIF information
				img.Write($"{fname}");
				writtenFile = fname;
			}
			Console.WriteLine($"Reaching: {fname}");
			return fname;
		}
	}
}
