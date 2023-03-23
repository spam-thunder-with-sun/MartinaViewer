using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace MartinaViewer
{
    class ProgettoRete
    {
        List<Rete> lista;
        IPAddress ip, subnet;
        int nSottoreti;
        public ProgettoRete(IPAddress ip, IPAddress subnet, int nSottoreti)
        {
            if (nSottoreti <= 1)
                throw new ArgumentException("Sottoreti non valide");

            this.ip = ip;
            this.subnet = subnet;
            this.nSottoreti = nSottoreti;

            lista = new List<Rete>();
        }

        public void Add(Rete elem)
        {
            if (elem != null)
                lista.Add(elem);
            return;
        }

        public void CalcolaRete()
        {
            lista.Sort();
            //Ordino le sottoreti della piu' grande alla piu' piccola

            int n, count;
            IPAddress tmp = this.ip;

            for (int i = 0; i < lista.Count; i++)
            {
                n = 1;
                count = 1;
                while (n < lista[i].HostSodd)
                {
                    n = n << 1;
                    count++;
                }

                //lista[i].CalcolaRete(this.ip, Rete.ConvertSubnetMask(32 - count + 1), 32 - count + 1);
                tmp = lista[i].CalcolaRete(tmp, IPAddress.Parse(Rete.ConvertSubnetMask(Convert.ToString(32 - count + 1))), 32 - count + 1);
            }

            return;
        }

        public new string ToString()
        {
            string str = "";
            for (int i = 0; i < lista.Count; i++)
            {
                str += lista[i].ToString() + "\n*****\t*****\t*****\n";
            }
            return str;
        }
        
    }
}
