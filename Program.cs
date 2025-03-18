using System.Numerics;
using System.Text;
using RayTrack;
using Color = System.Numerics.Vector3;

static bool HitSphere(Vector3 center, float r, Ray ray)
{
    Vector3 oc = center - ray.orig;
    var a = Vector3.Dot(ray.dir, ray.dir);
    var b = -2.0f * Vector3.Dot(ray.dir, oc);
    var c = Vector3.Dot(oc, oc) - r * r;
    var discriminant = b * b - 4 * a * c;
    return discriminant > 0;
}



static Color RayColor(Ray r)
{
    if (HitSphere(new Vector3(0, 0, -1), 0.5f, r))
    {
        return new Color(1, 0, 0);
    }
    Vector3 unitDir = r.dir / r.dir.Length();
    float a = 0.5f * (unitDir.Y + 1.0f);

    return (1.0f - a) * (new Color(1, 1, 1)) + a * (new Color(0.5f, 0.7f, 1));
}

static void WriteColor(StringBuilder stringBuilder, Color color)
{
    stringBuilder.Append((int)(color.X * 255) + " " + (int)(color.Y * 255) + " " + (int)(color.Z * 255) + "\n");
}

static void Main(string[] args)
{
    float aspectRatio = 16.0f / 9.0f;
    int imageWidth = 400;
    int imageHeight = (int)(imageWidth / aspectRatio);

    float focalLen = 1.0f;
    float viewportH = 2;
    float viewportW = viewportH * imageWidth / imageHeight;
    var cameraCenter = new Vector3(0, 0, 0);

    var viewportU = new Vector3(viewportW, 0, 0);
    var viewportV = new Vector3(0, -viewportH, 0);

    var pixelDeltaU = viewportU / imageWidth;
    var pixelDeltaV = viewportV / imageHeight;

    var viewportUpperLeft = cameraCenter - new Vector3(0, 0, focalLen) - viewportU / 2 - viewportV / 2;
    var pixel00Loc = viewportUpperLeft + 0.5f * (pixelDeltaU + pixelDeltaV);

    //Render
    StringBuilder stringBuilder = new StringBuilder("P3\n" + imageWidth + " " + imageHeight + "\n255\n");

    for (int i = 0; i < imageHeight; i++)
    {
        for (int j = 0; j < imageWidth; j++)
        {
            var pixelCenter = pixel00Loc + (j * pixelDeltaU) + (i * pixelDeltaV);
            var rayDir = pixelCenter - cameraCenter;
            Ray r = new Ray(cameraCenter, rayDir);
            Color pixelColor = RayColor(r);
            WriteColor(stringBuilder, pixelColor);
        }
    }

    File.WriteAllText("./image.ppm", stringBuilder.ToString());
}

Main(new[] { "" });