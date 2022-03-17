using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public static class CreateWorldText 
{
    public static TextMeshPro NewWorldText(Transform transform, Vector3 localPosition,int fontSize, string text, Color color, int sortOrder
        ,TMPro.TextAlignmentOptions textAlignment /*TMPro.TextContainerAnchors anchor*/) {
     
        GameObject gO = new GameObject("World Text", typeof(TextMeshPro));
        gO.transform.SetParent(transform);
        gO.transform.position = localPosition;
        TextMeshPro textMesh = gO.GetComponent<TextMeshPro>();
        textMesh.fontSize = fontSize;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortOrder;
        textMesh.alignment = textAlignment;
        //textMesh.TextContainerAnchors = anchor;
        textMesh.text = text;
        textMesh.color = color;
        return textMesh;
    }
}
