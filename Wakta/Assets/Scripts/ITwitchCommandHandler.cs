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
    public static readonly string CmdSuicide = "퇴장";
    public static readonly string CmdBan = "입장";
}



// !a command
public class TwitchLeftCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
        {
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdLeft).Length).TrimStart(' '), out panzee.cmdTimer);
            if (!success) panzee.cmdTimer = 9999;
            panzee.SetCommand(Panzee.Command.Left);
        }
    }
}

// !A command
public class TwitchLeftRunCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
        {
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdLeftRun).Length).TrimStart(' '), out panzee.cmdTimer);
            if (!success) panzee.cmdTimer = 9999;
            panzee.SetCommand(Panzee.Command.LeftRun);
        }
    }
}

// !d command
public class TwitchRightCommand : ITwitchCommandHandler {
    public void HandleCommand(TwitchCommandData data) {
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null) {
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdRight).Length).TrimStart(' '), out panzee.cmdTimer);
            if (!success) panzee.cmdTimer = 9999;
            panzee.SetCommand(Panzee.Command.Right);
        }
    }
}

// !D command
public class TwitchRightRunCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null)
        {
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdRightRun).Length).TrimStart(' '), out panzee.cmdTimer);
            if (!success) panzee.cmdTimer = 9999;
            panzee.SetCommand(Panzee.Command.RightRun);
        }
    }
}

// !w command
public class TwitchJumpCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data)
    {
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null) {
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdJump).Length).TrimStart(' '), out panzee.jumpTimer);
            if (!success) panzee.jumpTimer = 0;
            panzee.jumpTimerSet = 9999;
            panzee.SetCommand(Panzee.Command.Jump);
        }
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
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdJumpAuto).Length).TrimStart(' '), out panzee.jumpTimerSet);
            if (!success) panzee.jumpTimerSet = 0.5f;
            panzee.jumpTimer = 0;
            panzee.SetCommand(Panzee.Command.JumpAuto);
        }
    }
}

// !s command
public class TwitchStopCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);

        if (panzee != null) {
            panzee.SetCommand(Panzee.Command.Stop);
        }
    }
}

public class TwitchLeftJumpCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null) {
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdLeftJump).Length).TrimStart(' '), out panzee.cmdTimer);
            if (!success) panzee.cmdTimer = 9999;
            panzee.jumpTimer = 0;
            panzee.jumpTimerSet = 9999;
            panzee.SetCommand(Panzee.Command.LeftJump);
        }
    }
}

public class TwitchLeftJumpRunCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null) {
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdLeftJumpRun).Length).TrimStart(' '), out panzee.cmdTimer);
            if (!success) panzee.cmdTimer = 9999;
            panzee.jumpTimer = 0;
            panzee.jumpTimerSet = 9999;
            panzee.SetCommand(Panzee.Command.LeftJumpRun);
        }
    }
}

public class TwitchRightJumpCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null) {
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdRightJump).Length).TrimStart(' '), out panzee.cmdTimer);
            if (!success) panzee.cmdTimer = 9999;
            panzee.jumpTimer = 0;
            panzee.jumpTimerSet = 9999;
            panzee.SetCommand(Panzee.Command.RightJump);
        }
    }
}

public class TwitchRightJumpRunCommand : ITwitchCommandHandler
{
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null) {
            bool success = float.TryParse(data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdRightJumpRun).Length).TrimStart(' '), out panzee.cmdTimer);
            if (!success) panzee.cmdTimer = 9999;
            panzee.jumpTimer = 0;
            panzee.jumpTimerSet = 9999;
            panzee.SetCommand(Panzee.Command.RightJumpRun);
        }
    }
}

public class TwitchSuicideCommand : ITwitchCommandHandler {
    public void HandleCommand(TwitchCommandData data){
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (panzee != null) {
            string lastWord = data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdSuicide).Length).TrimStart(' ');
            panzee.Suicide(lastWord);
        }
    }
}

public class TwitchBanCommand : ITwitchCommandHandler {
    public void HandleCommand(TwitchCommandData data) {
        if (!PanzeeManager.Instance.IsSpawnable) return;
        Panzee panzee = null;
        PanzeeManager.Instance.panzeeDict.TryGetValue(data.Author, out panzee);
        if (data.Author.Equals(PanzeeManager.devName)) {
            if(panzee == null)
                PanzeeManager.Instance.SpawnPanzee(data.Author, "!입장");
            return;
        }
        if (panzee != null) {

            string lastWord = data.Message.Substring(0 + (TwitchCommands.CmdPrefix + TwitchCommands.CmdSuicide).Length).TrimStart(' ');
            panzee.Suicide("!입장을 쳐서 밴당했습니다");
        }
        PanzeeManager.Instance.banDict.Add(data.Author, true);
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
        _commands.Add(TwitchCommands.CmdSuicide, new TwitchSuicideCommand());
        _commands.Add(TwitchCommands.CmdBan, new TwitchBanCommand());
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


