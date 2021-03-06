﻿using MyAirport.Pim.Entities;
using MyAirport.Pim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCli
{
    class Program
    {
        static void Main(string[] args)
        {
            AbstractDefinition model = Factory.Model; // get either Natif or SQL model
            while (true)
            {
                Console.WriteLine("$ Type IATA code to search for luggages, or type 'quit' to quit");
                Console.Write("> ");
                string input = Console.ReadLine();
                if (input == "quit") { break; }
                Console.WriteLine("Looking for luggage " + input);
                List<BagageDefinition> bagageList = model.GetBagage(input);
                switch (bagageList.Count)
                {
                    case 0:
                        // TODO create
                        BagageDefinition newBagage = readBagageAttributes();
                        if (newBagage == null) // user quit
                        {
                            Console.WriteLine("$ Bagage creation canceled");
                            continue;
                        }
                        model.CreateBagage(newBagage);
                        Console.WriteLine("Created luggage " + newBagage.CodeIata);
                        break;
                    case 1:
                        Console.WriteLine(bagageList.First());
                        break;
                    default:
                        Console.WriteLine("Several luggages have been found:");
                        foreach (var bagage in bagageList)
                        {
                            Console.WriteLine("    * " + bagage + "\n");
                        }
                        break;
                }
            }
        }

        private static BagageDefinition readBagageAttributes()
        {
            string iata = readInput("IATA? (ex: 23232342)");
            string compagnie = readInput("Compagnie? (ex: UE)");
            string ligne = readInput("Ligne? (ex: 7594)");
            string escale = readInput("Escale? (ex: CDG)");
            string prioritaire = readInput("Prioritaire? (Y/N)");
            string en_continuationion = readInput("En continuation? (Y/N)");

            BagageDefinition bagage = new BagageDefinition();
            try
            {
                bagage.CodeIata = iata;
                bagage.Compagnie = compagnie;
                bagage.Ligne = ligne;
                bagage.Itineraire = escale;
                bagage.Prioritaire = prioritaire == "Y" ? true : false;
                bagage.EnContinuation = en_continuationion == "Y" ? true : false;
            }
            catch (Exception)
            {
                return null;
            }

            return bagage;
        }

        private static string readInput(string instruction)
        {
            Console.WriteLine("$ " + instruction);
            Console.Write("> ");
            return Console.ReadLine();
        }
    }
}
