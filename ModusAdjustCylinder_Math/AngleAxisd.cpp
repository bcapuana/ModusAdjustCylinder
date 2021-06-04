#include "pch.h"
#include "AngleAxisd.h"

ModusAdjustCylinder_Math::AngleAxisd::AngleAxisd(double angle, Vector3d^ vector)
{
	m_angleAxis = new Eigen::AngleAxisd(angle, vector->Vector);
}

ModusAdjustCylinder_Math::Vector3d^ ModusAdjustCylinder_Math::AngleAxisd::operator*(AngleAxisd^ matrix, Vector3d^ vector)
{
	return gcnew Vector3d(*matrix->m_angleAxis * vector->Vector);
}
