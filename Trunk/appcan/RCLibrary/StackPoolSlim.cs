/*(c)opyright 2013 b6d27bbb0f9ffeafa084af169e90610f767bbf5a
If you modify and distribute source files please be sure to indicate that they have been modified.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RCLibrary
{
    /// <summary>
    /// Stack pool Slim - future improvement ideas... 
    /// reference count to catch double release
    /// finalizable object support with internal release to totally avoid double release
    /// This is NOT thread safe, locking/lock free must be used around calls to this pool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StackPoolSlim<T> : IItemPool<T> where T : new()
    {
        #region Private Variables
        //LinkedList<T> _stack=null;
        const int DEFAULT_STACK_ALLOC = 25;
        LNode _freeHead = null;
        LNode _listHead = null;
        bool _disposeOnRelease = false;
        


        #endregion Private Variables

        public StackPoolSlim()
            : this(25, true)
        {

        }

        public StackPoolSlim(uint capacity)
            : this(capacity, false)
        {



        }


        public StackPoolSlim(uint capacity, bool disposeOnRelease)
        {
            _disposeOnRelease = disposeOnRelease;
            Alloc(capacity);

        }

        public T Get()
        {
            if (_listHead == null)
            {
                Alloc(DEFAULT_STACK_ALLOC);

            }

            LNode node = _listHead;
            _listHead = node.Next;

            T value = node.Value;
            FreeNode(node);

            return value;
        }

        private void Release(T item, bool noDispose)
        {
            if (_listHead == null)
            {
                _listHead = GetNode(item);
                _listHead.Next = null;
            }
            else
            {
                LNode node = GetNode(item);
                node.Next = _listHead;
                _listHead = node;
            }
        }

        public void Release(T item)
        {
            Release(item, false);
        }

        #region Private Methods
        private void Alloc(uint size)
        {
            for (int x = 0; x < size; x++)
            {
                T newItem = new T();
                Release(newItem, true);

            }
        }

        private LNode GetNode(T value)
        {
            if (_freeHead == null)
                return new LNode(value);

            LNode node = _freeHead;
            _freeHead = node.Next;

            node.Value = value;

            return node;
        }

        private void FreeNode(LNode node)
        {
            node.Value = default(T);
            node.Next = _freeHead;
            _freeHead = node;
            return;
        }
        #endregion Private Methods

        #region LNode class
        protected class LNode
        {
            public T Value;
            public LNode Next;

            public LNode(T value)
            {
                Value = value;
                Next = null;
            }

            /*~LNode()
            {
                int x = 0;
                x++;

            }*/


        }
        #endregion LNode class
    }

}
