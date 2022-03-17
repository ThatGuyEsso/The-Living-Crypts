using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugCommandBase
{
    private string _commnadID;
    private string _commandDescription;
    private string _commandFormat;

    public string CommandID { get { return _commnadID; } }
    public string CommandDesc { get { return _commandDescription; } }
    public string CommandFormat { get { return _commandFormat; } }
    
    public DebugCommandBase(string ID, string Desc, string Format)
    {
        _commnadID = ID;
        _commandDescription = Desc;
        _commandFormat = Format;

    }

}
public class DebugCommand : DebugCommandBase
{
    private Action command;
    public DebugCommand(string ID, string Desc, string Format, Action command) : base(ID, Desc, Format)
    {
        this.command = command;
    }

    public void CallCommand()
    {
        command.Invoke();
    }
}
