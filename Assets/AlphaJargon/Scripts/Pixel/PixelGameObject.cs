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
        public InspectableDictionary<string,PixelComponent> PixelComponents{get; private set;}
        public PixelPosition position = new PixelPosition(0,0);
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

        public dynamic add(string key, string value)
        {
            return add(key,value,gameObject);
        }
        
        public dynamic add(string key, string value, GameObject go)
        {
           PixelComponent newValue;
            try
            {
                newValue = (PixelComponent)go.AddComponent(System.Type.GetType($"PixelGame.{value}",true,true));
            }
            catch(Exception e)
            {
                Debug.Log(e);
                throw new MoonSharp.Interpreter.ScriptRuntimeException("Tried to add component that does not exist in namespace PixelGame. Check spelling.");
            }
            if(newValue)
            {
                // Add the game object to the scripts globals
                PixelComponents.Add(key,newValue);
                ((PixelComponent)newValue).Create(this);
                return newValue;
            }
            throw new MoonSharp.Interpreter.ScriptRuntimeException("Could Not Add Dynamic Value");
        }
    }
}