#pragma once

#include "pch.h"
#include "mkl.h"


extern "C" _declspec(dllexport) int Lab3CubicSpline(
	int nX, // число узлов сплайна
	const double* X, // массив узлов сплайна
	int nY, // размерность векторной функции
	const double* Y, // массив заданных значений векторной функции
	double d1L, // первая производная сплайна на левом конце
	double d1R, // первая производная сплайна на правом конце
	const int nS, // число узлов равномерной сетки, на которой
	// вычисляются значения сплайна и его производных
	double sL, // левый конец равномерной сетки
	double sR, // правый конец равномерной сетки
	double* splineValues, // массив вычисленных значений сплайна и производных
	double limitL, // левый конец отрезка интегрирования
	double limitR, // правый конец отрезка интегрирования
	double* integrals);
