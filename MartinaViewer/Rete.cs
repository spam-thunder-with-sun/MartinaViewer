using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace MartinaViewer
{
    class Rete : IComparable<Rete>
    {
        string nome;
        int hostRich, hostSodd, nBy1;
        IPAddress ip, subnet, broadcast;
        public int HostSodd => hostSodd;
        public int HostRich => hostRich;
        public string Nome => nome;

        public Rete(string nome, int hostRich)
        {
            //Calcolo host occupati
            int n = 1;
            while(n < hostRich + 2)
                n = n << 1;
            
            this.hostSodd = n;
            this.nome = nome;
            this.hostRich = hostRich;
        }

        public IPAddress CalcolaRete(IPAddress ip, IPAddress subnet, int nby1)
        {
            this.nBy1 = nby1;
            this.ip = ip;
            this.subnet = subnet;
            byte[] vetIp = Rete.ConvertIpAddress(ip);
            byte[] vetSub = Rete.ConvertIpAddress(subnet);
            this.broadcast = Rete.CalcolaBroadcastAddress(ip, subnet);

            //Devo restituire l'indirizzo dopo di quello di broadcast
            string[] s = this.broadcast.ToString().Split(".");
            byte[] b = new byte[4];

            for(int i = 0; i < 4; i++)
                b[i] = Convert.ToByte(s[i]);

            if(b[3] == 255)
            {
                b[3] = 0;
                if (b[2] == 255)
                {
                    b[2] = 0;
                    if (b[1] == 255)
                    {
                        b[1] = 0;
                        if (b[0] == 255)
                            throw new Exception("Indirizzo ip non valido! - Errore interno!");
                        else
                            b[0]++;
                    }
                    else
                        b[1]++;
                }
                else
                    b[2]++;
            }
            else
                b[3]++;

            return new IPAddress(b);
        }

        static IPAddress CalcolaBroadcastAddress(IPAddress ip, IPAddress sub)
        {
            string[] strIP = ip.ToString().Split('.');
            string[] strSub = sub.ToString().Split('.');
            ArrayList list = new ArrayList();

            for (int i = 0; i < 4; i++)
            {
                int n = Convert.ToInt32(strIP[i]) | (Convert.ToInt32(strSub[i]) ^ 255);
                list.Add(n.ToString());
            }
            return IPAddress.Parse($"{list[0]}.{list[1]}.{list[2]}.{list[3]}");
        }
        public new string ToString() => $"Nome rete: {nome}\nHost richiesti: {hostRich}\nIndirizzi occupati: {HostSodd}\n" +
        $"Indirizzo ipv4: {ip.ToString()}\\{nBy1}\nSubnetMask: {subnet.ToString()}\nIndirizzo di broadcast: {broadcast.ToString()}\n";
        public int CompareTo(Rete other)
        {
            if (this.hostRich < other.hostRich)
                return 1;
            else if (this.hostRich > other.hostRich)
                return -1;
            else
                return 0;
        }

        public static string ConvertSubnetMask(string netMask)//Metodo non fatto da me
        {
            string subNetMask = string.Empty;
            int calSubNet = 0;
            double result = 0;
            if (!string.IsNullOrEmpty(netMask))
            {
                calSubNet = 32 - Convert.ToInt32(netMask);
                if (calSubNet >= 0 && calSubNet <= 8)
                {
                    for (int ipower = 0; ipower < calSubNet; ipower++)
                    {
                        result += Math.Pow(2, ipower);
                    }
                    double finalSubnet = 255 - result;
                    subNetMask = "255.255.255." + Convert.ToString(finalSubnet);
                }
                else if (calSubNet > 8 && calSubNet <= 16)
                {
                    int secOctet = 16 - calSubNet;

                    secOctet = 8 - secOctet;

                    for (int ipower = 0; ipower < secOctet; ipower++)
                    {
                        result += Math.Pow(2, ipower);
                    }
                    double finalSubnet = 255 - result;
                    subNetMask = "255.255." + Convert.ToString(finalSubnet) + ".0";
                }
                else if (calSubNet > 16 && calSubNet <= 24)
                {
                    int thirdOctet = 24 - calSubNet;

                    thirdOctet = 8 - thirdOctet;

                    for (int ipower = 0; ipower < thirdOctet; ipower++)
                    {
                        result += Math.Pow(2, ipower);
                    }
                    double finalSubnet = 255 - result;
                    subNetMask = "255." + Convert.ToString(finalSubnet) + ".0.0";
                }
                else if (calSubNet > 24 && calSubNet <= 32)
                {
                    int fourthOctet = 32 - calSubNet;

                    fourthOctet = 8 - fourthOctet;

                    for (int ipower = 0; ipower < fourthOctet; ipower++)
                    {
                        result += Math.Pow(2, ipower);
                    }
                    double finalSubnet = 255 - result;
                    subNetMask = Convert.ToString(finalSubnet) + ".0.0.0";
                }
            }

            return subNetMask;
        }

        public static byte[] ConvertIpAddress(IPAddress ip) => ConvertIpAddress(ip.ToString());
        public static byte[] ConvertIpAddress(string ip)
        {
            string[] s = ip.Split(".");
            byte[] vet = new byte[4];

            for(int i = 0; i < 4; i++)
                vet[i] = Convert.ToByte(s[i]);

            return vet;
        }        
    }
}
