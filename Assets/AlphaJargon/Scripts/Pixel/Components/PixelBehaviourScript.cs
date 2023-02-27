using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

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
        string FileData;
        Script script = new Script();
        ScriptFunctionDelegate onKeyDown, onKeyUp;
        ScriptFunctionDelegate onUpdate, onStart;

        // public ScriptFunctionDelegate's to use as the pixel components
        ScriptFunctionDelegate onTriggerEnter, onTriggerStay, onTriggerExit;
        ScriptFunctionDelegate onCollisionEnter, onCollisionStay, onCollisionExit;
        
        void OnEnable()
        {
            JargonEngine.onKeyDownEvent += KeyDown;

            JargonEngine.onUpdateEvent += OnUpdateEventHandler;

            PixelCollider.onTriggerEnter += TriggerEnter;
            PixelCollider.onTriggerStay += TriggerStay;
            PixelCollider.onTriggerExit += TriggerExit;

            PixelCollider.onCollisionEnter += CollisionEnter;
            PixelCollider.onCollisionStay += CollisionStay;
            PixelCollider.onCollisionExit += CollisionExit;
        }

        void OnDisable()
        {
            JargonEngine.onKeyDownEvent -= KeyDown;
            
            JargonEngine.onUpdateEvent -= OnUpdateEventHandler;

            PixelCollider.onTriggerEnter -= TriggerEnter;
            PixelCollider.onTriggerStay -= TriggerStay;
            PixelCollider.onTriggerExit -= TriggerExit;

            PixelCollider.onCollisionEnter -= CollisionEnter;
            PixelCollider.onCollisionStay -= CollisionStay;
            PixelCollider.onCollisionExit -= CollisionExit;
        }

        public void add(string FileData)
        {
            // FileData = System.Text.RegularExpressions.Regex.Replace(FileData, @"\t", "");
            // this.FileData = FileData.Replace("[[", "").Replace("]]", "");
        }
        public void addPixelGameObjectToScriptGlobals(string key, IPixelObject value)
        {
            UserData.RegisterAssembly();
            script.Globals[key] = value;
        }
        public override void Create(PixelGameObject parent)
        {
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

            onStart = script.Globals.Get("Start") != DynValue.Nil ? script.Globals.Get("Start").Function.GetDelegate() : null;

            onKeyDown = script.Globals.Get("KeyDown") != DynValue.Nil ? script.Globals.Get("KeyDown").Function.GetDelegate() : null;

            onCollisionEnter = script.Globals.Get("OnCollisionEnter") != DynValue.Nil ? script.Globals.Get("OnCollisionEnter").Function.GetDelegate() : null;
            onCollisionStay = script.Globals.Get("OnCollisionStay") != DynValue.Nil ? script.Globals.Get("OnCollisionStay").Function.GetDelegate() : null;
            onCollisionExit = script.Globals.Get("OnCollisionExit") != DynValue.Nil ? script.Globals.Get("OnCollisionExit").Function.GetDelegate() : null;


            onTriggerEnter = script.Globals.Get("OnTriggerEnter") != DynValue.Nil ? script.Globals.Get("OnTriggerEnter").Function.GetDelegate() : null;
            onTriggerStay = script.Globals.Get("OnTriggerStay") != DynValue.Nil ? script.Globals.Get("OnTriggerStay").Function.GetDelegate() : null;
            onTriggerExit = script.Globals.Get("OnTriggerExit") != DynValue.Nil ? script.Globals.Get("OnTriggerExit").Function.GetDelegate() : null;
            
            
            // onAwake
            onStart?.Invoke();
            onUpdate = script.Globals.Get("Update") != DynValue.Nil ? script.Globals.Get("Update").Function.GetDelegate() : null;
        }

        // all event handlers that invoke script delegate
        private void OnUpdateEventHandler()
        {
            onUpdate?.Invoke();
        }

        // Key up and down
        private void KeyDown(string KeyCode)
        {
            onKeyDown?.Invoke(KeyCode);
        }
        //
        private void TriggerEnter(Collider2D other, PixelGameObject parent)
        {
            onTriggerEnter?.Invoke(DynValue.NewString(parent.name));
        }
        private void TriggerStay(Collider2D other, PixelGameObject parent)
        {
            onTriggerStay?.Invoke(DynValue.NewString(parent.name));
        }
        private void TriggerExit(Collider2D other, PixelGameObject parent)
        {
            onTriggerExit?.Invoke(DynValue.NewString(parent.name));
        }
        //
        private void CollisionEnter(Collision2D other, PixelGameObject parent)
        {
            onCollisionEnter?.Invoke(DynValue.NewString(parent.name));
        } 
        private void CollisionStay(Collision2D other, PixelGameObject parent)
        {
            onCollisionStay?.Invoke(DynValue.NewString(parent.name));
        }
        private void CollisionExit(Collision2D other, PixelGameObject parent)
        {
            onCollisionExit?.Invoke(DynValue.NewString(parent.name));
        }
    }
}