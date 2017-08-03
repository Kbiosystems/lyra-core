using LyraElectronics.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics.Sacia
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Collections.IEnumerable" />
    public class SaciaOutputs : IEnumerable
    {
        private List<bool> ouputs
        {
            get
            {
                return new List<bool>()
                {
                    Output1,
                    Output2,
                    Output3
                };
            }
        }

        /// <summary>
        ///     The current value of the first output.
        /// </summary>
        public bool Output1 { get; internal set; }

        /// <summary>
        ///     The current value of the second output.
        /// </summary>
        public bool Output2 { get; internal set; }

        /// <summary>
        ///     The current value of the third output.
        /// </summary>
        public bool Output3 { get; internal set; }

        /// <summary>
        /// Gets the <see cref="System.Boolean"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="System.Boolean"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public bool this[int index] { get { return ouputs[index]; } }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<bool> GetEnumerator()
        {
            return ouputs.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal IEnumerable<OutputChangedEventArgs> Parse(byte @byte)
        {
            if (Output1 != (@byte & 0x01) > 0)
            {
                Output1 = (@byte & 0x01) > 0;
                yield return new OutputChangedEventArgs(0, !Output1, Output1);
            }

            if (Output2 != (@byte & 0x02) > 0)
            {
                Output2 = (@byte & 0x02) > 0;
                yield return new OutputChangedEventArgs(1, !Output2, Output2);
            }

            if (Output3 != (@byte & 0x04) > 0)
            {
                Output3 = (@byte & 0x04) > 0;
                yield return new OutputChangedEventArgs(2, !Output3, Output3);
            }
        }
    }
}
