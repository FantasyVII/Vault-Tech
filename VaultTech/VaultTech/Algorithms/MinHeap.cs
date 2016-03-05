/*
 * <Copyright>
 * Owned by:- Vault 16 Software
 * Author:- Mustafa Al-Sibai
 * Date Created:- 9/April/2015
 * Date Moddified :- 11/April/2015
 * </Copyright>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultTech.Algorithms
{
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }
    }

    class MinHeap<T> where T : IHeapItem<T>
    {
        T[] Items;
        public int Count { get; private set; }

        public MinHeap(int MaxSize)
        {
            Items = new T[MaxSize];
        }

        public T this[int Index]
        {
            get
            {
                if (Index > Count)
                    Index = Count - 1;

                return Items[Index];
            }

            set { Items[Index] = value; }
        }

        public void Push(T Item)
        {
            Item.HeapIndex = Count;
            Items[Count] = Item;
            SortUp(Item);
            Count++;
        }

        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        public T Pop()
        {
            T FirstItem = Items[0];
            Count--;
            Items[0] = Items[Count];
            Items[0].HeapIndex = 0;
            SortDown(Items[0]);
            return FirstItem;
        }

        void SortDown(T Item)
        {
            while (true)
            {
                int LeftChildIndex = Item.HeapIndex * 2 + 1;
                int RightChildIndex = Item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (LeftChildIndex < Count)
                {
                    swapIndex = LeftChildIndex;

                    if (RightChildIndex < Count)
                        if (Items[LeftChildIndex].CompareTo(Items[RightChildIndex]) < 0)
                            swapIndex = RightChildIndex;

                    if (Item.CompareTo(Items[swapIndex]) < 0)
                        Swap(Item, Items[swapIndex]);
                    else
                        return;
                }
                else
                    return;
            }
        }

        void SortUp(T Item)
        {
            int ParentIndex = (Item.HeapIndex - 1) / 2;

            while (true)
            {
                T ParentItem = Items[ParentIndex];
                if (Item.CompareTo(ParentItem) > 0)
                    Swap(Item, ParentItem);
                else
                    break;

                ParentIndex = (Item.HeapIndex - 1) / 2;
            }
        }

        void Swap(T ItemA, T ItemB)
        {
            Items[ItemA.HeapIndex] = ItemB;
            Items[ItemB.HeapIndex] = ItemA;
            int ItemAIndex = ItemA.HeapIndex;
            ItemA.HeapIndex = ItemB.HeapIndex;
            ItemB.HeapIndex = ItemAIndex;
        }
    }
}