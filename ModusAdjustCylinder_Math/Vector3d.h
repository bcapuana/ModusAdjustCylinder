#pragma once
#include <dense>


namespace ModusAdjustCylinder_Math {
	public ref class Vector3d
	{
	protected:
		Eigen::Vector3d* m_vector = nullptr;

	public:
		property double x
		{
			double get() { return m_vector->x(); }
			void set(double value) { (*m_vector)[0] = value; }
		}
		property double y
		{
			double get() { return m_vector->y(); }
			void set(double value) { (*m_vector)[1] = value; }
		}
		property double z
		{
			double get() { return m_vector->z(); }
			void set(double value) { (*m_vector)[2] = value; }
		}

		property double default[int]
		{
			double get(int index) { return (*m_vector)[index]; }
		    void set(int index, double value) 
			{
			  (*m_vector)[index] = value;
			}
		}

	internal:
		property Eigen::Vector3d Vector {
			Eigen::Vector3d get() { return *m_vector; }
		}

	public:
		Vector3d();
		Vector3d(double x, double y, double z);
		Vector3d(array<double>^ xyz);
		Vector3d(double* xyz);
		Vector3d(Eigen::Vector3d vec);

		array<double>^ ToArray();

		static double operator * (Vector3d^ v1, Vector3d^ v2);
		static Vector3d^ operator ^ (Vector3d^ v1, Vector3d^ v2);
		static Vector3d^ operator + (Vector3d^ v1, Vector3d^ v2);
		static Vector3d^ operator - (Vector3d^ v1, Vector3d^ v2);
		static Vector3d^ operator * (Vector3d^ v1, double v);
		static Vector3d^ operator * (double v, Vector3d^ v1);
		static Vector3d^ operator / (Vector3d^ v1, double v);
		void Normalize();
		Vector3d^ Normalized();


		~Vector3d();
	};
}

