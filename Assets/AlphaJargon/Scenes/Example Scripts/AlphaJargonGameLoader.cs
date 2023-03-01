using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using InGameCodeEditor;

using PixelGame;
public class AlphaJargonGameLoader : MonoBehaviour
{
    public Transform GameTransform;

    public static AlphaJargonGameLoader Instance{private set; get;}
    private AlphaJargon _AlphaJargon;
    public AlphaJargon AlphaJargon
    {
        get
        {
            if(!_AlphaJargon)
            {
                // TODO add this to a instantiatble prefab
                AlphaJargon = (AlphaJargon)Instantiate(Resources.Load("AlphaJargon"),new Vector3(0,0,0),Quaternion.identity,GameTransform);
            }
            return _AlphaJargon;
        }
        private set
        {
            Destroy(AlphaJargon.gameObject);
        }
    }

    // UI STUFF
    public CodeEditor MyCodeEditor;
    public CodeEditor PrivateCodeEditor;
    public void CreateNewGame(string LevelCode, string LevelMyCodeEditor, string LevelPrivateCode)
    {
        MyCodeEditor.Text = LevelMyCodeEditor;
        PrivateCodeEditor.Text = "-- Scroll to see more\n" + LevelPrivateCode;
        HandleLuaFile(LevelCode);
    }

    private void HandleLuaFile(string text)
    {
        AlphaJargon.FileData = text;
        AlphaJargon.Ready();
        AlphaJargon.Set();
        AlphaJargon.Go();
    }

    // Button
    public void RunSelfCode()
    {
        AlphaJargon.Compiler.RunScript(MyCodeEditor.Text);
    }
}