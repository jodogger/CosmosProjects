using System.Collections.Generic;

namespace Kernel.Extentions
{
    public static class ListExtentions
    {
        public static List<T> RemoveAtX<T>(this List<T> list, int index)
        {
            List<T> newList = new List<T>();
            int i = 0;

            for (; i < index; i++)
                newList.Add(list[i]);

            i++;

            for (; i < list.Count; i++)
                newList.Add(list[i]);

            return newList;
        }

        public static List<T> InsertAtX<T>(this List<T> list, int index, T item)
        {
            List<T> newList = new List<T>();
            int i = 0;

            if (index < list.Count)
            {
                for (; i < index; i++)
                    newList.Add(list[i]);
            }

            newList.Add(item);

            for (; i < list.Count; i++)
                newList.Add(list[i]);

            return newList;
        }
    }
}
