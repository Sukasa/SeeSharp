using System;
using System.Collections;
using System.Collections.Generic;
using MoreLinq;
using System.Linq;

namespace SeeSharp.Rendering
{
    public class Heatmap : IEnumerable<int>
    {
        private Granularity _Granularity; // Divisor for map size -> heatmap size
        private int[] _Map;
        private int _Width;
        private int _Height;

        public struct MapQuantification<T>
        {
            public int Value;
            public T Quantification;

            public MapQuantification(int TargetValue, T AssociatedValue)
            {
                Value = TargetValue;
                Quantification = AssociatedValue;
            }
        }

        public enum Granularity : int
        {
            Full = 0,
            Half = 1,
            Quarter = 2,
            Eighth = 3,
            Sixteenth = 4
        }

        public Heatmap(RenderConfiguration Config, Granularity Granularity)
        {
            _Granularity = Granularity;
            _Width = (((Config.SubregionChunks.Width + 1) << 4) >> (int)_Granularity);
            _Height = (((Config.SubregionChunks.Height + 1) << 4) >> (int)_Granularity);
            _Map = new int[_Height * _Width];
        }

        public Heatmap(WorldMetrics Metrics, Granularity Granularity)
        {
            _Granularity = Granularity;
            _Width = (((Metrics.MaxX - Metrics.MinX + 1) << 4) >> (int)_Granularity);
            _Height = (((Metrics.MaxZ - Metrics.MinZ + 1) << 4) >> (int)_Granularity);
            _Map = new int[_Height * _Width];
        }

        public int this[int PixelX, int PixelZ]
        {
            get
            {
                return _Map[PixelX + (PixelZ * _Width)];
            }
            set
            {
                _Map[PixelX + (PixelZ * _Width)] = value;
            }
        }

        public void Normalize()
        {
            for (int x = 0; x < _Map.Length; x++)
            {
                _Map[x] >>= (int)_Granularity;
            }
        }

        /// <summary>
        ///     Quantifies the heatmap values to the nearest value in the supplied array
        /// </summary>
        /// <param name="Quantifications"></param>
        public void Quantify(IEnumerable<int> Quantifications) {
            for (int X = 0; X < _Map.Length; X++)
            {
                _Map[X] = Quantifications.MinBy(Q => Math.Abs(Q - _Map[X]));
            }
        }

        /// <summary>
        ///     Generates a copy of the heatmaps, quantized to the given enum values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Quantifications"></param>
        /// <returns></returns>
        public T[] MapQuantify<T>(IEnumerable<MapQuantification<T>> Quantifications)
        {
            T[] QuantifiedMap = new T[_Map.Length];
            for (int X = 0; X < _Map.Length; X++)
            {
                QuantifiedMap[X] = Quantifications.MinBy(Q => Math.Abs(Q.Value - _Map[X])).Quantification;
            }
            return QuantifiedMap;
        }

        /// <summary>
        ///     Smooths out the heatmap, blurring each pixel slightly with the pixels next to it.  Not recommended after a quantize.
        /// </summary>
        public void Smooth(int SmoothingDistance)
        {
            int[] SmoothedMap = new int[_Map.Length];

            for (int x = 0; x < _Map.Length; x++)
            {
                SmoothedMap[x] = (int)_Map.GetRange(x, SmoothingDistance, _Width).Average();
            }

            _Map = SmoothedMap;
        }

        public void Smooth()
        {
            Smooth(1);
        }

        public void Offset(int Offset)
        {
            for (int x = 0; x < _Width; x++)
                for (int y = 0; y < _Height; y++)
                    this.Offset(x, y, Offset);
        }

        public int Offset(int BlockX, int BlockZ, int Offset)
        {
            return _Map[((BlockX >> (int)_Granularity) + ((BlockZ >> (int)_Granularity) * _Width))] += Offset;
        }

        public void Factor(float Factor)
        {
            for (int x = 0; x < _Width; x++)
                for (int y = 0; y < _Height; y++)
                    this.Factor(x, y, Factor);
        }

        public int Factor(int BlockX, int BlockZ, float Factor)
        {
            return _Map[((BlockX >> (int)_Granularity) + ((BlockZ >> (int)_Granularity) * _Width))] = (int)((float)_Map[((BlockX >> (int)_Granularity) + ((BlockZ >> (int)_Granularity) * _Width))] * Factor);
        }

        public IEnumerator<int> GetEnumerator()
        {
            return ((IEnumerable<int>)_Map).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _Map.GetEnumerator();
        }

        public int Width {
            get { return _Width; }
        }

        public int Height
        {
            get { return _Height; }
        }
    }
}
