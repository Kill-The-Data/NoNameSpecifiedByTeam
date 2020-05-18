using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;


public class UIAlignmentTool : EditorWindow
{
    private float axis;

    //lots of nice icons
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

    //the items we use are sadly black in background, 
    //which clashes with pro skin quite badly, to fix 
    //this we simply change the color of the texture
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
        //recolor the buttons only for the pro-skin, otherwise you have light-
        //grey on white which isn't great either
        if(EditorGUIUtility.isProSkin)
        {
            ColorTextureWhite(AlignLeftIcon);
            ColorTextureWhite(AlignVCIcon);
            ColorTextureWhite(AlignRightIcon);
            ColorTextureWhite(AlignTopIcon);
            ColorTextureWhite(AlignHCIcon);
            ColorTextureWhite(AlignBottomIcon);
        }
        //we want out window to be quite a bit smaller than what unity allows on default
        minSize = new Vector2(10,10);
    }
    
    private void OnGUI()
    {
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

    //these are mostly the same so only
    //one of them is documented
    private void SelectLeftAxis()
    {
        bool first = true;
        
        //get all selected transforms
        foreach (var tf in Selection.transforms)
        {
            if (first){
                //make sure the axis is not just 0
                axis = tf.position.x;
                first = false;
            }
            
            //get the left-most(min) value and set it as the axis
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
    
    //same as above but for centering
    private void SelectCenterVAxis()
    {
        //reset the axis
        axis = 0;
        //get the center position 
        float[] xposes = Selection.transforms.Select(tf => tf.position.x).ToArray();
        axis = MathExtra.GetMedian(xposes);
    }

    private void SelectCenterHAxis()
    {
        axis = 0;
        
        float[] xposes = Selection.transforms.Select(tf => tf.position.y).ToArray();
        axis = MathExtra.GetMedian(xposes);
    }
    
    //align the selection on the vertical axis
    private void AlignSelectionVerticalOnAxis()
    {
        //iterate through all selected items
        foreach (var tf in Selection.transforms)
        {
            //change position.x and record the change so that CTRL+Z works
            var tfPosition = tf.position;
            Undo.RecordObject(tf, "Align On Axis");
            tfPosition.x = axis;
            tf.position = tfPosition;
        }
    }
    
    //align the selection the horizontal axis
    private void AlignSelectionHorizontalOnAxis()
    {
        //iterate through all selected items
        foreach (var tf in Selection.transforms)
        {
            //change position.y and record the change so that CTRL+Z works
            var tfPosition = tf.position;
            Undo.RecordObject(tf, "Align On Axis");
            tfPosition.y = axis;
            tf.position = tfPosition;
        }
    }
}
