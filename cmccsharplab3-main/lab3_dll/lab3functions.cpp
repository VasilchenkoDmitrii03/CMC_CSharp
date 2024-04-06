﻿#include "pch.h"
#include <iostream>
#include <time.h>
#include <math.h>
#include "mkl.h"
#include "lab3functons.h"

struct params
{
	const double* uniformX = NULL;
	const double* X = NULL;
	const double* Y = NULL;
};

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
	double* integrals) // массив значений интегралов
{
	MKL_INT s_order = DF_PP_CUBIC; // степень кубического сплайна
	MKL_INT s_type = DF_PP_NATURAL; // тип сплайна
	// тип граничных условий - первая производная на обоих концах
	MKL_INT bc_type = DF_BC_1ST_LEFT_DER | DF_BC_1ST_RIGHT_DER;
	// массив для коэффициентов сплайна
	double* scoeff = new double[nY * (nX - 1) * s_order];
	try
	{
		DFTaskPtr task;
		int status = -1;
		// Cоздание задачи (task)
		status = dfdNewTask1D(&task,
			nX, X,
			DF_NON_UNIFORM_PARTITION, // неравномерная сетка узлов
			nY, Y,
			DF_NO_HINT); // формат хранения значений векторной
		// функции по умолчанию (построчно)
		if (status != DF_STATUS_OK) throw 1;
		// Настройка параметров задачи
		double bc[2]{ d1L, d1R }; // массив граничных значений
		status = dfdEditPPSpline1D(task,
			s_order, s_type, bc_type, bc,
			DF_NO_IC, // тип условий во внутренних точках
			NULL, // массив значений для условий во внутренних точках
			scoeff,
			DF_NO_HINT); // формат упаковки коэффициентов сплайна
		// в одномерный массив (Row-major - построчно)
		if (status != DF_STATUS_OK) throw 2;
		// Создание сплайна
		status = dfdConstruct1D(task,
			DF_PP_SPLINE, // поддерживается только одно значение
			DF_METHOD_STD); // поддерживается только одно значение
		if (status != DF_STATUS_OK) throw 3;
		// Вычисление значений сплайна и его производных
		double grid[2]{ sL, sR };// массив концов равномерной сетки, на которой
		// вычисляются значения сплайна и производных
		int nDorder = 3; // число производных, которые вычисляются, плюс 1
		MKL_INT dorder[] = { 1, 1, 1 }; // вычисляются значения сплайна,
		// его первая и вторая производные
		status = dfdInterpolate1D(task,
			DF_INTERP, // вычисляются значения сплайна и его производных
			DF_METHOD_PP, // поддерживается только одно значение
			nS, grid,
			DF_UNIFORM_PARTITION, // значения сплайна и его производных
			// вычисляются на равномерной сетке
			nDorder, dorder,
			NULL, // нет дополнительной информации об узлах интерполяции
			splineValues,
			DF_NO_HINT, // формат упаковки результатов в одномерный массив
			NULL); // используется для ускорения вычислений на
		// неравномерной сетке; можно присвоить значение
		if (status != DF_STATUS_OK) throw 4;
		// Вычисление интеграла
		const int nSegm = 1; // число отрезков интегрирования
		double lEnds[nSegm]{ limitL }; // массив левых концов
		// отрезков интегрирования
		double rEnds[nSegm]{ limitR }; // массив правых концов
		// отрезков интегрирования
		status = dfdIntegrate1D(task,
			DF_METHOD_PP, // поддерживается только одно значение
			nSegm,
			lEnds,
			DF_NO_HINT, // не используется информация о массиве
			// левых концов отрезков интегрирования
			rEnds,
			DF_NO_HINT, // не используется информация о массиве
			// правых концов отрезков интегрирования
			NULL, // дополнительная информация о массиве левых концов
			// отрезков интегрирования
			NULL, // дополнительная информация о массиве правых концов
			// отрезков интегрирования
			integrals,
			DF_NO_HINT);// формат упаковки значений интегралов в массив
		if (status != DF_STATUS_OK) throw 5;
		// Освобождение ресурсов
		status = dfDeleteTask(&task);
		if (status != DF_STATUS_OK) throw 6;
	}
	catch (int ret)
	{
		delete[] scoeff;
		return ret;
	}
	delete[] scoeff;
	return 0;
}

