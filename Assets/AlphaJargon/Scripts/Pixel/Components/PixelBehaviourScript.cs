using System.Collections;

using System.Collections.Generic;
using UnityEngine;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using MoonSharp.Interpreter.Interop;

/*
script.Globals["test"] = new Action<string, MyEnum>(this.TestMethod);
```lua
test('hello world', MyEnum.Value1)
```
*/
namespace PixelGame
{
    [MoonSharp.Interpreter.MoonSharpUserData]
    public class PixelBehaviourScript : PixelComponent
    {
        public string FileData;
        public override PixelGameObject parent{get;set;}
        public Script script = new Script();
        ScriptFunctionDelegate onKeyDown, onKeyUp, onKeyStay;
        ScriptFunctionDelegate onUpdate, onEnable;

        // public ScriptFunctionDelegate's to use as the pixel components
        ScriptFunctionDelegate onTriggerEnter, onTriggerStay, onTriggerExit;
        ScriptFunctionDelegate onCollisionEnter, onCollisionStay, onCollisionExit;
        
        void OnEnable()
        {
            AlphaJargon.onKeyDownEvent += KeyDown;
            AlphaJargon.onKeyUpEvent += KeyUp;
            AlphaJargon.onKeyStayEvent += KeyStay;

            AlphaJargon.onUpdateEvent += () => onUpdate?.Invoke();

            PixelCollider.onTriggerEnterEvent += TriggerEnterEvent;
            PixelCollider.onCollisionEnterEvent  += CollisionEnterEvent;
        }

        void OnDisable()
        {
            AlphaJargon.onKeyDownEvent -= KeyDown;
            AlphaJargon.onKeyUpEvent -= KeyUp;
            AlphaJargon.onKeyStayEvent -= KeyStay;
            
            AlphaJargon.onUpdateEvent -= () => onUpdate?.Invoke();

            PixelCollider.onTriggerEnterEvent -= TriggerEnterEvent;
            PixelCollider.onCollisionEnterEvent  -= CollisionEnterEvent;
        }

        public void add(DynValue FileData)
        {
            string multiliteralString = FileData.ToString(); // Get the multiliteral string from the DynValue
            string normalString = multiliteralString.Substring(1, multiliteralString.Length - 2); // Remove the first and last quotes enclosing the string
            this.FileData = normalString;
        }
        public void addFile(DynValue FileData)
        {
            StartCoroutine(GetLuaFile(FileData.ToPrintString()));
        }
        private IEnumerator GetLuaFile(string filePath)
        {
            yield return LoadLuaFile.GetLuaFile(filePath, HandleLuaFile);
        }
        private void HandleLuaFile(string text)
        {
            this.FileData = text;
        }
        public override void Remove()
        {
            Destroy(this);
        }
        public void addPixelGameObjectToScriptGlobals(string key, IPixelObject value)
        {
            // Debug.Log($"key: {key} + value: {value}");
            UserData.RegisterAssembly();
            script.Globals[key] = value;
        }
        public void addAllPixelGameObjectToScriptGlobals(string key, IPixelObject value)
        {
            // Debug.Log($"key: {key} + value: {value}");
            UserData.RegisterAssembly();
            script.Globals[key] = value;
        }
        public override void Create(PixelGameObject parent)
        {
            this.parent = parent;
            addPixelGameObjectToScriptGlobals(parent.name,parent); 
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
            
            // adds a lot of the internal commands
            // script.Globals["internal"] = new Internal();

            DynValue fn = script.LoadString(FileData);
            fn.Function.Call();

            // cant do null checks cuz .Get returns DynValue.Nil not null
            // onStart = script.Globals.Get("Start").Function.GetDelegate() ?? null;

            onEnable = script.Globals.Get("Start") != DynValue.Nil ? script.Globals.Get("Start").Function.GetDelegate() : null;

            onKeyDown = script.Globals.Get("OnKeyDown") != DynValue.Nil ? script.Globals.Get("OnKeyDown").Function.GetDelegate(): null;
            onKeyUp = script.Globals.Get("OnKeyUp") != DynValue.Nil ? script.Globals.Get("OnKeyUp").Function.GetDelegate(): null;
            onKeyStay = script.Globals.Get("OnKeyStay") != DynValue.Nil ? script.Globals.Get("OnKeyStay").Function.GetDelegate(): null;

            onCollisionEnter = script.Globals.Get("OnCollisionEnter") != DynValue.Nil ? script.Globals.Get("OnCollisionEnter").Function.GetDelegate() : null;
            onCollisionStay = script.Globals.Get("OnCollisionStay") != DynValue.Nil ? script.Globals.Get("OnCollisionStay").Function.GetDelegate() : null;
            onCollisionExit = script.Globals.Get("OnCollisionExit") != DynValue.Nil ? script.Globals.Get("OnCollisionExit").Function.GetDelegate() : null;


            onTriggerEnter = script.Globals.Get("OnTriggerEnter") != DynValue.Nil ? script.Globals.Get("OnTriggerEnter").Function.GetDelegate() : null;
            onTriggerStay = script.Globals.Get("OnTriggerStay") != DynValue.Nil ? script.Globals.Get("OnTriggerStay").Function.GetDelegate() : null;
            onTriggerExit = script.Globals.Get("OnTriggerExit") != DynValue.Nil ? script.Globals.Get("OnTriggerExit").Function.GetDelegate() : null;
            
            
            // onAwake
            onEnable?.Invoke();
            onUpdate = script.Globals.Get("Update") != DynValue.Nil ? script.Globals.Get("Update").Function.GetDelegate() : null;
        }

        // Key up and down
        private void KeyDown(string KeyCode)
        {
            if(KeyCode != "None")
                onKeyDown?.Invoke(DynValue.NewString(KeyCode));
        }

        private void KeyUp(string KeyCode)
        {
            if(KeyCode != "None") // possibilty of this triggering is slim to none. But might as well.
                onKeyUp?.Invoke(DynValue.NewString(KeyCode));
        }
        private void KeyStay(string KeyCode)
        {
            if(KeyCode != "None") // possibilty of this triggering is slim to none. But might as well.
                onKeyStay?.Invoke(DynValue.NewString(KeyCode));
        }
        //
        private void TriggerEnterEvent(Pixel other, PixelGameObject parent)
        {
            onTriggerEnter?.Invoke(DynValue.NewString(parent.name));
        }
        //
        private void CollisionEnterEvent(Pixel other, PixelGameObject parent)
        {
            onCollisionEnter?.Invoke(DynValue.NewString(parent.name));
        } 
    }
}