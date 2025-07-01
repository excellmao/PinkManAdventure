using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    static Dictionary<string, List<Action<object[]>>> _listAction = new Dictionary<string, List<Action<object[]>>>();

    public static void Addlistener(string nameAction, Action<object[]> callback)
    {
        if (!_listAction.ContainsKey(nameAction))
        {
            _listAction.Add(nameAction, new List<Action<object[]>>());
        }
        _listAction[nameAction].Add(callback);
    }
    public static void Removelistener(string nameAction, Action<object[]> callback)
    {
        if (!_listAction.ContainsKey(nameAction)) return;
        _listAction[nameAction].Remove(callback);
    }

    public static void Notify(string nameAction, params object[] datas)
    {
        if(!_listAction.ContainsKey(nameAction)) return;
        foreach (var action in _listAction[nameAction])
        {
            try
            {
                action?.Invoke(datas);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
