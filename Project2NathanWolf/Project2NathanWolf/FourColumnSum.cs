////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//  Project:        Project2NathanWolf
//  File Name:      FourColumnSum.cs
//  Description:    Project 2 - Line Sums
//  Course:         CSCI 3230-001
//  Author:         Nathan Wolf, wolfnj@etsu.edu
//  Created:        Wednesday, October 17th, 2018 
//  Copyright:      Nathan Wolf, 2018
//
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2NathanWolf
{
    /// <summary>
    /// Class which contains the project algorithm
    /// This Algorithm will find the number of times a given sum can be found from 4
    /// columns of data. It will find the sum adding only one number from each column,
    /// at any position in the data  
    /// 
    /// </summary>
    class FourColumnSum
    {

        static Stopwatch sw;                   //Stopwatch for timing algorithm

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            int NumberOfTestCases;          //The number of test cases the user will input
            String strNumberOfTestCases;    //String format of NumberOfTestCases
            int NumberOfLines;          //The number of lines for each test case
            String strNumberOfLines;    //String format of NumberOfLines
            int Sum;                    //The sum to find from the lines
            String strSum;              //String format of Sum
            String strLines;            //Reads each in line 

            string[] values;             //for reading in the numbers for each line

            strNumberOfTestCases = Console.ReadLine();
            NumberOfTestCases = Int32.Parse(strNumberOfTestCases);

            double[] FirstColumn;      //Holds all numbers from the 1st column
            double[] SecondColumn;     //Holds all numbers from the 2nd column
            double[] ThirdColumn;      //Holds all numbers from the 3rd column
            double[] FourthColumn;     //Holds all numbers from the 4th column

            double[] RightSide;        //The array that holds all possible sums of 
                                    //first two columns
            int RightSideIndex;     //The index for the RightSide array 

            double[] LeftSide;        //The array that holds all possible sums of 
                                   //last two columns
            int LeftSideIndex;     //The index for the LeftSide array

            double FindNum;         //The number we which to find in the RightSide array

            int TotalNumberOfSums;      //The total number of sums for each test

            int[] FinalAnswers;           //Stores the final answers of the number of sums for each
                                          //test case that will be printed at the end

            int RightSideMin;               //The min index of the RightSide array
            int RightSideMax;               //The max index of the RightSide array


            FinalAnswers = new int[NumberOfTestCases];


            for (int i=0;i< NumberOfTestCases; i++)
            {
                sw = Stopwatch.StartNew();        //starts stopwatch



                TotalNumberOfSums = 0;      //Starts at zero for each test case
                strNumberOfLines = Console.ReadLine();
                NumberOfLines = Int32.Parse(strNumberOfLines);
                FirstColumn = new double[NumberOfLines];
                SecondColumn = new double[NumberOfLines];
                ThirdColumn = new double[NumberOfLines];
                FourthColumn = new double[NumberOfLines];
                strSum = Console.ReadLine();
                Sum = Int32.Parse(strSum);

                //Reading in each actual line
                for (int j = 0; j < NumberOfLines; j++)
                {
                    strLines = Console.ReadLine();
                    values = strLines.Split(' ');
                    FirstColumn[j]=Int32.Parse(values[0]);
                    SecondColumn[j] = Int32.Parse(values[1]);
                    ThirdColumn[j] = Int32.Parse(values[2]);
                    FourthColumn[j] = Int32.Parse(values[3]);
                }

                RightSide = new double[NumberOfLines*NumberOfLines];
                RightSideIndex = 0;
                for (int k = 0;k<FirstColumn.Length;k++)
                {
                    for(int l=0; l<SecondColumn.Length;l++)
                    {
                        RightSide[RightSideIndex] = FirstColumn[k] + SecondColumn[l];
                        RightSideIndex += 1;
                    }
                }

                LeftSide = new double[NumberOfLines*NumberOfLines];
                LeftSideIndex = 0;
                for (int k = 0; k < ThirdColumn.Length; k++)
                {
                    for (int l = 0; l < FourthColumn.Length; l++)
                    {
                        LeftSide[LeftSideIndex] = ThirdColumn[k] + FourthColumn[l];
                        LeftSideIndex += 1;
                    }
                }

                //SORT THE RightSide Array
                RightSide = MergeSort(RightSide);
                RightSideMin = 0;
                RightSideMax = RightSide.Length - 1;

                for(int k=0;k<LeftSide.Length;k++)
                {
                    //We want to find all possible LeftSide + RightSide = sum
                    //We can change this equation to RightSide = sum - LeftSide
                    //Store FindNum = sum - LeftSide
                    //We can then search all elements in the RightSide for FindNum using
                    //BinarySearch
                    FindNum = Sum - LeftSide[k];
                    TotalNumberOfSums += BinarySearch(RightSide, FindNum, RightSideMin, RightSideMax);

                }

                //Store the Final Answer to the test case
                FinalAnswers[i] = TotalNumberOfSums;

                sw.Stop();
            }//end for (int i=0;i< NumberOfTestCases; i++)




            for(int i=0; i<FinalAnswers.Length; i++)
            {
                Console.WriteLine($"{FinalAnswers[i]}");
            }


            Console.WriteLine($"Time used: {sw.Elapsed.TotalMilliseconds / 1000} secs");
            Console.ReadLine();
        }

        /// <summary>
        /// Merges the two arrays together in order
        /// </summary>
        /// <param name="B">The first array.</param>
        /// <param name="C">The second array.</param>
        /// <returns>The merged array</returns>
        private static double[] Merge(double[] B, double[] C)
        {
            int NewArraySize;           //size of the outputted array
            double[] MergedArray;       //The outputted Merged array
            int i;                      //the indexes of the B array
            int j;                      //the indexes of the C array
            int k;                      //the indexes of the returned array
            int StartExcess;
            NewArraySize = B.Length + C.Length;    //New array will be the size of the two arrays put together
            MergedArray = new double[NewArraySize];
            i = 0;
            j = 0;
            k = 0;
            //start all the indexes at zero


            if (NewArraySize == 2)
            {
                if (B[i] < C[j])
                {
                    MergedArray[k] = B[i];
                    MergedArray[k + 1] = C[j];
                }
                else
                {
                    MergedArray[k] = C[j];
                    MergedArray[k + 1] = B[i];
                }
            }
            else
            {
                while (k < NewArraySize)
                {
                    if (B[i] < C[j])
                    {
                        MergedArray[k] = B[i];
                        i += 1;
                        if (i == B.Length)
                        {
                            for (StartExcess = j; StartExcess <= (C.Length - 1); StartExcess++)
                            {
                                k += 1;
                                MergedArray[k] = C[StartExcess];
                            }
                            break;
                        }
                        k += 1;
                    }
                    else
                    {
                        MergedArray[k] = C[j];
                        j += 1;
                        if (j == C.Length)
                        {
                            for (StartExcess = i; StartExcess <= (B.Length - 1); StartExcess++)
                            {
                                k += 1;
                                MergedArray[k] = B[StartExcess];
                            }
                            break;
                        }
                        k += 1;
                    }
                }

            }



            return MergedArray;
        }//end Merge(double[] B,double[] C)


        /// <summary>
        /// Does the MergeSort Recursion
        /// </summary>
        /// <param name="A">The int array you wish to sort.</param>
        /// <returns>The sorted array</returns>
        private static double[] MergeSort(double[] A)
        {

            int half = A.Length / 2;

            if (A.Length <= 1)
            {
                return A;
            }

            return Merge(MergeSort(A.Take(half).ToArray()), MergeSort(A.Skip(half).ToArray()));

        }

        /// <summary>
        /// Performs the BinarySearch on an array.
        /// Finds the number of times the number appears in the array if any
        /// </summary>
        /// <param name="input">The array that is searched.</param>
        /// <param name="key">The number to find in input.</param>
        /// <param name="min">The minimum index of the input array.</param>
        /// <param name="max">The maximum index of the input array.</param>
        /// <returns>the number of times the number appears in the array if any</returns>
        private static int BinarySearch(double[] input, double key, int min, int max)
        {
            int NumberOfKeys;       //The number of times the key appears
            int LeftLook;           //Index for looking to the left of the array for key repeats
            int RightLook;          //Index for looking to the right of the array for key repeats
            int MaxRightLook;       //For checking that the Rightlook index does not go out of bounds


            MaxRightLook = input.Length - 1;
            NumberOfKeys = 0;
            if (min > max)
            {
                return NumberOfKeys;
            }
            else
            {
                int mid = (min + max) / 2;
                if (key == input[mid])
                {

                    NumberOfKeys += 1;
                    LeftLook = mid-1;
                    RightLook = mid+1;

                    
                    //Search to the left of mid to find any repeats
                    while(LeftLook>=0 && input[LeftLook] == key)
                    {
                        NumberOfKeys += 1;

                        LeftLook -= 1;
                     

                    }

                    //Search to the right of mid to find any repeats


                    while (RightLook < MaxRightLook && input[RightLook] == key)
                    {
                        NumberOfKeys += 1;

                        RightLook += 1;

                    }
  
                    

                    return NumberOfKeys;
                }
                else if (key < input[mid])
                {
                    return BinarySearch(input, key, min, mid - 1);
                }
                else
                {
                    return BinarySearch(input, key, mid + 1, max);
                }
            }
        }



    }


}

