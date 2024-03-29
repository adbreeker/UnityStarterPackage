using UnityEngine;

namespace adbreeker_UnityPackage
{
    public class Destroyable : MonoBehaviour
    {
        private void Start()
        {
            SplitMesh();
        }

        public void SplitMesh()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                Debug.LogError("MeshFilter component not found!");
                return;
            }

            Mesh originalMesh = meshFilter.mesh;

            // Get the vertices, triangles, and other data from the original mesh
            Vector3[] vertices = originalMesh.vertices;
            int[] triangles = originalMesh.triangles;
            Vector3[] normals = originalMesh.normals;
            Vector2[] uv = originalMesh.uv;

            // Create an array to hold the split meshes
            Mesh[] splitMeshes = new Mesh[triangles.Length / 3];

            // Split the mesh into smaller meshes
            for (int i = 0; i < splitMeshes.Length; i++)
            {
                splitMeshes[i] = new Mesh();
                splitMeshes[i].name = "SplitMesh_" + i;

                int triangleIndex = i * 3;

                // Assign vertices, triangles, normals, and UVs to the split mesh
                splitMeshes[i].vertices = new Vector3[]
                {
                vertices[triangles[triangleIndex]],
                vertices[triangles[triangleIndex + 1]],
                vertices[triangles[triangleIndex + 2]]
                };

                splitMeshes[i].triangles = new int[] { 0, 1, 2 };
                splitMeshes[i].normals = new Vector3[] { normals[triangles[triangleIndex]], normals[triangles[triangleIndex + 1]], normals[triangles[triangleIndex + 2]] };
                splitMeshes[i].uv = new Vector2[] { uv[triangles[triangleIndex]], uv[triangles[triangleIndex + 1]], uv[triangles[triangleIndex + 2]] };

                // You can perform any additional modifications to the split mesh here (e.g., scaling, rotating, etc.)

                splitMeshes[i].RecalculateBounds();
            }

            // Optionally, you can instantiate game objects with the split meshes
            for (int i = 0; i < splitMeshes.Length; i++)
            {
                GameObject splitObject = new GameObject("SplitObject_" + i);
                MeshFilter splitMeshFilter = splitObject.AddComponent<MeshFilter>();
                MeshRenderer splitMeshRenderer = splitObject.AddComponent<MeshRenderer>();
                splitMeshFilter.mesh = splitMeshes[i];
                // Set appropriate materials, shaders, etc., for the split objects
                splitMeshRenderer.materials = gameObject.GetComponent<MeshRenderer>().materials;
                // You can position and parent the split objects however you like
                splitObject.transform.position = transform.position;
                splitObject.transform.rotation = transform.rotation;
                splitObject.transform.localScale = transform.localScale;

                splitObject.AddComponent<SphereCollider>();
                splitObject.AddComponent<Rigidbody>();
            }

            // Destroy the original mesh if desired
            Destroy(gameObject);
        }

        public Mesh JoinMeshes(Mesh mesh1, Mesh mesh2)
        {
            // Combine vertices, triangles, normals, and UVs
            Vector3[] combinedVertices = CombineArrays(mesh1.vertices, mesh2.vertices);
            int[] combinedTriangles = CombineArrays(mesh1.triangles, mesh2.triangles);
            Vector3[] combinedNormals = CombineArrays(mesh1.normals, mesh2.normals);
            Vector2[] combinedUVs = CombineArrays(mesh1.uv, mesh2.uv);

            // Create a new mesh and assign the combined data
            Mesh combinedMesh = new Mesh();
            combinedMesh.name = "CombinedMesh";
            combinedMesh.vertices = combinedVertices;
            combinedMesh.triangles = combinedTriangles;
            combinedMesh.normals = combinedNormals;
            combinedMesh.uv = combinedUVs;
            combinedMesh.RecalculateBounds();

            // Assign the combined mesh to the MeshFilter component
            mesh1 = combinedMesh;
            Destroy(mesh2);
            // Optionally, you can destroy the second mesh if desired
            return mesh1;
        }

        private T[] CombineArrays<T>(T[] array1, T[] array2)
        {
            T[] combinedArray = new T[array1.Length + array2.Length];
            array1.CopyTo(combinedArray, 0);
            array2.CopyTo(combinedArray, array1.Length);
            return combinedArray;
        }
    }
}


