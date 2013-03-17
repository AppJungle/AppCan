/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;


namespace RCLibrary
{
    /// <summary>
    /// RCLinkedListNode is used by RCLinkedList
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RCLinkedListNode<T>
    {
        public T Value { get; set; }
        public RCLinkedListNode<T> Next { get; internal set; }
        public RCLinkedListNode<T> Previous { get; internal set; }
        public RCLinkedList<T> List { get; internal set; }

        /*
        ~RCLinkedListNode()
            {
                int x = 0;
                x++;
            }*/

    }

    /// <summary>
    /// RCLinkedList class is a reduced collection class.  This class is designed to reduce the number of
    /// collections that happen by keeping a pool of RCLinkedListNode's available.  By setting the capacity a pool of nodes will be created.
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RCLinkedList<T>
    {
        private RCLinkedListNode<T> _head;
        private RCLinkedListNode<T> _tail;
        //private static IItemPool<RCLinkedListNode<T>> _staticPool = new StackPoolEx<RCLinkedListNode<T>>(DEFAULT_CAPACITY, false, false);

        public RCLinkedListNode<T> First { get { return _head; } }
        public RCLinkedListNode<T> Last { get { return _tail; } }

        IItemPool<RCLinkedListNode<T>> _pool;

        const uint DEFAULT_CAPACITY = 5;


        public RCLinkedList() : this(new StackPool<RCLinkedListNode<T>>(DEFAULT_CAPACITY, false, false))
        {
            //_pool = new StackPool<RCLinkedListNode<T>>(DEFAULT_CAPACITY, false, false);
        }

        public RCLinkedList(uint capacity) : this(new StackPool<RCLinkedListNode<T>>(capacity, false, false))
        {

        }

        /// <summary>
        /// Constructor for RCLinkedList passing a IItemPool of type RCLinkedListNode
        /// to be used for pooling nodes. For best results initialize the pool with the max capacity needed.
        /// </summary>
        /// <param name="pool"></param>
        public RCLinkedList(IItemPool<RCLinkedListNode<T>> pool)
        {
            _pool = pool;

        }

        /// <summary>
        /// Add a RCLinkedListNode to the front/beginning of the list
        /// </summary>
        /// <param name="node"></param>
        public void AddFirst(RCLinkedListNode<T> node)
        {
            if (_head == null)
            {
                _head = node;
                _tail = node;

                node.Previous = null;
                node.Next = null;

            }
            else
            {
                node.Next = _head;
                node.Previous = null;
                _head.Previous = node;
                _head = node;
            }
            

            if (_tail == null)
                _tail = node;

        }

        /// <summary>
        /// Add a value to the beginning/front of the list
        /// </summary>
        /// <param name="value"></param>
        public void AddFirst(T value)
        {
            RCLinkedListNode<T> node=_pool.Get();
            node.Value = value;
            AddFirst(node);

        }


        /// <summary>
        /// Add a RCLinkedListNode to the end of the list
        /// </summary>
        /// <param name="node"></param>
        public void AddLast(RCLinkedListNode<T> node)
        {
            if (_tail == null)
            {
                _tail = node;
                _head = node;

                node.Previous = null;
                node.Next = null;

            }
            else
            {
                node.Previous = _tail;
                node.Next = null;
                _tail = node;
            }


            if (_head == null)
                _head = node;

        }

        /// <summary>
        /// Add a value to the end of the list
        /// </summary>
        /// <param name="value"></param>
        public void AddLast(T value)
        {
            RCLinkedListNode<T> node = _pool.Get();
            node.Value = value;
            AddLast(node);

        }


        /// <summary>
        ///  Remove the first item from the list
        /// </summary>
        public void RemoveFirst()
        {
            if (_head == null)
            {
                throw new InvalidOperationException("error: RCLinkedList<T> RemoveFirst called, list is empty.");
                
            }

            RCLinkedListNode<T> first = _head;

            if (_tail == _head && _head!=null)
            {
                _tail = null;
                _head = null;
            }

            if (_head != null)
            {
                _head = _head.Next;
                _head.Previous = null;
            }

            if (first!=null)
                _pool.Release(first);

        }


        /// <summary>
        /// Remoe the last item from the list
        /// </summary>
        public void RemoveLast()
        {
            if (_tail == null)
            {
                throw new InvalidOperationException("error: RCLinkedList<T> RemoveFirst called, list is empty.");

            }

            RCLinkedListNode<T> last = _tail;

            if (_tail == _head && _tail != null)
            {
                _tail = null;
                _head = null;
            }

            if (_tail != null)
            {
                _tail = _tail.Previous;
                _tail.Next = null;
            }

            if (last != null)
                _pool.Release(last);

        }


        /// <summary>
        /// Finds an item in the linked list.  Pass in a lambda expression that takes a type of the item in the linked list and compares it with what you want to find 
        /// lambda should return true if found, false otherwise
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public T Find(Func<T,bool> property) 
        {
            RCLinkedListNode<T> current = _head;
            while (current != null)
            {
                
                bool found = (bool)property(current.Value);
                if (found)
                    return current.Value;

                current = current.Next;
            }

            return default(T);
        }

        /// <summary>
        /// Remove all elements from the list, nodes are returned to the free pool
        /// </summary>
        public void Clear()
        {
            while (First != null)
            {
                RemoveFirst();
            }

        }


        /// <summary>
        /// Remove the item from the linked list.  If the item is in the list more than once it removes the first one with the value
        /// </summary>
        /// <param name="value">The value to find in the list to be removed</param>
        /// <returns>true if found to remove, false otherwise</returns>
        
        public bool Remove(T value)
        {
          EqualityComparer<T> c = EqualityComparer<T>.Default;
            RCLinkedListNode<T> current = _head;
            while (current != null)
            {
                if (c.Equals(value,current.Value))
                {
                    RCLinkedListNode<T> previous = current.Previous;
                    RCLinkedListNode<T> next = current.Next;

                    if (previous == null && next==null)
                    {
                        //This is the head and tail
                        RemoveFirst();

                        return true;

                    }

                    if (previous == null)
                    {
                        //this is the head but not the tail
                        RemoveFirst();
                        return true;

                    }

                    if (next == null)
                    {
                        //this is the tail
                        RemoveLast();
                        return true;

                    }

                    previous.Next = next;
                    next.Previous = previous;

                }
                

                current = current.Next;
            }

            return false;
        } 

    }
}
