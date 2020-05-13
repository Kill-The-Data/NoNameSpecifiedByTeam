using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;


public class UIAlignmentTool : EditorWindow
{
    private float axis;

    [SerializeField] private Texture2D AlignLeftIcon;
    [SerializeField] private Texture2D AlignVCIcon;
    [SerializeField] private Texture2D AlignRightIcon;
    [SerializeField] private Texture2D AlignTopIcon;
    [SerializeField] private Texture2D AlignHCIcon;
    [SerializeField] private Texture2D AlignBottomIcon;
    
    [MenuItem("Window/UIAlignmentHelper")]
    public static void ShowWindow()
    {
        GetWindow<UIAlignmentTool>().Show();
    }

    private void ColorTextureWhite(Texture2D tex)
    {
        List<Color> newPixels = new List<Color>();
        
        foreach(var pixl in tex.GetPixels())
        {
            newPixels.Add(pixl.a > 0.00001f ? Color.white : pixl);
        }
        tex.SetPixels(newPixels.ToArray());
        tex.Apply();
    }

    private void OnEnable()
    {
        ColorTextureWhite(AlignLeftIcon);
        ColorTextureWhite(AlignVCIcon);
        ColorTextureWhite(AlignRightIcon);
        ColorTextureWhite(AlignTopIcon);
        ColorTextureWhite(AlignHCIcon);
        ColorTextureWhite(AlignBottomIcon);
    }

    private void OnGUI()
    {
        minSize = new Vector2(10,10);
        if (GUILayout.Button(AlignLeftIcon))
        {
            SelectLeftAxis();
            AlignSelectionVerticalOnAxis();
        }

        if (GUILayout.Button(AlignVCIcon))
        {
            SelectCenterVAxis();
            AlignSelectionVerticalOnAxis();
        }
        
        if (GUILayout.Button(AlignRightIcon))
        {
            SelectRightAxis();
            AlignSelectionVerticalOnAxis();
        }
        if (GUILayout.Button(AlignTopIcon))
        {
            SelectTopAxis();
            AlignSelectionHorizontalOnAxis();
        }
        if (GUILayout.Button(AlignHCIcon))
        {
            SelectCenterHAxis();
            AlignSelectionHorizontalOnAxis();
        }
        if (GUILayout.Button(AlignBottomIcon))
        {
            SelectBottomAxis();
            AlignSelectionHorizontalOnAxis();
        }
    }

    private void SelectLeftAxis()
    {
        bool first = true;
        foreach (var tf in Selection.transforms)
        {
            if (first){
                axis = tf.position.x;
                first = false;
            }
            axis = Mathf.Min(axis, tf.position.x);
        }
    }
    
    private void SelectRightAxis()
    {
        bool first = true;
        foreach (var tf in Selection.transforms)
        {
            if (first){
                axis = tf.position.x;
                first = false;
            }
            axis = Mathf.Max(axis, tf.position.x);
        }
    }
    
    private void SelectTopAxis()
    {
        bool first = true;
        foreach (var tf in Selection.transforms)
        {
            if (first){
                axis = tf.position.y;
                first = false;
            }
            axis = Mathf.Max(axis, tf.position.y);
        }
    }
    
    private void SelectBottomAxis()
    {
        bool first = true;
        foreach (var tf in Selection.transforms)
        {
            if (first){
                axis = tf.position.y;
                first = false;
            }
            axis = Mathf.Min(axis, tf.position.y);
        }
    }
    private void SelectCenterVAxis()
    {
        int count = 0;
        axis = 0;
        foreach (var tf in Selection.transforms)
        {
            axis += tf.position.x;
            count++;
        }

        axis /= count;
    }

    private void SelectCenterHAxis()
    {
        int count = 0;
        axis = 0;
        foreach (var tf in Selection.transforms)
        {
            axis += tf.position.y;
            count++;
        }

        axis /= count;
    }
    
    private void AlignSelectionVerticalOnAxis()
    {
        foreach (var tf in Selection.transforms)
        {
            var tfPosition = tf.position;
            Undo.RecordObject(tf, "Align On Axis");
            tfPosition.x = axis;
            tf.position = tfPosition;
        }
    }
    private void AlignSelectionHorizontalOnAxis()
    {
        foreach (var tf in Selection.transforms)
        {
            var tfPosition = tf.position;
            Undo.RecordObject(tf, "Align On Axis");
            tfPosition.y = axis;
            tf.position = tfPosition;
        }
    }
}
