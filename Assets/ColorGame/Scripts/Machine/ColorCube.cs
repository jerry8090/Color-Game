using System.Collections.Generic;
using UnityEngine;
using ColorGame.Core;

namespace ColorGame.Machine
{
    /// <summary>
    /// A physical six-sided color cube (real 3D geometry, one material per face).
    /// It keeps the logical color of every face, so the result is never guessed from pixels.
    ///
    /// Face numbers (local directions): 0 = +Y (top), 1 = -Y, 2 = +X, 3 = -X, 4 = +Z, 5 = -Z.
    /// Colors stay attached to their faces while the cube rotates because they belong to the mesh.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Rigidbody))]
    public class ColorCube : MonoBehaviour
    {
        public const int FaceCount = 6;

        private static readonly Vector3[] FaceNormals =
        {
            Vector3.up, Vector3.down, Vector3.right, Vector3.left, Vector3.forward, Vector3.back
        };

        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Rigidbody body;
        [Tooltip("A white material. Each face gets its own copy, tinted with the face color.")]
        [SerializeField] private Material faceMaterialTemplate;
        [Tooltip("The white border around every face.")]
        [SerializeField] private Material frameMaterial;

        private readonly GameColor[] faceColors = new GameColor[FaceCount];
        private Material[] faceMaterials;
        private bool isReady;

        public Rigidbody Body => body;

        private void Awake()
        {
            if (meshFilter == null || meshRenderer == null || body == null ||
                faceMaterialTemplate == null || frameMaterial == null)
            {
                Debug.LogError("[ColorCube] A reference is missing on the ColorCube3D prefab.", this);
                return;
            }

            if (meshFilter.sharedMesh == null) meshFilter.sharedMesh = BuildMesh();

            // One material copy per face, so each face can have its own color.
            faceMaterials = new Material[FaceCount];
            Material[] all = new Material[FaceCount + 1];
            for (int i = 0; i < FaceCount; i++)
            {
                faceMaterials[i] = Instantiate(faceMaterialTemplate);
                all[i] = faceMaterials[i];
            }
            all[FaceCount] = frameMaterial;
            meshRenderer.sharedMaterials = all;

            isReady = true;
        }

        private void OnDestroy()
        {
            if (faceMaterials == null) return;
            foreach (Material material in faceMaterials)
            {
                if (material != null) Destroy(material);
            }
        }

        // ---------- Face colors ----------

        /// <summary>Gives each of the six faces its color. The array needs exactly six entries.</summary>
        public void SetFaceColors(GameColor[] colors, ColorGameConfig config)
        {
            if (!isReady || config == null) return;

            if (colors == null || colors.Length != FaceCount)
            {
                Debug.LogError($"[ColorCube] SetFaceColors needs exactly {FaceCount} colors.", this);
                return;
            }

            for (int i = 0; i < FaceCount; i++)
            {
                if (!config.TryGetColorDefinition(colors[i], out ColorDefinition definition)) continue;

                faceColors[i] = colors[i];
                faceMaterials[i].color = definition.uiColor;
            }
        }

        /// <summary>
        /// Uses the first six colors of the config in order, shifted by an offset.
        /// A simple "standard die" layout for the idle state.
        /// </summary>
        public void ApplyStandardLayout(ColorGameConfig config, int offset)
        {
            if (config == null || config.Colors.Count < FaceCount) return;

            GameColor[] colors = new GameColor[FaceCount];
            for (int i = 0; i < FaceCount; i++)
            {
                colors[i] = config.Colors[(i + offset) % config.Colors.Count].color;
            }
            SetFaceColors(colors, config);
        }

        public GameColor GetFaceColor(int faceIndex)
        {
            return faceColors[Mathf.Clamp(faceIndex, 0, FaceCount - 1)];
        }

        // ---------- Reading the physical orientation ----------

        /// <summary>The face whose outward direction points most upward right now (0 to 5).</summary>
        public int GetTopFaceIndex()
        {
            int best = 0;
            float bestUp = float.MinValue;

            for (int i = 0; i < FaceCount; i++)
            {
                float up = transform.TransformDirection(FaceNormals[i]).y;
                if (up > bestUp)
                {
                    bestUp = up;
                    best = i;
                }
            }
            return best;
        }

        /// <summary>The logical color of the face that is on top right now.</summary>
        public GameColor GetTopColor()
        {
            return faceColors[GetTopFaceIndex()];
        }

        // ---------- Mesh ----------

        /// <summary>
        /// Builds a unit cube with 7 submeshes: 0 to 5 = the colored face panels, 6 = all white frames.
        /// The editor builder saves this as an asset. ColorCube also builds it at run time if it is missing.
        /// </summary>
        public static Mesh BuildMesh(float frameWidth = 0.1f)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<int>[] faceTriangles = new List<int>[FaceCount];
            List<int> frameTriangles = new List<int>();

            float half = 0.5f;
            float inner = half - frameWidth;

            for (int f = 0; f < FaceCount; f++)
            {
                faceTriangles[f] = new List<int>();

                Vector3 n = FaceNormals[f];
                Vector3 u = Vector3.Cross(n, Mathf.Abs(n.y) > 0.9f ? Vector3.forward : Vector3.up).normalized;
                Vector3 v = Vector3.Cross(n, u).normalized;
                Vector3 center = n * half;

                // Inner (colored) square, then outer square for the frame ring.
                int innerStart = vertices.Count;
                AddSquare(vertices, normals, center, u, v, inner, n);
                int outerStart = vertices.Count;
                AddSquare(vertices, normals, center, u, v, half, n);

                AddQuad(faceTriangles[f], vertices, innerStart, innerStart + 1, innerStart + 2, innerStart + 3, n);

                for (int k = 0; k < 4; k++)
                {
                    int next = (k + 1) % 4;
                    AddQuad(frameTriangles, vertices, outerStart + k, outerStart + next, innerStart + next, innerStart + k, n);
                }
            }

            Mesh mesh = new Mesh { name = "ColorCubeMesh" };
            mesh.SetVertices(vertices);
            mesh.SetNormals(normals);
            mesh.subMeshCount = FaceCount + 1;
            for (int f = 0; f < FaceCount; f++) mesh.SetTriangles(faceTriangles[f], f);
            mesh.SetTriangles(frameTriangles, FaceCount);
            mesh.RecalculateBounds();
            return mesh;
        }

        private static void AddSquare(List<Vector3> vertices, List<Vector3> normals,
                                      Vector3 center, Vector3 u, Vector3 v, float size, Vector3 normal)
        {
            vertices.Add(center + (-u - v) * size);
            vertices.Add(center + (-u + v) * size);
            vertices.Add(center + (u + v) * size);
            vertices.Add(center + (u - v) * size);
            for (int i = 0; i < 4; i++) normals.Add(normal);
        }

        // Adds two triangles. The winding is flipped automatically if needed so the quad faces outward.
        private static void AddQuad(List<int> triangles, List<Vector3> vertices, int a, int b, int c, int d, Vector3 outward)
        {
            Vector3 facing = Vector3.Cross(vertices[b] - vertices[a], vertices[c] - vertices[a]);

            if (Vector3.Dot(facing, outward) >= 0f)
            {
                triangles.AddRange(new[] { a, b, c, a, c, d });
            }
            else
            {
                triangles.AddRange(new[] { a, c, b, a, d, c });
            }
        }
    }
}