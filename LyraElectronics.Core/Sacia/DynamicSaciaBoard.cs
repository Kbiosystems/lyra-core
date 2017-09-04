using LyraElectronics.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyraElectronics.Sacia
{
    /// <summary>
    ///    A dynamically addressed version of 
    ///    the <see cref="SaciaBoard"/> class.
    /// </summary>
    /// <seealso cref="LyraElectronics.Sacia.SaciaBoard" />
    public class DynamicSaciaBoard : SaciaBoard
    {
        private int[] PotentialSequenceNumbers;

        /// <summary>
        ///     Occurs when the sequence number changed.
        /// </summary>
        public event EventHandler<SequenceNumberChangedEventArgs> SequenceNumberChanged;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DynamicSaciaBoard"/> class.
        /// </summary>
        /// <param name="defaultSequenceNumber">
        ///     The initial CAN board sequence number as an integer 
        ///     representation of its binary dip switch position to
        ///     listen for.
        /// </param>
        /// <param name="listenForSequenceNumbers">
        ///     The sequence numbers to listen for.
        /// </param>
        /// <param name="controller">
        ///     The <see cref="CanController"/> associated with this board. 
        /// </param>
        public DynamicSaciaBoard(int defaultSequenceNumber, int[] listenForSequenceNumbers, CanController controller)
            : base(defaultSequenceNumber, controller)
        {
            PotentialSequenceNumbers = listenForSequenceNumbers;

            controller.CanMessageRecieved += (sender, args) =>
            {
                if (args.Message.Address == ((Range | (SequenceNumber << 4)) | 8))
                {
                    Parse(args.Message.Data);
                }
                else
                {
                    if (PotentialSequenceNumbers.Any(i => args.Message.Address == ((Range | (i << 4)) | 8)))
                    {
                        int oldNumber = SequenceNumber;

                        SequenceNumber = PotentialSequenceNumbers.First(i => args.Message.Address == ((Range | (i << 4)) | 8));

                        SequenceNumberChanged?.Invoke(this, new SequenceNumberChangedEventArgs(oldNumber, SequenceNumber));

                        Parse(args.Message.Data);
                    }
                }
            };
        }
    }
}
