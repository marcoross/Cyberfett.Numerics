using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Cyberfett.Numerics;
using ILNumerics;

namespace Test.Cyberfett.Numerics
{
    [TestFixture]
    public class TestVector
    {

        [Test]
        public void CreateByteVector()
        {
            using (var v = new RowVector<byte>(5))
            {
                Assert.That(v, Is.Not.Null);
                Assert.That(v.Length, Is.EqualTo(5));
                Assert.That(v.RefCount, Is.EqualTo(1));
            }
        }

        [Test]
        public void CreateShortVector()
        {
            using (var v = new RowVector<short>(5))
            {
                Assert.That(v, Is.Not.Null);
            }
        }

        [Test]
        public void CreateIntVector()
        {
            using (var v = new RowVector<int>(5))
            {
                Assert.That(v, Is.Not.Null);
            }
        }

        [Test]
        public void CreateLongVector()
        {
            using (var v = new RowVector<long>(5))
            {
                Assert.That(v, Is.Not.Null);
            }
        }

        [Test]
        public void CreateFloatVector()
        {
            using (var v = new RowVector<float>(5))
            {
                Assert.That(v, Is.Not.Null);
            }
        }

        [Test]
        public void CreateDoubleVector()
        {
            using (var v = new RowVector<double>(5))
            {
                Assert.That(v, Is.Not.Null);
            }
        }

        [Test]
        public void CreateCustomStructVector()
        {
            Assert.Throws<Exception>(() => new RowVector<mystruct>(8));
        }

        [Test]
        public void SetGetDouble()
        {
            using (var v = new RowVector<double>(10))
            {
                v[5] = 4.89;
                v[0] = 1.23;
                Assert.That(v[5], Is.EqualTo(4.89));
                Assert.That(v[0], Is.EqualTo(1.23));
                Assert.That(v.RefCount, Is.EqualTo(1));
            }
        }

        [Test]
        public void SetGetByte()
        {
            using (var v = new RowVector<byte>(12))
            {
                v[11] = 0xF3;
                v[5] = 0x81;
                Assert.That(v[11], Is.EqualTo(0xF3));
                Assert.That(v[5], Is.EqualTo(0x81));
            }
        }

        [Test]
        public void GetSetNegativeIndex()
        {
            using (var v = new RowVector<int>(12))
            {
                Assert.Throws<Exception>(() => { v[-5] = 3; });
                Assert.Throws<Exception>(() => { var a = v[-1]; });
            }
        }

        [Test]
        public void GetSetTooHighIndex()
        {
            using (var v = new RowVector<int>(12))
            {
                Assert.Throws<Exception>(() => { v[12] = 3; });
                Assert.Throws<Exception>(() => { var a = v[101]; });
            }
        }

        [Test]
        public void TransposeRowVector()
        {
            using (var v = new RowVector<int>(12))
            {
                v[3] = 3;
                v[5] = 5;
                var c = v.T;
                Assert.That(c, Is.InstanceOf<ColumnVector<int>>());
                Assert.That(c.Length, Is.EqualTo(12));
                Assert.That(c[3], Is.EqualTo(3));
                Assert.That(c[5], Is.EqualTo(5));
            }
        }

        [Test]
        public void TransposeColumnVector()
        {
            using (var v = new ColumnVector<int>(12))
            {
                v[3] = 3;
                v[5] = 5;
                var c = v.T;
                Assert.That(c, Is.InstanceOf<RowVector<int>>());
                Assert.That(c.Length, Is.EqualTo(12));
                Assert.That(c[3], Is.EqualTo(3));
                Assert.That(c[5], Is.EqualTo(5));
            }
        }

        [Test]
        public void DisposeTwice()
        {
            using (var v = new ColumnVector<int>(3))
            {
                v.Dispose();
                Assert.DoesNotThrow(() => v.Dispose());
            }
        }

        [Test]
        public void DoNotDisposeClone()
        {
            using (var v = new ColumnVector<int>(3))
            {
                Assert.That(v.RefCount, Is.EqualTo(1));
                using (var t = v.T)
                {
                    Assert.That(v.RefCount, Is.EqualTo(2));
                    Assert.That(t.RefCount, Is.EqualTo(2));

                    v.Dispose();
                    Assert.That(t.RefCount, Is.EqualTo(1));

                    Assert.Throws<NullReferenceException>(() => { v[2] = 3; });
                    Assert.DoesNotThrow(() => { t[2] = 3; });
                }
            }
        }

        [Test]
        public void DoNotDisposeOriginal()
        {
            using (var v = new ColumnVector<int>(3))
            {
                Assert.That(v.RefCount, Is.EqualTo(1));
                using (var t = v.T)
                {
                    Assert.That(v.RefCount, Is.EqualTo(2));
                    Assert.That(t.RefCount, Is.EqualTo(2));

                    t.Dispose();
                    Assert.That(v.RefCount, Is.EqualTo(1));

                    Assert.DoesNotThrow(() => { v[2] = 3; });
                    Assert.Throws<NullReferenceException>(() => { t[2] = 3; });
                }
            }
        }

        [Test]
        public void DoNotOverwriteDataInClone()
        {
            using (var v = new ColumnVector<int>(3))
            {
                v[0] = 1;
                v[1] = 2;
                v[2] = 3;
                using (var t = v.T)
                {
                    Assume.That(v.RefCount, Is.EqualTo(2));
                    Assume.That(t.RefCount, Is.EqualTo(2));
                    v[1] = 5;
                    Assert.That(v.RefCount, Is.EqualTo(1));
                    Assert.That(t.RefCount, Is.EqualTo(1));

                    Assume.That(v[0], Is.EqualTo(1));
                    Assume.That(v[1], Is.EqualTo(5));
                    Assume.That(v[2], Is.EqualTo(3));
                    Assert.That(t[0], Is.EqualTo(1));
                    Assert.That(t[1], Is.EqualTo(2));
                    Assert.That(t[2], Is.EqualTo(3));
                }
            }
        }

        [Test]
        public void DoNotOverwriteDataInOriginal()
        {
            using (var v = new ColumnVector<int>(3))
            {
                v[0] = 1;
                v[1] = 2;
                v[2] = 3;
                using (var t = v.T)
                {
                    Assume.That(v.RefCount, Is.EqualTo(2));
                    Assume.That(t.RefCount, Is.EqualTo(2));
                    t[1] = 5;
                    Assert.That(v.RefCount, Is.EqualTo(1));
                    Assert.That(t.RefCount, Is.EqualTo(1));

                    Assume.That(t[0], Is.EqualTo(1));
                    Assume.That(t[1], Is.EqualTo(5));
                    Assume.That(t[2], Is.EqualTo(3));
                    Assert.That(v[0], Is.EqualTo(1));
                    Assert.That(v[1], Is.EqualTo(2));
                    Assert.That(v[2], Is.EqualTo(3));
                }
            }
        }

        [Ignore]
        [Test]
        public void ManyVectorsInLoop()
        {
            for (int i = 0; i < 100000; i++ )
            {
                using (var oneMB = new ColumnVector<byte>(1024 * 1024))
                {
                    oneMB[13] = (byte)i;
                }
            }
        }

        [Ignore]
        [Test]
        public void ManyVectorsInLoopNoDispose()
        {
            for (int i = 0; i < 100000; i++)
            {
                var oneMB = new ColumnVector<byte>(1024 * 1024);
                oneMB[13] = (byte)i;
                System.GC.Collect();
            }
        }

        [Ignore]
        [Test]
        public void CompareArrayInitializationTimes()
        {
            var watch = new System.Diagnostics.Stopwatch();
            var size = 1024 * 1024;

            watch.Start();
            for (int i = 0; i < 100; i++)
            {
                using (var oneMB = new ColumnVector<byte>(size))
                {
                    //oneMB[13] = (byte)i;
                }
            }
            watch.Stop();
            var myTime = watch.ElapsedTicks;

            watch.Reset();
            watch.Start();
            for (int i = 0; i < 100; i++)
            {
                var oneMB = new byte[size];
                //oneMB[13] = (byte)i;
            }
            watch.Stop();
            var theirTime = watch.ElapsedTicks;

            watch.Reset();
            watch.Start();
            for (int i = 0; i < 100; i++)
            {
                using (ILScope.Enter())
                {
                    ILArray<byte> oneMB = ILMath.New<byte>(size);
                    //oneMB[13] = (byte)i;
                }
            }
            watch.Stop();
            var ilTime = watch.ElapsedTicks;

            Assert.That(myTime, Is.LessThanOrEqualTo(ilTime));
            Assert.That(myTime, Is.LessThanOrEqualTo(theirTime));

        }

    }

    struct mystruct {
        int a;
        double b;
    }

}
