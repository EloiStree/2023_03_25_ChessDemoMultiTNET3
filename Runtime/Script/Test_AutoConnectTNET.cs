using System.Collections;
using TNet;
using UnityEngine;
using UnityTools = TNet.UnityTools;

public class Test_AutoConnectTNET : MonoBehaviour
{
    public float m_timebetweenAutoconnectAttempt = 1;
    public int serverTcpPort = 5127;
    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(m_timebetweenAutoconnectAttempt);
            List<ServerList.Entry> list = LobbyClient.knownServers.list;

            Debug.Log("Connect ?");
            if (list.Count == 0) {

                Debug.Log("Create lan attempt");
                int udpPort = Random.Range(10000, 40000);
                LobbyClient lobby = GetComponent<LobbyClient>();

                if (lobby == null)
                {
                    if (TNServerInstance.Start(serverTcpPort, udpPort, "server.dat"))
                        TNManager.Connect();
                }
                else
                {
                    TNServerInstance.Type type = (lobby is TNUdpLobbyClient) ?
                        TNServerInstance.Type.Udp : TNServerInstance.Type.Tcp;

                    if (TNServerInstance.Start(serverTcpPort, udpPort, lobby.remotePort, "server.dat", type))
                        TNManager.Connect();
                }
            }
            else  if (list.Count > 0 )
            {
                Debug.Log("Attempt to connect.");
                TNManager.Connect(list.buffer[0].internalAddress, list.buffer[0].internalAddress);
               
            }
            yield return new WaitForSeconds(2);

            TNManager.JoinChannel(m_channelId, m_sceneToload, true, 255, null);
            Destroy(this);
        }
    }

    public int m_channelId=1;
    public string m_sceneToload= "3. Frequent Packets";


}
