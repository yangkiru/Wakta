using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;

public class TwitchChat : MonoBehaviour
{
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    private bool toggle = true;

    [TextArea]
    public string[] randomTalk;

    public string username, password, channelName;//https://twitchapps.com/tmi/
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
        //ReadChat();
        if (Input.GetKeyDown(KeyCode.Space))
            toggle = !toggle;
        if (toggle)
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
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                Panzee panzee = null;
                PanzeeManager.Instance.panzeeDict.TryGetValue(chatName, out panzee);

                if (panzee == null && PanzeeManager.Instance.panzeeDict.Count >= PanzeeManager.Instance.maxPanzee) {
                    return;
                }

                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);

                if (panzee != null && message[0].CompareTo('!') == 0) {
                    char command = char.ToLower(message[1]);
                    message = message.Substring(2);
                    bool isRandom = false;
                    switch (command) {
                        case 'd':
                            panzee.SetCommand(Panzee.Command.Right); break;
                        case 'a':
                            panzee.SetCommand(Panzee.Command.Left); break;
                        case 'w':
                            panzee.SetCommand(Panzee.Command.Jump); break;
                        case 's':
                            panzee.SetCommand(Panzee.Command.Wait); break;
                        default:
                            int random = UnityEngine.Random.Range(0, randomTalk.Length);
                            isRandom = true;
                            panzee.SetText(randomTalk[random]); break;
                    }
                    if(!isRandom && message.CompareTo(string.Empty) != 0)
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
