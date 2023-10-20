using SkiaSharp;

static SKPoint BezierPoint(Span<SKPoint> controlPoints, float t)
{
    float u = 1 - t;
    float tt = t * t;
    float uu = u * u;
    float uuu = uu * u;
    float ttt = tt * t;

    return new SKPoint(
        uu * controlPoints[0].X + 2 * u * t * controlPoints[1].X + tt * controlPoints[2].X, 
        uu * controlPoints[0].Y + 2 * u * t * controlPoints[1].Y + tt * controlPoints[2].Y);
}

static void DrawCatmullRomSpline(SKCanvas canvas, List<SKPoint> points)
{
    if (points.Count < 4)
        return;

    for (float alpha = 0; alpha <= 1; alpha += 0.01f)
    {

        float alpha2 = alpha * alpha;
        float alpha3 = alpha2 * alpha;

        for (int i=1; i < points.Count - 2; i++)
        {
            var p0 = points[i - 1];
            var p1 = points[i];
            var p2 = points[i + 1];
            var p3 = points[i + 2];

            float x = 0.5f * (
                (2.0f * p1.X) +
                (-p0.X + p2.X) * alpha +
                (2.0f * p0.X - 5.0f * p1.X + 4.0f * p2.X - p3.X) * alpha2 +
                (-p0.X + 3.0f * p1.X - 3.0f * p2.X + p3.X) * alpha3);

            float y = 0.5f * (
                (2.0f * p1.Y) +
                (-p0.Y + p2.Y) * alpha +
                (2.0f * p0.Y - 5.0f * p1.Y + 4.0f * p2.Y - p3.Y) * alpha2 +
                (-p0.Y + 3.0f * p1.Y - 3.0f * p2.Y + p3.Y) * alpha3);

            canvas.DrawPoint(x, y, SKColors.Yellow);

        }
    }
}

var bitmap = new SKBitmap(800, 600);
Console.WriteLine("Type something (press Esc to exist):");

using (var canvas = new SKCanvas(bitmap))
{
     
    while (true)
    {
        var keyInfo = Console.ReadKey(intercept: true);

        if (keyInfo.Key == ConsoleKey.Escape)
            break;

        Console.Write(keyInfo.KeyChar.ToString());

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

                    var pathPoints = new List<SKPoint>();
                    
                }
                else if (verb == SKPathVerb.Quad)
                {
                    canvas.DrawPoint(points[0], SKColors.Red);
                    canvas.DrawPoint(points[1], SKColors.Green);
                    canvas.DrawPoint(BezierPoint(points, 0.5f), SKColors.Blue);
                    canvas.DrawPoint(points[2], SKColors.Red);

                    paint.Color = SKColors.Gray;
                    paint.StrokeWidth = 1;
                    canvas.DrawLine(points[0], points[1], paint);
                    canvas.DrawLine(points[2], points[1], paint);

                    var pathPoints = new List<SKPoint>();
                    pathPoints.Add(points[0]);
                    pathPoints.Add(points[0]);
                    pathPoints.Add(BezierPoint(points, 0.25f));
                    pathPoints.Add(BezierPoint(points, 0.5f));
                    pathPoints.Add(BezierPoint(points, 0.75f));
                    pathPoints.Add(points[2]);
                    pathPoints.Add(points[2]);

                    DrawCatmullRomSpline(canvas, pathPoints);
                }
                else if (verb == SKPathVerb.Cubic)
                {

                }
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


