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

        public dynamic this[System.Type type] {
            get 
            {
                foreach(PixelComponent value in PixelComponents.Values)
                {
                    // https://stackoverflow.com/questions/983030/type-checking-typeof-gettype-or-is
                    if(value.GetType() == type)
                    {
                        return value;
                    }
                }
                return null;
            }
            private set{}
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
                newValue = (PixelComponent)go.AddComponent(System.Type.GetType($"PixelGame.{value}",true,true)); //BAD UPCASTING
            }
            catch(Exception e)
            {
                Debug.Log(e);
                throw new MoonSharp.Interpreter.ScriptRuntimeException("Tried to add component that does not exist in namespace PixelGame. Check spelling and capitalization.");
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