using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace motor 
{
    class Program
    {
        //pulsr pulsr3 = new pulsr();



        static void Main()
        {
            pulsr pulsr3 = new pulsr();


            pulsr3.InitializeCommunication();    // check comm
            Console.WriteLine("Check Successfull");


            pulsr3.DisableLowerMotor();
            pulsr3.DisableUpperMotor();
            Console.WriteLine("Motor Disabled");

            /*Console.WriteLine("Do you want to enable the Motors?     Y or N");
            string input = Console.ReadLine();
            if (input == "Y")
            {
                pulsr3.EnableLowerMotor();
                pulsr3.EnableUpperMotor();

                Console.WriteLine("Motor Disabled");
            }
            else if (input == "N") { };


            Console.WriteLine("Do you want to move UPPER motor?     Y or N");
            string upper = Console.ReadLine();
            if (upper == "Y")
            {
                //pulsr3.UpperMotorCW(1, 20);

                //pulsr3.UpperMotorCCW(1, 20);
            }
            else if (upper == "N") { };

            


            Console.WriteLine("Do you want to move LOWER motor?     Y or N");
            string lower = Console.ReadLine();
            if (lower == "Y")
            {
                //pulsr3.LowerMotorCW(1, 20);

                //pulsr3.LowerMotorCCW(1, 20);
            }
            else if (lower == "N") { }; 




            Console.WriteLine("Do you want to UpdateMotorSpeed?     Y or N");
            string spee = Console.ReadLine();
            if (spee == "Y")
            {
                pulsr3.UpdateMotorSpeed(30, 25);
            }
            else if (spee == "N") { }; */


            Console.WriteLine("Do you want to test the Motors?     Y or N");
            string input = Console.ReadLine();
            if (input == "Y")
            {
                //test();
                //assistiveTest();
                activeTest();
            }
            else if (input == "N") { }

            /// <summary>
            /// First Disable all motors
            /// Move the upper by 10 and -10 while lower is zero/disbaled
            /// Move the lower by 10 and -10 while upper is zero/disabled
            /// Diable all motors
            /// </summary>
            void test()
            {
                pulsr3.DisableLowerMotor();
                pulsr3.DisableUpperMotor();


                pulsr3.UpdateMotorSpeed(10 , 0);  //Move 10 counter clockwise UPPER R
                Thread.Sleep(1000);
                pulsr3.UpdateMotorSpeed(0, 0);  
                Thread.Sleep(1000);
                pulsr3.UpdateMotorSpeed(-10, 0);  //Move 10 clockwise UPPER F
                Thread.Sleep(1000);
                pulsr3.UpdateMotorSpeed(0, 0);  
                Thread.Sleep(1000);

                pulsr3.UpdateMotorSpeed(0, 10);  // Move 10 counter clockwise LOWER R
                Thread.Sleep(1000);
                pulsr3.UpdateMotorSpeed(0, 0);  
                Thread.Sleep(1000);
                pulsr3.UpdateMotorSpeed(0, -10); // Move 10 clockwise LOWER F
                Thread.Sleep(1000);
                pulsr3.UpdateMotorSpeed(0, 0);  
                Thread.Sleep(1000);


                pulsr3.UpdateMotorSpeed(0, 0);  // Disable both upper and lower motor

            }

            void assistiveTest() 
            {
                Console.WriteLine("Please enter a number:");
                string input = Console.ReadLine() ;
                int number;
                bool isNumber = Int32.TryParse(input, out number);

                List<int> levels = new List<int> { 5, 6, 7 };
                
                if (isNumber && levels.Contains(number)) 
                {
                    Console.WriteLine("You entered a number. Press x to exit ");

                    while (true)
                    {
                        // Logic 

                        List<int> upper_force_t = new List<int>(Enumerable.Repeat(32000, 70));
                        List<int> lower_force_t = new List<int>(Enumerable.Repeat(32000, 70));

                        pulsr3.UpdateUpperLoadCell();
                        Console.WriteLine("UPPER LINK FORCE: "+ pulsr3.upper.link_force);
                        pulsr3.UpdateLowerLoadCell();
                        Console.WriteLine("LOWER LINK FORCE: "+ pulsr3.lower.link_force);
                        //Thread.Sleep(2000);
                        pulsr3.UpdateSensorData();

                         upper_force_t.RemoveAt(0);
                         upper_force_t.Add(pulsr3.upper.link_force);

                         lower_force_t.RemoveAt(0);
                         lower_force_t.Add(pulsr3.lower.link_force);

                         double threshold_upper, threshold_lower;
                         double threshold = 1000;

                         if (number == 6)
                         {
                             threshold_upper = (threshold / 2) + ((threshold * Math.Cos(Math.PI * pulsr3.upper.angle / 180)) / 2);
                             threshold_lower = (threshold / 2) + ((threshold * Math.Cos(Math.PI * (pulsr3.lower.angle - 110) / 180)) / 2);
                         }
                         else
                         {
                             threshold_upper = threshold;
                             threshold_lower = threshold;
                         }
                        //Console.WriteLine("upper thres"+ threshold_upper);
                        //Console.WriteLine("lower thres" + threshold_lower);
                        //Thread.Sleep(2000);
                         double ls, us;
                         double dest = 10;

                         if (pulsr3.lower.link_force < 30000 - threshold_lower)
                         {
                             ls = dest;
                         }
                         else if (pulsr3.lower.link_force > 34000 + threshold_lower)
                         {
                             ls = -dest;
                         }
                         else
                         {
                             ls = 0;
                         }

                         if (pulsr3.upper.link_force < 30000 - threshold_upper)
                         {
                             us = dest;
                         }
                         else if (pulsr3.upper.link_force > 34000 + threshold_upper)
                         {
                             us = -dest;
                         }
                         else
                         {
                             us = 0;
                         }

                         //capping the speed
                         if (us < 0)
                         {
                             if (Math.Abs(us) > dest)
                             {
                                 us = -dest;
                             }
                         }
                         else
                         {
                             if (Math.Abs(us) < dest)
                             {
                                 us = dest;
                             }
                         }

                         if (ls < 0)
                         {
                             if (Math.Abs(ls) > dest)
                             {
                                 ls = -dest;
                             }
                         }
                         else
                         {
                             if (Math.Abs(ls) < dest)
                             {
                                 ls = dest;
                             }
                         }

                        // Update speed instruction
                         Console.WriteLine($"This is Upper Target {us} and Lower Target {ls}");

                         pulsr3.UpdateMotorSpeed((int)us, (int)ls);

                         Console.WriteLine("SPEED UPDATED");

                         Thread.Sleep(100); 





                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey(true).Key;
                            if (key == ConsoleKey.X)
                            {
                                Console.WriteLine("You pressed 'x'. Exiting the loop.");
                                pulsr3.UpdateMotorSpeed(0, 0);
                                break;
                            }
                        }

                    }
                }
                else
                {
                    Console.WriteLine("Enter a Correct Level");
                }

            }


            void activeTest() 
            {
                Console.WriteLine("Please enter a number:");
                string input = Console.ReadLine();
                int number;
                bool isNumber = Int32.TryParse(input, out number);

                List<int> levels = new List<int> { 9, 10, 11 };

                if (isNumber && levels.Contains(number))
                {
                    Console.WriteLine("You entered a number. Press x to exit ");

                    while (true)
                    {
                        // Logic 

                        //List<int> upper_force_t = new List<int>(Enumerable.Repeat(32000, 70));
                        //List<int> lower_force_t = new List<int>(Enumerable.Repeat(32000, 70));

                        pulsr3.UpdateUpperLoadCell();
                        Console.WriteLine("UPPER LINK FORCE: " + pulsr3.upper.link_force);
                        pulsr3.UpdateLowerLoadCell();
                        Console.WriteLine("LOWER LINK FORCE: " + pulsr3.lower.link_force);
                        //Thread.Sleep(2000);
                        pulsr3.UpdateSensorData();

                        //upper_force_t.RemoveAt(0);
                        //upper_force_t.Add(pulsr3.upper.link_force);

                        //lower_force_t.RemoveAt(0);
                        //lower_force_t.Add(pulsr3.lower.link_force);

                        //Console.WriteLine("upper thres"+ threshold_upper);
                        //Console.WriteLine("lower thres" + threshold_lower);
                        //Thread.Sleep(2000);

                        double ls, us;
                        double dest = 10;



                        // Read all lines from the text files
                        string[] usLines = File.ReadAllLines("upper_targets1txt");
                        string[] lsLines = File.ReadAllLines("lower_targets1.txt");


                        // Initialize lists for ls and us // Populate with the data from assistive
                        List<double> usList = new List<double>();
                        List<double> lsList = new List<double>();
                        

                        // Parse and add the values to the lsList
                        foreach (string line in lsLines)
                        {
                            lsList.Add(double.Parse(line));
                        }
                        // Parse and add the values to the usList
                        foreach (string line in usLines)
                        {
                            usList.Add(double.Parse(line));
                        }


                        // Ensure both lists have the same length
                        if (lsList.Count != usList.Count)
                        {
                            Console.WriteLine("Error: Lists are not the same length.");
                          
                            return;
                        }
                        if (lsList.Count == usList.Count)
                        {
                            Console.WriteLine(lsList.Count);
                        }

                        // Loop through the lists
                       /* for (int i = 0; i < lsList.Count; i++)
                        {
                            ls = lsList[i];
                            us = usList[i];

                            // Update motor speed
                            pulsr3.UpdateMotorSpeed((int)us, (int)ls);

                            Console.WriteLine("SPEED UPDATED");

                            Thread.Sleep(100);



                            if (Console.KeyAvailable)
                            {
                                var key = Console.ReadKey(true).Key;
                                if (key == ConsoleKey.X)
                                {
                                    Console.WriteLine("You pressed 'x'. Exiting the loop.");
                                    pulsr3.UpdateMotorSpeed(0, 0);
                                    break;
                                }
                            } 

                        }*/


                        // Update speed instruction
                        //pulsr3.UpdateMotorSpeed((int)us, (int)ls);

                        //Console.WriteLine("SPEED UPDATED");

                        //Thread.Sleep(100);





                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey(true).Key;
                            if (key == ConsoleKey.X)
                            {
                                Console.WriteLine("You pressed 'x'. Exiting the loop.");
                                pulsr3.UpdateMotorSpeed(0, 0);
                                break;
                            }
                        }

                    }
                }
                else
                {
                    Console.WriteLine("Enter a Correct Level");
                }
            };


            //pulsr3.DisableLowerMotor();
            //pulsr3.DisableUpperMotor();
            //Console.WriteLine("Motor Disabled");

        }

        
    }
}