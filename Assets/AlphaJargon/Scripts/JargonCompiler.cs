using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

[MoonSharpUserData]
public class JargonCompiler : MonoBehaviour
{
    [HideInInspector] public string FileData;

    Script script = new Script();
    public ScriptFunctionDelegate onAwake, onInitialize, onStart;

/*****************************************************/
    void OnEnable()
    {
        JargonEngine.awakeGameEvent += AwakeGame;
        JargonEngine.initializeGameEvent += InitializeGame;
        JargonEngine.startGameEvent += StartGame;
    }

    void OnDisable()
    {
        JargonEngine.awakeGameEvent -= AwakeGame;
        JargonEngine.initializeGameEvent -= InitializeGame;
        JargonEngine.startGameEvent -= StartGame;
    }
/*****************************************************/
    public void Init(JargonEngine Jargon)
    {
        UserData.RegisterAssembly();
        script.Globals["jargon"] = Jargon;
    }

    public void add(string FileData)
    {
        this.FileData = FileData;
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
        ((ScriptLoaderBase)script.Options.ScriptLoader).ModulePaths = ScriptLoaderBase.UnpackStringPaths(System.IO.Path.Combine(Application.persistentDataPath,"/modules/","?") + ".lua");

        try
        {
            DynValue fn = script.LoadString(FileData);
            fn.Function.Call();
        }
        catch (ScriptRuntimeException e)
        {
            Debug.LogError(e.DecoratedMessage);
        }

        onAwake = script.Globals.Get("AwakeGame") != DynValue.Nil ? script.Globals.Get("AwakeGame").Function.GetDelegate() : null;
        onInitialize = script.Globals.Get("InitializeGame") != DynValue.Nil ? script.Globals.Get("InitializeGame").Function.GetDelegate() : null;
        onStart = script.Globals.Get("StartGame") != DynValue.Nil ? script.Globals.Get("StartGame").Function.GetDelegate() : null;
    }

/*****************************************************/

    // all event handlers that invoke script delegate
    private void AwakeGame()
    {
        onAwake?.Invoke();
    }
    private void InitializeGame()
    {
        onInitialize?.Invoke();
    }
    private void StartGame()
    {
        onStart?.Invoke();
    }
}
