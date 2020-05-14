using System.Collections.Generic;

static class KvpExtensions
{
    
    //method to "deconstruct" KeyValuePairs
    //aka
    // KeyValuePair<TKey,TValue> v = ...;
    // var (Key,Value) = v; 
    public static void Deconstruct<TKey, TValue>(
        this KeyValuePair<TKey, TValue> kvp,
        out TKey key,
        out TValue value)
    {
        key = kvp.Key;
        value = kvp.Value;
    }
}