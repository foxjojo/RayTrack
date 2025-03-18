using System.Numerics;

namespace RayTrack;

public class Ray
{
    public Vector3 orig;
    public Vector3 dir;
    public Ray(Vector3 origin, Vector3 direction)
    {
        orig = origin;
        dir = direction;
    }
    public Vector3 At(float t) => orig + t * dir;
}