#pragma once
#include <dense>
#include "Vector3d.h"

namespace ModusAdjustCylinder_Math {
	public ref class AngleAxisd
	{
	protected:
		Eigen::AngleAxisd* m_angleAxis = nullptr;



	public:
		AngleAxisd(double angle, Vector3d^ vector);

		static Vector3d^ operator * (AngleAxisd^ matrix, Vector3d^ vector);
		



	};
}

