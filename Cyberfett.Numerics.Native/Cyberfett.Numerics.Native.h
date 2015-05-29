// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the CYBERFETTNUMERICSNATIVE_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// CYBERFETTNUMERICSNATIVE_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef CYBERFETTNUMERICSNATIVE_EXPORTS
#define CYBERFETTNUMERICSNATIVE_API __declspec(dllexport)
#else
#define CYBERFETTNUMERICSNATIVE_API __declspec(dllimport)
#endif
const int NOT_IMPLEMENTED = 10;
const int OUT_OF_BOUNDS = 11;

namespace Cyberfett
{
	namespace Numerics
	{
		namespace Native
		{

			class CYBERFETTNUMERICSNATIVE_API AbstractVector
			{
			public:
				AbstractVector(const int);
				int getLength();
				virtual void setValue(const int, void*) { throw NOT_IMPLEMENTED; };
				virtual void getValue(const int, void*) { throw NOT_IMPLEMENTED; };
				virtual AbstractVector* cloneRange(const int, const int) { throw NOT_IMPLEMENTED; };
				virtual int getRefCount() { throw NOT_IMPLEMENTED; };
				virtual void dispose() { throw NOT_IMPLEMENTED; };
			protected:
				void CheckIdx(const int);
				int length;
			};

			template <class T> class CYBERFETTNUMERICSNATIVE_API Vector : public AbstractVector
			{
			public:
				Vector(const int);
				~Vector();
				void setValue(const int, void*);
				void getValue(const int, void*);
				Vector<T>* cloneRange(const int, const int);
				int getRefCount();
				void dispose();
			private:
				Vector(T* data, int*, const int, const int);
				T* data;
				int* refCount;
				void lazyCopy();
			};

		}
	}
}
