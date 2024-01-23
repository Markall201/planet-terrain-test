using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Custom Editor for Planets, to allow us to modify planet colour and shape within the Planet object

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;

    Editor shapeEditor;
    Editor colourEditor;

    //method to override the existing editor
    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            // if things have changed, generate planet
            if (check.changed) 
            {
                planet.GeneratePlanet();
            }
        }
        // manual control
        if (GUILayout.Button("Generate Planet")) {
            planet.GeneratePlanet();
        }
        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref planet.shapeSettingsFoldout, ref shapeEditor);
        DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdated, ref planet.colourSettingsFoldout, ref colourEditor);
    }



    //Method to draw custom editor
    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        

        if (settings != null)
        {

            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            using (var check = new EditorGUI.ChangeCheckScope())
            {

                // create editor
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);

                    //Display editor
                    editor.OnInspectorGUI();

                    if (check.changed) 
                    {
                        if (onSettingsUpdated != null)
                        onSettingsUpdated();
                    }
                }
            }
        }
        
    }


    private void OnEnable() 
    {
        // cast target object to Planet
        planet = (Planet)target;
    }

    

}
