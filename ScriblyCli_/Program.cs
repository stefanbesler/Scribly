using SkiaSharp;

var bitmap = new SKBitmap(800, 600);
Console.WriteLine("Type something (press Esc to exist):");

using (var canvas = new SKCanvas(bitmap))
{

    while (true)
    {
        var keyInfo = Console.ReadKey(intercept: true);

        if (keyInfo.Key == ConsoleKey.Escape)
            break;

        canvas.Clear(SKColors.White);
        using (var paint = new SKPaint())
        {
            paint.TextSize = 600;
            paint.Color = SKColors.Black;
            paint.IsAntialias = true;

            var path = paint.GetTextPath(keyInfo.KeyChar.ToString(), 0, 500);

            canvas.DrawText(keyInfo.KeyChar.ToString(), 0, 500, paint);


            var it = path.CreateIterator(false);
            var points = new Span<SKPoint>(new SKPoint[4]);
            SKPathVerb verb;
            while ((verb = it.Next(points)) != SKPathVerb.Done)
            {
                if (verb == SKPathVerb.Move)
                {

                }
                else if (verb == SKPathVerb.Line)
                {
                    paint.Color = SKColors.Red;
                    paint.StrokeWidth = 2;
                    canvas.DrawPoint(points[0], SKColors.Red);
                    canvas.DrawPoint(points[1], SKColors.Red);
                    canvas.DrawLine(points[0], points[1], paint);
                }
                else if (verb == SKPathVerb.Quad)
				{
                    canvas.DrawPoint(points[0], SKColors.Red);
                    canvas.DrawPoint(points[1], SKColors.Green);
                    canvas.DrawPoint(points[2], SKColors.Red);

                    paint.Color = SKColors.Gray;
					paint.StrokeWidth = 1;
                    canvas.DrawLine(points[0], points[1], paint);
                    canvas.DrawLine(points[2], points[1], paint);
                }
                else if (verb == SKPathVerb.Cubic)
                {
                    
                }
            }

			foreach(var p in points)
			{
                canvas.DrawPoint(p, SKColors.Red);
            }
        }



		using (var image = SKImage.FromBitmap(bitmap))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 90))
		using (var stream = File.OpenWrite("output.png"))
        {
			var pixmap = image.PeekPixels();

			data.SaveTo(stream);
		}
	}
}



Console.WriteLine("Fin.");


