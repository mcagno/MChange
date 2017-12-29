Matteo Cagno, 29th December 2017
# Magic Purse

## Introduction
This code is provided as a solution to the Magic Purse technical test.
The solution contains the 

## Projects structure

#### MagicPurse.Library
A class library containing the main classes performing the calculations.

###### MagicPurseSync
Class that performs the calculation in a single thread.

###### MagicPurseAsync
Class that performs the calculation by using a parallelism for the calculations of the number of splits.

###### MagicPurseQueue
Class that performs the calculation by using a queue and parallel threads for calculation of the combinations and their possible splits.

###### Splitter
Class that given a combination of coins calculates the number of possible splits.

###### AmountParser
Class that parses and validate a string to obtain the number of halfpence to be given to the algorithm.

#### MagicPurse.Tests
Sets of unit tests for the classes of the MagicPurse.Library

#### MagicPurse
Console application, asking for an input by the user and returns the number of possibilities to share the coins for the give amount, according with the requirements provided.
The console application accepts an optional argument that allows to perform the calculation using the different methods available:
- -s (or no argument): performs the calculation in a single thread
- -a : performs the calculation by using parallel threads
- -q : performs the calculation by using parallel threads and a queue

