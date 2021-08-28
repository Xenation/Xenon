using System.IO;

using UnityEngine;

namespace Xenon.Screenshot {
	public class Bitmap {

		public static byte[] GetDIBBytes(Texture2D texture) {
			Color32[] pixels = texture.GetPixels32();
			MemoryStream stream = new MemoryStream(40 + GetPixelArraySize(texture.width, texture.height));
			BinaryWriter writer = new BinaryWriter(stream);
			WriteDIBHeader(writer, texture.width, texture.height);
			WritePixels(writer, texture.width, texture.height, pixels);
			return stream.GetBuffer();
		}

		public static byte[] GetBytes(Texture2D texture) {
			Color32[] pixels = texture.GetPixels32();
			MemoryStream stream = new MemoryStream(14 + 40 + GetPixelArraySize(texture.width, texture.height));
			BinaryWriter writer = new BinaryWriter(stream);
			WriteHeader(writer, (uint) stream.Capacity, 14 + 40); // Header not used here
			WriteDIBHeader(writer, texture.width, texture.height);
			WritePixels(writer, texture.width, texture.height, pixels);
			return stream.GetBuffer();
		}

		private static void WriteHeader(BinaryWriter writer, uint totalSize, uint pixelsPointer) {
			writer.Write((byte) 0x42); // B
			writer.Write((byte) 0x4d); // M
			writer.Write(totalSize);
			writer.Write((short) 0);
			writer.Write((short) 0);
			writer.Write(pixelsPointer);
		}

		private static void WriteDIBHeader(BinaryWriter writer, int width, int height) {
			// Using BITMAPINFOHEADER version here
			writer.Write((uint) 40);
			writer.Write(width);
			writer.Write(height);
			writer.Write((ushort) 1);	// one color plane
			writer.Write((ushort) 24);	// 24 bits per pixel
			writer.Write((uint) 0);		// Compression: none/bi_rgb
			writer.Write((uint) 0);		// img size, can be 0 when using no compression
			writer.Write((int) 11811);	// horizontal pixels per meter, 11 811 is the approximate equivalent to 300 dpi
			writer.Write((int) 11811);	// vertical pixels per meter
			writer.Write((uint) 0);		// paletter color count
			writer.Write((uint) 0);		// important colors, ignored
		}

		private static void WritePixels(BinaryWriter writer, int width, int height, Color32[] pixels) {
			int rowSize = width * 3;
			int padding = (4 - rowSize % 4);
			if (padding == 4) {
				padding = 0;
			}
			rowSize += padding;
			for (int y = 0; y < height; y++) {
				for (int x = 0; x < width; x++) {
					ref Color32 pixel = ref pixels[y * width + x];
					writer.Write(pixel.b);
					writer.Write(pixel.g);
					writer.Write(pixel.r);
				}
				for (int pad = 0; pad < padding; pad++) {
					writer.Write((byte) 0);
				}
			}
		}

		private static int GetPixelArraySize(int width, int height) {
			int rowSize = width * 3;
			int padding = (4 - rowSize % 4);
			if (padding == 4) {
				padding = 0;
			}
			rowSize += padding;
			return rowSize * height;
		}

	}
}
