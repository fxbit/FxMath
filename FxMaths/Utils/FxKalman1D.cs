using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FxMaths.Matrix;

namespace FxMaths.Utils
{
    public class FxKalman1D
    {

        #region Protected data.
        /// <summary>
        /// State.
        /// </summary>
        FxMatrixF m_x;
        /// <summary>
        /// Covariance.
        /// </summary>
        FxMatrixF m_p;
        /// <summary>
        /// Minimal covariance.
        /// </summary>
        FxMatrixF m_q;

        /// <summary>
        /// Minimal innovative covariance, keeps filter from locking in to a solution.
        /// </summary>
        float m_r;
        #endregion


        #region Constructors
        public FxKalman1D()
        {
            m_x = new FxMatrixF(1, 2);
            m_p = new FxMatrixF(2, 2);
            m_q = new FxMatrixF(2, 2);
            m_r = 0.1f;
        }

        public FxKalman1D(float qx, float qv, float r, float pd, float ix)
        {
            m_x = new FxMatrixF(1, 2);
            m_p = new FxMatrixF(2, 2);
            m_q = new FxMatrixF(2, 2);
            m_r = r;

            this.Reset(qx, qv, r, pd, ix);
        } 
        #endregion


        #region Reset

        /// <summary>
        /// Reset the filter.
        /// </summary>
        /// <param name="qx">Measurement to position state minimal variance.</param>
        /// <param name="qv">Measurement to velocity state minimal variance.</param>
        /// <param name="r">Measurement covariance (sets minimal gain).</param>
        /// <param name="pd">Initial variance.</param>
        /// <param name="ix">Initial position.</param>
        public void Reset(float qx, float qv, float r, float pd, float ix)
        {
            m_q[0] = qx;
            m_q[1] = qv;

            m_r = r;
            m_p[0] = m_p[3] = pd;
            m_p[1] = m_p[2] = 0;
            m_x[0] = ix;
            m_x[1] = 0;
        } 

        #endregion


        #region Update

        /// <summary>
        /// Update the state by measurement m at dt time from last measurement.
        /// </summary>
        /// <param name="mx">The next position.</param>
        /// <param name="mv">The next velocity.</param>
        /// <param name="dt">The time interval.</param>
        /// <returns></returns>
        public float Update(float m, float dt)
        {
            // Predict to now, then update.
            // Predict:
            //   X = F*X + H*U
            //   P = F*X*F^T + Q.
            // Update:
            //   Y = M – H*X          Called the innovation = measurement – state transformed by H.	
            //   S = H*P*H^T + R      S= Residual covariance = covariane transformed by H + R
            //   K = P * H^T *S^-1    K = Kalman gain = variance / residual covariance.
            //   X = X + K*Y          Update with gain the new measurement
            //   P = (I – K * H) * P  Update covariance to this time.

            // X = F*X + H*U
            float oldX = m_x[0];
            m_x[0] = m_x[0] + (dt * m_x[1]);

            // P = F*X*F^T + Q
            m_p[0] = m_p[0] + dt * (m_p[2] + m_p[1]) + dt * dt * m_p[3] + m_q[0];
            m_p[1] = m_p[1] + dt * m_p[3] + m_q[1];
            m_p[2] = m_p[2] + dt * m_p[3] + m_q[2];
            m_p[3] = m_p[3] + m_q[3];

            // Y = M – H*X  
            float y0 = m - m_x[0];
            float y1 = ((m - oldX) / dt) - m_x[1];

            // S = H*P*H^T + R 
            // Because H = [1, 0] this is easy, and s is a single value not a matrix to invert.
            float s = m_p[0] + m_r;

            // K = P * H^T *S^-1 
            float k = m_p[0] / s;

            // X = X + K*Y
            m_x[0] += y0 * k;
	        m_x[1] += y1 * k;

            // P = (I – K * H) * P
            for (int i = 0; i < 4; i++) m_p[i] = m_p[i] - k * m_p[i];

            // Return latest estimate.
            return m_x[0];
        } 

        #endregion
    }
}
