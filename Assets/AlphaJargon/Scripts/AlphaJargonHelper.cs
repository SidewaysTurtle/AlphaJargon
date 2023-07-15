using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGameCodeEditor;
using PixelGame;

public class AlphaJargonHelper : MonoBehaviour
{
    public static AlphaJargonHelper Instance{private set; get;}
    private GameObject AJGO;
    [TextArea(15,20)]
    public string FileData;
    private AlphaJargon _AlphaJargon;    
    public AlphaJargon AlphaJargon
    {
        get
        {
            if(!_AlphaJargon)
            {
                AJGO = new GameObject("AlphaJargon");
                AJGO.transform.parent = transform;
                AJGO.transform.localPosition = new Vector3(0,0,0);
                _AlphaJargon = AJGO.AddComponent<AlphaJargon>();
            }
            return _AlphaJargon;
        }
        private set
        {
            Destroy(AJGO);
            gameObject.AddComponent<AlphaJargon>();
        }
    }

    private void HandleAlphaJargon(string text = "")
    {
        if (text != "")
            AlphaJargon.FileData = text;
        if(this.FileData != "")
            AlphaJargon.FileData = this.FileData;
        AlphaJargon.Ready();
        AlphaJargon.Set();
        AlphaJargon.Go();
    }

    void Start()
    {
        HandleAlphaJargon();
    }
}
