using UnityEngine;
using UnityEngine.Rendering;

public class GridOverlay : MonoBehaviour
{
    public float gridSizeX;
    public float gridSizeY;
    private readonly int gridSizeZ = 0;

    private Material lineMaterial;

    public Color mainColor = new Color( 0f, 1f, 0f, 1f );
    public bool show = true;

    public float startX;
    public float startY;
    private readonly float startZ = 0;

    public float step = 1.0f;

    private void CreateLineMaterial()
    {
        if ( !lineMaterial )
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find( "Hidden/Internal-Colored" );
            lineMaterial = new Material( shader );
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt( "_SrcBlend", ( int )BlendMode.SrcAlpha );
            lineMaterial.SetInt( "_DstBlend", ( int )BlendMode.OneMinusSrcAlpha );
            // Turn backface culling off
            lineMaterial.SetInt( "_Cull", ( int )CullMode.Off );
            // Turn off depth writes
            lineMaterial.SetInt( "_ZWrite", 0 );
        }
    }

    private void OnPostRender()
    {
        CreateLineMaterial();
        // set the current material
        lineMaterial.SetPass( 0 );

        GL.Begin( GL.LINES );

        if ( step == 0 )
            Debug.Log( "Step is 0, wont draw Grid. Step it to higher than 0." );

        if ( show )
        {
            GL.Color( mainColor );

            //Layers
            for ( float j = 0; j <= gridSizeY; j += step )
            {
                //X axis lines
                for ( float i = 0; i <= gridSizeZ; i += step )
                {
                    GL.Vertex3( startX, startY + j, startZ + i );
                    GL.Vertex3( startX + gridSizeX, startY + j, startZ + i );
                }

                //Z axis lines
                for ( float i = 0; i <= gridSizeX; i += step )
                {
                    GL.Vertex3( startX + i, startY + j, startZ );
                    GL.Vertex3( startX + i, startY + j, startZ + gridSizeZ );
                }
            }

            //Y axis lines
            for ( float i = 0; i <= gridSizeZ; i += step )
            {
                for ( float k = 0; k <= gridSizeX; k += step )
                {
                    GL.Vertex3( startX + k, startY, startZ + i );
                    GL.Vertex3( startX + k, startY + gridSizeY, startZ + i );
                }
            }
        }

        GL.End();
    }
}