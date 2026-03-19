


using System;
using Karianakis.Utilities;

public class InvoAdvanced
{
    public static invoAdvancedBuilder Simple
         (Action<invoAdvancedBuilder> action, float delay)
             => new invoAdvancedBuilder(action, delay , 0 );

    public static invoAdvancedBuilder Repeat
        (Action<invoAdvancedBuilder> action, float delay, int repeat) 
            => new invoAdvancedBuilder(action, delay, repeat);
        
    public static invoAdvancedBuilder Infinite
        (Action<invoAdvancedBuilder> action, float delay) 
            => new invoAdvancedBuilder(action, delay, InvoBase._infiniteRepeats);

}