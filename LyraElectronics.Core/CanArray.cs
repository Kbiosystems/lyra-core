using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace LyraElectronics
{
    public class CanArray : IEnumerable
    {
        private List<CanBoard> _boards { get; set; }

        public CanController Controller { get; private set; }

        public CanArray()
        {
            _boards = new List<CanBoard>();
        }

        public T GetBoardAt<T>(int address) where T : CanBoard
        {
            return (T)_boards.Where(b => typeof(T) == b.GetType()).FirstOrDefault(i => i.SequenceNumber == address);
        }

        /// <summary>
        /// Gets the <see cref="System.Boolean"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="System.Boolean"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public CanBoard this[int index]
        {
            get
            {
                if (index < _boards.Count - 1 || index > _boards.Count - 1)
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "A board at index {0} does not exist.", index.ToString()));
                }
                return _boards[index];
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<CanBoard> GetEnumerator()
        {
            return _boards.GetEnumerator();
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
    }
}
