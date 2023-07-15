using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

[MoonSharpUserData]
public class AlhaJargonCodeEditor : MonoBehaviour
{
    [HideInInspector] public string FileData;

    Script script = new Script();
    public void Init(AlphaJargon Jargon)
    {
        UserData.RegisterAssembly();
        script.Globals["jargon"] = Jargon;
    }

    public void add(string FileData)
    {
        this.FileData = FileData;
    }
    public void addPixelGameObjectToJargonScriptGlobals(string key, IPixelObject value)
    {
        UserData.RegisterAssembly();
        script.Globals[key] = value;
    }
    public void RunScript()
    {
        RunScript(this.FileData);
    }
    public void RunScript(string FileData)
    {
        UserData.RegisterAssembly();

        // sets default options
        script.Options.DebugPrint = (x) => {Debug.Log(x);};
        ((ScriptLoaderBase)script.Options.ScriptLoader).IgnoreLuaPathGlobal = true;
        // ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = ScriptLoaderBase.UnpackStringPaths(System.IO.Path.Combine(Application.persistentDataPath,"/modules/","?") + ".lua");
        try
        {
            DynValue fn = script.LoadString(FileData);
            fn.Function.Call();
        }
        catch (ScriptRuntimeException e)
        {
            Debug.LogError(e.DecoratedMessage);
        }
    }
}
