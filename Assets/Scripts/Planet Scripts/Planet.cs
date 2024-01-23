using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Thank you to Sebastian Lague for his Procedural Planets tutorials which I leaned heavily on for this.

public class Planet : MonoBehaviour
{
    [Range(2,1024)]      // attributes: range of resolutions 
   public int resolution = 10;

   // for the Editor
   public bool autoUpdate = true;

    // for the Editor, what faces do we want rendered.
   public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
   public FaceRenderMask faceRenderMask;

   public ShapeSettings shapeSettings;
   public ColourSettings colourSettings;

   [HideInInspector]
   public bool shapeSettingsFoldout;

   [HideInInspector]
   public bool colourSettingsFoldout;

    // initialise shape
   ShapeGenerator shapeGenerator = new ShapeGenerator();
   // initialise colour for shading now
   ColourGenerator colourGenerator = new ColourGenerator();
   
    //create 6 MeshFilters for displaying the faces on the Cube Sphere
   
   [SerializeField]       //attributes so it's saved in the editor
   MeshFilter[] meshFilters;
   TerrainFace[] terrainFaces;
  

    // generate the planet on scene start
    void Start() {
        GeneratePlanet();
    }

    void Initialise() {
    
        // update shape
        shapeGenerator.UpdateSettings(shapeSettings);

        // update colour for shading now
        colourGenerator.UpdateSettings(colourSettings);

        // create new meshes if needed
        // try to find old meshes first
        meshFilters = transform.GetComponentsInChildren<MeshFilter>();

        if (meshFilters == null || meshFilters.Length == 0) {
            meshFilters = new MeshFilter[6];
        }
        
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

        // create each MeshFilter and TerrainFace - one per side of the cube
        for(int iter = 0; iter < 6; iter++) {

            // only create a new mesh object if we need to
            if (meshFilters[iter] == null) {
                
                GameObject meshObject = new GameObject("mesh");
                

                // just to tidy things up
                meshObject.transform.parent = transform;

                // Add a mesh renderer and give it a material
                meshObject.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
            
                meshFilters[iter] = meshObject.AddComponent<MeshFilter>();
            
                // give the MeshFilter some mesh
                meshFilters[iter].sharedMesh = new Mesh();

                // add colliders
                MeshCollider meshcollider = meshObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
                meshcollider.sharedMesh = meshFilters[iter].sharedMesh;
            }
            
            // assign material to meshes
            meshFilters[iter].GetComponent<MeshRenderer>().sharedMaterial = colourSettings.planetMaterial;


            terrainFaces[iter] = new TerrainFace(shapeGenerator, meshFilters[iter].sharedMesh, resolution, directions[iter]);
            
            // option to not render the face - for editor
            // predicate - if the enum says all should be rendered, or just the current face, render it
            bool renderFace = (faceRenderMask == FaceRenderMask.All || (int) faceRenderMask -1 == iter);
            meshFilters[iter].gameObject.SetActive(renderFace);
       }
   }

    // Method for generating the planet for all values
    public void GeneratePlanet() {
        Initialise();
        GenerateMesh();
        GenerateColours();
    }

    // Method for generating the planet if the shape's changed 
    public void OnShapeSettingsUpdated() {
       if (autoUpdate) 
       {
            Initialise();
            GenerateMesh();
       }
       
   }

    // Method for generating the planet if the colour's changed 
    public void OnColourSettingsUpdated() {
        if (autoUpdate) 
        {
            Initialise();
            GenerateColours();
        }
    }


    // a method to construct the planet's meshes
    void GenerateMesh() {
        for (int i = 0; i < 6; i++)
        {   
            // only generate the mesh if the mesh face is active
            if (meshFilters[i].gameObject.activeSelf) 
            {
                terrainFaces[i].ConstructMesh();
            }
        }
        // on any change, update the elevation
        colourGenerator.UpdateElevation(shapeGenerator.elevationMinMax);

    }

// A method that changes the planet's colour values based on the settings
    void GenerateColours() {
        colourGenerator.UpdateColours();

        // loop to update UVs
        for (int i = 0; i < 6; i++)
        {   
            // only update UVs if the mesh face is active
            if (meshFilters[i].gameObject.activeSelf) 
            {
                terrainFaces[i].UpdateUVs(colourGenerator);
            }
        }
    }


}

