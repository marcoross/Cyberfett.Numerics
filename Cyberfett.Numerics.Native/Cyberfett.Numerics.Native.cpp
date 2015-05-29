// Cyberfett.Numerics.Native.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "Cyberfett.Numerics.Native.h"

Cyberfett::Numerics::Native::AbstractVector::AbstractVector(const int length) 
{
	this->length = length;
}

int Cyberfett::Numerics::Native::AbstractVector::getLength()
{
	return length;
}

void Cyberfett::Numerics::Native::AbstractVector::CheckIdx(const int idx)
{
	if (idx < 0 || idx >= length) throw OUT_OF_BOUNDS;
}


template <class T> Cyberfett::Numerics::Native::Vector<T>::Vector(const int length) : AbstractVector(length)
{
	data = new T[length];
	refCount = new int[1];
	*refCount = 1;
}
template <class T> Cyberfett::Numerics::Native::Vector<T>::Vector(T* data, int* refCount, const int from, const int length) : AbstractVector(length)
{
	CheckIdx(from);
	CheckIdx(from + length - 1);
	this->data = data + from;
	this->refCount = refCount;
	*refCount += 1;
}

template <class T> Cyberfett::Numerics::Native::Vector<T>::~Vector()
{
	dispose();
}

template <class T> void Cyberfett::Numerics::Native::Vector<T>::dispose()
{
	if (*refCount > 1)
	{
		*refCount -= 1;
		data = NULL;
		refCount = NULL;
	}
	else
	{
		delete[] data;
		delete[] refCount;
		data = NULL;
		refCount = NULL;
	}
}

template <class T> int Cyberfett::Numerics::Native::Vector<T>::getRefCount()
{
	return *refCount;
}

template <class T> void Cyberfett::Numerics::Native::Vector<T>::lazyCopy()
{
	if (*refCount > 1)
	{
		T* orig = data;
		data = new T[length];
		memcpy(data, orig, sizeof(T) * length);

		int* oldRefCount = refCount;
		refCount = new int[1];
		*refCount = 1;
		*oldRefCount -= 1;
	}
}

template <class T> void Cyberfett::Numerics::Native::Vector<T>::setValue(const int idx, void* value)
{
	lazyCopy();
	CheckIdx(idx);
	T* cast = (T*)value;
	data[idx] = *cast;
}

template <class T> void Cyberfett::Numerics::Native::Vector<T>::getValue(const int idx, void* value)
{
	CheckIdx(idx);
	T* cast = (T*)value;
	*cast = data[idx];
}

template <class T> Cyberfett::Numerics::Native::Vector<T>* Cyberfett::Numerics::Native::Vector<T>::cloneRange(const int from, const int length)
{
	return new Vector<T>(data, refCount, from, length);
}

template CYBERFETTNUMERICSNATIVE_API class Cyberfett::Numerics::Native::Vector<char>;
template CYBERFETTNUMERICSNATIVE_API class Cyberfett::Numerics::Native::Vector<short>;
template CYBERFETTNUMERICSNATIVE_API class Cyberfett::Numerics::Native::Vector<int>;
template CYBERFETTNUMERICSNATIVE_API class Cyberfett::Numerics::Native::Vector<long>;
template CYBERFETTNUMERICSNATIVE_API class Cyberfett::Numerics::Native::Vector<float>;
template CYBERFETTNUMERICSNATIVE_API class Cyberfett::Numerics::Native::Vector<double>;