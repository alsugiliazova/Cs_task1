using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Lab1
{
    class Program
    {
        public static Complex F1(Vector2 v2)
        {
            return new Complex(v2.X, v2.Y);
        }
        static void Main()
        {
            //1. V2DataArray
            V2DataArray arr = new V2DataArray("Object", new DateTime(2021, 01, 01), 3, 2, new Vector2(1.8f, 1.5f), F1);
            Console.WriteLine(arr.ToLongString("N1"));
            V2DataList list = arr;
            Console.WriteLine(list.ToLongString("N1"));
            Console.WriteLine($"Count: {arr.Count}, MinDistance: {arr.MinDistance}");
            Console.WriteLine($"Count: {list.Count}, MinDistance: {list.MinDistance}");

            //2. V2MainCollection
            V2MainCollection collection = new V2MainCollection();
            V2DataArray arr2 = new V2DataArray("Object2_2", new DateTime(2021, 01, 01), 2, 0, new Vector2(0.5f, 1.15f), F1);
            V2DataList list2 = new V2DataList("List", new DateTime(2021, 01, 01));
            collection.Add(arr);
            collection.Add(arr2);
            collection.Add(list);
            collection.Add(list2);
            Console.WriteLine(collection.ToLongString("N1"));

            //3. Count и MinDistance 
            for (int i = 0; i < collection.Count; ++i)
            {
                Console.WriteLine($"Count: {collection[i].Count}, MinDistance: {collection[i].MinDistance}");
            }
        }
    }
}
