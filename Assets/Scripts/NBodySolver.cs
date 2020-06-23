using System;
using System.Collections.Generic;
using UnityEngine;

public class NBodySolver : MonoBehaviour
{
    public List<CelestialBody> CelestialBodies;
    public float SimulationDelta = 0.001f;
    static public float GravitationalConstant = 1; // Change to 6.67e-11 for real world value.

    // This calculates gravity using Newton's law of universal gravitation. https://en.wikipedia.org/wiki/Newton%27s_law_of_universal_gravitation
    // Vector form.
    // F_21 = -G * ((m_1 * m_2) / |r_12|^2) * ^r_12
    // Where:
    // F_21 is the force applied on object 2 exerted by object 1. Unit is newtons(N).
    // G is the gravitational constant. 
    // m_1 and m_2 are respectively the masses of objects 1 and 2. Unit is kilograms(kg).
    // |r_12| is |r_2 - r_1| is the distance between objects 1 and 2. Unit is meters(m).
    // ^r_12 is (r_2 - r_1) / |r_2 - r_1| the unit vector from object 1 to 2.
    // r is the distance between the centers of masses. Unit is meters(m).
    static Vector3 GForce(CelestialBody b0, CelestialBody b1)
    {
        var r_vec = b0.transform.position - b1.transform.position;
        var r_mag = Vector3.Magnitude(r_vec);
        var r_hat = r_vec / r_mag;
        var force_mag = GravitationalConstant * b0.mass * b1.mass / (float)Math.Pow(r_mag, 2);
        var force_vec = -force_mag * r_hat;
        return force_vec;
    }

    void FixedUpdate()
    {
        foreach (var b0 in CelestialBodies)
        {
            if (b0 == null)
                continue;

            var force = new Vector3();
            foreach (var b1 in CelestialBodies)
            {
                if (b1 == null || b0 == b1) // Don't calculate force for it self.
                    continue;
                
                force += GForce(b0, b1);
            }

            b0.force = force;
            b0.momentum += b0.force * SimulationDelta;
            b0.transform.position += b0.momentum / b0.mass * SimulationDelta;
        }
    }
}
