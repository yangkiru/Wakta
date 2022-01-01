using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TwitchChat : MonoBehaviour
{
    // might want to use these while testing with your own information

    public static TwitchChat Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TwitchChat();
            }

            return _instance;
        }
    }

    private static TwitchChat _instance;

    private CommandCollection _commands;
    private TcpClient _twitchClient;
    private StreamReader _reader;
    private StreamWriter _writer;
    private int timer = 0;

    private TwitchCredentials credentials = new TwitchCredentials
    {
        ChannelName = "woowakgood",
        Username = "woowakgood",
        Password = "oauth:rh47hlslk2bns0om844zngzaq9dv0m"
    };
    
    void Awake()
    {
        _instance = this;
        Connect(credentials, new CommandCollection());
    }

    void Update()
    {
        if (_twitchClient != null && _twitchClient.Connected)
        {
            ReadChat();
        }
        else if (++timer > 60)
        {
            Connect(credentials, new CommandCollection());
        }
    }

    public void SetNewCommandCollection(CommandCollection commands)
    {
        _commands = commands;
    }

    public void Connect(TwitchCredentials credentials, CommandCollection commands)
    {
        _commands = commands;
        _twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        _reader = new StreamReader(_twitchClient.GetStream());
        _writer = new StreamWriter(_twitchClient.GetStream());

        _writer.WriteLine("PASS " + credentials.Password);
        _writer.WriteLine("NICK " + credentials.Username);
        _writer.WriteLine("USER " + credentials.Username + " 8 * :" + credentials.Username);
        _writer.WriteLine("JOIN #" + credentials.ChannelName);
        _writer.Flush();
    }

    private void ReadChat()
    {
        if (_twitchClient.Available > 0)
        {
            string message = _reader.ReadLine();
            //Debug.Log(message);

            // Twitch sends a PING message every 5 minutes or so. We MUST respond back with PONG or we will be disconnected 
            if (message.Contains("PING"))
            {
                _writer.WriteLine("PONG");
                _writer.Flush();
                return;
            }

            if (message.Contains("PRIVMSG"))
            {
                var splitPoint = message.IndexOf("!", 1);
                var author = message.Substring(0, splitPoint);
                author = author.Substring(1);

                // users message
                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);

                if (message.StartsWith(TwitchCommands.CmdPrefix))
                {
                    // get the first word
                    int index = message.IndexOf(" ");
                    string command = index > -1 ? message.Substring(0, index) : message;
                    _commands.ExecuteCommand(
                        command,
                        new TwitchCommandData
                        {
                            Author = author,
                            Message = message
                        });

                    return;
                }

                Panzee panzee = null;
                PanzeeManager.Instance.panzeeDict.TryGetValue(author, out panzee);

                if (panzee == null)
                {
                    // Fail Because It's Full
                    if (PanzeeManager.Instance.panzeeDict.Count >= PanzeeManager.Instance.maxPanzee)
                        return;
                    else {
                        // Join
                        bool isBan = false;
                        PanzeeManager.Instance.banDict.TryGetValue(author, out isBan);
                        if (!isBan)
                            PanzeeManager.Instance.SpawnPanzee(author, message);
                        
                    }
                }
                else
                    panzee.SetText(message);

                return;
            }

            if (message.Contains("Invalid NICK"))
            {
                Debug.Log("Error: Invalid NICK");
                _twitchClient.Close();
                _twitchClient = null;
            }
        }
    }
}

    

