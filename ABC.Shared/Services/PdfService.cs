using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;

namespace ABC.Shared.Services
{
	public class PdfService
	{
		public byte[] GeneratePdf(string content)
		{
			using (var document = new PdfDocument())
			{
				var page = document.AddPage();
				using (var graphics = XGraphics.FromPdfPage(page))
				{
					var font = new XFont("Arial", 12, XFontStyle.Regular);
					var format = new XStringFormat();
					graphics.DrawString(content, font, XBrushes.Black,
						new XRect(10, 10, page.Width, page.Height), format);
				}

				using (var stream = new MemoryStream())
				{
					document.Save(stream, false);
					return stream.ToArray();
				}
			}
		}
	}
}
