using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using UnityEditor.SearchService;
public class CSharpForGIT : MonoBehaviour
{
    Thread mThread;
    public string connectionIP = "127.0.0.1";
    public int connectionPort = 25001;
    IPAddress localAdd;
    TcpListener listener;
    TcpClient client;
    Vector3 receivedPos = Vector3.zero;

    Dictionary<string, GameObject> gameObjects;
    string[] data;
    //GameObject hand = GameObject.Find("hand (1)");
    bool running;

    private void Update()
    {
        //transform.position = receivedPos; //assigning receivedPos in SendAndReceiveData()
        if (data != null)
        {
            foreach (string s in data)
            {
                print(s);
                string[] p = s.Split(':');
                {
                    Debug.Log(p[0].ToString());
                    if (gameObjects.ContainsKey(p[0]))
                    {
                        GameObject gameObject = gameObjects[p[0]];
                        string[] pos = p[1].Split(',');
                        gameObject.transform.position = new Vector3(-float.Parse(pos[0])/2, -float.Parse(pos[1])/2, -float.Parse(pos[2])/2);
                    }
                }
            }
        }
    }

    private void Start()
    {
        gameObjects = new Dictionary<string, GameObject>() { { "lHand", GameObject.Find("lHand") }, { "lIndex0", GameObject.Find("lIndex0") }, { "lIndex1", GameObject.Find("lIndex1") }, { "lIndex2", GameObject.Find("lIndex2") }, { "lIndex3", GameObject.Find("lIndex3") }, { "lMid0", GameObject.Find("lMid0") }, { "lMid1", GameObject.Find("lMid1") }, { "lMid2", GameObject.Find("lMid2") }, { "lMid3", GameObject.Find("lMid3") }, { "lRing0", GameObject.Find("lRing0") }, { "lRing1", GameObject.Find("lRing1") }, { "lRing2", GameObject.Find("lRing2") }, { "lRing3", GameObject.Find("lRing3") }, { "lPinky0", GameObject.Find("lPinky0") }, { "lPinky1", GameObject.Find("lPinky1") }, { "lPinky2", GameObject.Find("lPinky2") }, { "lPinky3", GameObject.Find("lPinky3") }, { "lThumb1", GameObject.Find("lThumb1") }, { "lThumb2", GameObject.Find("lThumb2") }, { "lThumb3", GameObject.Find("lThumb3") } };

        ThreadStart ts = new ThreadStart(GetInfo);
        mThread = new Thread(ts);
        mThread.Start(); 
       // GameObject hand = GameObject.Find("hand (1)");
    }

    void GetInfo()
    {
        localAdd = IPAddress.Parse(connectionIP);
        listener = new TcpListener(IPAddress.Any, connectionPort);
        listener.Start();

        client = listener.AcceptTcpClient();

        running = true;
        while (running)
        {
            SendAndReceiveData();
        }
        listener.Stop();
    }

    void SendAndReceiveData()
    {
        NetworkStream nwStream = client.GetStream();
        byte[] buffer = new byte[client.ReceiveBufferSize];

        //---receiving Data from the Host----
        int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
        string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead); //Converting byte data to string

        if (dataReceived != null)
        {
            //---Using received data---
            // receivedPos = StringToVector3(dataReceived); //<-- assigning receivedPos value from Python
            data = dataReceived.Split('{');
            /*foreach (string s in data)
            {
                print(s);
                string[] p = s.Split(':');
                {
                    Debug.Log(p[0].ToString());
                    if (gameObjects.ContainsKey(p[0]))
                    {
                        GameObject gameObject = gameObjects[p[0]];
                        string[] pos = p[1].Split(',');
                        gameObject.transform.position = new Vector3(float.Parse(pos[1]), float.Parse(pos[2]), float.Parse(pos[3]));
                    } 
                }
            }*/
            //print("received pos data, and moved the Cube!");

            //---Sending Data to Host----
            byte[] myWriteBuffer = Encoding.ASCII.GetBytes("Hey I got your message Python! Do You see this massage?"); //Converting string to byte data
            nwStream.Write(myWriteBuffer, 0, myWriteBuffer.Length); //Sending the data in Bytes to Python
        }
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
    /*
    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
    */
}