﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MarsRover.Domain;
using MarsRover.Domain.Enums;
using MarsRover.Domain.Models;

namespace MarsRover.ConsoleApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter the height of the plateau. For example: 5");
                int y;
                while (!int.TryParse(Console.ReadLine(), out y))
                {
                    Console.WriteLine("Enter the height of the plateau. For example: 5.");
                }

                Console.WriteLine("Enter the width of the plateau. For example: 5");
                int x;
                while (!int.TryParse(Console.ReadLine(), out x))
                {
                    Console.WriteLine("Enter the width of the plateau. For example: 5.");
                }

                Console.WriteLine("How many Rovers will be scanning the plateau?");
                int z;
                while (!int.TryParse(Console.ReadLine(), out z))
                {
                    Console.WriteLine("How many Rovers will be scanning the plateau? Please enter only numbers");
                }

                var hazards = new List<Coordinate>();
                for (int i = 0; i < z; i++)
                {
                    Console.WriteLine($@"Enter the landing X coordinate of rover {i + 1}.");
                    int xx;
                    while (!int.TryParse(Console.ReadLine(), out xx))
                    {
                        Console.WriteLine($@"Enter the landing X coordinate of rover {i + 1}.");
                    }
                    Console.WriteLine($@"Enter the landing Y coordinate of rover {i + 1}.");
                    int yy;
                    while (!int.TryParse(Console.ReadLine(), out yy))
                    {
                        Console.WriteLine($@"Enter the landing Y coordinate of rover {i + 1}.");
                    }

                    string zz;
                    Console.WriteLine($@"Enter the landing direction of rover {i + 1}. Format: N, S, W or E");
                    while (true)
                    {
                        zz = Console.ReadLine();
                        if (!Regex.IsMatch(zz, "[^NSWE]"))
                        {
                            Console.WriteLine($@"Enter the landing direction of rover {i + 1}. Format: N, S, W or E");
                            break;
                        }
                    }

                    Console.WriteLine($@"Enter the Commands you want rover {i + 1} to execute. Format: MRLM (M = move,  R = turn right, L = turn left). No spaces.");
                    string commands = Console.ReadLine();
                    
                    var result = new Domain.MarsRover(new Rover(new Plateau(x, y), new Location(new Coordinate(xx,yy), (DirectionEnum)Enum.Parse(typeof(DirectionEnum),zz)), 
                             commands, hazards, i + 1))
                        ?.ExecuteCommands();
                    if (result?.EndLocation?.Coordinate == null)
                    {
                        Console.WriteLine("We were unable to track your rover with the instructions you entered. Press any key to exit.");
                        return;
                    }

                    if (result.IsSuccess)
                    {
                        Console.WriteLine(
                                $@"Rover {i + 1} will be stopping on {result.EndLocation.Coordinate.X} {result.EndLocation.Coordinate.Y} {result.EndLocation.Heading.ToString()}."
                            );
                        hazards.Add(new Coordinate(result.EndLocation.Coordinate.X, result.EndLocation.Coordinate.Y));
                    }
                    Console.WriteLine(result.Message);

                }

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went terribly wrong and spun the squad in an orbit around Jupiter. Press enter to exit the program and try again.");
                Console.ReadLine();
            }
        }
    }
}
