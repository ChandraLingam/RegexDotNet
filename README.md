# Learning Regular Expression with .NET
## Purpose
Regular Expression Interactive Tool allows you to visually verify patterns and action.
Application is in C# and uses regular expression engine that is part of .NET framework.

Developed with Visual Studio Community 2015 Edition.  
Compile the solution and run it!

Originally developed as part of Learning Regular Expression with .NET Course: https://www.udemy.com/learning-regular-expression-with-net/?couponCode=NEW2017

Author: Chandra Lingam

License: GNU General Public License, version 3 (GPL-3.0) https://opensource.org/licenses/GPL-3.0


## Usage
Pattern - Specify pattern here  
Text - Text in which to look for Pattern  
Result - Output of the operation  
Replacement Pattern - Specify replacement text or pattern (to be used with Replace button)  


## Actions
Match - Find the first match 
Next Match - Find subsequent matches 
Replace - Find and replace all content that matches the search Pattern with Replacement Text
Split - Split text


## Standalone Files (can be embedded as Unit Test)
UsageExample.cs - Shows different capabilities of .NET regex engine and features
RobocopLogExample.cs - Shows how to parse robocopy log files
Data - Contains Robocopy Log files to be used with RobocopyLogExample.cs (modify file path)
