using System;
using System.Text.RegularExpressions;
using System.Net;

namespace MartinaViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" * * * MartinaViewer * * * ");

            //Setto le variabili
            IPAddress ipv4Rete, subnetMask;
            int nSottoreti;
            string supp;
            int n;
            ProgettoRete reteTot;

            Inizio:
            Console.WriteLine("\n ... OK Iniziamo!\n");

            Console.WriteLine("Passo 1) Rilassati!\n");

            C1:
            Console.Write("Passo 2) Dammi l'indirizzo ipv4 della rete!\nScrivilo bene e controlla che non ci siano errori\nIndirizzo ipv4 della rete: ");
            supp = Console.ReadLine();
            if (!IPAddress.TryParse(supp, out ipv4Rete))
            {
                Console.WriteLine("Indirizzo sbagliato! Ricontrolla!\n");
                goto C1;
            }
            Console.WriteLine("Perfetto!");
            Console.WriteLine($"Indirizzo ip : {ipv4Rete.ToString()}");

            C2:
            Console.Write("Passo 3) Dammi la subnetMask della rete!\nScrivilo bene e controlla che non ci siano errori\nSubnetMask rete: ");
            supp = Console.ReadLine();
            if (Int32.TryParse(supp, out n))
            {
                subnetMask = IPAddress.Parse(Rete.ConvertSubnetMask(Convert.ToString(n)));
            }
            else if (!IPAddress.TryParse(supp, out subnetMask))
            {
                Console.WriteLine("Indirizzo sbagliato! Ricontrolla!\n");
                goto C2;
            }
            Console.WriteLine("Perfetto!");
            Console.WriteLine($"Subnet Mask : {subnetMask.ToString()}");

            C3:
            Console.Write("Passo 4) Dimmi quante sottoreti devono esserci nella rete!\nScrivilo bene e controlla che non ci siano errori\nNumero sottoreti nella rete: ");
            if (!Int32.TryParse(Console.ReadLine(), out nSottoreti) || nSottoreti <= 1)
            {
                Console.WriteLine("Impossibile! Ricontrolla!\n");
                goto C3;
            }
            Console.WriteLine("Perfetto!");

            C4:
            Console.Write("Attendi sto creando la rete ...");
            try
            {
                reteTot = new ProgettoRete(ipv4Rete, subnetMask, nSottoreti);
            }
            catch
            {
                Console.Write("\nPROBLEMA! NON E' STATO POSSIBILE ISTANZIARE UNA NUOVA RETE!\n Ripeti il procedimento!");
                goto Inizio;
            }
            Console.WriteLine(" OK");

            Console.WriteLine("Passo 5) Inserisci i dati delle sottoreti!\nScrivilo bene e controlla che non ci siano errori\n");
            for(int i = 0; i <nSottoreti; i++)
            {
                Console.WriteLine($"Rete n{i + 1}: ");
                Console.Write("Inserici nome rete: ");
                supp = Console.ReadLine();
                C4_1:
                Console.Write("Inserici numero di host richiesti: ");
                if (!Int32.TryParse(Console.ReadLine(), out n) || nSottoreti <= 0)
                {
                    Console.WriteLine("Impossibile! Ricontrolla!\n");
                    goto C4_1;
                }

                Console.Write("OK creo nuova sottorete ... ");
                try
                {
                    reteTot.Add(new Rete(supp, n));
                }
                catch
                {
                    Console.Write("\nPROBLEMA! NON E' STATO POSSIBILE ISTANZIARE UNA NUOVA SOTTORETE!\n Ripeti il procedimento!");
                    goto C4;
                }
                Console.WriteLine(" OK");
            }
            Console.WriteLine("\nTutte le reti sono state create correttamente! ");

            Console.WriteLine("Passo 6)Attendi! Lasciami calcolare in pace! \n");
            Console.Write("Calcolo rete . . .");

            reteTot.CalcolaRete();

            Console.WriteLine("OK");
            Console.WriteLine("Ecco i risulati: ");

            Console.WriteLine(reteTot.ToString());

            Console.ReadKey();
        }
    }
}
