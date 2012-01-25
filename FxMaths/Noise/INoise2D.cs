using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxMaths.Noise
{
    public interface INoise2D
    {
        float GetValue( float x, float y );
    }
}
