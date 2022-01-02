using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;
using TMPro;

public class TwitchChat : MonoBehaviour
{
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

	public TextMeshProUGUI readedChat;
	private int readed = 0;

	public bool isUseFile = false;
	private bool toggle = true;

	public string username = "woowakgood";
	public string password = "oauth:prdj3v10p14d3tgxzqp1m72hygao9t";
	public string channelName = "woowakgood";//https://twitchapps.com/tmi/
    void Start()
    {
		Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if(!twitchClient.Connected) {
            Connect();
        }
		ReadChat();
    }

    private void Connect() {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());

        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();
    }

    private void ReadChat() {
	    if (twitchClient.Available > 0) {
		    readedChat.text = (++readed).ToString();
		    var message = reader.ReadLine();
		    if (message.Contains("PRIVMSG")) {
			    Panzee panzee = null;

			    var splitPoint = message.IndexOf("!", 1);
			    var chatName = message.Substring(1, splitPoint - 1);

			    PanzeeManager.Instance.panzeeDict.TryGetValue(chatName, out panzee);

			    splitPoint = message.IndexOf(":", 1);
			    message = message.Substring(splitPoint + 1);
			    
			    if (chatName.CompareTo("yangkiru") == 0) {
				    PanzeeManager.Instance.panzeeDict.TryGetValue(chatName, out panzee);
				    if (panzee == null) {
					    splitPoint = message.IndexOf(":", 1);
					    var temp = message.Substring(splitPoint + 1);
					    Join(chatName, temp);
				    }
			    }

			    if (panzee != null && message[0].CompareTo('!') == 0) {
				    char command = message[1];
				    message = message.Substring(2);
				    bool success;
				    switch (command) {
					    case 'd':
						    success = float.TryParse(message.TrimStart(' '), out panzee.cmdTimer);
						    if (!success) panzee.cmdTimer = 9999;
						    panzee.SetCommand(Panzee.Command.Right);
						    return;
					    case 'D':
						    success = float.TryParse(message.TrimStart(' '), out panzee.cmdTimer);
						    if (!success) panzee.cmdTimer = 9999;
						    panzee.SetCommand(Panzee.Command.RightRun);
						    return;
					    case 'a':
						    success = float.TryParse(message.TrimStart(' '), out panzee.cmdTimer);
						    if (!success) panzee.cmdTimer = 9999;
						    panzee.SetCommand(Panzee.Command.Left);
						    return;
					    case 'A':
						    success = float.TryParse(message.TrimStart(' '), out panzee.cmdTimer);
						    if (!success) panzee.cmdTimer = 9999;
						    panzee.SetCommand(Panzee.Command.LeftRun);
						    return;
					    case 'w':
						    success = float.TryParse(message.TrimStart(' '), out panzee.jumpTimer);
						    if (!success) panzee.jumpTimer = 0;
						    panzee.jumpTimerSet = 9999;
						    panzee.SetCommand(Panzee.Command.Jump);
						    return;
					    case 'W':
						    success = float.TryParse(message.TrimStart(' '), out panzee.jumpTimerSet);
						    if (!success) panzee.jumpTimerSet = 0.5f;
						    panzee.jumpTimer = 0;
						    panzee.SetCommand(Panzee.Command.JumpAuto);
						    return;
					    case 's':
					    case 'S':
						    panzee.SetCommand(Panzee.Command.Wait);
						    return;
					    case 'q':
						    success = float.TryParse(message.TrimStart(' '), out panzee.cmdTimer);
						    if (!success) panzee.cmdTimer = 9999;
						    panzee.jumpTimer = 0;
						    panzee.jumpTimerSet = 9999;
						    panzee.SetCommand(Panzee.Command.LeftJump);
						    return;
					    case 'Q':
						    success = float.TryParse(message.TrimStart(' '), out panzee.cmdTimer);
						    if (!success) panzee.cmdTimer = 9999;
						    panzee.jumpTimer = 0;
						    panzee.jumpTimerSet = 9999;
						    panzee.SetCommand(Panzee.Command.LeftJumpRun);
						    return;
					    case 'e':
						    success = float.TryParse(message.TrimStart(' '), out panzee.cmdTimer);
						    if (!success) panzee.cmdTimer = 9999;
						    panzee.jumpTimer = 0;
						    panzee.jumpTimerSet = 9999;
						    panzee.SetCommand(Panzee.Command.RightJump);
						    return;
					    case 'E':
						    success = float.TryParse(message.TrimStart(' '), out panzee.cmdTimer);
						    if (!success) panzee.cmdTimer = 9999;
						    panzee.jumpTimer = 0;
						    panzee.jumpTimerSet = 9999;
						    panzee.SetCommand(Panzee.Command.RightJumpRun);
						    return;
					    default:
						    message = string.Format("{0}은/는 명령어를 칠 줄 몰라요!", chatName);
						    break;
				    }

				    if (message.CompareTo(string.Empty) != 0)
					    panzee.SetText(message);
			    }
			    else if (panzee != null)
				    panzee.SetText(message);
			    else if (PanzeeManager.Instance.panzeeDict.Count - (PanzeeManager.Instance.panzeeArray[5] != null ? 1 : 0) < 5) {
				    Join(chatName, message);
			    }
		    }
	    }
    }

    private void Join(string chatName, string message) {
	    PanzeeManager.Instance.SpawnPanzee(chatName, message);
    }
}
