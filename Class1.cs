using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Lab1
{
    struct DataItem
    {
        public Vector2 xy { get; set; }
        public Complex field { get; set; }
        public DataItem(Vector2 vvalue, Complex fvalue)
        {
            xy = vvalue;
            field = fvalue;
        }
        public string ToLongString(string format)
        {
            return $"X: {xy.X.ToString(format)}, Y: {xy.Y.ToString(format)}, Value: {field.ToString(format)}, Absolute value: {Complex.Abs(field).ToString(format)}";
        }
        public override string ToString()
        {
            return ToLongString("");
        }
    }
    abstract class V2Data
    {
        public string id { get; }
        public DateTime date { get; }
        public V2Data(string id_, DateTime date_)
        {
            id = id_;
            date = date_;
        }
        public abstract int Count { get; }
        public abstract float MinDistance { get; }
        public abstract string ToLongString(string format);
        public override string ToString()
        {
            return $"Str: {id}, Date: {date}";
        }
    }

    class V2DataList : V2Data
    {
        public List<DataItem> ItemList { get; }
        public V2DataList(string id, DateTime d) : base(id, d)
        {
            ItemList = new List<DataItem>();
        }
        public bool Add(DataItem newItem)
        {
            if (ItemList.Exists(x => x.xy == newItem.xy))
            {
                return false;
            }
            else
            {
                ItemList.Add(newItem);
                return true;
            }
        }
        public int AddDefaults(int nItems, Fv2Complex F)
        {
            int n = 0;
            int k = 1;
            int x = k;
            int y = k;
            for (int i = 0; i < nItems; ++i)
            {
                x = k + x;
                y = k + y;
                k += 1;
                Vector2 v = new Vector2(x, y);
                Complex field = F(v);
                DataItem newItem = new DataItem(v, field);
                if (Add(newItem)) ++n;
            }
            return n;
        }
        public override int Count
        {
            get => ItemList.Count;
        }
        public override float MinDistance
        {
            get
            {
                float min_distance = float.MaxValue;
                for (int i = 0; i < Count; ++i)
                    for (int j = i + 1; j < Count; ++j)
                    {
                        float cur_distance = Vector2.Distance(ItemList[i].xy, ItemList[j].xy);
                        if (cur_distance <= min_distance)
                        {
                            min_distance = cur_distance;
                        }
                    }
                if (Count == 0 || Count == 1) min_distance = 0;
                return min_distance;
            }
        }
        public override string ToString()
        {
            return "V2DataList: " + base.ToString() + " Count: " + ItemList.Count;
        }
        public override string ToLongString(string format)
        {
            string str = "";
            for (int i = 0; i < Count; ++i)
            {
                str += "\n" + ItemList[i].ToLongString(format);
            }
            return ToString() + str + "\n";
        }
    }
    class V2DataArray : V2Data
    {
        public Complex[,] values { get; }
        public int nx { get; }
        public int ny { get; }
        public Vector2 nxy { get; }
        public V2DataArray(string id, DateTime d) : base(id, d)
        {
            values = new Complex[0, 0];
        }
        public V2DataArray(string id, DateTime d, int nx, int ny, Vector2 nxy, Fv2Complex F) : base(id, d)
        {
            this.nx = nx;
            this.ny = ny;
            this.nxy = nxy;
            values = new Complex[nx, ny];
            for (int i = 0; i < nx; ++i)
            {
                for (int j = 0; j < ny; ++j)
                {
                    float x = i * nxy.X;
                    float y = j * nxy.Y;
                    Vector2 xy = new Vector2(x, y);
                    values[i, j] = F(xy);
                }
            }
        }
        public override int Count
        {
            get => nx * ny;
        }
        public override float MinDistance
        {
            get
            {
                float min_distance = float.MaxValue;
                min_distance = Math.Min(nxy.X, nxy.Y);
                if (nx == 1) min_distance = nxy.Y;
                if (ny == 1) min_distance = nxy.X;
                if (Count == 0 || Count == 1) min_distance = 0;
                return min_distance;
            }
        }
        public override string ToString()
        {
            return $"V2DataArray: {base.ToString()}, nx: {nx}, ny: {ny}, nxy: {nxy.X} {nxy.Y}";
        }
        public override string ToLongString(string format)
        {
            string str = "";
            for (int i = 0; i < nx; ++i)
            {
                for (int j = 0; j < ny; ++j)
                {
                    str += $"\n X: {(i * nxy.X).ToString(format)}, Y: {(j * nxy.Y).ToString(format)}, Value: {values[i, j].ToString(format)}, Abs: {Complex.Abs(values[i, j]).ToString(format)}";
                }
            }
            return ToString() + str + "\n";
        }

        public static implicit operator V2DataList(V2DataArray arr)
        {
            V2DataList list = new V2DataList(arr.id, arr.date);
            for (int i = 0; i < arr.nx; ++i)
            {
                for (int j = 0; j < arr.ny; ++j)
                {
                    float x = i * arr.nxy.X;
                    float y = j * arr.nxy.Y;
                    Vector2 xy = new Vector2(x, y);
                    Complex field = arr.values[i, j];
                    DataItem newItem = new DataItem(xy, field);
                    list.Add(newItem);
                }
            }
            return list;
        }
    }

    class V2MainCollection
    {
        private List<V2Data> V2DataList;
        public int Count
        {
            get => V2DataList.Count;
        }
        public V2Data this[int i]
        {
            get => V2DataList[i];
        }
        public bool Contains(string ID)
        {
            return V2DataList.Exists(x => x.id == ID);
        }
        public V2MainCollection()
        {
            V2DataList = new List<V2Data>();
        }
        public bool Add(V2Data v2Data)
        {
            if (Contains(v2Data.id))
            {
                return false;
            }
            else
            {
                V2DataList.Add(v2Data);
                return true;
            }
        }
        public string ToLongString(string format)
        {
            string str = "";
            foreach (var data in V2DataList)
            {
                str += "\n" + data.ToLongString(format);
            }
            return str;
        }
        public override string ToString()
        {
            string str = "";
            foreach (var data in V2DataList)
            {
                str += data.ToString() + "\n";
            }
            return str;
        }
    }

    public delegate Complex Fv2Complex(Vector2 v2);
}
