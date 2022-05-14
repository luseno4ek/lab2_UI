// pch.cpp: файл исходного кода, соответствующий предварительно скомпилированному заголовочному файлу

#include "pch.h"

// При использовании предварительно скомпилированных заголовочных файлов необходим следующий файл исходного кода для выполнения сборки.

extern "C" _declspec(dllexport) double 
InterpolateMKL(const int length1, const int length2, const double* points, 
				const double* func, const double* derivatives1, const double* derivatives2, double* res1, double* res2)
{
	// переменная для задачи интерполяции первого сплайна
	DFTaskPtr task1;
	// переменная для задачи интерполяции второго сплайна
	DFTaskPtr task2;
	// максимальный порядок вычисляемой производной + 1 
	int norder = 3;
	// массив для хранения коэффициентов первого сплайна
	double* spline_coeff1 = new double[(length1 - 1) * DF_PP_CUBIC];
	// массив для хранения коэффициентов второго сплайна
	double* spline_coeff2 = new double[(length1 - 1) * DF_PP_CUBIC];
	// массив, определяющий, что будут вычислены только значения сплайнов и вторых производных
	MKL_INT dorder[] = { 1 , 0 , 1};
	// интервал, на котором по равномерной сетке вычисляются значения сплайнов
	double interval[] = { points[0],  points[length1 - 1] };

	// создание задач интерполяции
	int status1 = dfdNewTask1D(&task1, length1, points, DF_NON_UNIFORM_PARTITION, 1, func, DF_NO_HINT);
	int status2 = dfdNewTask1D(&task2, length1, points, DF_NON_UNIFORM_PARTITION, 1, func, DF_NO_HINT);
	if ((status1 != DF_STATUS_OK) | (status2 != DF_STATUS_OK))
	{
		return status1 - 0.1;
	}
	// конфигурация задач -- задание граничных условий вторых производных
	status1 = dfdEditPPSpline1D(task1, DF_PP_CUBIC, DF_PP_NATURAL, 
		DF_BC_2ND_LEFT_DER | DF_BC_2ND_RIGHT_DER, derivatives1, DF_NO_IC, NULL, spline_coeff1, DF_NO_HINT);
	status2 = dfdEditPPSpline1D(task2, DF_PP_CUBIC, DF_PP_NATURAL,
		DF_BC_2ND_LEFT_DER | DF_BC_2ND_RIGHT_DER, derivatives2, DF_NO_IC, NULL, spline_coeff2, DF_NO_HINT);
	if ((status1 != DF_STATUS_OK) | (status2 != DF_STATUS_OK))
	{
		return status1 - 0.2;
	}
	// построение сплайнов
	status1 = dfdConstruct1D(task1, DF_PP_SPLINE, DF_METHOD_STD);
	status2 = dfdConstruct1D(task2, DF_PP_SPLINE, DF_METHOD_STD);
	if ((status1 != DF_STATUS_OK) | (status2 != DF_STATUS_OK))
	{
		return status1 - 0.3;
	}
	// вычисление значений сплайнов и их производных
	status1 = dfdInterpolate1D(task1, DF_INTERP, DF_METHOD_PP, length2, interval, 
		DF_UNIFORM_PARTITION, norder, dorder, NULL, res1, DF_NO_HINT, NULL);
	status2 = dfdInterpolate1D(task2, DF_INTERP, DF_METHOD_PP, length2, interval,
		DF_UNIFORM_PARTITION, norder, dorder, NULL, res2, DF_NO_HINT, NULL);
	if ((status1 != DF_STATUS_OK) | (status2 != DF_STATUS_OK))
	{
		return status1 - 0.4;
	}
	// удаление задач
	status1 = dfDeleteTask(&task1);
	status2 = dfDeleteTask(&task2);
	if ((status1 != DF_STATUS_OK) | (status2 != DF_STATUS_OK))
	{
		return status1 - 0.5;
	}
	return status1;
}