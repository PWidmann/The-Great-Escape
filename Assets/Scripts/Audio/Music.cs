using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Music", menuName = "Music")]
public class Music : ScriptableObject
{
    public AudioClip audioFile; 

    public string authorName;
    public string titleName;
    public string webUrl;
}
