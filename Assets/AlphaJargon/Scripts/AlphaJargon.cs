using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[MoonSharp.Interpreter.MoonSharpUserData]
public class AlphaJargon : JargonEngine
{
    [HideInInspector] public JargonCompiler Compiler;
    void Awake()
    {
        Compiler = gameObject.AddComponent<JargonCompiler>();
        Compiler.Init(game);

        Compiler.FileData = "";
    }
    public override void AwakeGame()
    {
        awakeGameEvent?.Invoke();
    }

    public override void InitializeGame()
    {
        initializeGameEvent?.Invoke();
    }

    public override void StartGame()
    {
        startGameEvent?.Invoke();
    }
}
