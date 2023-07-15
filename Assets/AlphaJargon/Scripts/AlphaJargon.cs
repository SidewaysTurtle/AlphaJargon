using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using BuildingBlocks.DataTypes;
using System.Text;

using MoonSharp.Interpreter;

using PixelGame;

[MoonSharpUserData]
public class AlphaJargon : MonoBehaviour, IPixelObject
{
    // Button Delegates
    public delegate void OnKeyDelegate(string KeyCode);
    public static OnKeyDelegate onKeyDownEvent, onKeyUpEvent, onKeyStayEvent;
    [HideInInspector] public AJState CurrAJState = AJState.PreSet;  
    [TextArea(15,20)]
    public string FileData;

    // Totality Execution Order
        // Awake, start, onenable and ondisable need to be native to the script
    public delegate void OnUpdateDelegate();
    public static OnUpdateDelegate onUpdateEvent;

    // Execution Order
    public delegate void TotalityExecutionOrder();
    public static TotalityExecutionOrder awakeGameEvent, initializeGameEvent, startGameEvent;

    // Managers
    PixelScreenManager PixelScreenManager;

    void OnEnable()
    {
        PixelScreenManager = gameObject.AddComponent<PixelScreenManager>();
    }

    void FixedUpdate()
    {
        // Input and FixedUpdate dont play nicely
        onUpdateEvent?.Invoke();
    }
    KeyCode currKeyDown;
    public void OnGUI()
    {
        if (Input.anyKeyDown)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                onKeyDownEvent?.Invoke(e.keyCode.ToString());
                currKeyDown = e.keyCode;
            }
        }
        if (Input.anyKey)
        {
            Event e = Event.current;
            if (e.isKey)
                onKeyStayEvent?.Invoke(e.keyCode.ToString());
        }

        if(Input.GetKeyUp(currKeyDown))
        {
            onKeyUpEvent?.Invoke(currKeyDown.ToString());
        }
    }
    [HideInInspector] public JargonCompiler Compiler;
    [HideInInspector] public AlhaJargonCodeEditor CodeEditor;
    public void Ready()
    {
        Compiler = gameObject.AddComponent<JargonCompiler>();
        CodeEditor = gameObject.AddComponent<AlhaJargonCodeEditor>(); 
        Compiler.Init(this);
        Compiler.add(FileData);
        CurrAJState = AJState.Set;
    }

    public void Set()
    {
        CurrAJState = AJState.Running;
        Compiler.RunScript();
        // StartCoroutine(RunInGameScripts());
    }
    public void Go()
    {
        AwakeGame();
        InitializeGame();
        StartCoroutine(StartGame());
    }
    

    public void AwakeGame()
    {
        awakeGameEvent?.Invoke();
    }

    public void InitializeGame()
    {
        initializeGameEvent?.Invoke();
    }

    public IEnumerator StartGame()
    {
        yield return null;
        startGameEvent?.Invoke();
    }
    // Obsolete, ingame code is now controlled with events
    // public IEnumerator RunInGameScripts()
    // {
    //     yield return null;
    //     foreach(PixelGameObject pgo in PixelGameObjects.Values)
    //         foreach(PixelComponent comp in pgo.PixelComponents.Values)
    //             if(comp is PixelBehaviourScript)
    //             {
    //                 PixelBehaviourScript script = (PixelBehaviourScript)comp;
    //                 script.RunScript();
    //             }
    // }

    // public Image Skybox;
    public InspectableDictionary<string,PixelGameObject> PixelGameObjects = new InspectableDictionary<string, PixelGameObject>();

    public PixelGameObject this[string key] {
        get 
        {
            return PixelGameObjects[key];
        }
        set
        {
            PixelGameObjects[key] = value;
        }
    }

    public PixelGameObject add(string key)
    {
        return add(key,transform);
    }
    // add components to gameobjects
    public PixelGameObject add(string key, Transform parentTransform)
    {
        if(!PixelGameObjects.Keys.Contains(key))  
        {
            PixelGameObject value = Instantiate<PixelGameObject>(Resources.Load<PixelGameObject>("Prefabs/Game/PixelGameObject"), parentTransform);
            value.name = key;
            Compiler.addPixelGameObjectToJargonScriptGlobals(key,value);
            CodeEditor.addPixelGameObjectToJargonScriptGlobals(key,value);
            PixelGameObjects.Add(key, value);
            return value;
        }
        throw new ScriptRuntimeException("Key already used to make PixelGameObject");
    }

/*****************************************************/

//     public string SpriteStringMaker(DynValue SpriteString)
//     {
//         // https://github.com/moonsharp-devs/moonsharp/blob/master/src/MoonSharp.Interpreter/Interop/Converters/TableConversions.cs
//         // The level of abstraction in their code makes me want to commit sepukku
//         // layers and layers of fucking private internal
//         return SpriteStringMaker((Dictionary<PixelPosition,char>) SpriteString.ToObject(typeof(Dictionary<PixelPosition,char>)));
//     }
//     public string SpriteStringMaker(Dictionary<PixelPosition,char> SpriteString)
//     {
//         StringBuilder Default = new StringBuilder("oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo");
//         foreach(PixelPosition key in SpriteString.Keys)
//         {
//             Default[(int)((key.x * 8) + key.y)] = SpriteString[key];
//         }
//         return Default.ToString();
//     }
}

public enum AJState
{
    PreSet,
    Set,
    Running
}