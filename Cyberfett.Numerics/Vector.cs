using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLR = Cyberfett.Numerics.CLR;

namespace Cyberfett.Numerics
{
    public abstract class Vector<D, VectorType, TransposedType> : CLR.Vector<D>, IDisposable
        where D : struct
        where VectorType : Vector<D, VectorType, TransposedType>
        where TransposedType : Vector<D, TransposedType, VectorType>
    {
        public Vector(int length) : base(length)
        { }

        protected Vector(Vector<D, VectorType, TransposedType> v) 
            : base(v, 0, v.Length)
        { }
        protected Vector(Vector<D, TransposedType, VectorType> v)
            : base(v, 0, v.Length)
        { }

        ~Vector()
        {
            Dispose();
        }

        public D this[int idx] {
            get { return Get(idx); }
            set { Set(idx, value); }
        }

        public int Length {
            get { return GetLength(); }
        }

        public int RefCount
        {
            get { return GetRefCount(); }
        }

        public abstract TransposedType T { get; }

    }

    public class ColumnVector<D> : Vector<D, ColumnVector<D>, RowVector<D>>, IDisposable
        where D : struct
    {
        public ColumnVector(int length) : base(length) 
        { }

        internal ColumnVector(Vector<D, RowVector<D>, ColumnVector<D>> v) : base(v)
        { }
        internal ColumnVector(Vector<D, ColumnVector<D>, RowVector<D>> v)
            : base(v)
        { }

        override public RowVector<D> T 
        {
            get 
            {
                return new RowVector<D>(this);
            }
        } 

    }

    public class RowVector<D> : Vector<D, RowVector<D>, ColumnVector<D>>, IDisposable
        where D : struct
    {
        public RowVector(int length) : base(length)
        { }

        internal RowVector(Vector<D, RowVector<D>, ColumnVector<D>> v) : base(v)
        { }
        internal RowVector(Vector<D, ColumnVector<D>, RowVector<D>> v)
            : base(v)
        { }

        override public ColumnVector<D> T
        {
            get
            {
                return new ColumnVector<D>(this);
            }
        } 
    }

}
