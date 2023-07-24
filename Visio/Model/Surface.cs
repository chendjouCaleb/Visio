using System.Collections.Generic;
using SkiaSharp;

namespace Visio.Model
{
	public class Surface
	{
		private SKCanvas _skCanvas;
		public float Width { get; set; }
		public float Height { get; set; }

		public HashSet<Canvas> CanvasList { get; set; } = new();
		
		
	}
}