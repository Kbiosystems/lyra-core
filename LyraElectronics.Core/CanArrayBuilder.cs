using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics
{
    public class CanArrayBuilder
    {
        public CanController Controller { get; set; }

        public CanArrayBuilder AddSaciaBoard(int address)
        {
            return this;
        }
        
        public CanArrayBuilder AddDynamicSaciaBoard(int initialAddress, int[] availableAddresses)
        {
            return this;
        }

        public CanArrayBuilder AddDynamicSaciaBoard(int initialAddress, int[] availableAddresses, Action<int, int> onSequenceNumberChanged)
        {
            return this;
        }

        public CanArray Build()
        {
            return new CanArray();
        }
    }
}
