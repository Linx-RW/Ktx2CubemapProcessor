using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubemapAssemblerForm
{

    //
    //  This kind of worked but isn't very good, the triangles are very visible. Would recommend a separate program in the future
    //
    public partial class MainWindow
    {
        public static Image<L16> GenerateHeightmapParallel(string objFilePath, int width = 2048, int height = 1024)
        {
            var triangles = OBJLoader.LoadOBJ(objFilePath);
            var bvh = new BVHNode(triangles, 0, triangles.Count);

            var heightmap = new float[width, height];
            float minDistance = float.MaxValue;
            float maxDistance = float.MinValue;

            Parallel.For(0, height, y =>
            {
                for (int x = 0; x < width; x++)
                {
                    float u = (float)x / width;
                    float v = (float)y / height;
                    float phi = u * 2.0f * (float)Math.PI;
                    float theta = v * (float)Math.PI;

                    float sinTheta = (float)Math.Sin(theta);
                    float cosTheta = (float)Math.Cos(theta);
                    float sinPhi = (float)Math.Sin(phi);
                    float cosPhi = (float)Math.Cos(phi);

                    Vec3 direction = new Vec3(sinTheta * cosPhi, cosTheta, sinTheta * sinPhi);
                    Ray ray = new Ray(new Vec3(0, 0, 0), direction);

                    if (bvh.Intersect(ray, out float distance, out _))
                    {
                        heightmap[x, y] = distance;
                        lock (bvh)
                        {
                            minDistance = Math.Min(minDistance, distance);
                            maxDistance = Math.Max(maxDistance, distance);
                        }
                    }
                    else
                    {
                        heightmap[x, y] = 0;
                    }
                }
            });

            var image = new Image<L16>(width, height);
            float range = maxDistance - minDistance;

            if (range > 0)
            {
                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        float normalized = (heightmap[x, y] - minDistance) / range;
                        ushort pixelValue = (ushort)(normalized * 65535);
                        image[x, y] = new L16(pixelValue);
                    }
                });
            }

            return image;
        }
    }



    public class HeightmapGenerator
    {
        public static Image<L16> GenerateHeightmapFromMesh(string objFilePath, int width = 2048, int height = 1024)
        {
            // Load mesh
            var triangles = OBJLoader.LoadOBJ(objFilePath);

            if (triangles.Count == 0)
                throw new Exception("No triangles loaded from OBJ file");

            // Build BVH
            var bvh = new BVHNode(triangles, 0, triangles.Count);

            // Create heightmap
            var heightmap = new float[width, height];
            float minDistance = float.MaxValue;
            float maxDistance = float.MinValue;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float u = (float)x / width;
                    float v = (float)y / height;

                    float phi = u * 2.0f * (float)Math.PI;
                    float theta = v * (float)Math.PI;

                    float sinTheta = (float)Math.Sin(theta);
                    float cosTheta = (float)Math.Cos(theta);
                    float sinPhi = (float)Math.Sin(phi);
                    float cosPhi = (float)Math.Cos(phi);

                    Vec3 direction = new Vec3(
                        sinTheta * cosPhi,
                        cosTheta,
                        sinTheta * sinPhi
                    );

                    Ray ray = new Ray(new Vec3(0, 0, 0), direction);

                    if (bvh.Intersect(ray, out float distance, out _))
                    {
                        heightmap[x, y] = distance;
                        minDistance = Math.Min(minDistance, distance);
                        maxDistance = Math.Max(maxDistance, distance);
                    }
                    else
                    {
                        heightmap[x, y] = 0;
                    }
                }
            }

            var image = new Image<L16>(width, height);
            float range = maxDistance - minDistance;

            if (range > 0)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        float normalized = (heightmap[x, y] - minDistance) / range;
                        ushort pixelValue = (ushort)(normalized * 65535);
                        image[x, y] = new L16(pixelValue);
                    }
                }
            }

            return image;
        }
    }

    //
    //  Utils
    //

    // Vector3 with basic operations
    public struct Vec3
    {
        public float X, Y, Z;

        public Vec3(float x, float y, float z) { X = x; Y = y; Z = z; }
        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vec3 operator *(Vec3 a, float b) => new Vec3(a.X * b, a.Y * b, a.Z * b);
        public static Vec3 operator /(Vec3 a, float b) => new Vec3(a.X / b, a.Y / b, a.Z / b);
        public float Dot(Vec3 b) => X * b.X + Y * b.Y + Z * b.Z;
        public Vec3 Cross(Vec3 b) => new Vec3(Y * b.Z - Z * b.Y, Z * b.X - X * b.Z, X * b.Y - Y * b.X);
        public float LengthSquared() => X * X + Y * Y + Z * Z;
        public float Length() => (float)Math.Sqrt(LengthSquared());
        public Vec3 Normalized() => this / Length();
    }

    // Ray for tracing
    public struct Ray
    {
        public Vec3 Origin;
        public Vec3 Direction;

        public Ray(Vec3 origin, Vec3 direction)
        {
            Origin = origin;
            Direction = direction.Normalized();
        }

        public Vec3 At(float t) => Origin + Direction * t;
    }

    // Triangle face
    public class Triangle
    {
        public Vec3 V0, V1, V2;
        public Vec3 Normal;
        public BBox Bounds;

        public Triangle(Vec3 v0, Vec3 v1, Vec3 v2)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            CalculateNormal();
            CalculateBounds();
        }

        private void CalculateNormal()
        {
            Vec3 edge1 = V1 - V0;
            Vec3 edge2 = V2 - V0;
            Normal = edge1.Cross(edge2).Normalized();
        }

        private void CalculateBounds()
        {
            Bounds = new BBox(V0);
            Bounds.Expand(V1);
            Bounds.Expand(V2);
        }

        public bool Intersect(Ray ray, out float t, out Vec3 barycentric)
        {
            t = 0;
            barycentric = new Vec3();

            const float EPSILON = 1e-6f;

            Vec3 edge1 = V1 - V0;
            Vec3 edge2 = V2 - V0;
            Vec3 h = ray.Direction.Cross(edge2);
            float a = edge1.Dot(h);

            if (a > -EPSILON && a < EPSILON)
                return false; // Ray is parallel to triangle

            float f = 1.0f / a;
            Vec3 s = ray.Origin - V0;
            float u = f * s.Dot(h);

            if (u < 0.0 || u > 1.0)
                return false;

            Vec3 q = s.Cross(edge1);
            float v = f * ray.Direction.Dot(q);

            if (v < 0.0 || u + v > 1.0)
                return false;

            t = f * edge2.Dot(q);

            if (t > EPSILON)
            {
                barycentric = new Vec3(1 - u - v, u, v);
                return true;
            }

            return false;
        }
    }

    public class BBox
    {
        public Vec3 Min;
        public Vec3 Max;

        public BBox()
        {
            Min = new Vec3(float.MaxValue, float.MaxValue, float.MaxValue);
            Max = new Vec3(float.MinValue, float.MinValue, float.MinValue);
        }

        public BBox(Vec3 point)
        {
            Min = point;
            Max = point;
        }

        public void Expand(Vec3 point)
        {
            Min.X = Math.Min(Min.X, point.X);
            Min.Y = Math.Min(Min.Y, point.Y);
            Min.Z = Math.Min(Min.Z, point.Z);
            Max.X = Math.Max(Max.X, point.X);
            Max.Y = Math.Max(Max.Y, point.Y);
            Max.Z = Math.Max(Max.Z, point.Z);
        }

        public void Expand(BBox other)
        {
            Expand(other.Min);
            Expand(other.Max);
        }

        public Vec3 Center => (Min + Max) * 0.5f;

        public bool Intersect(Ray ray, float tMin = 0, float tMax = float.MaxValue)
        {
            for (int i = 0; i < 3; i++)
            {
                float invD = 1.0f / GetComponent(ray.Direction, i);
                float t0 = (GetComponent(Min, i) - GetComponent(ray.Origin, i)) * invD;
                float t1 = (GetComponent(Max, i) - GetComponent(ray.Origin, i)) * invD;

                if (invD < 0.0f)
                {
                    float temp = t0;
                    t0 = t1;
                    t1 = temp;
                }

                tMin = Math.Max(t0, tMin);
                tMax = Math.Min(t1, tMax);

                if (tMax <= tMin)
                    return false;
            }
            return true;
        }

        private float GetComponent(Vec3 v, int i)
        {
            return i == 0 ? v.X : (i == 1 ? v.Y : v.Z);
        }
    }

    public class BVHNode
    {
        public BBox Bounds;
        public BVHNode Left;
        public BVHNode Right;
        public List<Triangle> Triangles;
        public bool IsLeaf => Triangles != null;

        public BVHNode(List<Triangle> triangles, int start, int end, int depth = 0)
        {
            Triangles = new List<Triangle>(end - start);

            Bounds = new BBox();
            for (int i = start; i < end; i++)
            {
                Triangles.Add(triangles[i]);
                Bounds.Expand(triangles[i].Bounds);
            }

            int count = Triangles.Count;
            if (count <= 4 || depth >= 24)
            {
                return;
            }

            Vec3 size = Bounds.Max - Bounds.Min;
            int axis = 0;
            if (size.Y > size.X) axis = 1;
            if (size.Z > GetAxis(size, axis)) axis = 2;

            Triangles.Sort((a, b) =>
            {
                float aCenter = GetAxis(a.Bounds.Center, axis);
                float bCenter = GetAxis(b.Bounds.Center, axis);
                return aCenter.CompareTo(bCenter);
            });

            int mid = count / 2;

            Left = new BVHNode(Triangles, 0, mid, depth + 1);
            Right = new BVHNode(Triangles, mid, count, depth + 1);

            Triangles = null;
        }

        private float GetAxis(Vec3 v, int axis)
        {
            return axis == 0 ? v.X : (axis == 1 ? v.Y : v.Z);
        }

        public bool Intersect(Ray ray, out float t, out Triangle hitTriangle)
        {
            t = float.MaxValue;
            hitTriangle = null;

            if (!Bounds.Intersect(ray))
                return false;

            if (IsLeaf)
            {
                bool hit = false;
                foreach (var triangle in Triangles)
                {
                    if (triangle.Intersect(ray, out float tLocal, out _))
                    {
                        if (tLocal < t && tLocal > 0.001f)
                        {
                            t = tLocal;
                            hitTriangle = triangle;
                            hit = true;
                        }
                    }
                }
                return hit;
            }

            bool leftHit = Left.Intersect(ray, out float tLeft, out Triangle leftTriangle);
            bool rightHit = Right.Intersect(ray, out float tRight, out Triangle rightTriangle);

            if (leftHit && rightHit)
            {
                if (tLeft < tRight)
                {
                    t = tLeft;
                    hitTriangle = leftTriangle;
                }
                else
                {
                    t = tRight;
                    hitTriangle = rightTriangle;
                }
                return true;
            }
            else if (leftHit)
            {
                t = tLeft;
                hitTriangle = leftTriangle;
                return true;
            }
            else if (rightHit)
            {
                t = tRight;
                hitTriangle = rightTriangle;
                return true;
            }

            return false;
        }
    }

    public class OBJLoader
    {
        public static List<Triangle> LoadOBJ(string filePath)
        {
            var triangles = new List<Triangle>();
            var vertices = new List<Vec3>();

            var lines = System.IO.File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2) continue;

                switch (parts[0])
                {
                    case "v": // Vertex
                        if (parts.Length >= 4)
                        {
                            float x = float.Parse(parts[1]);
                            float y = float.Parse(parts[2]);
                            float z = float.Parse(parts[3]);
                            vertices.Add(new Vec3(x, y, z));
                        }
                        break;

                    case "f": // Face
                        if (parts.Length >= 4)
                        {
                            var faceVertices = new List<Vec3>();

                            for (int i = 1; i < parts.Length; i++)
                            {
                                string[] vertexParts = parts[i].Split('/');
                                int vertexIndex = int.Parse(vertexParts[0]) - 1; // OBJ is 1-indexed
                                if (vertexIndex >= 0 && vertexIndex < vertices.Count)
                                {
                                    faceVertices.Add(vertices[vertexIndex]);
                                }
                            }

                            if (faceVertices.Count >= 3)
                            {
                                for (int i = 2; i < faceVertices.Count; i++)
                                {
                                    triangles.Add(new Triangle(
                                        faceVertices[0],
                                        faceVertices[i - 1],
                                        faceVertices[i]
                                    ));
                                }
                            }
                        }
                        break;
                }
            }

            Console.WriteLine($"Loaded {triangles.Count} triangles from {filePath}");
            return triangles;
        }
    }
}
