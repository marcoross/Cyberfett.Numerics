// This is the main DLL file.

#include "stdafx.h"

#include "Cyberfett.Numerics.CLR.h"

generic <class T> Cyberfett::Numerics::CLR::Vector<T>::Vector(const int length)
{
	Type^ dataType = GetType()->GetGenericArguments()[0];
	if (dataType == Byte::typeid) {
		nv = new Native::Vector<char>(length);
	}
	else if (dataType == short::typeid) {
		nv = new Native::Vector<short>(length);
	}
	else if (dataType == int::typeid) {
		nv = new Native::Vector<int>(length);
	}
	else if (dataType == Int64::typeid) {
		nv = new Native::Vector<long>(length);
	}
	else if (dataType == float::typeid) {
		nv = new Native::Vector<float>(length);
	}
	else if (dataType == double::typeid) {
		nv = new Native::Vector<double>(length);
	}
	else {
		throw gcnew Exception("Invalid Vector type: " + dataType->Name);
	}
	
}

generic <class T> Cyberfett::Numerics::CLR::Vector<T>::Vector(Vector<T>^ other, const int from, const int length)
{
	try
	{
		nv = other->nv->cloneRange(from, length);
	}
	catch (int error)
	{
		switch (error)
		{
		case OUT_OF_BOUNDS: throw gcnew Exception("Index was out of bounds");
		default: throw error;
		}
	}
}

generic <class T> Cyberfett::Numerics::CLR::Vector<T>::~Vector()
{
	if (nv != nullptr)
	{
		nv->dispose();
		delete[] nv;
		nv = nullptr;
	}
}

generic <class T> int Cyberfett::Numerics::CLR::Vector<T>::GetRefCount()
{
	return nv->getRefCount();
}

generic <class T> int Cyberfett::Numerics::CLR::Vector<T>::GetLength()
{
	return nv->getLength();
}

generic <class T> T Cyberfett::Numerics::CLR::Vector<T>::Get(const int idx)
{
	try 
	{
		T val = T();
		nv->getValue(idx, &val);
		return val;
	}
	catch (int error)
	{
		switch (error)
		{
		case OUT_OF_BOUNDS: throw gcnew Exception("Index " + idx + " was out of bounds");
		default: throw error;
		}
	}
}

generic <class T> void Cyberfett::Numerics::CLR::Vector<T>::Set(const int idx, T value)
{
	try 
	{
		nv->setValue(idx, &value);
	}
	catch (int error)
	{
		switch (error)
		{
		case OUT_OF_BOUNDS: throw gcnew Exception("Index " + idx + " was out of bounds");
		default: throw error;
		}
	}
}