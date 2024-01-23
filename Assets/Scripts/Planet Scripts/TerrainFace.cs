using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Thank you to Sebastian Lague for his Procedural Planets tutorials which I leaned heavily on for this.

public class TerrainFace
{

    ShapeGenerator shapeGenerator;  //shape generator object
    Mesh mesh;          // the mesh object
    int resolution;     // how detailed it needs to be - vertices on each edge of the mesh
    Vector3 localUp;      //which way is up
    
    // the two local x and z axes as such, perpendicular to localUp
    Vector3 axisA;
    Vector3 axisB;
    

    
    // constructor!!!!
    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp) {
        this.shapeGenerator = shapeGenerator;
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;
        
        //perpendicular to localUp
        this.axisA = new Vector3(localUp.y, localUp.z, localUp.x);

        //perpendicular to axisA and localUp - cross Product matrix
        this.axisB = Vector3.Cross(localUp, axisA);
    }
    
    //Methoid called to construct the mesh

    public void ConstructMesh() {
        // Store all the vertices in an array. 
        //Total number of vertices in the mesh = area of the square
        Vector3[] vertices = new Vector3[resolution * resolution];
        Vector2[] uv = mesh.uv;

        // Store the triangles' vertices in an array.
        // Number of squares = (resolution-1) * (resolution - 1)
        // number of triangles = 2 * (resolution-1) * (resolution - 1)
        // so the number of vertices to store = 2 * 3 * (resolution-1) * (resolution - 1)

        int[] triangles = new int[6 * (resolution-1) * (resolution - 1)];
        // number of triangle vertices
        int triIndex = 0;
        
        // loops to build the mesh
        int i = 0;
        
        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                
                //what vertex we're on
                i = x + y * resolution;
                
                // how close to complete each loop is
                Vector2 percentAlongAxes = new Vector2(x,y) / (resolution - 1);
                
                // how far along each axis we are, between -1 and +1 (including y);
                Vector3 pointOnUnitCube = localUp + (percentAlongAxes.x - 0.5f) * 2 * axisA + (percentAlongAxes.y - 0.5f)* 2* axisB;
                
                // used to just be pointOnUnitCube.normalized, however this gave seams. To make it better, we apply a nifty equation in the method PointOnCubeToPointOnSphere()
                Vector3 pointOnUnitSphere = PointOnCubeToPointOnSphere(pointOnUnitCube);
                
                // Create triangles within the mesh

                //get the current vertex position - now uses the ShapeGenerator object to determine shape
                vertices[i] = shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);
                
                //triangles within a square of 4 vertices (6 vertices as 2 overlap): 
                //i, i + resolution + 1, i + resolution
                // i, i + 1, i + resolution + 1

                // don't go beyond the mesh edge
                if (x != resolution - 1 && y != resolution - 1) {
                    //make the triangles
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;
                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
               
                i++;
                

            }
        }

        // assign the stuff we've generated to the mesh object
        mesh.Clear();               // remove existing data to prevent errors
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        
        if (mesh.uv.Length == uv.Length) {
            mesh.uv = uv;
        }
        

    }


    public void UpdateUVs(ColourGenerator colourGenerator) {
        Vector2[] uv = new Vector2[resolution * resolution];

        // find all points on the unit sphere
        int i = 0;
         for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                
                //what vertex we're on
                i = x + y * resolution;
                
                // how close to complete each loop is
                Vector2 percentAlongAxes = new Vector2(x,y) / (resolution - 1);
                
                // how far along each axis we are, between -1 and +1 (including y);
                Vector3 pointOnUnitCube = localUp + (percentAlongAxes.x - 0.5f) * 2 * axisA + (percentAlongAxes.y - 0.5f)* 2* axisB;

                // used to just be pointOnUnitCube.normalized, however this gave seams. To make it better, we apply a nifty equation in the method PointOnCubeToPointOnSphere()
                Vector3 pointOnUnitSphere = PointOnCubeToPointOnSphere(pointOnUnitCube);

                // Each UV is a Vector2 reference to a position on the Texture, so this lines up exactly with PointToCoordinate().ToUV() which we were using for shape generation
                uv[i] = GeoMaths.PointToCoordinate(pointOnUnitSphere).ToUV();

            }
         } 
        mesh.uv = uv;
    }

    // thanks Sebastian Lague, and in turn Mathproofs.blogspot.com
    // p is the point on the Cube
    public static Vector3 PointOnCubeToPointOnSphere(Vector3 p) {
        // square each dimension
        float x2 = p.x * p.x;
        float y2 = p.y * p.y;
        float z2 = p.z * p.z;

        // apply nifty equation
        float xout = p.x * Mathf.Sqrt(1 - (y2 + z2) / 2 + (y2 * z2) / 3);
        float yout = p.y * Mathf.Sqrt(1 - (z2 + x2) / 2 + (z2 * x2) / 3);
        float zout = p.z * Mathf.Sqrt(1 - (x2 + y2) / 2 + (x2 * y2) / 3);

        // return the new x, y and z coords as a Vector3
        return new Vector3(xout, yout, zout);
    }

}
