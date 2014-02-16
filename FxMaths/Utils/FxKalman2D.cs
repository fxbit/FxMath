using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FxMaths.Matrix;

namespace FxMaths.Utils
{
    public class FxKalman2D
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
        public FxKalman2D()
        {
            m_x = new FxMatrixF(1, 2);
            m_p = new FxMatrixF(2, 2);
            m_q = new FxMatrixF(2, 2);
            m_r = 0.1f;
        }

        public FxKalman2D(float qx, float qv, float r, float pd, float ix)
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
            m_q[0] = qx * qx;
            m_q[1] = m_q[2] = qv * qx;
            m_q[3] = qv * qv;

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
        public float Update(float mx, float mv, float dt)
        {
            // Predict to now, then update.
            // Predict:
            //   X = F*X + H*U
            //   P = F*P*F^T + Q.
            // Update:
            //   Y = M – H*X          Called the innovation = measurement – state transformed by H.	
            //   S = H*P*H^T + R      S= Residual covariance = covariane transformed by H + R
            //   K = P * H^T *S^-1    K = Kalman gain = variance / residual covariance.
            //   X = X + K*Y          Update with gain the new measurement
            //   P = (I – K * H) * P  Update covariance to this time.
            //
            // Same as 1D but mv is used instead of delta m_x[0], and H = [1,1].

            // X = F*X + H*U
            FxMatrixF f = new FxMatrixF(2, 2) { Data = new float[] { 1, dt, 0, 1 } };
            
            // U = {0,0};
            m_x = f.Multiply(m_x) as FxMatrixF;

            // P = F*P*F^T + Q
            m_p = f.MultiplyABAT(m_p) as FxMatrixF;
            m_p.Add(m_q);

            // Y = M – H*X  
            FxMatrixF y = new FxMatrixF(1, 2) { Data = new float[] { mx - m_x[0], mv - m_x[1] } };

            // S = H*P*H^T + R 
            FxMatrixF s = m_p.Copy();
            s[0] += m_r;
            s[3] += m_r * 0.1f;

            // K = P * H^T *S^-1 
            FxMatrixF sinv = s.Inverse() as FxMatrixF;
            FxMatrixF k = new FxMatrixF(2, 2, 0f); // inited to zero.

            if (sinv as Object != null)
            {
                k = m_p.Multiply(sinv) as FxMatrixF;
            }

            // X = X + K*Y
            m_x.Add(k.Multiply(y));


            // P = (I – K * H) * P
            FxMatrixF id = new FxMatrixF(2, 2) { Data = new float[] { 1, 0, 0, 1 } };
            id.Subtract(k);
            id.Multiply(m_p);
            m_p = id;


            // return latest estimate
            return m_x[0];
        } 

        #endregion
    }
}
