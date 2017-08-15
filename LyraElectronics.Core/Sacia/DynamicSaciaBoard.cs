using LyraElectronics.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyraElectronics.Sacia
{
    public class DynamicSaciaBoard : SaciaBoard
    {
        private int[] PotentialSequenceNumbers;

        public event EventHandler<SequenceNumberChangedEventArgs> SequenceNumberChanged;

        public DynamicSaciaBoard(int initialSequenceNumber, int[] listenForSequenceNumbers, CanController controller)
            : base(initialSequenceNumber, controller)
        {
            PotentialSequenceNumbers = listenForSequenceNumbers;

            controller.CanMessageRecieved += (sender, args) =>
            {
                if (PotentialSequenceNumbers.Any(i => args.Message.Address == ((Range | (i << 4)) | 8)))
                {
                    int oldNumber = SequenceNumber;

                    SequenceNumber = PotentialSequenceNumbers.First(i => args.Message.Address == ((Range | (i << 4)) | 8));

                    SequenceNumberChanged?.Invoke(this, new SequenceNumberChangedEventArgs(oldNumber, SequenceNumber));

                    Parse(args.Message.Data);
                }
            };
        }
    }
}
