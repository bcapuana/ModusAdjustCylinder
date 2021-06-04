#include "pch.h"
#include "Vector3d.h"

ModusAdjustCylinder_Math::Vector3d::Vector3d()
{
    m_vector = new Eigen::Vector3d(0.0,0.0,0.0);
}

ModusAdjustCylinder_Math::Vector3d::Vector3d(double x, double y, double z)
{
    m_vector = new Eigen::Vector3d(x, y, z);
}

ModusAdjustCylinder_Math::Vector3d::Vector3d(array<double>^ xyz)
{
    if (xyz->Length != 3) throw gcnew System::Exception("The lenght of the array must be 3");
    double x = xyz[0];
    double y = xyz[1];
    double z = xyz[2];
    m_vector = new Eigen::Vector3d(x,y,z);
}

ModusAdjustCylinder_Math::Vector3d::Vector3d(double* xyz)
{
    int length = *(&xyz + 1) - xyz;
    if (length != 3)throw gcnew System::Exception("The lenght of the array must be 3");
    m_vector = new Eigen::Vector3d(xyz[0], xyz[1], xyz[2]);
}

ModusAdjustCylinder_Math::Vector3d::Vector3d(Eigen::Vector3d vec)
{
    m_vector = new Eigen::Vector3d(vec);
}

array<double>^ ModusAdjustCylinder_Math::Vector3d::ToArray()
{
    array<double>^ output = gcnew array<double>(3);
    Eigen::Vector3d v = Vector;
    for (int i = 0; i < 3; i++) {
        output[i] = v[i];
    }
    return output;
}

double ModusAdjustCylinder_Math::Vector3d::operator*(Vector3d^ v1, Vector3d^ v2)
{
    return v1->m_vector->dot(*(v2->m_vector));
}

ModusAdjustCylinder_Math::Vector3d^ ModusAdjustCylinder_Math::Vector3d::operator^(Vector3d^ v1, Vector3d^ v2)
{
    return gcnew Vector3d(v1->m_vector->cross(*(v2->m_vector)));
}

ModusAdjustCylinder_Math::Vector3d^ ModusAdjustCylinder_Math::Vector3d::operator+(Vector3d^ v1, Vector3d^ v2)
{
    return gcnew Vector3d(*v1->m_vector + *v2->m_vector);
}

ModusAdjustCylinder_Math::Vector3d^ ModusAdjustCylinder_Math::Vector3d::operator-(Vector3d^ v1, Vector3d^ v2)
{
    return gcnew Vector3d(*v1->m_vector - *v2->m_vector);
}

ModusAdjustCylinder_Math::Vector3d^ ModusAdjustCylinder_Math::Vector3d::operator*(Vector3d^ v1, double v)
{
    return gcnew Vector3d(*v1->m_vector * v);
}

ModusAdjustCylinder_Math::Vector3d^ ModusAdjustCylinder_Math::Vector3d::operator*(double v, Vector3d^ v1)
{
    return v1 * v;
}

ModusAdjustCylinder_Math::Vector3d^ ModusAdjustCylinder_Math::Vector3d::operator/(Vector3d^ v1, double v)
{
    return v1 * (1.0 / v);
}



void ModusAdjustCylinder_Math::Vector3d::Normalize()
{
    m_vector->normalize();
}

ModusAdjustCylinder_Math::Vector3d^ ModusAdjustCylinder_Math::Vector3d::Normalized()
{
    return gcnew Vector3d(m_vector->normalized());
}


ModusAdjustCylinder_Math::Vector3d::~Vector3d()
{
    delete m_vector;
}
