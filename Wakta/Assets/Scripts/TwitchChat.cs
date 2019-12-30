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

	public TextMeshProUGUI developerText;

	public bool isUseFile = false;
	private bool toggle = true;

	public string filePath = "Assets/Resources/twitch_setting.txt";
	public string username, password, channelName;//https://twitchapps.com/tmi/
    void Start()
    {
		if (isUseFile) {
			StreamReader reader = new StreamReader(filePath);
			username = reader.ReadLine();
			password = reader.ReadLine();
			channelName = reader.ReadLine();
		}
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
        if(twitchClient.Available > 0) {
            var message = reader.ReadLine();
            if(message.Contains("PRIVMSG")) {
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(1, splitPoint-1);
				if (chatName.CompareTo("yangkiru") == 0) {
					splitPoint = message.IndexOf(":", 1);
					var temp = message.Substring(splitPoint + 1);
					if (temp[0].CompareTo('\'') == 0) {
						developerText.text = temp.Substring(1);
						Debug.Log(developerText.text);
						developerText.canvasRenderer.SetAlpha(1);
						developerText.CrossFadeAlpha(0, 5, false);
					} else if (temp[0].CompareTo(';') == 0) {
						temp = temp.Substring(1);
						string[] command = temp.Split(' ');
						switch(command[0]) {
							case "scene":
								command[1]
								break;
						}
					}
				}

                Panzee panzee = null;
                PanzeeManager.Instance.panzeeDict.TryGetValue(chatName, out panzee);

                if (panzee == null && PanzeeManager.Instance.panzeeDict.Count >= PanzeeManager.Instance.maxPanzee) {
                    return;
                }

                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);

                if (panzee != null && message[0].CompareTo('!') == 0) {
                    char command = message[1];
                    message = message.Substring(2);
                    switch (command) {
                        case 'd':
                            panzee.SetCommand(Panzee.Command.Right); break;
						case 'D':
							panzee.SetCommand(Panzee.Command.RightDash); break;
						case 'a':
                            panzee.SetCommand(Panzee.Command.Left); break;
						case 'A':
							panzee.SetCommand(Panzee.Command.LeftDash); break;
						case 'w':
                            panzee.SetCommand(Panzee.Command.Jump); break;
						case 'W':
							panzee.SetCommand(Panzee.Command.SuperJump); break;
						case 's':
                            panzee.SetCommand(Panzee.Command.Wait); break;
                        default:
							message = string.Format("{0}은/는 명령어를 칠 줄 몰라요!", chatName); break;
                    }
                    if(message.CompareTo(string.Empty) != 0)
                        panzee.SetText(message);
                } else if (panzee != null)
                    panzee.SetText(message);
                else
                    Join(chatName, message);
            }
        }
    }

    private void Join(string chatName, string message) {
        PanzeeManager.Instance.SpawnPanzee(chatName, message);
    }
}
