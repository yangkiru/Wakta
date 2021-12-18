using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITwitchCommandHandler
{
    void HandleCommand(TwitchCommandData data);
}

public struct TwitchCommandData {
    public string Author;
    public string Message;
}

public struct TwitchCredentials {
    public string ChannelName;
    public string Username;
    public string Password;
}

public static class TwitchCommands {
    public static readonly string CmdPrefix = "!";
    public static readonly string CmdLeft = "a";
    public static readonly string CmdLeftRun = "A";
    public static readonly string CmdRight = "d";
    public static readonly string CmdRightRun = "D";
    public static readonly string CmdJump = "w";
    public static readonly string CmdJumpAuto = "W";
    public static readonly string CmdStop = "s";
    public static readonly string CmdStopSemi = "S";
    public static readonly string CmdLeftJump = "q";
    public static readonly string CmdLeftJumpRun = "Q";
    public static readonly string CmdRightJump = "e";
    public static readonly string CmdRightJumpRun = "E";
}

/*EXAMPLES - This is how I would impletement this interface and create classes with actual command logic

!message command
public class TwitchDisplayMessageCommand : ITwitchCommandHandler {
    public void HandleCommand(TwitchCommandData data){
        Debug.Log($"<color=cyan>Raw Message:{data.Message}</color>");

        // strip the !message command from the message and trim the leading whitespace
        string actualMessage = data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdMessage).Length).TrimStart(' ');
        Debug.Log($"<color=cyan>{data.Author} says {actualMessage}</color>");
    }
}*/

// !a command
public class TwitchLeftCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
            panzee.SetCommand(Panzee.Command.Left);
    }
}

// !A command
public class TwitchLeftRunCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
            panzee.SetCommand(Panzee.Command.LeftRun);
    }
}

// !d command
public class TwitchRightCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
            panzee.SetCommand(Panzee.Command.Right);
    }
}

// !D command
public class TwitchRightRunCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
            panzee.SetCommand(Panzee.Command.RightRun);
    }
}

// !w command
public class TwitchJumpCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data)
    {
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
            panzee.SetCommand(Panzee.Command.Jump);
    }
}

// !W command
public class TwitchJumpAutoCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
        {
            panzee.isJumpAuto = true;
            panzee.jumpTimer = 0;
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdJumpAuto).Length).TrimStart(' '), out panzee.jumpTimerSet);
            if (!success) panzee.jumpTimerSet = 0.5f;
        }
    }
}

// !s command
public class TwitchStopCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
            panzee.SetCommand(Panzee.Command.Stop);
    }
}

public class TwitchLeftJumpCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
            panzee.SetCommand(Panzee.Command.LeftJump);
    }
}

public class TwitchLeftJumpRunCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
            panzee.SetCommand(Panzee.Command.LeftJumpRun);
    }
}

public class TwitchRightJumpCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
            panzee.SetCommand(Panzee.Command.RightJump);
    }
}

public class TwitchRightJumpRunCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
            panzee.SetCommand(Panzee.Command.RightJumpRun);
    }
}

public class CommandCollection {

    private Dictionary<string, ITwitchCommandHandler> _commands;

    public CommandCollection(){
        _commands = new Dictionary<string, ITwitchCommandHandler>();
        _commands.Add(TwitchCommands.CmdLeft, new TwitchLeftCommand());
        _commands.Add(TwitchCommands.CmdLeftRun, new TwitchLeftRunCommand());
        _commands.Add(TwitchCommands.CmdRight, new TwitchRightCommand());
        _commands.Add(TwitchCommands.CmdRightRun, new TwitchRightRunCommand());
        _commands.Add(TwitchCommands.CmdJump, new TwitchJumpCommand());
        _commands.Add(TwitchCommands.CmdJumpAuto, new TwitchJumpAutoCommand());
        TwitchStopCommand stopCommand = new TwitchStopCommand();
        _commands.Add(TwitchCommands.CmdStop, stopCommand);
        _commands.Add(TwitchCommands.CmdStopSemi, stopCommand);
        _commands.Add(TwitchCommands.CmdLeftJump, new TwitchLeftJumpCommand());
        _commands.Add(TwitchCommands.CmdLeftJumpRun, new TwitchLeftJumpRunCommand());
        _commands.Add(TwitchCommands.CmdRightJump, new TwitchRightJumpCommand());
        _commands.Add(TwitchCommands.CmdRightJumpRun, new TwitchRightJumpRunCommand());
    }

    public bool HasCommand(string command){
        return _commands.ContainsKey(command) ? true : false;
    }

    public void ExecuteCommand(string command, TwitchCommandData data){
        command = command.Substring(1); // remove exclamation point
        if(HasCommand(command)){
            _commands[command].HandleCommand(data);
        }
    }
}


