// Cyberfett.Numerics.CLR.h

#pragma once
#include "..\Cyberfett.Numerics.Native\Cyberfett.Numerics.Native.h"

using namespace System;

namespace Cyberfett {
	namespace Numerics {
		namespace CLR {

			generic <class T>
				public ref class Vector
				{
				public:
					Vector(const int);
					~Vector();
				protected:
					Vector(Vector<T>^ other, const int from, const int length);
					int GetLength();
					T Get(const int);
					void Set(const int, T);
					int GetRefCount();
				private:
					Native::AbstractVector* nv;
				};
		}
	}
}
