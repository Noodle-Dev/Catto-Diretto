using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using System.Net.Sockets;
using System.IO;

public class TwitchConnect : MonoBehaviour
{
    public UnityEvent<string, string> OnChatMessage;
    TcpClient Twitch;
    StreamReader Reader;
    StreamWriter Writer;
    const string URL = "irc.chat.twitch.tv";
    const int PORT = 6667;
    string user = "";
    // get https://www.twitchapps.com/tmi/

    public string OAuth = "";
    string Channel = "";
    float PingCounter = 0f;

    [Header("UI Elements")]
    public GameObject placeholder;
    public GameObject placeholderUser;
    public GameObject placeholderChanel;
    public GameObject PanelConf;
    public GameObject Warning;
    public GameObject Succes;
    public GameObject noInternet;
    public void ConnectToTwitch()
    {
        Twitch = new TcpClient(URL, PORT);
        Reader = new StreamReader(Twitch.GetStream());
        Writer = new StreamWriter(Twitch.GetStream());

        Writer.WriteLine("PASS " + OAuth);
        Writer.WriteLine("NICK " + user.ToLower());
        Writer.WriteLine("JOIN #" + Channel.ToLower());
        Writer.Flush();
    }
    private void Awake() {
        OAuth = PlayerPrefs.GetString("BrowseKey", OAuth);
        user = PlayerPrefs.GetString("UserKey", user);
        Channel = PlayerPrefs.GetString("ChannelKey", Channel);
        
        placeholder.GetComponent<TMPro.TextMeshProUGUI>().text = OAuth;
        placeholderUser.GetComponent<TMPro.TextMeshProUGUI>().text = user;
        placeholderChanel.GetComponent<TMPro.TextMeshProUGUI>().text = Channel;
        ConnectToTwitch();
    }
    private void Start() {
        ConnectToTwitch();
    }
    private void Update() {
        ACtiveThePanel();
        CheckAouth();
        CheckInternetConnection();
        PingCounter += Time.deltaTime;
        if(PingCounter > 60)
        {
            Writer.WriteLine("PING " + URL);
            Writer.Flush();
            PingCounter = 0;
        }
        if(!Twitch.Connected)
        {
            ConnectToTwitch();
        }

        if(Twitch.Available > 0)
        {
            string messages = Reader.ReadLine();
            if(messages.Contains("PRIVMSG"))
            {
                int splitPoint = messages.IndexOf("!");
                string chatter = messages.Substring(1, splitPoint - 1);

                splitPoint = messages.IndexOf(":", 1);
                string msg = messages.Substring(splitPoint + 1);

                OnChatMessage?.Invoke(chatter, msg);
            }
            print(messages);
       
        }
    }
    void ACtiveThePanel()
    {
         if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(PanelConf.activeInHierarchy)
            {
              PanelConf.SetActive(false);
            }
            else
            {
                PanelConf.SetActive(true);
            }
        }
    }
    public void SetOAuth(string oauthKey)
    {
        OAuth = oauthKey.ToString();
        PlayerPrefs.SetString("BrowseKey", OAuth);
        ConnectToTwitch();
    }
    public void SetUser(string userName)
    {
        user = userName.ToString();
        PlayerPrefs.SetString("UserKey", user);
        ConnectToTwitch();
    }
    public void SetChannel(string ChannelName)
    {
        Channel = ChannelName.ToString();
        PlayerPrefs.SetString("ChannelKey", Channel);
        ConnectToTwitch();
    }

    public void GetUrlOAuth()
    {
        Application.OpenURL("https://www.twitchapps.com/tmi/");
    }
    public void ChangeBackgroundRed()
    {
       Camera.main.GetComponent<Camera>().backgroundColor = Color.red;
    }
    public void ChangeBackgroundBlue()
    {
       Camera.main.GetComponent<Camera>().backgroundColor = Color.blue;
    }
    public void ChangeBackgroundGreen()
    {
       Camera.main.GetComponent<Camera>().backgroundColor = Color.green;
    }
    void CheckAouth()
    {
        if(OAuth.Contains("oauth"))
        {
Warning.SetActive(false);
            Succes.SetActive(true);
        }
        else
        {
Warning.SetActive(true);
            Succes.SetActive(false);
        }
    }
    private void CheckInternetConnection()
    {
        NetworkReachability internetStatus = Application.internetReachability;

        if (internetStatus == NetworkReachability.NotReachable)
        {
            noInternet.SetActive(true);
        }
        else
        {
            noInternet.SetActive(false);
        }
 
    }
}
