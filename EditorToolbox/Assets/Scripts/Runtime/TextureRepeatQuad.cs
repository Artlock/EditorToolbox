using UnityEngine;

namespace ToolboxEngine
{
    [ExecuteAlways]
    public class TextureRepeatQuad : MonoBehaviour
    {
        private Vector3 currentLocalScale;

        private void Start()
        {
            RefreshMesh();
        }

        private void Update()
        {
            if (transform.localScale != currentLocalScale)
            {
                RefreshMesh();
            }
        }

        private Mesh GetMesh(MeshFilter meshFilter)
        {
            if (Application.isPlaying)
                return meshFilter.mesh;
            else
                return meshFilter.sharedMesh;
        }

        private void RefreshMesh()
        {
            currentLocalScale = transform.localScale;

            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null) return;

            Mesh mesh = GetMesh(meshFilter);
            if (mesh == null) return;

            string meshID = "Mesh" + GetInstanceID();

            if (Application.isPlaying)
            {
                // Update name if needed
                if (meshFilter.mesh.name != meshID)
                    mesh.name = meshID;
            }
            else
            {
                // ISSUE :
                // We dont want to replace the shared mesh every time we resize it
                // SOLUTION :
                // We resolve that by enforcing a specific naming convention for shared meshes
                // And if the convention isnt applied we know the shared mesh wasnt replaced yet and do so

                if (meshFilter.sharedMesh.name != meshID)
                {
                    // By setting the shared mesh of our mesh filter we only replace the shared mesh of this specific filter
                    // It does not override the shared mesh for other mesh filters
                    // Meaning it allows us to ungroup our specific mesh

                    // Instantiate a copy
                    Mesh meshCopy = Instantiate(meshFilter.sharedMesh);

                    // Update name since we recreate a new mesh
                    meshCopy.name = meshID;

                    // Assign the copy as sharedmesh for that mesh in particular
                    meshFilter.sharedMesh = meshCopy;

                    // Update required to work with the previously assigned mesh variable outside of the scope since we changed the mesh
                    mesh = meshFilter.sharedMesh;
                }
            }

            mesh.uv = SetupUVMap();

            Renderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer.sharedMaterial.mainTexture.wrapMode != TextureWrapMode.Repeat)
                meshRenderer.sharedMaterial.mainTexture.wrapMode = TextureWrapMode.Repeat;
        }

        private Vector2[] SetupUVMap()
        {
            // WHY THIS METHOD :
            // We change the UV map of the mesh to affect every mesh individually
            // WHY NOT THE USUAL METHOD :
            // If we were to change the material tiling instead we would affect every occurence of the material the same way
            // That isnt all that great since it means that we would need multiple materials to have multiple diffent tilings
            // It would also force us to setup the tiling manually for each material depending on the scale of the object using said material

            // WHAT ISSUE DOES THAT PROVOKE :
            // The only issue with that is that its effects are only visible in play mode
            // HOW DO WE GO AROUND IT :
            // But we can remediate that by using an editor script

            float scaleX = transform.localScale.x;
            float scaleY = transform.localScale.y;

            Vector2[] meshUVs = new Vector2[4];

            meshUVs[0] = new Vector2(0f, 0f);
            meshUVs[1] = new Vector2(scaleX, 0f);
            meshUVs[2] = new Vector2(0f, scaleY);
            meshUVs[3] = new Vector2(scaleX, scaleY);

            return meshUVs;
        }
    }
}
