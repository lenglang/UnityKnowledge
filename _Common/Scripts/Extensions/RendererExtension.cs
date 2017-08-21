using UnityEngine;
namespace WZK
{
    public static class RendererExtension
    {
        public static Rect GetScreenRect(this Renderer renderer, Camera camera)
        {
            Rect rect = new Rect();

            rect.min = camera.WorldToScreenPoint(renderer.bounds.min);//bottom-left
            rect.max = camera.WorldToScreenPoint(renderer.bounds.max);//top-right

            return rect;
        }

        public static bool IsVisibleByCamera(this Renderer renderer, Camera camera)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }

        public static bool Raycast(this Renderer renderer, Ray ray)
        {
            if (renderer.GetComponent<MeshFilter>() == null)
                return false;

            Mesh mesh = renderer.GetComponent<MeshFilter>().mesh;

            float t = 0, u = 0, v = 0;
            for (int i = 0; i < mesh.triangles.Length / 3; i++)
            {
                Vector3 v0 = (renderer.transform.rotation * (Matrix4x4.Scale(renderer.transform.lossyScale) * mesh.vertices[mesh.triangles[i * 3]])) + renderer.transform.position;
                Vector3 v1 = (renderer.transform.rotation * (Matrix4x4.Scale(renderer.transform.lossyScale) * mesh.vertices[mesh.triangles[i * 3 + 1]])) + renderer.transform.position;
                Vector3 v2 = (renderer.transform.rotation * (Matrix4x4.Scale(renderer.transform.lossyScale) * mesh.vertices[mesh.triangles[i * 3 + 2]])) + renderer.transform.position;

                if (IntersectTriangle(ray, v0, v1, v2, ref t, ref u, ref v))
                    return true;
            }

            return false;
        }

        private static bool IntersectTriangle(Ray ray, Vector3 v0, Vector3 v1, Vector3 v2, ref float t, ref float u, ref float v, bool cullface = true)
        {
            // Find vectors for two edges sharing vert0
            Vector3 edge1 = v1 - v0;
            Vector3 edge2 = v2 - v0;

            // Begin calculating determinant - also used to calculate U parameter
            Vector3 pvec = Vector3.Cross(ray.direction, edge2);

            // If determinant is near zero, ray lies in plane of triangle
            float det = Vector3.Dot(edge1, pvec);

            if (cullface)
            {
                if (det < 0.0001f)
                    return false;
            }
            else
            {
                if (det > -0.0001f && det < 0.0001f)
                    return false;
            }

            Vector3 tvec = ray.origin - v0;
            // Calculate U parameter and test bounds
            u = Vector3.Dot(tvec, pvec);
            if (u < 0.0f || u > det)
                return false;

            // Prepare to test V parameter
            Vector3 qvec = Vector3.Cross(tvec, edge1);

            // Calculate V parameter and test bounds
            v = Vector3.Dot(ray.direction, qvec);
            if (v < 0.0f || u + v > det)
                return false;

            // Calculate t, scale parameters, ray intersects triangle
            t = Vector3.Dot(edge2, qvec);
            float fInvDet = 1.0f / det;
            t *= fInvDet;
            u *= fInvDet;
            v *= fInvDet;

            return true;
        }
    }
}