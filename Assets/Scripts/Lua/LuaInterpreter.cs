using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

public class LuaInterpreter
{
    private DynValue _c = null;
    private Script _interpreter;
    private Dictionary<string, LuaInterpreterHandlerBase> _handlers = new Dictionary<string, LuaInterpreterHandlerBase>();

    public LuaInterpreter(string script)
    {
        _interpreter = new Script();
        UserData.RegisterAssembly();
        _interpreter.DoString(script);
    }

    public void AddHandler(string key, LuaInterpreterHandlerBase handler)
    {
        _interpreter.Globals[key] = handler;
        _handlers.Add(key, handler);
    }

    public bool HasNextScript()
    {
        return _c == null || _c.Coroutine.State != CoroutineState.Dead;
    }

    public void Resume()
    {
        if (_c == null)
        {
            DynValue func = _interpreter.Globals.Get("main");
            _c = _interpreter.CreateCoroutine(func);
        }
        _c.Coroutine.Resume();
    }

    public IEnumerator WaitCoroutine()
    {
        foreach (var handler in _handlers.Values)
        {
            yield return handler.WaitCoroutine();
        }
    }
}