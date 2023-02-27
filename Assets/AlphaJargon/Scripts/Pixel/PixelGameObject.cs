using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BuildingBlocks.DataTypes;
using MoonSharp.Interpreter;

using System;
/*
C# is statically typed
but supports dynamic return type and parameter
Probably will break something in the future but its fine for now.
<requirements>
    dynamic = '.NET 4'
</requirements>
*/
namespace PixelGame
{
    [MoonSharpUserData]
    public class PixelGameObject : MonoBehaviour, IPixelObject
    {
        public InspectableDictionary<string,PixelComponent> PixelComponents{get; protected set;}
        void OnEnable()
        {
            PixelComponents = new InspectableDictionary<string, PixelComponent>(); 
            UserData.RegisterAssembly();

        }
        
        public dynamic this[string key] {
            get 
            {
                return PixelComponents[key];
            }
            set
            {
                PixelComponents.Add(key,value);
            }
        }

        public dynamic add(string key, dynamic value)
        {
            if(value is string)
            {
                try
                {
                    // WARNING:
                    // could be seen as messy and error prone code
                    // use a hashmap or enum instead (?)

                    // apparently you dont need the assembly (which in this case would be UnityEngine) 
                        //string assembly = $"PixelGame.{value}, UnityEngine";
                        // https://learn.microsoft.com/en-us/dotnet/api/system.type.gettype
                        // https://learn.microsoft.com/en-us/dotnet/api/system.type.assemblyqualifiedname

                    // assembly qualified name of the type, throws error, ignores case
                    System.Type newValue = System.Type.GetType($"PixelGame.{value}",true,true); 
                    return add(key,newValue,gameObject);
                }
                catch(Exception e)
                {
                    Debug.Log(e);
                    throw new MoonSharp.Interpreter.ScriptRuntimeException("Tried to add component that does not exist in namespace PixelGame. Check spelling.");
                }
            }
            return add(key,value,gameObject);
        }
        
        public dynamic add(string key, dynamic value, GameObject go)
        {
            dynamic newValue = go.AddComponent(value);
            if(newValue)
            {
                // FIXME: Bad place to put this
                if(newValue is PixelBehaviourScript) // if its a script
                    newValue.addPixelGameObjectToScriptGlobals(go.name,this); // Add the game object to the scripts globals
                PixelComponents.Add(key,newValue);
                if(newValue is ISpawnable)
                    newValue.Create(this);
                return newValue;
            }
            throw new MoonSharp.Interpreter.ScriptRuntimeException("Could Not Add Dynamic Value");
        }
    }
}