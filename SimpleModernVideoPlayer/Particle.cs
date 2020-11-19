using System;
using System.Collections.Generic;
using Windows.Foundation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using System.Numerics;

namespace SimpleModernVideoPlayer
{
    public class Particle
    {
        /// <summary>
        /// 初始半径
        /// </summary>
        public double DefaultRadius;
        /// <summary>
        /// 形状
        /// </summary>
        public Ellipse Shape;
        /// <summary>
        /// 坐标
        /// </summary>
        public Point Position;
        /// <summary>
        /// 速度
        /// </summary>
        public Vector2 Velocity;
        /// <summary>
        /// 粒子和线段的集合
        /// </summary>
        public Dictionary<Particle, Line> ParticleLines;
    }
}
