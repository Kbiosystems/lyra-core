using LyraElectronics.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LyraElectronics.Sacia
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Collections.IEnumerable" />
    public class SaciaInputs : IEnumerable
    {
        private List<bool> inputs {  get 
            {
                return new List<bool>()
                {
                    Input1,
                    Input2,
                    Input3
                };
            } }

        /// <summary>
        ///     The current value of the first input.
        /// </summary>
        public bool Input1 { get; private set; }

        /// <summary>
        ///     The current value of the second input.
        /// </summary>
        public bool Input2 { get; private set; }

        /// <summary>
        ///     The current value of the third input.
        /// </summary>
        public bool Input3 { get; private set; }

        /// <summary>
        /// Gets the <see cref="System.Boolean"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="System.Boolean"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public bool this[int index]
        {
            get
            {
                if (index < 0 || index > 2)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "An input at index {0} does not exist.", index.ToString()));
                }
                return inputs[index];
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<bool> GetEnumerator()
        {
            return inputs.GetEnumerator();
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

        internal IEnumerable<InputChangedEventArgs> Parse(byte @byte)
        {
            if (Input1 != (@byte & 0x01) > 0)
            {
                Input1 = (@byte & 0x01) > 0;
                yield return new InputChangedEventArgs(0, !Input1, Input1);
            }

            if (Input2 != (@byte & 0x02) > 0)
            {
                Input2 = (@byte & 0x02) > 0;
                yield return new InputChangedEventArgs(1, !Input2, Input2);
            }

            if (Input3 != (@byte & 0x04) > 0)
            {
                Input3 = (@byte & 0x04) > 0;
                yield return new InputChangedEventArgs(2, !Input3, Input3);
            }
        }
    }
}
